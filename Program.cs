using System.Data.SQLite;
using DbfDataReader;


namespace DbfRead
{
    class Program
    {
        static void Main(string[] args)
        {
            var skipDeleted = true;
            var pathFiles = @"D:\";
            var dbfPath = pathFiles + "example.DBF";
            string dbfName = Path.GetFileNameWithoutExtension(dbfPath);
            var sqliteDb = pathFiles + dbfName + ".db";

            if (!File.Exists(sqliteDb))
            {

                SQLiteConnection.CreateFile(sqliteDb);

            }
            else
            {
                File.Delete(sqliteDb);
                SQLiteConnection.CreateFile(sqliteDb);

            }
            using (var dbfTable = new DbfTable(dbfPath))
            {
                var dbfRecord = new DbfRecord(dbfTable);

                using (var connection = new SQLiteConnection($"Data Source={sqliteDb};Version=3;"))
                {
                    connection.Open();

                    string sql = $"CREATE TABLE IF NOT EXISTS {dbfName}  (";
                    foreach (var dbfColumn in dbfTable.Columns)
                    {
                        sql += dbfColumn.ColumnName + " TEXT,";
                    }
                    sql = sql.TrimEnd(',');
                    sql += ")";
                    SQLiteCommand command = new SQLiteCommand(sql, connection);
                    command.ExecuteNonQuery();
                }
                using (var connection = new SQLiteConnection($"Data Source={sqliteDb};Version=3;"))
                {
                    connection.Open();
                    using var transaction = connection.BeginTransaction();
                    while (dbfTable.Read(dbfRecord))
                    {
                        if (skipDeleted && dbfRecord.IsDeleted)
                        {
                            continue;
                        }

                        string sql = $"INSERT INTO {dbfName} VALUES (";
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
