http://stackoverflow.com/questions/2188954/see-if-user-is-part-of-active-directory-group-in-c-sharp-asp-net

*** works on localhost but does not work once deployed on server
//var CurrentUser = User.Identity.Name.JustGetSoeId(); //JustGetSoeId is an extension method to get the ID
                //var CurrentUser = WindowsIdentity.GetCurrent().Name.JustGetSoeId();
                //var CurrentUser = HttpContext.Session["<ID>"].ToString();

                //var wi = new WindowsIdentity(CurrentUser);
                //var wp = new WindowsPrincipal(wi);
                //var ADAccount = ConfigurationManager.AppSettings["ADAccount"]; //DOMAIN\USERNAME from web.config
                //bool inRole = wp.IsInRole(@ADAccount);
