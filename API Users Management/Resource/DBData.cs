using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace MaxiUsers.Resource
{
    public class DbData
    {
        public static string connectionString = "Server=localhost;Port=3306;Database=maxiusers;Uid=root;Pwd=valentina;";

        public static DataTable List(string query)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.CommandType = CommandType.Text;

                DataTable table = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(table);

                return table;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in DbData.List: " + ex.Message);
                return null;
            }
            finally
            {
                connection.Close();
            }
        }

        public static bool Update(string query)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.CommandType = CommandType.Text;

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in DbData.Update: " + ex.Message);
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        public static bool Create(string query)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.CommandType = CommandType.Text;

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in DbData.Create: " + ex.Message);
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        public static bool Delete(string query)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.CommandType = CommandType.Text;

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in DbData.Delete: " + ex.Message);
                return false;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}


