using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace hsm_portal_medico
{

    public partial class login : System.Web.UI.Page
    {
		public void btnOkClicked(object sender, EventArgs args)
		{
			
			Conexao cn = new Conexao();
            SqlParameter sqlPar = new SqlParameter();
            ArrayList colPar = new ArrayList();
            StringBuilder strSQL = new StringBuilder();
			SqlDataReader sqlReader;

            sqlPar.DbType = DbType.String;
			sqlPar.Value = txtUsuario.Text;
			sqlPar.ParameterName = "@Apelido";
            colPar.Add(sqlPar);

			strSQL.Append("Select codigo, senha from Medico Where Apelido=@Apelido").AppendLine();

			sqlReader = cn.OpenReaderWithParam(strSQL.ToString(),colPar);
            if (sqlReader.Read())
			{
				if (sqlReader["senha"].ToString().ToUpper() == 
				    Encrypt(txtSenha.Text, Encrypt("CT informatica", "^%$#@")).ToUpper())
				{
					FormsAuthentication.RedirectFromLoginPage(sqlReader["codigo"].ToString(), false);
				}
				else
                {
                    lblMensagem.Text = "Atencao senha inalida!";
                    FormsAuthentication.SignOut();
                }
			}
			else {
				lblMensagem.Text = "Atencao usario inalido!";
				FormsAuthentication.SignOut();
			}
		}
        
		public string Encrypt(string Password, string Key)
        {
			string strAUX = "";
			int c, I, l, x;

			l = Key.Length;
			for (I = 1; I <= Password.Length; I++)
            {
				x = I % l;
				if (x == 0) x = -1; else x = 0;

				c = Convert.ToInt32(Convert.ToChar(Key.Substring(((I % l) - l * x)-1, 1)));
				strAUX += Convert.ToChar(
					Convert.ToInt32(Convert.ToChar(
						Password.Substring(I-1, 1)
					)) ^ c
				);
            }
            return strAUX;
        }
	}
}