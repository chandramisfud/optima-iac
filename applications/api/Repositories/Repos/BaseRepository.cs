using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using System.Data;
using System.Data.SqlClient;

namespace Repositories.Repos
{
    public class BaseRepository
    {
        readonly IConfiguration __config;
        public BaseRepository(IConfiguration config, object host)
        {
            __config = config;
        }
        public  IDbConnection Connection
        {
            get
            {
                return new SqlConnection(__config.GetConnectionString("DefaultConnection"));
            }
        }
    }
}
