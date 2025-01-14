public static class AuthModule {

    public static IServiceCollection AddAuthModule(this IServiceCollection services, string connectionString) {
        // Repository 등록
        services.AddScoped<IAuthRepository>(provider => new AuthRepository(connectionString));
        services.AddScoped<IUserRepository>(provider => {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            return new UserRepository(connectionString);
        });

        // Service 등록
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}