using Educational.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Volo.Abp;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;
using Yitter.IdGenerator;

namespace Educational;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(EducationalApplicationModule),
    typeof(EducationalEntityFrameworkCoreModule),
    typeof(AbpAspNetCoreMvcUiLeptonXLiteThemeModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule)
)]
public class EducationalHttpApiHostModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        //PreConfigure<OpenIddictBuilder>(builder =>
        //{
        //    builder.AddValidation(options =>
        //    {
        //        options.AddAudiences("Educational");
        //        options.UseLocalServer();
        //        options.UseAspNetCore();
        //    });
        //});
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // 配置认证
        var configuration = context.Services.GetConfiguration();
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        ConfigureAuthentication(context, configuration);
        ConfigureAuthentication(context);
        ConfigureBundles();
        ConfigureUrls(configuration);
        ConfigureConventionalControllers();
        ConfigureVirtualFileSystem(context);
        ConfigureCors(context, configuration);
        ConfigureSwaggerServices(context);
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
    {
        // 配置JWT Bearer认证
        context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,                     // 验证签发方
                    ValidIssuer = configuration["Jwt:Issuer"], // 合法签发方(取自配置)

                    ValidateAudience = true,                   // 验证接收方
                    ValidAudience = configuration["Jwt:Audience"], // 合法接收方(取自配置)

                    ValidateIssuerSigningKey = true,           // 验证签名密钥
                    IssuerSigningKey = new SymmetricSecurityKey( // 签名密钥(取自配置)
                        Encoding.UTF8.GetBytes(configuration["Jwt:SecurityKey"]))
                };
            });
    }




    private void ConfigureAuthentication(ServiceConfigurationContext context)
    {
        //context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        //context.Services.Configure<AbpClaimsPrincipalFactoryOptions>(options =>
        //{
        //    options.IsDynamicClaimsEnabled = true;
        //});
    }

    private void ConfigureBundles()
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options.StyleBundles.Configure(
                LeptonXLiteThemeBundles.Styles.Global,
                bundle =>
                {
                    bundle.AddFiles("/global-styles.css");
                }
            );
        });
    }

    private void ConfigureUrls(IConfiguration configuration)
    {
        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            options.RedirectAllowedUrls.AddRange(configuration["App:RedirectAllowedUrls"]?.Split(',') ?? Array.Empty<string>());

            //options.Applications["Angular"].RootUrl = configuration["App:ClientUrl"];
            //options.Applications["Angular"].Urls[AccountUrlNames.PasswordReset] = "account/reset-password";
        });
    }

    private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        if (hostingEnvironment.IsDevelopment())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<EducationalDomainSharedModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}Educational.Domain.Shared"));
                options.FileSets.ReplaceEmbeddedByPhysical<EducationalDomainModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}Educational.Domain"));
                options.FileSets.ReplaceEmbeddedByPhysical<EducationalApplicationContractsModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}Educational.Application.Contracts"));
                options.FileSets.ReplaceEmbeddedByPhysical<EducationalApplicationModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}Educational.Application"));
            });
        }
    }

    private void ConfigureConventionalControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(EducationalApplicationModule).Assembly);
        });
    }

    private void ConfigureSwaggerServices(ServiceConfigurationContext context)
    {
        context.Services.AddSwaggerGen(
            options =>
            {
                options.SwaggerDoc("公告", new OpenApiInfo { Title = "公告管理", Version = "v1" });
                options.SwaggerDoc("课程", new OpenApiInfo { Title = "课程管理", Version = "v1" });
                options.SwaggerDoc("组织机构", new OpenApiInfo { Title = "组织机构管理", Version = "v1" });
                options.SwaggerDoc("职位", new OpenApiInfo { Title = "职位管理", Version = "v1" });
                options.SwaggerDoc("权限", new OpenApiInfo { Title = "权限管理", Version = "v1" });
                options.SwaggerDoc("角色", new OpenApiInfo { Title = "角色管理", Version = "v1" });
                options.SwaggerDoc("专题", new OpenApiInfo { Title = "专题管理", Version = "v1" });
                options.SwaggerDoc("成员", new OpenApiInfo { Title = "成员管理", Version = "v1" });
                options.SwaggerDoc("物料", new OpenApiInfo { Title = "物料管理", Version = "v1" });
                options.SwaggerDoc("成员分配角色", new OpenApiInfo { Title = "成员分配角色管理", Version = "v1" });
                options.SwaggerDoc("角色分配权限", new OpenApiInfo { Title = "角色分配权限管理", Version = "v1" });
                options.SwaggerDoc("科目管理", new OpenApiInfo { Title = "科目管理管理", Version = "v1" });
                
                options.DocInclusionPredicate((doc, desc) =>
                {
                    if (!desc.GroupName.IsNullOrWhiteSpace())
                    {
                        return doc == desc.GroupName;
                    }
                    return true;
                });

                options.HideAbpEndpoints();
                options.CustomSchemaIds(type => type.FullName);

                // JWT Bearer认证配置
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT认证（直接输入Token，无需加'Bearer '前缀）", // 简化的中文描述
                    Name = "Authorization",        // HTTP头部字段名
                    In = ParameterLocation.Header, // Token位置（请求头）
                    Type = SecuritySchemeType.Http,// 认证类型
                    Scheme = "bearer",            // 认证方案
                    BearerFormat = "JWT"          // Token格式
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
                });
            }
        );
    }

    private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
    {

        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(configuration["App:CorsOrigins"]?
                        .Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .Select(o => o.RemovePostFix("/"))
                        .ToArray() ?? Array.Empty<string>())
                    .WithAbpExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();

        if (!env.IsDevelopment())
        {
            app.UseErrorPage();
        }

        app.UseCorrelationId();
        app.MapAbpStaticAssets();
        app.UseRouting();
        app.UseCors();
        app.UseAuthentication();
        //app.UseAbpOpenIddictValidation();

        //if (MultiTenancyConsts.IsEnabled)
        //{
        //    app.UseMultiTenancy();
        //}

        //ѩ��Id
        YitIdHelper.SetIdGenerator(new IdGeneratorOptions(1));

        app.UseUnitOfWork();
        app.UseDynamicClaims();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseAbpSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/公告/swagger.json", "公告管理 v1");
            c.SwaggerEndpoint("/swagger/课程/swagger.json", "课程管理 v1");
            c.SwaggerEndpoint("/swagger/组织机构/swagger.json", "组织机构管理 v1");
            c.SwaggerEndpoint("/swagger/职位/swagger.json", "职位管理 v1");
            c.SwaggerEndpoint("/swagger/权限/swagger.json", "权限管理 v1");
            c.SwaggerEndpoint("/swagger/角色/swagger.json", "角色管理 v1");
            c.SwaggerEndpoint("/swagger/专题/swagger.json", "专题管理 v1");
            c.SwaggerEndpoint("/swagger/成员/swagger.json", "成员管理 v1");
            c.SwaggerEndpoint("/swagger/科目管理/swagger.json", "科目管理 v1");
            
            var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
            c.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
            c.OAuthScopes("Educational");
        });

        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }
}
