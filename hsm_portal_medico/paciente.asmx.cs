using System;
using System.Xml;
using System.Text;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using Newtonsoft.Json;

namespace hsm_portal_medico
{
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
		public string celular { get; set; }
		public string telefone { get; set; }
		public string email { get; set; }
    }

	[WebService(Namespace = "http://www.example.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class paciente : System.Web.Services.WebService
    {
		[WebMethod]
		public string setPacienteCPF(objPaciente obj)
		{
            Conexao cn = new Conexao();
            SqlParameter sqlPar = new SqlParameter();
            ArrayList colPar = new ArrayList();
            string strSQL ="";

			sqlPar.DbType = DbType.String;
            sqlPar.Value = obj.bairro;
			sqlPar.ParameterName = "@bairro";
            colPar.Add(sqlPar);

            sqlPar.DbType = DbType.String;
            sqlPar.Value = obj.carteirinha;
			sqlPar.ParameterName = "@carteirinha";
            colPar.Add(sqlPar);

            sqlPar.DbType = DbType.String;
            sqlPar.Value = obj.celular;
			sqlPar.ParameterName = "@celular";
            colPar.Add(sqlPar);

            sqlPar.DbType = DbType.String;
            sqlPar.Value = obj.cep;
			sqlPar.ParameterName = "@cep";
            colPar.Add(sqlPar);

			sqlPar.DbType = DbType.Int32;
            sqlPar.Value = obj.codigo;
			sqlPar.ParameterName = "@codigo";
            colPar.Add(sqlPar);

            sqlPar.DbType = DbType.Int32;
            sqlPar.Value = obj.convenio;
			sqlPar.ParameterName = "@convenio";
            colPar.Add(sqlPar);
            
			sqlPar.DbType = DbType.String;
			sqlPar.Value = obj.cpf;
			sqlPar.ParameterName = "@cpf";
            colPar.Add(sqlPar);

            sqlPar.DbType = DbType.String;
            sqlPar.Value = obj.email;
			sqlPar.ParameterName = "@email";
			colPar.Add(sqlPar);

            sqlPar.DbType = DbType.Int32;
            sqlPar.Value = obj.estadocivil;
			sqlPar.ParameterName = "@estadocivil";
            colPar.Add(sqlPar);

			sqlPar.DbType = DbType.String;
            sqlPar.Value = obj.mae;
			sqlPar.ParameterName = "@mae";
            colPar.Add(sqlPar);

            sqlPar.DbType = DbType.Date;
            sqlPar.Value = obj.nascimento;
			sqlPar.ParameterName = "@nascimento";
            colPar.Add(sqlPar);

			sqlPar.DbType = DbType.String;
            sqlPar.Value = obj.nome;
			sqlPar.ParameterName = "@nome";
            colPar.Add(sqlPar);
           
            sqlPar.DbType = DbType.String;
            sqlPar.Value = obj.pai;
			sqlPar.ParameterName = "@pai";
            colPar.Add(sqlPar);

			sqlPar.DbType = DbType.String;
            sqlPar.Value = obj.plano;
			sqlPar.ParameterName = "@plano";
            colPar.Add(sqlPar);

            sqlPar.DbType = DbType.Int32;
            sqlPar.Value = obj.profissao;
			sqlPar.ParameterName = "@profissao";
            colPar.Add(sqlPar);

			sqlPar.DbType = DbType.String;
            sqlPar.Value = obj.rg;
			sqlPar.ParameterName = "@rg";
            colPar.Add(sqlPar);

            sqlPar.DbType = DbType.String;
            sqlPar.Value = obj.sexo;
			sqlPar.ParameterName = "@sexo";
            colPar.Add(sqlPar);

            sqlPar.DbType = DbType.String;
            sqlPar.Value = obj.telefone;
			sqlPar.ParameterName = "@telefone";
            colPar.Add(sqlPar);

            sqlPar.DbType = DbType.String;
            sqlPar.Value = obj.titular;
			sqlPar.ParameterName = "@titular";
            colPar.Add(sqlPar);

            sqlPar.DbType = DbType.Date;
            sqlPar.Value = obj.validade_cart;
			sqlPar.ParameterName = "@validade_cart";
            colPar.Add(sqlPar);
            

			strSQL = "Insert into PACIENTE (CNPJCPF, nome, DATA_NASCIM, sexo, rg, EST_CIVIL, profissao, NOMEPAI," +
				"NOMEMAE, convenio, CONVENIO_PLANO, NRCONVENIO, TITULAR, CONVENIO_VALIDADE_CARTEIRA, CEP, BAIRRO, " +
				"Celular, FONERESID, EMAIL";
			

			return "Ok";
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
				"Isnull(Celular,'') celular, Isnull(FONERESID,'') telefone, Isnull(EMAIL,'') email " +
				"From PACIENTE where CNPJCPF=@CPF";
            
            tb = cn.OpenDataSetWithParam(strSQL.ToString(), "Paciente", colPar).Tables[0];

			return JsonConvert.SerializeObject(tb, Newtonsoft.Json.Formatting.None);
        }

		[WebMethod]
		public string getEstadoCivil()
		{
			Conexao cn = new Conexao();
            DataTable tb;

			tb = cn.OpenDataSet("SELECT Codigo, Nome FROM ESTADO_CIVIL Order by NOME", "EstadoCivil").Tables[0];

			return JsonConvert.SerializeObject(tb, Newtonsoft.Json.Formatting.None);
		}

    	[WebMethod]
        public string getProfissoes()
        {
            Conexao cn = new Conexao();
            DataTable tb;

    		tb = cn.OpenDataSet("SELECT Codigo, Nome FROM TABPROFISSOES Order by NOME", "Profissoes").Tables[0];

			return JsonConvert.SerializeObject(tb, Newtonsoft.Json.Formatting.None);
        }

		[WebMethod]
        public string getConvenio()
        {
            Conexao cn = new Conexao();
            DataTable tb;

			tb = cn.OpenDataSet("SELECT Codigo, Nome FROM CONVENIO Order by NOME", "Convenios").Tables[0];

			return JsonConvert.SerializeObject(tb, Newtonsoft.Json.Formatting.None);
        }

		[WebMethod]
        public string getBairro()
        {
            Conexao cn = new Conexao();
            DataTable tb;

			tb = cn.OpenDataSet("SELECT Codigo, Nome FROM BAIRRO Where NOME<>'' Order by NOME", "Bairros").Tables[0];

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
}
