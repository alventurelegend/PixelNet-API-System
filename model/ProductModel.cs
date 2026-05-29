using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PIXELAPP_API.model
{
    public class ModelPost { 
        public int ProductID { get; set; }
        public string Name { get; set; }
        public int KategoriID { get; set; }
        public string Vendor { get; set; }
    }
    
}
