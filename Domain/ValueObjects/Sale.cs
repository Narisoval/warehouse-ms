using Domain.Validation;
using ValueOf;
namespace Domain.ValueObjects;

public class Sale : ValueOf<decimal,Sale>
{
    private static readonly Range<decimal> SaleRange = Range<decimal>.Create(0M, 100M);

    protected override void Validate()
    {
        if (Value < SaleRange.Min || Value > SaleRange.Max)
        {
            
        }
    }
}