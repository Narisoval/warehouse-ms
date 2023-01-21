using System.Text;

namespace Domain.UnitTests.Fixtures.Generators;

public static class ProductNameGenerator
{
    private const int MinimalProductNameLength = 10;
    private const int MaximalProductNameLength = 70;
    
    public static IEnumerable<object[]> GenerateProductNames()
    {
        StringBuilder stringToReturn = new StringBuilder("");
        for (int i = 0; i < MinimalProductNameLength; i++)
        {
            stringToReturn.Append(i);
            yield return new object[]{stringToReturn.ToString()};
        }
        yield return new object[]{new string('*',MaximalProductNameLength+1)};
    }
}