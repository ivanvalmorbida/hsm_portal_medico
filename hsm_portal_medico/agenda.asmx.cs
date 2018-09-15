using System;
using System.Text;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using Newtonsoft.Json;
using System.IO;
using System.Drawing;

namespace hsm_portal_medico
{
	[WebService(Namespace = "http://www.example.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService()]
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

			sqlPar = new SqlParameter();
			sqlPar.DbType = DbType.Int32;
			sqlPar.Value = anestesia;
			sqlPar.ParameterName = "@Anestesia";
            colPar.Add(sqlPar);
            
			sqlPar = new SqlParameter();
            sqlPar.DbType = DbType.Int32;
            sqlPar.Value = tempo;
            sqlPar.ParameterName = "@tempo";
            colPar.Add(sqlPar);

			strSQL.Append("SELECT * from (").AppendLine();
			strSQL.Append("SELECT DATA_CONSULTA,").AppendLine();
            strSQL.Append("cast(cast(YEAR(data_consulta) as VARCHAR(4)) + '-' + cast(month(data_consulta) as VARCHAR(2))").AppendLine();
            strSQL.Append("+ '-' + cast(day(data_consulta) as VARCHAR(2)) + ' ' +").AppendLine();
			strSQL.Append("iif(MIN(HORA) >= 1000, left(cast(MIN(HORA) as VARCHAR(4)), 2) + ':' + RIGHT(cast(MIN(HORA) as VARCHAR(4)), 2),").AppendLine();
			strSQL.Append("left(cast(MIN(HORA) as VARCHAR(4)), 1) + ':' + RIGHT(cast(MIN(HORA) as VARCHAR(4)), 2)) as DATETIME) as HoraIni,").AppendLine();
			strSQL.Append("cast(cast(YEAR(data_consulta) as VARCHAR(4)) + '-' + cast(month(data_consulta) as VARCHAR(2))").AppendLine();
            strSQL.Append("+ '-' + cast(day(data_consulta) as VARCHAR(2)) + ' ' +").AppendLine();
			strSQL.Append("iif(MAX(HORA) >= 1000, left(cast(MAX(HORA) as VARCHAR(4)), 2) + ':' + RIGHT(cast(MAX(HORA) as VARCHAR(4)), 2),").AppendLine();
			strSQL.Append("left(cast(MAX(HORA) as VARCHAR(4)), 1) + ':' + RIGHT(cast(MAX(HORA) as VARCHAR(4)), 2)) as DATETIME) as HoraFim").AppendLine();
			strSQL.Append("FROM AGENDA_HOSPITAL").AppendLine(); 
			strSQL.Append("WHERE MEDICOEXE=@Med AND MEDICO IN(SELECT CODIGO FROM MEDICO WHERE TIPO='I' and TipoAnestesia=@Anestesia)").AppendLine();
            strSQL.Append("AND STATUS=(SELECT VALOR FROM PARAMETROS_SOFTWARE Where PARAMETRO='glb_str_STATUS_RESERVADO')").AppendLine();
            strSQL.Append("AND DATA_CONSULTA>=Cast(dateadd(d, iif(datepart(dw, getdate())=7, 5,").AppendLine(); 
            strSQL.Append("iif(datepart(dw, getdate())=1, 4, 3)), getdate()) as DATE)").AppendLine();
			strSQL.Append("GROUP BY DATA_CONSULTA) as d where datediff (minute, HoraIni, HoraFim)>=@tempo").AppendLine();
         
			tb = cn.OpenDataSetWithParam(strSQL.ToString(), "Agenda", colPar).Tables[0];

            return JsonConvert.SerializeObject(tb, Formatting.None);
        }
        
        [WebMethod]
        public string SaveFile(FileData data)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(data.Data)))
                {
					FileStream file = new FileStream("/home/ivan/uploads/" + data.Name, FileMode.Create, FileAccess.Write);
                    ms.WriteTo(file);
                    file.Close();
                    ms.Close();

					return "OK";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public class FileData
        {
            public string Data { get; set; }
            public string ContentType { get; set; }
            public string Name { get; set; }
        }
    }
}


//UPDATE AGENDA_HOSPITAL set medicoexe=27, status='RES' 
//where DATA_CONSULTA > getdate() + 5 and datepart(dw, DATA_CONSULTA)= 3
