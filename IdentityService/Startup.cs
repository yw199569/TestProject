using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityService.Models;
using IdentityService.Common;
using IdentityServer4.AccessTokenValidation;
namespace IdentityService
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options=>
            {
                options.Filters.Add<TokenFilter>();//添加自定义的过滤器
            });
            services.AddIdentityServer()//Ids4服务
                 .AddDeveloperSigningCredential()//添加开发人员签名凭据
                 .AddTestUsers(InMemoryConfiguration.Users().ToList())
                 .AddInMemoryIdentityResources(InMemoryConfiguration.GetIdentityResources()) //添加内存apiresource
                 .AddInMemoryApiResources(InMemoryConfiguration.GetApiResources())
                 .AddInMemoryApiScopes(InMemoryConfiguration.GetApiScopes())
                 .AddInMemoryClients(InMemoryConfiguration.GetClients());//把配置文件的Client配置资源放到内存

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)//添加验证
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://localhost:5001";
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
            }
            app.UseRouting();
            app.UseIdentityServer();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });                
            });            
        }
    }
}
