# Employee Management System

## Project Overview

This system is designed to manage employees and facilitate vacation request workflows within a company. The key functionalities include:

- Viewing and managing employee information.
- Submitting, approving, and declining vacation requests.
- Maintaining a structured workflow for vacation approvals based on reporting hierarchies.

## Features

### 1. Employee Management

- View employee details (name, department, position, reporting structure).
- Update employee information (name, department, position, salary).
- Manage vacation days balance.

### 2. Vacation Request Workflow

- Employees can submit vacation requests.
- Prevents overlapping vacation requests.
- Managers can approve or decline vacation requests.
- Employees can view vacation request history.

### 3. Database & Technology Stack

- **Database:** SQL Server (Code-First using Entity Framework Core)
- **Back-end:** C# .NET Core
- **ORM:** Entity Framework Core
- **Development Approach:** Code-First with migrations

## Database Design

### Entities Implemented

1. **Department** 
2. **Position** 
3. **Employee** 
4. **VacationType** 
5. **RequestState** 
6. **VacationRequest** 

### Relationships

- Employee reports to another employee
- Employees belong to departments and positions.
- Vacation requests reference employees and request states.

## Setup Instructions

### Prerequisites

Ensure you have the following installed:

- .NET Core SDK
- SQL Server Management Studio (SSMS)
- Entity Framework Core CLI

### Steps to Run the Project

1. **Clone the Repository:**

   ```sh
   git clone https://github.com/omarHJ/Employee-Management-and-Vacation-System.git
   cd EmployeeManagementSystem
   ```

2. **Apply Migrations and Seed Data**

   ```sh
   dotnet ef database update
   ```

3. **Run the Application**

   ```sh
   dotnet run
   ```

## CRUD Operations Implemented

### Employee Management

- Add, Update, Retrieve Employees
- Fetch Employees with Pending Requests
- Get Employee Details by Employee Number
- Update Employee Vacation Days Balance

### Vacation Requests

- Submit Vacation Requests (preventing overlaps)
- Approve/Decline Requests
- Retrieve Pending and Approved Requests

## Queries & Performance Considerations

- **Get all employees**: Retrieves employee details efficiently.
- **Get employee by unique number**: Uses optimized LINQ queries.
- **Get employees with pending requests**: Filters efficiently using `Any()`.
- **Get approved vacation history**: Joins with vacation types and managers.
- **Get pending vacation requests for manager**: Uses reporting structure to filter relevant requests.
