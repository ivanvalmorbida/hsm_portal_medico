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
			return "Ok";
            /*Conexao cn = new Conexao();
            SqlParameter sqlPar = new SqlParameter();
            ArrayList colPar = new ArrayList();
            StringBuilder strSQL = new StringBuilder();

            sqlPar.DbType = DbType.String;
			sqlPar.Value = objPac.cpf;
            sqlPar.ParameterName = "@CPF";
            colPar.Add(sqlPar);

			objPes.CODIGO;
            = $scope.cpf
            objPes.NOME = $scope.nome
            objPes.DATA_NASCIM = $scope.nascimento
            objPes.SEXO = $scope.sexo
            objPes.RG = $scope.rg
            objPes.EST_CIVIL = $scope.estadocivil
            objPes.PROFISSAO = $scope.profissao
            objPes.NOMEPAI = $scope.pai
            objPes.NOMEMAE = $scope.mae
            objPes.CONVENIO = $scope.convenio
            objPes.CONVENIO_PLANO = $scope.plano
            objPes.NRCONVENIO = $scope.carteirinha
            objPes.TITULAR = $scope.titular
            objPes.CONVENIO_VALIDADE_CARTEIRA = $scope.validade_cart
            objPes.CEP = $scope.cep
            objPes.BAIRRO = $scope.bairro
            objPes.Celular = $scope.celular
            objPes.FONERESID = $scope.telefone
            objPes.EMAIL = $scope.email*/
		}

		[WebMethod]
		public string getPacienteCPF(string strCPF)
        {
			Conexao cn = new Conexao();
            SqlParameter sqlPar = new SqlParameter();
            ArrayList colPar = new ArrayList();
            StringBuilder strSQL = new StringBuilder();
            DataTable tb;

			sqlPar.DbType = DbType.String;
            sqlPar.Value = strCPF;
            sqlPar.ParameterName = "@CPF";
            colPar.Add(sqlPar);

			strSQL.Append("SELECT * FROM PACIENTE where CNPJCPF=@CPF").AppendLine();

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
