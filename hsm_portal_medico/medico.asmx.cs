using System;
using System.Web;
using System.Web.Services;

namespace hsm_portal_medico
{
	[WebService(Namespace = "http://www.example.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[System.Web.Script.Services.ScriptService()]
	public class medico : System.Web.Services.WebService
	{
        
		[WebMethod]
		public string getMedico()
		{
			return System.Web.HttpContext.Current.User.Identity.Name.ToString();
		}
	}
}
