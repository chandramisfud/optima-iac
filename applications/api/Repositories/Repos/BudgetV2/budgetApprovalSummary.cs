using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repos
{
    public static class budgetApprovalSummary
    {
        public static long _FiveBio = 5000000000;
        public class GetSummaryData
        {
            public string batchId { get; set; } = string.Empty;
            public string step { get; set; } = string.Empty;
            public string processSummary { get; set; } = string.Empty;
            public int below5BioQty { get; set; } 
            public decimal below5Bio { get; set; }
            public int above5BioQty { get; set; }
            public decimal above5Bio { get; set; }

        }
        public class GetSummaryFailedData
        {
            public string promoId { get; set; } = string.Empty;
            public string promoRefId { get; set; } = string.Empty;
            public decimal cost { get; set; }

        }

        public class PostSummaryData
        {
            public string batchId { get; set; } = string.Empty;
            public string processName { get; set; } = string.Empty;
            public int qty { get; set; } = 0;
            public decimal cost { get; set; } = 0;
            public int is5Bio { get; set; } = 0; 
            public int nourut { get; set; } = 0; 
        }

        public static string GenerateTableSummary(List<GetSummaryData> dataList, List<GetSummaryFailedData> failedList)
        {
            StringBuilder htmlBuilder = new StringBuilder();
            try
            {
                // Tambahkan bagian awal HTML
                htmlBuilder.Append(@"
            <table>
                <thead>
                <tr>
                    <th rowspan=""2"">Summary</th>
                    <th colspan=""2"" class=""table_header"">Up to 5 bio</th>
                    <th colspan=""2"" class=""table_header"">Above 5 bio</th>
                    <th colspan=""2"" class=""table_header"">Total</th>
                </tr>
                <tr>
                    <th class=""table_header"">In Qty</th>
                    <th class=""table_header"">In Value</th>
                    <th class=""table_header"">In Qty</th>
                    <th class=""table_header"">In Value</th>
                    <th class=""table_header"">In Qty</th>
                    <th class=""table_header"">In Value</th>
                </tr>
                </thead>
                <tbody>"
                );

                // Looping setiap baris data
                foreach (var item in dataList)
                {
                    htmlBuilder.AppendFormat(@"
                    <tr>
                        <td class=""desc"">{0}</td>
                        <td class=""qty"">{1}</td>
                        <td class=""value"">{2:N0}</td>
                        <td class=""qty"">{3}</td>
                        <td class=""value"">{4:N0}</td>
                        <td class=""qty"">{5}</td>
                        <td class=""value"">{6:N0}</td>
                    </tr>",
                        item.processSummary,
                        item.below5BioQty,
                        item.below5Bio,
                        item.above5BioQty,
                        item.above5Bio,
                        item.below5BioQty + item.above5BioQty,
                        item.below5Bio + item.above5Bio);
                }
                // Looping failed data below5bio
                foreach (var item in failedList.Where(x=>x.cost <= _FiveBio))
                {
                    htmlBuilder.AppendFormat(@"
                    <tr>
                        <td class=""promo_failed"">{0}</td>
                        <td class=""qty""></td>
                        <td class=""value"">{1:N0}</td>
                        <td class=""qty""></td>
                        <td class=""value""></td>
                        <td class=""qty""></td>
                        <td class=""value""></td>
                    </tr>",
                        item.promoRefId, item.cost
                    );
                }
                // Looping failed data above5bio
                foreach (var item in failedList.Where(x => x.cost > _FiveBio))
                {
                    htmlBuilder.AppendFormat(@"
                    <tr>
                        <td class=""promo_failed"">{0}</td>
                        <td class=""qty""></td>
                        <td class=""value""></td>
                        <td class=""qty""></td>
                        <td class=""value"">{1:N0}</td>
                        <td class=""qty""></td>
                        <td class=""value""></td>
                    </tr>",
                        item.promoRefId, item.cost
                    );
                }
                htmlBuilder.Append(@"  
            </tbody>
            </table>");
            }
            catch (Exception ex)
            {
                BGLogger.WriteLog("ERR Generate table summary " + ex.ToString());
            }
            return htmlBuilder.ToString();

        }
        public static string GenerateTableFailedSummary(List<GetSummaryFailedData> dataList)
        {
            StringBuilder htmlBuilder = new StringBuilder();

            try
            {
                if (dataList != null && dataList.Count > 0)
                {
                    // Tambahkan bagian awal HTML
                    htmlBuilder.Append(@"
                <h2>List Of Promo IDs That Failed to Send Email to 1st Approver</h2>
				<table>			
					<thead>
					<tr>
						<th class=""table_header"">Promo ID</th>
						<th class=""table_header"">Cost</th>
					</tr>
				    </thead>
                    <tbody>
                ");

                    // Looping setiap baris data
                    foreach (var item in dataList)
                    {
                        htmlBuilder.AppendFormat(@"
                    <tr>
                        <td class=""desc"">{0}</td>
                        <td class=""value"">{1:N0}</td>
                    </tr>",
                            item.promoRefId,
                            item.cost);
                    }
                    htmlBuilder.Append(@"  
                    </tbody>
                </table>"
                    );
                }
            }
            catch ( Exception ex )
            {
                BGLogger.WriteLog("ERR Generate table failed summary " + ex.ToString());
            }
            return htmlBuilder.ToString();

        }
    }
}
