# Employee Leave & Attendance Management System

A full-stack employee leave and attendance management system built as a .NET internship project. The system is designed to manage employees, departments, designations, leave policies, leave balances, leave requests, attendance, authentication, authorization, approvals, dashboards, reports, and future enterprise features.

## Project Overview

The Employee Leave & Attendance Management System provides a centralized platform for organizations to manage employee information, daily attendance, leave balances, and leave approval workflows.

The project follows a role-based workflow:

- **Admin** – controls the complete system and manages master data.
- **HR** – manages employees, departments, designations, leave types, and attendance.
- **Manager** – reviews and approves/rejects employee leave requests.
- **Employee** – views personal information, marks attendance, checks leave balance, and applies for leave.

## Main Objectives

- Manage employee records in one system.
- Manage departments and designations.
- Maintain leave types and leave balances.
- Allow employees to apply for leave.
- Validate leave balance before submitting requests.
- Provide manager approval and rejection workflow.
- Track employee attendance with check-in and check-out.
- Support attendance statuses such as Present, Absent, Half Day, Late, WFH, Holiday, and On Leave.
- Secure APIs using JWT authentication and role-based authorization.
- Provide RESTful APIs for frontend and future integrations.
- Support reporting and dashboard development.
- Provide a foundation for notifications, microservices, cloud deployment, and optional AI features.

## Technology Stack

### Backend

- ASP.NET Core Web API
- .NET 10
- C#
- Entity Framework Core
- SQL Server / LocalDB
- ADO.NET / Dapper-ready architecture for future data-access requirements
- JWT Bearer Authentication
- Role-Based Authorization
- Swagger / OpenAPI
- DataAnnotations validation

### Frontend

- React
- JavaScript / JSX
- Axios
- React Router
- Dashboard-based UI

### Testing

- xUnit
- Moq
- Integration/API testing
- Swagger-based manual API testing

### DevOps / Deployment

- Git
- GitHub
- GitHub Actions
- Docker
- Kubernetes
- Microsoft Azure
- Application monitoring and logging

### Future / Optional Technologies

- RabbitMQ
- Background workers
- MongoDB
- API Gateway
- AI-assisted leave and attendance insights

## System Architecture

```text
React Frontend
      |
      | HTTP / REST APIs
      v
ASP.NET Core Web API
      |
      +-- Controllers
      |
      +-- DTOs / Validation
      |
      +-- Services
      |
      +-- Entity Framework Core
      |
      v
SQL Server / LocalDB
```

Authentication flow:

```text
User Login
   |
   v
AuthController
   |
   v
Validate User
   |
   v
JWT Token
   |
   v
React / Swagger
   |
   v
Authorization by Role
```

## Roles and Permissions

| Feature | Admin | HR | Manager | Employee |
|---|---:|---:|---:|---:|
| Login | Yes | Yes | Yes | Yes |
| View employees | Yes | Yes | Yes | Yes |
| Create employee | Yes | Yes | No | No |
| Update employee | Yes | Yes | No | No |
| Delete employee | Yes | No | No | No |
| Manage departments | Yes | Yes | No | No |
| Manage designations | Yes | Yes | No | No |
| Manage leave types | Yes | Yes | No | No |
| Manage leave balances | Yes | HR/Admin workflow | No | View own balance |
| Apply for leave | Yes | Yes | Yes | Yes |
| Approve/reject leave | Yes | No | Yes | No |
| Mark attendance | Yes | Yes | Yes | Yes |
| Update attendance status | Yes | Yes | No | No |
| View reports/dashboard | Yes | Yes | Yes | Employee dashboard |

> Permissions can be expanded as additional frontend screens and policies are implemented.

## Core Modules

### 1. Authentication and Authorization

- User registration
- User login
- JWT token generation
- JWT validation
- Role-based authorization
- Protected API endpoints
- Admin-only endpoints
- Employee, HR, and Manager access control

### 2. Employee Management

Employee records contain:

- Employee ID
- Employee code
- Full name
- Email
- Phone
- Department
- Designation
- Linked user account
- Joining date
- Active/inactive status

Supported operations:

- Get all employees
- Get employee by ID
- Create employee
- Update employee
- Delete employee
- Duplicate email validation
- Duplicate employee-code validation
- Department validation
- Designation validation
- User-account linking validation

### 3. Department Management

Departments contain:

