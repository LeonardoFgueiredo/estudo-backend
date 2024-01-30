using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MeuTodo.Data;
using MeuTodo.Models;
using MeuTodo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeuTodo.Controllers
{
    [ApiController]
    [Route(template: "v1")]
    public class TodoController : ControllerBase
    {
       
        [HttpGet]
        [Route(template:"todos")]
        public async Task<IActionResult> GetAsync(
            [FromServices]AppDbContext context
            ){
            var todos = await context
            .Todos
            . AsNoTracking()
            .ToListAsync();
            return Ok(todos);
        }

        [HttpGet]
        [Route(template:"todos/{id}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromServices]AppDbContext context, 
            [FromRoute]int id
            ){
            var todo = await context
            .Todos
            . AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
            return todo == null ? NotFound() : Ok(todo);
        }

        [HttpPost]
        [Route(template:"todos")]
        public async Task<IActionResult> PostAsync(
            [FromServices] AppDbContext context, 
            [FromBody] CreateTodoViewModel model
        ){
            if(!ModelState.IsValid)
                return BadRequest();

            var todo = new Todo {
                Title = model.Title,
                Done = false,
                Date = DateTime.Now,
            };

            try
            {

            await context.Todos.AddAsync(todo); //salva na memória
            await context.SaveChangesAsync(); //salva no banco
            return Created(uri:"v1/todos/{todo.Id}",todo); //retorna um created
            }
            catch (System.Exception)
            {
                
                return BadRequest();
            }
        }

        [HttpPut]
        [Route(template:"todos/{id}")]
        public async Task<IActionResult> PutAsync(
            [FromServices] AppDbContext context, 
            [FromBody] CreateTodoViewModel model,
            [FromRoute] int id
        ){
            if(!ModelState.IsValid)
                return BadRequest();

            var todo = await context.Todos.FirstOrDefaultAsync(x=> x.Id == id);

            if( todo == null)
                return NotFound();
            try
            {
                todo.Title = model.Title;

                context.Todos.Update(todo); //salva na memória
                await context.SaveChangesAsync(); //salva no banco
                return Ok(todo);
            }
            catch (System.Exception)
            {
                
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route(template:"todos/{id}")]
        public async Task<IActionResult> DeleteAsync(
            [FromServices] AppDbContext context, 
            [FromRoute] int id
        ){
            var todo = await context.Todos.FirstOrDefaultAsync(x=> x.Id == id);

            try
            {
                context.Todos.Remove(todo);
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (System.Exception)
            {
                
                return BadRequest();
            }
        }
    }
}