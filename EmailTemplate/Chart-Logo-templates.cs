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
using System.Diagnostics;

namespace <NS>
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
                    var riskIssueCount = da.Get(...)



                    var dtPerson = personGeids.ToDataTable();
                   
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

                        File.WriteAllText("testfile" + Guid.NewGuid() + ".txt", formattedData);

                        bool IsTestMode = Convert.ToBoolean(ConfigurationManager.AppSettings["IsTestMode"]);

                        

                        Console.WriteLine("Mail sent");

                        //memStreamPieChart.Flush();
                        //memStreamPieChart.Dispose();

                        //DisplayContentTypes(alternateViewFromString);

                        alternateViewFromString.Dispose();
                        //File.Delete(chartfilePath);
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

            string logoFilePath = executablePath + @"\Images\EmailLogo.PNG";
            string loginLogoFilePath = executablePath + @"\Images\download.png";


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
            stringBuilder.AppendFormat("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">");
            stringBuilder.AppendFormat("<html xmlns=\"http://www.w3.org/1999/xhtml\"><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
            stringBuilder.AppendFormat("<meta name=\"viewport\" content=\"width=device-width,initial-scale=1.0\"/></head>");
            stringBuilder.AppendFormat("<body style='background: #FFFFFF; margin:0; padding:0; border:0;'>");
            stringBuilder.AppendFormat("<table align=\"center\" width='675' style='font-family: Verdana; background: #EDEDDE; vertical-align: top;' border='0' cellpadding='0' cellspacing='0'>");
            stringBuilder.AppendFormat("<tr><td valign='top'><img src=\"cid:Logo\" width='675'/></td></tr>");
            stringBuilder.AppendFormat("<tr><td valign='top' style='text-align: center;font-size: 12px;'>Weekly Summary Report for " + dtName.Rows[counter]["Name"].ToString() + " - " + DateTime.Now.ToString("MMMM dd, yyyy") + "</td></tr>");

            stringBuilder.AppendFormat("<tr><td valign='top' style='font-size: 12px; font-family: Verdana;'>Alerts</td></tr>");

            stringBuilder.AppendFormat("<tr valign='top' style='font-family: Verdana;'>");
            if (riskCount.Count > 0)
            {
                stringBuilder.AppendFormat("<td style=\"width: 675\"><table width='100%'><tr><td style=\"width: 50%\"><table width='100%' border='0'><tr valign='top' style='font-size: 12px; color: Red; font-family: Verdana;'>");
                stringBuilder.AppendFormat("<td style='width: 12%'><a style='text-decoration:none; color: Red;' href='http://ptswvmwebu601.nam.nsroot.net/CAMPDashboard#/' target='_blank'>" + riskCount[0].RiskIssueCntOvrDue + "</a></td>");
            }
            stringBuilder.AppendFormat("<td style='width:88%' style='font-size: 12px;'>You have overdue actions that need your immediate attention.</td></tr>");

            if (riskCount.Count > 0)
            {
                stringBuilder.AppendFormat("<tr valign='top' style='font-size: 12px; font-family: Verdana;'><td style='width: 12%'>");
                stringBuilder.AppendFormat("<a style='text-decoration:none; href='http://ptswvmwebu601.nam.nsroot.net/CAMPDashboard#/' target='_blank'>" + riskCount[0].RiskIssueCntUpCmg + "</a></td>");
            }
            stringBuilder.AppendFormat("<td style='width:88%' style='font-size: 12px;'>You have risks and/or commitments that are currently due in the next 90 days.</td></tr></table></td>");

            //stringBuilder.AppendFormat("<td style=\"width: 50%\"><table width='100%' border='0'><tr><td valign='top'><img src=\"cid:RiskPieChart\" width='375' height='150' />");
            stringBuilder.AppendFormat("<td style=\"width: 50%\"><table width='100%' border='0'><tr><td valign='top'><img src=\"cid:RiskPieChart\" />");
            stringBuilder.AppendFormat("</td></tr></table></td></tr></table></td></tr>");

            stringBuilder.AppendFormat("<tr><td valign='top' style=\"text-align:center; font-size: 12px; font-family: Verdana;\">Risk and Commitments by category</td></tr>");

            stringBuilder.AppendFormat("<tr><td>[OverdueRisksTable]</td></tr>");

            if (dtCurrent.Rows.Count > 0)
            {
                stringBuilder.AppendFormat("<tr><td>[CurrentRisksTable]</td></tr>");
            }
            if (dtUpcoming.Rows.Count > 0)
            {
                stringBuilder.AppendFormat("<tr><td>[UpcomingRisksTable]</td></tr>");
            }
            stringBuilder.AppendFormat("<tr><td align='right'><a href='http://ptswvmwebu601.nam.nsroot.net/CAMPDashboard#/' target='_blank' style=\"font-family: Verdana; font-size:12px;\">Login to CAMP</a></td></tr>");
            stringBuilder.AppendFormat("</table></body></html>");

            string data = string.Empty;

            formattedData = stringBuilder.ToString().Replace("[OverdueRisksTable]", toHTML_Table(dtOverdue));
            data = formattedData;

            if (dtCurrent.Rows.Count > 0)
            {
                data = data.Replace("[CurrentRisksTable]", toHTML_Table(dtCurrent));
            }
            if (dtUpcoming.Rows.Count > 0)
            {
                data = data.Replace("[UpcomingRisksTable]", toHTML_Table(dtUpcoming));
            }
            formattedData = data;
        }

        private static Chart CustomizeChart(DataTable dtRiskCount)
        {
            ColorConverter cc3 = new ColorConverter();
            Chart riskPieChart = new Chart();
            riskPieChart.Width = 375;
            riskPieChart.Height = 150;
            riskPieChart.BackColor = (Color)cc3.ConvertFromString("#EDEDDE");

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
                BackColor = (Color)cc2.ConvertFromString("#EDEDDE"),
                Font = new Font("Verdana", 8),
                LegendStyle = LegendStyle.Table
            };
            riskPieChart.Legends.Add(legend); //add legends


            /*Create ChartArea*/
            ColorConverter cc1 = new ColorConverter();

            ChartArea chartArea = new ChartArea { Name = "PieChartArea" };
            chartArea.BackColor = (Color)cc1.ConvertFromString("#EDEDDE");
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

            riskPieChart.Series["pieSeries"].ChartType = SeriesChartType.Pie; //set to "PIE" chart
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

        public string toHTML_Table(DataTable targetTable)
        {
            if (targetTable.Rows.Count == 0)
                return "";

            System.Text.StringBuilder myBuilder = new System.Text.StringBuilder();

            myBuilder.Append("<table width='100%' cellpadding='0' cellspacing='0'>");
            myBuilder.Append("<tr align='left' valign='top' style='background-color: #C9C9C9; font-size: 12px; font-family: Verdana; color: Black;'>"); //grdHeader class will be in CSS file

            foreach (DataColumn myColumn in targetTable.Columns)
            {
                string colName = myColumn.ColumnName.ToLower();
                if (colName.Equals("type"))
                {
                    myBuilder.Append("<td align='left' valign='top' style=\"width: 15%;\">");
                }
                if (colName.Equals("appname"))
                {
                    myBuilder.Append("<td align='left' valign='top' style=\"width: 48%;\">");
                }
                if (colName.Equals("duedate"))
                { 
                    myBuilder.Append("<td align='left' valign='top' style=\"width: 13%;\">");
                }
                if (colName.Equals("priority"))
                {
                    myBuilder.Append("<td align='center' valign='top' style=\"width: 8%;\">");
                }
                if (colName.Equals("status"))
                {
                    myBuilder.Append("<td align='left' valign='top' style=\"width: 9%;\">");
                }
                if (colName.Equals("age"))
                {
                    myBuilder.Append("<td align='center' valign='top' style=\"width: 7%;\">");
                }
                myBuilder.Append(myColumn.ColumnName);
                myBuilder.Append("</td>");
            }
            myBuilder.Append("</tr>");
            //Add the data rows.
            int intI = 0;
            int totRow = targetTable.Rows.Count - 1;
            foreach (DataRow myRow in targetTable.Rows)
            {
                if (intI % 2 == 0)
                    //myBuilder.Append("<tr align='left' style='font-family: Verdana; font-weight: normal; font-size: 10px; background-color: rgb(242, 251, 254);'>");
                    myBuilder.Append("<tr align='left' style='font-family: Verdana; font-weight: normal; font-size: 11px; background-color:#F2FBFE;'>");
                else
                    myBuilder.Append("<tr align='left' style='font-family: Verdana; font-weight: normal; font-size: 11px; background-color:#FFFFFF;'>");

                int colcount = 0;
                foreach (DataColumn myColumn in targetTable.Columns)
                {
                    //myBuilder.Append("<td padding='10px' align='left' valign='top'><label style=\"cursor:pointer;\">");
                    string colName = myColumn.ColumnName;
                    string columnValue = myRow[colName].ToString();

                    if (colName.ToLower().Equals("status"))
                    {
                        if (columnValue.ToLower().Equals("overdue"))
                        {
                            myBuilder.Append("<td align='left' valign='top' style='background-color:red; color:white'>");
                        }
                        if (columnValue.ToLower().Equals("current"))
                        {
                            myBuilder.Append("<td align='left' valign='top' style='background-color:#FFA500; color:white'>");
                        }
                        if (columnValue.ToLower().Equals("upcoming"))
                        {
                            myBuilder.Append("<td align='left' valign='top' style='background-color:#008000; color:white'>");
                        }
                    }
                    else
                    {
                        if (colName.ToLower().Equals("priority"))
                        {
                            myBuilder.Append("<td align='right' valign='top'>");
                        }
                        else if (colName.ToLower().Equals("age"))
                        {
                            myBuilder.Append("<td align='right' valign='top'>");
                        }
                        else
                        {
                            myBuilder.Append("<td align='left' valign='top'>");
                        }
                    }

                    string truncatedvalue = (columnValue.Length > 55) ? columnValue.Substring(0, 55) + ".." : columnValue;
                    myBuilder.Append(truncatedvalue);
                    myBuilder.Append("</td>");

                    colcount++;
                }
                myBuilder.Append("</tr>");
                intI += 1;
            }
            //Close tags.
            return myBuilder.Append("</table>").ToString();
        }
    }
}
