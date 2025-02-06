public static class UserModule {

    public static IServiceCollection AddUserModule(this IServiceCollection services, string connectionString) {

        // QueryLogger 등록
        services.AddScoped<QueryLogger>();

        // Repository 등록
        services.AddScoped <IUserInfoRepository>(provider => {
            var queryLogger = provider.GetRequiredService<QueryLogger>();
            return new UserInfoRepository(connectionString, queryLogger);
            });
        services.AddScoped<IUserDataRepository>(provider => {
            var queryLogger = provider.GetRequiredService<QueryLogger>();
            return new UserDataRepository(connectionString, queryLogger);
            });

        //Service 등록
        services.AddScoped<IUserInfoService, UserInfoService>();
        services.AddScoped<IUserDataService, UserDataService>();

        return services;
    }
}
