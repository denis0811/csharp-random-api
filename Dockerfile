# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY RandomNumberApi.csproj ./
RUN dotnet restore
COPY . .
RUN dotnet publish "RandomNumberApi.csproj" -c Release -o /app

# Stage 2: Run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app .
EXPOSE 80

# This is the corrected entrypoint.
# It uses the dotnet host to run your application correctly.
# This environment variable explicitly tells the ASP.NET Core app to listen on port 80.
ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "RandomNumberApi.dll"]
