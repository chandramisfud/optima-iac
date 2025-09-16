using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;

namespace Repositories.Repos
{
    public class DNUploadAttachmentRepo : IDNUploadAttachmentRepo
    {
        readonly IConfiguration __config;
        public DNUploadAttachmentRepo(IConfiguration config)
        {
            __config = config;
        }
        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(__config.GetConnectionString("DefaultConnection"));
            }
        }
        //   debetnote/listattach
        public async Task<IList<DNListAttachment>> GetDNListAttachment(string periode, int distributor, string userId, bool isdnmanual)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@periode", periode);
                __param.Add("@distributor", distributor);
                __param.Add("@userid", userId);
                __param.Add("@isdnmanual", isdnmanual);

                var __res = await conn.QueryAsync<DNListAttachment>("ip_debetnote_list_upload_attach", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return __res.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        //   dnattachment/store
        public async Task<DNCreateAttachmentReturn> SaveDNAttachment(DNCreateAttachmentParam param)
        {
            try
            {
                using IDbConnection conn = Connection;

                var __param = new DynamicParameters();
                __param.Add("@id", param.DNId);
                __param.Add("@doclink", param.DocLink);
                __param.Add("@filename", param.FileName);
                __param.Add("@createby", param.CreateBy);

                var result = await conn.QueryAsync<DNCreateAttachmentReturn>("ip_dn_attachment_store", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        //   searchDNbyRefid/post
        public async Task<IList<SearchParamDNbyRefidDto>> SearchDNByRefid(string refId)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@refid", refId);
                var result = await conn.QueryAsync<SearchParamDNbyRefidDto>("ip_search_dnbyrefid", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.AsList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}