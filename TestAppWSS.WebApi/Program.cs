using TestAppWSS.DAL;
using Microsoft.EntityFrameworkCore;
using TestAppWSS.Services.Interfaces;
using TestAppWSS.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var database_type = builder.Configuration["Database"];

switch (database_type)
{
    default: throw new InvalidOperationException($"��� �� {database_type} �� ��������������");

    case "SqlServer":
        builder.Services.AddDbContext<Database>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
        break;
}

builder.Services.AddTransient<IDbInitializer, DbInitializer>();

builder.Services.AddScoped<INodeData, NodeData>();

var app = builder.Build();


//�������������� ���� ������
await using (var scope = app.Services.CreateAsyncScope())
{
    var db_initializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
    await db_initializer.InitializeAsync(RemoveBefore: false);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
