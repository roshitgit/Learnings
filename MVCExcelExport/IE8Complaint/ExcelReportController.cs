using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI;
using System.Web;


namespace CAMPDashboard.Controllers
{
    public class ExcelReportController : Controller
    {
        //
        // GET: /ExcelReport/
        private readonly IRiskIssues riskIssues = null;
        public ExcelReportController()
        {
            //Resolve dependency to get concrete class 
            if (riskIssues == null)
                riskIssues = DependencyResolver.Current.GetService<IRiskIssues>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool ExcludeColumns(string colName)
        {
            string[] columnNames = { "n1", "n2" };
            if (columnNames.Contains(colName))
                return false;
            else
                return true;
        }


      
        public void GenerateExcelReport(string RIOwner)
        {
            List<RiskIssueDetails> list = new List<RiskIssueDetails>();

            try
            {
                list = riskIssues.GetRiskIssueDetails(RIOwner, User.Identity.Name.LoggedInUserData());

                HttpContext context = System.Web.HttpContext.Current;
                HttpResponse response = context.Response;

                response.ClearContent();
                response.Buffer = true;
                response.Clear();
                response.ClearHeaders();
                response.Charset = "";
                response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");

                //response.ContentType = "application/vnd.ms-excel";
                response.ContentType = "application/ms-excel"; //for .xls
                //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; //for .xlsx

                Response.AddHeader("Content-Disposition", "_inline; filename=CitiApplicationManagerPortal.xls");

                DataTable data = list.ToDataTable();

                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                    {
                        htw.AddAttribute(HtmlTextWriterAttribute.Border, "1");
                        htw.RenderBeginTag(HtmlTextWriterTag.Table); //Start Html Table 

                        htw.RenderBeginTag(HtmlTextWriterTag.Tr); //Inserting Title of the Report
                        htw.AddAttribute(HtmlTextWriterAttribute.Colspan, data.Columns.Count.ToString());

                        //htw.AddAttribute(HtmlTextWriterAttribute.Colspan, list.GetType().GetGenericArguments()[0].GetProperties().Length.ToString());

                        htw.AddAttribute(HtmlTextWriterAttribute.Align, "Center");
                        htw.AddAttribute(HtmlTextWriterAttribute.Style, "font-family:Arial;font-size:12pt;font-weight:bold;");
                        htw.RenderBeginTag(HtmlTextWriterTag.Td);
                        htw.Write("Citi Application Manager Portal Excel Report");
                        htw.RenderEndTag();
                        htw.RenderEndTag();

                        htw.RenderBeginTag(HtmlTextWriterTag.Tr);//Writing Report Column Headings

                        //PropertyInfo[] pi = list.GetType().GetProperties();
                        //foreach (var column in pi)
                        //{
                        //    htw.AddAttribute(HtmlTextWriterAttribute.Style, "font-family:Arial;font-size:8pt;");
                        //    htw.RenderBeginTag(HtmlTextWriterTag.Th);
                        //    htw.Write(column.Name);
                        //    htw.RenderEndTag();
                        //}

                        foreach (DataColumn dc in data.Columns)
                        {
                            string name = dc.ColumnName.ToLower();
                            if (ExcludeColumns(name))
                            {
                                htw.AddAttribute(HtmlTextWriterAttribute.Style, "font-family:Arial;font-size:8pt;");
                                htw.RenderBeginTag(HtmlTextWriterTag.Th);
                                htw.Write(dc.ColumnName);
                                htw.RenderEndTag();
                            }
                        }

                        htw.RenderEndTag();

                        //IEnumerable<RiskIssueDetails> ieRID = list.AsEnumerable();
                        //foreach (RiskIssueDetails rid in ieRID)
                        //{

                        //    foreach (var column in rows)
                        //    {
                        //        htw.AddAttribute(HtmlTextWriterAttribute.Style, @"font-family:Arial;font-size:8pt;mso-number-format:\@;");
                        //        htw.RenderBeginTag(HtmlTextWriterTag.Td);
                        //        htw.Write(data.v);
                        //        htw.RenderEndTag();
                        //   }

                        //}

                        foreach (DataRow dr in data.Rows)
                        {
                            htw.RenderBeginTag(HtmlTextWriterTag.Tr);//Writing Data 

                            foreach (DataColumn dc in data.Columns)
                            {
                                string name = dc.ColumnName.ToLower();
                                if (ExcludeColumns(name))
                                {
                                    htw.AddAttribute(HtmlTextWriterAttribute.Style, @"font-family:Arial;font-size:8pt;mso-number-format:\@;");
                                    htw.RenderBeginTag(HtmlTextWriterTag.Td);
                                    htw.Write(dr[dc]);
                                    htw.RenderEndTag();
                                }
                            }

                            htw.RenderEndTag();
                        }

                        response.Output.Write(sw.ToString());
                        response.Flush();
                        context.ApplicationInstance.CompleteRequest();
                        response.End();
                    }
                }
            }
            catch (Exception ex)
            {
                HttpResponseMessage message = new HttpResponseMessage()
                {
                    Content = new StringContent("Error Exporting Data")
                };

                throw new System.Web.Http.HttpResponseException(message);
            }
        }
    }
}
