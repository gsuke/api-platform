using Newtonsoft.Json.Schema;

namespace Gsuke.ApiPlatform.Misc
{
    public class ColumnType
    {
        private static Dictionary<int, string> _sqlColumnType = new Dictionary<int, string>{
            {(int)JSchemaType.String, "varchar(64)"},
            {(int)JSchemaType.Number, "double"},
            {(int)JSchemaType.Integer, "integer"},
            {(int)JSchemaType.Boolean, "boolean"},
        };

        public static string? ConvertJSchemaTypeToSqlColumnType(JSchemaType? type)
        {
            if (type is null)
            {
                return null;
            }
            string? result;
            _sqlColumnType.TryGetValue((int)type, out result);
            return result;
        }
    }
}
