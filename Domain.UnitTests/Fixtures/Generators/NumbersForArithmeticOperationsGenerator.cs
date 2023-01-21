namespace Domain.UnitTests.Fixtures.Generators;

public static class NumbersForArithmeticOperationsGenerator
{
    private const int ArithmeticTestsNumber = 15;
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
}