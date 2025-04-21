# Use SDK image — contains everything you need to build and run
FROM mcr.microsoft.com/dotnet/sdk:8.0

WORKDIR /app

# Copy everything and restore/build
COPY . .
RUN dotnet restore "./OrderApi.csproj"
RUN dotnet publish "./OrderApi.csproj" -c Release -o /app/publish

# Set working directory to published output
WORKDIR /app/publish

# Expose port your app listens on
EXPOSE 8080

# Start the app
ENTRYPOINT ["dotnet", "OrderApi.dll"]