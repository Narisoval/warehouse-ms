using System.Text;

namespace Domain.UnitTests.Entities.Product;

public static class Generators
{
    private const int ArithmeticTestsNumber = 15;
    private const int MinimalProductNameLength = 10;
    private const int MaximalProductNameLength = 70;
    
    private const int MinimalDescriptionLength = 30 ;
    private const int MaximalDescriptionLength = 1200 ;
    public static IEnumerable<object[]> GenerateNumbersForSum()
    {
        var rnd = new Random();
        for (int i = 0; i < ArithmeticTestsNumber; i++)
        {
            int firstNum = rnd.Next(50000);
            int secondNum = rnd.Next(50000);
            yield return new object[] { firstNum, secondNum, firstNum + secondNum };
        }     
    }
    
    public static IEnumerable<object[]> GenerateNumbersForSubtraction()
    {
        var rnd = new Random();
        for (int i = 0; i < ArithmeticTestsNumber; i++)
        {
            int firstNum = rnd.Next(50000);
            int secondNum = rnd.Next(firstNum);
            yield return new object[] { firstNum, secondNum, firstNum - secondNum };
        }
    }
    
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
    
    public static IEnumerable<object[]> GenerateDescriptions()
    {
        for (int i = 0; i < MinimalDescriptionLength; i++)
        {
            yield return new object[]{new string('*',i)};
        }
        yield return new object[]{new string('*',MaximalDescriptionLength+1)};
    }


}