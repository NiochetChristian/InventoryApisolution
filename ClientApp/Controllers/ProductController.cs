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

        #region Index&Detail

        public ActionResult Index()
        {
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            var result = new List<Product>();
            try
            {
                var http = new HttpClient();
                var products = http.GetFromJsonAsync<List<Product>>(appSettings.ApiUrl + "api/Product/GetProducts").GetAwaiter();
                result = products.GetResult();
            }
            catch (Exception e)
            {
                result = new List<Product>();
            }

            return View((result?.OrderByDescending(p => p.ProductStatus == "Disponible").ToList() ?? new()).AsEnumerable());
        }

        public ActionResult Details(string id)
        {
            var http = new HttpClient();
            var products = http.GetFromJsonAsync<Product>(appSettings.ApiUrl + "api/Product/GetProduct/" + id).GetAwaiter();
            var result = products.GetResult();

            return result == null
                ? RedirectToAction("HandleError", "Product")
                : View(result);
        }
        #endregion

        #region Create

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

                    var response = client.PostAsync(appSettings.ApiUrl + "api/Product/AddProduct", jsonContent).GetAwaiter();
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

        #endregion

        #region Edit
        public ActionResult Edit(string id)
        {
            var http = new HttpClient();
            var products = http.GetFromJsonAsync<Product>(appSettings.ApiUrl + "api/Product/GetProduct/" + id).GetAwaiter();
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

                    var response = client.PutAsync(appSettings.ApiUrl + "api/Product/UpdateProduct", jsonContent).GetAwaiter();
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
        #endregion

        #region Delete&MarkDefective
        public ActionResult Delete(string id)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", appSettings.TokenBearer);

                var response = client.DeleteAsync(appSettings.ApiUrl + "api/Product/DeleteProduct/" + id).GetAwaiter();
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

                var response = client.PatchAsync(appSettings.ApiUrl + "api/Product/MarkProductAsDefective/" + id, null).GetAwaiter();
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
        #endregion

        [Route("Product/HandleError")]
        public IActionResult HandleError()
        {
            // Guardar un mensaje de error en TempData
            TempData["ErrorMessage"] = "Ha ocurrido un error inesperado.";
            return RedirectToAction("Index", "Product");
        }
    }
}
