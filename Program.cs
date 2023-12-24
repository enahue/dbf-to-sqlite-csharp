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
                //Con
                SQLiteConnection connection = new SQLiteConnection("Data Source=database.db;Version=3;");
                connection.Open();
                var dbfRecord = new DbfRecord(dbfTable);
                //read column names and create sqlite database table
                string sql = "CREATE TABLE IF NOT EXISTS cliente  (";
                foreach (var dbfColumn in dbfTable.Columns)
                {
                    sql += dbfColumn.ColumnName + " TEXT,";

                }
                sql = sql.Substring(0, sql.Length - 1);
                sql += ")";
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                command.ExecuteNonQuery();
                while (dbfTable.Read(dbfRecord))
                {
                    if (skipDeleted && dbfRecord.IsDeleted)
                    {
                        continue;
                    }
                    using var transaction = connection.BeginTransaction();
                    //insert data into sqlite database table
                    sql = "INSERT INTO cliente VALUES (";
                    foreach (var dbfValue in dbfRecord.Values)
                    {
                        sql += "'" + dbfValue.ToString() + "',";
                    }
                    sql = sql.Substring(0, sql.Length - 1);
                    sql += ")";
                    command = new SQLiteCommand(sql, connection);
                    command.ExecuteNonQuery();

                    Console.WriteLine(sql);
                    transaction.Commit();
                }
            }
        }
    }
}
