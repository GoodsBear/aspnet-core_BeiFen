using Volo.Abp.Settings;

namespace Educational.Settings;

public class EducationalSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(EducationalSettings.MySetting1));
    }
}
