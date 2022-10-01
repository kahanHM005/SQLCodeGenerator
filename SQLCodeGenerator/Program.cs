
using SQLCodeGenerator;
using System.Data;

DAL.ConnectionString = "Data Source=.;Initial Catalog=Test;Integrated Security=True";
int st = 0;
bool q = false;
while(!q)
{
    PrintMenu();
    ConsoleKeyInfo c = Console.ReadKey();
    switch(c.Key)
    {
        case ConsoleKey.Q:
        case ConsoleKey.D3:
            q = true;
            break;
        case ConsoleKey.I:
        case ConsoleKey.D1:
            st = 1;
            break;
        case ConsoleKey.U:
        case ConsoleKey.D2:
            st = 2;
            break;
    }
    if(!q)
    {
        Console.Write("\n\n\n\t Enter Table Name :  ");
        string? t = Console.ReadLine();
        if (!String.IsNullOrEmpty(t))
        {
            string script = GenerateScript(t, st);
            Console.WriteLine("\n\n\n" + script);
            Console.Write("\n\n\n\t   Press any key to go to main menu ");
            Console.ReadKey();
        }
    }
}

static void PrintMenu()
{
    Console.Clear();
    Console.WriteLine("******************Insert Update sql script generator**********************");
    Console.WriteLine("\n\n\n\t1. (I) Insert Script ");
    Console.WriteLine("\t2. (U) Update Script ");
    Console.WriteLine("\t3. (Q) Quit ");
    Console.Write("\n\t   Enter your choice : ");
}

static string GenerateScript(string? table, int type)
{
    string sql = "";
    try
    {
        string query = @"SELECT COL.name AS ColumnName, ST.name as DataType, COL.max_length
                            From sys.columns COL INNER JOIN sys.tables TAB 
                            On COL.object_id = TAB.object_id INNER JOIN sys.types ST on ST.user_type_id = COL.user_type_id
                            where TAB.name = '" + table + "'";

        DataSet ds = DAL.GetDataSet(query);

        if (ds.Tables[0].Rows.Count == 0)
            return "Table " + table + " not found";

        List<string> s1 = new List<string>();
        List<string> s2 = new List<string>();
        List<string> s3 = new List<string>();
        List<string> s4 = new List<string>();
        foreach (DataRow r in ds.Tables[0].Rows)
        {
            string t = "";
            string? n = r["ColumnName"].ToString();
            string? dt = r["DataType"].ToString();
            string? l = r["max_length"].ToString();
            s1.Add("[" + n + "]");
            s3.Add("@" + n);
            s4.Add("[" + n + "]" + " = " + "@" + n);
            switch (dt)
            {
                case "bigint":
                    t = "DAL.CreateParameter(\"@" + n + "\",SqlDbType.BigInt,-1,vvv)";
                    break;
                case "bit":
                    t = "DAL.CreateParameter(\"@" + n + "\",SqlDbType.Bit,-1,vvv)";
                    break;
                case "char":
                    t = "DAL.CreateParameter(\"@" + n + "\",SqlDbType.Char," + l + ",vvv)";
                    break;
                case "datetime":
                    t = "DAL.CreateParameter(\"@" + n + "\",SqlDbType.DateTime,-1,vvv)";
                    break;
                case "decimal":
                    t = "DAL.CreateParameter(\"@" + n + "\",SqlDbType.Decimal,-1,vvv)";
                    break;
                case "float":
                    t = "DAL.CreateParameter(\"@" + n + "\",SqlDbType.Float,-1,vvv)";
                    break;
                case "int":
                    t = "DAL.CreateParameter(\"@" + n + "\",SqlDbType.Int,-1,vvv)";
                    break;
                case "nchar":
                    t = "DAL.CreateParameter(\"@" + n + "\",SqlDbType.NChar," + l + ",vvv)";
                    break;
                case "nvarchar":
                    t = "DAL.CreateParameter(\"@" + n + "\",SqlDbType.NVarChar," + l + ",vvv)";
                    break;
                case "smallint":
                    t = "DAL.CreateParameter(\"@" + n + "\",SqlDbType.SmallInt,-1,vvv)";
                    break;
                case "tinyint":
                    t = "DAL.CreateParameter(\"@" + n + "\",SqlDbType.TinyInt,-1,vvv)";
                    break;
                case "varchar":
                    t = "DAL.CreateParameter(\"@" + n + "\",SqlDbType.VarChar," + l + ",vvv)";
                    break;
            }
            s2.Add(t);
        }
        if (type == 1)
            sql = "Insert Into " + table + "(" + String.Join(",", s1.ToArray())
                + ") Values( " + String.Join(",", s3.ToArray()) + ")," + Environment.NewLine + String.Join("," + Environment.NewLine, s2.ToArray()) + ");";
        else
            sql = "Update " + table + " Set " + String.Join(",", s4.ToArray()) + " Where Col = @value," + Environment.NewLine
                + String.Join("," + Environment.NewLine, s2.ToArray()) + ");";
    }
    catch(Exception ex)
    {
        return ex.ToString();
    }
    return sql;
}
