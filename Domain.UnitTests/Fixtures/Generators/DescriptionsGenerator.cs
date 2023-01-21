namespace Domain.UnitTests.Fixtures.Generators;

public static class DescriptionsGenerator
{
    private const int MinimalDescriptionLength = 30 ;
    private const int MaximalDescriptionLength = 1200 ;
    
    public static IEnumerable<object[]> GenerateDescriptions()
    {
        for (int i = 0; i < MinimalDescriptionLength; i++)
        {
            yield return new object[]{new string('*',i)};
        }
        yield return new object[]{new string('*',MaximalDescriptionLength+1)};
    }
}