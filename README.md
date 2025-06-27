# Project Management System

## Overview
A comprehensive project management application built with ASP.NET Core 8.0 and Entity Framework Core, featuring project tracking, team management, task organization, and timeline visualization.

## Features
- **Projects**: Create and manage projects with status tracking
- **Boards**: Kanban-style task organization
- **Teams**: Team creation and member management
- **Tasks/Tickets**: Detailed task management with assignments, tags, and comments
- **Milestones**: Project milestone tracking
- **Timelines**: Project timeline visualization with events
- **Tags**: Task categorization with color-coded labels

## Technology Stack
- **Backend**: ASP.NET Core 9.0 Web API
- **Database**: SQLite
- **ORM**: Entity Framework Core 9.0
- **Documentation**: Swagger/OpenAPI

## Database Schema
The system uses the following main entities:
- Projects (with status tracking)
- Boards (Kanban boards)
- Tasks (with priority, status, assignments)
- Teams & TeamMembers
- Milestones
- Timelines & TimelineEvents
- Tags (with many-to-many relationship to tasks)
- Comments (task discussions)

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- SQL Server Express/LocalDB (2019 or later)
- Visual Studio 2022 or VS Code

### Installation
1. Clone the repository
2. Navigate to the project directory
3. Update the connection string in `appsettings.json` if needed
4. Run the following commands:

```bash
dotnet restore
dotnet ef database update
dotnet run
```

### API Endpoints
The API provides the following endpoints:

**Projects**
- GET /api/projects - Get all projects
- GET /api/projects/{id} - Get project by ID
- POST /api/projects - Create new project
- PUT /api/projects/{id} - Update project
- DELETE /api/projects/{id} - Delete project
- GET /api/projects/status/{status} - Get projects by status

**Tasks**
- GET /api/tasks - Get all tasks
- GET /api/tasks/{id} - Get task by ID
- POST /api/tasks - Create new task
- PUT /api/tasks/{id} - Update task
- DELETE /api/tasks/{id} - Delete task
- GET /api/tasks/board/{boardId} - Get tasks by board
- GET /api/tasks/status/{status} - Get tasks by status
- POST /api/tasks/{taskId}/assign/{memberId} - Assign task to member
- POST /api/tasks/{taskId}/tags/{tagId} - Add tag to task
- DELETE /api/tasks/{taskId}/tags/{tagId} - Remove tag from task

**Teams**
- GET /api/teams - Get all teams
- GET /api/teams/{id} - Get team by ID
- POST /api/teams - Create new team
- PUT /api/teams/{id} - Update team
- DELETE /api/teams/{id} - Delete team
- POST /api/teams/members - Add team member
- DELETE /api/teams/members/{memberId} - Remove team member
- POST /api/teams/{teamId}/projects/{projectId} - Assign team to project

**Boards**
- GET /api/boards - Get all boards
- GET /api/boards/{id} - Get board by ID
- POST /api/boards - Create new board
- PUT /api/boards/{id} - Update board
- DELETE /api/boards/{id} - Delete board
- GET /api/boards/project/{projectId} - Get boards by project

**Milestones**
- GET /api/milestones - Get all milestones
- GET /api/milestones/{id} - Get milestone by ID
- POST /api/milestones - Create new milestone
- PUT /api/milestones/{id} - Update milestone
- DELETE /api/milestones/{id} - Delete milestone
- GET /api/milestones/project/{projectId} - Get milestones by project

**Tags**
- GET /api/tags - Get all tags
- GET /api/tags/{id} - Get tag by ID
- POST /api/tags - Create new tag
- PUT /api/tags/{id} - Update tag
- DELETE /api/tags/{id} - Delete tag

**Timelines**
- GET /api/timelines - Get all timelines
- GET /api/timelines/{id} - Get timeline by ID
- POST /api/timelines - Create new timeline
- PUT /api/timelines/{id} - Update timeline
- DELETE /api/timelines/{id} - Delete timeline
- GET /api/timelines/project/{projectId} - Get timelines by project
- POST /api/timelines/{timelineId}/events - Add timeline event

### Database Setup
The application uses Code First approach with Entity Framework Core. The database will be automatically created when you first run the application.

Default connection string uses LocalDB:
```
Server=(localdb)\\mssqllocaldb;Database=ProjectManagementDB;Trusted_Connection=true;MultipleActiveResultSets=true
```

For SQL Server Express, update to:
```
Server=.\\SQLEXPRESS;Database=ProjectManagementDB;Trusted_Connection=true;MultipleActiveResultSets=true
```

### Swagger Documentation
When running in development mode, visit `https://localhost:5001/swagger` to explore the API documentation and test endpoints.

## External Components
- **Entity Framework Core**: Object-relational mapping
- **Swashbuckle.AspNetCore**: API documentation with Swagger
- **Microsoft.EntityFrameworkCore.SqlServer**: SQL Server provider

## Project Structure
```
ProjectManagement/
├── Controllers/           # API controllers
├── Data/                 # Database context
├── DTOs/                 # Data transfer objects
├── Models/               # Entity models
├── Services/             # Business logic services
├── Program.cs            # Application entry point
├── appsettings.json      # Configuration
└── README.md            # This file
```