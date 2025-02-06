using Dapper;
using MySqlConnector;
using System.Data;
using System.Text.RegularExpressions;

public class QueryLogger {
    private readonly string _connectionString;
    private readonly ILogger<QueryLogger> _logger;

    public QueryLogger(IConfiguration configuration, ILogger<QueryLogger> logger) {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
        _logger = logger;
    }

    public async Task<int> ExecuteAsync(string sql, object param = null) {
        string loggedSql = FormatSqlWithParams(sql, param);
        _logger.LogInformation("Executing SQL: {Query}", loggedSql);

        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            return await db.ExecuteAsync(sql, param);
        }
    }

    public async Task<T> QuerySingleAsync<T>(string sql, object param = null) {
        string loggedSql = FormatSqlWithParams(sql, param);
        _logger.LogInformation("Executing SQL: {Query}", loggedSql);

        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            return await db.QuerySingleOrDefaultAsync<T>(sql, param);
        }
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null) {
        string loggedSql = FormatSqlWithParams(sql, param);
        _logger.LogInformation("Executing SQL: {Query}", loggedSql);

        using (IDbConnection db = new MySqlConnection(_connectionString)) {
            return await db.QueryAsync<T>(sql, param);
        }
    }

    private string FormatSqlWithParams(string sql, object param) {
        if (param == null) return sql;

        var paramDict = param.GetType().GetProperties()
            .ToDictionary(p => "@" + p.Name, p => p.GetValue(param)?.ToString() ?? "NULL");

        foreach (var kvp in paramDict) {
            sql = Regex.Replace(sql, $"\\{kvp.Key}\\b", $"'{kvp.Value}'", RegexOptions.IgnoreCase);
        }

        return sql;
    }
}
