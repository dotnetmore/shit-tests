namespace ShitTests;

public interface ICurrencyRateProvider
{
    decimal GetRate(string currency);
}