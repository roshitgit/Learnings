using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI;
using WebApi = System.Web.Http;

namespace CAMPDashboard.Controllers
{
    public class ExcelReportController : ApiController
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
            string[] columnNames = { "application", "riskmitigationlink" };
            if (columnNames.Contains(colName))
                return false;
            else
                return true;
        }


        [WebApi.HttpPost]
        [WebApi.ActionName("ExcelFile")]
        public HttpResponseMessage GenerateExcelReport([FromBody] List<RiskIssueDetails> Id)
        {
            try
            {
                DataTable data = Id.ToDataTable();

                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                    {
                        htw.AddAttribute(HtmlTextWriterAttribute.Border, "1");
                        htw.RenderBeginTag(HtmlTextWriterTag.Table); //Start Html Table 

                        htw.RenderBeginTag(HtmlTextWriterTag.Tr); //Inserting Title of the Report
                        htw.AddAttribute(HtmlTextWriterAttribute.Colspan, data.Columns.Count.ToString());

                        htw.AddAttribute(HtmlTextWriterAttribute.Align, "Center");
                        htw.AddAttribute(HtmlTextWriterAttribute.Style, "font-family:Verdana;font-size:12pt;font-weight:bold;");
                        htw.RenderBeginTag(HtmlTextWriterTag.Td);
                        htw.Write("Citi Application Manager Portal Excel Report");
                        htw.RenderEndTag();
                        htw.RenderEndTag();

                        htw.RenderBeginTag(HtmlTextWriterTag.Tr);//Writing Report Column Headings

                        foreach (DataColumn dc in data.Columns)
                        {
                            string name = dc.ColumnName.ToLower();
                            if (ExcludeColumns(name))
                            {
                                htw.AddAttribute(HtmlTextWriterAttribute.Style, "font-family:Verdana;font-size:8pt;");
                                htw.RenderBeginTag(HtmlTextWriterTag.Th);
                                htw.Write(dc.ColumnName);
                                htw.RenderEndTag();
                            }
                        }

                        htw.RenderEndTag();

                        foreach (DataRow dr in data.Rows)
                        {
                            htw.RenderBeginTag(HtmlTextWriterTag.Tr);//Writing Data 

                            foreach (DataColumn dc in data.Columns)
                            {
                                string name = dc.ColumnName.ToLower();
                                if (ExcludeColumns(name))
                                {
                                    htw.AddAttribute(HtmlTextWriterAttribute.Style, @"font-family:Verdana;font-size:8pt;mso-number-format:\@;");
                                    htw.RenderBeginTag(HtmlTextWriterTag.Td);
                                    htw.Write(dr[dc]);
                                    htw.RenderEndTag();
                                }
                            }

                            htw.RenderEndTag();
                        }

                         using (var memoStream = new MemoryStream())
                        {
                            using (StreamWriter writer = new StreamWriter(memoStream))
                            {
                                writer.Write(sw.ToString());
                                writer.Flush();
                            }
                            memoStream.Flush();
                            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                            result.Content = new ByteArrayContent(memoStream.ToArray());

                            //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.ms-excel");

                            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                            result.Content.Headers.ContentDisposition.FileName = "CitiApplicationManagerPortal.xls";
                            return result; 
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException uae)
            {
                HttpResponseMessage message = new HttpResponseMessage()
                {
                    Content = new StringContent(uae.Message)
                };

                throw new System.Web.Http.HttpResponseException(message);
                
            }
            catch (Exception ex)
            {
                HttpResponseMessage message = new HttpResponseMessage()
                {
                    Content = new StringContent(ex.Message)
                };

                throw new System.Web.Http.HttpResponseException(message);
                
            }
        }
    }
}
