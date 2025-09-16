using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;

namespace Repositories.Repos
{
    public class ToolsFileRepository : IToolsFileRepository
    {
        readonly IConfiguration __config;
        readonly string tblFiles = "mstfiles";
        public ToolsFileRepository(IConfiguration config)
        {
            __config = config;
        }
        //   public IDbConnection Connection => throw new NotImplementedException();
        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(__config.GetConnectionString("DefaultConnection"));
            }
        }
        public async Task<Entities.Models.Files> GetFilesByCode(string filecode)
        {
            Entities.Models.Files? res = null;
            using (IDbConnection conn = Connection)
            {
                conn.Open();
                var sql =
                    @"SELECT * FROM [dbo].[{0}]
                    WHERE fleuniqcode='{1}'"
                    ;
                var __res = await conn.QueryAsync<Entities.Models.Files>(String.Format(sql, this.tblFiles, filecode));
                if (__res != null)
                {
                    res = __res.First();
                }
            }
            return res!;
        }

        public string GetUniqCode()
        {
            string res = String.Empty;
            using (IDbConnection conn = Connection)
            {
                conn.Open();
                var sql = @"DECLARE @id varchar(255);
                    set @id = (SELECT NEWID());
                    select @id;";
                res = conn.Query<string>(sql).First();
            }
            return res;
        }
        public async Task<bool> UpdateNameByCode(string filecode, string filename)
        {
            bool res = false;
            using (IDbConnection conn = Connection)
            {
                conn.Open();
                var sql =
                    @"UPDATE [dbo].[{0}]
                    SET flename='{2}'
                    WHERE fleuniqcode='{1}'"
                    ;
                var __res = await conn.ExecuteAsync(String.Format(sql, this.tblFiles, filecode, filename));
                if (__res > 0)
                {
                    res = true;
                }
            }
            return res;
        }

        /// <summary>
        /// Insert filename, return filecode 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public async Task<string> Insert(string filename, string uniqCode)
        {
            try
            {
                using IDbConnection conn = Connection;
                conn.Open();
                var sql = @"
                DECLARE @identity INT;
                INSERT INTO [dbo].[{0}]
                    (
                    [flename], [fleuniqcode]
                      )                
                VALUES
                    ('{1}', '{2}');
                SET @identity = (SELECT SCOPE_IDENTITY())
                SELECT * FROM [dbo].[{0}] WHERE fleid=@identity
                ";
                Console.WriteLine(String.Format(sql, this.tblFiles, filename, uniqCode));
                var __res = await conn.QueryAsync<Entities.Models.Files>(String.Format(sql, this.tblFiles, filename, uniqCode));
                return __res.First().fleuniqcode.ToString()!;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}