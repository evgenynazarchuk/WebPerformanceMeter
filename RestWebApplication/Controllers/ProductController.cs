namespace RestWebApplication.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using RestWebApplication.Services;
    using RestWebApplication.Models;

    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        protected readonly DataAccess Data;

        public ProductController(DataAccess data)
        {
            this.Data = data;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult Post([FromBody] string value)
        {
            return Ok();
        }

        [HttpPut]
        public IActionResult Put([FromBody] string value)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok();
        }
    }
}
