using ConferencePlanner.GraphQL;
using ConferencePlanner.GraphQL.Data;
using ConferencePlanner.GraphQL.DataLoader;
using ConferencePlanner.GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using static System.IO.Path;

var dbPath = Combine(GetTempPath(), "conferences.db");

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPooledDbContextFactory<ApplicationDbContext>(
        _ => _.UseSqlite($"Data Source={dbPath}")
    );

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddType<SpeakerType>()
    .AddDataLoader<SpeakerByIdDataLoader>()
    .AddDataLoader<SessionByIdDataLoader>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.UseRouting();
app.UseEndpoints(_ => _.MapGraphQL());

app.MapGet("/", () => "Hello World!");

app.Run();
