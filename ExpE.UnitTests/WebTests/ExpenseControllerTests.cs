using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpE.Domain.Models;
using ExpE.Repository.Interfaces;
using ExpE.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ExpE.UnitTests.WebTests
{
    [TestClass]
    public class ExpenseControllerTests
    {
        //[TestMethod]
        //public void Post_CorrectExpense_RedirectRoute()
        //{
        //    //arrange 
        //    var mockObject = new Mock<IExpensesRepository>();
        //    var mockCategory = new Mock<ICategoryRepository>();

        //    mockObject.Setup(x => x.Add(It.IsAny<Expense>())).Returns(true);

        //    var catId = Guid.NewGuid();

        //    Category category = new Category()
        //    {
        //        Id = catId,
        //        Title = "Testas"
        //    };

        //    mockCategory.Setup(x => x.GetById(It.IsAny<string>())).Returns(category);
            
        //    var controller = new ExpensesController(mockObject.Object, mockCategory.Object);

        //    var model = new ExpenseViewModel()
        //    {
        //        Amount = 10,
        //        CategoryId = catId.ToString(),
        //        Date = new DateTime(),
        //        Location = "alytus"
        //    };
        //    //act
        //    var result = controller.Create(model);

        //    //assert
        //    Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
        //}

        //[TestMethod]
        //public void Post_CorrectExpense_ServerError()
        //{
        //    //arrange 
        //    var mockObject = new Mock<IExpensesRepository>();
        //    var mockCategory = new Mock<ICategoryRepository>();

        //    mockObject.Setup(x => x.Add(It.IsAny<Expense>())).Returns(false);
        //    var catId = Guid.NewGuid();
        //    Category category = new Category()
        //    {
        //        Id = catId,
        //        Title = "Testas"
        //    };

        //    mockCategory.Setup(x => x.GetById(It.IsAny<string>())).Returns(category);

        //    var controller = new ExpensesController(mockObject.Object, mockCategory.Object);

        //    var model = new ExpenseViewModel()
        //    {
        //        Amount = 10,
        //        CategoryId = catId.ToString(),
        //        Date = new DateTime(),
        //        Location = "alytus"
        //    };
        //    //act
        //    var result = controller.Create(model);

        //    //assert
        //    Assert.IsInstanceOfType(result.Result, typeof(BadRequestResult));
        //}

    }
}
