using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
//
public sealed class Conexao
{
    private SqlConnection DBCon = new SqlConnection();
	private string strConection = "Server=localhost;Database=hsm;User ID=SA;Password=Ivanluis1;Trusted_Connection=False;Connection Timeout=30;Pooling=False;";
    private string _MSG;

    public string MSG
    {
        get
        {
            return _MSG;
        }
    }

    public SqlConnection Connection
    {
        get
        {
            return DBCon;
        }
    }

    public string StringConection
    {
        get
        {
            return strConection;
        }
        set
        {
            strConection = value;
        }
    }

    public void OpenConection()
    {
        if (DBCon.State == ConnectionState.Closed)
        {            
            DBCon = new SqlConnection(strConection);
            DBCon.Open();
        }
    }

    public void CloseConection()
    {
        if (DBCon.State == ConnectionState.Open)
            DBCon.Close();
    }

    public SqlDataReader OpenReader(string strSQL)
    {
        if (DBCon.State == ConnectionState.Closed)
            OpenConection();

        SqlCommand SQLCommand = new SqlCommand(strSQL, DBCon);
        SqlDataReader SQLReader = SQLCommand.ExecuteReader();
        return SQLReader;
    }

    public DataSet OpenDataSet(string strSQL, string strTabela)
    {
        if (DBCon.State == ConnectionState.Closed)
            OpenConection();

        SqlDataAdapter SQLDataAd = new SqlDataAdapter(strSQL, DBCon);
        DataSet myDataSet = new DataSet();
        SQLDataAd.Fill(myDataSet, strTabela);
        return myDataSet;
	}

    public SqlDataAdapter DataAdapter(string strSQL)
    {
        if (DBCon.State == ConnectionState.Closed)
            OpenConection();
        SqlDataAdapter SQLDataAd = new SqlDataAdapter(strSQL, DBCon);
        return SQLDataAd;
    }

    public void Execute(string strSQL)
    {
        if (DBCon.State == ConnectionState.Closed)
            OpenConection();

        SqlCommand SQLComand = new SqlCommand(strSQL, DBCon);
        try
        {
            _MSG = "";
            SQLComand.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            _MSG = e.Message;
        }
    }

	public void ExecuteWithParam(string strSQL, ArrayList SQLPar)
    {
        if (DBCon.State == ConnectionState.Closed)
            OpenConection();

        SqlCommand SQLCommandWithPar = new SqlCommand();

        SQLCommandWithPar.Connection = DBCon;
        SQLCommandWithPar.CommandText = strSQL;
		foreach (SqlParameter xPar in SQLPar)
            SQLCommandWithPar.Parameters.AddWithValue(xPar.ParameterName, xPar.Value);
		
        try
        {
            _MSG = "";
            SQLCommandWithPar.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            _MSG = e.Message;
        }
    }

    public SqlDataReader OpenReaderWithParam(string strSQL, ArrayList SQLPar)
    {
        SqlDataReader rd;
        
        if (DBCon.State == ConnectionState.Closed)
            OpenConection();

        SqlCommand SQLCommandWithPar = new SqlCommand();

        SQLCommandWithPar.Connection = DBCon;
        SQLCommandWithPar.CommandText = strSQL;
		foreach (SqlParameter xPar in SQLPar)
            SQLCommandWithPar.Parameters.AddWithValue(xPar.ParameterName, xPar.Value);

		rd = SQLCommandWithPar.ExecuteReader();
        return rd;
    }

	public DataSet OpenDataSetWithParam(string strSQL, string strTabela, ArrayList SQLPar)
    {
        if (DBCon.State == ConnectionState.Closed)
            OpenConection();

        SqlCommand SQLCommandWithPar = new SqlCommand();

        SQLCommandWithPar.Connection = DBCon;
        SQLCommandWithPar.CommandText = strSQL;
		foreach (SqlParameter xPar in SQLPar)
            SQLCommandWithPar.Parameters.AddWithValue(xPar.ParameterName, xPar.Value);

        SqlDataAdapter SQLDataAd = new SqlDataAdapter(SQLCommandWithPar);
        DataSet myDataSet = new DataSet();

        SQLDataAd.Fill(myDataSet, strTabela);
        return myDataSet;
    }

