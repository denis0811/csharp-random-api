# Stage 1: Build the application
# Use the official .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
# This leverages Docker's layer caching to speed up builds if only code changes
COPY RandomNumberApi.csproj ./
RUN dotnet restore

# Copy the rest of the application code
COPY . .

# Publish the application for release
# --no-restore prevents restoring packages again
# -o /app specifies the output directory inside the container
RUN dotnet publish "RandomNumberApi.csproj" -c Release -o /app --no-restore

# Stage 2: Run the application
# Use a smaller, runtime-only image for the final application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy the published application from the build stage
COPY --from=build /app .

# Expose the port your application listens on (ASP.NET Core defaults to 80/443, but 80 is fine for Render's proxy)
EXPOSE 80

# Define the entrypoint for the container
# This command runs your compiled application
ENTRYPOINT ["dotnet", "RandomNumberApi.dll"]
