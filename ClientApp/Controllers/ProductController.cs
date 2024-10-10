using ClientApp.Models;
using InventoryClientAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace ClientApp.Controllers
{
    public class ProductController : Controller
    {
        AppSettings appSettings;
        public ProductController(IOptions<AppSettings> _settings)
        {
            appSettings = _settings.Value;
        }

        // GET: ProductController
        public ActionResult Index()
        {
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            var http = new HttpClient();
            var products = http.GetFromJsonAsync<List<Product>>("https://localhost:44330/api/Product/GetProducts").GetAwaiter();
            var result = products.GetResult();
            return View((result?.OrderByDescending(p => p.ProductStatus == "Disponible").ToList() ?? new()).AsEnumerable());
        }

        public ActionResult Details(string id)
        {
            var http = new HttpClient();
            var products = http.GetFromJsonAsync<Product>("https://localhost:44330/api/Product/GetProduct/" + id).GetAwaiter();
            var result = products.GetResult();

            return result == null
                ? RedirectToAction("HandleError", "Product")
                : View(result);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Dictionary<string, string> iForm = new();
                foreach (var item in collection)
                    iForm.Add(item.Key, item.Value.ToString() ?? string.Empty);

                Product product = new()
                {
                    ProductId = Guid.NewGuid().ToString(),
                    ProductName = iForm["ProductName"],
                    ProductionType = iForm["ProductionType"],
                    ProductStatus = iForm["ProductStatus"],
                    DateAdded = DateTime.Now,
                    LastUpdated = DateTime.Now
                };

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", appSettings.TokenBearer);

                    var jsonContent = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");

                    var response = client.PostAsync("https://localhost:44330/api/Product/AddProduct", jsonContent).GetAwaiter();
                    var result = response.GetResult();

                    if (result.StatusCode == System.Net.HttpStatusCode.Created || result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Error al crear el producto");
                        return RedirectToAction("HandleError", "Product");
                    }
                }
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(string id)
        {
            var http = new HttpClient();
            var products = http.GetFromJsonAsync<Product>("https://localhost:44330/api/Product/GetProduct/" + id).GetAwaiter();
            try
            {
                var result = products.GetResult();
                return View(result);
            }
            catch (Exception)
            {
                return RedirectToAction("HandleError", "Product");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, IFormCollection collection)
        {
            try
            {
                Dictionary<string, string> iForm = new();
                foreach (var item in collection)
                    iForm.Add(item.Key, item.Value.ToString() ?? string.Empty);

                Product product = new()
                {
                    ProductId = iForm["ProductId"],
                    ProductName = iForm["ProductName"],
                    ProductionType = iForm["ProductionType"],
                    ProductStatus = iForm["ProductStatus"],
                    DateAdded = DateTime.Parse(iForm["DateAdded"]),
                    LastUpdated = DateTime.Now
                };

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", appSettings.TokenBearer);

                    var jsonContent = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");

                    var response = client.PutAsync("https://localhost:44330/api/Product/UpdateProduct", jsonContent).GetAwaiter();
                    var result = response.GetResult();

                    if (result.StatusCode == System.Net.HttpStatusCode.Created || result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return RedirectToAction("HandleError", "Product");
                    }
                }
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(string id)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", appSettings.TokenBearer);

                var response = client.DeleteAsync("https://localhost:44330/api/Product/DeleteProduct/" + id).GetAwaiter();
                var result = response.GetResult();

                if (result.StatusCode == System.Net.HttpStatusCode.Created || result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("HandleError", "Product");
                }
            }
        }

        public ActionResult MarkProductAsDefective(string id)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", appSettings.TokenBearer);

                var response = client.PatchAsync("https://localhost:44330/api/Product/MarkProductAsDefective/" + id, null).GetAwaiter();
                var result = response.GetResult();

                if (result.StatusCode == System.Net.HttpStatusCode.Created || result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("HandleError", "Product");
                }
            }
        }

        [Route("Product/HandleError")]
        public IActionResult HandleError()
        {
            // Guardar un mensaje de error en TempData
            TempData["ErrorMessage"] = "Ha ocurrido un error inesperado.";
            return RedirectToAction("Index", "Product");
        }
    }
}
