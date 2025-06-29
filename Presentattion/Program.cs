using Application.Interfaces;
using Application.Interfaces.AppRule;
using Application.Interfaces.Information;
using Application.Interfaces.ProApprovalStep;
using Application.Interfaces.ProProposal;
using Application.Interfaces.Users;
using Application.Services;
using Infrastructure.Command;
using Infrastructure.Persistence;
using Infrastructure.Querys;
using Interfaces.ProApprovalStep;
using Interfaces.ProProposal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddCors(x => x.AddDefaultPolicy(c => c.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});


builder.Services.AddDbContext<ApprovalProjectDB>(options =>
{
    options.UseSqlServer("Server=localhost;Database=ProjectDB;Trusted_Connection=True;TrustServerCertificate=True;Persist Security Info=true");
});


builder.Services.AddScoped<IProjectProposalCommand, ProjectProposalCommand>();
builder.Services.AddScoped<IProjectProposalQuery, ProjectProposalQuery>();
builder.Services.AddScoped<IApprovalRuleQuery, ApprovalRuleQuery>();
builder.Services.AddScoped<IApprovalStepCommand, ApprovalStepCommand>();
builder.Services.AddScoped<IApprovalStepQuery, ApprovalStepQuery>();
builder.Services.AddScoped<IUserCommand, UserCommand>();
builder.Services.AddScoped<IUserQuery, UserQuery>();
builder.Services.AddScoped<IInformationQuery, InformationQuery>();
builder.Services.AddScoped<IInformationService, InformationService>();
builder.Services.AddScoped<IProjectProposalService, ProjectProposalService>();
builder.Services.AddScoped<IApprovalStepService, ProjectApprovalStepService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
