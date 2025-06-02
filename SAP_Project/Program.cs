using BusinesssLogicLayer.Common;
using BusinesssLogicLayer.Interfaces;
using BusinesssLogicLayer.Services;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repasitories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null; // PascalCase saqlanadi
});

builder.Services.AddMemoryCache();
builder.Services.AddScoped<SapSession>();

// Add HttpClient services
builder.Services.AddHttpClient<ISapAuthServiceInterface, SapAuthServiceRepasitory>();
builder.Services.AddHttpClient<ISapEmployeeService, SapEmployeeService>()
    .ConfigureHttpClient(client =>
    {
        client.Timeout = TimeSpan.FromMinutes(5); 
    });

builder.Services.AddHttpClient<IInvoiceService, InvoiceService>();
builder.Services.AddHttpClient<IItemsService, ItemService>();
builder.Services.AddHttpClient<IIncomingPaymentService, IncomingPaymentService>();
builder.Services.AddHttpClient<IPurchaseInvoiceService, PurchaseInvoiceService>();
builder.Services.AddHttpClient<IChartOfAccountService, ChartOfAccountService>();
builder.Services.AddHttpClient<ISupplierService, SupplierService>();
builder.Services.AddHttpClient<ICustomerService, CustomerService>();
builder.Services.AddHttpClient<ISQLInterfaces, SQLServices>();
builder.Services.AddHttpClient<ISapQueryInterface, SapQueryService>();


// Swagger qo'shish uchun Swashbuckle xizmatini qo'shish
builder.Services.AddEndpointsApiExplorer(); // API endpointlarini kashf etish uchun
builder.Services.AddSwaggerGen(); // Swagger generator

var app = builder.Build();

// Swagger middleware-larni faollashtirish
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();             // Swagger JSON endpointini yoqish
    app.UseSwaggerUI();           // Swagger UI ni yoqish
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
