using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using API_Users_Management.Resources;
using MaxiUsers.Resource;

namespace API_Users_Management.Controllers
{
    [ApiController]
    [Route("api")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Route("users")]
        public IActionResult ListUsers()
        {
            List<Parameter> parameters = new List<Parameter>();

            DataTable users = DbData.List("GetAllUsers", parameters);
            
            if (users != null)
            {
                var userList = new List<object>();
                foreach (DataRow row in users.Rows)
                {    
                    var user = new
                    {
                        Id_person = row["Id_person"],
                        Id_type = row["type_user_Id_type_user"], // Aquí accede al tipo de usuario correcto
                        Name = row["Name"],
                        Address = row["Address"],
                        Mail = row["Mail"]
                    };
                    userList.Add(user);
                }

                string jsonUsers = JsonConvert.SerializeObject(userList);

                return Ok(jsonUsers); 
            }
            else
            {
                return StatusCode(500, "Error retrieving users"); 
            }
        }


        [HttpGet]
        [Route("users/{id}")]
        public IActionResult GetUserById(int id)
        {
            Console.WriteLine("Received ID: " + id);

            Parameter idParameter = new Parameter("p_Id_person", id.ToString());
            List<Parameter> parameters = new List<Parameter>() { idParameter };
            DataTable user = DbData.List("ReadUserById", parameters);

            if (user != null && user.Rows.Count > 0)
            {
                var foundUser = new
                {
                    Id_person = user.Rows[0]["Id_person"],
                    Id_type = user.Rows[0]["type_user_Id_type_user"],
                    Name = user.Rows[0]["Name"],
                    Address = user.Rows[0]["Address"],
                    Mail = user.Rows[0]["Mail"]
                };

                string jsonUser = JsonConvert.SerializeObject(foundUser);

                return Ok(jsonUser);
            }
            else
            {
                return NotFound("User not found");
            }
        }

        [HttpPatch]
        [Route("users/{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UpdatedUserDTO updatedUser)
        {
            Parameter idParameter = new Parameter("p_Id_person", id.ToString());
            Parameter typeParameter = new Parameter("p_Id_type", updatedUser.IdType.ToString());
            Parameter nameParameter = new Parameter("p_Name", updatedUser.Name);
            Parameter addressParameter = new Parameter("p_Address", updatedUser.Address);
            Parameter mailParameter = new Parameter("p_Mail", updatedUser.Email);

            List<Parameter> parameters = new List<Parameter>() 
            { 
                idParameter,
                typeParameter,
                nameParameter,
                addressParameter,
                mailParameter
            };

            bool updateSuccess = DbData.Update("UpdateUserById", parameters);

            if (updateSuccess)
            {
                return Ok("User updated successfully");
            }
            else
            {
                return StatusCode(500, "Error updating user");
            }
        }


        [HttpPost]
        [Route("users")]
        public IActionResult CreateUser([FromBody] NewUserDTO newUser)
        {
            Parameter typeParameter = new Parameter("p_Id_type", newUser.Id_type.ToString());
            Parameter nameParameter = new Parameter("p_Name", newUser.Name);
            Parameter addressParameter = new Parameter("p_Address", newUser.Address);
            Parameter mailParameter = new Parameter("p_Mail", newUser.Email);

            List<Parameter> parameters = new List<Parameter>() 
            { 
                typeParameter,
                nameParameter,
                addressParameter,
                mailParameter
            };

            bool creationSuccess = DbData.Create("CreateUser", parameters);

            if (creationSuccess)
            {
                return Ok("User created successfully");
            }
            else
            {
                return StatusCode(500, "Error creating user");
            }
        }

        [HttpDelete]
        [Route("users/{id}")]
        public IActionResult DeleteUser(int id)
        {
            Parameter idParameter = new Parameter("p_Id_person", id.ToString());
            List<Parameter> parameters = new List<Parameter>() { idParameter };

            bool deletionSuccess = DbData.Delete("DeleteUserById", parameters);

            if (deletionSuccess)
            {
                return Ok("User deleted successfully");
            }
            else
            {
                return StatusCode(500, "Error deleting user");
            }
        }
    }
}
