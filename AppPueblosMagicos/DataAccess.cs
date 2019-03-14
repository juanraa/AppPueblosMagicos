using AppPueblosMagicos.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace AppPueblosMagicos
{
    public static class DataAccess
    {
        public static void InitializeDatabase()
        {
            using(SqliteConnection db =
                new SqliteConnection("Filename=sqliteSample.db"))
            {
                db.Open();

                String tableCommand = "CREATE TABLE IF NOT " +
                    "EXISTS MyTable (Primary_Key INTEGER PRIMARY KEY, " +
                    "Text_Entry NVARCHAR(4096) NULL)";

                SqliteCommand createTable = new SqliteCommand(tableCommand, db);

                createTable.ExecuteReader();
            }
        }

        public static void AddData(string inputText)
        {
            using(SqliteConnection db =
                new SqliteConnection("Filename=sqliteSample.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "INSERT INTO MyTable VALUES (NULL, @Entry);";
                insertCommand.Parameters.AddWithValue("@Entry", inputText);

                insertCommand.ExecuteReader();

                db.Close();
            }

        }

        public static void DeleteData(int id)
        {
            using(SqliteConnection db =
                new SqliteConnection("Filename=sqliteSample.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "DELETE FROM MyTable WHERE Primary_Key = " + id+" ;";

                insertCommand.ExecuteReader();

                db.Close();
            }

        }

        public static List<ModelDB> GetData()
        {
            List<ModelDB> entries = new List<ModelDB>();

            using(SqliteConnection db =
                new SqliteConnection("Filename=sqliteSample.db"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT * from MyTable", db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while(query.Read())
                {
                    ModelDB modelDB = new ModelDB();
                    modelDB.IdDB = query.GetInt32(0);
                    modelDB.ContentDB = query.GetString(1);

                    entries.Add(modelDB);
                }

                db.Close();
            }

            return entries;
        }
    }
}
