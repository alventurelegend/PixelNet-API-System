using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PIXELAPP_API.model
{
    public class User {
        public string Name { get; set; }
        public string Role { get; set; }
        public string Alamat { get; set; }
        public string Username  {get; set; }
        public string Password { get; set; }
    }

    public class DeleteID {
    public int Id { get ; set; }
    }

    public class UpdateUser {
        public string Name { get; set; }
        public string Role { get; set; }
        public string Alamat { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        
    }


}
