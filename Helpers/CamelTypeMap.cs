using Dapper;
using System.Reflection;
using System.Text.RegularExpressions;

public class CamelTypeMap : SqlMapper.ITypeMap {
    private readonly SqlMapper.ITypeMap _defaultTypeMap;

    public CamelTypeMap(Type type) {
        _defaultTypeMap = new DefaultTypeMap(type);
    }

    public ConstructorInfo FindConstructor(string[] names, Type[] types) {
        return _defaultTypeMap.FindConstructor(names, types);
    }

    public SqlMapper.IMemberMap GetConstructorParameter(ConstructorInfo constructor, string columnName) {
        return _defaultTypeMap.GetConstructorParameter(constructor, columnName);
    }

    public SqlMapper.IMemberMap GetMember(string columnName) {
        // Convert snake_case to CamelCase
        var propertyName = Regex.Replace(columnName, "_([a-z])", match => match.Groups[1].Value.ToUpper());

        // Try to find the property using the converted name
        var member = _defaultTypeMap.GetMember(propertyName);

        if (member == null) {
            // Fallback to default behavior
            member = _defaultTypeMap.GetMember(columnName);
        }

        return member;
    }

    public ConstructorInfo FindExplicitConstructor() {
        return _defaultTypeMap.FindExplicitConstructor();
    }
}