- ID
- Name
- Description
- Active status

Operations:

- List departments
- Create department
- Update department
- Delete department

### 4. Designation Management

Designations contain:

- ID
- Name
- Description/status as required by the implementation

Operations:

- List designations
- Create designation
- Update designation
- Delete designation

### 5. Leave Type Management

Leave types define available leave categories and their yearly allowance.

Examples:

- Sick Leave
- Casual Leave
- Earned/Annual Leave
- Other organization-specific leave types

Each leave type supports:

- Name
- Total allowed days
- Active status

### 6. Leave Balance Management

Leave balances track employee leave usage.

Each balance contains:

- Employee ID
- Leave Type ID
- Total days
- Used days
- Remaining days

The remaining balance is recalculated when leave usage changes.

### 7. Leave Request Management

Employees can submit leave requests with:

- Employee
- Leave type
- Start date
- End date
- Number of days
- Reason
- Status
- Applied date
- Manager comment

Default status:

```text
Pending
```

Possible workflow states:

```text
Pending -> Approved
Pending -> Rejected
```

### 8. Attendance Management

Attendance records contain:

- Employee ID
- Attendance date
- Check-in time
- Check-out time
- Status
- Remarks

Supported statuses:

```text
Present
Absent
Half Day
Late
WFH
Holiday
On Leave
```

Current API workflow includes:

- View all attendance
- View attendance by ID
- View employee attendance
- Check in
- Check out
- Update attendance status and remarks
- Prevent duplicate check-in for the same attendance record

## Leave Approval Workflow

```text
Employee
   |
   | Apply for Leave
   v
System validates leave balance
   |
   | Balance available
   v
Pending
   |
   v
Manager reviews request
   |
   +-------------------+
   |                   |
 Approve             Reject
   |                   |
   v                   v
Deduct balance       Rejected
   |
   v
Approved
```

### Approval Rules

1. Employee submits a leave request.
2. The system validates the employee and leave type.
3. The system checks the available leave balance.
4. The request starts with `Pending` status.
5. Manager or authorized Admin reviews the request.
6. On approval, the used leave days are increased.
7. Remaining leave days are reduced.
8. On rejection, the leave balance is not deducted.
9. The request status is updated accordingly.

## Attendance Workflow

```text
Employee Login
     |
     v
Check In
     |
     v
Attendance Record Created
     |
     v
Check Out
     |
     v
Attendance Completed
```

HR/Admin can additionally update attendance status and remarks when required.

## Database Design

The planned database contains the following major tables/entities:

```text
Users
Roles / Role information
Employees
Departments
Designations
LeaveTypes
LeaveBalances
LeaveRequests
Attendance
Holidays
Notifications
```

### Main Relationships

```text
User 1 ---- 0/1 Employee

Department 1 ---- * Employees

Designation 1 ---- * Employees

Employee 1 ---- * LeaveBalances

LeaveType 1 ---- * LeaveBalances

Employee 1 ---- * LeaveRequests

LeaveType 1 ---- * LeaveRequests

Employee 1 ---- * Attendance
```

## Current Database Entities

The current EF Core model includes:

- `User`
- `Employee`
- `Department`
- `Designation`
- `LeaveType`
- `LeaveBalance`
- `LeaveRequest`
- `Attendance`

EF Core migrations are included in the repository so the database schema can be recreated from the migration history.

## API Endpoints

Base URL in local development is provided by the ASP.NET Core launch settings.

### Authentication

```http
POST /api/Auth/register
POST /api/Auth/login
```

### Employees

```http
GET    /api/Employees
GET    /api/Employees/{id}
POST   /api/Employees
PUT    /api/Employees/{id}
DELETE /api/Employees/{id}
```

### Departments

```http
GET    /api/Departments
POST   /api/Departments
PUT    /api/Departments/{id}
DELETE /api/Departments/{id}
```

### Designations

```http
GET    /api/Designations
POST   /api/Designations
PUT    /api/Designations/{id}
DELETE /api/Designations/{id}
```

### Leave Types

```http
GET    /api/LeaveTypes
POST   /api/LeaveTypes
PUT    /api/LeaveTypes/{id}
DELETE /api/LeaveTypes/{id}
```

### Leave Balances

```http
GET    /api/LeaveBalances
GET    /api/LeaveBalances/{id}
POST   /api/LeaveBalances
PUT    /api/LeaveBalances/{id}
DELETE /api/LeaveBalances/{id}
```

