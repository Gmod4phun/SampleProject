FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY WebApi.csproj .
RUN dotnet restore "WebApi.csproj"
COPY . .
RUN dotnet publish "WebApi.csproj" -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0 as final
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT [ "dotnet", "WebApi.dll" ]
