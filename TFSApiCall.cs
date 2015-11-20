using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using System.Security.Principal;
using System.Net.Http.Headers;

namespace TFSApiIntegration
{
    public class WorkItemDetails
    {
        public string id;
        public string rev;
        public IDictionary<string, string> fields;
        public string Url;
    }

    public class TFSPostData
    {
        public string query { get; set; }
        //public string value { get; set; }
    }

    public partial class TFSApiTester : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GetWorkItem();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="WiId"></param>
        /// <returns></returns>
        private void GetWorkItem(string username = null, string password = null, int? WiId = null)
        {
            //var URI = new Uri("http://tfs.wlb2.eur.nsroot.net:8080/tfs/Saturn/CAMPDev.AMTool/_apis/wit/wiql?api-version=1.0");
            //string Url = "https://ssgsonline.visualstudio.com/defaultcollection/_apis/wit/workitems?id=1&api-version=1.0";
            string Url = "http://tfs.wlb2.eur.nsroot.net:8080/tfs/Saturn/CAMPDev.AMTool/_apis/wit/wiql?api-version=1.0";

            //string Url = "http://tfs.wlb2.eur.nsroot.net:8080/tfs/Saturn/CAMPDev.AMTool/_apis/wit/workitems?id=1&api-version=1.0";
            //string Url = "http://tfs.wlb2.eur.nsroot.net:8080/tfs/Saturn/CAMPDev.AMTool/_apis/wit/workitems?id=27585&api-version=1.0";

            string shortVersion = "Select * FROM WorkItems WHERE [System.TeamProject] = 'CAMPDev.AMTool' AND [System.IterationPath] = 'CAMPDev.AMTool \\Release 10 - December 04th 2015' " +
                                            " AND (State = 'Active') AND [System.WorkItemType] = 'User Story' order by Title";

            string query = "Select [System.Id],[System.Title],[System.CreatedBy],[System.State] FROM WorkItems Where [System.WorkItemType] = 'Task' AND [State] <> 'Closed'";

            string workItemLinks = "SELECT [System.WorkItemType], [System.Title],[System.State],[Microsoft.VSTS.Scheduling.StoryPoints],[System.IterationPath], " +
                        " [System.Tags] FROM  WorkItemLinks WHERE (Source.[System.WorkItemType] IN GROUP 'Microsoft.RequirementCategory' ) " +
                        " AND Source.[System.IterationPath] UNDER 'CAMPDev.AMTool' AND ((Source.[System.AreaPath] UNDER 'CAMPDev.AMTool\\Admin Control Panel' " +
                       " OR Source.[System.AreaPath] UNDER 'CAMPDev.AMTool\\Automated ETL process' OR Source.[System.AreaPath] UNDER 'CAMPDev.AMTool\\Back End' " +
                        " OR Source.[System.AreaPath] UNDER 'CAMPDev.AMTool\\Backlog' OR Source.[System.AreaPath] UNDER 'CAMPDev.AMTool\\Defects' " +
                        " OR Source.[System.AreaPath] UNDER 'CAMPDev.AMTool\\Dev tasks' OR Source.[System.AreaPath] UNDER 'CAMPDev.AMTool\\Generic Data load - UI' " +
                        " OR Source.[System.AreaPath] UNDER 'CAMPDev.AMTool\\Knife Onboarding' OR Source.[System.AreaPath] UNDER 'CAMPDev.AMTool\\New Knife Analysis' " +
                        " OR Source.[System.AreaPath] UNDER 'CAMPDev.AMTool\\Quality Assurance' OR Source.[System.AreaPath] UNDER 'CAMPDev.AMTool\\Reporting' " +
                        " OR Source.[System.AreaPath] UNDER 'CAMPDev.AMTool\\Test Case Prep' OR Source.[System.AreaPath] UNDER 'CAMPDev.AMTool\\UI Enhancements' " +
                        " OR Source.[System.AreaPath] UNDER 'CAMPDev.AMTool\\Upcoming User Stories and Analysis')) AND (Target.[System.WorkItemType] " +
                    " IN GROUP 'Microsoft.RequirementCategory' ) AND Target.[System.IterationPath] UNDER 'CAMPDev.AMTool' AND Target.[System.State] IN ('Active','Resolved') " +
                    " AND ((Target.[System.AreaPath] UNDER 'CAMPDev.AMTool\\Admin Control Panel' OR Target.[System.AreaPath] UNDER 'CAMPDev.AMTool\\Automated ETL process' " +
                      "  OR Target.[System.AreaPath] UNDER 'CAMPDev.AMTool\\Back End' OR Target.[System.AreaPath] UNDER 'CAMPDev.AMTool\\Backlog' " +
                       " OR Target.[System.AreaPath] UNDER 'CAMPDev.AMTool\\Defects' OR Target.[System.AreaPath] UNDER 'CAMPDev.AMTool\\Dev tasks' " +
                       " OR Target.[System.AreaPath] UNDER 'CAMPDev.AMTool\\Generic Data load - UI' OR Target.[System.AreaPath] UNDER 'CAMPDev.AMTool\\Knife Onboarding' " +
                       " OR Target.[System.AreaPath] UNDER 'CAMPDev.AMTool\\New Knife Analysis' OR Target.[System.AreaPath] UNDER 'CAMPDev.AMTool\\Quality Assurance' " +
                       " OR Target.[System.AreaPath] UNDER 'CAMPDev.AMTool\\Reporting' OR Target.[System.AreaPath] UNDER 'CAMPDev.AMTool\\Test Case Prep' " +
                       " OR Target.[System.AreaPath] UNDER 'CAMPDev.AMTool\\UI Enhancements' OR Target.[System.AreaPath] UNDER 'CAMPDev.AMTool\\Upcoming User Stories and Analysis')) " +
                    " AND [System.Links.LinkType] = 'System.LinkTypes.Hierarchy-Forward' ORDER BY [Microsoft.VSTS.Common.StackRank] ASC,[System.Id] ASC MODE (Recursive, ReturnMatchingChildren)";

            #region Using HttpClient
            using (var handler = new HttpClientHandler { UseDefaultCredentials = true }) //enable Integrated Windows Authentication (NTLM) on HttpClientHandler 
            using (HttpClient client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
                //                      System.Text.ASCIIEncoding.ASCII.GetBytes(
                //                      string.Format("{0}:{1}", username, password))));

                //HttpResponseMessage response = client.PostAsync("tfs/Saturn/CAMPDev.AMTool/_apis/wit/wiql?api-version=1.0",
                //    new StringContent(req, Encoding.Default, "application/json")).Result;

                //if (!response.IsSuccessStatusCode)
                //{

                //}
                #region "Using FormUrlEncodedContent"
                //var URI = new Uri("http://tfs.wlb2.eur.nsroot.net:8080/tfs/Saturn/CAMPDev.AMTool/_apis/wit/wiql?api-version=1.0");

                //using (var client = new HttpClient())
                //{

                //    var values = new Dictionary<string, string>
                //    {
                //       { "query", "Select [System.WorkItemType],[System.Title],[System.State],[Microsoft.VSTS.Scheduling.Effort],[System.IterationPath] FROM WorkItemLinks WHERE Source.[System.WorkItemType] IN GROUP 'Microsoft.RequirementCategory' AND Target.[System.WorkItemType] IN GROUP 'Microsoft.RequirementCategory' AND Target.[System.State] IN ('New','Approved','Committed') AND [System.Links.LinkType] = 'System.LinkTypes.Hierarchy-Forward' ORDER BY [Microsoft.VSTS.Common.BacklogPriority] ASC,[System.Id] ASC MODE (Recursive, ReturnMatchingChildren)" }
                //    };

                //    var content = new FormUrlEncodedContent(values);

                //    var response = await client.PostAsync(URI, content);

                //    var responseString = await response.Content.ReadAsStringAsync();

                //}
                #endregion
                #region GET Call
                //using (HttpResponseMessage response = client.GetAsync(Url).Result)
                //{
                //    response.EnsureSuccessStatusCode();
                //    string responseBody = await response.Content.ReadAsStringAsync();
                //    WorkItemDetails wiDetails = JsonConvert.DeserializeObject<WorkItemDetails>(responseBody);
                //    //Console.WriteLine("Work Item ID: \t" + wiDetails.id);
                //    //foreach (KeyValuePair<string, string> fld in wiDetails.fields)
                //    //{
                //    //    Console.WriteLine(fld.Key + ":\t" + fld.Value);
                //    //}
                //}
                #endregion
               
            }
            #endregion

            #region "Using RESTSHARP"
            //var restClient = new RestClient("http://tfs.wlb2.eur.nsroot.net:8080/");
            ////restClient.Authenticator = new HttpBasicAuthenticator(
            //var restRequest = new RestRequest("tfs/Saturn/CAMPDev.AMTool/_apis/wit/wiql?api-version=1.0", Method.POST);
            //restRequest.RequestFormat = DataFormat.Json;
            //restRequest.AddBody(new TFSPostData
            //{
            //    query = shortVersion
            //});
            //// execute the request
            //IRestResponse response = restClient.Execute(restRequest);
            //var content = response.Content; // raw content as string
            #endregion

            #region "Using WebClient"
            var data = JsonConvert.SerializeObject(new
            {
                query = shortVersion
            });

            using (var webClient = new WebClient { UseDefaultCredentials = true })
            {
                webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json; charset=utf-8");
                byte[] dataBytes = webClient.UploadData(Url, "POST", Encoding.UTF8.GetBytes(data));
            }

            //using (WebClient client = new WebClient())
            //{
            //    var URI = new Uri("http://tfs.wlb2.eur.nsroot.net:8080/tfs/Saturn/CAMPDev.AMTool/_apis/wit/wiql?api-version=1.0");

            //    //client.Headers[HttpRequestHeader.Accept] = "application/json";          //specify what format you want to get back
            //    //client.Headers[HttpRequestHeader.ContentType] = "application/json";     //specify what format you are sending.
            //    //client.Encoding = Encoding.UTF8;                                        // only reqd for uploading and downloading strings.
            //    client.Credentials = CredentialCache.DefaultNetworkCredentials;

            //    //bypass certificate error
            //    //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            //    //var dataToSend = JsonConvert.SerializeObject("field1=value1");
            //    //string returnedData = client.UploadString(URI, "POST", dataToSend);

            //    //System.Text.Encoding.ASCII.GetBytes("field1=value1&amp;field2=value2")

            //    string query = "Select [System.Id],[System.Title],[System.CreatedBy],[System.State] " +
            //    "FROM WorkItems Where [System.WorkItemType] = 'Task' AND [State] <> 'Closed' ";

            //    NameValueCollection reqparm = new NameValueCollection();
            //    reqparm.Add("query", query);

            //    byte[] responsebytes = client.UploadValues(URI, "POST", reqparm);
            //    string responsebody = Encoding.UTF8.GetString(responsebytes);

            //}
            #endregion

            #region "Using HttpWebRequest"
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URI);
            //request.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
            //request.Method = "POST";

            //var data = Encoding.ASCII.GetBytes(postData);

            //using (var stream = request.GetRequestStream())
            //{
            //    stream.Write(data, 0, data.Length);
            //}

            //var response = (HttpWebResponse)request.GetResponse();

            //var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            #endregion
        }
    }
}
