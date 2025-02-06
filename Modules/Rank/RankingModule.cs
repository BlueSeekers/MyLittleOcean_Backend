public static class RankingModule {
    public static IServiceCollection AddRankingModule(this IServiceCollection services, string connectionString) {
        // QueryLogger 등록
        services.AddScoped<QueryLogger>();

        // Repository 등록
        services.AddScoped<IRankingRepository>(provider => {
            var queryLogger = provider.GetRequiredService<QueryLogger>();
            return new RankingRepository(connectionString, queryLogger);
        });

        // Service 등록
        services.AddScoped<IRankingService, RankingService>();

        return services;
    }
}