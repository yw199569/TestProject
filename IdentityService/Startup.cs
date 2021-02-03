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
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using Microsoft.OpenApi.Models;

namespace IdentityService
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<TokenFilter>();//����Զ���Ĺ�����
            });
            services.AddIdentityServer()//Ids4����
                    .AddDeveloperSigningCredential()//��ӿ�����Աǩ��ƾ��
                    .AddTestUsers(InMemoryConfiguration.Users().ToList())
                    .AddInMemoryIdentityResources(InMemoryConfiguration.GetIdentityResources()) //����ڴ�apiresource
                    .AddInMemoryApiResources(InMemoryConfiguration.GetApiResources())
                    .AddInMemoryApiScopes(InMemoryConfiguration.GetApiScopes())
                    .AddInMemoryClients(InMemoryConfiguration.GetClients());//�������ļ���Client������Դ�ŵ��ڴ�

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)//�����֤
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://localhost:5001";
                });
            services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "My API",
                        Version = "V1",
                        Description = "API for rey",
                        Contact = new OpenApiContact() { Name = "rey", Email = "yw199569@qq.com" }
                    });
                    var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
                    var xmx = Path.Combine(basePath, "IdentityService.xml");
                    c.IncludeXmlComments(xmx);//添加swagger注释
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "Please enter into field the word 'Bearer' followed by a space and the JWT value",
                        Name = "Authorization",//http请求的
                        In = ParameterLocation.Header,//标记从哪个地方传入验证
                        Type = SecuritySchemeType.ApiKey//加密类型
                    });
                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        { new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference()
            {
                Id = "Bearer",
                Type = ReferenceType.SecurityScheme//传入类型
            }
        }, Array.Empty<string>() }
                    });
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
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "my api");

            });
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
