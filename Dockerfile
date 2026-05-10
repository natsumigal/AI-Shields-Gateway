FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY AiGovernance.sln ./
COPY Directory.Build.props ./
COPY src/AiGovernance.Core/AiGovernance.Core.csproj src/AiGovernance.Core/
COPY src/AiGovernance.Infrastructure/AiGovernance.Infrastructure.csproj src/AiGovernance.Infrastructure/
COPY src/AiGovernance.Api/AiGovernance.Api.csproj src/AiGovernance.Api/

RUN dotnet restore AiGovernance.sln

COPY . .
RUN dotnet publish src/AiGovernance.Api/AiGovernance.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "AiGovernance.Api.dll"]
