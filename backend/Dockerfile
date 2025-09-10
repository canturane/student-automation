
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app


COPY . .


RUN dotnet publish ./src/StudentApi/StudentApi.csproj -c Release -o /out


FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /out .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "StudentApi.dll"]
