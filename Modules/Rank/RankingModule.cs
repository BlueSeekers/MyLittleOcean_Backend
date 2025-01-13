public static class RankingModule
{
    public static IServiceCollection AddRankingModule(this IServiceCollection services, string connectionString)
    {
        // Repository 등록
        services.AddScoped<IRankingRepository>(provider => new RankingRepository(connectionString));

        // Service 등록
        services.AddScoped<IRankingService, RankingService>();

        return services;
    }
}