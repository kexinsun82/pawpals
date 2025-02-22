# PawPals ASP.NET Passion Project

## Overview
PawPals is a web application built using ASP.NET Core and Entity Framework that allows admins to manage connections between members and their pets. The application includes CRUD (Create, Read, Update, Delete) functionalities for members, pets, and connections.

## Features
### Member Page
- **Index**: Displays a list of all members.
- **Create**: Adds a new member.
- **Details**: Shows detailed information about a specific member.
- **Edit**: Updates member information.
- **Delete**: Deletes an existing member after confirmation.

### Pet Page
- **List (Index)**: Displays all pets.
- **Create**: Adds a new pet.
- **Details**: Shows detailed information about a specific pet.
- **Edit**: Updates pet information.
- **Delete**: Deletes an existing pet after confirmation.

### Connection Page
- **Index**: Displays a list of all connections.
- **Create**: Adds a new connection.
- **Delete**: Deletes an existing connection after confirmation.

## Prerequisites
Before running this project, ensure you have the following installed:
- .NET 6 or later
- Visual Studio 2022 or later / VS Code with C# extension
- SQL Server (LocalDB or any preferred database)

## How to Run the Project
1. **Clone the Repository**
   ```sh
   git clone <repository-url>
   cd pawpals
   ```

2. **Setup the Database**
   - Update `appsettings.json` with your database connection string.
   - Run migrations and update the database:
     ```sh
     dotnet ef database update
     ```

3. **Run the Application**
   ```sh
   dotnet run
   ```
   - Alternatively, open the project in Visual Studio and press `Ctrl + F5`.

4. **Access the Web App**
   - Navigate to `http://localhost:5280` in your browser.

## Technologies Used
- **Backend:** ASP.NET Core MVC, Entity Framework Core
- **Frontend:** Razor Views, Bootstrap
- **Database:** SQL Server
- **Tools:** VS Code

## Contributing
Feel free to submit pull requests or report issues if you find any bugs.
