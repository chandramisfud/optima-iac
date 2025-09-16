using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;
using System.Data;
using System.Data.SqlClient;

namespace Repositories.Repos
{
    public class MasterDistributorRepository : IDistributorRepository
    {
        readonly IConfiguration __config;
        public MasterDistributorRepository(IConfiguration config)
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

        public async Task<DistributorCreateReturn> CreateDistributor(DistributorCreate body)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            using IDbConnection conn = Connection;
            try
            {
                var __query = @" 
                                DECLARE @message varchar(255); 
                                DECLARE @identity INT; 
                                DECLARE @return varchar(255); 
                                -- Cek Jika data sudah ada 
                                IF NOT EXISTS 
                                ( 
                                SELECT Id FROM tbmst_distributor
                                WHERE LongDesc = @LongDesc 
                                AND isnull(IsDeleted, 0) = 0 
                                ) 
                                BEGIN 
                                INSERT INTO [dbo].tbmst_distributor 
                                (
                                [LongDesc] 
                                ,[ShortDesc] 
                                ,CompanyName
                                ,Address
                                ,NPWP
                                ,Phone
                                ,Fax
                                ,NoRekening
                                ,BankName
                                ,BankCabang
                                ,ClaimManager
                                ,SAPCode
                                ,SAPCodex
                                ,[IsActive] 
                                ,[CreateOn] 
                                ,[CreateBy] 
                                ,[CreatedEmail] 
                                ) 
                                VALUES 
                                ( 
                                @LongDesc 
                                ,@ShortDesc 
                                ,@CompanyName
                                ,@Address
                                ,@NPWP
                                ,@Phone
                                ,@Fax
                                ,@NoRekening
                                ,@BankName
                                ,@BankCabang
                                ,@ClaimManager
                                ,@SAPCode
                                ,@SAPCodex
                                ,1
                                ,@CreateOn
                                ,@CreateBy 
                                ,@CreatedEmail 
                                ) 
                                SET @identity = (SELECT SCOPE_IDENTITY()) 
                                SELECT 
                                Id, 
                                LongDesc, 
                                ShortDesc, 
                                CreateOn, 
                                CreateBy, 
                                RefId, 
                                CreatedEmail, 
                                CompanyName, 
                                Address, 
                                NPWP, 
                                Phone, 
                                Fax, 
                                NoRekening,
                                BankName, 
                                BankCabang, 
                                ClaimManager, 
                                SAPCode, 
                                SAPCodex 
                                FROM tbmst_distributor WHERE Id = @identity
                                END
                                ELSE
                                BEGIN
                                -- message error jika data sudah ada
                                SET @return = (SELECT RefId FROM tbmst_distributor WHERE LongDesc = @LongDesc) 
                                SET @message = 'Distributor with RefId = ' + @return + ' is already exist'
                                RAISERROR(@message, --Message text.

                                16, --Severity.

                                1-- State.

                                );
                                END";

                var __res = await conn.QueryAsync<DistributorCreateReturn>(__query, new
                {
                    LongDesc = body.LongDesc,
                    ShortDesc = body.ShortDesc,
                    CompanyName = body.CompanyName,
                    Address = body.Address,
                    NPWP = body.NPWP,
                    Phone = body.Phone,
                    Fax = body.Fax,
                    NoRekening = body.NoRekening,
                    BankName = body.BankName,
                    BankCabang = body.BankCabang,
                    ClaimManager = body.ClaimManager,
                    SAPCode = body.SAPCode,
                    SAPCodex = body.SAPCodex,
                    CreateOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                    CreateBy = body.CreateBy,
                    CreatedEmail = body.CreatedEmail,
                });
                return __res.FirstOrDefault()!;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<DistributorDeleteReturn> DeleteDistributor(DistributorDelete body)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            using IDbConnection conn = Connection;
            try
            {
                var __query = @" 
                                UPDATE [dbo].tbmst_distributor 
                                SET 
                                IsActive=0 
                                ,IsDeleted=1 
                                ,DeletedBy=@DeletedBy 
                                ,DeletedOn=@DeletedOn
                                ,DeleteEmail=@DeleteEmail 
                                WHERE 
                                Id=@Id 
                                SELECT Id, DeletedBy, IsDeleted, DeletedOn, DeleteEmail, RefId FROM tbmst_distributor WHERE Id=@Id 
                                ";
                var __res = await conn.QueryAsync<DistributorDeleteReturn>(__query, new
                {
                    Id = body.Id,
                    DeletedBy = body.DeletedBy,
                    DeletedOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                    DeleteEmail = body.DeletedEmail
                });
                return __res.FirstOrDefault()!;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<DistributorModel> GetDistributorById(DistributorById body)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @" 
                                SELECT * FROM tbmst_distributor
                                WHERE Id = @Id AND ISNULL(IsDeleted, 0) = 0";
                var __res = await conn.QueryAsync<DistributorModel>(__query, body);
                return __res.FirstOrDefault()!;

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }


        public async Task<BaseLP> GetDistributorLandingPage(string keyword, string sortColumn, string sortDirection = "ASC",
            int dataDisplayed = 10, int pageNum = 0)
        {
            BaseLP res = null!;
            try
            {
                //int offset = (currentPage-1) * dataDisplayed;
                string userFilter = "";
                if (!String.IsNullOrEmpty(keyword))
                {
                    userFilter = " CONCAT_WS(' ', RefId, LongDesc, ShortDesc, companyName, address, claimManager ) LIKE '%" + keyword + "%' ";
                }
                else
                {
                    userFilter = " 1=1 ";
                }
                // pageNum= -1, show all data

                var __query = @"SELECT COUNT(*) as TotalCount    
                        , SUM(case when {0} then 1 else 0 end) as FilteredCount
                        FROM tbmst_distributor WHERE isnull(IsDeleted, 0)=0 
                        ";

                var paging = @"
                        OFFSET {0}  * {1} ROWS
                        FETCH NEXT {1} ROWS ONLY";

                __query += @"
                        SELECT * 
                        FROM tbmst_distributor
                        WHERE {0} AND isnull(IsDeleted, 0)=0
                        ORDER BY {1} {2}                     
                        ";

                // if set -1 dont add paging format
                if (dataDisplayed >= 0)
                {
                    __query += String.Format(paging, pageNum, dataDisplayed);
                }

                __query = string.Format(__query, userFilter, sortColumn, sortDirection);

                using IDbConnection conn = Connection;
                var __res = await conn.QueryMultipleAsync(__query);
                res = __res.ReadSingle<BaseLP>();
                res.Data = __res.Read<DistributorSelect>().Cast<object>().ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }
        public async Task<DistributorLandingPage> GetDistributorLandingPageOLD(DistributorListRequest body)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"
                        BEGIN 
                        SET NOCOUNT ON; 
                        DECLARE @query nvarchar(MAX); 
                        DECLARE @TotalCount AS INT = (SELECT COUNT(*) FROM tbmst_distributor WHERE ISNULL(IsDeleted, 0) = 0) 
                        DECLARE @FirstRec int, @LastRec int 
                        SET @FirstRec = @PageNumber * @PageSize + 1; 
                        SET @LastRec = (@PageNumber + 1) * @PageSize; 
                        SET @Search = LTRIM(RTRIM(@Search)) 
                        SELECT 
                        count(id) FilteredCount, 
                        @TotalCount AS TotalCount 
                        FROM tbmst_distributor 
                        WHERE (ISNULL(@Search, '') = '' 
                        OR LongDesc LIKE '%' + @Search + '%' 
                        OR ShortDesc LIKE '%' + @Search + '%'
                        OR CompanyName LIKE '%' + @Search + '%' 
                        OR Address LIKE '%' + @Search + '%' 
                        OR NPWP LIKE '%' + @Search + '%' 
                        OR Phone LIKE '%' + @Search + '%' 
                        OR Fax LIKE '%' + @Search + '%' 
                        OR NoRekening LIKE '%' + @Search + '%' 
                        OR BankName LIKE '%' + @Search + '%'
                        OR BankCabang LIKE '%' + @Search + '%'
                        OR ClaimManager LIKE '%' + @Search + '%'
                        OR RefId LIKE '%' + @Search + '%')
                        AND ISNULL(IsDeleted, 0) = 0
                        SET @query='SELECT 
                        Id, 
                        LongDesc, 
                        ShortDesc, 
                        RefId,
                        CompanyName,
                        Address,
                        NPWP,
                        Phone,
                        Fax,
                        NoRekening,
                        BankName,
                        BankCabang,
                        ClaimManager
                        FROM ( 
                        SELECT ROW_NUMBER() OVER (ORDER BY ' + 
                        @SortColumn + ' ' + @SortDirection + ' 
                        ) 
                        AS RowNum, 
                        COUNT(*) OVER() as FilteredCount, 
                        Id, 
                        LongDesc, 
                        ShortDesc, 
                        RefId,
                        CompanyName,
                        Address,
                        NPWP,
                        Phone,
                        Fax,
                        NoRekening,
                        BankName,
                        BankCabang,
                        ClaimManager
                        FROM tbmst_distributor 
                        WHERE (LongDesc LIKE ''%' + isnull(@Search, '') + '%'' 
                        OR ShortDesc LIKE ''%' + isnull(@Search, '') + '%''
                        OR CompanyName LIKE ''%' + isnull(@Search, '') + '%''
                        OR Address LIKE ''%' + isnull(@Search, '') + '%'' 
                        OR NPWP LIKE ''%' + isnull(@Search, '') + '%'' 
                        OR Phone LIKE ''%' + isnull(@Search, '') + '%'' 
                        OR Fax LIKE ''%' + isnull(@Search, '') + '%'' 
                        OR NoRekening LIKE ''%' + isnull(@Search, '') + '%'' 
                        OR BankName LIKE ''%' + isnull(@Search, '') + '%''
                        OR BankCabang LIKE ''%' + isnull(@Search, '') + '%'' 
                        OR ClaimManager LIKE ''%' + isnull(@Search, '') + '%'' 
                        OR RefId LIKE ''%' + isnull(@Search, '') + '%'')
                        AND ISNULL(IsDeleted, 0) = 0
                        ) a 
                        WHERE RowNum BETWEEN ' + cast(@FirstRec as nvarchar) + ' AND ' + cast(@LastRec as nvarchar); 
                        EXEC sp_executesql @query 
                        END";

                var __object = await conn.QueryMultipleAsync(__query,
                new
                {
                    Search = body.Search,
                    PageNumber = body.PageNumber,
                    PageSize = body.PageSize,
                    SortColumn = body.SortColumn.ToString(),
                    SortDirection = body.SortDirection.ToString()

                });
                var __res = __object.ReadSingle<DistributorLandingPage>();
                __res.Data = __object.Read<DistributorSelect>().ToList();

                return __res;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<DistributorUpdateReturn> UpdateDistributor(DistributorUpdate body)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            using IDbConnection conn = Connection;
            try
            {
                var __query = @" 
                                DECLARE @messageout varchar(255) 
                                -- Cek Jika data sudah ada 
                                IF NOT EXISTS( 
                                SELECT id FROM tbmst_distributor
                                WHERE LongDesc = @LongDesc 
                                AND isnull(IsDeleted, 0) = 0 
                                and id<>@id 
                                ) 
                                BEGIN 
                                UPDATE [dbo].tbmst_distributor 
                                SET 
                                [LongDesc] = @LongDesc 
                                ,[ShortDesc] = @ShortDesc 
                                ,CompanyName=@CompanyName
                                ,Address=@Address
                                ,NPWP=@NPWP
                                ,Phone=@Phone
                                ,Fax=@Fax
                                ,NoRekening=@NoRekening
                                ,BankName=@BankName
                                ,BankCabang=@BankCabang
                                ,ClaimManager=@ClaimManager
                                ,SAPCode=@SAPCode
                                ,SAPCodex=@SAPCodex
                                ,[ModifiedOn] = @ModifiedOn 
                                ,[ModifiedBy] = @ModifiedBy 
                                ,[ModifiedEmail] = @ModifiedEmail
                                WHERE 
                                Id=@Id 
                                SELECT 
                                Id, 
                                LongDesc, 
                                ShortDesc, 
                                ModifiedOn, 
                                ModifiedBy, 
                                ModifiedEmail, 
                                RefId,
                                CompanyName,
                                Address,
                                NPWP,
                                Phone,
                                Fax,
                                NoRekening,
                                BankName,
                                BankCabang,
                                ClaimManager,
                                SAPCode,
                                SAPCodex 
                                FROM tbmst_distributor WHERE Id = @Id 
                                END
                                ELSE
                                BEGIN
                                -- message error jika data sudah ada
                                SET @messageout = 'Distributor already exist'
                                RAISERROR(@messageout, --Message text.

                                16, --Severity.

                                1-- State.

                                );

                                END";
                var __res = await conn.QueryAsync<DistributorUpdateReturn>(__query, new
                {
                    Id = body.Id,
                    LongDesc = body.LongDesc,
                    ShortDesc = body.ShortDesc,
                    CompanyName = body.CompanyName,
                    Address = body.Address,
                    NPWP = body.NPWP,
                    Phone = body.Phone,
                    Fax = body.Fax,
                    NoRekening = body.NoRekening,
                    BankName = body.BankName,
                    BankCabang = body.BankCabang,
                    ClaimManager = body.ClaimManager,
                    SAPCode = body.SAPCode,
                    SAPCodex = body.SAPCodex,
                    ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                    ModifiedBy = body.ModifiedBy,
                    ModifiedEmail = body.ModifiedEmail,
                });
                return __res.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

    }
}
