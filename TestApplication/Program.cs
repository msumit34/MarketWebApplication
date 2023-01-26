using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args); 

builder.Services.AddControllersWithViews();

builder.Services.AddCors(options => {
    options.AddPolicy("Policy1", builder =>
    {
        builder.WithOrigins("https://localhost:7116")
        .WithMethods("POST", "GET", "PUT", "DELETE")
        .WithHeaders(HeaderNames.ContentType);
    });
});

var app = builder.Build();

if (!app.Environment.IsDevelopment()) 
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts(); 
}

app.UseCors("Policy1");


app.UseHttpsRedirection();
app.UseStaticFiles(); 

app.UseRouting(); 

app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
