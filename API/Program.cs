using API.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDataContext>();

var app = builder.Build();

//1-POST
app.MapPost("/api/tarefas/", ([FromBody] Tarefa tarefa, [FromServices] AppDataContext ctx) => 
{
    var status = ctx.Status.Find(tarefa.StatusId);
    if(status == null)
    {
        return Results.BadRequest("status nulo");
    }

    tarefa.Status = status;

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
    var tarefas = ctx.Tarefas.Include(t => t.Status).ToList();
    if(tarefas.Any())
    {
        return Results.Ok(tarefas);
    }

    return Results.NotFound();
});

//3-GET POR ID
app.MapGet("/api/tarefas/{id}", ([FromRoute] int id, [FromServices] AppDataContext ctx) =>
{
    Tarefa? tarefa = ctx.Tarefas.Include(t => t.Status).FirstOrDefault(t => t.Id == id);

    if(tarefa != null)
    {
        return Results.Ok(tarefa);
    }

    return Results.NotFound();
});


//4-ATUALIZAR TAREFA PUT
app.MapPut("/api/tarefas/{id}", ([FromRoute] int id,
                                [FromBody] Tarefa tarefa,
                                [FromServices] AppDataContext ctx) =>
{
    Tarefa? entidade = ctx.Tarefas.Include(t => t.Status).FirstOrDefault(t => t.Id == id);

    if(tarefa.Id == null)
    {
        return Results.BadRequest("Tarefa inexistente");
    }

    entidade.Titulo =  tarefa.Titulo;
    entidade.StatusId = tarefa.StatusId;
    entidade.DataVencimento = tarefa.DataVencimento;

    ctx.Tarefas.Update(entidade);
    ctx.SaveChanges();
    return Results.Ok(ctx.Tarefas.Include(t => t.Status).FirstOrDefault(t => t.Id == id));
});

//5-


app.Run();
