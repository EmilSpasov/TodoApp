# TodoApp

## Overview

TodoApp is a simple To-do list management system built with ASP.NET Core and Entity Framework.
It allows users to create, edit, and manage tasks with optional due dates.
The application provides functionality to list pending and overdue tasks and marks tasks as completed.

## Prerequisites

- .NET 8 SDK
- Docker (Docker Desktop, Rancher Desktop, or any other Docker-compatible environment)
- Visual Studio 2022 or Visual Studio Code

## Getting Started

### Clone the Repository

https://github.com/EmilSpasov/TodoApp.git

## Running the Application in Docker

### Build and Start Docker Containers

Navigate to TodoApp.Api where docker-compose.yaml is located

docker-compose up --build

**Access the application:**

Open your browser and navigate to `http://localhost:5000` for the API and `http://localhost:5016/swagger/index.html` for the Swagger UI.

## Project Structure

- `TodoApp.Api`: The main API project
- `TodoApp.Core`: Core business logic and interfaces
- `TodoApp.Infrastructure`: Data access and repository implementations
- `TodoApp.Tests`: Unit tests for the application

## API Endpoints

- **Create Task**: `POST /api/tasks`
- **Edit Task**: `PUT /api/tasks/{id}`
- **Get Task by ID**: `GET /api/tasks/{id}`
- **List Pending Tasks**: `GET /api/tasks/pending`
- **List Overdue Tasks**: `GET /api/tasks/overdue`

## Database Configuration

The application uses SQLite for both local development and deployment in containers. The database file is located in the `data` directory relative to the application's base directory. The connection string is configured in `Program.cs`.

## Importing Postman Collection

1. **Open Postman**.
2. **Click on the "Import" button** in the top left corner.
3. **Select the "File" tab**.
4. **Click on "Upload Files"**.
5. **Select the `TodoApp.postman_collection.json` file** from the `postman` directory.
6. **Click on the "Import" button** to finish importing the collection.

### Stopping the Application

docker-compose down
