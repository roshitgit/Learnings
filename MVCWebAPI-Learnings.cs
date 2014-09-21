/*Use MVC Web API if pages load large datasets/json's. Large means data rows exceeding 5000 records in table.
 * Use Regular MVC controller only for smaller applications which is not data centric.
 * There is restriction to the size of JSON data that can be returned from MVC controller JSONResult Action method witin regular
    MVC controller.
 * Both MVC regular controller & Web API controller can be used within an app.*/

//Code Conventions:
//Regular MVC Controller:
public class ChartsController : Controller
    {
        public JsonResult GetData(<params>)
        {
            try
            {
                var list = chartsRepo.Get(<params>);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ErrorMessage = ex.Message });
            }
        }
  }
  
  
//Web API controller:
public class ChartsController : ApiController
    {
        [System.Web.Http.HttpGet]
        [System.Web.Http.ActionName("GetData")] 
        public List<MonthWiseRiskIssues> GetData(string Id)
        {
            try
            {
                return chartsRepo.GetDataFromDB(Id);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
        }
    
    }
