using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using PIXELAPP_API.database;
using PIXELAPP_API.model;
using System.Security.Cryptography.X509Certificates;

namespace PIXELAPP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Transaction : ControllerBase
    {
        //=========================== KONEKSI KE DATABASE==================================
        private string conn = DatabaseConfig.ConnectionString;//=
        //=================================================================================


        [HttpPost]
        public IActionResult Pesan(TransaksiModel buy)
        {
            using (MySqlConnection Koneksi = new MySqlConnection(conn))
            {
                Koneksi.Open();

                string queryInsert = @"INSERT INTO transaction 
                                     (TransactionID, NamaPelanggan, Telpon, Price, DP, Kurang, Status, CreatedAt, UpdateAt) 
                                     VALUES 
                                     (@ID, @NamaPelanggan, @Telpon, @Price, @DP, @Kurang, @Status, @CreatedAt, @UpdateAt)";
                using (MySqlCommand cmd = new MySqlCommand(queryInsert, Koneksi))
                {
                    cmd.Parameters.AddWithValue("@ID", buy.TransactionID);
                    cmd.Parameters.AddWithValue("@NamaPelanggan", buy.NamaPelanggan);
                    cmd.Parameters.AddWithValue("@Telpon", buy.Telpon);
                    cmd.Parameters.AddWithValue("@Price", buy.Price);
                    cmd.Parameters.AddWithValue("@DP", buy.DP);
                    cmd.Parameters.AddWithValue("@Kurang", buy.Kurang);
                    cmd.Parameters.AddWithValue("@Status", "PROSES");
                    cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UpdateAt", DateTime.Now);
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        return Ok(new { message = "Berhasil Menambahkan Pesanan" });
                    }
                    else
                    {
                        return BadRequest(new { message = "Gagal Menambahkan pesanan" });
                    }
                }
            }
        }

        //DETAIL TRANSAKSI POST
        [HttpPost("detail")]
        public IActionResult DetailPost(TransactionDetailModel buy)
        {
            using (MySqlConnection Koneksi = new MySqlConnection(conn))
            {
                Koneksi.Open();
                string queryInsert = @"INSERT INTO transaction_detail
                      (Transaction_detail_ID, TransactionID, ProductID, Quantity, Price, CreatedAt, UpdateAt)
                      VALUES
                      (@detailId, @TransactionID, @ProductID, @Quantity, @Price, @CreatedAt, @UpdateAt)";
                using (MySqlCommand cmd = new MySqlCommand(queryInsert, Koneksi))
                {
                    cmd.Parameters.AddWithValue("@detailId", buy.Transaction_detailID);
                    cmd.Parameters.AddWithValue("@TransactionID", buy.TransactionID);
                    cmd.Parameters.AddWithValue("@ProductID", buy.ProductID);
                    cmd.Parameters.AddWithValue("@Quantity", buy.Quantity);
                    cmd.Parameters.AddWithValue("@Price", buy.Price);
                    cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UpdateAt", DateTime.Now);
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        return Ok(new { message = "Sukses Menambahkan Detail Transaksi" });
                    }
                    else
                    {
                        return BadRequest(new { message = "Gagal Menambahkan Detail Transaksi" });
                    }
                }
            }
        }

        [HttpGet("Product")]
        public IActionResult Harga()
        {
            using (MySqlConnection Koneksi = new MySqlConnection(conn))
            {
                Koneksi.Open();
                var list = new List<object>();

                string queryGet = "SELECT p.ProductID AS ProductID, p.Name AS ProductName FROM product p JOIN product_tier t ON p.ProductID = t.ProductID GROUP BY p.ProductID";
                using (MySqlCommand cmd = new MySqlCommand(queryGet, Koneksi))
                {
                    using MySqlDataReader Rd = cmd.ExecuteReader();

                    while (Rd.Read())
                    {
                        list.Add(new
                        {
                            ProductID = Rd["ProductID"],
                            ProductName = Rd["ProductName"]
                        });
                    }

                    return Ok(list);
                }
            }
        }

        [HttpPost("Harga")]
        public IActionResult Qty(GetPrice get)
        {
            using (MySqlConnection Koneksi = new MySqlConnection(conn))
            {
                Koneksi.Open();

                string queryGet = "SELECT Price FROM product_tier WHERE ProductID = @idProduct AND @qty BETWEEN minQty AND maxQty";
                using (MySqlCommand cmd = new MySqlCommand(queryGet, Koneksi))
                {
                    cmd.Parameters.AddWithValue("@idProduct", get.ProductID);
                    cmd.Parameters.AddWithValue("@qty", get.Quantity);
                    using (MySqlDataReader Rd = cmd.ExecuteReader())
                    {
                        if (Rd.Read())
                        {
                            return Ok(new
                            {
                                Price = Rd["Price"]
                            });

                        }
                        else
                        {
                            return NotFound(new { message = "Data Product Tidak Ditemukan" });
                        }
                    }
                }
            }
        }

        [HttpGet("transaksi")]
        public IActionResult get()
        {
            using (MySqlConnection Koneksi = new MySqlConnection(conn))
            {
                Koneksi.Open();
                var List = new List<object>();
                string queryGet = "SELECT * FROM transaction";
                using (MySqlCommand cmd = new MySqlCommand(queryGet, Koneksi))
                {
                    using (MySqlDataReader Rd = cmd.ExecuteReader())
                    {
                        while (Rd.Read())
                        {
                            List.Add(new
                            {
                                TransactionID = Rd["TransactionID"],
                                NamaPelanggan = Rd["NamaPelanggan"],
                                Telpon = Rd["Telpon"],
                                Price = Rd["Price"],
                                DP = Rd["DP"],
                                Kurang = Rd["Kurang"],
                                Status = Rd["Status"],
                                CreatedAt = Rd["CreatedAt"],
                                UpdateAt = Rd["UpdateAt"]
                            });
                        }
                        return Ok(List);
                    }
                }
            }
        }


        //==============================DELIVERY API==============================
        [HttpGet("detail/{id}")]
        public IActionResult get(string id)
        {
            using (MySqlConnection koneksi = new MySqlConnection(conn))
            {
                koneksi.Open();
                string query = "SELECT p.Name, d.Quantity FROM transaction_detail d JOIN product p ON p.ProductID = d.ProductID WHERE d.TransactionID = @id";
                var list = new List<object>();  

                using (MySqlCommand cmd = new MySqlCommand(query, koneksi))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (MySqlDataReader Rd = cmd.ExecuteReader())
                    {
                        while (Rd.Read())
                        {
                            list.Add(new
                            {
                                Name = Rd["Name"],
                                Quantity = Rd["Quantity"]
                            });
                        }
                        return Ok(list);
                    }
                }
            }
        }

        [HttpPut("delivery/{id}")]
        public IActionResult put(putDelivery edit,string id) {
            using (MySqlConnection Koneksi = new MySqlConnection(conn)) { 
            Koneksi.Open();
                string queryPut = "UPDATE transaction SET Status = @status WHERE TransactionID = @id";
                using (MySqlCommand cmd = new MySqlCommand(queryPut, Koneksi))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@status", edit.Status);
                    int result = cmd.ExecuteNonQuery();
                    if (result > 0) {
                        return Ok(new { message = "Berhasil Update Status" });
                    } else
                    {
                        return NotFound(new { message = "Gagal Update Status" });
                    }
                }
            }
        }

        [HttpDelete("transaksi/{id}")]
        public IActionResult delete(string id)
        {
            using (MySqlConnection Koneksi = new MySqlConnection(conn))
            {
                Koneksi.Open();
                string queryDelete = "DELETE FROM transaction WHERE TransactionID = @id";
                using (MySqlCommand cmd = new MySqlCommand(queryDelete, Koneksi))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        return Ok(new { message = "Berhasil Menghapus Transaksi" });
                    }
                    else
                    {
                        return NotFound(new { message = "Gagal Menghapus Transaksi" });
                    }
                }
            }
        }
        }
}
    