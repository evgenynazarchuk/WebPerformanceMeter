#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["WebPerformanceMeterLogServer/WebPerformanceMeterLogServer.csproj", "WebPerformanceMeterLogServer/"]
RUN dotnet restore "WebPerformanceMeterLogServer/WebPerformanceMeterLogServer.csproj"
COPY . .
WORKDIR "/src/WebPerformanceMeterLogServer"
RUN dotnet build "WebPerformanceMeterLogServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebPerformanceMeterLogServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebPerformanceMeterLogServer.dll"]