using PersonManagement.DAL.Repositories;
using PersonManagement.DAL;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(option =>
{
    option.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://127.0.0.1:5500").AllowAnyHeader().AllowAnyMethod();
    });
});

Console.WriteLine(builder.Environment.EnvironmentName);
 DataAccessSettings._connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string \'DefaultConnection\' was not found");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors("AllowFrontend");
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
