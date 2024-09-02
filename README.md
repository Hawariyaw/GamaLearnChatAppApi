# Gama Learn Chat App

Gama Learn Chat App is a real-time chat application built using .NET 8, C#, SignalR for real-time communication, SQLite as the database, JWT for authentication, and HangFire for job queuing and resending failed chats.

## Features

- **Real-time Chat**: Communication powered by SignalR.
- **Authentication**: Secure your APIs using JWT tokens.
- **SQLite Database**: Lightweight and simple data storage.
- **HangFire**: Background job processing, including resending failed chats.
- **Swagger Documentation**: Explore API endpoints easily.
- **HangFire Dashboard**: Monitor and manage background jobs.

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQLite](https://www.sqlite.org/download.html)

### Installation

1. **Clone the repository:**
   ```bash
   git clone https://gitlab.com/your-repo/GamaLearnChatApp.git
   cd GamaLearnChatApp
   ```
2. **Restore the dependencies:**
   ```bash
   dotnet restore
   ```
3. **Apply Migrations:**
   Ensure the ChatApp.db SQLite database is created and updated with the latest migrations.
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```
   This will create the necessary tables in the ChatApp.db SQLite database.
   
### Running the Application Locally
1. **Run the application:**
    ```bash
    dotnet run --project src/GamaLearn.ChatApp
    ```
2. **Access the API documentation:**
   Once the app is running, you can access the Swagger documentation at:
   - Local: http://localhost:5001/swagger/index.html
   - Hosted: https://corechatappapi-b7h4ckdhfnfdczbx.eastus-01.azurewebsites.net/swagger/index.html
3. **Access the HangFire dashboard::**
   - Local: http://localhost:5001/hangfire-dashboard
   - Hosted: https://corechatappapi-b7h4ckdhfnfdczbx.eastus-01.azurewebsites.net/hangfire-dashboard

### Configuration
Ensure your appsettings.json or environment variables are correctly configured for JWT authentication and database connections.
```
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=ChatApp.db"
  },
  "Jwt": {
    "Key": "YourSecretKeyHere",
    "Issuer": "YourIssuer",
    "Audience": "YourAudience"
  },
  "Hangfire": {
    "DashboardPath": "/hangfire-dashboard"
  }
}
```
### Deploying to Azure
Before deploying, ensure that your SQLite database is properly configured. SQLite is suitable for development and small-scale 
deployments, but for larger production environments, consider using a more robust database like SQL Server or PostgreSQL.
1. Build the application:
```
dotnet publish -c Release -o ./publish
```
2. Deploy to Azure:
Use the Azure CLI or Visual Studio to deploy the application to Azure App Service. Ensure that your environment variables are set in the Azure portal, including connection strings and JWT settings.

### License
This project is licensed under the MIT License - see the LICENSE file for details.

### Contact
For any questions or support, please contact the development team.
- hewipoul@gmail.com
- +971554689346
