# Mergington High School Activities

A full-stack application built with ASP.NET Core and React that allows students to view and sign up for extracurricular activities.

## Technology Stack

- **Backend**: ASP.NET Core with .NET 10 (Minimal APIs)
- **Frontend**: React with KendoReact Free components
- **Development Environment**: VS Code with GitHub Copilot

## Features

- View all available extracurricular activities
- Sign up for activities with validation
- Modern, responsive React UI with KendoReact components
- RESTful API built with ASP.NET Core

## Getting Started

### Prerequisites

- .NET 10 SDK
- Node.js 20+ and npm

### Running the Application

#### Option 1: Full Stack (Production-like)

1. Build the React frontend:
   ```bash
   cd src/client-app
   npm install
   npm run build
   ```

2. Copy the build to wwwroot:
   ```bash
   cd ..
   rm -rf wwwroot
   mkdir wwwroot
   cp -r client-app/build/* wwwroot/
   ```

3. Run the ASP.NET Core backend:
   ```bash
   dotnet run
   ```

4. Open your browser and go to: http://localhost:5000

#### Option 2: Development Mode with Hot Reload

1. Start the ASP.NET Core backend:
   ```bash
   cd src
   dotnet run
   ```

2. In a new terminal, start the React dev server:
   ```bash
   cd src/client-app
   npm install
   npm start
   ```

3. Open your browser and go to: http://localhost:3000

### Using VS Code Debugger

Press F5 in VS Code and select "Launch Full Stack" to start both backend and frontend with debugging enabled.

## API Endpoints

| Method | Endpoint                                | Description                           |
| ------ | --------------------------------------- | ------------------------------------- |
| GET    | `/api/activities`                       | Get all activities with their details |
| POST   | `/api/activities/{activityName}/signup` | Sign up for an activity               |

### Example POST Request

```json
POST /api/activities/Chess%20Club/signup
Content-Type: application/json

{
  "email": "student@mergington.edu"
}
```

## Data Model

The application uses the following models:

1. **Activity** (C# class):
   - Description (string)
   - Schedule (string)
   - MaxParticipants (int)
   - Participants (List<string> of emails)

2. **SignupRequest** (C# class):
   - Email (string)

All data is stored in memory, which means data will be reset when the server restarts.
