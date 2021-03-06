﻿    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DAL;
using BLL;

namespace WEB_APP
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSession();

            services.AddScoped<ICities_DB, Cities_DB>();
            services.AddScoped<ICitiesManager, CitiesManager>();

            services.AddScoped<IDishes_DB, Dishes_DB>();
            services.AddScoped<IDishesManager, DishesManager>();

            services.AddScoped<IOrdersManager, OrdersManager>();
            services.AddScoped<IOrder_DB, Orders_DB>();

            services.AddScoped<IOrder_DishManager, Order_DishManager>();
            services.AddScoped<IOrder_Dish_DB, Order_Dish_DB>();

            services.AddScoped<IStaffsManager, StaffsManager>();
            services.AddScoped<IStaffs_DB, Staffs_DB>();

            services.AddScoped<IRestaurants_DB, Restaurants_DB>();
            services.AddScoped<IRestaurantsManager, RestaurantsManager>();

            services.AddScoped<ICustomers_DB, Customers_DB>();
            services.AddScoped<ICustomersManager, CustomersManager>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}");
            });
        }
    }
}
