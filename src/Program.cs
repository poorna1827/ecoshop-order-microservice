using Microsoft.EntityFrameworkCore;
using OrderMicroservice.DbContexts;
using OrderMicroservice.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<OrderDbContext>(options => {
    options.UseSqlite(builder.Configuration["ConnectionStrings:DevDatabase"]);
});

builder.Services.AddScoped<IApiService, ApiService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsDevelopment())
{
    app.UseCors(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
