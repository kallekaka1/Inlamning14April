FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["MorMorsBageruMVC.csproj", "./"]
RUN dotnet restore "MorMorsBageruMVC.csproj"

COPY . .
RUN dotnet publish "MorMorsBageruMVC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:5000

ENTRYPOINT ["dotnet", "MorMorsBageruMVC.dll"]