using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TeacherControl.Data;
using TeacherControl.Models;
using TeacherControl.Repositories;
using TeacherControl.Repositories.Interfaces;
using TeacherControl.Services;
using TeacherControl.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ILessonRepository, LessonRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ILessonService, LessonService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();



// =========================
// 🔐 JWT AUTHENTICATION
// =========================
// Configuração da autenticação baseada em tokens JWT (JSON Web Tokens)
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["key"]);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer            = true,
            ValidateAudience          = true,
            ValidateLifetime          = true,
            ValidateIssuerSigningKey  = true,
            ValidIssuer               = jwtSettings["Issuer"],
            ValidAudience             = jwtSettings["Audience"],
            IssuerSigningKey          = new SymmetricSecurityKey(key),
            ClockSkew                 = TimeSpan.Zero,                  // Remove o delay padrão na expiração (5 minutos)
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


// Configura a geração da documentação Swagger
// Swagger é uma ferramenta para documentar e testar APIs REST
builder.Services.AddSwaggerGen(options =>
{
    // Cria a documentação da versão v1 da API
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API Teacher Control ", // Título da API
        Version = "v1",                                     // Versão da API
        Description = "API para controle de Professores com autenticação JWT" // Descrição
    });

    // 🔐 Configuração JWT no Swagger
    // Define como o Swagger deve lidar com autenticação JWT na interface
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",              // Nome do cabeçalho HTTP
        Type = SecuritySchemeType.Http,     // Tipo de esquema de segurança (HTTP)
        Scheme = "bearer",                   // Esquema Bearer (para JWT)
        BearerFormat = "JWT",                // Formato do token
        In = ParameterLocation.Header,      // Local do token (cabeçalho HTTP)
        Description = "Digite: Bearer SEU_TOKEN" // Texto de ajuda na interface do Swagger
    });

    // Adiciona requisito de segurança para todos os endpoints
    // Faz com que o Swagger exija o token JWT para acessar endpoints protegidos
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme, // Tipo de referência (esquema de segurança)
                    Id = "Bearer"                        // ID do esquema definido acima
                }
            },
            new string[] {} // Array vazio indica que se aplica a todos os escopos
        }
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();