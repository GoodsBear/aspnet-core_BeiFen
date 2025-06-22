using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.SettingManagement;

namespace Educational;

[DependsOn(
    typeof(EducationalDomainSharedModule),
    typeof(AbpSettingManagementApplicationContractsModule),
    typeof(AbpObjectExtendingModule)
)]
public class EducationalApplicationContractsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        EducationalDtoExtensions.Configure();
    }
}
