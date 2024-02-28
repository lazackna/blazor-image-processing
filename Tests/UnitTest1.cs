using BlazorApp.Shared.Processing;
using Newtonsoft.Json;

namespace Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        ScaleProcess process = new ScaleProcess(100, 100);
        string json = JsonConvert.SerializeObject(process,new JsonSerializerSettings 
        { 
            TypeNameHandling = TypeNameHandling.All 
        });

        var scale = JsonConvert.DeserializeObject<ImageProcess>(json,new JsonSerializerSettings 
        { 
            TypeNameHandling = TypeNameHandling.All 
        });

        Assert.Pass();
    }
}