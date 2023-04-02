using Domain.Primitives;

namespace Domain.ValueObjects;

public sealed class CompanyName : LengthRestrictedValueObject<CompanyName>
{
    internal override Range<int> LengthRange { get; } = Range<int>.Create(2, 50);
}