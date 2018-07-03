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
	public class agenda : System.Web.Services.WebService
    {


		[WebMethod]
        public string getAgendas(int medico, int anestesia, int tempo)
        {
            Conexao cn = new Conexao();
			SqlParameter sqlPar = new SqlParameter();
            ArrayList colPar = new ArrayList();
            StringBuilder strSQL = new StringBuilder();
            DataTable tb;

			sqlPar.DbType = DbType.Int32;
			sqlPar.Value = medico;
            sqlPar.ParameterName = "@Med";
            colPar.Add(sqlPar);
   
			strSQL.Append("SELECT DATA_CONSULTA, MIN(HORA) HoraIni, MAX(HORA) HoraFim FROM AGENDA_HOSPITAL").AppendLine(); 
            strSQL.Append("WHERE MEDICOEXE=@Med AND MEDICO IN(SELECT CODIGO FROM MEDICO WHERE TIPO='I')").AppendLine();
            strSQL.Append("AND STATUS=(SELECT VALOR FROM PARAMETROS_SOFTWARE Where PARAMETRO='glb_str_STATUS_RESERVA')").AppendLine();
            strSQL.Append("AND DATA_CONSULTA>=Cast(dateadd(d, iif(datepart(dw, getdate())=7, 5,").AppendLine(); 
            strSQL.Append("    iif(datepart(dw, getdate())=1, 4, 3)), getdate()) as DATE)").AppendLine();
            strSQL.Append("GROUP BY DATA_CONSULTA").AppendLine();       

			tb = cn.OpenDataSetWithParam(strSQL.ToString(), "Agenda", colPar).Tables[0];

            return JsonConvert.SerializeObject(tb, Formatting.None);
        }

    }
}
