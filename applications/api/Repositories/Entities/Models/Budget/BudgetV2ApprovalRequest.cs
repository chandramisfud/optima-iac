namespace Repositories.Entities
{


    // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
    public class Body
    {
        public Body(BudgetTableAttribute attrib)
        {
            // Assigning values to the properties
            fontWeigth = attrib.fontWeigth;
            borderTop = attrib.borderTop;
            textAlign = attrib.textAlign;
            verticalAlign = attrib.verticalAlign;
            rowSpan = attrib.rowSpan;
            colSpan = attrib.colSpan;
            backgroundColor = attrib.backgroundColor;
            textColor = attrib.textColor;
        }
        public object text { get; set; }
        public string textAlign { get; set; } 
        public string textColor { get; set; } 
        public string fontWeigth { get; set; } 
        public string verticalAlign { get; set; }
        public string borderTop { get; set; }
        public string backgroundColor { get; set; }
        public int rowSpan { get; set; } = 1;
        public int colSpan { get; set; } = 1;
        public List<tblValue> value { get; set; }
    }

    public class Header
    {
        public Header(BudgetTableAttribute attrib)
        {
            // Assigning values to the properties
            fontWeigth = attrib.fontWeigth;
            borderTop = attrib.borderTop;
            textAlign = attrib.textAlign;
            verticalAlign = attrib.verticalAlign;
            rowSpan = attrib.rowSpan;
            colSpan = attrib.colSpan;
            backgroundColor = attrib.backgroundColor;
            textColor = attrib.textColor;
        }
        public string headerName { get; set; }
        public string textAlign { get; set; } 
        public string textColor { get; set; }
        public string fontWeigth { get; set; }
        public string verticalAlign { get; set; } 
        public string borderTop { get; set; }
        public string backgroundColor { get; set; } 
        public int rowSpan { get; set; } = 1;
        public int colSpan { get; set; } = 1;
    }

    public class BudgetV2ApprovalRequestRespon
    {
      //  public string subCategory { get; set; }
      //  public Signature signature { get; set; }
        public List<Header> header { get; set; }
        public List<SubHeader> subHeader { get; set; }
        public List<Body> body { get; set; }
        public List<Body> footer { get; set; }
    }

    public class Signature
    {
        public string userName1 { get; set; }
        public string userProfileName1 { get; set; }
        public string userName2 { get; set; }
        public string userProfileName2 { get; set; }
        public string userName3 { get; set; }
        public string userProfileName3 { get; set; }
    }

    public class SubHeader 
    {
        // Constructor with parameters
        public SubHeader(BudgetTableAttribute attrib)
        {
            // Assigning values to the properties
            fontWeigth = attrib.fontWeigth;
            borderTop = attrib.borderTop;
            borderBottom = attrib.borderTop;
            textAlign = attrib.textAlign;
            verticalAlign = attrib.verticalAlign;
            //subHeader.valueSubHeader =  subHeader.valueSubHeader;
            rowSpan = attrib.rowSpan;
            colSpan = attrib.colSpan;
            backgroundColor = attrib.backgroundColor;
            textColor = attrib.textColor;
        }
        public string headerName { get; set; }
        public string textAlign { get; set; }
        public string textColor { get; set; } 
        public string fontWeigth { get; set; }
        public string verticalAlign { get; set; }
        public string borderTop { get; set; }
        public string backgroundColor { get; set; }
        public int rowSpan { get; set; } 
        public int colSpan { get; set; } 
        public List<tblHeader> valueSubHeader { get; set; }
        public string borderBottom { get; set; }
    }

    public class tblValue
    {
        public tblValue(BudgetTableAttribute attrib)
        {
            // Assigning values to the properties
            fontWeigth = attrib.fontWeigth;
            borderBottom = attrib.borderTop;
            textAlign = attrib.textAlign;
            verticalAlign = attrib.verticalAlign;
            //subHeader.valueSubHeader =  subHeader.valueSubHeader;
            rowSpan = attrib.rowSpan;
            colSpan = attrib.colSpan;
            backgroundColor = attrib.backgroundColor;
            textColor = attrib.textColor;
        }
        public decimal value { get; set; }
        public string textAlign { get; set; }
        public string textColor { get; set; } 
        public string fontWeigth { get; set; } 
        public string verticalAlign { get; set; } 
        public string borderBottom { get; set; }
        public string backgroundColor { get; set; } 
        public int rowSpan { get; set; } = 1;
        public int colSpan { get; set; } = 1;
    }

    public class tblHeader
    {
        public tblHeader(BudgetTableAttribute attrib)
        {
            // Assigning values to the properties
            fontWeigth = attrib.fontWeigth;
            borderTop = attrib.borderTop;
            textAlign = attrib.textAlign;
            verticalAlign = attrib.verticalAlign;
            //subHeader.valueSubHeader =  subHeader.valueSubHeader;
            rowSpan = attrib.rowSpan;
            colSpan = attrib.colSpan;
            backgroundColor = attrib.backgroundColor;
            textColor = attrib.textColor;
        }
        public string headerName { get; set; }
        public string textAlign { get; set; } 
        public string textColor { get; set; } 
        public string fontWeigth { get; set; } 
        public string verticalAlign { get; set; } 
        public string borderTop { get; set; }
        public string backgroundColor { get; set; } 
        public int rowSpan { get; set; } = 1;
        public int colSpan { get; set; } = 1;
    }
    public class BudgetTableAttribute
    {
        public string fkey { get; set; }
        public string headerName { get; set; } 
        public string textAlign { get; set; } 
        public string textColor { get; set; } 
        public string fontWeigth { get; set; } 
        public string verticalAlign { get; set; } 
        public string borderTop { get; set; } 
        public string backgroundColor { get; set; } 
        public int rowSpan { get; set; } = 1;
        public int colSpan { get; set; } = 1;
    }
    public class BudgetV2ApprovalRequestData
    {
        public string channel { get; set; } 
        public string entitySGM { get; set; }
        public int promoIdTotCountSGM { get; set; }
        public decimal totInvestmentSGM { get; set; }
        public string entityNIS { get; set; }
        public int promoIdTotCountNIS { get; set; }
        public decimal totInvestmentNIS { get; set; }
        public string entityNMN { get; set; }
        public int promoIdTotCountNMN { get; set; }
        public decimal totInvestmentNMN { get; set; }
        public int promoIdTotCount { get; set; }
        public decimal totInvestment { get; set; }

    }
    public class BudgetReportData {
        public string batchId { get; set; }
        public string period { get; set; }
        public string periodDesc { get; set; }
        public List<object> budgetOver5Bio { get; set; }
        public BudgetV2ApprovalRequestRespon budgetBellow5BioAll { get; set; }
        public BudgetV2ApprovalRequestRespon budgetBellow5BioContractual { get; set; }
        public BudgetV2ApprovalRequestRespon budgetBellow5BioNonContractual { get; set; }
        public List<dynamic> budgetPromoList { get; set; }
        public List<object> emailApproval { get; set; }
        public List<object> emailApprovalSigned { get; set; }
        public List<object> nextApproval { get; set; }
        //public List<BudgetTableAttribute> budgetAttribute { get;set;}
                     
    }

    public class periodRespon
    {
        public string period { get; set; }
        public string periodDesc { get; set; }
    }

    public class BudgetApprovalDataByBatch
    {
        public List<object> budgetOver5Bio { get; set; }
        public List<object> budgetBellow5BioAll { get; set; }
        public List<object> budgetBellow5BioContractual { get; set; }
        public List<object> budgetBellow5BioNonContractual { get; set; }
        public List<object> emailApproval { get; set; }
        public object? period { get; set; }

    }
}
