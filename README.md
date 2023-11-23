# DDES

## Building the Projects

### Prerequisites

- [.NET 7 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-7.0.404-windows-x64-installer)
- dotnet MAUI workload, this can be installed using the command `dotnet workload install maui`.
- An IDE of your choice (Visual Studio 2022 or Rider)

### Building and Running The Server

1. Open a terminal.
2. Navigate to the `DDES.Server` folder.
3. Run the command `dotnet restore` to restore dependencies.
4. Run the command `dotnet build` to build the project binaries.
5. Run the command `dotnet run` to run the project.

### Building and running the Application

1. Open an IDE of your choice.
2. Set the startup project to be `DDES.Application`.
3. Run the application.

## Using the App

- Make sure the server is started before the application.
- Credentials for the supplier are as follows; Username: supplier, Password: Password123!
- Credentials for the customer are as follows; Username: customer, Password: Password123!
- Mock data loaded into the server can be at `/DDES.Server/Data`.

