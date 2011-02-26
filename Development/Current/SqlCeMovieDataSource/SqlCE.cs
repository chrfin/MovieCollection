//#define debug_output

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlServerCe;
using System.Diagnostics;
using System.Data;
using System.IO;
using MovieDataSource.Properties;

namespace MovieDataSource
{
    internal class SqlCE
    {
        /// <summary>
        /// Creates the new DB.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="overwrite">if set to <c>true</c> overwrite exisiting file.</param>
        /// <returns></returns>
        public static string CreateNewDB(string filename, bool overwrite)
        {
            string file = filename;
            if (File.Exists(file))
            {
                if (overwrite)
                    File.Delete(file);
                else
                {
                    int cnt = 1;
                    while (File.Exists(Path.ChangeExtension(file, "_" + cnt + Path.GetExtension(file)))) cnt++;
                    file = Path.ChangeExtension(file, "_" + cnt + Path.GetExtension(file));
                }
            }

            SqlCeEngine engine = new SqlCeEngine(GetConnectionString(file));
            engine.CreateDatabase();
            engine.Dispose();

            SqlCeConnection con = new SqlCeConnection(GetConnectionString(file));
            con.Open();
            SqlCeCommand cmd = con.CreateCommand();
            cmd.CommandText = Resources.DataBase;
            ExecuteNonQuery(cmd);
            con.Close();
            con.Dispose();

            return file;
        }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public static string GetConnectionString(string filename)
        {
            return string.Format("Data Source={0};Max Database Size=4091;File Mode=Exclusive;Persist Security Info=False;", filename);
        }

        /// <summary>
        /// Executes the query and returns the affected rows.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        /// <remarks>Documented by DAC, 2008-11-13.</remarks>
        public static int ExecuteNonQuery(SqlCeCommand command)
        {
            command.CommandText = command.CommandText.TrimEnd(' ', ';');
#if DEBUG && debug_output
            WriteDebugInfos(command);
#endif
            int count = 0;
            if (!command.CommandText.Contains(";"))
                count = command.ExecuteNonQuery();
            else
            {
                foreach (string q in command.CommandText.Split(';'))
                {
                    if (q.TrimStart(' ', '\r', '\n').StartsWith("--") || q.TrimStart(' ', '\r', '\n').Length < 5)
                        continue;

                    SqlCeCommand cmd = command.Connection.CreateCommand();
                    cmd.CommandText = q;
                    foreach (SqlCeParameter param in command.Parameters)
                        cmd.Parameters.Add(param.ParameterName, param.Value == null ? DBNull.Value : param.Value);
                    count += cmd.ExecuteNonQuery();
                }
            }
            return count;
        }

        /// <summary>
        /// Executes the query and returns a reader.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        /// <remarks>Documented by DAC, 2008-11-13.</remarks>
        public static SqlCeDataReader ExecuteReader(SqlCeCommand command)
        {
            command.CommandText = command.CommandText.TrimEnd(' ', ';');
#if DEBUG && debug_output
            WriteDebugInfos(command);
#endif
            if (!command.CommandText.Contains(";"))
                return command.ExecuteReader(CommandBehavior.SequentialAccess);
            else
            {
                SqlCeCommand cmd = null;
                foreach (string q in command.CommandText.Split(';'))
                {
                    if (cmd != null)
                    {
                        cmd.ExecuteNonQuery();
                        cmd = null;
                    }

                    cmd = command.Connection.CreateCommand();
                    cmd.CommandText = q;
                    foreach (SqlCeParameter param in command.Parameters)
                        cmd.Parameters.Add(param.ParameterName, param.Value);
                }
                return cmd.ExecuteReader(CommandBehavior.SequentialAccess);
            }
        }

        /// <summary>
        /// Executes the query and returns the first cell.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        /// <remarks>Documented by DAC, 2008-11-13.</remarks>
        public static object ExecuteScalar(SqlCeCommand command)
        {
            command.CommandText = command.CommandText.TrimEnd(' ', ';');
#if DEBUG && debug_output
            WriteDebugInfos(command);
#endif
            if (!command.CommandText.Contains(";"))
                return command.ExecuteScalar();
            else
            {
                SqlCeCommand cmd = null;
                foreach (string q in command.CommandText.Split(';'))
                {
                    if (cmd != null)
                    {
                        cmd.ExecuteNonQuery();
                        cmd = null;
                    }

                    cmd = command.Connection.CreateCommand();
                    cmd.CommandText = q;
                    foreach (SqlCeParameter param in command.Parameters)
                        cmd.Parameters.Add(param.ParameterName, param.Value);
                }
                return cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// Executes the query and returns the first cell in its nullable representative.
        /// </summary>
        /// <typeparam name="T">The type from which the nullable representative is returned.</typeparam>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        /// <remarks>Documented by CFI, 2008-08-12.</remarks>
        public static T? ExecuteScalar<T>(SqlCeCommand command) where T : struct
        {
            object res = ExecuteScalar(command);
            return (res == null || res is DBNull) ? (T?)null : (T?)Convert.ChangeType(res, typeof(T));
        }

        /// <summary>
        /// Writes the debug infos.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <remarks>Documented by DAC, 2008-07-30.</remarks>
        private static void WriteDebugInfos(SqlCeCommand command)
        {
            string str = string.Format("Query ({0}): {1}", DateTime.Now.ToString("HH:mm:ss"), command.CommandText);
            if (command.Parameters.Count > 0)
            {
                str += "\tParameters:";
                foreach (SqlCeParameter parameter in command.Parameters)
                    str += string.Format(" {0}={1};", parameter.ParameterName, parameter.Value);
            }
            Debug.WriteLine(str);
        }
    }
}
