using System;
using System.IO;
using System.Data;
using System.Data.SQLite;
using DbfDataReader;
using System.Transactions;


namespace DbfRead
{
    class Program
    {
        static void Main(string[] args)
        {
            var skipDeleted = true;
            var dbfPath = @"D:\CLIENTES.DBF";
            using (var dbfTable = new DbfTable(dbfPath))
            {
                var dbfRecord = new DbfRecord(dbfTable);
                //Con
                using (var connection = new SQLiteConnection("Data Source=database.db;Version=3;"))
                {
                    connection.Open();

                    //read column names and create sqlite database table
                    string sql = "CREATE TABLE IF NOT EXISTS cliente  (";
                    foreach (var dbfColumn in dbfTable.Columns)
                    {
                        sql += dbfColumn.ColumnName + " TEXT,";
                    }
                    sql = sql.TrimEnd(',');
                    sql += ")";
                    SQLiteCommand command = new SQLiteCommand(sql, connection);
                    command.ExecuteNonQuery();
                }
                using (var connection = new SQLiteConnection("Data Source=database.db;Version=3;"))
                {
                    connection.Open();
                    using var transaction = connection.BeginTransaction();
                    while (dbfTable.Read(dbfRecord))
                    {
                        if (skipDeleted && dbfRecord.IsDeleted)
                        {
                            continue;
                        }

                        //insert data into sqlite database table
                        string sql = "INSERT INTO cliente VALUES (";
                        foreach (var dbfValue in dbfRecord.Values)
                        {
                            sql += "'" + dbfValue.ToString().Replace("'", "''") + "',";
                        }
                        sql = sql.TrimEnd(',');
                        sql += ")";
                        SQLiteCommand command = new SQLiteCommand(sql, connection);
                        command.ExecuteNonQuery();

                        Console.WriteLine(sql);
                    }
                    transaction.Commit();
                }
            }
        }
    }
}
