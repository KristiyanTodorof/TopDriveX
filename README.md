# 🚗 TopDriveX - Vehicle Marketplace Platform

> A modern, full-featured vehicle marketplace built with ASP.NET Core 8 and Clean Architecture principles.

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg)](CONTRIBUTING.md)

## 📋 Table of Contents

- [About](#about)
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Architecture](#architecture)
- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [Database Schema](#database-schema)
- [Configuration](#configuration)
- [Contributing](#contributing)
- [License](#license)

## 🎯 About

TopDriveX is a comprehensive vehicle marketplace platform designed for the Bulgarian market. It connects buyers and sellers, providing a seamless experience for listing, browsing, and purchasing vehicles online.

### Key Objectives

- **User-Friendly**: Intuitive interface for both buyers and sellers
- **Secure**: Role-based authentication and authorization
- **Scalable**: Clean Architecture for maintainability and growth
- **Modern**: Built with latest .NET technologies and best practices

## ✨ Features

### For Users

- 🔐 **User Authentication**
  - Registration and login with ASP.NET Identity
  - Email confirmation
  - Password recovery
  - Role-based access (User, Dealer, Admin)

- 📝 **Advertisement Management**
  - Create detailed vehicle listings
  - Upload multiple images (up to 15 per listing)
  - Edit and delete own advertisements
  - Multi-step form with validation

- 🔍 **Advanced Search & Filtering**
  - Filter by make, model, year, price, mileage
  - Location-based search
  - Real-time results
  - Pagination support

- ❤️ **Favorites System**
  - Save favorite listings
  - Quick access to saved vehicles
  - One-click toggle

- 👤 **User Dashboard**
  - View personal advertisements
  - Track favorites
  - Manage profile settings
  - View statistics (views, favorites)

### For Admins

- 🛠️ **Admin Panel**
  - Manage all advertisements
  - Edit any listing
  - Delete inappropriate content
  - User management

### Additional Features

- 📱 Responsive design (mobile, tablet, desktop)
- 🖼️ Image gallery with thumbnails
- 📊 Real-time statistics
- 🔔 Notification preferences
- 🌐 Multi-language support (Bulgarian)
- 🎨 Modern UI with Tailwind CSS

## 🛠️ Tech Stack

### Backend

- **Framework**: ASP.NET Core 8.0
- **Language**: C# 12
- **ORM**: Entity Framework Core 8.0
- **Database**: SQL Server
- **Authentication**: ASP.NET Identity
- **Architecture**: Clean Architecture / Onion Architecture

### Frontend

- **UI Framework**: Razor Pages / MVC
- **CSS Framework**: Tailwind CSS 3.4
- **JavaScript**: Vanilla JS (ES6+)
- **Icons**: Heroicons

### Tools & Libraries

- **Image Processing**: Custom ImageService
- **Validation**: Data Annotations
- **Unit of Work**: Custom implementation
- **Repository Pattern**: Generic repositories

## 🏗️ Architecture

The project follows **Clean Architecture** principles with clear separation of concerns:

```
TopDriveX/
├── TopDriveX.Domain/          # Enterprise Business Rules
│   ├── Models/                # Domain entities
│   ├── Enums/                 # Domain enumerations
│   └── Interfaces/            # Domain interfaces
│
├── TopDriveX.Application/     # Application Business Rules
│   ├── Contracts/             # Service interfaces
│   ├── Dtos/                  # Data Transfer Objects
│   ├── ViewModels/            # View Models
│   └── Services/              # Application services
│
├── TopDriveX.Infrastructure/  # Infrastructure Layer
│   ├── Data/                  # Database context
│   ├── Repositories/          # Data access
│   ├── Services/              # Infrastructure services
│   └── Migrations/            # EF Core migrations
│
└── TopDriveX.Web/             # Presentation Layer
    ├── Controllers/           # MVC Controllers
    ├── Views/                 # Razor views
    ├── wwwroot/              # Static files
    └── Extensions/            # Helper extensions
```

### Design Patterns

- **Repository Pattern**: Abstraction over data access
- **Unit of Work**: Transaction management
- **Dependency Injection**: Loose coupling
- **Service Layer**: Business logic separation
- **DTO Pattern**: Data transfer optimization

## 🚀 Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server 2019+](https://www.microsoft.com/sql-server) or SQL Server Express
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/TopDriveX.git
   cd TopDriveX
   ```

2. **Update connection string**
   
   Edit `appsettings.json` in `TopDriveX.Web`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=.;Database=TopDriveX;Trusted_Connection=True;TrustServerCertificate=True"
     }
   }
   ```

3. **Apply migrations**
   ```bash
   cd TopDriveX.Web
   dotnet ef database update --project ../TopDriveX.Infrastructure
   ```

4. **Seed initial data**
   
   The database will be seeded with:
   - Vehicle makes and models
   - Vehicle types
   - Admin user

5. **Run the application**
   ```bash
   dotnet run --project TopDriveX.Web
   ```

6. **Access the application**
   
   Navigate to: `https://localhost:5001`

### Default Credentials

- **Admin**: admin@topdrivex.bg / Admin@123

> ⚠️ **Important**: Change default passwords in production!

## 📁 Project Structure

```
TopDriveX/
├── TopDriveX.Domain/
│   ├── Models/
│   │   ├── User.cs
│   │   ├── Vehicle.cs
│   │   ├── Advertisement.cs
│   │   ├── Make.cs
│   │   ├── Model.cs
│   │   ├── VehicleImage.cs
│   │   └── Favorite.cs
│   └── Enums/
│       ├── FuelType.cs
│       ├── TransmissionType.cs
│       ├── BodyStyle.cs
│       └── VehicleCondition.cs
│
├── TopDriveX.Application/
│   ├── Contracts/
│   │   ├── IVehicleService.cs
│   │   ├── IMakeService.cs
│   │   └── IImageService.cs
│   ├── Dtos/
│   │   ├── VehicleDto.cs
│   │   └── VehicleListDto.cs
│   └── ViewModels/
│       ├── CreateAdvertisementViewModel.cs
│       └── EditAdvertisementViewModel.cs
│
├── TopDriveX.Infrastructure/
│   ├── Data/
│   │   └── ApplicationDbContext.cs
│   ├── Repositories/
│   │   ├── Repository.cs
│   │   └── UnitOfWork.cs
│   └── Services/
│       ├── VehicleService.cs
│       └── ImageService.cs
│
└── TopDriveX.Web/
    ├── Controllers/
    │   ├── HomeController.cs
    │   ├── VehiclesController.cs
    │   ├── AccountController.cs
    │   └── DashboardController.cs
    ├── Views/
    │   ├── Home/
    │   ├── Vehicles/
    │   └── Dashboard/
    └── wwwroot/
        ├── css/
        ├── js/
        └── images/
```

## 🗄️ Database Schema

### Core Tables

- **AspNetUsers** - User accounts
- **Vehicles** - Vehicle information
- **Advertisements** - Listing details
- **VehicleImages** - Image URLs
- **Makes** - Vehicle manufacturers
- **Models** - Vehicle models
- **VehicleTypes** - Vehicle categories
- **Favorites** - User saved listings

### Relationships

```
User 1 ─── ∞ Advertisement
Vehicle 1 ─── 1 Advertisement
Vehicle 1 ─── ∞ VehicleImage
Make 1 ─── ∞ Model
Make 1 ─── ∞ Vehicle
User 1 ─── ∞ Favorite
Advertisement 1 ─── ∞ Favorite
```

## ⚙️ Configuration

### App Settings

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=TopDriveX;..."
  },
  "ImageSettings": {
    "MaxFileSize": 10485760,
    "MaxImagesPerVehicle": 15,
    "AllowedExtensions": [".jpg", ".jpeg", ".png", ".webp"]
  }
}
```

## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 📞 Contact

- **Email**: contact@topdrivex.bg
- **Website**: https://topdrivex.bg

## 🗺️ Roadmap

### Version 1.0 (Current)
- ✅ User authentication
- ✅ Vehicle listings
- ✅ Search and filter
- ✅ Favorites system
- ✅ User dashboard
- ✅ Admin panel

### Version 2.0 (Planned)
- 🔲 Real-time chat
- 🔲 Email notifications
- 🔲 Vehicle comparison
- 🔲 Payment integration
- 🔲 Mobile app

---

Made with ❤️ in Bulgaria
