using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using pruebaAPI.DTO;
using static Dapper.SqlBuilder;

namespace pruebaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogController : ControllerBase
    {
        private readonly IConfiguration _config;

        public LogController(IConfiguration config)
        {
            _config = config;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchLogs(
            [FromQuery] string? logLevel,
            [FromQuery] string? source,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate
        )
        {
            var builder = new SqlBuilder();
            var template = builder.AddTemplate(@"
                SELECT LogID, LogLevel, LogMessage, LogDate, Source, AdditionalData, [inf extra] AS infoExtra
                FROM Logs /**where**/
                ORDER BY LogDate DESC
            ");

            if (!string.IsNullOrEmpty(logLevel))
                builder.Where("LogLevel = @LogLevel", new { logLevel });

            if (!string.IsNullOrEmpty(source))
                builder.Where("Source = @Source", new { source });

            if (fromDate.HasValue)
                builder.Where("LogDate >= @FromDate", new { FromDate = fromDate });

            if (toDate.HasValue)
                builder.Where("LogDate <= @ToDate", new { ToDate = toDate });

            using var connection = GetConnection();
            //QueryAsync: se usa para ejecutar una consulta que devuelve varios registros, devuelve un enumerable
            var logs = await connection.QueryAsync<LogsDto>(template.RawSql, template.Parameters);

            return Ok(logs);
        }

        [HttpPost("insert")]
        public async Task<IActionResult> CreateLog([FromBody] LogsDto newLog)
        {
            if (newLog == null)
            {
                return BadRequest("El log a insertar no puede ser null");
            }

            // Composición de la consulta SQL para insertar un nuevo log
            var sql = @"
                INSERT INTO Logs (LogLevel, LogMessage, LogDate, Source, AdditionalData, [inf extra])
                VALUES (@LogLevel, @LogMessage, @LogDate, @Source, @AdditionalData, @infoExtra);
                SELECT CAST(SCOPE_IDENTITY() AS INT);  -- Esto retorna el ID del nuevo registro insertado
            ";
            using var connection = GetConnection();
            //ExecuteScalarAsync: se usa cuando ejeecutas una consulta que solo devueelve un solo valor 
            var newId = await connection.ExecuteScalarAsync<int>(sql, newLog);
            //te devuelve un 201
            return CreatedAtAction(nameof(CreateLog), new { id = newId }, newLog);
        }
    }
}
