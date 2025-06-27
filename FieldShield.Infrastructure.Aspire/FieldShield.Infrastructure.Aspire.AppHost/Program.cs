var builder = DistributedApplication.CreateBuilder(args);

var cacheDConnectionString = builder.AddConnectionString("RedisDb");
var postgresDbConnectionString = builder.AddConnectionString("FileShieldDb");

builder.AddProject<Projects.FieldShield_WebApi>("fieldshield-webapi")
    .WithReference(postgresDbConnectionString)
    .WithReference(cacheDConnectionString);

builder.AddProject<Projects.FieldShield_SeawedFileAPI>("seawedfs-webapi");

builder.Build().Run();
