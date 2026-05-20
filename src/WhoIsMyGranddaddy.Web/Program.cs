using Dapper;
using WhoIsMyGranddaddy.Core;
using WhoIsMyGranddaddy.Web.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

var connectionString = builder.Configuration.GetConnectionString("Granddaddy")
    ?? throw new InvalidOperationException("Missing connection string 'Granddaddy'.");

builder.Services.AddScoped<IPersonRepository>(_ => new SqlPersonRepository(connectionString));
builder.Services.AddScoped<GenealogyService>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

app.Run();