### Leave Requests

```http
GET    /api/LeaveRequests
GET    /api/LeaveRequests/{id}
POST   /api/LeaveRequests
PUT    /api/LeaveRequests/{id}
```

### Attendance

```http
GET    /api/Attendance
GET    /api/Attendance/{id}
GET    /api/Attendance/employee/{employeeId}
POST   /api/Attendance/checkin
PUT    /api/Attendance/checkout/{id}
PUT    /api/Attendance/status/{id}
```

### Authentication Test Endpoints

```http
GET /api/Test/public
GET /api/Test/protected
GET /api/Test/admin
```

## Swagger / OpenAPI

Swagger is enabled in the Development environment.

The Swagger interface can be used to:

- Explore all API endpoints.
- Test GET, POST, PUT, and DELETE operations.
- Authenticate using a JWT Bearer token.
- Test role-protected endpoints.
- Validate request and response models.

## Validation

The API uses ASP.NET Core model validation and DataAnnotations for input validation.

Examples include:

- Required fields
- Email format validation
- Phone validation
- String length validation
- Positive ID validation
- Duplicate employee email validation
- Duplicate employee code validation
- Department existence validation
- Designation existence validation
- User existence validation
- Leave balance validation
- Duplicate attendance check-in validation

## Security

### JWT Authentication

JWT is used for API authentication.

The token contains claims for:

- User ID
- Name
- Email
- Role

### Role-Based Authorization

Examples:

```csharp
[Authorize]
```

```csharp
[Authorize(Roles = "Admin")]
```

```csharp
[Authorize(Roles = "Admin,HR")]
```

```csharp
[Authorize(Roles = "Admin,Manager")]
```

### Secret Management

Local database connection strings and JWT signing keys are stored using **.NET User Secrets** and are not committed to GitHub.

Do not add real production secrets, passwords, connection strings, API keys, or tokens to the repository.

## Project Structure

```text
EmployeeLeaveManagement.API/
│
├── Controllers/
│   ├── AttendanceController.cs
│   ├── AuthController.cs
│   ├── DepartmentsController.cs
│   ├── DesignationsController.cs
│   ├── EmployeesController.cs
│   ├── LeaveBalancesController.cs
│   ├── LeaveRequestsController.cs
│   ├── LeaveTypesController.cs
│   └── TestController.cs
│
├── DTOs/
│   ├── CreateEmployeeDto.cs
│   ├── EmployeeDto.cs
│   ├── LoginDto.cs
│   ├── LoginResponseDto.cs
│   ├── RegisterDto.cs
│   └── UpdateEmployeeDto.cs
│
├── Data/
│   └── AppDbContext.cs
│
├── Models/
│   ├── Attendance.cs
│   ├── Department.cs
│   ├── Designation.cs
│   ├── Employee.cs
│   ├── LeaveBalance.cs
│   ├── LeaveRequest.cs
│   ├── LeaveType.cs
│   └── User.cs
│
├── Services/
│   └── JwtService.cs
│
├── Migrations/
│
├── Properties/
│   └── launchSettings.json
│
├── Program.cs
├── appsettings.json
└── EmployeeLeaveManagement.API.csproj
```

## Getting Started

### Prerequisites

Install:

- Visual Studio 2026 or compatible Visual Studio version
- .NET 10 SDK
- SQL Server LocalDB or SQL Server
- Git
- Node.js and npm for the React frontend

### Clone the Repository

```bash
git clone https://github.com/mothichandkarri-bit/EmployeeLeaveManagement.git
cd EmployeeLeaveManagement
```

### Configure Backend Secrets

From the API project directory:

```bash
dotnet user-secrets init
```

