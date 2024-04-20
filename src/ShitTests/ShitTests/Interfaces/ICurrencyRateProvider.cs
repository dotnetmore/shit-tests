namespace ShitTests.Interfaces;

public interface ICurrencyRateProvider
{
    decimal GetRate(string currency);
}