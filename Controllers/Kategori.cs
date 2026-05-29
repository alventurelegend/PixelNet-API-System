using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using PIXELAPP_API.database;

namespace PIXELAPP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Kategori : ControllerBase
    {

        //=========================== KONEKSI KE DATABASE==================================
        private string conn = DatabaseConfig.ConnectionString;//=
        //=================================================================================

        [HttpGet]
        public IActionResult KategoriGet()
        {
            using (MySqlConnection Koneksi = new MySqlConnection(conn))
            {
                Koneksi.Open();
                string query = "SELECT * FROM kategori";
                var list = new List<object>();
                using (MySqlCommand cmd = new MySqlCommand(query, Koneksi))
                using (MySqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(new
                        {
                            KategoriID = rd["KategoriID"],
                            Name = rd["Name"]             
                        });
                    }
                }
                return Ok(list);
            }
        }
    }
}
