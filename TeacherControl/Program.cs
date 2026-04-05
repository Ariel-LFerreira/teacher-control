using Microsoft.EntityFrameworkCore;
using TeacherControl.Data;
using TeacherControl.Repositories;
using TeacherControl.Repositories.Interfaces;
using TeacherControl.Services;
using TeacherControl.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args); // <-- deve vir primeiro

// Add services to the container. // Serviços
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração do banco de dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

//=============================================================================
//INJEÇÃO DE DEPENDENCIA: Repository
//=============================================================================
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ILessonRepository, LessonRepository>();
//=============================================================================
//INJEÇÃO DE DEPENDENCIA: Services
//=============================================================================
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ILessonService, LessonService>();


var app = builder.Build();

app.MapControllers();

// Garante que o banco de dados foi criado
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}