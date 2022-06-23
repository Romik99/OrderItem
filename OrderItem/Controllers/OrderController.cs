using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OrderItem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        public static List<Cart> CartItems = new List<Cart>();
        // GET: api
        // POST api/<OrderController>
        [HttpPost("{id}")]
        public IActionResult Post(int id)
        {
            MenuItem a = new MenuItem();
            HttpResponseMessage response = new HttpResponseMessage();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:32060/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    response = client.GetAsync("api/MenuItem/" + id).Result;
                    string responseValue = response.Content.ReadAsStringAsync().Result;
                    a = JsonConvert.DeserializeObject<MenuItem>(responseValue);
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            if (a == null)
                return NotFound();
            Cart c = new Cart();
            c.menuItemId = id;
            c.menuItemName = a.Name;
            c.userId = 1;
            Cart cn = CartItems.FirstOrDefault();
            if (cn != null)
            {
                c.Id = CartItems.Max(x => x.Id) + 1;
                CartItems.Add(c);
            }
            else
            {
                c.Id = 1;
                CartItems.Add(c);
            }
            return Ok(c);

        }

    }
}
