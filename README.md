# 🎮 Game Info API

A modern, production-ready REST API for managing game details built with **ASP.NET Core** and **PostgreSQL**.

![.NET](https://img.shields.io/badge/.NET-8.0+-blue?style=flat-square&logo=dotnet)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-14+-336791?style=flat-square&logo=postgresql)
![License](https://img.shields.io/badge/License-MIT-green?style=flat-square)

---

## 📋 Table of Contents

- [Features](#-features)
- [Prerequisites](#-prerequisites)
- [Installation](#-installation)
- [API Endpoints](#-api-endpoints)

---

## ✨ Features

- ✅ **RESTful API** - Clean, well-documented endpoints
- ✅ **PostgreSQL Database** - Reliable data persistence
- ✅ **Entity Framework Core** - ORM for database operations
- ✅ **OpenAPI/Scalar Documentation** - Interactive API explorer
- ✅ **CORS Enabled** - Secure cross-origin requests
- ✅ **Database Migrations** - Version-controlled schema changes
- ✅ **Docker Support** - Containerized deployment
- ✅ **Render Deployment Ready** - One-click cloud deployment

---

## 🎯 Prerequisites

- **.NET 10.0+**
- **PostgreSQL 14+**
- **Git**
- **Docker** (optional, for containerized deployment)

---

## 🚀 Installation

### 1. Clone the Repository

```bash
git clone https://github.com/your-username/TestAPI.git
cd TestAPI
```

### 2. Install Dependencies

```bash
cd API
dotnet restore
```

### 3. Create `.env` File (Local Development)

Create a `.env` file in the `API` directory:

```env
DB_CONNECTION=Host=localhost;Port=5432;Database=gamedb;Username=postgres;Password=your_password
```

### 4. Run Database Migrations

```bash
dotnet ef database update
```

### 5. Start the Application

```bash
dotnet run
```

The API will be available at: `https://localhost:5007`

---

## 🔌 API Endpoints

### Base URL
```
Local:       https://localhost:5007
Production:  https://gameinfoapi.onrender.com
```

### Endpoints

#### 📖 Get All Games
```http
GET /gamedetails
```

**Response:**
```json
[
  {
    "id": 1,
    "title": "The Legend of Zelda",
    "description": "Adventure game",
    "coverImageUrl": "https://example.com/zelda.jpg"
  }
]
```

#### 🎮 Get Game by ID
```http
GET /gamedetails/{id}
```

**Example:** `GET /gamedetails/1`

#### ➕ Create Game
```http
POST /gamedetails
Content-Type: application/json

{
  "title": "Elden Ring",
  "description": "Action RPG",
  "coverImageUrl": "https://example.com/elden.jpg"
}
```

#### ✏️ Update Game
```http
PUT /gamedetails/{id}
Content-Type: application/json

{
  "title": "Elden Ring: Shadow of the Erdtree",
  "description": "Updated description",
  "coverImageUrl": "https://example.com/elden-updated.jpg"
}
```

#### 🗑️ Delete Game
```http
DELETE /gamedetails/{id}
```

---

## 🏗️ Project Structure

```
TestAPI/
├── API/
│   ├── Controllers/           # API endpoints
│   │   └── GameDetailsController.cs
│   ├── Data/                  # Database context
│   │   └── AppDbContext.cs
│   ├── Models/                # Entity models & DTOs
│   │   ├── GameDetail.cs
│   │   └── GameDetailDTO.cs
│   ├── Migrations/            # Database migrations
│   ├── Program.cs             # Application startup
│   ├── API.csproj
│   └── appsettings.json
├── Dockerfile                 # Docker configuration
├── TestAPI.sln               # Solution file
└── README.md                 # This file
```

---

**Made with ❤️ by Dhruv Darji**
