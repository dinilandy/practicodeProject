using Microsoft.EntityFrameworkCore;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddSingleton<ToDoDbContext>();
builder.Services.AddDbContext<ToDoDbContext>();
//cors
builder.Services.AddCors(option=>option.AddPolicy("AllowAll",builder=>{
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();
app.UseCors("AllowAll");
if(app.Environment.IsDevelopment()){
    app.UseSwagger();
    app.UseSwaggerUI();
}
//שליפת כל הנתונים 
app.MapGet("/", async (ToDoDbContext dbContext) =>
{
    var data = await dbContext.Items.ToListAsync();
    return Results.Ok(data);
});

//עדכון משימה
app.MapPut("/Item/{id}/{IsComplete}", async (ToDoDbContext dbContext, int id, bool IsComplete) =>
{
    var data = await dbContext.Items.FindAsync(id);
    if (data is null)
        return Results.NotFound();
    
    data.IsComplete = IsComplete;
    await dbContext.SaveChangesAsync();
    return Results.Ok(dbContext.Items);
});
// מחיקת משימה
app.MapDelete("/Item/{id}", async (ToDoDbContext dbContext, int id) =>
{
        if(await dbContext.Items.FindAsync(id) is Item item)
        {
            dbContext.Remove(item);
            await dbContext.SaveChangesAsync();
           return Results.Ok(dbContext.Items);
        }      
        return Results.NotFound();
});
//הוספת משימה
app.MapPost("/Item/{nmae}", async (ToDoDbContext dbContext, string nmae) =>
{       Item item=new Item();
           item.Nmae=nmae;
        dbContext.Items.Add(item);
    await dbContext.SaveChangesAsync();
    return Results.Ok(dbContext.Items);
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
/*using Microsoft.AspNetCore.Authorization.Infrastructure;
using TodoApi;
using System.Web.Http.Cors;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
// builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
// {
//     builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
// }));
builder.Services.AddScoped<ToDoDbContext>();
builder.Services.AddDbContext<ToDoDbContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

app.UseCors("corsapp");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.UseSwagger(options =>
{
    options.SerializeAsV2 = true;
});

app.MapGet("/todoitems", async (ToDoDbContext db) =>
    await db.Items.ToListAsync());

app.MapGet("/todoitems/complete", async (ToDoDbContext db) =>
    await db.Items.Where(t => t.IsComplete == true).ToListAsync());

app.MapGet("/todoitems/{id}", async (int id, ToDoDbContext db) =>
    await db.Items.FindAsync(id)
        is Item todo
            ? Results.Ok(todo)
            : Results.NotFound());

app.MapPost("/todoitems", async (Item todo, ToDoDbContext db) =>
{
    db.Items.Add(todo);
    await db.SaveChangesAsync();

    return Results.Created($"/todoitems/{todo.Id}", todo);
});

app.MapPut("/todoitems/{id}", async (int id, Item inputTodo, ToDoDbContext db) =>
{
    var todo = await db.Items.FindAsync(id);

    if (todo is null) return Results.NotFound();

    todo.Name = inputTodo.Name;
    todo.IsComplete = inputTodo.IsComplete;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/todoitems/{id}", async (int id, ToDoDbContext db) =>
{
    if (await db.Items.FindAsync(id) is Item todo)
    {
        db.Items.Remove(todo);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.Run();














/*
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton< ToDoDbContext>();
var app = builder.Build();

// Add a route handler that uses a service from DI
app.MapGet("/", (ToDoDbContext service) => service.Items);
app.MapPost("/{Id}/{Name}/{IsComplete}", (ToDoDbContext service) => service.Items.Add());
app.MapPut("/", () => "This is a PUT");
app.MapDelete("/", () => "This is a DELETE");

app.MapGet("/", () => "Hello World!");


app.Run();*/

/*
await using var provider = new ServiceCollection()
            .AddScoped<ToDoDbContext>()
            .BuildServiceProvider();
using (var scope = provider.CreateScope())
        {
            var foo = scope.ServiceProvider.GetRequiredService<ToDoDbContext>();
        }

/*==============================================================================================================
 <PackageReference Include="Swashbuckle.AspNetCore.Swagger" Version="6.5.0" />
    <PackageReference Include="Swashbuckle.AspNetCore.SwaggerGen" Version="6.5.0" />
    <PackageReference Include="Swashbuckle.AspNetCore.SwaggerUI" Version="6.5.0" />
  </===========================================================
  <PackageReference Include="Microsoft.AspNet.WebApi.Cors" Version="5.3.0" />
    <PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="7.0.10" />
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="7.0.2" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="7.0.2">
   */
