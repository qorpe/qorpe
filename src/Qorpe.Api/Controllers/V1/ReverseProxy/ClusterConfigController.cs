using Asp.Versioning;
using LiteDB;
using Microsoft.AspNetCore.Mvc;
using Yarp.ReverseProxy.Configuration;

namespace Qorpe.Api.Controllers.V1.ReverseProxy;

[Route("Qorpe.Api/V{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class ClusterConfigController : BaseController
{
    [HttpPost]
    public IActionResult Index([FromBody] RouteConfig routeConfig)
    {
        //using (var db = new LiteDatabase(@"MyData.db"))
        //{
        //    // Koleksiyon oluşturur
        //    var collection = db.GetCollection<Person>("people");

        //    // Yeni bir belge ekler
        //    collection.Insert(new Person { Name = "Alice" });
        //    collection.Insert(new Person { Name = "Bob" });

        //    // Verileri sorgular
        //    var people = collection.FindAll();
        //    foreach (var person in people)
        //    {
        //        Console.WriteLine($"ID: {person.Id}, Name: {person.Name}");
        //    }
        //}

        using (var db = new LiteDatabase(@"MyData.db"))
        {
            // Koleksiyon oluşturur
            var collection = db.GetCollection<RouteConfig>("routes");

            // Yeni bir belge ekler
            collection.Insert(routeConfig);

            // Verileri sorgular
            var routes = collection.FindAll();
            foreach (var route in routes)
            {
                Console.WriteLine($"ID: {route.ClusterId}, Name: {route.CorsPolicy}");
            }
        }
        return Ok();
    }
}

public class Person
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

class Customer
{
    public int CustomerId { get; set; }
    public string? Name { get; set; }
}