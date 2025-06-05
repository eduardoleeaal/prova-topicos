using API.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDataContext>();

var app = builder.Build();


//2-LISTAR TODOS
app.MapGet("/api/tarefas", ([FromServices] AppDataContext ctx) => {
    var tarefas = ctx.Tarefas.Include(t => t.Titulo).ToList();
    if(tarefas.Any())
    {
        return Results.Ok(tarefas);
    }

    return Results.NotFound();
});


app.Run();
