using System.ComponentModel.DataAnnotations.Schema;
namespace pruebaAPI.DTO
{
    public class LogsDto
    {
        public int LogID { get; set; }
        public string LogLevel { get; set;}
        public string LogMessage { get; set; }
        public DateTime LogDate { get; set; }
        public string Source { get; set; }
        public string AdditionalData { get; set; }
        public string infoExtra { get; set; }
        

    }
}
