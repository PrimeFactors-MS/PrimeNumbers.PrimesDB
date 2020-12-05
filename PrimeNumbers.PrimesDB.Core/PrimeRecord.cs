namespace PrimeNumbers.PrimesDB.Core
{
    public record PrimeRecord(ulong Number, bool IsPrime, ulong[] PrimeFactors);
}
