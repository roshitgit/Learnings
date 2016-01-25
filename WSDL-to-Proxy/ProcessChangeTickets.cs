using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Reflection;
using Aspose.Email;
using Aspose.Email.Mail;
//using ServiceNowChangeAPI;
//using ServiceNowProdChangeAPI;

namespace ConsoleAppClient
{
    /// <summary>
    /// 
    /// </summary>
    public class ProcessChangeTickets
    {
        public string executingExePath
        {
            get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); }
        }

        public string fileUploadPath
        {
            get { return @"\\nasmwd012v2cti\planview_vol2\PTSWDocsDev\"; }
        }
        public string ticketDirectory { get; set; }


        public void ServiceNowProdApiCall()
        {
            ServiceNowProdChangeAPI.query query = new ServiceNowProdChangeAPI.query();
            SetAsposeLicense();

            //on webproject
            //licEmail.SetLicense(HttpContext.Current.Server.MapPath("AsposeLicense\Aspose.Email.lic"))

            MailMessage mm = new MailMessage();

            try
            {
                mm.From = ConfigurationManager.AppSettings["FromEmail"].ToString();
                mm.To = ConfigurationManager.AppSettings["ToEmail"].ToString();

                ServiceNowProdChangeAPI.ServiceNow_Prod_ChangeQuery changeQuery = new
                ServiceNowProdChangeAPI.ServiceNow_Prod_ChangeQuery { Credentials = new NetworkCredential("ptsw", "pt5w") };

                //string[] changeTickets = { "CHG0000064953", "CHG0000064641", "CHG0000064645",
                //                     "CHG0000064781","CHG0000064782","CHG0000064783",
                //                     "CHG0000064784","CHG0000064787","CHG0000064788"};

                string[] changeTickets = { "CHG0000064953", "CHG0000064782" }; // one approved, once cancelled

                ProcessProdTickets(query, mm, changeQuery, changeTickets);
            }
            catch (Exception ex)
            {
                //throw ex.Message;
            }
        }

        private void ProcessProdTickets(ServiceNowProdChangeAPI.query query, MailMessage mm,
            ServiceNowProdChangeAPI.ServiceNow_Prod_ChangeQuery changeQuery, string[] changeTickets)
        {
            var serviceNowUrl = ConfigurationManager.AppSettings["ServiceNowProductionLink"];

            foreach (string ticket in changeTickets)
            {
                mm.Subject = "TFS Integration Ticket #: " + ticket;
                query.change_number = ticket;
                ServiceNowProdChangeAPI.queryResponse qr = changeQuery.query(query);

                string body = "<html><body>";


                foreach (ServiceNowProdChangeAPI.queryResponseGroup qrg in qr.approvals)
                {
                    var assignmentGroup = qrg.assignment_group;
                    var comments = qrg.comments;
                    var status = qrg.status;
                    var location = qrg.location;

                    body += "<div><span>Assignment Group: " + assignmentGroup + "</span><br />";
                    body += "<span><b>Status: " + status + "</b></span><br />";
                    body += "<span>Comments: " + comments + "</span><br />";
                    body += "<span>Location: " + location + "</span><br /><br /></div>";
                    //body += "<span>ServiceNowURL: " + serviceNowUrl + "</span><br /><br /></div>";

                }

                body += "</body></html>";

                //ticketDirectory = executingExePath + "\\" + ticket;
                mm.HtmlBody = body;

                SaveFileOnNASDrive(mm, ticket);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void ServiceNowDevApiCall()
        {
            ServiceNowChangeAPI.query query = new ServiceNowChangeAPI.query();
            SetAsposeLicense();

            //on webproject
            //licEmail.SetLicense(HttpContext.Current.Server.MapPath("AsposeLicense\Aspose.Email.lic"))

            MailMessage mm = new MailMessage();

            try
            {
                mm.From = ConfigurationManager.AppSettings["FromEmail"].ToString();
                mm.To = ConfigurationManager.AppSettings["ToEmail"].ToString();

                ServiceNowChangeAPI.ServiceNow_ChangeQuery changeQuery = new
                ServiceNowChangeAPI.ServiceNow_ChangeQuery { Credentials = new NetworkCredential("ptsw", "pt5w") };

                //string[] changeTickets = { "CHG0000064953", "CHG0000064641", "CHG0000064645",
                //                     "CHG0000064781","CHG0000064782","CHG0000064783",
                //                     "CHG0000064784","CHG0000064787","CHG0000064788"};

                //string[] changeTickets = { "CHG0000064783", "CHG0000064787" }; // all approved
                string[] changeTickets = { "CHG0000064783" }; // all approved

                ProcessDevTickets(query, mm, changeQuery, changeTickets);
            }
            catch (Exception ex)
            {
                //throw ex.Message;
            }
        }

        private void ProcessDevTickets(ServiceNowChangeAPI.query query, MailMessage mm,
            ServiceNowChangeAPI.ServiceNow_ChangeQuery changeQuery, string[] changeTickets)
        {
            var serviceNowUrl = ConfigurationManager.AppSettings["ServiceNowDevLink"];

            foreach (string ticket in changeTickets)
            {
                mm.Subject = "TFS Integration Ticket #: " + ticket;
                query.change_number = ticket;
                ServiceNowChangeAPI.queryResponse qr = changeQuery.query(query);

                string body = "<html><body>";


                foreach (ServiceNowChangeAPI.queryResponseGroup qrg in qr.approvals)
                {
                    var assignmentGroup = qrg.assignment_group;
                    var comments = qrg.comments;
                    var status = qrg.status;
                    var location = qrg.location;

                    body += "<div><span>Assignment Group: " + assignmentGroup + "</span><br />";
                    body += "<span><b>Status: " + status + "</b></span><br />";
                    body += "<span>Comments: " + comments + "</span><br />";
                    body += "<span>Location: " + location + "</span><br /><br /></div>";
                    //body += "<span>ServiceNowURL: " + serviceNowUrl + "</span><br /><br /></div>";

                }

                body += "</body></html>";

                //ticketDirectory = executingExePath + "\\" + ticket;
                mm.HtmlBody = body;

                SaveFileOnNASDrive(mm, ticket);
            }
        }

        private void SaveFileOnNASDrive(MailMessage mm, string ticket)
        {
            //this.ticketDirectory = fileUploadPath + @"\" + ticket;
            this.ticketDirectory = fileUploadPath + @"\ServiceNowChangeTickets\" + ticket;
            Impersonation.Impersonation imp = new Impersonation.Impersonation();

            if (imp.impersonateValidUser(fileUploadPath))
            {
                if (!Directory.Exists(ticketDirectory))
                    Directory.CreateDirectory(ticketDirectory);

                mm.Save(ticketDirectory + "\\" + ticket + ".msg", MessageFormat.Msg); //save .msg file on disk

                imp.undoImpersonation(fileUploadPath);
            }
        }

        private void SetAsposeLicense()
        {
            //set license
            License asposeLicense = new License();
            asposeLicense.SetLicense(this.executingExePath + "\\" + "Aspose.Email.lic");
        }


        /// <summary>
        /// 
        /// </summary>
        //private static void ServiceNowGroupApiCall()
        //{
        //    ServiceNowGroupAPI.query query = new ServiceNowGroupAPI.query();

        //    ServiceNowGroupAPI.ServiceNow_GroupQuery groupQuery = new
        //        ServiceNowGroupAPI.ServiceNow_GroupQuery();

        //    groupQuery.Credentials = new NetworkCredential("ptsw", "pt5w");

        //    string[] groups = { "CTO GL AD ECR - GFTS SOLUTIONS DEV" };

        //    foreach (string name in groups)
        //    {
        //        query.group = name;
        //        ServiceNowGroupAPI.queryResponse qr = groupQuery.query(query);
        //    }
        //}
    }
}
