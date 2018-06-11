using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace hsm_portal_medico
{

    public partial class Default : System.Web.UI.Page
    {
        public void button1Clicked(object sender, EventArgs args)
        {
            button1.Text = "You clicked me";

			Conexao cn = new Conexao();
            SqlParameter sqlPar = new SqlParameter();
            ArrayList colPar = new ArrayList();
            StringBuilder strSQL = new StringBuilder();
            DataTable tb;

            sqlPar.DbType = DbType.String;
			sqlPar.Value = "10101012";
            sqlPar.ParameterName = "@Cod";
            colPar.Add(sqlPar);

            strSQL.Append("SELECT Codigo, Nome FROM Exame where Codigo=@Cod").AppendLine();

            tb = cn.OpenDataSetWithParam(strSQL.ToString(), "Cirurgia", colPar).Tables[0];
        }
    }
}
