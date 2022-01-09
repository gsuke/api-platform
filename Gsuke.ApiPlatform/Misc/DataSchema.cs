using Newtonsoft.Json.Schema;
using Gsuke.ApiPlatform.Errors;
using Newtonsoft.Json;

namespace Gsuke.ApiPlatform.Misc
{
    public class DataSchema
    {
        /// <summary>
        /// データスキーマ文字列の妥当性を検証し、JSchemaに変換する
        /// </summary>
        /// <param name="dataSchema"></param>
        /// <returns>正しければJSchemaを返す、不正ならばnullを返す</returns>
        // TODO: このメソッドからエラー詳細を返すべき
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

            // それぞれのカラムの妥当性を確認する
            var hasId = false;
            foreach (KeyValuePair<string, JSchema> property in jSchema.Properties)
            {
                // 指定されたTypeが対応していること
                if (ColumnType.ConvertJSchemaTypeToSqlColumnType(property.Value.Type ?? JSchemaType.None) is null)
                {
                    return (null, new DataSchemaDefinitionError("プロパティ内のタイプは string, number, integer, boolean のみ対応しています。"));
                }
                // idカラムが含まれていること
                if (property.Key == "id")
                {
                    hasId = true;
                }
            }
            if (!hasId)
            {
                return (null, new DataSchemaDefinitionError("値「id」を含める必要があります。"));
            }

            // additionalPropertiesを強制的にfalseにする
            jSchema.AllowAdditionalProperties = false;
            // TODO: 他にも前処理が必要なはず

            return (jSchema, new NoError());
        }
    }
}
