using System.Text.Json.Serialization;

namespace RuokalistaApp.Models
{
    public class Ruokalista
    {
        [JsonIgnore]
        public int Id { get; set; }
        public int WeekId { get; set; }
        public int Year { get; set; }
        public string Maanantai { get; set; }
        public string Tiistai { get; set; }
        public string Keskiviikko { get; set; }
        public string Torstai { get; set; }
        public string Perjantai { get; set; }
    }
}
