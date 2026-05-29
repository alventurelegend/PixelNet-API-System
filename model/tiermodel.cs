using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PIXELAPP_API.model
{
  public class tierPost
    {
        public int ProductID { get; set; }
        public int minQty { get; set; }
        public int maxQty { get; set; }
        public int Modal { get; set;}
        public int Price { get; set; }
    }
}
