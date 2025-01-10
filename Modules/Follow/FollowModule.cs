public static class FollowModule {
    public static IServiceCollection AddFollowModule(this IServiceCollection services, string connectionString) {
        // Repository 등록
        services.AddScoped<IFollowRepository>(provider => new FollowRepository(connectionString));

        // Service 등록
        services.AddScoped<IFollowService, FollowService>();

        return services;
    }
}
