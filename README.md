# 🚀 Task Management System

An enterprise task and project management system tailored for software development teams. The system features strict role-based access control between Managers and Developers (Employees) to streamline workflow tracking and progress monitoring.

---

## 🛠️ Tech Stack

The project is built on top of Microsoft's robust and modern technology stack:

* **Core Framework:** .NET 8.0 (ASP.NET Core MVC)
* **ORM (Database Access):** Entity Framework Core 8.0 (Code First Approach)
* **Database Engine:** SQL Server
* **Frontend UI:** Razor Pages (CSHTML), Bootstrap 5, Bootstrap Icons
* **System Architecture:** * **MVC Pattern** for clear separation of concerns.
    * **Service Layer (Interface & Service)** to encapsulate business logic independently.
    * **ViewModels** to optimize data transfer between Controllers and Views securely.
    * **Cookie Authentication** for secure login session management and role-based authorization.

---

## 🔑 Core Features & Workflow

### 1. Role-Based Authorization
* **Manager:** Create, edit, and delete projects; assign tasks to employees; monitor the team's global dashboard.
* **Employee (Dev):** Access a personalized workspace, accept/reject assignments, and update task execution progress.

### 2. Task Lifecycle (ProjectTaskStatus)
Tasks progress through a structured workflow to ensure accountability:
`Pending` ➡️ `Accepted` / `Rejected` ➡️ `InProgress` ➡️ `Completed`.

---