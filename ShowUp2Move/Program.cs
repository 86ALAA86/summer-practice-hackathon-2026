var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromHours(8);
});

builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(x => {
    x.MultipartBodyLengthLimit = 10_000_000; // 10MB
});


var app = builder.Build();

// Set connection string for BOTH namespaces

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