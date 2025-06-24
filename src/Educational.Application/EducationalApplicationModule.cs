using CSRedis;
using Educational.StaffTypes;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;

namespace Educational;

[DependsOn(
    typeof(EducationalDomainModule),
    typeof(EducationalApplicationContractsModule),
    typeof(AbpSettingManagementApplicationModule),
    typeof(EducationalDomainSharedModule)
    )]
public class EducationalApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<EducationalApplicationModule>();
        });
    }
}