Set the local database connection:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=(localdb)\\MSSQLLocalDB;Database=EmployeeLeaveManagementDB;Trusted_Connection=True;TrustServerCertificate=True;"
```

Set the JWT key:

```bash
dotnet user-secrets set "Jwt:Key" "YOUR_LOCAL_JWT_SECRET"
```

### Restore Packages

```bash
dotnet restore
```

### Apply Database Migrations

```bash
dotnet ef database update
```

### Run the API

```bash
dotnet run
```

Then open the Swagger URL displayed in the terminal.

## Frontend Setup

The planned frontend is a React application.

Typical setup:

```bash
npm install
npm run dev
```

The React frontend should communicate with the ASP.NET Core API using Axios or the browser Fetch API.

### Planned Frontend Screens

#### Authentication

- Login
- Registration where applicable

#### Admin Dashboard

- Total employees
- Departments
- Leave requests
- Attendance summary
- Pending approvals
- Reports

#### HR Dashboard

- Employee management
- Department management
- Designation management
- Leave type management
- Attendance management

#### Manager Dashboard

- Pending leave requests
- Employee leave history
- Approve/reject actions
- Manager comments

#### Employee Dashboard

- Profile
- Check in
- Check out
- Attendance history
- Leave balance
- Apply leave
- Leave request history

## Testing Strategy

### Manual API Testing

Swagger is used for initial endpoint and workflow verification.

Important scenarios include:

- Successful registration
- Duplicate email registration
- Successful login
- Invalid login
- Protected endpoint without token
- Protected endpoint with valid token
- Admin-only endpoint with Admin token
- Employee CRUD
- Duplicate employee email/code
- Invalid department/designation/user
- Leave request with sufficient balance
- Leave request without sufficient balance
- Manager approval
- Manager rejection
- Leave balance deduction after approval
- Attendance check-in
- Duplicate check-in
- Attendance check-out
- Attendance status update

### Automated Testing

The project can be extended with:

- xUnit unit tests
- Moq service/controller tests
- Integration tests
- API endpoint tests
- Authorization tests
- Leave workflow tests
- Attendance workflow tests

## Error Handling

The API uses appropriate HTTP status codes such as:

| Status | Meaning |
|---|---|
| 200 | Successful request |
| 201 | Resource created where applicable |
| 400 | Invalid request / validation / business rule failure |
| 401 | Authentication required or invalid credentials |
| 403 | Authenticated user lacks permission |
| 404 | Resource not found |
| 500 | Unexpected server error |

## Logging and Monitoring

The project uses ASP.NET Core logging configuration and can be extended with structured logging and centralized monitoring.

Planned production monitoring can include:

- Application logs
- API response monitoring
- Error tracking
- Database health monitoring
- Authentication failure monitoring
- Performance metrics
- Azure Application Insights or equivalent monitoring

## Notifications

The workflow supports a future notification system for events such as:

- Leave request submitted
- Leave approved
- Leave rejected
- Attendance reminders
- Missing check-out
- HR/admin announcements

Notifications can later be implemented using database notifications, email, RabbitMQ, or background workers.

## Microservices / Enterprise Extension

The current application is a modular ASP.NET Core Web API. The architecture can be expanded into microservices when required.

Possible services:

```text
API Gateway
   |
   +-- Auth Service
   +-- Employee Service
   +-- Leave Service
   +-- Attendance Service
   +-- Notification Service
   +-- Reporting Service
```

### RabbitMQ

RabbitMQ can be used for asynchronous events such as:

```text
LeaveApproved
LeaveRejected
AttendanceMarked
NotificationRequested
```

### Background Workers

Background workers can process:

- Notifications
- Attendance reminders
- Missing check-out alerts
- Scheduled reports
- Leave balance jobs

### MongoDB

MongoDB can optionally be used for high-volume or flexible data such as:

- Audit logs
- Notification history
- Activity logs
- Analytics events

## API Gateway

A future API Gateway can provide:

- Central routing
- Authentication checks
- Rate limiting
- Request logging
- Service discovery
- Unified API entry point

## Docker

The backend can be containerized using Docker.

Example workflow:

```bash
docker build -t employee-leave-api .
docker run -p 8080:8080 employee-leave-api
```

Production configuration should provide database and JWT settings through environment variables or a secure secret store rather than storing secrets inside the image.

## Kubernetes

For production-scale deployment, Kubernetes can manage:

- API replicas
- Rolling deployments
- Service discovery
- Configuration
- Secrets
- Health checks
- Horizontal scaling

Possible Kubernetes components:

```text
Deployment
Service
ConfigMap
Secret
Ingress
HorizontalPodAutoscaler
```

## Azure Deployment

The system can be deployed to Microsoft Azure using services such as:

- Azure App Service or Azure Container Apps
- Azure SQL Database
- Azure Container Registry
- Azure Key Vault
- Application Insights
- Azure Storage where required

Production secrets should be stored in Azure Key Vault or another secure secret-management solution.

## CI/CD with GitHub Actions

A future GitHub Actions pipeline can perform:

```text
Push to GitHub
     |
     v
