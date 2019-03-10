#r "Newtonsoft.Json"

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;


public static void Run(TimerInfo myTimer, ILogger log, out string outputEventHubMessage)
{
    log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
    
    var box = new Box() {ID = 1, Type = "Box", Color = "White", Status = "Empty"};

    outputEventHubMessage = JsonConvert.SerializeObject(box);
}

public class Box
{
    public int ID {get;set;}
    public string Type {get;set;}
    public string Color {get;set;}
    public string Status {get;set;}
}