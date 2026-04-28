# E-Commerce API Setup Guide - .NET & MySQL

## 📋 Project Structure Created

```
E-COMMERCE/
├── Models/
│   └── Product.cs (Product model with validation)
├── Controllers/
│   └── ProductsController.cs (REST API endpoints)
├── Data/
│   └── ECommerceDbContext.cs (Database context)
├── Program.cs (Updated with MySQL configuration)
└── E-COMMERCE.csproj (Updated with EF Core packages)
```

---

## 🔧 Step-by-Step Setup

### Step 1: Restore NuGet Packages
Run this command in the terminal at project root:
```powershell
dotnet restore
```

### Step 2: Install MySQL Database
If you don't have MySQL installed:
- **Windows**: Download from https://dev.mysql.com/downloads/mysql/
- **Or use MySQL Docker**: 
  ```powershell
  docker run --name ecommerce-mysql -e MYSQL_ROOT_PASSWORD=your_password -e MYSQL_DATABASE=ecommerce_db -p 3306:3306 -d mysql:8.0
  ```

### Step 3: Update Connection String
Edit `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ecommerce_db;User=root;Password=your_password;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

**Connection String Breakdown:**
- `Server=localhost` - MySQL server location
- `Database=ecommerce_db` - Database name
- `User=root` - MySQL username
- `Password=your_password` - MySQL password (change this!)

### Step 4: Create Database Migrations
```powershell
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Step 5: Run the Application
```powershell
dotnet run
```

The API will start at: `https://localhost:7123` (or the port shown in console)

---

## 📡 API Endpoints

### Get All Products
```
GET /api/products
```
Response:
```json
[
  {
    "productId": 1,
    "name": "Laptop",
    "description": "High-performance laptop",
    "price": 999.99,
    "stock": 50,
    "category": "Electronics",
    "createdDate": "2026-01-28T10:30:00",
    "updatedDate": "2026-01-28T10:30:00",
    "isActive": true
  }
]
```

### Get Single Product
```
GET /api/products/1
```

### Create Product
```
POST /api/products
Content-Type: application/json

{
  "name": "Laptop",
  "description": "High-performance laptop",
  "price": 999.99,
  "stock": 50,
  "category": "Electronics",
  "isActive": true
}
```

### Update Product
```
PUT /api/products/1
Content-Type: application/json

{
  "productId": 1,
  "name": "Gaming Laptop",
  "description": "Updated description",
  "price": 1299.99,
  "stock": 45,
  "category": "Electronics",
  "isActive": true
}
```

### Delete Product
```
DELETE /api/products/1
```

---

## 🗄️ Key Concepts Explained

### 1. **Models** (Models/Product.cs)
- Represents your database table structure
- `[Key]` marks the primary key
- `[Required]` ensures the field cannot be null
- `[Range]` validates numeric values
- `[StringLength]` limits text length

### 2. **DbContext** (Data/ECommerceDbContext.cs)
- Bridge between C# code and database
- Manages database operations
- `DbSet<Product>` creates Products table
- `OnModelCreating` configures table details

### 3. **Controller** (Controllers/ProductsController.cs)
- Handles HTTP requests
- Uses dependency injection: `private readonly ECommerceDbContext _context`
- CRUD operations: Create, Read, Update, Delete

### 4. **Async/Await**
- `await` makes database operations non-blocking
- `Task<T>` represents asynchronous work
- Better performance for web applications

---

## 🛠️ NuGet Packages Added

| Package | Purpose |
|---------|---------|
| Microsoft.EntityFrameworkCore | ORM for database operations |
| Pomelo.EntityFrameworkCore.MySql | MySQL driver for EF Core |
| Microsoft.EntityFrameworkCore.Tools | Enables migrations (dotnet ef commands) |

---

## 🚀 Testing the API

### Using Visual Studio Code REST Client:
Create file `.vscode/rest-client.http`:

```
### Get all products
GET http://localhost:5000/api/products

### Create product
POST http://localhost:5000/api/products
Content-Type: application/json

{
  "name": "Mouse",
  "description": "Wireless mouse",
  "price": 29.99,
  "stock": 100,
  "category": "Accessories",
  "isActive": true
}

### Get product by id
GET http://localhost:5000/api/products/1

### Update product
PUT http://localhost:5000/api/products/1
Content-Type: application/json

{
  "productId": 1,
  "name": "USB Mouse",
  "description": "Wireless USB mouse",
  "price": 34.99,
  "stock": 95,
  "category": "Accessories",
  "isActive": true
}

### Delete product
DELETE http://localhost:5000/api/products/1
```

### Using Postman:
1. Import the endpoints into Postman
2. Set request method (GET, POST, PUT, DELETE)
3. Add JSON body for POST/PUT
4. Click Send

---

## 📝 Project Structure Explanation

```
appsettings.json          ← Database connection string here
Program.cs                ← Database configuration (already updated)
E-COMMERCE.csproj         ← NuGet packages (already updated)
│
├─ Models/
│  └─ Product.cs          ← Database table definition
│
├─ Controllers/
│  └─ ProductsController.cs  ← API endpoints (GET, POST, PUT, DELETE)
│
└─ Data/
   └─ ECommerceDbContext.cs  ← Database context (ORM bridge)
```

---

## ⚠️ Common Issues & Solutions

### "Database connection failed"
- Check MySQL is running
- Verify connection string in appsettings.json
- Ensure database and user exist

### "Could not load type Microsoft.EntityFrameworkCore"
- Run: `dotnet restore`

### "Migrations not found"
- Run: `dotnet ef migrations add InitialCreate`
- Then: `dotnet ef database update`

### Port already in use
- The app tries port 5000/5001 by default
- Check `Properties/launchSettings.json` to change ports

---

## 📚 Next Steps (Future Features)

1. **Add Authentication**: JWT tokens for user login
2. **Add Categories Model**: Separate table for product categories
3. **Add Orders Model**: Track customer purchases
4. **Add Error Handling**: Better error responses
5. **Add Validation**: Input validation on API calls
6. **Add Logging**: Track application events
7. **Add Pagination**: Handle large product lists

---

## 💡 Quick Reference

| Task | Command |
|------|---------|
| Restore packages | `dotnet restore` |
| Build project | `dotnet build` |
| Run project | `dotnet run` |
| Create migration | `dotnet ef migrations add MigrationName` |
| Update database | `dotnet ef database update` |
| Remove last migration | `dotnet ef migrations remove` |
| View database | MySQL Workbench or MySQL CLI |

---

## 🎯 What You Have Now

✅ **Product Model** - Complete with validations  
✅ **ProductController** - All CRUD operations  
✅ **Database Context** - MySQL connection configured  
✅ **MySQL Integration** - Ready to use  
✅ **Async Operations** - Non-blocking database calls  
✅ **RESTful API** - Standard HTTP methods  

---

**Happy coding! 🚀**
