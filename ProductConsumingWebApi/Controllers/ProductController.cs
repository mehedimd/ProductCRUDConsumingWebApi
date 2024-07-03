using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using ProductConsumingWebApi.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;

namespace ProductConsumingWebApi.Controllers
{
    public class ProductController : Controller
    {
        string baseUrl = "https://localhost:7265";
        private readonly HttpClient client;
        public ProductController()
        {

            client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Clear();
        }
        // http get
        public async Task<IActionResult> Index()
        {
            var products = new List<Product>();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage res = await client.GetAsync("api/Product");
            if (res.IsSuccessStatusCode)
            {
                var productResponse = res.Content.ReadAsStringAsync().Result;

                products = JsonConvert.DeserializeObject<List<Product>>(productResponse);
            }

            return View(products);
        }

        // http get
        [HttpGet]
        public IActionResult Create()
        {
            List<SelectListItem> category = new List<SelectListItem>()
            {
                new SelectListItem(){Text = "Electronics", Value = "electronics"},
                new SelectListItem(){Text = "Grocaries", Value = "grocaries"},
                new SelectListItem(){Text = "Beauty", Value = "beauty"},
            };
            ViewBag.categories = category;
            return View();
        }
        // http post
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            try
            {
                var data = JsonConvert.SerializeObject(product);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage res = await client.PostAsync("api/Product", content);
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return View();
            }
            return View();
        }
        // delete
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                
                HttpResponseMessage res = await client.DeleteAsync($"api/product/{id}");
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {

                throw;
            }
            return RedirectToAction("Index");
        }

        // put get (edit)
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var product = new Product();

                HttpResponseMessage res = await client.GetAsync($"api/product/{id}");
                if (res.IsSuccessStatusCode)
                {
                    var productResponse = res.Content.ReadAsStringAsync().Result;
                    product = JsonConvert.DeserializeObject<Product>(productResponse);
                    return View(product);
                }
                
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction("Index");
        }
        // put
        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            try
            {
                var data = JsonConvert.SerializeObject(product);
                StringContent prod = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage res = await client.PutAsync($"api/product", prod);
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {

                throw;
            }
            return View(product);
        }
    }
}
