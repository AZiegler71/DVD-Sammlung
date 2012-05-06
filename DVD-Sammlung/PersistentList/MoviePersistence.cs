using System.Collections.Generic;
using System.IO;
using FirebirdSql.Data.FirebirdClient;

namespace DvdCollection.PersistentList
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    internal static class MoviePersistence
    {
        internal static void StoreMovies (List<MovieInfo> movieList)
        {
        }

        internal static List<MovieInfo> LoadMovies ()
        {
            if (!File.Exists (DB_FILENAME))
            {
                CreateDatabase ();
                return new List<MovieInfo> ();
            }

            //...

            return new List<MovieInfo> ();
        }

        internal static void Delete (MovieInfo movieInfo)
        {
            throw new System.NotImplementedException ();
        }

        internal static void Add (MovieInfo movieInfo)
        {
            throw new System.NotImplementedException ();
        }

        private static void CreateDatabase ()
        {
            string connectionString = string.Format (CONNECTION_STRING_FORMAT_FIREBIRD, DB_FILENAME);
            FbConnection.CreateDatabase (connectionString);

            using (FbConnection connection = new FbConnection (connectionString))
            {
                connection.Open ();
                RunStatements (connection, GetTableCreateStatementList ());
            }
        }

        private static void RunStatements (FbConnection connection, IList<string> statements)
        {
            using (FbTransaction transaction = connection.BeginTransaction ())
            {
                foreach (string statement in statements)
                {
                    using (FbCommand cmd = new FbCommand (statement, connection, transaction))
                    {
                        cmd.ExecuteNonQuery ();
                    }
                }
                transaction.Commit ();
            }
        }

        private static List<string> GetTableCreateStatementList ()
        {

            List<string> statements = new List<string> ();

            statements.Add (@"CREATE TABLE MOVIE_DATA (
                                TITLE VARCHAR(200) NOT NULL,
                                GENRES VARCHAR(300),
                                DESCRIPTION VARCHAR(1000),
                                RATING VARCHAR(200), 
                                COVER BLOB,
                                DVD VARCHAR(100) NOT NULL,
                                FILE_DATA_X INTEGER NOT NULL,
                                FILE_DATA_Y INTEGER NOT NULL,
                                FILE_DATA_DURATION FLOAT NOT NULL,
                                DB_RELEVANT_TITLE VARCHAR(200) NOT NULL,
                                PRIMARY KEY (TITLE)
                            );"
                );
            return statements;
        }

        private static readonly string CONNECTION_STRING_FORMAT_FIREBIRD = "User=u;Password=p;Database={0};Port=3050;Dialect=3;Charset=NONE;Role=;Connection lifetime=15;Pooling=true;MinPoolSize=0;MaxPoolSize=50;Packet Size=8192;ServerType=1;";
        private static readonly string DB_FILENAME = "dvd.fdb";
    }
}