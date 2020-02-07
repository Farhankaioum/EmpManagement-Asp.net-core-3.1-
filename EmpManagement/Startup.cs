using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EmpManagement.Models;
using EmpManagement.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace EmpManagement
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            this._config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //Get connection string
            services.AddDbContextPool<AppDbContext>(
                        options => options.UseSqlServer(_config.GetConnectionString("EmpDBConnection")));

            services.AddMvc(options => {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                options.Filters.Add(new AuthorizeFilter(policy));

            }).AddXmlSerializerFormatters();



            //claim registration
            services.AddAuthorization(options => {
                options.AddPolicy("DeleteRolePolicy",
                    policy => policy.RequireClaim("Delete Role"));

                options.AddPolicy("EditRolePolicy",
                    policy => policy.RequireClaim("Edit Role"));

                // customize authorization using func in RequireAssertion method
                options.AddPolicy("specialEditRolePolicy", policy =>
                    policy.RequireAssertion(context =>
                    (context.User.IsInRole("Admin") && context.User.HasClaim(c => c.Type == "Edit Role"))
                      || context.User.IsInRole("Super Admin")
                    ));

                // custom authorization

            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("EditRolePolicyRestricted", policy =>
                    policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement())
                );
            });
                
            //custom authorization registration 
            services.AddSingleton<IAuthorizationHandler, CanEditOnlOtherAdminRolesAndClaimsHandler>();
            //services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();

            //custom authorization
            services.AddHttpContextAccessor();

            //access denied path registration
            services.ConfigureApplicationCookie(options => {
                options.AccessDeniedPath = new PathString("/Administration/AccessDenied");
            });
            
            services.AddIdentity<ApplicationUser, IdentityRole>(options => {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            })
              .AddEntityFrameworkStores<AppDbContext>();//for identity user 

            services.AddScoped<IEmployeeRepository,SQLEmployeeRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithRedirects("/Error/{0}");//centralised 404 error handled
                app.UseExceptionHandler("/Error");//Global exception handled
            }

            

            app.UseDefaultFiles();
            app.UseStaticFiles();

            
            app.UseRouting();


            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
