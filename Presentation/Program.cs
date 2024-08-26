using Presentation.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.RegisterServices();

var app = builder.Build();

app.RegisterMiddlewares();

app.RegisterMemberEndpoints();
app.RegisterBookEndpoints();
app.RegisterLibraryEndpoints();

app.MapGet("", () => "Welcome to the OP Library API!");

app.Run();