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
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;
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
        // 添加验证码服务（基于配置）
        context.Services.AddCaptcha(context.Services.GetConfiguration());


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
                options.SwaggerDoc("菜单", new OpenApiInfo { Title = "菜单管理", Version = "v1" });
                options.SwaggerDoc("学员", new OpenApiInfo { Title = "学员管理", Version = "v1" });
                options.SwaggerDoc("跟进", new OpenApiInfo { Title = "跟进管理", Version = "v1" });
                options.SwaggerDoc("积分", new OpenApiInfo { Title = "积分管理", Version = "v1" });
                options.SwaggerDoc("公告", new OpenApiInfo { Title = "公告管理", Version = "v1" });
                options.SwaggerDoc("课程", new OpenApiInfo { Title = "课程管理", Version = "v1" });
                options.SwaggerDoc("排课", new OpenApiInfo { Title = "排课管理", Version = "v1" });
                options.SwaggerDoc("科目", new OpenApiInfo { Title = "科目管理", Version = "v1" });
                options.SwaggerDoc("年级", new OpenApiInfo { Title = "年级管理", Version = "v1" });
                options.SwaggerDoc("班级", new OpenApiInfo { Title = "班级管理", Version = "v1" });
                options.SwaggerDoc("教室", new OpenApiInfo { Title = "教室管理", Version = "v1" });
                options.SwaggerDoc("组织机构", new OpenApiInfo { Title = "组织机构管理", Version = "v1" });
                options.SwaggerDoc("职位", new OpenApiInfo { Title = "职位管理", Version = "v1" });
                options.SwaggerDoc("权限", new OpenApiInfo { Title = "权限管理", Version = "v1" });
                options.SwaggerDoc("角色", new OpenApiInfo { Title = "角色管理", Version = "v1" });
                options.SwaggerDoc("专题", new OpenApiInfo { Title = "专题管理", Version = "v1" });
                options.SwaggerDoc("成员", new OpenApiInfo { Title = "成员管理", Version = "v1" });
                options.SwaggerDoc("薪资", new OpenApiInfo { Title = "薪资管理", Version = "v1" });
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
                //向每个接口的 Swagger 文档中添加自定义响应头信息，便于前端或测试人员了解接口返回的 header。
                options.OperationFilter<AddResponseHeadersFilter>();
                //自动在需要授权的接口的 summary 说明中添加“需要授权”字样，方便在 Swagger UI 上直观区分哪些接口需要登录或权限。
                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                //给需要授权的接口自动加上小锁标识（即 Swagger UI 上的“锁”图标），并自动生成授权相关的说明和参数。
                options.OperationFilter<SecurityRequirementsOperationFilter>();
                //让 Swagger 生成的模型（Schema）ID 使用完整类名，避免不同命名空间下同名类冲突。
                options.CustomSchemaIds(type => type.FullName);
                //获取应用程序运行时的根目录。
                var basePath = AppDomain.CurrentDomain.BaseDirectory;
                // 添加Application层的XML注释
                var applicationXmlPath = Path.Combine(basePath, "Educational.Application.xml");
                if (File.Exists(applicationXmlPath))
                {
                    //启用XML注释
                    options.IncludeXmlComments(applicationXmlPath);
                }

                // 添加HttpApi层的XML注释
                var httpApiXmlPath = Path.Combine(basePath, "Educational.HttpApi.Host.xml");
                if (File.Exists(httpApiXmlPath))
                {
                    //启用XML注释
                    options.IncludeXmlComments(httpApiXmlPath);
                }
                //隐藏 ABP 框架自动生成的一些基础端点，只展示你自己定义的 API，更加简洁。
                options.HideAbpEndpoints();

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

        //雪花Id
        YitIdHelper.SetIdGenerator(new IdGeneratorOptions(1));

        app.UseUnitOfWork();
        app.UseDynamicClaims();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseAbpSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/菜单/swagger.json", "菜单管理 v1");
            c.SwaggerEndpoint("/swagger/学员/swagger.json", "学员管理 v1");
            c.SwaggerEndpoint("/swagger/跟进/swagger.json", "跟进管理 v1");
            c.SwaggerEndpoint("/swagger/积分/swagger.json", "积分管理 v1");
            c.SwaggerEndpoint("/swagger/公告/swagger.json", "公告管理 v1");
            c.SwaggerEndpoint("/swagger/年级/swagger.json", "年级管理 v1");
            c.SwaggerEndpoint("/swagger/课程/swagger.json", "课程管理 v1");
            c.SwaggerEndpoint("/swagger/排课/swagger.json", "排课管理 v1");
            c.SwaggerEndpoint("/swagger/科目/swagger.json", "科目管理 v1");
            c.SwaggerEndpoint("/swagger/班级/swagger.json", "班级管理 v1");
            c.SwaggerEndpoint("/swagger/教室/swagger.json", "教室管理 v1");
            c.SwaggerEndpoint("/swagger/组织机构/swagger.json", "组织机构管理 v1");
            c.SwaggerEndpoint("/swagger/职位/swagger.json", "职位管理 v1");
            c.SwaggerEndpoint("/swagger/权限/swagger.json", "权限管理 v1");
            c.SwaggerEndpoint("/swagger/角色/swagger.json", "角色管理 v1");
            c.SwaggerEndpoint("/swagger/专题/swagger.json", "专题管理 v1");
            c.SwaggerEndpoint("/swagger/成员/swagger.json", "成员管理 v1");
            c.SwaggerEndpoint("/swagger/科目管理/swagger.json", "科目管理 v1");
            c.SwaggerEndpoint("/swagger/薪资/swagger.json", "薪资管理 v1");
            
            c.SwaggerEndpoint("/swagger/物料/swagger.json", "物料管理 v1");
            c.SwaggerEndpoint("/swagger/成员分配角色/swagger.json", "成员分配角色管理 v1"); 
            c.SwaggerEndpoint("/swagger/角色分配权限/swagger.json", "角色分配权限管理 v1");
            //设置模型（Model）在 Swagger UI 中默认展开的层级深度为1
            //接口参数或返回值是嵌套对象时，默认只展开一层，便于界面简洁
            c.DefaultModelExpandDepth(1);
            //设置文档的默认展开方式为“列表模式”
            //这样所有的 API 分组（如 Controller）在 Swagger UI 左侧会以列表形式全部展开，方便快速浏览所有接口
            c.DocExpansion(DocExpansion.List);
            // 设置模型渲染方式为“Model”，即优先显示模型结构（字段、类型等），而不是 Example（示例数据）。
            //这样开发者可以更直观地看到接口参数和返回值的结构。
            c.DefaultModelRendering(ModelRendering.Example);
            //设置模型默认展开深度为 -1，表示所有模型都折叠（不展开）。
            //这通常用于让页面更简洁，用户需要时再手动展开模型结构
            c.DefaultModelExpandDepth(-1);
            //设置 Swagger UI 的访问路径为 /swagger。
            c.RoutePrefix = "swagger";
            //注入自定义 CSS 样式表，路径为 /swagger-ui/custom.css。
            c.InjectStylesheet("/swagger-ui/custom.css");

            var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
            c.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
            c.OAuthScopes("Educational");
        });

        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }
}
