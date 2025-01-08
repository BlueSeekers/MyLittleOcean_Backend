public static class AuthModule {

    public static IServiceCollection AddUserModule(this IServiceCollection services, string connectionString) {
        // Repository 등록
        services.AddScoped<IAuthRepository>(provider => new AuthRepository(connectionString));

        // Service 등록
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}