using Dapper.FluentMap.Mapping;
namespace pruebaAPI.DTO
{
    public class LogDtoMapp: EntityMap<LogsDto>
    {
        public LogDtoMapp()
        {
            Map(p => p.LogID).ToColumn("LogID");
            Map(p => p.LogLevel).ToColumn("LogLevel");
            Map(p => p.LogMessage).ToColumn("LogMessage");
            Map(p => p.LogDate).ToColumn("LogDate");
            Map(p => p.Source).ToColumn("Source");
            Map(p => p.AdditionalData).ToColumn("AdditionalData");
            Map(p => p.infoExtra).ToColumn("inf extra");
        }
    }
}
