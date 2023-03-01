using Domain.Primitives;

namespace Domain.ValueObjects;

public sealed class CompanyName : LengthRestrictedValueObject<CompanyName>, IRanged<uint>
{
    Range<uint> IRanged<uint>.Range { get; } = Range<uint>.Create(2, 50);
}