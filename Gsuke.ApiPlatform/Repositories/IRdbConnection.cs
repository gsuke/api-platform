using Npgsql;

namespace Gsuke.ApiPlatform.Repositories
{
    public interface IRdbConnection
    {
        NpgsqlCommand Command(string sql);
    }
}
