
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using MyossUpgradeScheduler;
using MyossUpgradeScheduler.Models;

namespace MyossUpgradeScheduler.Services
{
    public class ReminderService
    {
        
        OSSEntities db = new OSSEntities();

        MailService objMailService = new MailService();


        #region Answer Solution Delivered 
        public string Reminder1EmailService()
        {
          //  ASDClass resultresponse = new ASDClass(); /*Single record*/
            List<ASDClass> resultresponse = new List<ASDClass>();
            try
            {

                resultresponse = db.Database.SqlQuery<ASDClass>("Ticket_Reminder_ASD @LoginUIID",
                                  new SqlParameter("@LoginUIID", "OSS Administrator")).ToList();
                if (resultresponse!=null)
                {
                    if(resultresponse.Count>0)
                    {
                        for (int i = 0; i < resultresponse.Count-1; i++)
                        {

                            objMailService.SendMail("TicketASDReminder", "System Administrator"
                                , resultresponse[i].TID, resultresponse[i].Reminder1 ?? ""
                                , resultresponse[i].Reminder2 ?? ""
                                , resultresponse[i].ClosedUpdate ??"", "");


                        }
                    }
                }
                return "ASD Mail Sent";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion


        #region Additional Info Required/Waiting Approval from Management 
        public string Reminder2EmailService()
        {
            //  ASDClass resultresponse = new ASDClass(); /*Single record*/
            List<ASDClass> resultresponse = new List<ASDClass>();
            try
            {

                resultresponse = db.Database.SqlQuery<ASDClass>("Ticket_Reminder_AIRWAM @LoginUIID",
                                  new SqlParameter("@LoginUIID", "OSS Administrator")).ToList();
                if (resultresponse != null)
                {
                    if (resultresponse.Count > 0)
                    {
                        for (int i = 0; i < resultresponse.Count - 1; i++)
                        {

                            objMailService.SendMail("TicketAIRWAMReminder", "System Administrator"
                                , resultresponse[i].TID
                                ,resultresponse[i].Reminder1 ?? ""
                                , resultresponse[i].Reminder2 ?? ""
                                , resultresponse[i].ClosedUpdate ?? ""
                                 , resultresponse[i].EMailFlag ?? "");


                        }
                    }
                }
                return "ASD Mail Sent";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion


        public string GetWebConfigValues(string Web_Key)
        {
            string response = string.Empty;
            try
            {
                response = db.Database.SqlQuery<string>("GetWebConfigValues @Web_Key",
                 new SqlParameter("@Web_Key", Web_Key)).FirstOrDefault();
                return response;
            }
            catch (Exception ex)
            {
                return response;
            }

        }

    }



}