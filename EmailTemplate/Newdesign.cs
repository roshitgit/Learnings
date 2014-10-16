using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;
using System.IO;
using System.Net.Mail;
using System.Collections.ObjectModel;
using System.Net.Mime;
using System.Collections;
using System.Reflection;
using System.Data;
using System.Windows.Forms;
using System.ComponentModel;
using System.Configuration;
using CAMPExceptionEmailer;
using System.Diagnostics;

namespace NS
{
    public class GeneratePieChart
    {
        private DataRepository da = new DataRepository();
        public void CreateChart()
        {
            try
            {
                Console.WriteLine("fetching data...");

                var personGeids = da.GetPersonInfoForUATTesting();
                //var personGeids = da.GetPersonInfoForProduction();

                for (int i = 0; i < 3; i++)
                {
                    var riskIssueCount = da.GetRiskIssuesTilesCount(personGeids[i].geid, personGeids[i].geid);
                    Console.WriteLine("riskIssueCount..." + riskIssueCount.Count);

                    var ridOverdue = da.GetOverdueRisksForEmail(personGeids[i].geid, personGeids[i].geid);
                    Console.WriteLine("ridOverdue..." + ridOverdue.Count);

                    var ridCurrent = da.GetCurrentRisksForEmail(personGeids[i].geid, personGeids[i].geid);
                    Console.WriteLine("ridCurrent..." + ridCurrent.Count);

                    var ridUpcoming = da.GetUpcomingRisksForEmail(personGeids[i].geid, personGeids[i].geid);
                    Console.WriteLine("ridUpcoming..." + ridUpcoming.Count);

                    var dtPerson = personGeids.ToDataTable();
                    var dtOverdue = ridOverdue.ToDataTable();
                    var dtCurrent = ridCurrent.ToDataTable();
                    var dtUpcoming = ridUpcoming.ToDataTable();
                    var dtRiskCount = riskIssueCount.ToDataTable();

                    if (dtOverdue.Rows.Count > 0)
                    {
                        Console.WriteLine("converted to datatable...");

                        Chart riskPieChart = CustomizeChart(dtRiskCount);

                        StringBuilder stringBuilder;
                        string formattedData;
                        CreateEmailLayout(riskIssueCount, dtPerson, dtOverdue, dtCurrent, dtUpcoming, out stringBuilder, out formattedData, i);


                        string address = ConfigurationManager.AppSettings["EmailsFrom"];
                        MailMessage message = new MailMessage();
                        message.From = new MailAddress(address);
                        message.Subject = "";
                        message.IsBodyHtml = true;
                        message.Body = ((object)stringBuilder).ToString();


                        AlternateView alternateViewFromString;
                        string chartfilePath;
                        SetupLinkedResource(riskPieChart, formattedData, message, out alternateViewFromString, out chartfilePath);

                        //File.WriteAllText("testfile" + Guid.NewGuid() + ".txt", formattedData);

                        bool IsTestMode = Convert.ToBoolean(ConfigurationManager.AppSettings["IsTestMode"]);

                        if (IsTestMode)
                        {
                            message.To.Add(ConfigurationManager.AppSettings["EmailsTo"]);
                            message.CC.Add(ConfigurationManager.AppSettings["EmailsCc"]);

                            new SmtpClient()
                            {
                                //Host = ConfigurationManager.AppSettings["SMTP-Server"],
                                //Port = 25
                                Host = ConfigurationManager.AppSettings["SMTP-Local"],
                                Port = 26
                            }.Send(message);
                        }
                        else
                        {
                            string escalationRoutes = da.GetEscalationRouteMailIds(personGeids[i].geid);
                            escalationRoutes = escalationRoutes.TrimEnd(',');
                            message.To.Add(escalationRoutes);

                            new SmtpClient()
                            {
                                //Host = ConfigurationManager.AppSettings["SMTP-Server"],
                                //Port = 25
                                Host = ConfigurationManager.AppSettings["SMTP-Local"],
                                Port = 26
                            }.Send(message);
                        }

                        Console.WriteLine("Mail sent");

                        //memStreamPieChart.Flush();
                        //memStreamPieChart.Dispose();

                        //DisplayContentTypes(alternateViewFromString);

                        alternateViewFromString.Dispose();
                        File.Delete(chartfilePath);
                        Console.WriteLine("chartfilePath..." + chartfilePath);
                    }
                }
            }
            catch (Exception ex)
            {
                new Emailer().Publish(new CustomException { ErrorMessage = ex.Message, MethodName = new StackTrace().GetFrame(1).GetMethod().Name, StackTrace = ex.StackTrace, Source = ex.Source });
            }
        }

