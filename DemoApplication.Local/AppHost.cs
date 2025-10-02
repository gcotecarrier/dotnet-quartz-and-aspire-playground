using System.Reflection;

var builder = DistributedApplication.CreateBuilder(args);

// Read the embedded SQL script
var assembly = Assembly.GetExecutingAssembly();
using var stream = assembly.GetManifestResourceStream("DemoApplication.Local.quartzSqlServerSetup.sql");
using var reader = new StreamReader(stream!);
var quartzSetupScript = reader.ReadToEnd();

var mailDev = builder.AddContainer("maildev", "maildev/maildev")
    .WithEndpoint(5002, 1080, "http")
    .WithHttpHealthCheck();

var sqlServer = builder.AddSqlServer("sql");
var database = sqlServer.AddDatabase("myDB")
    .WithCreationScript(quartzSetupScript);

var worker = builder.AddProject<Projects.DemoApplication_Worker>("worker")
    .WithReference(database)
    .WaitFor(database)
    .WaitFor(mailDev);

// The webapp isn't really developed, but I wanted to highlight what it would look like
var webapp = builder.AddProject<Projects.DemoApplication_Web>("webapp")
    .WithReference(database)
    .WaitFor(database)
    .WaitFor(mailDev)
    .WithEndpoint(5001, 8080);

builder.Build().Run();