var builder = DistributedApplication.CreateBuilder(args);

#region Database

var username = builder.AddParameter("username", secret: true);
var password = builder.AddParameter("password", secret: true);

var postgres = builder.AddPostgres("postgres", username, password)
    .WithDataVolume("qorpe-db-data")
    .WithPgAdmin(containerName: "pgAdmin");
var qorpedb = postgres.AddDatabase("qorpe");

#endregion

#region Application(s)

var console = builder.AddNpmApp("qorpe-console", "../../../console")
    .WithHttpEndpoint(51545, 51545, isProxied: false)
    .WithHttpHealthCheck("/");

var hubHost = builder.AddProject<Projects.Qorpe_Hub_Host>("qorpe-hub-host")
    .WithReference(qorpedb)
    .WaitFor(qorpedb);

var schedulerHost = builder.AddProject<Projects.Qorpe_Scheduler_Host>("qorpe-scheduler-host")
    .WithReference(qorpedb)
    .WaitFor(qorpedb)
    .WithReplicas(3);

var gateHost = builder.AddProject<Projects.Qorpe_Gate_Host>("qorpe-gate-host")
    .WithReference(qorpedb)
    .WaitFor(qorpedb);

#endregion

#region Service Discovery

gateHost.WithReference(hubHost)
        .WithReference(schedulerHost)
        .WithReference(console);

hubHost.WithReference(gateHost);
schedulerHost.WithReference(gateHost);
console.WithReference(gateHost);

#endregion

builder.Build().Run();