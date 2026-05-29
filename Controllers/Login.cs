using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using PIXELAPP_API.database;
using PIXELAPP_API.model;
namespace PIXELAPP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Login : ControllerBase
    {

        //=========================== KONEKSI KE DATABASE==================================
        private string conn = DatabaseConfig.ConnectionString;//=
        //=================================================================================

        [HttpPost]
        public IActionResult LoginUser(LoginModel post)
        {
           using (MySqlConnection Koneksi = new MySqlConnection(conn))
            {
                Koneksi.Open();

                string loginQuery = "SELECT * FROM users WHERE Username = @user AND Password = @pass";
                using (MySqlCommand CMD = new MySqlCommand(loginQuery, Koneksi))
                {
                    CMD.Parameters.AddWithValue("@user", post.Username);
                    CMD.Parameters.AddWithValue("@pass", (post.Password));
                    using (MySqlDataReader Rd =CMD.ExecuteReader()) {

                       
                        if (Rd.Read())
                        { 
                            return Ok(new
                            {
                                Username = Rd["Username"],
                                Role = Rd["Role"],
                                Message = "Login Berhasil"
                            });

                        } else
                        {
                            return NotFound(new { message = "Login gagal Username / Password Tidak ditemukan"});
                        }
                    }
                    
                }
            }
        }
    }
}
