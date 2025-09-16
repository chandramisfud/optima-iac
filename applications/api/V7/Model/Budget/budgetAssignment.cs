namespace V7.Model.Budget
{
    public class BudgetAssignmentListParam : LPParam
    {
        public string? year { get; set; }
        public int entity { get; set; }
        public int distributor { get; set; }
        public int budgetparent { get; set; }
    }

    public class BudgetAssignmentAllocationSourceParam : LPParam
    {
        public string? year { get; set; }
        public int entity { get; set; }
        public int distributor { get; set; }
    }


    public class BudgetAssignmentCreateParam
    {
        public int budgetId { get; set; }
        public string? frownId { get; set; }
        public int allocationId { get; set; }
        public double budgetAmount { get; set; }
        public BudgetAssignmentCreateDetail[]? assignmentDetail { get; set; }
    }

    public class BudgetAssignmentCreateDetail
    {
        public int assignmentId { get; set; }
        public string? refId { get; set; }
        public string? ownId { get; set; }
        public string? desc { get; set; }
        public double budgetAmount { get; set; }
     //   public string? periode { get; set; }
        public int budgetSourceId { get; set; }
    }

    public class BudgetAssignmentUpdateParam
    {
        public string? assignmentId { get; set; }
        public BudgetAssignmentUpdateDetail[]? assignmentDetail { get; set; }
    }

    public class BudgetAssignmentUpdateDetail
    {
        public int id { get; set; }
        public string? ownId { get; set; }
        public string? desc { get; set; }
        public double budgetAmount { get; set; }
    }

}
