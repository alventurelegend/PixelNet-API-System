using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PIXELAPP_API.model
{
  public class MutasiClass
    {
        public int Nominal { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }

    public class GetMutasiClass
    {
        public int MutasiID { get; set; }
        public int Nominal { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
        public DateTime UpdateAt { get; set; }
    }

    public class UpdateMutasiClass
    {
        public int Nominal { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
