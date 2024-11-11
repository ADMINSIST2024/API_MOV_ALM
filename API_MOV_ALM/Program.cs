using API_MOV_ALM.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Models;
using Services.Repository.Implementacion;
using Services.Repository.Interface;
using ServiceStack.Text;
using System.Text;

var builder = WebApplication.CreateBuilder(args);




builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUsuarioRepository<Usuario>, UsuarioRepository>();
builder.Services.AddScoped<ICompañiaRepository<Compañia>, CompañiaRepository>();
builder.Services.AddScoped<IAlmacenRepository<Almacen>, AlmacenRepository>();
builder.Services.AddScoped<ITipoDocumentoRepository<TipoDocumento>, TipoDocumentoRepository>();
builder.Services.AddScoped<ITipoMovimientoRepository<TipoMovimiento>, TipoMovimientoRepository>();
builder.Services.AddScoped<ICentroCostoRepository<CentroCosto>, CentroCostoRepository>();
builder.Services.AddScoped<IGeneralRepository<General>, GeneralRepository>();
builder.Services.AddScoped<IAltaRepository<Alta>, AltaRepository>();
builder.Services.AddScoped<IMovimientoRepository<Movimientos>, MovimientoRepository>();


builder.Services.AddScoped<IJwtRepository<Jwt>, JwtRepository>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options=>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew=TimeSpan.Zero,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        
    };
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .AllowAnyOrigin() // Permitir cualquier origen
            .AllowAnyMethod() // Permitir cualquier método (GET, POST, etc.)
            .AllowAnyHeader()); // Permitir cualquier cabecera
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
  
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
