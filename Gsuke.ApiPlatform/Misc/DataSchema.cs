using Newtonsoft.Json.Schema;

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
        public static JSchema? ParseDataSchema(string dataSchema)
        {
            JSchema jSchema;
            try
            {
                jSchema = JSchema.Parse(dataSchema);
            }
            catch
            {
                return null;
            }

            var hasIdColumn = false;

            foreach (KeyValuePair<string, JSchema> property in jSchema.Properties)
            {
                // 指定されたTypeが対応していること
                if (ColumnType.ConvertJSchemaTypeToSqlColumnType(property.Value.Type ?? JSchemaType.None) is null)
                {
                    return null;
                }
                // idカラムが含まれていること
                if (property.Key == "id")
                {
                    hasIdColumn = true;
                }
            }

            if (!hasIdColumn)
            {
                return null;
            }

            return jSchema;
        }
    }
}
