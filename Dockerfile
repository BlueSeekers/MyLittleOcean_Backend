FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# ȯ�� ���� �⺻�� ���� (���� �� ���� �� ����ȭ��)
ENV BUILD_CONFIGURATION=Debug
ENV ASPNETCORE_ENVIRONMENT=Development

# ARG ���� (ENV ���� ����Ͽ� ����ȭ)
ARG BUILD_CONFIGURATION=$BUILD_CONFIGURATION
ARG ASPNETCORE_ENVIRONMENT=$ASPNETCORE_ENVIRONMENT

WORKDIR /src
COPY ["mylio.csproj", "."]
# ��Ű�� ����
RUN dotnet restore "./mylio.csproj"
COPY . .
WORKDIR "/src/."
# ���� ����
RUN dotnet build "./mylio.csproj" -c $BUILD_CONFIGURATION -o /app/build
# �� ������ ���� Publish �ܰ�
FROM build AS publish
# ENV�� ���� ARG ���� ����
ARG BUILD_CONFIGURATION=$BUILD_CONFIGURATION
ARG ASPNETCORE_ENVIRONMENT=$ASPNETCORE_ENVIRONMENT
RUN dotnet publish "./mylio.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ���� ���� �̹��� ����
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# ���� �� ȯ�� ����
ENV BUILD_CONFIGURATION=$BUILD_CONFIGURATION
ENV ASPNETCORE_ENVIRONMENT=$ASPNETCORE_ENVIRONMENT
ENTRYPOINT ["dotnet", "mylio.dll"]