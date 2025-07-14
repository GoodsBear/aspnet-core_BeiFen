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
using Serilog; // Added for console output, can be removed after debugging
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims; // 新增命名空间
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;

//using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite;
//using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
//using Volo.Abp.OpenIddict; // 移除此命名空间，如果不再使用 OpenIddict
using Volo.Abp.Security.Claims;
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
    //typeof(AbpAspNetCoreMvcUiLeptonXLiteThemeModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule)
// 移除此处对 typeof(AbpOpenIddictAspNetCoreModule) 的依赖
)]
public class EducationalHttpApiHostModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        // 此处不再需要任何 OpenIddict 相关的预配置
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {

        Configure<AbpAntiForgeryOptions>(options =>
        {
            options.TokenCookie.Expiration = TimeSpan.FromDays(365);
            options.AutoValidate = false;
        });


        // 配置认证
        var configuration = context.Services.GetConfiguration();
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        // 添加验证码服务（基于配置）
        context.Services.AddCaptcha(context.Services.GetConfiguration());


        ConfigureAuthentication(context, configuration);
        //ConfigureAuthentication(context); // 避免重复调用
        //ConfigureBundles(); // 保持注释，如果您不需要 Bundles 配置
        ConfigureUrls(configuration);
        ConfigureConventionalControllers();
        ConfigureVirtualFileSystem(context);
        ConfigureCors(context, configuration);
        ConfigureSwaggerServices(context);
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:SecurityKey"])),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                // 【关键：在这里添加 Events 用于调试和 Claims 处理】
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        // 设置断点，查看 JWT 验证是否失败及原因
                        Log.Error($"JWT 认证失败: {context.Exception?.Message}");
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        // 设置断点，查看 JWT Token 是否验证成功。
                        // 如果进入这里，说明 Token 本身没问题，问题出在 Claims 处理上。
                        Log.Information("JWT Token 验证成功！开始处理 Claims...");

                        var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                        if (claimsIdentity != null)
                        {
                            // 原始 JWT Token 中的 Claims (根据您的截图进行映射)
                            var originalIdClaim = claimsIdentity.FindFirst("Id"); // JWT Payload 中的 "Id"
                            var originalStaffAccountClaim = claimsIdentity.FindFirst("StaffAccount"); // JWT Payload 中的 "StaffAccount"
                            var originalRolesClaim = claimsIdentity.FindFirst("Roles"); // JWT Payload 中的 "Roles"
                            var originalPermsClaim = claimsIdentity.FindFirst("Perms"); // JWT Payload 中的 "Perms"

                            // 1. 添加 UserId (对应 Id)
                            if (originalIdClaim != null && !string.IsNullOrWhiteSpace(originalIdClaim.Value))
                            {
                                // 添加标准 NameIdentifier
                                if (!claimsIdentity.HasClaim(ClaimTypes.NameIdentifier, originalIdClaim.Value))
                                {
                                    claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, originalIdClaim.Value));
                                }
                                // 添加 AbpUserId
                                if (!claimsIdentity.HasClaim(AbpClaimTypes.UserId, originalIdClaim.Value))
                                {
                                    claimsIdentity.AddClaim(new Claim(AbpClaimTypes.UserId, originalIdClaim.Value));
                                }
                            }

                            // 2. 添加 UserName (对应 StaffAccount)
                            if (originalStaffAccountClaim != null && !string.IsNullOrWhiteSpace(originalStaffAccountClaim.Value))
                            {
                                // 添加标准 Name Claim (如果需要)
                                if (!claimsIdentity.HasClaim(ClaimTypes.Name, originalStaffAccountClaim.Value))
                                {
                                    claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, originalStaffAccountClaim.Value));
                                }
                                // 添加 AbpUserName
                                if (!claimsIdentity.HasClaim(AbpClaimTypes.UserName, originalStaffAccountClaim.Value))
                                {
                                    claimsIdentity.AddClaim(new Claim(AbpClaimTypes.UserName, originalStaffAccountClaim.Value));
                                }
                            }

                            // 3. 处理 Roles Claim (逗号分隔的字符串)
                            if (originalRolesClaim != null && !string.IsNullOrWhiteSpace(originalRolesClaim.Value))
                            {
                                var roles = originalRolesClaim.Value.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                                          .Select(r => r.Trim())
                                                          .Where(r => !string.IsNullOrWhiteSpace(r));
                                foreach (var role in roles)
                                {
                                    if (!claimsIdentity.HasClaim(AbpClaimTypes.Role, role))
                                    {
                                        claimsIdentity.AddClaim(new Claim(AbpClaimTypes.Role, role));
                                    }
                                }
                                // 移除原始的 "Roles" claim，因为它已被解析并添加为多个单独的 Claim
                                claimsIdentity.RemoveClaim(originalRolesClaim);
                            }

                            // 4. 处理 Perms Claim (逗号分隔的字符串)
                            if (originalPermsClaim != null && !string.IsNullOrWhiteSpace(originalPermsClaim.Value))
                            {
                                var permissions = originalPermsClaim.Value.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                                                 .Select(p => p.Trim())
                                                                 .Where(p => !string.IsNullOrWhiteSpace(p));
                                //foreach (var permission in permissions)
                                //{
                                //    if (!claimsIdentity.HasClaim(AbpClaimTypes.Permission, permission))
                                //    {
                                //        claimsIdentity.AddClaim(new Claim(AbpClaimTypes.Permission, permission));
                                //    }
                                //}
                                // 移除原始的 "Perms" claim
                                claimsIdentity.RemoveClaim(originalPermsClaim);
                            }

                            // 【重要：在这里设置断点！】
                            // 检查 context.Principal.Identity.Claims 集合，
                            // 确认 ClaimsTypes.NameIdentifier, AbpClaimTypes.UserId, AbpClaimTypes.UserName, AbpClaimTypes.Role, AbpClaimTypes.Permission 是否正确存在且值正确
                            Log.Information("Claims 处理完成。当前 Principal 中的 Claims:");
                            Log.Information($"ClaimsPrincipal 是否认证: {context.Principal.Identity.IsAuthenticated}");
                            foreach (var claim in context.Principal.Claims)
                            {
                                Log.Information($"  Claim Type: {claim.Type}, Claim Value: {claim.Value}");
                            }

                            // 将修改后的 ClaimsPrincipal 赋值回 context
                            // 这一步确保后续中间件使用更新后的 Principal
                            context.Principal = new ClaimsPrincipal(claimsIdentity);
                        }
                        return Task.CompletedTask;
                    },
                    OnMessageReceived = context =>
                    {
                        // 设置断点，检查 Token 是否被正确从请求中提取
                        Log.Information($"收到 Token (部分): {context.Token?.Substring(0, Math.Min(context.Token.Length, 30))}...");
                        return Task.CompletedTask;
                    }
                };
            });

        // Claims 映射配置 (保留，它仍然会被 AbpClaimsPrincipalFactory 和 UseDynamicClaims 利用)
        Configure<AbpClaimsPrincipalFactoryOptions>(options =>
        {
            options.ClaimsMap[Volo.Abp.Security.Claims.AbpClaimTypes.UserId] = new List<string> { "Id" };
            options.ClaimsMap[Volo.Abp.Security.Claims.AbpClaimTypes.UserName] = new List<string> { "StaffAccount" };
            options.ClaimsMap[Volo.Abp.Security.Claims.AbpClaimTypes.Role] = new List<string> { "Roles" }; // 虽然我们在 OnTokenValidated 中处理了，但这里作为映射配置保留
            //options.ClaimsMap[Volo.Abp.Security.Claims.AbpClaimTypes.Permission] = new List<string> { "Perms" }; // 同样保留映射
            options.IsDynamicClaimsEnabled = true; // 确保动态 Claims 启用
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
                options.SwaggerDoc("报名记录", new OpenApiInfo { Title = "报名记录管理", Version = "v1" });
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
                options.SwaggerDoc("节假日", new OpenApiInfo { Title = "节假日管理", Version = "v1" });
                
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
                    Name = "Authorization",       // HTTP头部字段名
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
        // app.UseAbpOpenIddictValidation(); // 【重要：保持此行注释状态】

        //if (MultiTenancyConsts.IsEnabled)
        //{
        //    app.UseMultiTenancy();
        //}
        app.UseDynamicClaims(); // 此中间件会处理 ClaimsPrincipal 并填充 ICurrentUser

        //雪花Id
        YitIdHelper.SetIdGenerator(new IdGeneratorOptions(1));

        app.UseUnitOfWork();
        app.UseAuthorization(); // 必须在 UseDynamicClaims 之后，因为它依赖于填充好的 ICurrentUser

        app.UseSwagger();
        app.UseAbpSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/菜单/swagger.json", "菜单管理 v1");
            c.SwaggerEndpoint("/swagger/学员/swagger.json", "学员管理 v1");
            c.SwaggerEndpoint("/swagger/跟进/swagger.json", "跟进管理 v1");
            c.SwaggerEndpoint("/swagger/积分/swagger.json", "积分管理 v1");
            c.SwaggerEndpoint("/swagger/报名记录/swagger.json", "报名记录管理 v1");
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
            c.SwaggerEndpoint("/swagger/节假日/swagger.json", "节假日管理 v1");
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