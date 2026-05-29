using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PIXELAPP_API.model
{
    public class TransaksiModel {
        public string TransactionID { get; set; }
        public string NamaPelanggan { get; set; }
        public string Telpon { get; set; }
        public Decimal Price { get; set; }
        public Decimal DP { get; set; }
        public Decimal Kurang { get; set; }
    }   

    public class GetPrice
    {
        public int ProductID { get; set; }
        public Decimal Quantity { get; set; } 
    }

    public class TransactionDetailModel
    {
        public string Transaction_detailID { get; set; }
        public string TransactionID { get; set; }
        public int ProductID { get; set; }
        public Decimal Quantity { get; set; }
        public Decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }

    //=====================DELIVERY FORM========================
    public class putDelivery
    {
        public string Status { get; set; }
    }
    //=====================DELIVERY FORM========================

}
