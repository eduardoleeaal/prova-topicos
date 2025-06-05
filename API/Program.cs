using API.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDataContext>();

var app = builder.Build();

//1-POST
app.MapPost("/api/tarefas/", ([FromBody] Tarefa tarefa, [FromServices] AppDataContext ctx) => 
{
    if(tarefa == null || (tarefa.Titulo.Length < 3))
    {
        return Results.BadRequest("Os requisitos para criar a tarefa não foram atendidos.");
    }

    ctx.Tarefas.Add(tarefa);
    ctx.SaveChanges();
    return Results.Created("", tarefa);
});

//2-LISTAR TODOS
app.MapGet("/api/tarefas", ([FromServices] AppDataContext ctx) => {
    var tarefas = ctx.Tarefas.Include(t => t.Titulo).ToList();
    if(tarefas.Any())
    {
        return Results.Ok(tarefas);
    }

    return Results.NotFound();
});

//3-GET POR ID

app.Run();
