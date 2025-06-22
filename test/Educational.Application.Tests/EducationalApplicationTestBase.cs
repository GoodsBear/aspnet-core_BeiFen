using Volo.Abp.Modularity;

namespace Educational;

public abstract class EducationalApplicationTestBase<TStartupModule> : EducationalTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
