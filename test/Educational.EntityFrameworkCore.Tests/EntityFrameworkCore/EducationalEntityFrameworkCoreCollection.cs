using Xunit;

namespace Educational.EntityFrameworkCore;

[CollectionDefinition(EducationalTestConsts.CollectionDefinitionName)]
public class EducationalEntityFrameworkCoreCollection : ICollectionFixture<EducationalEntityFrameworkCoreFixture>
{

}
