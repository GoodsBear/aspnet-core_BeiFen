using Educational.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Educational.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class EducationalController : AbpControllerBase
{
    protected EducationalController()
    {
        LocalizationResource = typeof(EducationalResource);
    }
}
