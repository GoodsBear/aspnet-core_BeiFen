using System;
using System.Collections.Generic;
using System.Text;
using Educational.Localization;
using Volo.Abp.Application.Services;

namespace Educational;

/* Inherit your application services from this class.
 */
public abstract class EducationalAppService : ApplicationService
{
    protected EducationalAppService()
    {
        LocalizationResource = typeof(EducationalResource);
    }
}
