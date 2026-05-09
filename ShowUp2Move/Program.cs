var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromHours(8);
});

var app = builder.Build();
ShowUp2Move_DAL.clsDataAccessSettings.ConnectionString =
    builder.Configuration.GetConnectionString("ShowUp2Move")!;

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();