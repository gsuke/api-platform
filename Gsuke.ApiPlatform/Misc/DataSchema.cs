using Newtonsoft.Json.Schema;
using Gsuke.ApiPlatform.Errors;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Gsuke.ApiPlatform.Misc
{
    public class DataSchema
    {
        // プロパティ名のルール
        // - 英小文字・半角数字・アンダースコアのみ
        // - 先頭に数字は不可
        // - 1～32文字
        // TODO: もう少し条件を緩和したい
        public static string ColumnRagex { get; } = @"^[a-z_][a-z0-9_]{0,31}$";

        /// <summary>
        /// データスキーマ文字列の妥当性を検証し、JSchemaに変換する
        /// </summary>
        /// <param name="dataSchema"></param>
        /// <returns>正しければJSchemaを返す、不正ならばnullを返す</returns>
        public static (JSchema?, Error) ParseDataSchema(string dataSchema)
        {
            // パースする
            JSchema jSchema;
            try
            {
                jSchema = JSchema.Parse(dataSchema);
            }
            catch (JsonException)
            {
                return (null, new JsonError());
            }

            // それぞれのプロパティの妥当性を確認する
            var hasId = false;
            foreach (KeyValuePair<string, JSchema> property in jSchema.Properties)
            {
                // Typeが対応しているか
                if (ColumnType.ConvertJSchemaTypeToSqlColumnType(property.Value.Type ?? JSchemaType.None) is null)
                {
                    return (null, new DataSchemaDefinitionError("プロパティ内のタイプは string, number, integer, boolean のみ対応しています。"));
                }

                if (property.Key == "id")
                {
                    // idプロパティが含まれているか
                    hasId = true;

                    // idプロパティの型がstringであるか
                    if (property.Value.Type != JSchemaType.String)
                    {
                        return (null, new DataSchemaDefinitionError("「id」プロパティのタイプはstringにする必要があります。"));
                    }
                }
                // プロパティ名がルールに従っているか
                if (!Regex.IsMatch(property.Key, ColumnRagex))
                {
                    return (null, new DataSchemaDefinitionError("プロパティ名が命名規則に従っていません。"));
                }
            }

            // idプロパティが含まれているか
            if (!hasId)
            {
                return (null, new DataSchemaDefinitionError("「id」プロパティを含める必要があります。"));
            }

            // idプロパティがRequiredになっているか
            if (jSchema.Required.FirstOrDefault(name => name == "id") is null)
            {
                return (null, new DataSchemaDefinitionError("「id」プロパティはRequiredに指定する必要があります。"));
            }

            // additionalPropertiesを強制的にfalseにする
            jSchema.AllowAdditionalProperties = false;

            return (jSchema, new NoError());
        }
    }
}
