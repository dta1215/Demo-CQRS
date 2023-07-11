using Demo.Application.Configuration.Validations;
using Demo.Application.Products.CreateProduct;
using Demo.Application.Products.GetProducts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Demo_CQRS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator
                                )
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> AllProduts()
        {
            var result = await _mediator.Send(new GetProductsQuery());

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string Name)
        {
            var validator = new CreateProductCommandValidator();
            var validatorResult = validator.Validate(new CreateProductCommand { Name = Name });

            if (!validatorResult.IsValid)
            {
                return BadRequest(validatorResult.Errors.Select(x => new
                {
                    x.PropertyName,
                    x.ErrorMessage
                }));
            }

            var dbStatus =  await _mediator.Send(new CreateProductCommand { Name = Name });

            return Ok(dbStatus);
        }
    }
}
