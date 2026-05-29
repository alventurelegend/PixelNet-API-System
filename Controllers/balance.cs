using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using PIXELAPP_API.database;
using PIXELAPP_API.model;

namespace PIXELAPP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class balance : ControllerBase
    {
        //=========================== KONEKSI KE DATABASE==================================
        private string conn = DatabaseConfig.ConnectionString;//=
        //=================================================================================


        [HttpGet]
        public IActionResult GetDelivery()
        {
            using (MySqlConnection Koneksi = new MySqlConnection(conn))
            {
                Koneksi.Open();
                string queryDelivery = "SELECT MutasiID, Nominal, Action, Description, CreatedAt, UpdateAt FROM balance";
                var list = new List<object>();
                using (MySqlCommand cmd = new MySqlCommand(queryDelivery, Koneksi))
                {
                    using (MySqlDataReader Rd = cmd.ExecuteReader())
                    {

                        while (Rd.Read())
                        {
                            list.Add(new
                            {
                                MutasiID = Rd["MutasiID"],
                                Nominal = Rd["Nominal"],
                                Action = Rd["Action"],
                                Description = Rd["Description"],
                                UpdateAt = Rd["UpdateAt"]
                            });
                        }
                        return Ok(list);
                    }
                }
            }
        }

        //INSERT BALANCE MUTASI
        [HttpPost]
        public IActionResult InsertMutasi([FromBody] MutasiClass mutasi)
        {
            using (MySqlConnection Koneksi = new MySqlConnection(conn))
            {
                try
                {
                    Koneksi.Open();

                    string queryInsert = @"INSERT INTO balance
                                   (Nominal, Action, Description, CreatedAt, UpdateAt)
                                   VALUES
                                   (@Nominal, @Action, @Description, @CreatedAt, @UpdateAt)";

                    using (MySqlCommand cmd = new MySqlCommand(queryInsert, Koneksi))
                    {
                        cmd.Parameters.AddWithValue("@Nominal", mutasi.Nominal);
                        cmd.Parameters.AddWithValue("@Action", mutasi.Action);
                        cmd.Parameters.AddWithValue("@Description", mutasi.Description);
                        cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                        cmd.Parameters.AddWithValue("@UpdateAt", DateTime.Now);

                        cmd.ExecuteNonQuery();

                        return Ok(new
                        {
                            message = "Mutasi berhasil ditambahkan"
                        });
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new
                    {
                        message = "Terjadi kesalahan",
                        error = ex.Message
                    });
                }
            }
        }

        //DELETE
        [HttpDelete("{MutasiID}")]
        public IActionResult delete(int MutasiID)
        {
            using (MySqlConnection Koneksi = new MySqlConnection(conn))
            {
                Koneksi.Open();
                string queryDelete = "DELETE FROM balance WHERE MutasiID = @id";
                using (MySqlCommand cmd = new MySqlCommand(queryDelete, Koneksi))
                {
                    cmd.Parameters.AddWithValue("@id", MutasiID);
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        return Ok(new
                        {
                            message = "Berhasil Menghapus Data"
                        });
                    } else
                    {
                        return NotFound(new
                        {
                            message = "Id Mutasi tidak ditemukan"
                        });
                    }
                }
            }
        }
    }
}
