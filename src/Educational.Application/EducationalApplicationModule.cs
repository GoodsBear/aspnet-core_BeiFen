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
    typeof(EducationalDomainSharedModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpAutoMapperModule),
    (typeof(AbpSwashbuckleModule))
    )] 
 public class EducationalApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<EducationalApplicationModule>(validate: false); // 临时关闭验证
        });
        //Configure<AbpAutoMapperOptions>(options =>
        //{
        //    options.AddMaps<EducationalApplicationModule>();
        //});
    }
}
