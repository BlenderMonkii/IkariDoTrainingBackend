using IkariDoTrainingBackend.Data;
using IkariDoTrainingBackend.Models;
using IkariDoTrainingBackend.Models.Exercises;
using IkariDoTrainingBackend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowSpecificOrigins",
        policy =>
        {
            // Here you can set any origin, method, header
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

// Swagger konfigurieren
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Ikarido Training API",
        Version = "v1"
    });

    // Base URL auf HTTPS setzen
    c.AddServer(new OpenApiServer
    {
        Url = "https://localhost:44341",
        Description = "Development-Server"
    });
});


// Connection String aus der appsettings.json auslesen
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 34)) 
    ));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITrainingPlanService, TrainingPlanService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();
builder.Services.AddScoped<ISpecificExerciseService<FingerboardExercise>, FingerboardExerciseService>();
builder.Services.AddScoped<IExecutionService, ExecutionService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowSpecificOrigins");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
