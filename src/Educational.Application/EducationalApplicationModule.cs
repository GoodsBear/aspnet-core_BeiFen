using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;

namespace Educational;

[DependsOn(
    typeof(EducationalDomainModule),
    typeof(EducationalApplicationContractsModule),
    typeof(AbpSettingManagementApplicationModule)
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
