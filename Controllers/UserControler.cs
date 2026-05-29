using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using PIXELAPP_API.database;
using PIXELAPP_API.model;

namespace PIXELAPP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {


        //=======================HASH PASSWORD=================================
        private string Hashing(string password)                             //=
        {                                                                   //=
            using (var sha = System.Security.Cryptography.SHA256.Create())  //=
            {                                                               //=
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);   //=
                var hash = sha.ComputeHash(bytes);                          //=
                return Convert.ToBase64String(hash);                        //=
            }                                                               //=
        }                                                                   //=
        //=====================================================================



        //=========================== KONEKSI KE DATABASE==================================
        private string conn = DatabaseConfig.ConnectionString;//=
        //=================================================================================


        //=============================================================INSERT USER=============================================================
        [HttpPost]
        public IActionResult Regis(User kirim)
        {
            using (MySqlConnection Koneksi = new MySqlConnection(conn))
            {
                Koneksi.Open();
                string checkQuery = "SELECT COUNT(*) FROM users WHERE Username = @username";
                string insertQuery = "INSERT INTO users (Name, Role, Alamat, Username, Password, CreatedAt, updateAt) VALUES (@name, @role, @alamat, @username, @password, @created_at, @update_at)";

                using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, Koneksi))
                {
                    checkCmd.Parameters.AddWithValue("@username", kirim.Username.Trim());
                    int userCount = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (userCount > 0)
                    {
                        return BadRequest(new { message = "Username sudah digunakan!" });
                    }
                }

                using (MySqlCommand Cmd = new MySqlCommand(insertQuery, Koneksi))
                {
                    Cmd.Parameters.AddWithValue("@name", kirim.Name);
                    Cmd.Parameters.AddWithValue("@role", kirim.Role);
                    Cmd.Parameters.AddWithValue("@alamat", kirim.Alamat);
                    Cmd.Parameters.AddWithValue("@username", kirim.Username.Trim());
                    Cmd.Parameters.AddWithValue("@password", Hashing(kirim.Password.Trim()));
                    Cmd.Parameters.AddWithValue("@created_at", DateTime.Now);
                    Cmd.Parameters.AddWithValue("@update_at", DateTime.Now);
                    Cmd.ExecuteNonQuery();
                }
            }
            return Ok(new { message = "User Insert Succes" });
        }
        //=============================================================INSERT USER=============================================================

        //=============================================================GET USER================================================================
        [HttpGet]
        public IActionResult GetAll()
        {
            var list = new List<object>();

            using (MySqlConnection Koneksi = new MySqlConnection(conn))
            {
                Koneksi.Open();

                string query = "SELECT *  FROM users";

                using (MySqlCommand cmd = new MySqlCommand(query, Koneksi))
                using (MySqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(new
                        {
                            UserID = rd["UserID"],
                            Name = rd["Name"],
                            Role = rd["Role"],
                            Alamat = rd["Alamat"],
                            Username = rd["Username"],
                            CreatedAt = rd["CreatedAt"],
                            UpdateAt = rd["UpdateAt"]
                        });
                    }
                }
            }

            return Ok(list);
        }
        //=============================================================GET USER================================================================

        //=============================================================DELETE USER=============================================================

        [HttpDelete("{id}")]
        public IActionResult delete(int id)
        {
            using (MySqlConnection Koneksi = new MySqlConnection(conn))
            {
                Koneksi.Open();

                string deleteQUERY = "DELETE FROM users WHERE UserID = @id";
                using (MySqlCommand cmd = new MySqlCommand(deleteQUERY, Koneksi))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return Ok(new { message = "User deleted successfully!" });
                    }
                    else
                    {
                        return NotFound(new { message = "User not found!" });
                    }
                }
            }

        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateUser set)
        {
            using (MySqlConnection Koneksi = new MySqlConnection(conn))
            {
                Koneksi.Open();
                string checkQuery = "SELECT COUNT(*) FROM users WHERE Username = @username AND UserID != @id";
                string UpdateQuery = "UPDATE users SET Name = @name, Role = @role, Alamat = @alamat, Username = @username, Password = @password, updateAt = @update_at WHERE UserID = @id";


                using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, Koneksi))
                {
                    checkCmd.Parameters.AddWithValue("@username", set.Username.Trim());
                    checkCmd.Parameters.AddWithValue("@id", id);
                    int userCount = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (userCount > 0)
                    {
                        return Conflict(new { message = "Username sudah digunakan!" });
                    }
                }

                using (MySqlCommand cmd = new MySqlCommand(UpdateQuery, Koneksi))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@name", set.Name);
                    cmd.Parameters.AddWithValue("@role", set.Role);
                    cmd.Parameters.AddWithValue("@alamat", set.Alamat);
                    cmd.Parameters.AddWithValue("@username", set.Username);
                    cmd.Parameters.AddWithValue("@password", Hashing(set.Password.Trim()));
                    cmd.Parameters.AddWithValue("@update_at", DateTime.Now);
                    
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0 )
                    {
                        return Ok(new { message = "User updated successfully!" });
                    } else
                    {
                        return NotFound(new { message = "User not found!" });
                    }
                }
            }
        }
    }
}
