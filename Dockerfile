FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# ���� ���� ���� ���� (Debug)
WORKDIR /src
COPY ["mylio.csproj", "."]
# ��Ű�� ����
RUN dotnet restore "./mylio.csproj"
COPY . .
WORKDIR "/src/."
# ���� ���� (���� Debug)
RUN dotnet build "./mylio.csproj" -c Debug -o /app/build
# �� ������ ���� Publish �ܰ�
FROM build AS publish
# ���� ���� ���� ���� (Debug)
RUN dotnet publish "./mylio.csproj" -c Debug -o /app/publish /p:UseAppHost=false

# ���� ���� �̹��� ����
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# ASPNETCORE_ENVIRONMENT ���� (Development)
ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "mylio.dll"]