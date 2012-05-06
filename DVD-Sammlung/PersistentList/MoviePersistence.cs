using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using FirebirdSql.Data.FirebirdClient;

namespace DvdCollection.PersistentList
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    internal static class MoviePersistence
    {
        #region public methods

        internal static List<MovieInfo> LoadMovies ()
        {
            if (!File.Exists (DB_FILENAME))
            {
                CreateDatabase ();
                return new List<MovieInfo> ();
            }

            List<MovieInfo> result = new List<MovieInfo> ();

            string commandText = @"SELECT * FROM MOVIE_DATA;";

            using (FbConnection connection = new FbConnection (string.Format (CONNECTION_STRING_FORMAT_FIREBIRD, DB_FILENAME)))
            {
                connection.Open ();

                using (FbTransaction transaction = connection.BeginTransaction ())
                {
                    using (FbCommand cmd = new FbCommand (commandText, connection, transaction))
                    {
                        FbDataReader reader = cmd.ExecuteReader ();
                        while (reader.Read ())
                        {
                            MovieFileData fileData = new MovieFileData ()
                            {
                                Duration = reader.GetDouble (7),
                                X = reader.GetInt32 (5),
                                Y = reader.GetInt32 (6)
                            };

                            string rawTitlePath = reader.GetString (8);
                            string dvdName = reader.GetString (4);
                            MovieInfo info = new MovieInfo (rawTitlePath, dvdName, fileData)
                            {
                                CoverImage = null,// BitmapSource.Create (reader.GetBytes (3))
                                Description = reader.GetString (1),
                                Genres = reader.GetString (0),
                                Rating = reader.GetString (2)
                            };

                            result.Add (info);
                        }
                    }
                    transaction.Commit ();
                }
            }

            // Write into the new database structure
            //CreateDatabase ();
            //foreach (MovieInfo info in result)
            //{
            //    Add (info);
            //}

            return result;
        }

        internal static void Add (MovieInfo movieInfo)
        {
            string commandText = @"INSERT INTO MOVIE_DATA (
                                    LOCATION, FILE_DATA_X, FILE_DATA_Y, FILE_DATA_DURATION, RAW_TITLE_PATH) 
                                    VALUES (@location, @file_data_x, @file_data_y, @file_duration, @raw_title_path)";

            using (FbConnection connection = new FbConnection (string.Format (CONNECTION_STRING_FORMAT_FIREBIRD, DB_FILENAME)))
            {
                connection.Open ();

                using (FbTransaction transaction = connection.BeginTransaction ())
                {
                    using (FbCommand cmd = new FbCommand (commandText, connection, transaction))
                    {
                        cmd.Parameters.Add (new FbParameter ("@location", movieInfo.DvdName));
                        cmd.Parameters.Add (new FbParameter ("@file_data_x", movieInfo.FileData.X));
                        cmd.Parameters.Add (new FbParameter ("@file_data_y", movieInfo.FileData.Y));
                        cmd.Parameters.Add (new FbParameter ("@file_duration", movieInfo.FileData.Duration));
                        cmd.Parameters.Add (new FbParameter ("@raw_title_path", movieInfo.RawTitlePath));

                        cmd.ExecuteNonQuery ();
                    }
                    transaction.Commit ();
                }
            }
        }

        internal static void Delete (MovieInfo movieInfo)
        {//new FirebirdSql.Data.FirebirdClient.FbCommandBuilder().
        }

        internal static void Update (MovieInfo movieInfo)
        {
            SqlCommand command = new SqlCommand ();
            command.CommandText = @"INSERT INTO MOVIE_DATA (
                                    LOCATION, FILE_DATA_X, FILE_DATA_Y, FILE_DATA_DURATION, RAW_TITLE_PATH) 
                                    VALUES ('Los Angeles', 900, '10.Jan.1999')";
            command.Parameters.Add (new SqlParameter ("@PROD_ID", 100));



            //JpegBitmapEncoder encoder = new JpegBitmapEncoder ();
            //encoder.Frames.Add (BitmapFrame.Create (bmSource));
            //MemoryStream stream = new MemoryStream ();
            //encoder.Save (stream);

        }

        #endregion

        #region private methods

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
                                GENRES VARCHAR(300),
                                DESCRIPTION VARCHAR(1000),
                                RATING VARCHAR(200), 
                                COVER BLOB,
                                LOCATION VARCHAR(100) NOT NULL,
                                FILE_DATA_X INTEGER NOT NULL,
                                FILE_DATA_Y INTEGER NOT NULL,
                                FILE_DATA_DURATION FLOAT NOT NULL,
                                RAW_TITLE_PATH VARCHAR(300) NOT NULL,
                                PRIMARY KEY (RAW_TITLE_PATH, LOCATION)
                            );"
                );
            return statements;
        }

        #endregion

        #region private fields

        private static readonly string CONNECTION_STRING_FORMAT_FIREBIRD = "User=u;Password=p;Database={0};Port=3050;Dialect=3;Charset=NONE;Role=;Connection lifetime=15;Pooling=true;MinPoolSize=0;MaxPoolSize=50;Packet Size=8192;ServerType=1;";
        private static readonly string DB_FILENAME = "dvd.fdb";

        #endregion
    }
}