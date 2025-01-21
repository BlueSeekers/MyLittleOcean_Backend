FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# 환경 변수 기본값 설정 (빌드 및 실행 시 동기화됨)
ENV BUILD_CONFIGURATION=Debug
ENV ASPNETCORE_ENVIRONMENT=Development

# ARG 설정 (ENV 값을 사용하여 동기화)
ARG BUILD_CONFIGURATION=$BUILD_CONFIGURATION
ARG ASPNETCORE_ENVIRONMENT=$ASPNETCORE_ENVIRONMENT

WORKDIR /src
COPY ["mylio.csproj", "."]
# 패키지 복원
RUN dotnet restore "./mylio.csproj"
COPY . .
WORKDIR "/src/."
# 빌드 실행
RUN dotnet build "./mylio.csproj" -c $BUILD_CONFIGURATION -o /app/build
# 앱 배포를 위한 Publish 단계
FROM build AS publish
# ENV를 통해 ARG 값을 설정
ARG BUILD_CONFIGURATION=$BUILD_CONFIGURATION
ARG ASPNETCORE_ENVIRONMENT=$ASPNETCORE_ENVIRONMENT
RUN dotnet publish "./mylio.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 최종 실행 이미지 설정
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# 실행 시 환경 변수
ENV BUILD_CONFIGURATION=$BUILD_CONFIGURATION
ENV ASPNETCORE_ENVIRONMENT=$ASPNETCORE_ENVIRONMENT
ENTRYPOINT ["dotnet", "mylio.dll"]