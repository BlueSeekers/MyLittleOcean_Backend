public static class UserModule {

    public static IServiceCollection AddUserModule(this IServiceCollection services, string connectionString) {
        // Repository 등록
        services.AddScoped <IUserInfoRepository>(provider => new UserInfoRepository(connectionString));
        services.AddScoped<IUserDataRepository>(provider => new UserDataRepository(connectionString));

        //Service 등록
        services.AddScoped<IUserInfoService, UserInfoService>();
        services.AddScoped<IUserDataService, UserDataService>();

        return services;
    }
}
