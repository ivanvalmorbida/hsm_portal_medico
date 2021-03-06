﻿using System;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace hsm_portal_medico
{
    [WebService(Namespace = "http://www.example.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[System.Web.Script.Services.ScriptService()]
	public class paciente : System.Web.Services.WebService
    {
		[WebMethod]
		public string setPacienteCPF(objPaciente obj)
		{
			if (obj.cep!=null) 
				obj.cep = obj.cep.Replace(".", "").Replace("-", "");
			
            Conexao cn = new Conexao();
            SqlParameter sqlPar = new SqlParameter();
			string strSQL;

			List<SqlParameter> colPar = cn.PreencherPar(obj);

			if (obj.codigo == 0)
			{
				colPar = cn.RemoverPar(colPar, "codigo");

				strSQL = "Insert into PACIENTE (cnpjcpf, nome, data_nascim, sexo, rg, est_civil, profissao, " +
    				"nomepai, nomemae, convenio, convenio_plano, nrconvenio, titular, convenio_validade_carteira, " +
    				"cep, bairro, celular, foneresid, email, contato) values (@cpf, @nome, @nascimento, @sexo, @rg, @estadocivil, " +
    				"@profissao, @pai, @mae, @convenio, @plano, @carteirinha, @titular, @validade_cart, @cep, " +
    				"@bairro, @celular, @telefone, @email, @contato)";
			}
			else
			{
				strSQL = "Update PACIENTE set cnpjcpf=@cpf, nome=@nome, data_nascim=@nascimento, sexo=@sexo, " +
					"rg=@rg, est_civil=@estadocivil, profissao=@profissao, nomepai=@pai, nomemae=@mae, " +
					"convenio=@convenio, convenio_plano=@plano, nrconvenio=@carteirinha, titular=@titular, " +
					"convenio_validade_carteira=@validade_cart, cep=@cep, bairro=@bairro, celular=@celular, " +
					"foneresid=@telefone, email=@email, contato=@contato where codigo=@codigo";
			}
			cn.ExecuteWithParam(strSQL, colPar);
			
			if (obj.codigo == 0)
			{
				strSQL = "select max(codigo) from PACIENTE";
				SqlDataReader x = cn.OpenReader(strSQL);
				if (x.Read()) { return x[0].ToString(); } else { return "0"; }
			}
			else{
				return obj.codigo.ToString();
			}
		}
         
		[WebMethod]
		public string getPacienteCPF(string strCPF)
        {
			Conexao cn = new Conexao();
            SqlParameter sqlPar = new SqlParameter();
            ArrayList colPar = new ArrayList();
            string strSQL="";
            DataTable tb;

			sqlPar.DbType = DbType.String;
            sqlPar.Value = strCPF;
            sqlPar.ParameterName = "@CPF";
            colPar.Add(sqlPar);

			strSQL="SELECT codigo, CNPJCPF cpf, Isnull(nome,'') nome, DATA_NASCIM nascimento, " +
			    "Isnull(sexo,'') sexo, rg, Isnull(EST_CIVIL,0) estadocivil, Isnull(profissao,0) profissao, " +
			    "Isnull(NOMEPAI,'') pai, Isnull(NOMEMAE,'') mae, Isnull(convenio,0) convenio, " +
				"Isnull(CONVENIO_PLANO,'') plano, Isnull(NRCONVENIO,'') carteirinha, Isnull(TITULAR,'') titular, " +
				"CONVENIO_VALIDADE_CARTEIRA validade_cart, Isnull(CEP,'') cep, Isnull(BAIRRO,'') bairro, " +
				"Isnull(Celular,'') celular, Isnull(FONERESID,'') telefone, Isnull(EMAIL,'') email, Isnull(contato,'') contato, " +
				"(SELECT Nome FROM TABPROFISSOES where codigo=PACIENTE.profissao) as profissao_, "+
				"(SELECT Nome FROM BAIRRO Where Codigo=PACIENTE.BAIRRO) as bairro_ "+
				"From PACIENTE where CNPJCPF=@CPF";
            
            tb = cn.OpenDataSetWithParam(strSQL.ToString(), "Paciente", colPar).Tables[0];

			return JsonConvert.SerializeObject(tb, Newtonsoft.Json.Formatting.None);
        }

		[WebMethod]
		public string getEstadoCivil()
		{
			Conexao cn = new Conexao();
            DataTable tb;

			tb = cn.OpenDataSet("SELECT Codigo as value, Nome as text FROM ESTADO_CIVIL Order by NOME", "EstadoCivil").Tables[0];

			return JsonConvert.SerializeObject(tb, Newtonsoft.Json.Formatting.None);
		}

    	[WebMethod]
        public string getProfissoes()
        {
            Conexao cn = new Conexao();
            DataTable tb;

    		tb = cn.OpenDataSet("SELECT Codigo as value, Nome as text FROM TABPROFISSOES Order by NOME", "Profissoes").Tables[0];

			return JsonConvert.SerializeObject(tb, Newtonsoft.Json.Formatting.None);
        }

		[WebMethod]
        public string getConvenio()
        {
            Conexao cn = new Conexao();
            DataTable tb;

			tb = cn.OpenDataSet("SELECT Codigo as value, Nome as text FROM CONVENIO Order by NOME", "Convenios").Tables[0];

			return JsonConvert.SerializeObject(tb, Newtonsoft.Json.Formatting.None);
        }

		[WebMethod]
        public string getBairro()
        {
            Conexao cn = new Conexao();
            DataTable tb;

			tb = cn.OpenDataSet("SELECT Codigo as value, Nome as text FROM BAIRRO Where NOME<>'' Order by NOME", "Bairros").Tables[0];

			return JsonConvert.SerializeObject(tb, Newtonsoft.Json.Formatting.None);
        }

		[WebMethod]
        public string getCEP(string strCep)
        {
			strCep = strCep.Substring(0, 5);
            Conexao cn = new Conexao();
            DataTable tb;

			tb = cn.OpenDataSet("SELECT Cidade, UF, DDD FROM CEP Where Codigo='"+strCep+"'", "Bairros").Tables[0];

			return JsonConvert.SerializeObject(tb, Newtonsoft.Json.Formatting.None);
        }
	}

	public class objPaciente
    {
        public int codigo { get; set; }
        public string cpf { get; set; }
        public string nome { get; set; }
        public DateTime nascimento { get; set; }
        public string sexo { get; set; }
        public string rg { get; set; }
        public int estadocivil { get; set; }
        public int profissao { get; set; }
        public string pai { get; set; }
        public string mae { get; set; }
        public int convenio { get; set; }
        public string plano { get; set; }
        public string carteirinha { get; set; }
        public string titular { get; set; }
        public DateTime validade_cart { get; set; }
        public string cep { get; set; }
        public string bairro { get; set; }
        public string contato { get; set; }
        public string celular { get; set; }
        public string telefone { get; set; }
        public string email { get; set; }
    }
}
