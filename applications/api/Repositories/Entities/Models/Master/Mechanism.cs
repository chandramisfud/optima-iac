using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Repositories.Entities.Models
{
    public class MechanismModel
    {
        public int Id { get; set; }
        public int EntityId { get; set; }
        public string? Entity { get; set; }
        public int SubCategoryId { get; set; }
        public string? SubCategory { get; set; }
        public int ActivityId { get; set; }
        public string? Activity { get; set; }
        public int SubActivityId { get; set; }
        public string? SubActivity { get; set; }
        public int ProductId { get; set; }
        public string? Product { get; set; }
        public string? Requirement { get; set; }
        public string? Discount { get; set; }
        public string? Mechanism { get; set; }
        public int ChannelId { get; set; }
        public string? Channel { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
        public string? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }
        public int IsDelete { get; set; }
        public string? DeleteOn { get; set; }
        public string? DeleteBy { get; set; }
        public string? DeleteEmail { get; set; }

    }
    public class GetAttributeParentNewBody
    {
        public int budgetid { get; set; }
        public string? attribute { get; set; }
        public int[]? arrayParent { get; set; }
        public string? status { get; set; }
    }

    public class InsertMechanismeBody
    {
        public int EntityId { get; set; }
        public string? Entity { get; set; }
        public int SubCategoryId { get; set; }
        public string? SubCategory { get; set; }
        public int ActivityId { get; set; }
        public string? Activity { get; set; }
        public int SubActivityId { get; set; }
        public string? SubActivity { get; set; }
        public int ProductId { get; set; }
        public string? Product { get; set; }
        public string? Requirement { get; set; }
        public string? Discount { get; set; }
        public string? Mechanism { get; set; }
        public int ChannelId { get; set; }
        public string? Channel { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
    }

    public class ReturnInsertMechanisme
    {
        public int Id { get; set; }
        public int EntityId { get; set; }
        public string? Entity { get; set; }
        public int SubCategoryId { get; set; }
        public string? SubCategory { get; set; }
        public int ActivityId { get; set; }
        public string? Activity { get; set; }
        public int SubActivityId { get; set; }
        public string? SubActivity { get; set; }
        public int ProductId { get; set; }
        public string? Product { get; set; }
        public string? Requirement { get; set; }
        public string? Discount { get; set; }
        public string? Mechanism { get; set; }
        public int ChannelId { get; set; }
        public string? Channel { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
    }
    public class UpdateMechanismeBody
    {
        public int Id { get; set; }
        public int EntityId { get; set; }
        public string? Entity { get; set; }
        public int SubCategoryId { get; set; }
        public string? SubCategory { get; set; }
        public int ActivityId { get; set; }
        public string? Activity { get; set; }
        public int SubActivityId { get; set; }
        public string? SubActivity { get; set; }
        public int ProductId { get; set; }
        public string? Product { get; set; }
        public string? Requirement { get; set; }
        public string? Discount { get; set; }
        public string? Mechanism { get; set; }
        public int ChannelId { get; set; }
        public string? Channel { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }
    }
    public class ReturnUpdateMechanisme
    {
        public int Id { get; set; }
        public int EntityId { get; set; }
        public string? Entity { get; set; }
        public int SubCategoryId { get; set; }
        public string? SubCategory { get; set; }
        public int ActivityId { get; set; }
        public string? Activity { get; set; }
        public int SubActivityId { get; set; }
        public string? SubActivity { get; set; }
        public int ProductId { get; set; }
        public string? Product { get; set; }
        public string? Requirement { get; set; }
        public string? Discount { get; set; }
        public string? Mechanism { get; set; }
        public int ChannelId { get; set; }
        public string? Channel { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
        public string? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }
    }
    public class DeleteMechanismeBody
    {
        public int Id { get; set; }
        public string? DeleteBy { get; set; }
        public string? DeleteEmail { get; set; }
    }

    public class GetMechanismByParam
    {
        public int entityid { get; set; }
        public int subcategoryid { get; set; }
        public int activityid { get; set; }
        public int subactivityid { get; set; }
        public int productid { get; set; }
        public int channelid { get; set; }
        public string? startdate { get; set; }
        public string? enddate { get; set; }

    }
    public class ResGetAttributeBrandForPromo
    {
        public int flag { get; set; }
        public int Id { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }

    }

    public class GetAttributeBrandForPromoBody
    {
        public int budgetid { get; set; }
        public string? attribute { get; set; }
        public int entity { get; set; }
        public int promoplanid { get; set; }
        public int[]? parent { get; set; }
    }

    public class GetMechanismById
    {
        public int id { get; set; }
    }
    public class GetAttributeByParentBodyReq
    {
        public string? attribute { get; set; }
        public string? longdesc { get; set; }
    }
    public class GetAttributeByParentRes
    {
        public int id { get; set; }
        public string? LongDesc { get; set; }
    }

    public class MechanismeListByParamRes
    {
        public int Id { get; set; }
        public int EntityId { get; set; }
        public string? Entity { get; set; }
        public int SubActivityId { get; set; }
        public string? SubActivity { get; set; }
        public int ProductId { get; set; }
        public string? Product { get; set; }
        public int BrandId { get; set; }
        public string? Brand { get; set; }
        public string? Requirement { get; set; }
        public string? Discount { get; set; }
        public string? Mechanism { get; set; }
        public string? Channel { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
        public string? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }
        public int IsDelete { get; set; }
        public string? DeleteOn { get; set; }
        public string? DeleteBy { get; set; }
        public string? DeleteEmail { get; set; }
    }

    public class ResponseImport
    {
        public List<ResponseImportDto>? data { get; set; }
        public ImportRecordTotal? totRec { get; set; }
    }
    public class ResponseImportDto
    {
        public string? doc { get; set; }
        public string? status { get; set; }
    }
    public class ImportRecordTotal
    {
        public int failedRec { get; set; }
        public int successRec { get; set; }
        public int totRec { get; set; }
    }

    public class EntityForMechanism
    {
        public int id { get; set; }
        public string? LongDesc { get; set; }
    }

    public class SubCategoryforMechanism
    {
        public int id { get; set; }
        public string? LongDesc { get; set; }
    }

    public class ActivityforMechanism
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
    }
    public class SubActivityforMechanism
    {
        public int id { get; set; }
        public string? LongDesc { get; set; }
    }
    public class ChannelforMechanism
    {
        public int id { get; set; }
        public string? LongDesc { get; set; }
    }
    public class ProductforMechanism
    {
        public int id { get; set; }
        public string? LongDesc { get; set; }
    }

    public class MechanismListRequest
    {
        public string? Search { get; set; }
        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;
        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public MechanismSortColumn SortColumn { get; set; }
        public MechanismSortDirection SortDirection { get; set; }
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MechanismSortColumn
    {
        Id,
        EntityId,
        Entity,
        SubCategoryId,
        SubCategory,
        ActivityId,
        Activity,
        SubActivityId,
        SubActivity,
        ProductId,
        Product,
        Requirement,
        Discount,
        Mechanism,
        ChannelId,
        Channel
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MechanismSortDirection
    {
        asc,
        desc
    }

    public class ReturnDeleteMechanism
    {
        public int Id { get; set; }
        public int IsDelete { get; set; }
        public string? DeleteOn { get; set; }
        public string? DeleteBy { get; set; }
        public string? DeleteEmail { get; set; }

    }
}