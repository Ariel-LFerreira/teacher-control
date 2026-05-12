    using FluentValidation;
    using FluentValidation.AspNetCore;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.OpenApi.Models;
    using TeacherControl.Data;
    using TeacherControl.Extensions;
    using TeacherControl.Middlewares;
    using TeacherControl.Models;
    using TeacherControl.Repositories;
    using TeacherControl.Repositories.Interfaces;
    using TeacherControl.Services;
    using TeacherControl.Services.Interfaces;
    using TeacherControl.Validators;

    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseMySql(
            builder.Configuration.GetConnectionString("DefaultConnection"),
            ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
        ));

    // Padronizar erro do FluentValidation (ModelState)
    builder.Services.Configure<ApiBehaviorOptions>(options =>
    {
        // O FluentValidation automático NÃO passa pelo middleware = POR ISSO PRECISO DO "InvalidModelStateResponseFactory"
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .SelectMany(x => x.Value.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            var response = new ErrorResponse
            {
                Message = "Validation failed",
                Errors = errors
            };

            return new BadRequestObjectResult(response);
        };
    });

    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IRoleRepository, RoleRepository>();
    builder.Services.AddScoped<ILessonRepository, LessonRepository>();

    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IRoleService, RoleService>();
    builder.Services.AddScoped<ILessonService, LessonService>();
    builder.Services.AddScoped<ITokenService, TokenService>();
    builder.Services.AddScoped<IAuthService, AuthService>();

    builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
    builder.Services.AddScoped<LoggingMiddleware>();
    builder.Services.AddScoped<ExceptionMiddleware>();

    // =========================
    // 🔐 JWT AUTHENTICATION
    // =========================
    // Configuração da autenticação baseada em tokens JWT (JSON Web Tokens)
    /*var jwtSettings = builder.Configuration.GetSection("Jwt");
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
        });*/
    builder.Services.AddJwtConfig(builder.Configuration); //SEPAREI A PARTE ACIMA.
    
    //  FluentValidation    // 
    builder.Services.AddFluentValidationAutoValidation();
    builder.Services.AddValidatorsFromAssemblyContaining<UserRequestValidator>();
    builder.Services.AddValidatorsFromAssemblyContaining<LessonRequestValidator>();
    builder.Services.AddValidatorsFromAssemblyContaining<RoleRequestValidator>();
    builder.Services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();


    builder.Services.AddAuthorization();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "API Teacher Control ",
            Version = "v1", 
            Description = "API para controle de Professores com autenticação JWT"
        });

        // Configuração JWT no Swagger
        // Define como o Swagger deve lidar com autenticação JWT na interface
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",  
            Type = SecuritySchemeType.Http,
            Scheme = "bearer", 
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Digite: Bearer SEU_TOKEN"
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
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
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

    //Middlewares:
    app.UseMiddleware<LoggingMiddleware>();
    app.UseMiddleware<ExceptionMiddleware>();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();