using Dapper;

public static class DapperExtensions {
    public static void UseSnakeCaseToCamelCaseMapping() {
        SqlMapper.TypeMapProvider = type => new CamelTypeMap(type);
    }
}
