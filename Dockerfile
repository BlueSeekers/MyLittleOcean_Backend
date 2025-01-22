FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# 빌드 구성 강제 설정 (Debug)
WORKDIR /src
COPY ["mylio.csproj", "."]
# 패키지 복원
RUN dotnet restore "./mylio.csproj"
COPY . .
WORKDIR "/src/."
# 빌드 실행 (강제 Debug)
RUN dotnet build "./mylio.csproj" -c Debug -o /app/build
# 앱 배포를 위한 Publish 단계
FROM build AS publish
# 빌드 구성 강제 설정 (Debug)
RUN dotnet publish "./mylio.csproj" -c Debug -o /app/publish /p:UseAppHost=false

# 최종 실행 이미지 설정
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# ASPNETCORE_ENVIRONMENT 설정 (Development)
ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "mylio.dll"]