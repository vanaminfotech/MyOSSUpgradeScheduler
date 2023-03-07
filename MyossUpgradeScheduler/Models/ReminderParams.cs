using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyossUpgradeScheduler.Models
{
    public class ReminderParams
    {
    }
    
    public class ASDClass
    {
            public int TID { get; set; }
            public string TicketNo { get; set; }
            public int RequestorUserTableId { get; set; }
            public string    RequestorName        { get; set; }
            public string AnswerSolutionDeliveredDate { get; set; }
            public string Reminder1 { get; set; }
            public string Reminder2 { get; set; }
            public string ClosedUpdate { get; set; }

        public string EMailFlag { get; set; }

        public string RequestorEmail { get; set; }
    }
}