        private static void SetupLinkedResource(Chart riskPieChart, string formattedData, MailMessage message, out AlternateView alternateViewFromString, out string chartfilePath)
        {
            alternateViewFromString = AlternateView.CreateAlternateViewFromString(formattedData, null, "text/html");
            string executablePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            string logoFilePath = executablePath + @"\Images\citilogo_header.png";
            string loginLogoFilePath = executablePath + @"\Images\log-in.png";


            chartfilePath = executablePath + @"\Image" + Guid.NewGuid().ToString() + ".png";
            //MemoryStream memStreamPieChart = new MemoryStream();
            //memStreamPieChart.Position = 0;
            //riskPieChart.SaveImage((Stream)memStreamPieChart, ChartImageFormat.Png);
            riskPieChart.SaveImage(chartfilePath, ChartImageFormat.Png);
            Console.WriteLine("charts saved...");

            LinkedResource logolinkedResource = new LinkedResource(logoFilePath);
            logolinkedResource.ContentId = "Logo";
            logolinkedResource.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;


            LinkedResource pieChartlinkedResource = new LinkedResource(chartfilePath);
            //LinkedResource pieChartlinkedResource = new LinkedResource((Stream)memStreamPieChart, "image/png");
            pieChartlinkedResource.ContentId = "RiskPieChart";
            pieChartlinkedResource.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;

            LinkedResource loginlinkedResource = new LinkedResource(loginLogoFilePath);
            loginlinkedResource.ContentId = "Login";
            loginlinkedResource.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;


            alternateViewFromString.LinkedResources.Add(loginlinkedResource);
            alternateViewFromString.LinkedResources.Add(logolinkedResource);
            alternateViewFromString.LinkedResources.Add(pieChartlinkedResource);

            message.AlternateViews.Add(alternateViewFromString);
            Console.WriteLine("Added to alternate views...");
        }

