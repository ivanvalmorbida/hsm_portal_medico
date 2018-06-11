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
