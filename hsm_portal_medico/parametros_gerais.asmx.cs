using System.Web.Services;
using System.Data.SqlClient;

namespace hsm_portal_medico
{
    [WebService(Namespace = "http://www.example.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService()]
    public class parametros_gerais : System.Web.Services.WebService
    {

        [WebMethod]
        public string getParticular()
        {
            Conexao cn = new Conexao();
            string strSQL;

            strSQL = "SELECT COD_PARTICULAR FROM PARAMETROS_GERAIS";
            SqlDataReader x = cn.OpenReader(strSQL);
            if (x.Read()) { return x[0].ToString(); } else { return "0"; }
        }
    }
}