        private void CreateEmailLayout(List<RiskIssuesCount> riskCount, DataTable dtName,
            DataTable dtOverdue, DataTable dtCurrent, DataTable dtUpcoming, out StringBuilder stringBuilder, out string formattedData, int counter)
        {
            stringBuilder = new StringBuilder();
            stringBuilder.Append("<html>");
            stringBuilder.Append("<head>");
            stringBuilder.Append("<title></title>");
            stringBuilder.Append("</head>");
            stringBuilder.Append("<body>");
            stringBuilder.Append("<table cellpadding=\"0\" cellspacing=\"0\" style=\"\font-family: Helvetica, Arial; color: #2b2b2b; width:100%; background: #afd8f8;\">");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td colspan=\"3\" style=\" font-family: Helvetica, Arial; color: #2b2b2b; height:10px;\"></td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td width=\"20%\"></td>");
            stringBuilder.Append("<td width=\"60%\" style=\"padding-bottom:10px;\">");
            stringBuilder.Append("<table cellpadding=\"0\" cellspacing=\"0\" style=\"width:100%; background-color:#004173;\">");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style=\"font-family: Helvetica, Arial; color:white; font-size:16pt; padding:20px 0px 20px 10px;\">");
            stringBuilder.Append("Citi Application Manager Portal (CAMP)");
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td style=\" font-family: Helvetica, Arial; color: #2b2b2b; text-align:right; padding-right:10px;\">");
            stringBuilder.Append("<img src=\"cid:Logo\" />");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td colspan=\"2\">");
            stringBuilder.Append("<table cellpadding=\"0\" cellspacing=\"10\" style=\" font-family: Helvetica, Arial; color: #2b2b2b; width:100%; background-color:white;\">");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td>");
            stringBuilder.AppendFormat("<h2 style=\" font-family: Helvetica, Arial; color: #2b2b2b; margin-top: 5px;  margin-bottom: 0;\">Weekly Commitment Summary for {0}</h2>", dtName.Rows[counter]["Name"]);
            stringBuilder.AppendFormat("<h3 style=\" font-family: Helvetica, Arial; color: #2b2b2b; color: #9a9a9a; margin-top: 5px; margin-bottom:8px; font-size: 11pt;\">{0}</h3>", DateTime.Now.ToString("dddd, MMMM dd, yyyy"));
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td style=\" font-family: Helvetica, Arial; color: #2b2b2b; text-align:right; vertical-align:top; padding-top:5px;\">");
            stringBuilder.Append("<a href=\"http://camp.nam.nsroot.net/\" target=\"_blank\" title=\"Log into CAMP\"><img src=\"cid:Login\" style=\" font-family: Helvetica, Arial; color: #2b2b2b; border: none;\" /></a>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td colspan=\"2\" style=\" font-family: Helvetica, Arial; color: #2b2b2b; border: 1px solid #dddddd; background:#eeeeee; margin: 5px; padding: 5px; padding-bottom: 10px;\">");
            stringBuilder.Append("<h4 style=\" font-family: Helvetica, Arial; color: #2b2b2b; color: #05589b; font-size: 16pt; margin-bottom:16px;\">Alerts</h4>");
            if (riskCount.Count > 0)
            {
                stringBuilder.Append("<p style=\"font-size:10pt; margin:0px 0px 8px 0px;\">");
                stringBuilder.AppendFormat("<span style=\" font-family: Helvetica, Arial; color: #2b2b2b; background: #cc4a4a;\">&nbsp;&nbsp;</span>&nbsp;&nbsp;<strong style=\"font-size:12pt;\">{0}</strong> overdue risk commitments that need your immediate attention", riskCount[0].RiskIssueCntOvrDue);
                stringBuilder.Append("</p>");
                stringBuilder.Append("<p style=\"font-size:10pt; margin:0px 0px 8px 0px;\">");
                stringBuilder.AppendFormat("<span style=\" font-family: Helvetica, Arial; color: #2b2b2b; background: #edc242;\">&nbsp;&nbsp;</span>&nbsp;&nbsp;<strong style=\"font-size:12pt;\">{0}</strong> risk commitments that are currently due in the next 90 days", riskCount[0].RiskIssueCntUpCmg);
                stringBuilder.Append("</p>");
            }
            else
            {
                stringBuilder.Append("<p style=\"font-size:10pt; margin:0px 0px 8px 0px;\">No overdue or upcoming risk commitments</p>");
            }
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td colspan=\"2\" style=\" font-family: Helvetica, Arial; color: #2b2b2b; border: 1px solid #dddddd; margin: 5px; padding: 5px; padding-bottom: 10px;\">");
            stringBuilder.Append("<h4 style=\" font-family: Helvetica, Arial; color: #2b2b2b; color: #05589b; font-size: 16pt; margin-bottom:16px;\">Risk Commitments by Category</h4>");
            stringBuilder.Append("<img src=\"cid:RiskPieChart\" />");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td colspan=\"2\" style=\" font-family: Helvetica, Arial; color: #2b2b2b; border: 1px solid #dddddd; margin: 5px; padding: 5px; padding-bottom: 10px;\">");
            stringBuilder.Append("<h4 style=\" font-family: Helvetica, Arial; color: #2b2b2b; color: #05589b; font-size: 16pt; margin-bottom:16px;\">Top 10 Risk Commitments</h4>");
            stringBuilder.Append("<table cellpadding=\"10\" cellspacing=\"0\" style=\" font-family: Helvetica, Arial; color: #2b2b2b; width:100%; border: none; font-size: 10pt;\">");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<th style=\" font-family: Helvetica, Arial; color: #2b2b2b; text-align:left;\">Status</th>");
            stringBuilder.Append("<th style=\" font-family: Helvetica, Arial; color: #2b2b2b; text-align:left;\">Type</th>");
            stringBuilder.Append("<th style=\" font-family: Helvetica, Arial; color: #2b2b2b; text-align:left;\">Application Name</th>");
            stringBuilder.Append("<th style=\" font-family: Helvetica, Arial; color: #2b2b2b; text-align:left;\">Due Date</th>");
            stringBuilder.Append("<th style=\" font-family: Helvetica, Arial; color: #2b2b2b; text-align:left;\">Priority</th>");
            stringBuilder.Append("<th style=\" font-family: Helvetica, Arial; color: #2b2b2b; text-align:left;\">Age</th>");
            stringBuilder.Append("</tr>");
            if (dtOverdue.Rows.Count > 0)
            {
                foreach (DataRow myRow in dtOverdue.Rows)
                {
                    stringBuilder.Append("<tr>");
                    stringBuilder.AppendFormat("<td style=\"font-family: Helvetica, Arial; color: #2b2b2b; border-top:1px solid #dddddd;\" nowrap=\"true\"><span><span style=\" font-family: Helvetica, Arial; color: #2b2b2b; background: #cc4a4a;\">&nbsp;&nbsp;</span>&nbsp;&nbsp;{0}</td>", myRow["Status"]);
                    stringBuilder.AppendFormat("<td style=\"font-family: Helvetica, Arial; color: #2b2b2b; border-top:1px solid #dddddd;\">{0}</td>", myRow["Type"]);
                    stringBuilder.AppendFormat("<td style=\"font-family: Helvetica, Arial; color: #2b2b2b; border-top:1px solid #dddddd;\">{0}</td>", myRow["AppName"].ToString().Replace("-", " - "));
                    stringBuilder.AppendFormat("<td style=\"font-family: Helvetica, Arial; color: #2b2b2b; border-top:1px solid #dddddd;\">{0}</td>", myRow["DueDate"]);
                    stringBuilder.AppendFormat("<td style=\"font-family: Helvetica, Arial; color: #2b2b2b; border-top:1px solid #dddddd;\">{0}</td>", myRow["Priority"]);
                    stringBuilder.AppendFormat("<td style=\"font-family: Helvetica, Arial; color: #2b2b2b; border-top:1px solid #dddddd;\">{0}</td>", myRow["Age"]);
                    stringBuilder.Append("</tr>");   
                }
            }
            if (dtCurrent.Rows.Count > 0)
            {
                foreach (DataRow myRow in dtCurrent.Rows)
                {
                    stringBuilder.Append("<tr>");
                    stringBuilder.AppendFormat("<td style=\"font-family: Helvetica, Arial; color: #2b2b2b; border-top:1px solid #dddddd;\" nowrap=\"true\"><span><span style=\" font-family: Helvetica, Arial; color: #2b2b2b; background: #edc242;\">&nbsp;&nbsp;</span>&nbsp;&nbsp;{0}</td>", myRow["Status"]);
                    stringBuilder.AppendFormat("<td style=\"font-family: Helvetica, Arial; color: #2b2b2b; border-top:1px solid #dddddd;\">{0}</td>", myRow["Type"]);
                    stringBuilder.AppendFormat("<td style=\"font-family: Helvetica, Arial; color: #2b2b2b; border-top:1px solid #dddddd;\">{0}</td>", myRow["AppName"].ToString().Replace("-", " - "));
                    stringBuilder.AppendFormat("<td style=\"font-family: Helvetica, Arial; color: #2b2b2b; border-top:1px solid #dddddd;\">{0}</td>", myRow["DueDate"]);
                    stringBuilder.AppendFormat("<td style=\"font-family: Helvetica, Arial; color: #2b2b2b; border-top:1px solid #dddddd;\">{0}</td>", myRow["Priority"]);
                    stringBuilder.AppendFormat("<td style=\"font-family: Helvetica, Arial; color: #2b2b2b; border-top:1px solid #dddddd;\">{0}</td>", myRow["Age"]);
                    stringBuilder.Append("</tr>");   
                }
            }
            if (dtUpcoming.Rows.Count > 0)
            {
                foreach (DataRow myRow in dtUpcoming.Rows)
                {
                    stringBuilder.Append("<tr>");
                    stringBuilder.AppendFormat("<td style=\"font-family: Helvetica, Arial; color: #2b2b2b; border-top:1px solid #dddddd;\" nowrap=\"true\"><span><span style=\" font-family: Helvetica, Arial; color: #2b2b2b; background: #4ea84e;\">&nbsp;&nbsp;</span>&nbsp;&nbsp;{0}</td>", myRow["Status"]);
                    stringBuilder.AppendFormat("<td style=\"font-family: Helvetica, Arial; color: #2b2b2b; border-top:1px solid #dddddd;\">{0}</td>", myRow["Type"]);
                    stringBuilder.AppendFormat("<td style=\"font-family: Helvetica, Arial; color: #2b2b2b; border-top:1px solid #dddddd;\">{0}</td>", myRow["AppName"].ToString().Replace("-", " - "));
                    stringBuilder.AppendFormat("<td style=\"font-family: Helvetica, Arial; color: #2b2b2b; border-top:1px solid #dddddd;\">{0}</td>", myRow["DueDate"]);
                    stringBuilder.AppendFormat("<td style=\"font-family: Helvetica, Arial; color: #2b2b2b; border-top:1px solid #dddddd;\">{0}</td>", myRow["Priority"]);
                    stringBuilder.AppendFormat("<td style=\"font-family: Helvetica, Arial; color: #2b2b2b; border-top:1px solid #dddddd;\">{0}</td>", myRow["Age"]);
                    stringBuilder.Append("</tr>");   
                }
            }
            stringBuilder.Append("</table>"); 
            stringBuilder.Append("</td>"); 
            stringBuilder.Append("</tr>"); 
            stringBuilder.Append("</table>"); 
            stringBuilder.Append("</td>"); 
            stringBuilder.Append("</tr>"); 
            stringBuilder.Append("<tr>"); 
            stringBuilder.Append("<td colspan=\"2\" style=\" font-family: Helvetica, Arial; padding-top:5px; padding-bottom:5px; padding-right:10px; background:#afd8f8; color:#004173; font-size:9pt; text-align:right;\">"); 
            stringBuilder.AppendFormat("&copy; Citigroup {0}", DateTime.Now.ToString("yyyy")); 
            stringBuilder.Append("</td>"); 
            stringBuilder.Append("</tr>"); 
            stringBuilder.Append("</table>"); 
            stringBuilder.Append("</td>"); 
            stringBuilder.Append("<td width=\"20%\"></td>"); 
            stringBuilder.Append("</tr>"); 
            stringBuilder.Append("</table>"); 
            stringBuilder.Append("</body>"); 
            stringBuilder.Append("</html>");

            formattedData = stringBuilder.ToString();
        }

        private static Chart CustomizeChart(DataTable dtRiskCount)
        {
            ColorConverter cc3 = new ColorConverter();
            Chart riskPieChart = new Chart();
            riskPieChart.Width = 450;
            riskPieChart.Height = 250;
            riskPieChart.BackColor = (Color)cc3.ConvertFromString("#FFFFFF");

            /*Create Tiles*/
            //Title title = new Title { Name = "CAMP Email Template", ShadowOffset = 3 };
            //riskPieChart.Titles.Add(title); //add title

            ColorConverter cc2 = new ColorConverter();
            /*Create Legend*/
            Legend legend = new Legend
            {
                Name = "RiskTotal",
                Alignment = StringAlignment.Near,
                Docking = Docking.Right,
                IsTextAutoFit = true,
                BackColor = (Color)cc2.ConvertFromString("#FFFFFF"),
                Font = new Font("Verdana", 8),
                LegendStyle = LegendStyle.Table
            };
            riskPieChart.Legends.Add(legend); //add legends


            /*Create ChartArea*/
            ColorConverter cc1 = new ColorConverter();

            ChartArea chartArea = new ChartArea { Name = "PieChartArea" };
            chartArea.BackColor = (Color)cc1.ConvertFromString("#FFFFFF");
            chartArea.BorderWidth = 0;
            riskPieChart.ChartAreas.Add(chartArea); //add chartarea

            /*Create Series*/
            Series series1 = new Series { Name = "pieSeries" };
            riskPieChart.Series.Add(series1);

            //if (dtRiskCount.Rows.Count > 0)
            //{
            //    int totalCount = Convert.ToInt32(dtRiskCount.Rows[0]["riskissueidcnttotal"]);
            //}

            for (int i = 1; i < dtRiskCount.Rows.Count; i++)
            {
                string riskName = dtRiskCount.Rows[i]["ricategoryname"].ToString().ToUpper();
                //double percentValue = ((Convert.ToDouble(dtRiskCount.Rows[i]["riskissueidcnttotal"]) / totalCount) * 100);
                //riskPieChart.Series["pieSeries"].Points.AddXY(riskName, percentValue); 
                riskPieChart.Series["pieSeries"].Points.AddXY(riskName, Convert.ToInt32(dtRiskCount.Rows[i]["riskissueidcnttotal"]));
            }

            ColorConverter cc = new ColorConverter();
            string[] arrColors = ConfigurationManager.AppSettings["PieChartSeriesColors"].Split(',');
            Random random = new Random();
            var seriesCollection = riskPieChart.Series["pieSeries"];

            for (int i = 0; i < seriesCollection.Points.Count; i++)
            {
                seriesCollection.Points[i].Color = (Color)cc.ConvertFromString(arrColors[i].ToString());
                //seriesCollection.Points[i].Label = string.Empty;
                //seriesCollection.Points[i].IsVisibleInLegend = true;
            }


            //Random random = new Random();
            //foreach (var item in riskPieChart.Series["pieSeries"].Points)
            //{
            //    System.Drawing.Color c = System.Drawing.Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
            //    item.Color = c;
            //    item.IsVisibleInLegend = true;
            //}

            //riskPieChart.Series["pieSeries"]["PieLabelStyle"] = "Outside";
            riskPieChart.Series["pieSeries"]["PieLabelStyle"] = "Disabled";

            riskPieChart.Series["pieSeries"].ChartType = SeriesChartType.Doughnut;
            riskPieChart.ChartAreas["PieChartArea"].Area3DStyle.Enable3D = false;
            //riskPieChart.ChartAreas["PieChartArea"].Area3DStyle.Inclination = -50;
            riskPieChart.Legends["RiskTotal"].Enabled = true;

            //riskPieChart.Series["pieSeries"].Label = "#VALX #PERCENT{P0}";
            riskPieChart.Series["pieSeries"].IsVisibleInLegend = true;
            //riskPieChart.Series["pieSeries"].Label = "#VALY - (#VALX)\n#PERCENT";
            //riskPieChart.Series["pieSeries"].Label = "#VALY - (#VALX)";
            riskPieChart.Series["pieSeries"].Label = "#VALY - #VALX";
            return riskPieChart;
        }

        //public string toHTML_Table(DataTable targetTable)
        //{
        //    if (targetTable.Rows.Count == 0)
        //        return "";

        //    System.Text.StringBuilder myBuilder = new System.Text.StringBuilder();

        //    myBuilder.Append("<table width='100%' cellpadding='0' cellspacing='0'>");
        //    myBuilder.Append("<tr align='left' valign='top' style='background-color: #C9C9C9; font-size: 12px; font-family: Verdana; color: Black;'>"); //grdHeader class will be in CSS file

        //    foreach (DataColumn myColumn in targetTable.Columns)
        //    {
        //        string colName = myColumn.ColumnName.ToLower();
        //        if (colName.Equals("type"))
        //        {
        //            myBuilder.Append("<td align='left' valign='top' style=\"width: 15%;\">");
        //        }
        //        if (colName.Equals("appname"))
        //        {
        //            myBuilder.Append("<td align='left' valign='top' style=\"width: 48%;\">");
        //        }
        //        if (colName.Equals("duedate"))
        //        { 
        //            myBuilder.Append("<td align='left' valign='top' style=\"width: 13%;\">");
        //        }
        //        if (colName.Equals("priority"))
        //        {
        //            myBuilder.Append("<td align='center' valign='top' style=\"width: 8%;\">");
        //        }
        //        if (colName.Equals("status"))
        //        {
        //            myBuilder.Append("<td align='left' valign='top' style=\"width: 9%;\">");
        //        }
        //        if (colName.Equals("age"))
        //        {
        //            myBuilder.Append("<td align='center' valign='top' style=\"width: 7%;\">");
        //        }
        //        myBuilder.Append(myColumn.ColumnName);
        //        myBuilder.Append("</td>");
        //    }
        //    myBuilder.Append("</tr>");
        //    //Add the data rows.
        //    int intI = 0;
        //    int totRow = targetTable.Rows.Count - 1;
        //    foreach (DataRow myRow in targetTable.Rows)
        //    {
        //        if (intI % 2 == 0)
        //            //myBuilder.Append("<tr align='left' style='font-family: Verdana; font-weight: normal; font-size: 10px; background-color: rgb(242, 251, 254);'>");
        //            myBuilder.Append("<tr align='left' style='font-family: Verdana; font-weight: normal; font-size: 11px; background-color:#F2FBFE;'>");
        //        else
        //            myBuilder.Append("<tr align='left' style='font-family: Verdana; font-weight: normal; font-size: 11px; background-color:#FFFFFF;'>");

        //        int colcount = 0;
        //        foreach (DataColumn myColumn in targetTable.Columns)
        //        {
        //            //myBuilder.Append("<td padding='10px' align='left' valign='top'><label style=\"cursor:pointer;\">");
        //            string colName = myColumn.ColumnName;
        //            string columnValue = myRow[colName].ToString();

        //            if (colName.ToLower().Equals("status"))
        //            {
        //                if (columnValue.ToLower().Equals("overdue"))
        //                {
        //                    myBuilder.Append("<td align='left' valign='top' style='background-color:red; color:white'>");
        //                }
        //                if (columnValue.ToLower().Equals("current"))
        //                {
        //                    myBuilder.Append("<td align='left' valign='top' style='background-color:#FFA500; color:white'>");
        //                }
        //                if (columnValue.ToLower().Equals("upcoming"))
        //                {
        //                    myBuilder.Append("<td align='left' valign='top' style='background-color:#008000; color:white'>");
        //                }
        //            }
        //            else
        //            {
        //                if (colName.ToLower().Equals("priority"))
        //                {
        //                    myBuilder.Append("<td align='right' valign='top'>");
        //                }
        //                else if (colName.ToLower().Equals("age"))
        //                {
        //                    myBuilder.Append("<td align='right' valign='top'>");
        //                }
        //                else
        //                {
        //                    myBuilder.Append("<td align='left' valign='top'>");
        //                }
        //            }

        //            string truncatedvalue = (columnValue.Length > 55) ? columnValue.Substring(0, 55) + ".." : columnValue;
        //            myBuilder.Append(truncatedvalue);
        //            myBuilder.Append("</td>");

        //            colcount++;
        //        }
        //        myBuilder.Append("</tr>");
        //        intI += 1;
        //    }
        //    //Close tags.
        //    return myBuilder.Append("</table>").ToString();
        //}
    }
}
