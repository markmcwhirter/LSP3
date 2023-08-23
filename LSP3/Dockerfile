FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["LSP3/LSP3.csproj", "LSP3/"]
RUN dotnet restore "LSP3/LSP3.csproj"
COPY . .
WORKDIR "/src/LSP3"
RUN dotnet build "LSP3.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LSP3.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LSP3.dll"]
