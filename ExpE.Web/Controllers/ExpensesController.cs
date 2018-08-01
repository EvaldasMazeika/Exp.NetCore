using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpE.Domain;
using ExpE.Domain.Models;
using ExpE.Repository;
using ExpE.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace ExpE.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly IRepository _repo;

        public ExpensesController(IRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        [Route("expenses")]
        public async Task<ActionResult<Expense>> InsertExpense([FromBody] Expense model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            model.Id = ObjectId.GenerateNewId().ToString();

            var isAdded = await _repo.AddExpense(model);
            if (isAdded)
            {
                return CreatedAtAction(nameof(GetExpense), new { id = model.Id }, model);
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("expenses/{id}")]
        public async Task<ActionResult<Expense>> GetExpense(string id)
        {
            return await _repo.GetExpenseById(id);
        }

        [HttpGet]
        [Route("expenses")]
        public Task<IEnumerable<Expense>> GetAll()
        {
            return _repo.GettAllExpenses();
        }

    }
}