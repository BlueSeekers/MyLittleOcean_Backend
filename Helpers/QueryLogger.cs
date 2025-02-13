using System.Text.RegularExpressions;

public class QueryLogger {
    private readonly ILogger<QueryLogger> _logger;

    public QueryLogger(IConfiguration configuration, ILogger<QueryLogger> logger) {
        _logger = logger;
    }

    public async Task<int> ExecuteAsync(string sql, object param = null) {
        string loggedSql = FormatSqlWithParams(sql, param);
        _logger.LogInformation("Executing SQL: {Query}", loggedSql);

        // 실제 DB 실행 제거
        await Task.CompletedTask; // 비동기 메서드 유지
        return 0; // 항상 0 리턴
    }

    public async Task<T> QuerySingleAsync<T>(string sql, object param = null) {
        string loggedSql = FormatSqlWithParams(sql, param);
        _logger.LogInformation("Executing SQL: {Query}", loggedSql);

        await Task.CompletedTask; // 비동기 메서드 유지
        return default(T); // 기본값 반환
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null) {
        string loggedSql = FormatSqlWithParams(sql, param);
        _logger.LogInformation("Executing SQL: {Query}", loggedSql);

        await Task.CompletedTask; // 비동기 메서드 유지
        return Enumerable.Empty<T>(); // 빈 컬렉션 반환
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
