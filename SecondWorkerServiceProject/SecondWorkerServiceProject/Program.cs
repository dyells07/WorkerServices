using SecondWorkerServiceProject.Data.Api;
using SecondWorkerServiceProject.Data;
using SecondWorkerServiceProject.Services.v1;
using SecondWorkerServiceProject;


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddTransient<UserDbContext>();
        services.AddDbContext<UserDbContext>();
        services.AddTransient<UserEqualizerService>();
        services.AddHttpClient<PlaceHolderClient>(client =>
        {
            client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
        });
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
