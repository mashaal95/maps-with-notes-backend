using MapNotesAPI;
using Microsoft.EntityFrameworkCore;
using MapNotesAPI.Interfaces;
using MapNotesAPI.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<TestDbContext>(option =>
    option.UseSqlServer("Name=ConnectionStrings:TestDb"));

    // Register INotesRepo
builder.Services.AddScoped<INotesRepository, NotesRepository>();

    // Register IUserRepo
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();


app.UseCors(
    options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
);

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
