using Letovi.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Letovi.Web.Hubs;
using Newtonsoft.Json;
using System.Text.Json.Serialization;



internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllersWithViews();
        // Add services to the container.

        builder.Services.AddMvc()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
        builder.Services.AddSignalR();

        builder.Services.AddDbContext<FlightDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("FlightDbConnectionString")));


        builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<FlightDbContext>().AddDefaultTokenProviders();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }


        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.MapHub<ReservationHub>("/reservationHub");

        app.Run();
    }
}