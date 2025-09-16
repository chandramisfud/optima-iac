using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Repositories.Entities.Dtos;
using Repositories.Contracts;

namespace Repositories.Repos
{
    public class ReferencesRepository : IReferencesRepository
    {
        readonly IConfiguration _config;
        public ReferencesRepository(IConfiguration config)
        {
            _config = config;
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public Task<User> GetDistributorByProfileId(string profileId)
        {
            using IDbConnection conn = Connection;
            var userDictionary = new Dictionary<string, User>();
            string sql = @"SELECT A.[id]
                            ,A.[username]
                            ,A.[email]
                            ,A.[password]
                            ,A.[usergroupid]
                            ,A.[userlevel]
                            ,A.[isLogin]
                            ,A.[lastLogin]
                            ,A.[department]
                            ,A.[jobtitle]
                            ,A.[contactinfo]
                            ,B.[DistributorId]
                            ,C.[LongDesc] as DistributorLongDesc
                            ,A.[registered]
                            ,A.[code]
                            ,A.[password_change]
                            ,A.[token]
                            ,A.[token_date]
                            ,A.[userinput]
                            ,A.[dateinput]
                            ,A.[useredit]
                            ,A.[dateedit]
                            ,A.[isdeleted]
                            ,A.[deletedby]
                            ,A.[deletedon]
                FROM tbset_user AS A LEFT JOIN 
                tbset_user_distributor AS B ON A.id = B.UserId 
                INNER JOIN tbmst_distributor AS C ON B.DistributorId=C.Id
                WHERE A.id =@id";
            var list = conn.Query<User, UserDistributor, User>(
        sql, (user, distributor) =>
        {
            User userEntry;

            if (!userDictionary.TryGetValue(user.id!, out userEntry!))
            {
                userEntry = user;
                userEntry.distributorlist = new List<UserDistributor>();
                userDictionary.Add(userEntry.id!, userEntry);
            }

            userEntry.distributorlist!.Add(distributor);
            return userEntry;
        },
            new { Id = profileId },
            splitOn: "DistributorId")
            .Distinct()
            .FirstOrDefault();

            return Task.FromResult(
                list
            )!;
        }
    }
}
