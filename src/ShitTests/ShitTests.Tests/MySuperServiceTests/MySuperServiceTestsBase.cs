using NSubstitute;
using ShitTests.Interfaces;

namespace ShitTests.Tests.MySuperServiceTests;

public abstract class MySuperServiceTestsBase
{
    protected readonly IDatabase MockDatabase = Substitute.For<IDatabase>();
    protected readonly ICache Cache = Substitute.For<ICache>();
    protected readonly ICurrencyRateProvider CurrencyRateProvider = Substitute.For<ICurrencyRateProvider>();
    protected readonly ISecurityChecker SecurityChecker = Substitute.For<ISecurityChecker>();
    protected readonly IEmploymentService EmploymentService = Substitute.For<IEmploymentService>();
    protected readonly ICashier Cashier = Substitute.For<ICashier>();
    protected readonly IEmailSender EmailSender = Substitute.For<IEmailSender>();
    protected readonly IGovernmentService GovernmentService = Substitute.For<IGovernmentService>();
    protected readonly IWeatherService WeatherService = Substitute.For<IWeatherService>();
    protected readonly ITimeService TimeService = Substitute.For<ITimeService>();
    protected readonly IEmailRegistry EmailRegistry = Substitute.For<IEmailRegistry>();


    protected MySuperService MySuperService => new(MockDatabase, Cache, CurrencyRateProvider,
        SecurityChecker, EmploymentService, Cashier,
        EmailSender, GovernmentService, WeatherService,
        TimeService, EmailRegistry, new MySimpleService());
}