FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# 빌드 구성 설정 (Release , Debug)  -- prod 배포시 꼭 변경하기
ARG BUILD_CONFIGURATION=Debug
# 환경 변수 설정 (Development, Production) -- prod 배포시 꼭 변경하기
ARG ASPNETCORE_ENVIRONMENT=Development

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
# (Release , Debug)  -- prod 배포시 꼭 변경하기
ARG BUILD_CONFIGURATION=Debug
RUN dotnet publish "./mylio.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 최종 실행 이미지 설정
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# ASPNETCORE_ENVIRONMENT (Development)  -- prod 배포시 주석처리하기
ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "mylio.dll"]