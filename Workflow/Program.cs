using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Workflow.Data;
using Workflow.Models;
using Workflow.Repositories;
using Workflow.Services.LeaveRequestWorkflow.model;
using Workflow.Services.LeaveRequestWorkflow.WorkflowDefinition;
using Workflow.Services.LeaveRequestWorkflow.WorkflowStep;
//using Workflow.Services.Workflows;
using WorkflowCore.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add Database Connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));

// Register Repository
builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();

builder.Services.AddWorkflow();
builder.Services.AddTransient<NotifyStep>();
builder.Services.AddTransient<PrintDecisionStep>();
builder.Services.AddTransient<CreateWorkflowInstanceInfoStep>();
builder.Services.AddTransient<UpdateWorkflowStatusStep>();
builder.Services.AddTransient<IWorkflow<LeaveRequestData>, LeaveApprovalWorkflow>();

// Add services to the container.
builder.Services.AddControllersWithViews();



var app = builder.Build();
// Resolve Workflow Host
var host = app.Services.GetRequiredService<IWorkflowHost>();

// Register workflows BEFORE starting
// Register workflows BEFORE starting
host.RegisterWorkflow<LeaveApprovalWorkflow, LeaveRequestData>();

// Start Workflow Engine
host.Start();



// Enable Migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();  // Auto-run migrations
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Application}/{action=Index}/{id?}");


app.Run();
