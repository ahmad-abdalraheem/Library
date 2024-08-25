using API.Endpoints;
using Application.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
builder.RegisterServices();

var app = builder.Build();

app.RegisterMiddlewares();

app.RegisterMemberEndpoints();
app.RegisterBookEndpoints();
app.RegisterLibraryEndpoints();

app.MapGet("/", () => "Welcome to the OP Library API!");

app.Run();