using Volo.Abp.Modularity;

namespace Educational;

[DependsOn(
    typeof(EducationalDomainModule),
    typeof(EducationalTestBaseModule)
)]
public class EducationalDomainTestModule : AbpModule
{

}
