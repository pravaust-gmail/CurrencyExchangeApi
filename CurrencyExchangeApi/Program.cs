using CurrencyExchangeApi.Infrastructure;
using CurrencyExchangeApi.Models.Responses;
using CurrencyExchangeApi.Services;
using CurrencyExchangeApi.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IQuoteRequestValidator, QuoteRequestValidator>();
builder.Services.AddScoped<IQuoteService, QuoteService>();
builder.Services.AddScoped<ITransferService, TransferService>();
builder.Services.AddHttpClient<IExchangeRateProvider, FrankfurterExchangeRateProvider>();
builder.Services.AddSingleton<IExchangeRateStore, InMemoryExchangeRateStore>();
builder.Services.AddSingleton<IStore<CreateQuoteResponse>>(sp =>
    new InMemoryStore<CreateQuoteResponse>(x => x.QuoteId));

builder.Services.AddSingleton<IStore<CreateTransferResponse>>(sp =>
    new InMemoryStore<CreateTransferResponse>(x => x.TransferId));

var app = builder.Build();

// Enable Swagger (in all environments — optional)
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();