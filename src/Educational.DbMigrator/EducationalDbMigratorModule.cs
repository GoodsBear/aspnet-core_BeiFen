using Educational.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Educational.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(EducationalEntityFrameworkCoreModule),
    typeof(EducationalApplicationContractsModule)
    )]
public class EducationalDbMigratorModule : AbpModule
{
}
