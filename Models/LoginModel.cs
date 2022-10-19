using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace RuokalistaApp.Models
{
    public class LoginModel
    {
        
        public int id { get; set; }

        
        public string username { get; set; }

        
        public string password { get; set; }
    }
}
