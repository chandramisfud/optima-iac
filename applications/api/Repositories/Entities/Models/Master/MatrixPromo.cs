namespace Repositories.Entities.Models
{
    public class MatrixPromoModel
    {
        public int id { get; set; }
        public string? Periode { get; set; }
        public string? Initiator { get; set; }
        public int EntityId { get; set; }
        public string? Entity { get; set; }
        public int DistributorId { get; set; }
        public string? Distributor { get; set; }
        public int SubActivityTypeId { get; set; }
        public string? SubActivityType { get; set; }
        public int ChannelId { get; set; }
        public string? Channel { get; set; }
        public int SubChannelId { get; set; }
        public string? SubChannel { get; set; }
        public double MinInvestment { get; set; }
        public double MaxInvestment { get; set; }
        public string? MatrixApprover { get; set; }
        public string? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
        public string? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryShortDesc { get; set; }
        public string? CategoryLongDesc { get; set; }
    }
    public class MatrixPromoApprovalBodyReq
    {
        public string? periode { get; set; }
        public int entity { get; set; }
        public int distributor { get; set; }
        public string? userid { get; set; }
    }

    public class MatrixPromoApprovalInsert
    {
        //EP1 2024 #143 
        //public string? periode { get; set; }
        public int entityid { get; set; }
        public int distributorid { get; set; }
        public int subactivitytypeid { get; set; }
        public int channelid { get; set; }
        public int subchannelid { get; set; }
        public string? initiator { get; set; }
        public double mininvestment { get; set; }
        public double maxinvestment { get; set; }
        public string? userid { get; set; }
        public string? useremail { get; set; }
        public int categoryId { get; set; }
        public IList<MatrixApproverDetail>? matrixApprover { get; set; }
    }
    public class MatrixPromoApproverDetail
    {
        public int id { get; set; }
        public int SeqApproval { get; set; }
        public string? Approver { get; set; }
    }
    public class MatrixApproverDetail
    {
        public int SeqApproval { get; set; }
        public string? Approver { get; set; }
    }
    public class MatrixPromoApprovalUpdate
    {
        public int id { get; set; }
        public string? periode { get; set; }
        public int entityid { get; set; }
        public int distributorid { get; set; }
        public int subactivitytypeid { get; set; }
        public int channelid { get; set; }
        public int subchannelid { get; set; }
        public string? initiator { get; set; }
        public double mininvestment { get; set; }
        public double maxinvestment { get; set; }
        public string? userid { get; set; }
        public string? useremail { get; set; }
        public int categoryId { get; set; }
        public IList<MatrixApproverDetail>? matrixApprover { get; set; }
    }
    public class GetMatrixPromoAprovalbyIdBody
    {
        public int id { get; set; }
    }

    public class GetMatrixPromoAprovalbyIdResult
    {
        public MatrixPromoModel? header { get; set; }
        public IList<MatrixPromoApproverDetail>? detailMatrix { get; set; }

    }

    public class EntityforMatrixPromo
    {
        public int id { get; set; }
        public string? LongDesc { get; set; }
    }
    public class DistributorforMatrixPromo
    {
        public int DistributorId { get; set; }
        public string? LongDesc { get; set; }
    }
    public class SubActivityTypeforMatrixPromo
    {
        public int id { get; set; }
        public string? LongDesc { get; set; }
    }
    public class ChannelforMatrixPromo
    {
        public int id { get; set; }
        public string? LongDesc { get; set; }
    }
    public class SubChannelforMatrixPromo
    {
        public int id { get; set; }
        public string? LongDesc { get; set; }
    }
     public class CategoryforMatrixPromo
    {
        public int id { get; set; }
        public string? LongDesc { get; set; }
    }
    public class InitiatorforMatrixPromo
    {
        public string? id { get; set; }
    }
}