Restore dependencies
     |
     v
Build
     |
     v
Run tests
     |
     v
Build Docker image
     |
     v
Publish image
     |
     v
Deploy to Azure / Kubernetes
```

## Optional AI Features

AI can be added without changing the core leave and attendance workflow.

Possible features:

- Leave trend analysis
- Attendance anomaly detection
- Absenteeism prediction
- Leave recommendation based on historical patterns
- Employee attendance insights
- Natural-language HR assistant
- Automated report summaries

AI features should be implemented as optional services and should not bypass existing authorization or business rules.

## Reports and Dashboards

Planned reports include:

- Daily attendance report
- Monthly attendance report
- Employee attendance history
- Department attendance summary
- Leave utilization report
- Leave balance report
- Pending leave requests
- Approved/rejected leave report
- Late attendance report
- Absenteeism trends

## Development Roadmap

### Phase 1 – Backend Foundation

- [x] ASP.NET Core Web API setup
- [x] SQL Server / LocalDB connection
- [x] Entity Framework Core
- [x] Database migrations
- [x] User authentication
- [x] JWT authentication
- [x] Role-based authorization

### Phase 2 – Core Modules

- [x] Employee management
- [x] Department management
- [x] Designation management
- [x] Leave type management
- [x] Leave balance management
- [x] Leave request workflow
- [x] Attendance management

### Phase 3 – Frontend

- [ ] React application
- [ ] Login page
- [ ] Role-based routing
- [ ] Admin dashboard
- [ ] HR dashboard
- [ ] Manager dashboard
- [ ] Employee dashboard
- [ ] Leave screens
- [ ] Attendance screens
- [ ] Reports

### Phase 4 – Quality

- [ ] Service/repository refinement
- [ ] xUnit tests
- [ ] Moq tests
- [ ] Integration tests
- [ ] API authorization tests
- [ ] Centralized exception handling
- [ ] Structured logging

### Phase 5 – Advanced Features

- [ ] Notifications
- [ ] RabbitMQ
- [ ] Background workers
- [ ] MongoDB for audit/analytics data
- [ ] API Gateway
- [ ] Microservice decomposition

### Phase 6 – DevOps and Cloud

- [ ] Docker
- [ ] GitHub Actions CI/CD
- [ ] Kubernetes
- [ ] Azure deployment
- [ ] Azure Key Vault
- [ ] Application monitoring

### Phase 7 – Optional AI

- [ ] Attendance anomaly detection
- [ ] Leave trend analysis
- [ ] Absenteeism prediction
- [ ] AI HR assistant

## Git Workflow

Recommended workflow:

```bash
git status
git add .
git commit -m "Describe your change"
git push origin main
```

Never commit:

- JWT secrets
- Passwords
- Database passwords
- API keys
- `.vs/`
- `bin/`
- `obj/`
- Local user-specific configuration

## Current Implementation Status

The backend currently includes working authentication, JWT authorization, employee management, department management, designation management, leave types, leave balances, leave requests, and attendance APIs.

The implemented leave workflow has been tested with employee submission, manager approval, and leave-balance deduction. Attendance check-in, check-out, duplicate check-in prevention, and status/remarks updates have also been tested.

The React frontend, automated test suite, notifications, reporting dashboards, RabbitMQ, microservices, Docker/Kubernetes deployment, Azure deployment, and AI features are planned extensions unless separately implemented.

## Sample Development Roles

For local development, create test users through the authentication API and assign roles such as:

```text
Admin
HR
Manager
Employee
```

Do not publish real passwords in this README or in source control.

## License

This project is currently intended as an internship/learning project. Add an appropriate open-source license before distributing it publicly if required.

## Author

**mothichandkarri**

GitHub: https://github.com/mothichandkarri-bit

## Project Summary

The Employee Leave & Attendance Management System demonstrates a practical enterprise-style application using ASP.NET Core Web API, Entity Framework Core, SQL Server, JWT authentication, role-based authorization, REST APIs, and a planned React frontend. It is structured so that the core HR workflow can later be extended with automated testing, notifications, messaging, microservices, Docker, Kubernetes, Azure, monitoring, dashboards, and AI-assisted analytics.
