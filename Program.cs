using WorkflowEngine.Services;
using WorkflowEngine.Storage;
using WorkflowEngine.Validation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register our custom services
builder.Services.AddSingleton<IWorkflowRepository, InMemoryWorkflowRepository>();
builder.Services.AddSingleton<WorkflowValidator>();
builder.Services.AddScoped<IWorkflowService, WorkflowService>();

// Add CORS for development
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();
