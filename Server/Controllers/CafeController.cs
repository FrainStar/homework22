namespace homework22;

using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.IO; 
using System.Net.Http;
using System.Text; 
using System.Threading.Tasks;
using System.Collections.Generic;


[ApiController]
public class StoreController : ControllerBase
{
    
    private readonly IProductRepository _productRepository;

    public StoreController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    [HttpPost]
    [Route("/store/add")]
    public IActionResult Add([FromBody] Product product)
    {
        _productRepository.AddProduct(product);
        return Ok($"Кофе {product.Name} добавлено");
    }

    [HttpPost]
    [Route("/store/remove")]
    public IActionResult Remove([FromBody] RemoveProductClass productt)
    {
        var product = _productRepository.GetProductByName(productt.Name);
        if (product != null)
        {
            _productRepository.RemoveProduct(productt);
            return Ok($"Кофе {product.Name} удалено");
        }
        else
        {
            return NotFound($"{product.Name} не найден");
        }
    }

    [HttpGet]
    [Route("/store/show")]
    public IActionResult Show()
    {
        return Ok(_productRepository.GetAllProducts());
    }
}