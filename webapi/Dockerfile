FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

WORKDIR /src
COPY WebApi.csproj .
RUN dotnet restore "WebApi.csproj"
COPY . .
RUN dotnet publish "WebApi.csproj" -c Release -o /publish


RUN dotnet ef

# RUN rm -rf /Migrations

RUN dotnet ef migrations add InitialCreate
RUN dotnet ef database update

FROM mcr.microsoft.com/dotnet/aspnet:6.0 as final
WORKDIR /app
COPY --from=build /publish .
COPY --from=build /src/employees.db .

EXPOSE 5000 80

ENTRYPOINT [ "dotnet", "WebApi.dll" ]
