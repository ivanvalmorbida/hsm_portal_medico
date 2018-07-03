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

		[WebMethod]	
		public string getCirurgiaNomCod(string strX)
		{
			Conexao cn = new Conexao();
            StringBuilder strSQL = new StringBuilder();
            DataTable tb;

			strSQL.Append("SELECT nome, Codigo as value, Codigo+' - '+Nome as text FROM Exame").AppendLine();
            strSQL.Append("where nome like '%"+strX+"%' or codigo like '%"+strX+"%' order by nome").AppendLine();

            tb = cn.OpenDataSet(strSQL.ToString(), "Cirurgia").Tables[0];

            return JsonConvert.SerializeObject(tb, Formatting.None);			
		}

		[WebMethod]	
		public string getCirurgias()
		{
			Conexao cn = new Conexao();
            StringBuilder strSQL = new StringBuilder();
            DataTable tb;

			strSQL.Append("SELECT Codigo as value, Nome as text FROM Exame order by text").AppendLine();

            tb = cn.OpenDataSet(strSQL.ToString(), "Cirurgia").Tables[0];

            return JsonConvert.SerializeObject(tb, Formatting.None);			
		}
	}
}