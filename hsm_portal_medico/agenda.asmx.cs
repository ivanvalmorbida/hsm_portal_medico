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
        public string getUsuarioDados()
        {
            Conexao cn = new Conexao();
			SqlParameter sqlPar = new SqlParameter();
            ArrayList colPar = new ArrayList();
            StringBuilder strSQL = new StringBuilder();
            DataTable tb;

			sqlPar.DbType = DbType.Int32;
			sqlPar.Value = System.Web.HttpContext.Current.User.Identity.Name.ToString();
            sqlPar.ParameterName = "@Med";
            colPar.Add(sqlPar);

			strSQL.Append("SELECT nome, crm, ufcrm, Case when (select count(*) from AGENDA_HOSPITAL where NOMEPACI='HORARIO RESERVADO'" +
                " and DATA_CONSULTA>GETDATE() and MEDICOEXE=MEDICO.codigo)>0 then '1' else '0' end as reserva from MEDICO").AppendLine(); 
			strSQL.Append("WHERE codigo=@Med").AppendLine(); 
         
			tb = cn.OpenDataSetWithParam(strSQL.ToString(), "Medico", colPar).Tables[0];

            return JsonConvert.SerializeObject(tb, Formatting.None);
        }

		[WebMethod]
        public string getAgendamentos()
        {
            Conexao cn = new Conexao();
			SqlParameter sqlPar = new SqlParameter();
            ArrayList colPar = new ArrayList();
            StringBuilder strSQL = new StringBuilder();
            DataTable tb;

			sqlPar.DbType = DbType.Int32;
			sqlPar.Value = System.Web.HttpContext.Current.User.Identity.Name.ToString();
            sqlPar.ParameterName = "@Med";
            colPar.Add(sqlPar);

			strSQL.Append("SELECT *, CONVERT(varchar,HoraIni,103)+' '+CONVERT(varchar,HoraIni,108) as data_hora").AppendLine(); 
			strSQL.Append("from(SELECT cast(cast(YEAR(data_consulta) as VARCHAR(4)) + RIGHT('0' + cast(month(data_consulta) as VARCHAR(2)), 2) +").AppendLine(); 
			strSQL.Append("RIGHT('0' + cast(day(data_consulta) as VARCHAR(2)), 2) + ' ' +").AppendLine(); 
			strSQL.Append("iif(HORA >= 1000, left(cast(HORA as VARCHAR(4)), 2) + ':' + RIGHT(cast(HORA as VARCHAR(4)), 2),").AppendLine(); 
			strSQL.Append("left(cast(HORA as VARCHAR(4)), 1) + ':' + RIGHT(cast(HORA as VARCHAR(4)), 2)) as DATETIME) as HoraIni,").AppendLine(); 
            strSQL.Append("NOMEPACI as paciente, e1.NOME as procedimento1, e2.NOME as procedimento2,").AppendLine();
            strSQL.Append("e3.NOME as procedimento3, e4.NOME as procedimento4, e5.NOME as procedimento5").AppendLine();
            strSQL.Append("FROM AGENDA_HOSPITAL as a").AppendLine();
            strSQL.Append("INNER JOIN EXAME as e1 on e1.CODIGO=a.EXAME1").AppendLine();
            strSQL.Append("LEFT JOIN EXAME as e2 on e2.CODIGO=a.EXAME2").AppendLine();
            strSQL.Append("LEFT JOIN EXAME as e3 on e3.CODIGO=a.EXAME3").AppendLine();
            strSQL.Append("LEFT JOIN EXAME as e4 on e4.CODIGO=a.EXAME4").AppendLine();
            strSQL.Append("LEFT JOIN EXAME as e5 on e5.CODIGO=a.EXAME5").AppendLine();
			strSQL.Append("WHERE a.MEDICOEXE=@Med AND a.DATA_CONSULTA>=getdate() AND isnull(a.EXAME1,'')<>'') as d").AppendLine(); 
         
			tb = cn.OpenDataSetWithParam(strSQL.ToString(), "Agenda", colPar).Tables[0];

            return JsonConvert.SerializeObject(tb, Formatting.None);
        }

		[WebMethod]
        public string getAgendas(int anestesia, int tempo, string reserva)
        {
            Conexao cn = new Conexao();
			SqlParameter sqlPar = new SqlParameter();
            ArrayList colPar = new ArrayList();
            StringBuilder strSQL = new StringBuilder();
            DataTable tb;

			sqlPar.DbType = DbType.Int32;
			sqlPar.Value = System.Web.HttpContext.Current.User.Identity.Name.ToString();
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

			strSQL.Append("SELECT *, CONVERT(varchar,HoraIni,103)+' '+CONVERT(varchar,HoraIni,108) as HoraIniF from (").AppendLine();
			strSQL.Append("SELECT MEDICO as Sala, Isnull(MEDICOEXE,0) as Medico, DATA_CONSULTA,").AppendLine();
			strSQL.Append("cast(cast(YEAR(data_consulta) as VARCHAR(4)) + RIGHT('0'+cast(month(data_consulta) as VARCHAR(2)), 2)+").AppendLine();
            strSQL.Append("RIGHT('0' + cast(day(data_consulta) as VARCHAR(2)), 2) + ' ' +").AppendLine();
			strSQL.Append("iif(MIN(HORA) >= 1000, left(cast(MIN(HORA) as VARCHAR(4)), 2) + ':' + RIGHT(cast(MIN(HORA) as VARCHAR(4)), 2),").AppendLine();
			strSQL.Append("left(cast(MIN(HORA) as VARCHAR(4)), 1) + ':' + RIGHT(cast(MIN(HORA) as VARCHAR(4)), 2)) as DATETIME) as HoraIni,").AppendLine();
			strSQL.Append("cast(cast(YEAR(data_consulta) as VARCHAR(4)) + RIGHT('0'+cast(month(data_consulta) as VARCHAR(2)), 2)+").AppendLine();
            strSQL.Append("RIGHT('0' + cast(day(data_consulta) as VARCHAR(2)), 2) + ' ' +").AppendLine();
			strSQL.Append("iif(MAX(HORA) >= 1000, left(cast(MAX(HORA) as VARCHAR(4)), 2) + ':' + RIGHT(cast(MAX(HORA) as VARCHAR(4)), 2),").AppendLine();
			strSQL.Append("left(cast(MAX(HORA) as VARCHAR(4)), 1) + ':' + RIGHT(cast(MAX(HORA) as VARCHAR(4)), 2)) as DATETIME) as HoraFim").AppendLine();
			strSQL.Append("FROM AGENDA_HOSPITAL").AppendLine(); 
			strSQL.Append("WHERE MEDICO IN(SELECT CODIGO FROM MEDICO WHERE TIPO='I' and TipoAnestesia=@Anestesia)").AppendLine();
            strSQL.Append("AND ((MEDICOEXE=@Med and STATUS=(SELECT VALOR FROM PARAMETROS_SOFTWARE Where PARAMETRO='glb_str_STATUS_RESERVADO') and NOMEPACI='HORARIO RESERVADO')").AppendLine();

            if (reserva=="0") {
                strSQL.Append("OR (Isnull(MEDICOEXE, 0)=0 and Isnull(STATUS, '')='' and Isnull(NOMEPACI, '')='')").AppendLine();
            }

            strSQL.Append(")AND DATA_CONSULTA>=Cast(dateadd(d, iif(datepart(dw, getdate())=7, 3,").AppendLine(); 
            strSQL.Append("iif(datepart(dw, getdate())=1, 2, 1)), getdate()) as DATE)").AppendLine();
			strSQL.Append("GROUP BY Medico, DATA_CONSULTA, Isnull(MEDICOEXE,0)) as d where datediff (minute, HoraIni, HoraFim)>=@tempo").AppendLine();
			strSQL.Append("Order by Medico desc, HoraIni").AppendLine(); 
         
			tb = cn.OpenDataSetWithParam(strSQL.ToString(), "Agenda", colPar).Tables[0];

            return JsonConvert.SerializeObject(tb, Formatting.None);
        }
        
		[WebMethod]
		public string Agendar(objAgendar obj)//int sala, int medico, int paciente, int tempo, DateTime horai)
        {
            Conexao cn = new Conexao();
            SqlParameter sqlPar = new SqlParameter();
            ArrayList colPar = new ArrayList();
            StringBuilder strSQL = new StringBuilder();
   
            sqlPar = new SqlParameter();
            sqlPar.DbType = DbType.Int32;
            sqlPar.Value = obj.sala;
            sqlPar.ParameterName = "@sala";
            colPar.Add(sqlPar);

            sqlPar = new SqlParameter();
            sqlPar.DbType = DbType.Int32;
            sqlPar.Value = System.Web.HttpContext.Current.User.Identity.Name.ToString();
            sqlPar.ParameterName = "@medico";
            colPar.Add(sqlPar);

            sqlPar = new SqlParameter();
            sqlPar.DbType = DbType.Int32;
            sqlPar.Value = obj.paciente;
			sqlPar.ParameterName = "@paciente";
            colPar.Add(sqlPar);

            DateTime data = DateTime.Now;

            sqlPar = new SqlParameter();
            sqlPar.DbType = DbType.Date;
            sqlPar.Value = obj.horaini.Date;
            sqlPar.ParameterName = "@data";
            colPar.Add(sqlPar);

            string hora = obj.horaini.Hour.ToString() + obj.horaini.Minute.ToString("00");

            sqlPar = new SqlParameter();
            sqlPar.DbType = DbType.Int32;
            sqlPar.Value = hora;
            sqlPar.ParameterName = "@horai";
            colPar.Add(sqlPar);

            obj.horaini = obj.horaini.AddMinutes(obj.tempo);
            hora = obj.horaini.Hour.ToString() + obj.horaini.Minute.ToString("00");

            sqlPar = new SqlParameter();
            sqlPar.DbType = DbType.Int32;
            sqlPar.Value = hora;
            sqlPar.ParameterName = "@horaf";
            colPar.Add(sqlPar);
            
            sqlPar = new SqlParameter();
            sqlPar.DbType = DbType.String;
            sqlPar.Value = obj.guia;
            sqlPar.ParameterName = "@guia";
            colPar.Add(sqlPar);

            sqlPar = new SqlParameter();
            sqlPar.DbType = DbType.String;
            sqlPar.Value = obj.autorizacao;
            sqlPar.ParameterName = "@autorizacao";
            colPar.Add(sqlPar);

            sqlPar = new SqlParameter();
            sqlPar.DbType = DbType.Date;
            sqlPar.Value = obj.data_autoriza.Date;
            sqlPar.ParameterName = "@data_autoriza";
            colPar.Add(sqlPar);

            sqlPar = new SqlParameter();
            sqlPar.DbType = DbType.Date;
            sqlPar.Value = obj.valid_autoriza.Date;
			sqlPar.ParameterName = "@valid_autoriza";
            colPar.Add(sqlPar);
            
            strSQL.Append("UPDATE AGENDA_HOSPITAL set medicoexe=@medico, paciente=@paciente,").AppendLine(); 
            strSQL.Append("NOMEPACI=(select nome from paciente where codigo=@paciente),").AppendLine();
            strSQL.Append("Convenio=(select convenio from paciente where codigo=@paciente),").AppendLine();
            strSQL.Append("REQUISICAO=@guia, AUTORIZACAO=@autorizacao, DATA_AUTORIZACAO=@data_autoriza,").AppendLine();
            strSQL.Append("DATA_VALIDADE_AUTORIZACAO=@valid_autoriza").AppendLine();

            if(obj.procedimentos.Count>0){
                sqlPar = new SqlParameter();
                sqlPar.DbType = DbType.String;
				sqlPar.Value = obj.procedimentos[0].ToString();
                sqlPar.ParameterName = "@EXAME1";
                colPar.Add(sqlPar);

                strSQL.Append(",EXAME1=@EXAME1").AppendLine();
            }
			if (obj.procedimentos.Count > 1)
            {
                sqlPar = new SqlParameter();
                sqlPar.DbType = DbType.String;
                sqlPar.Value = obj.procedimentos[1].ToString();
                sqlPar.ParameterName = "@EXAME2";
                colPar.Add(sqlPar);

                strSQL.Append(",EXAME2=@EXAME2").AppendLine();
            }
			if (obj.procedimentos.Count > 2)
            {
                sqlPar = new SqlParameter();
                sqlPar.DbType = DbType.String;
                sqlPar.Value = obj.procedimentos[2].ToString();
                sqlPar.ParameterName = "@EXAME3";
                colPar.Add(sqlPar);

                strSQL.Append(",EXAME3=@EXAME3").AppendLine();
            }            
			if (obj.procedimentos.Count > 3)
            {
                sqlPar = new SqlParameter();
                sqlPar.DbType = DbType.String;
                sqlPar.Value = obj.procedimentos[3].ToString();
                sqlPar.ParameterName = "@EXAME4";
                colPar.Add(sqlPar);

                strSQL.Append(",EXAME4=@EXAME4").AppendLine();
            }            
			if (obj.procedimentos.Count > 4)
            {
                sqlPar = new SqlParameter();
                sqlPar.DbType = DbType.String;
                sqlPar.Value = obj.procedimentos[4].ToString();
                sqlPar.ParameterName = "@EXAME5";
                colPar.Add(sqlPar);

                strSQL.Append(",EXAME5=@EXAME5").AppendLine();
            }            

			strSQL.Append("where DATA_CONSULTA=@data and medico=@sala and hora>=@horai and hora<@horaf").AppendLine();
			cn.ExecuteWithParamOld(strSQL.ToString(), colPar);
            
            return "Ok";
        }

		public class objAgendar
		{
			public int sala { get; set; }
			public int paciente { get; set; }
			public int tempo { get; set; }
			public DateTime horaini { get; set; }
			public string guia { get; set; }
			public string autorizacao { get; set; }
            public DateTime data_autoriza { get; set; }
			public DateTime valid_autoriza { get; set; }
            public ArrayList procedimentos { get; set; }
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