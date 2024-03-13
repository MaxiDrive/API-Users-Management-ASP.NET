using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace API_Users_Management.Controllers
{
    [ApiController]
    [Route("api")]
    public class UserController : ControllerBase
    {
        public static string connectionString = "Server=localhost;Port=3306;Database=maxiusers;Uid=root;Pwd=valentina;";

        [HttpGet]
        [Route("users")]
        public IActionResult ListUsers()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM maxiusers.person";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    DataTable users = new DataTable();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(users);

                    if (users.Rows.Count > 0)
                    {
                        var userList = new List<object>();
                        foreach (DataRow row in users.Rows)
                        {
                            var user = new
                            {
                                Id_person = row["Id_person"],
                                Id_type = row["type_user_Id_type_user"],
                                Name = row["Name"],
                                Address = row["Address"],
                                Mail = row["Mail"],
                                UserName = row["UserName"],
                                Password = row["Password"],
                                Age = row["Age"],
                                Img = row["Img"] != DBNull.Value ? Convert.ToBase64String((byte[])row["Img"]) : null  // Convertir bytes a Base64 si no es nulo
                            };
                            userList.Add(user);
                        }

                        string jsonUsers = JsonConvert.SerializeObject(userList);

                        return Ok(jsonUsers);
                    }
                    else
                    {
                        return NotFound("No users found");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in ListUsers: " + ex.Message);
                    return StatusCode(500, "Error retrieving users");
                }
            }
        }

        [HttpGet]
        [Route("users/{id}")]
        public IActionResult GetUserById(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = $"SELECT * FROM maxiusers.person WHERE Id_person = {id}";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    DataTable userTable = new DataTable();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(userTable);

                    if (userTable.Rows.Count > 0)
                    {
                        DataRow row = userTable.Rows[0];
                        var foundUser = new
                        {
                            Id_person = row["Id_person"],
                            Id_type = row["type_user_Id_type_user"],
                            Name = row["Name"],
                            Address = row["Address"],
                            Mail = row["Mail"],
                            UserName = row["UserName"],
                            Password = row["Password"],
                            Age = row["Age"],
                            Img = row["Img"] != DBNull.Value ? Convert.ToBase64String((byte[])row["Img"]) : null // Convertir bytes a Base64 si la imagen no es nula
                        };

                        string jsonUser = JsonConvert.SerializeObject(foundUser);

                        return Ok(jsonUser);
                    }
                    else
                    {
                        return NotFound("User not found");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in GetUserById: " + ex.Message);
                    return StatusCode(500, "Error retrieving user");
                }
            }
        }

        [HttpPatch]
        [Route("users/{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UpdatedUserDTO updatedUser)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "UPDATE maxiusers.person SET ";
                    List<string> updateValues = new List<string>();

                    if (updatedUser.IdType != null)
                        updateValues.Add($"type_user_Id_type_user = {updatedUser.IdType}");

                    if (!string.IsNullOrEmpty(updatedUser.Name))
                        updateValues.Add($"Name = '{updatedUser.Name}'");

                    if (!string.IsNullOrEmpty(updatedUser.Address))
                        updateValues.Add($"Address = '{updatedUser.Address}'");

                    if (!string.IsNullOrEmpty(updatedUser.Email))
                        updateValues.Add($"Mail = '{updatedUser.Email}'");

                    if (!string.IsNullOrEmpty(updatedUser.UserName))
                        updateValues.Add($"UserName = '{updatedUser.UserName}'");

                    if (!string.IsNullOrEmpty(updatedUser.Password))
                        updateValues.Add($"Password = '{updatedUser.Password}'");

                    if (updatedUser.Age != null)
                        updateValues.Add($"Age = '{updatedUser.Age:yyyy-MM-dd}'");

                    if (updatedUser.Img != null)
                    {
                        // Convertir la imagen a su representación en cadena de bytes
                        string imgBase64 = Convert.ToBase64String(updatedUser.Img);
                        updateValues.Add($"Img = '{imgBase64}'");
                    }

                    string updateSet = string.Join(", ", updateValues);

                    query += $"{updateSet} WHERE Id_person = {id}";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return Ok("User updated successfully");
                    }
                    else
                    {
                        return NotFound("User not found");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in UpdateUser: " + ex.Message);
                    return StatusCode(500, "Error updating user");
                }
            }
        }

        [HttpPost]
        [Route("users")]
        public IActionResult CreateUser([FromBody] NewUserDTO newUser)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = $"INSERT INTO maxiusers.person (type_user_Id_type_user, Name, Address, Mail, UserName, Password, Age, Img) VALUES (@Id_type, @Name, @Address, @Email, @UserName, @Password, @Age, @Img)";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Id_type", newUser.Id_type);
                    cmd.Parameters.AddWithValue("@Name", newUser.Name);
                    cmd.Parameters.AddWithValue("@Address", newUser.Address);
                    cmd.Parameters.AddWithValue("@Email", newUser.Email);
                    cmd.Parameters.AddWithValue("@UserName", newUser.UserName);
                    cmd.Parameters.AddWithValue("@Password", newUser.Password);
                    cmd.Parameters.AddWithValue("@Age", newUser.Age);
                    cmd.Parameters.AddWithValue("@Img", newUser.Img); // Aquí pasamos la imagen como bytes
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return Ok("User created successfully");
                    }
                    else
                    {
                        return StatusCode(500, "Error creating user");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in CreateUser: " + ex.Message);
                    return StatusCode(500, "Error creating user");
                }
            }
        }

        [HttpDelete]
        [Route("users/{id}")]
        public IActionResult DeleteUser(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = $"DELETE FROM maxiusers.person WHERE Id_person = {id}";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return Ok("User deleted successfully");
                    }
                    else
                    {
                        return NotFound("User not found");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in DeleteUser: " + ex.Message);
                    return StatusCode(500, "Error deleting user");
                }
            }
        }
    }
}


