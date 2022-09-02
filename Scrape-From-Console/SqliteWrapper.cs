using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrape_From_Console
{
    public static class SqliteWrapper
    {
        //public static readonly string connectionString = new SqliteConnectionStringBuilder()
        //{
        //    Mode = SqliteOpenMode.ReadWriteCreate,
        //    Filename = 
        //}.ToString();

        public static SqliteConnection Connect()
        {
            var connection = new SqliteConnection("DataSource=PeePeePooPoo.db");
            connection.Open();
            return connection;
        }

        public static bool TryEnteringARow(SqliteConnection connection, Posting post, string tableName)
        {
            int boolAsNum = post.IsAvailable ? 1 : 0;

            var com = connection.CreateCommand();
            com.CommandText = 
                $"INSERT INTO {tableName} (PostingText, PostingLink, ImageLink, Location , IsAvailable)" +
                $"VALUES( \"{post.PostingText.Replace('\"', ' ')}\",	\"{post.PostingLink}\", \"{post.ImgLink}\", \"{post.Location}\", {boolAsNum});";

            int inserted = com.ExecuteNonQuery();

            if (inserted == 1) return true;
            else return false;
        }

        /// <summary>
        /// Creates an SQLite table of a specified name
        /// </summary>
        /// <param name="connection">to the database</param>
        /// <param name="tableName">table to create</param>
        /// <returns>true if sucsessful, false if not, else throws exept.</returns>
        public static async Task<bool> CreateTableIfNotExists(SqliteConnection connection,
            string tableName)
        {
            var command = connection.CreateCommand();
            command.CommandText =
                $@"CREATE TABLE IF NOT EXISTS {tableName} (
	            id INTEGER PRIMARY KEY,
	            PostingText TEXT NOT NULL,
	            PostingLink TEXT NOT NULL,
	            ImageLink TEXT NOT NULL,
	            Location TEXT NOT NULL,
	            IsAvailable INTEGER DEFAULT 0
                );";

            int expect0RowsInserted = await command.ExecuteNonQueryAsync();

            return tableAlreadyExists(connection, tableName);
        }

        public static bool tableAlreadyExists(SqliteConnection openConnection, string tableName)
        {
            var sql =
            "SELECT name FROM sqlite_master WHERE type='table' AND name='" + tableName + "';";
            if (openConnection.State == System.Data.ConnectionState.Open)
            {
                SqliteCommand command = new SqliteCommand(sql, openConnection);
                SqliteDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    return true;
                }
                return false;
            }
            else
            {
                throw new System.ArgumentException("Data.ConnectionState must be open");
            }
        }


        public static bool InsertRow(Posting post, SqliteConnection connection)
        {
            return true;
        }

        public static bool ThisPostIsInTheDatabase(Posting post, SqliteConnection connection)
        {
            return true;

        }

    }
}
