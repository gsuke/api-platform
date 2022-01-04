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
            // TODO: "Integer, Null" のように格納されるNull許容型に対応していない
            // Console.WriteLine(type.Value);
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
