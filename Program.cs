using EMS.Data;
using EMS.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
RegisterRepositories(builder.Services);
var app = builder.Build();
ConfigureMiddleware(app);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Departments}/{action=Index}/{id?}");
app.Run();

static void RegisterRepositories(IServiceCollection services)
{
    services.AddScoped<IEmployeeRepository, EmployeeRepository>();
    services.AddScoped<IDepartmentRepository, DepartmentRepository>();
}

static void ConfigureMiddleware(WebApplication app)
{
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }
    
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthorization();
}
