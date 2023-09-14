using TecAlliance.Service2.Services;

namespace TecAlliance.Service2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddLogging(builder =>
            {
                builder.AddConsole();
            });

            var Configuration = builder.Configuration;

            builder.Services.AddHttpClient("EmployeeApi", client =>
            {
                client.BaseAddress = new Uri(Configuration["EmployeeApiUrl"]);
            });

            builder.Services.AddHostedService<EmployeeReportService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();


            app.MapRazorPages();

            app.Run();
        }
    }
}