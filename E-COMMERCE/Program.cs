using ECommerce.Data;
using ECommerce.Services;
using ECommerce.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddOpenApi();

// Add DbContext with MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Missing connection string 'DefaultConnection'.");

var jwtIssuer = builder.Configuration["JwtConfig:Issuer"]
    ?? throw new InvalidOperationException("Missing JwtConfig:Issuer.");

var jwtAudience = builder.Configuration["JwtConfig:Audience"]
    ?? throw new InvalidOperationException("Missing JwtConfig:Audience.");

var jwtKey = builder.Configuration["JwtConfig:key"]
    ?? throw new InvalidOperationException("Missing JwtConfig:key.");

builder.Services.AddDbContext<ECommerceDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

// autentiacion jwt service
builder.Services.AddAuthentication(options =>
{
   options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
   options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
   options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = jwtAudience,
        ValidIssuer = jwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddAuthorization();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<PasswordHasher<User>>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5173",
                "https://localhost:5173",
                "http://localhost:4173",
                "https://localhost:4173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddControllers();

var app = builder.Build();

await EnsureDatabaseIsReadyAsync(app);

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

var httpsPort = builder.Configuration.GetValue<int?>("HttpsRedirection:HttpsPort");
if (!app.Environment.IsDevelopment() || httpsPort.HasValue)
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseCors("AllowFrontend");

// jwt
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

static async Task EnsureDatabaseIsReadyAsync(WebApplication app)
{
    await using var scope = app.Services.CreateAsyncScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ECommerceDbContext>();

    await dbContext.Database.MigrateAsync();

    var skuExists = await SchemaObjectExistsAsync(
        dbContext,
        "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'Products' AND COLUMN_NAME = 'Sku';");

    if (!skuExists)
    {
        app.Logger.LogWarning("Schema drift detected: adding missing Products.Sku column.");
        await dbContext.Database.ExecuteSqlRawAsync("ALTER TABLE `Products` ADD COLUMN `Sku` varchar(100) NULL;");
    }

    var storefrontSectionsTableExists = await SchemaObjectExistsAsync(
        dbContext,
        "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'StorefrontSections';");

    if (!storefrontSectionsTableExists)
    {
        app.Logger.LogWarning("Schema drift detected: creating missing StorefrontSections table.");
        await dbContext.Database.ExecuteSqlRawAsync(@"
CREATE TABLE `StorefrontSections` (
    `StorefrontSectionId` int NOT NULL AUTO_INCREMENT,
    `Slug` varchar(100) NOT NULL,
    `Category` varchar(200) NOT NULL,
    `Title` varchar(200) NOT NULL,
    `Description` varchar(500) NOT NULL,
    `FeaturedEyebrow` varchar(100) NOT NULL,
    `AllEyebrow` varchar(100) NOT NULL,
    `AllTitle` varchar(200) NOT NULL,
    `AllDescription` varchar(500) NOT NULL,
    `EmptyTitle` varchar(200) NOT NULL,
    `EmptyDescription` varchar(500) NOT NULL,
    `UpdatedDate` datetime(6) NOT NULL,
    PRIMARY KEY (`StorefrontSectionId`)
) CHARACTER SET=utf8mb4;");
    }

    var storefrontSectionsSlugIndexExists = await SchemaObjectExistsAsync(
        dbContext,
        "SELECT COUNT(*) FROM INFORMATION_SCHEMA.STATISTICS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'StorefrontSections' AND INDEX_NAME = 'IX_StorefrontSections_Slug';");

    if (!storefrontSectionsSlugIndexExists)
    {
        app.Logger.LogWarning("Schema drift detected: creating missing StorefrontSections slug index.");
        await dbContext.Database.ExecuteSqlRawAsync("CREATE UNIQUE INDEX `IX_StorefrontSections_Slug` ON `StorefrontSections` (`Slug`);");
    }
}

static async Task<bool> SchemaObjectExistsAsync(ECommerceDbContext dbContext, string sql)
{
    var connection = dbContext.Database.GetDbConnection();
    var shouldCloseConnection = connection.State != System.Data.ConnectionState.Open;

    if (shouldCloseConnection)
    {
        await connection.OpenAsync();
    }

    try
    {
        await using var command = connection.CreateCommand();
        command.CommandText = sql;
        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result) > 0;
    }
    finally
    {
        if (shouldCloseConnection)
        {
            await connection.CloseAsync();
        }
    }
}
