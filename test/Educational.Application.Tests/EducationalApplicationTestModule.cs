using Volo.Abp.Modularity;

namespace Educational;

[DependsOn(
    typeof(EducationalApplicationModule),
    typeof(EducationalDomainTestModule)
)]
public class EducationalApplicationTestModule : AbpModule
{

}
