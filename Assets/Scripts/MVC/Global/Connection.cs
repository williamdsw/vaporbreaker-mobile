using Mono.Data.Sqlite;
using System.Data;

namespace MVC.Global
{
    /// <summary>
    /// Connection with database
    /// </summary>
    public class Connection
    {
        private IDbConnection connection;
        private IDbCommand command;
        private IDbTransaction transaction;
        private IDataReader reader;

        public Connection()
        {
            connection = new SqliteConnection(string.Format("URI=file:{0}", Configuration.Properties.DatabasePath));
        }

        /// <summary>
        /// Execute desired query
        /// </summary>
        /// <param name="query"> Desired query </param>
        /// <returns> Instance of IDataReader </returns>
        protected IDataReader ExecuteQuery(string query)
        {
            connection.Open();
            command = connection.CreateCommand();
            command.CommandText = query;
            reader = command.ExecuteReader();
            return reader;
        }

        /// <summary>
        /// Execute a INSERT | UPDATE | DELETE query
        /// </summary>
        /// <param name="query"> Desired query </param>
        /// <returns> Number of affected rows </returns>
        protected int ExecuteNonQuery(string query)
        {
            connection.Open();
            command = connection.CreateCommand();
            command.CommandText = query;
            return command.ExecuteNonQuery();
        }

        /// <summary>
        /// Close all related objects
        /// </summary>
        protected void CloseConnection()
        {
            if (reader != null)
            {
                reader.Close();
            }

            if (command != null)
            {
                command.Dispose();
            }

            if (connection != null)
            {
                connection.Close();
            }
        }
    }
}