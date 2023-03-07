using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyossUpgradeScheduler.Models
{

    public class MailParams
    {
    }


    public class MailLogs
        {
            public int? Mailid { get; set; }
            public string EmailSubject { get; set; }
            public string ToEmail { get; set; }
            public string CCEmail { get; set; }
            public string EmailDate { get; set; }
            public string EmailSentBy { get; set; }
            public string EmailBody { get; set; }
            public int? ModuleId { get; set; }
            public string ModuleName { get; set; }
            public string LoginUIID { get; set; }

            public int ResponseCode { get; set; }
            public string ResponseMessage { get; set; }


        }
        public class MailResponse
        {
            public string EmailSubject { get; set; }
            public string ToEmail { get; set; }
            public string CCMail { get; set; }
            public string EmailBody { get; set; }

            public int ResponseCode { get; set; }
            public string ResponseMessage { get; set; }
        }


    }