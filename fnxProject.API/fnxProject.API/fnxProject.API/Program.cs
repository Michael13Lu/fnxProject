using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
	options.Listen(System.Net.IPAddress.Loopback, 7150); // HTTP
	options.Listen(System.Net.IPAddress.Loopback, 7151, listenOptions =>
	{
		listenOptions.UseHttps(); // HTTPS
	});
});

builder.Services.AddHttpClient();

// Add services to the container. 
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

app.UseSession();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection(); 
app.UseCors(policy =>
	policy.AllowAnyOrigin()
		  .AllowAnyHeader()
		  .AllowAnyMethod());

app.UseAuthorization();

app.MapControllers();

app.Run();
