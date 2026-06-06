using Contacts.Application.Interfaces.UseCases;
using Contacts.Application.UseCases;
using Contacts.Infrastructure.Extensions;
using Contacts.Infrastructure.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureContactsSqlContext(builder.Configuration);
builder.Services.ConfigureUnitOfWork();
builder.Services.AddValidators();
builder.Services.AddScoped<IContactUseCase, ContactUseCase>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(s => { s.SwaggerEndpoint("/swagger/v1/swagger.json", "Contacts API"); });

app.UseRouting();
app.MapControllers();

app.ApplyMigrations();

app.Run();