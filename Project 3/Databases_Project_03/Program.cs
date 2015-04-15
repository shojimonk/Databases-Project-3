// <copyright file="Program.cs" company="ENGI3675">
// The Database Access class.
// </copyright>
namespace Assignment3
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Npgsql;

    /// <summary>
    /// accesses database
    /// </summary>
    public class DataAccess
    {
        /// <summary>
        /// Main method containing method calls for SQL queries to be run.
        /// </summary>
        public static void Main()
        {
            NpgsqlConnection conn = Connect();
            conn.Open();

            try
            {
                Console.WriteLine("\nSelect * on Crime_reports (all rows): \nNumber of rows in Database: {0:G}", Rowcount(false, false));
                Console.WriteLine("Number of rows in indexed Database: {0:G}", Rowcount(true, false));
                
                Console.WriteLine("\nNow, using our 'select - where' command, which is: ");
                Console.WriteLine("\texplain analyze select category, dayofweek, mmddyyyy ");
                Console.WriteLine("\tfrom crime_reports where y_coord > 37.76 order by y_coord;\n");
                
                Console.WriteLine("On our un-indexed Table: \n");

                Console.WriteLine("Number of rows: {0:G}", Rowcount(false, true));

                Console.WriteLine(explainAnalyze(false));

                Console.WriteLine("\n\n");
                Console.WriteLine("And, on our Indexed Table: \n");
                Console.WriteLine("Number of rows: {0:G}", Rowcount(true, true));
                Console.WriteLine(explainAnalyze(true));
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Builds connection and returns connection string.
        /// </summary>
        /// <returns> NpgsqlConnection object</returns>
        private static NpgsqlConnection Connect()
        {
            NpgsqlConnectionStringBuilder myBuilder = new NpgsqlConnectionStringBuilder()
            {
                Host = "127.0.0.1",
                Port = 5432,
                Database = "LU.ENGI3675.Proj03",
                IntegratedSecurity = true
            };
            ////myBuilder.UserName= "postgres";
            // myBuilder.Password = "Ir18ssantos";
            NpgsqlConnection conn = new NpgsqlConnection(myBuilder);
            return conn;
        }

        /// <summary>
        /// Calls pre-set SQL query based on status of boolean flags and counts rows returned by query
        /// </summary>
        /// <param name="hasindex"> boolean if SQL query on indexed table or not</param>
        /// <param name="haswhere"> boolean if SQL query to include "where" clause</param>
        /// <returns> long integer of row count of query</returns>
        public static long Rowcount(bool hasindex, bool haswhere)
        {
            long count = 0;
            string indexed = "";
            string where = "";

            if (hasindex)
            {
                indexed = "_indexed";
            }
            if (haswhere)
            {
                where = " where y_coord > 37.76";
            }
            using (NpgsqlConnection conn = Connect())
            {
                conn.Open();
                NpgsqlCommand command = new NpgsqlCommand("select count(*) as rowcount from Crime_Reports" + indexed + where, conn);
                NpgsqlDataReader table = command.ExecuteReader();
                foreach(IDataRecord row in table)
                {
                    count = (long)row[0];
                }
                
                return count;
            }

        }

        /// <summary>
        /// Calls explain analyze on same SQL query as "Rowcount" method, on indexed or non-indexed table.
        /// </summary>
        /// <param name="hasindex"> boolean if query to be run on indexed table</param>
        /// <returns> string of explain analyze results</returns>
        public static string explainAnalyze(bool hasindex)
        {
            string explained = "";
            string indexed = "";
            if(hasindex)
            {
                indexed = "_indexed";
            }

            using(NpgsqlConnection conn = Connect())
            {
                conn.Open();
                NpgsqlCommand command = new NpgsqlCommand("explain analyze select category, dayofweek, mmddyyyy from crime_reports" + indexed + " where y_coord > 37.76 order by y_coord;", conn);
                NpgsqlDataReader table = command.ExecuteReader();
                foreach (IDataRecord row in table)
                {
                    explained += (string)table[0]+"\n";
                }
            }
            

            return explained;
        }

        /// <summary>
        /// method that calls other SQL call methods, and times their total execution.
        /// </summary>
        /// <param name="hasindex"> SQL </param>
        /// <param name="haswhere"></param>
        /// <param name="analyze"></param>
        /// <returns></returns>
        public static TimeSpan Timing(bool hasindex, bool haswhere, bool analyze)
        {
            TimeSpan timer;
            DateTime t1, t2;
            if (analyze)
            {
                t1 = DateTime.Now;
                explainAnalyze(hasindex);
                t2 = DateTime.Now;
            }
            else
            {
                t1 = DateTime.Now;
                Rowcount(hasindex, haswhere);
                t2 = DateTime.Now;
            }
            timer = t2 - t1;
            return timer;
        }
    }
}
