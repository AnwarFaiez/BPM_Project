# Enterprise BPM System - API Documentation

## Process Service API

Base URL: `http://localhost:5001/api`

### Processes

#### Create Process Definition
```http
POST /processes
Content-Type: application/json

{
  "name": "Leave Request Process",
  "description": "Employee leave request workflow",
  "key": "leave-request",
  "bpmnXml": "<bpmn:definitions...>",
  "category": "HR"
}
```

#### Get Process Definition
```http
GET /processes/{id}
```

#### Publish Process
```http
POST /processes/{id}/publish
```

#### Start Workflow Instance
```http
POST /processes/{id}/start
Content-Type: application/json

{
  "businessKey": "LR-2024-001",
  "variables": {
    "employeeName": "John Doe",
    "leaveDays": 5
  }
}
```

### Workflows

#### Get Workflow Instance
```http
GET /workflows/{id}
```

#### Suspend Workflow
```http
POST /workflows/{id}/suspend
```

#### Resume Workflow
```http
POST /workflows/{id}/resume
```

#### Terminate Workflow
```http
POST /workflows/{id}/terminate
Content-Type: application/json

"Cancelled by user request"
```

### Tasks

#### Get Task Instance
```http
GET /tasks/{id}
```

#### Complete Task
```http
POST /tasks/{id}/complete
Content-Type: application/json

{
  "variables": {
    "approved": true,
    "approverComments": "Approved"
  }
}
```

## Response Formats

### Success Response
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "Leave Request Process",
  "status": "Active",
  "createdAt": "2024-02-09T10:00:00Z"
}
```

### Error Response
```json
{
  "error": "Process definition not found",
  "statusCode": 404,
  "timestamp": "2024-02-09T10:00:00Z"
}
```

## Swagger UI

Interactive API documentation available at:
- Development: `http://localhost:5001/`
- Production: Configure in deployment
