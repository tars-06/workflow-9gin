# Configurable Workflow Engine

A minimal backend service built with **.NET 8** to manage configurable, state-machine-based workflows.  
This project was developed as part of the **Infonetica Software Engineer Intern** take-home assignment.

---

## 🚀 Quick Start

### ✅ Requirements
- [`.NET 8 SDK`](https://dotnet.microsoft.com/en-us/download)
- [`jq`](https://stedolan.github.io/jq/) – for parsing JSON in the test script

---

### ▶️ Running the App

1. Clone this repository and navigate into it:

   ```bash
   git clone https://github.com/tars-06/workflow-9gin.git
   cd WorkflowEngine
   ```

2. Restore packages and launch the application:

   ```bash
   dotnet restore
   dotnet run
   ```

Once running, you can access the API at:  
`http://localhost:5189`

Swagger UI is also available at:  
`http://localhost:5189/swagger`

---

### 🧪 Running Tests

Make sure the test script is executable, then run it:

```bash
chmod +x testScript.sh
./testScript.sh
```

It covers end-to-end scenarios like workflow creation, transitions, and error handling.

---

## 🌐 API Overview

| Method | Endpoint                                           | Description                          |
|--------|----------------------------------------------------|--------------------------------------|
| POST   | `/api/workflow-definitions`                        | Create a new workflow definition     |
| GET    | `/api/workflow-definitions/{id}`                   | Fetch a specific workflow definition |
| GET    | `/api/workflow-definitions`                        | List all available definitions       |
| POST   | `/api/workflow-instances`                          | Start a new workflow instance        |
| GET    | `/api/workflow-instances/{id}`                     | Get instance details and history     |
| POST   | `/api/workflow-instances/{id}/execute`             | Execute an action on an instance     |
| GET    | `/api/workflow-instances/{id}/available-actions`   | List actions available at this state |
| GET    | `/api/workflow-instances/{id}/current-state`       | Get the current state of the instance|

---

## 📦 Sample Workflow Definition

Here’s a basic example of a workflow definition you can send via the API:

```json
{
  "name": "Order Processing Workflow",
  "states": [
    {"id": "draft", "name": "Draft", "isInitial": true, "isFinal": false, "enabled": true},
    {"id": "submitted", "name": "Submitted", "isInitial": false, "isFinal": false, "enabled": true},
    {"id": "processing", "name": "Processing", "isInitial": false, "isFinal": false, "enabled": true},
    {"id": "completed", "name": "Completed", "isInitial": false, "isFinal": true, "enabled": true}
  ],
  "actions": [
    {"id": "submit", "name": "Submit Order", "enabled": true, "fromStates": ["draft"], "toState": "submitted"},
    {"id": "process", "name": "Process Order", "enabled": true, "fromStates": ["submitted"], "toState": "processing"},
    {"id": "complete", "name": "Complete Order", "enabled": true, "fromStates": ["processing"], "toState": "completed"}
  ]
}
```

---

## 🧱 Project Structure

```
WorkflowEngine/
├── Controllers/   # HTTP API endpoints
├── Models/        # Core domain objects
├── Services/      # Business logic layer
├── Storage/       # In-memory data repositories
├── Validation/    # Workflow validation rules
├── DTOs/          # Data transfer objects
└── Exceptions/    # Custom error types
```

---

## ✨ Key Features

- Configurable workflows with named states and actions
- In-memory data handling using thread-safe dictionaries
- Strict validation for definitions and transitions
- Modular architecture for maintainability
- Swagger-based API exploration and testing

---

## ✅ Validation Rules

### Workflow Definitions
- Must have exactly one initial state
- State and action IDs must be unique
- All referenced states must be valid and enabled

### Workflow Runtime
- Only enabled actions can be executed
- Transitions must be valid from the current state
- No transitions allowed from a final state

### HTTP Status Codes
- `400` – Bad request / Validation error
- `404` – Resource not found
- `409` – Invalid action or state transition

---

## ⚠️ Known Limitations

- Data is stored in-memory — restarting the app will reset everything
- No concurrency or parallel instance handling
- No database integration (as per assignment scope)
- Focused on core functionality due to time constraints (2 hours)

---

## 🔮 Possible Future Enhancements

- Add database support (e.g., Entity Framework + SQLite or PostgreSQL)
- Enable concurrent workflows with locking and transaction handling
- Add support for custom state/action metadata
- Integrate audit logging and version history
- Secure the API with authentication & role-based access control

---

## ✅ Test Coverage

The test script included in the repo checks for:

- Valid and invalid workflow definitions
- Workflow instance creation and state transitions
- Error responses for invalid operations
- All critical API endpoints and flows

---

Built with ❤️ using **.NET 8** and **ASP.NET Core**  
Designed with clean architecture and simplicity in mind
> "Some say this looks AI-generated. I say: *thanks for the compliment*."              
> ~Aaditya Saraf