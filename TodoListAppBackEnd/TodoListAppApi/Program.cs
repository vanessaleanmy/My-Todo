using Microsoft.EntityFrameworkCore;
using TodoListAppApi.Data;
using TodoListAppApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<TodoDbContext>(options => options.UseInMemoryDatabase("TodoDb"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var uiOrigin = builder.Configuration.GetSection("FrontEnd")["UiUrl"];
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowTodoListUi", policy =>
    {
        policy.WithOrigins(uiOrigin?? "http://localhost:4200")
             .AllowAnyMethod()
             .AllowAnyHeader();
    });
});

builder.Services.AddScoped<ITodoServices, TodoServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors("AllowTodoListUi");

app.Run();
