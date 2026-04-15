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
- [Environment Setup](#-environment-setup)
- [GitHub Secrets Configuration](#-github-secrets-configuration)
- [Deployment](#-deployment)
- [API Endpoints](#-api-endpoints)
- [Usage Examples](#-usage-examples)
- [Contributing](#-contributing)

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

- **.NET 8.0+** or **.NET 10.0+**
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

## 🔧 Environment Setup

### Local Development

#### Step 1: Create `.env` File
```bash
# API/.env
DB_CONNECTION=Host=localhost;Port=5432;Database=gamedb;Username=postgres;Password=your_password
```

#### Step 2: Configure PostgreSQL
```bash
# Linux/macOS
createdb gamedb

# Windows (using pgAdmin)
# 1. Open pgAdmin
# 2. Right-click "Databases" → Create → Database
# 3. Name: gamedb
```

#### Step 3: Apply Migrations
```bash
dotnet ef database update
```

### Production Environment

#### `appsettings.json` (Production)
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

The production connection string comes from **GitHub Secrets** (see next section).

---

## 🔐 GitHub Secrets Configuration

### Why Use GitHub Secrets?

GitHub Secrets securely store sensitive data (passwords, API keys, connection strings) without exposing them in your code.

### Setting Up GitHub Secrets

#### Step 1: Go to Your Repository Settings
1. Navigate to your GitHub repository
2. Click **Settings** (top menu)
3. Click **Secrets and variables** → **Actions** (left sidebar)

#### Step 2: Create Secrets

Click **New repository secret** and add the following:

##### **Essential Secrets**

| Secret Name | Value | Example |
|------------|-------|---------|
| `DATABASE_URL` | PostgreSQL connection string | `postgresql://user:password@host:5432/dbname` |
| `RENDER_API_KEY` | Render deployment API key | *(generate from Render dashboard)* |

##### **How to Create Each Secret**

**1. DATABASE_URL Secret**
```
Secret name: DATABASE_URL
Secret value: Host=your-db-host.c.us-east-1.rds.amazonaws.com;Port=5432;Database=gamedb;Username=postgres;Password=your_secure_password
```

**2. RENDER_API_KEY Secret**
```
Secret name: RENDER_API_KEY
Secret value: (Get from https://dashboard.render.com/account/api-tokens)
```

#### Step 3: Use Secrets in GitHub Actions

Create `.github/workflows/deploy.yml`:

```yaml
name: Deploy to Render

on:
  push:
    branches:
      - main

jobs:
  deploy:
    runs-on: ubuntu-latest
    
    steps:
      - uses: actions/checkout@v3
      
      - name: Deploy to Render
        env:
          DATABASE_URL: ${{ secrets.DATABASE_URL }}
          RENDER_API_KEY: ${{ secrets.RENDER_API_KEY }}
        run: |
          # Your deployment script here
          curl -X POST https://api.render.com/deploy \
            -H "Authorization: Bearer ${{ secrets.RENDER_API_KEY }}" \
            --data '{"service":"your-service-id"}'
```

---

## 📤 Deployment

### Option 1: Deploy to Render.com (Recommended)

#### Step 1: Connect GitHub Repository
1. Go to [render.com](https://render.com)
2. Click **New +** → **Web Service**
3. Connect your GitHub repository
4. Select the TestAPI repository

#### Step 2: Configure Environment Variables
In the Render dashboard:
1. Go to **Environment** tab
2. Add environment variable:
   ```
   Key: DB_CONNECTION
   Value: (Get from your GitHub SECRET: DATABASE_URL)
   ```

#### Step 3: Set Build and Start Commands
```
Build Command: cd API && dotnet build -c Release
Start Command: cd API && dotnet run --no-build -c Release
```

#### Step 4: Deploy
Click **Create Web Service** and Render will automatically deploy!

### Option 2: Manual Deployment with Docker

#### Build Docker Image
```bash
docker build -t gameapi:latest -f Dockerfile .
```

#### Run Container
```bash
docker run -e DB_CONNECTION="your_connection_string" \
           -p 5007:5007 \
           gameapi:latest
```

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

## 💡 Usage Examples

### Using cURL

```bash
# Get all games
curl -X GET https://localhost:5007/gamedetails

# Create a game
curl -X POST https://localhost:5007/gamedetails \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Cyberpunk 2077",
    "description": "Sci-fi RPG",
    "coverImageUrl": "https://example.com/cyberpunk.jpg"
  }'

# Update a game
curl -X PUT https://localhost:5007/gamedetails/1 \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Cyberpunk 2077: Phantom Liberty",
    "description": "Updated with DLC",
    "coverImageUrl": "https://example.com/cyberpunk-dlc.jpg"
  }'

# Delete a game
curl -X DELETE https://localhost:5007/gamedetails/1
```

### Using JavaScript/Fetch

```javascript
// Get all games
fetch('https://gameinfoapi.onrender.com/gamedetails')
  .then(res => res.json())
  .then(data => console.log(data));

// Create a game
fetch('https://gameinfoapi.onrender.com/gamedetails', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({
    title: 'Starfield',
    description: 'Space exploration RPG',
    coverImageUrl: 'https://example.com/starfield.jpg'
  })
})
  .then(res => res.json())
  .then(data => console.log(data));
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

## 🤝 Contributing

1. **Fork** the repository
2. **Create** a feature branch: `git checkout -b feature/your-feature`
3. **Commit** your changes: `git commit -m 'Add your feature'`
4. **Push** to the branch: `git push origin feature/your-feature`
5. **Submit** a Pull Request

---

## 📝 License

This project is licensed under the **MIT License** - see the [LICENSE](LICENSE) file for details.

---

## 🆘 Troubleshooting

### CORS Errors
- **Issue:** `Access to XMLHttpRequest has been blocked by CORS policy`
- **Solution:** Check that CORS middleware is properly configured in `Program.cs`

### Database Connection Failed
- **Issue:** `Failed to connect to database`
- **Solution:** 
  - Verify `DB_CONNECTION` environment variable
  - Ensure PostgreSQL is running
  - Check database credentials

### Migrations Not Applied
- **Issue:** `Unable to create an object of type 'AppDbContext'`
- **Solution:** Run `dotnet ef database update` in the API directory

---

## 📞 Support

For issues and questions:
- Create a new [GitHub Issue](https://github.com/your-username/TestAPI/issues)
- Check existing [Issues](https://github.com/your-username/TestAPI/issues?q=is%3Aissue)

---

## 🚀 What's Next?

- Add authentication & authorization (JWT)
- Implement caching (Redis)
- Add rate limiting
- Write unit & integration tests
- Add API versioning

---

**Made with ❤️ by [Your Name]**
