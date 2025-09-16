using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using MimeKit;

namespace Repositories.Entities.Dtos
{
    public class EmailBody
    {
        public string?[]? email { get; set; }
        public string? subject { get; set; }
        public string? body { get; set; }
        public string?[]? cc { get; set; }
        public string?[]? bcc { get; set; }
        public List<IFormFile>? attachment { get; set; }

    }

    public class EmailResult
    {
        public string? email_to { get; set; }
        public string? email_cc { get; set; }
    }

    public class EmailBodyReq
    {
        public string? id { get; set; }
        public string? param { get; set; }
    }

}
