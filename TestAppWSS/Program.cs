using Microsoft.EntityFrameworkCore;
using TestAppWSS.DAL;
using TestAppWSS.Services;
using TestAppWSS.Services.Interfaces;
using TestAppWSS.WebApi.Clients;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


var database_type = builder.Configuration["Database"];

switch (database_type)
{
    default: throw new InvalidOperationException($"Тип БД {database_type} не поддерживается");

    case "SqlServer":
        builder.Services.AddDbContext<Database>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
        break;
}

builder.Services.AddTransient<IDbInitializer, DbInitializer>();


//Работаем через клиента, который шлет запросы на web-api
builder.Services.AddHttpClient("WebApi", client => client.BaseAddress = new(builder.Configuration["WebApi"]))
    .AddTypedClient<INodeData, DepartmentsClient>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
