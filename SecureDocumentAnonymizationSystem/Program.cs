using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;
using SecureDocumentAnonymizationSystem.Data;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// CORS politikasını tanımlıyoruz
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
        policy.WithOrigins("http://localhost:3000") // React uygulamanızın çalıştığı adres
              .AllowAnyHeader()
              .AllowAnyMethod());
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100MB limit
});

builder.Services.AddControllers();

builder.Services.AddSwaggerGen().AddEndpointsApiExplorer();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();


app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "upload")),
    RequestPath = "/upload"
});


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// CORS politikasını kullanıyoruz
app.UseCors("AllowReactApp");

app.UseRouting();
app.MapControllers();
app.MapScalarApiReference();

app.Run();
