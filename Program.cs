using Microsoft.EntityFrameworkCore;
using ODMS.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Database connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration
        .GetConnectionString("DefaultConnection")));

// 2. CORS — allows HTML pages to call the API
builder.Services.AddCors(options =>
    options.AddPolicy("AllowAll", p =>
        p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

// 3. Controllers + JSON settings (prevents circular reference errors)
builder.Services.AddControllers()
    .AddJsonOptions(x =>
        x.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

var app = builder.Build();

app.UseCors("AllowAll");
app.UseDefaultFiles();   // serves index.html automatically
app.UseStaticFiles();    // serves all files from wwwroot

app.MapControllers();

app.Run();
