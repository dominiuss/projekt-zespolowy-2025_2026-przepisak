FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY PrzepisakApi.csproj .
RUN dotnet restore PrzepisakApi.csproj

COPY . .
RUN dotnet publish PrzepisakApi.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app .

EXPOSE 5035

ENTRYPOINT ["dotnet", "PrzepisakApi.dll"]

