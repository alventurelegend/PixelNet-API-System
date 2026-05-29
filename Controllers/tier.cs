using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using PIXELAPP_API.database;
using PIXELAPP_API.model;

namespace PIXELAPP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Tier : ControllerBase

    {
        //=========================== KONEKSI KE DATABASE==================================
        private string conn = DatabaseConfig.ConnectionString;//=
        //=================================================================================

        [HttpPost]
        public IActionResult Posttier(tierPost tier)
        {
            using (MySqlConnection Koneksi = new MySqlConnection(conn)) {
                Koneksi.Open();
                string queryInsert = "INSERT INTO product_tier (ProductID, minQty, maxQty, Modal, Price) VALUES (@product_id, @min, @max, @modal, @price)";
                using (MySqlCommand cmd = new MySqlCommand(queryInsert, Koneksi))
                {
                    cmd.Parameters.AddWithValue("@product_id", tier.ProductID);
                    cmd.Parameters.AddWithValue("@min", tier.minQty);
                    cmd.Parameters.AddWithValue("@max", tier.maxQty);
                    cmd.Parameters.AddWithValue("@modal", tier.Modal);
                    cmd.Parameters.AddWithValue("@price", tier.Price);
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        return Ok("Berhasil Menambahkan Tier");
                    } else
                    {
                        return BadRequest("Gagal Menambahkan Tier");
                    }
                }
            }
        }

        [HttpGet]
        public IActionResult GetTier()
        {
            var list = new List<object>();

            using (MySqlConnection Koneksi = new MySqlConnection(conn)) {
                Koneksi.Open();

                string getQuery = "SELECT t.TierID, p.Name AS NameProduct, k.Name AS kategoriName, t.minQty, t.maxQty, t.Modal, t.Price FROM product_tier t JOIN product p ON t.ProductID = p.ProductID JOIN kategori k ON p.KategoriID = k.KategoriID;";
                using (MySqlCommand CMD = new MySqlCommand(getQuery, Koneksi))
                using (MySqlDataReader Rd = CMD.ExecuteReader())
                {
                    while (Rd.Read())
                    {
                        list.Add(new
                        {
                            TierID = Rd["TierID"],
                            NameProduct = Rd["NameProduct"],
                            KategoriName = Rd["KategoriName"],
                            minQty = Rd["minQty"],
                            maxQty = Rd["maxQty"],
                            Modal = Rd["Modal"],
                            Price = Rd["Price"]
                        });
                    }
                    return Ok(list);
                }

            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using (MySqlConnection Koneksi = new MySqlConnection(conn))
            {
                Koneksi.Open();

                string delQuery = "DELETE FROM product_tier WHERE TierID = @idTier";
                using (MySqlCommand CMD = new MySqlCommand(delQuery, Koneksi)) {
                CMD.Parameters.AddWithValue("@idTier", id);
                int respon = CMD.ExecuteNonQuery();

                if(respon > 0)
                    {
                        return Ok(new { message = "Berhasil Menghapus ProductTier" });
                    } else
                    {
                        return NotFound(new { message = "ID ProductTier tidak ditemukan" });
                    }
                }
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(tierPost post, int id)
        {
            using (MySqlConnection Koneksi = new MySqlConnection(conn))
            {
                Koneksi.Open();

                string updateQuery = "UPDATE product_tier SET ProductID = @idProduct, minQty = @minQty, maxQty = @maxQty, Modal = @modal, Price = @price WHERE TierID = @idTier";
                using (MySqlCommand CMD = new MySqlCommand(updateQuery, Koneksi))
                {
                    CMD.Parameters.AddWithValue("@idTier", id);
                    CMD.Parameters.AddWithValue("@idProduct", post.ProductID);
                    CMD.Parameters.AddWithValue("@minQty", post.minQty);
                    CMD.Parameters.AddWithValue("@maxQty", post.maxQty);
                    CMD.Parameters.AddWithValue("@modal", post.Modal);
                    CMD.Parameters.AddWithValue("@price", post.Price);
                    int respon = CMD.ExecuteNonQuery();

                    if (respon > 0) {
                        return Ok(new { message = "Berhasil Update product Tier" });
                    } else
                    {
                        return NotFound(new { message = "Gagal Update product Tier" });
                    }
                } 
            }
        }
    }
}
