using System;
using System.Text;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using Newtonsoft.Json;

namespace hsm_portal_medico
{
	[WebService(Namespace = "http://www.example.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class cirurgia : System.Web.Services.WebService
    {
		[WebMethod]
		public string getCirurgiaCod(string strCod)
		{
			Conexao cn = new Conexao();
            SqlParameter sqlPar = new SqlParameter();
            ArrayList colPar = new ArrayList();
            StringBuilder strSQL = new StringBuilder();
            DataTable tb;

            sqlPar.DbType = DbType.String;
            sqlPar.Value = strCod;
            sqlPar.ParameterName = "@Cod";
            colPar.Add(sqlPar);

			strSQL.Append("SELECT Codigo, Nome FROM Exame where Codigo=@Cod").AppendLine();

            tb = cn.OpenDataSetWithParam(strSQL.ToString(), "Cirurgia", colPar).Tables[0];

            return JsonConvert.SerializeObject(tb, Formatting.None);			
		}
    }
}
