using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient; 
using API_Users_Management.Resources;

namespace MaxiUsers.Resource
{
    public class DbData
    {
        public static string connectionString = "Server=localhost;Port=3306;Database=maxiusers;Uid=root;Pwd=valentina;";

        public static DataTable List(string storedProcedureName, List<Parameter> parameters = null)
        {
            MySqlConnection connection = new MySqlConnection(connectionString); 

            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(storedProcedureName, connection); 
                cmd.CommandType = CommandType.StoredProcedure;

                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        cmd.Parameters.AddWithValue(parameter.Name, parameter.Value);
                    }
                }

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
        
         public static bool Update(string storedProcedureName, List<Parameter> parameters)
        {  
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(storedProcedureName, connection);
                cmd.CommandType = CommandType.StoredProcedure;

                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        cmd.Parameters.AddWithValue(parameter.Name, parameter.Value);
                    }
                }

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

        public static bool Create(string storedProcedureName, List<Parameter> parameters)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(storedProcedureName, connection);
                cmd.CommandType = CommandType.StoredProcedure;

                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        cmd.Parameters.AddWithValue(parameter.Name, parameter.Value);
                    }
                }

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

        public static bool Delete(string storedProcedureName, List<Parameter> parameters)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(storedProcedureName, connection);
                cmd.CommandType = CommandType.StoredProcedure;

                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        cmd.Parameters.AddWithValue(parameter.Name, parameter.Value);
                    }
                }

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


