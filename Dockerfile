FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# ���� ���� ���� (Release , Debug)  -- prod ������ �� �����ϱ�
ARG BUILD_CONFIGURATION=Debug
# ȯ�� ���� ���� (Development, Production) -- prod ������ �� �����ϱ�
ARG ASPNETCORE_ENVIRONMENT=Development

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
# (Release , Debug)  -- prod ������ �� �����ϱ�
ARG BUILD_CONFIGURATION=Debug
RUN dotnet publish "./mylio.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ���� ���� �̹��� ����
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# ASPNETCORE_ENVIRONMENT (Development)  -- prod ������ �ּ�ó���ϱ�
ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "mylio.dll"]