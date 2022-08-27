using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using truYumWebApplication.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;


namespace truYumWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MenuItemController : ControllerBase
    {
        private readonly AppDBContext context;
        public MenuItemController(AppDBContext ctx)
        {
            this.context = ctx;
        }


        [HttpGet]       
        [ProducesResponseType(200, Type = typeof(List<MenuItem>))]
        public async Task<IActionResult> GetMenuItem()
        {
            var query = from item in context.MenuItems
                        select item;
            var result = await query.ToListAsync();
            return Ok(result);
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(200, Type = typeof(MenuItem))]
        [ProducesResponseType(404)]
        public IActionResult GetMenuItem(int Id)
        {
            var MenuItem = context.MenuItems.Find(Id);
            if (MenuItem == null)
            {
                return NotFound();
            }
            return Ok(MenuItem);

        }
    }
}
