using Microsoft.Extensions.Localization;
using Educational.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Educational;

[Dependency(ReplaceServices = true)]
public class EducationalBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<EducationalResource> _localizer;

    public EducationalBrandingProvider(IStringLocalizer<EducationalResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
