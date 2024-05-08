# Attendance API Documentation (Asp.NET APi, Ef Core, LinQ, MySQL)
 


## Features

- **Authentication & Authorization**: Provides secure authentication and authorization mechanisms for users.
- **Dynamic Data Handling**: Automatically manages professor data based on their subjects in the current hall.
- **Effortless Reporting**: Generates today, monthly, and semester attendance data based on the professor's role (subject) without additional effort from the professor.
- **Excel Export**: Professors can download attendance data as an Excel sheet for today, monthly, and semester reports.

## Endpoints

### Authentication

- **POST /api/AccountController/login**: Authenticates a user and returns a JWT token.
- **GET /api/AccountController/Schedule**: Registers a new user.

### Attendance Management

- **GET /api/attendance/today**: Retrieves today's attendance data.
- **GET /api/attendance/monthly**: Retrieves monthly attendance data.
- **GET /api/attendance/semester**: Retrieves semester attendance data.
----------------------------------------------------------------------------
- **GET /api/attendance/today/excel**: Downloads today's attendance data as an Excel sheet.
- **GET /api/attendance/monthly/excel**: Downloads monthly attendance data as an Excel sheet.
- **GET /api/attendance/semester/excel**: Downloads semester attendance data as an Excel sheet.

## Authentication & Authorization

- **JWT Authentication**: Utilizes JSON Web Tokens for secure authentication.
- **Role-based**: Implements authorization based on user roles (e.g., professor).

## Usage

### 1. Authentication

To authenticate, send a POST request to `/api/Account/Login` with valid credentials. You will receive a JWT token in the response headers, which you should include in subsequent requests as an Authorization header with the value `Bearer <token>`.


### 2. Attendance Retrieval

You can retrieve attendance data using the provided endpoints:

- `/api/H406Attend/TodayAttends`: Get today's attendance data.
- `/api/H406Attend/MonthAttends`: Get monthly attendance data.
- `/api/H406Attend/SemsterAttends'`: Get semester attendance data.

### 3. Excel Export

Professors can download attendance data as an Excel sheet using the following endpoints:

- `/api/DownloadAsExcel/TodatAttendExcel`: Download today's attendance data as an Excel sheet.
- `/api/DownloadAsExcel/MonthAttendExcel`: Download monthly attendance data as an Excel sheet.
- `/api/DownloadAsExcel/SemsterAttendExcel`: Download semester attendance data as an Excel sheet.



## Team 
Omar Mohamed : Software Enginner | Back-End Devolper 

## License

This project is licensed under the MIT License 
