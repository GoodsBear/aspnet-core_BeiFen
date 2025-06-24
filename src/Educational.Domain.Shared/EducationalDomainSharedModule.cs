using CSRedis;
using Educational.Localization;
using Educational.Tools;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AuditLogging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Educational;

[DependsOn(
    typeof(AbpAuditLoggingDomainSharedModule),
    typeof(AbpBackgroundJobsDomainSharedModule),
    typeof(AbpSettingManagementDomainSharedModule)
    )]
public class EducationalDomainSharedModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        EducationalGlobalFeatureConfigurator.Configure();
        EducationalModuleExtensionConfigurator.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // 配置Redis客户端
        var redisConfiguration = context.Services.GetConfiguration().GetSection("Redis");
        var redisConnection = redisConfiguration["Connection"];

        // 注册CSRedis客户端
        context.Services.AddSingleton<CSRedisClient>(new CSRedisClient(redisConnection));

        // 注册你的Redis帮助类
        context.Services.AddTransient(typeof(RedisHelp<>));

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<EducationalDomainSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<EducationalResource>("en")
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("/Localization/Educational");

            options.DefaultResourceType = typeof(EducationalResource);
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("Educational", typeof(EducationalResource));
        });
    }
}
