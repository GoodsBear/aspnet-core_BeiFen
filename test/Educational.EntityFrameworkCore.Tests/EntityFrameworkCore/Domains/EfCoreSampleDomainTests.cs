using Educational.Samples;
using Xunit;

namespace Educational.EntityFrameworkCore.Domains;

[Collection(EducationalTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<EducationalEntityFrameworkCoreTestModule>
{

}
