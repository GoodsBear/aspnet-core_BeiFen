using CSRedis;
using Educational.StaffTypes;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;
using Volo.Abp.Swashbuckle;

namespace Educational;

[DependsOn(
    typeof(EducationalDomainModule),
    typeof(EducationalApplicationContractsModule),
    typeof(AbpSettingManagementApplicationModule),
    typeof(EducationalDomainSharedModule)
[DependsOn(typeof(AbpSwashbuckleModule))]
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<EducationalApplicationModule>();
        });
    }
}