	public ArrayList PreencherPar(object objX)
    {
        SqlParameter sqlPar;
		ArrayList colPar = new ArrayList();

        foreach (System.Reflection.PropertyInfo p in objX.GetType().GetProperties())
        {
            if (p.CanRead)
            {
                sqlPar = new SqlParameter();
				sqlPar.DbType = DbType.String; // TipoDB(p.PropertyType.Name);
                sqlPar.Value = p.GetValue(objX, null);
                if (sqlPar.Value == null/* TODO Change to default(_) if this is not a reference type */ & p.PropertyType.Name != "Boolean")
                    sqlPar.Value = DBNull.Value;
                sqlPar.ParameterName = "@" + p.Name;
                colPar.Add(sqlPar);
            }
        }

        return colPar;
    }

	public ArrayList RemoverPar(ArrayList colPar, string strPar)
    {
        SqlParameter sqlPar;

        for (int i = 1; i <= colPar.Count; i++)
        {
			sqlPar = (ArrayList)colPar[i];
            if (sqlPar.ParameterName == "@" + strPar)
            {
                colPar.Remove(i);
                break;
            }
        }

        return colPar;
    }

	public object TipoProp(object o, string strTipo)
    {
        if (o.ToString() == "")
        {
            if (strTipo == "String")
                return "";
            else if (strTipo == "Boolean")
				return false;
            else if (strTipo == "DateTime")
                return null;
            else
				return 0;
        }
        else if (strTipo == "Boolean")
			return (o.ToString() != "0");
        else
			return o;
    }

    public DbType TipoDB(string strTipo)
    {
        if (strTipo == "String")
			return DbType.String;
        else if (strTipo == "Int16")
			return DbType.Int16;
        else if (strTipo == "Int32")
			return DbType.Int32;
        else if (strTipo == "Int64")
			return DbType.Int64;
        else
			return DbType.String;
    }

    public object PreencherObj(SqlDataReader sqlReader, object objX)
    {
        foreach (System.Reflection.PropertyInfo p in objX.GetType().GetProperties())
        {
            if (p.CanRead)
                p.SetValue(objX, TipoProp(sqlReader(p.Name), p.PropertyType.Name), null/* TODO Change to default(_) if this is not a reference type */);
        }

		return objX;
    }
}

            //private string strConection = "Server=<Server>;Database=<DB>;User ID=SA;Password=CTInfo&Kleros4002;Trusted_Connection=False;Connection Timeout=30;Pooling=False;";

            /*System.IO.StreamReader fluxoTexto;
            string winDir, strX;

            winDir = System.Environment.GetEnvironmentVariable("windir");
            strX = "";
            if (System.IO.File.Exists(winDir + @"\Kleros_Clinica.ini"))
            {
                fluxoTexto = new System.IO.StreamReader(winDir + @"\Kleros_Clinica.ini");
                strX = fluxoTexto.ReadLine();
                while (true)
                {
                    if (Strings.Left(strX, 4) == "PATH")
                        break;
                    try
                    {
                        strX = fluxoTexto.ReadLine();
                    }
                    catch (Exception ex)
                    {
                        break;
                    }
                }
                fluxoTexto.Close();
            }

            strX = Strings.Right(strX, Strings.Len(strX) - 7);
            strX = strX + "Config.xml";

            XmlDocument objXml = new XmlDocument();
            objXml.Load(strX);

            strX = objXml.SelectSingleNode("config").ChildNodes[0].ChildNodes[0].InnerText;

            string[] matriz = strX.Split(";");
            strX = matriz[1];
            strX = Strings.Right(strX, Strings.Len(strX) - 12);
            strConection = strConection.Replace("<Server>", strX);

            strX = matriz[2];
            strX = Strings.Right(strX, Strings.Len(strX) - 16);
            strConection = strConection.Replace("<DB>", strX);*/
