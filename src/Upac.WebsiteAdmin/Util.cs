namespace Upac.WebsiteAdmin
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;

    public static class Util
    {
        #region Methods

        public static void CopyDirectory(string Src, string Dst)
        {
            String[] Files;

            if (Dst[Dst.Length - 1] != Path.DirectorySeparatorChar)
                Dst += Path.DirectorySeparatorChar;
            if (!Directory.Exists(Dst)) Directory.CreateDirectory(Dst);
            Files = Directory.GetFileSystemEntries(Src);
            foreach (string Element in Files)
            {
                // Sub directories
                if (Directory.Exists(Element))
                    CopyDirectory(Element, Dst + Path.GetFileName(Element));
                // Files in directory
                else
                    File.Copy(Element, Dst + Path.GetFileName(Element), true);
            }
        }

        public static void CreateDatabase(string name, string folderPath)
        {
            string mdfFilename = Path.Combine(folderPath, name + ".mdf");
            string ldfFilename = Path.Combine(folderPath, name + ".ldf");

            System.Data.SqlClient.SqlConnection tmpConn;
            string sqlCreateDBQuery;
            tmpConn = new SqlConnection();
            tmpConn.ConnectionString = "SERVER = .; DATABASE = master; User ID = sa; Pwd = password";
            sqlCreateDBQuery = "" +
                               "CREATE DATABASE " + name + " " +
                               "ON  " +
                               "( NAME = " + name + "_dat, " +
                               "    FILENAME = '" + mdfFilename + "', " +
                               "    SIZE = 10, " +
                               "    MAXSIZE = 100, " +
                               "    FILEGROWTH = 5 ) " +
                               "LOG ON " +
                               "( NAME = " + name + "_log, " +
                               "    FILENAME = '" + ldfFilename + "', " +
                               "    SIZE = 5MB, " +
                               "    MAXSIZE = 100MB, " +
                               "    FILEGROWTH = 5MB ) ; ";

            SqlCommand myCommand = new SqlCommand(sqlCreateDBQuery, tmpConn);
            try
            {
                tmpConn.Open();
                myCommand.ExecuteNonQuery();
                Console.WriteLine("Database has been created successfully!");
            }
            finally
            {
                tmpConn.Close();
            }

            myCommand = new SqlCommand("ALTER DATABASE " + name + " SET RECOVERY SIMPLE", tmpConn);
            try
            {
                tmpConn.Open();
                myCommand.ExecuteNonQuery();
                Console.WriteLine("Recovery set to simple!");
            }
            finally
            {
                tmpConn.Close();
            }
            return;
        }

        public static void DeleteDatabase(string name)
        {
            System.Data.SqlClient.SqlConnection tmpConn;
            string sqlCreateDBQuery;
            tmpConn = new SqlConnection();
            tmpConn.ConnectionString = "SERVER = .; DATABASE = master; User ID = sa; Pwd = password";
            sqlCreateDBQuery = "DROP DATABASE " + name;

            SqlCommand myCommand = new SqlCommand(sqlCreateDBQuery, tmpConn);
            try
            {
                tmpConn.Open();
                myCommand.ExecuteNonQuery();
                Console.WriteLine(string.Format("Database {0} has been dropped successfully!", name));
            }
            catch(Exception ex)
            {
                Console.WriteLine(string.Format("Could not drop database {0}!", name));
                Trace.WriteLine(string.Format("Could not drop database {0}!", name));
            }
            finally
            {
                tmpConn.Close();
            }
        }

        public static void EnsureFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Console.WriteLine("CreateDirectory: " + path);
                Directory.CreateDirectory(path);
            }
        }

        public static void ModifyWebconfig(string dbstring, string path)
        {
            string searchFor = @"datalayer=SqlServer;server=.\sqlexpress;database=DATABASE;user id=DBUSER;password=PASSWORD";
            Replace(ref path, ref searchFor, ref dbstring);
            searchFor = @"server=.\SQLEXPRESS;database=DATABASE;user id=USER;password=PASS";
            Replace(ref path, ref searchFor, ref dbstring);
        }

        public static bool Replace(ref string file, ref string searchFor, ref string replaceWith)
        {
            try
            {
                //get a StreamReader for reading the file
                StreamReader reader = new StreamReader(file);

                //read the entire file at once
                string contents = reader.ReadToEnd();

                //close up and dispose
                reader.Close();
                reader.Dispose();

                //use regular expressions to search and replace our text
                contents = contents.Replace(searchFor, replaceWith);

                //get a StreamWriter for writing the new text to the file
                StreamWriter writer = new StreamWriter(file);

                //write the contents
                writer.Write(contents);

                //close up and dispose
                writer.Close();
                writer.Dispose();

                //return successful
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion Methods
    }
}