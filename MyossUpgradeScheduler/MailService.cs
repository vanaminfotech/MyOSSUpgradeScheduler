
using MyossUpgradeScheduler.Models;
using MyossUpgradeScheduler.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Xml.Linq;
using MyossUpgradeScheduler;
using MyossUpgradeScheduler.Models;

namespace MyossUpgradeScheduler.Services
{
    public class MailService
    {
        OSSEntities db = new OSSEntities();

        public List<MailLogs> GetMailLogs(MailLogs input)

        {
            List<MailLogs> response = new List<MailLogs>();
            try
            {

                response = db.Database.SqlQuery<MailLogs>("EmailHistory_v1 @ModuleName,@ModuleId,@LoginUIID",
                    new SqlParameter("@ModuleName", input.ModuleName ?? ""),
                    new SqlParameter("@ModuleId", input.ModuleId),
                    new SqlParameter("@LoginUIID", input.LoginUIID)
                   ).ToList();
                return response;
            }
            catch (Exception ex)
            {
                response[0].ResponseCode = 401;
                response[0].ResponseMessage = ex.Message;
                return response;


            }
        }

        public void SendMail(string ModuleName, string LoginUIID, int Moduleid, string Additional_Info1 = ""
            , string Additional_Info2 = "", string Additional_Info3 = "", string Additional_Info4 = "")
        {

            MailResponse response = new MailResponse();
            List<MailResponse> responseList = new List<MailResponse>();

            if (ModuleName == "TicketASDReminder" || ModuleName == "TicketAIRWAMReminder")
            {
                response = db.Database.SqlQuery<MailResponse>("Ticket_Mail_Procedure_v1 @ModuleName,@Module_id" +
                    ",@Additional_Info1,@Additional_Info2,@Additional_Info3,@Additional_Info4,@LoginUIID",
               new SqlParameter("@ModuleName", ModuleName ?? ""),
               new SqlParameter("@Module_id", Moduleid),
                new SqlParameter("@Additional_Info1", Additional_Info1 ?? ""),
                new SqlParameter("@Additional_Info2", Additional_Info2 ?? ""),
                new SqlParameter("@Additional_Info3", Additional_Info3 ?? ""),
                new SqlParameter("@Additional_Info4", Additional_Info4 ?? ""),
               new SqlParameter("@LoginUIID", LoginUIID)).FirstOrDefault();
            }
           
            else if (ModuleName == "UCRAssessmentReminder")
            {
                response = db.Database.SqlQuery<MailResponse>("UCR_Mail_Procedure_v1 @ModuleName,@Module_id" +
                    ",@Additional_Info1,@Additional_Info2,@Additional_Info3,@Additional_Info4,@LoginUIID",
               new SqlParameter("@ModuleName", ModuleName ?? ""),
               new SqlParameter("@Module_id", Moduleid),
                new SqlParameter("@Additional_Info1", Additional_Info1 ?? ""),
                new SqlParameter("@Additional_Info2", Additional_Info2 ?? ""),
                new SqlParameter("@Additional_Info3", Additional_Info3 ?? ""),
                new SqlParameter("@Additional_Info4", Additional_Info4 ?? ""),
               new SqlParameter("@LoginUIID", LoginUIID)).FirstOrDefault();
            }
            else
            {
                response.ResponseCode = 401;
                response.ResponseMessage = "Modulename not defined in mail procedure";
                return;
            }


            if (response != null)
            {

                if (response.ResponseCode == 200)
                {
                    if (response.EmailSubject != null && response.EmailSubject != ""
                            && response.EmailBody != null && response.EmailBody != ""
                            && response.ToEmail != null && response.ToEmail != "")


                        ComposeSendMail(response.EmailSubject, response.EmailBody, response.ToEmail
                            , response.CCMail, LoginUIID, Moduleid, ModuleName);
                }
            }
            if (responseList != null)
            {
                if (responseList.Count > 0)
                {
                    foreach (var item in responseList)
                    {
                        if (item.ResponseCode == 200)
                        {
                            if (item.EmailSubject != null && item.EmailSubject != ""
                                && item.EmailBody != null && item.EmailBody != ""
                                && item.ToEmail != null && item.ToEmail != "")
                                ComposeSendMail(item.EmailSubject, item.EmailBody, item.ToEmail, item.CCMail, LoginUIID, Moduleid, ModuleName);
                        }
                    }
                }
            }

        }



