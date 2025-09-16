using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace V7.Model
{
    /// <summary>
    /// defult param in every landing page
    /// </summary>
    public class LPParam
    {
        /// <summary>
        /// search text in every string column
        /// </summary>
        public string? Search { get; set; } 
        /// <summary>
        /// Page number start form 0
        /// </summary>
        public int PageNumber { get; set; } = 0;
        /// <summary>
        /// default value is 10, to show all set -1
        /// </summary>
        public int PageSize { get; set; } = 10;

    }

    public class LPParamReq
    {
       
    
        /// <summary>
        /// Page number start form 0
        /// </summary>
        public int PageNumber { get; set; } = 0;
        /// <summary>
        /// default value is 10, to show all set -1
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// search text in every string column
        /// </summary>
        public string? txtSearch { get; set; }
        /// <summary>
        /// order by column name
        /// </summary>
        public string? order { get; set; }
        /// <summary>
        /// sort direction ASC/DESC
        /// </summary>
        public string? sort { get; set; } = "ASC";

    }

    public class LPPagingParam
    {
        /// <summary>
        /// search text in every string column
        /// </summary>
        public string? search { get; set; }
        /// <summary>
        /// Page number start form 0
        /// </summary>
        public int pageNumber { get; set; } = 0;
        /// <summary>
        /// default value is 10, to show all set -1
        /// </summary>
        public int pageSize { get; set; } = 10;
        public string sortColumn { get; set; } = string.Empty;
        /// <summary>
        /// Sort Direction
        /// </summary>
        public string sortDirection { get; set; } = string.Empty;

    }
}
