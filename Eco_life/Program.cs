using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Eco_life.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Configuração da string de conexão com o banco de dados
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("A string de conexão 'DefaultConnection' não está configurada.");
}

// Adiciona o contexto do banco de dados ao contêiner de serviços
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Configuração do Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Adiciona serviços de sessão
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Adiciona suporte a Razor Pages
builder.Services.AddRazorPages();

var app = builder.Build();

// Configurações de ambiente
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.MapRazorPages();
app.Run();

void TestDatabaseConnection(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            if (context.Database.CanConnect())
            {
                Console.WriteLine("Conexão com o banco de dados bem-sucedida!");
            }
            else
            {
                Console.WriteLine("Falha na conexão com o banco de dados.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocorreu uma exceção: {ex.Message}");
        }
    }
}
