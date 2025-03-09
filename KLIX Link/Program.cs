using System.Text.Json.Serialization;
using KLIX_Link.Data.Repositories;
using KLIX_Link_Core;
using KLIX_Link_Core.IRepositories;
using KLIX_Link_Core.Repositories;
using KLIX_Link_Core.Services;
using KLIX_Link_Service.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Service

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFileService, UserFileService>();



//Repository

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserFileRepository, UserFileRepository>();


//Data

builder.Services.AddDbContext<KLIX_Link.Data.DataContext>(options =>
{
    options.UseSqlServer("Data Source=DESKTOP-13C4MS2;Initial Catalog=KLIX Link;Integrated Security=true");
});


//builder.Services.AddSingleton<DataContext>();
//

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(ProfileMapping));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
