using Educational.Samples;
using Xunit;

namespace Educational.EntityFrameworkCore.Applications;

[Collection(EducationalTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<EducationalEntityFrameworkCoreTestModule>
{

}