        public int ComposeSendMail(string emailsubject, string emailbody, string toemail, string ccEmail, string LoginUIID, int moduleid, string type)
        {

            try
            {

                List<string> to = new List<string>();
                to.Add(toemail);

                List<string> cc = new List<string>();
                cc.Add(ccEmail);

                if (emailsubject != null && emailsubject != "" && toemail != null && toemail != "")
                {
                    SendMail(to, emailsubject, emailbody, cc);
                }
                return 0;
            }
            catch (Exception ex)
            {

                return 1;
            }
        }


        public static void SendMail(List<string> to, string title, string body, List<string> cc)
        {
            //try
            //{
            ReminderService ObjService1 = new ReminderService();
            string[] tomail = new string[100];
            string[] ccmail = new string[100];
            List<string> ddd = new List<string>();
            string SendMail = "true";

            if (SendMail == "true" && to != null && to.Count > 0)
            {
                for (int i = 0; i < to.Count; i++)
                {
                    if (to[i] != null)
                    {
                        tomail = to[i].Split(',');
                    }
                }
                if (tomail.Length > 0)
                {
                    var tomailid = tomail.Distinct().ToList();
                    for (int j = 0; j < tomailid.Count; j++)
                    {
                        to.Add(tomail[j]);
                    }

                    to.RemoveAt(0);
                }

                for (int i = 0; i < cc.Count; i++)
                {
                    if (cc[i] != null)
                        ccmail = cc[i].Split(',');
                }
                if (ccmail.Length > 0)
                {
                    var ccmailid = ccmail.Distinct().ToList();
                    for (int j = 0; j < ccmailid.Count; j++)
                    {
                        cc.Add(ccmail[j]);
                    }

                    cc.RemoveAt(0);
                }
                to = to.Distinct().ToList();
                cc = cc.Distinct().ToList();


                string SMTP = ObjService1.GetWebConfigValues("SMTPServer");
                int port = Convert.ToInt32(ObjService1.GetWebConfigValues("SMTPport"));
                string userId = ObjService1.GetWebConfigValues("SMTPEMailUserId");
                string password = ObjService1.GetWebConfigValues("SMTPEMailPassword");
                string fromAddress = ObjService1.GetWebConfigValues("MyossFromMailId");
                string FromEMailDisplayName = ObjService1.GetWebConfigValues("MyossEmailDisplayName");

                var smtpClient = new SmtpClient(SMTP, port)
                {
                    DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = true,
                    Credentials = new NetworkCredential(userId, password),
                    EnableSsl = false
                };
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(fromAddress, FromEMailDisplayName);
                mail.Subject = title;
                mail.Body = body;
                mail.IsBodyHtml = true;
                mail.BodyEncoding = Encoding.UTF8;
                mail.SubjectEncoding = Encoding.UTF8;

                if (to != null)
                {
                    foreach (string recipient in to)
                    {
                        if (!string.IsNullOrEmpty(recipient))
                            mail.To.Add(new MailAddress(recipient));
                    }
                }
                //CC to recipient
                if (cc != null)
                {
                    //mail.CC.Add(new MailAddress(cc));
                    foreach (string recipient in cc)
                    {
                        if (!string.IsNullOrEmpty(recipient))
                            mail.CC.Add(new MailAddress(recipient));
                    }
                }


                ServicePointManager.ServerCertificateValidationCallback =
               delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
               {
                   return true;
               };

                SmtpClient smtp = new SmtpClient();


                try
                {
                    smtpClient.Send(mail);
                }
                catch (Exception ex)
                {

                }

            }

        }

    }
}