using System.Data;
using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface IMechanismRepository
    {
        // Task<IList<MechanismModel>> GetMechanismeLists();
        Task<BaseLP> GetMechanismLandingPage(
           string keyword
           , string sortColumn
           , string sortDirection
           , int dataDisplayed
           , int pageNum
           );
        Task DeleteMechanisme(DeleteMechanismeBody body);
        Task<ReturnInsertMechanisme> CreateMechanisme(InsertMechanismeBody body);
        Task<ReturnUpdateMechanisme> UpdateMechanisme(UpdateMechanismeBody body);
        Task<IList<MechanismeListByParamRes>> GetMechanismeListByParam(GetMechanismByParam body);
        Task<IList<MechanismModel>> GetMechanismeListById(GetMechanismById body);
        Task<IList<ResponseImportDto>> ImportMechanism(DataTable mechanism, string userid, string useremail);
        Task<IList<GetAttributeByParentRes>> GetMechanismAttributeByParent(GetAttributeByParentBodyReq body);
        Task<IList<EntityForMechanism>> GetEntityForMechanisms();
        Task<IList<ChannelforMechanism>> GetChannelForMechanisms();
        Task<BaseLP> GetSubAccountLandingPage
       (
           string keyword
           , string sortColumn
           , string sortDirection
           , int dataDisplayed
           , int pageNum
       );
        Task<BaseLP> GetProductLandingPage(
            string keyword
            , string sortColumn
            , string sortDirection
            , int dataDisplayed
            , int pageNum
            );
         Task<BaseLP> GetSubActivityLandingPage(string keyword
            , string sortColumn
            , string sortDirection
            , int dataDisplayed
            , int pageNum);
        Task<ImportRecordTotal> ImportMechanismWithStatInfo(DataTable mechanism, string userid, string useremail);
        Task<object> GetMechanismTemplate();
    }
}