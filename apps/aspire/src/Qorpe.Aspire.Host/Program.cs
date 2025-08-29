var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Qorpe_Hub_Host>("qorpe-hub-host");

builder.AddProject<Projects.Qorpe_Scheduler_Host>("qorpe-scheduler-host");

builder.AddProject<Projects.Qorpe_Gate_Host>("qorpe-gate-host");

builder.Build().Run();