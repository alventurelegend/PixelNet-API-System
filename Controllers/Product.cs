using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using PIXELAPP_API.database;
using PIXELAPP_API.model;

namespace PIXELAPP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Product : ControllerBase
    {
        //=========================== KONEKSI KE DATABASE==================================
        private string conn = DatabaseConfig.ConnectionString;//=
        //=================================================================================


        [HttpGet]
        public IActionResult GetAll()
        {
            var list = new List<object>();

            using (MySqlConnection Koneksi = new MySqlConnection(conn))
            {
                Koneksi.Open();

                string query = "SELECT p.ProductID, p.Name, k.Name AS KategoriName, p.Vendor FROM product p JOIN kategori k ON p.KategoriID = k.KategoriID;";

                using (MySqlCommand cmd = new MySqlCommand(query, Koneksi))
                using (MySqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(new
                        {
                            ProductID = rd["ProductID"],
                            Name = rd["Name"],
                            KategoriName = rd["KategoriName"],
                            Vendor = rd["Vendor"]
                        });
                    }
                }
            }

            return Ok(list);
        }



        [HttpPost]
        public IActionResult PostProduct(ModelPost post)
        {
            using (MySqlConnection Koneksi = new MySqlConnection(conn))
            {
                Koneksi.Open();
                string insertQuery = "INSERT INTO product (Name, KategoriID, Vendor, CreatedAt, UpdateAt) VALUES (@name, @kategori_id, @vendor, @created_at, @update_at)";

                using (MySqlCommand Cmd = new MySqlCommand(insertQuery, Koneksi))
                {
                    Cmd.Parameters.AddWithValue("@name", post.Name);
                    Cmd.Parameters.AddWithValue("@kategori_id", post.KategoriID);
                    Cmd.Parameters.AddWithValue("@vendor", post.Vendor);
                    Cmd.Parameters.AddWithValue("@created_at", DateTime.Now);
                    Cmd.Parameters.AddWithValue("@update_at", DateTime.Now);
                    int rowsAffected = Cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return Ok(new { message = "Produk berhasil ditambahkan!" });
                    }
                    else
                    {
                        return StatusCode(500, new { message = "Gagal menambahkan produk." });
                    }
                }
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DelProduct(int id)
        {
            using (MySqlConnection Koneksi = new MySqlConnection(conn))
            {
                Koneksi.Open();
                string queryDel = "DELETE FROM product WHERE ProductID = @id";
                using (MySqlCommand cmd = new MySqlCommand(queryDel, Koneksi))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        return Ok(new { message = "Product Berhasil dihapus" });
                    }
                    else
                    {
                        return NotFound(new { message = "Product tidak ditemukan" });
                    }
                }
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, ModelPost update)
        {
            using (MySqlConnection Koneksi = new MySqlConnection(conn))
            {
                Koneksi.Open();
                string queryUpdate = "UPDATE product SET Name = @name, KategoriID = @kategori_id, Vendor = @vendor, UpdateAt = @update_at WHERE ProductID = @id";
                using (MySqlCommand cmd = new MySqlCommand(queryUpdate, Koneksi))
                {
                    cmd.Parameters.AddWithValue("@name", update.Name);
                    cmd.Parameters.AddWithValue("@kategori_id", update.KategoriID);
                    cmd.Parameters.AddWithValue("@vendor", update.Vendor);
                    cmd.Parameters.AddWithValue("@update_at", DateTime.Now);
                    cmd.Parameters.AddWithValue("@id", id);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return Ok(new { message = "Produk berhasil diperbarui!" });
                    }
                    else
                    {
                        return NotFound(new { message = "Produk tidak ditemukan." });
                    }
                }
            }
        }
    }
}
