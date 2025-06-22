using Volo.Abp.Modularity;

namespace Educational;

/* Inherit from this class for your domain layer tests. */
public abstract class EducationalDomainTestBase<TStartupModule> : EducationalTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
