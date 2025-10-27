using Microsoft.EntityFrameworkCore;
using GymManagmentDAL.Data.Contexts;
using Microsoft.AspNetCore.Identity;
using GymManagmentDAL.Repositories.Interfaces;
using GymManagmentDAL.Repositories.Classes;
using GymManagmentBLL;
using GymManagmentBLL.Services.Interfaces;
using GymManagmentBLL.Services.Classes;

namespace GymManagmentPL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();


            builder.Services.AddDbContext<Dbcontext>(options =>

            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

            });

            //builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(IGenericRepository<>));

            //builder.Services.AddScoped<IPlanRepository,PlanRepository>();

             builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
             builder.Services.AddScoped<ISessionRepository, SessionRepository>();
             builder.Services.AddAutoMapper(x =>
             {
                x.AddProfile(new MappingProfiles());
             });
             builder.Services.AddScoped<IMemberService, MemberService>();
             builder.Services.AddScoped<IPlanService, PLanService>();
             builder.Services.AddScoped<ISessionService, SessionService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
