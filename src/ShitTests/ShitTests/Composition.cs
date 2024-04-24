using Pure.DI;
using ShitTests.Interfaces;
using ShitTests.Services;

namespace ShitTests;

public partial class Composition
{
    private void Setup() =>
        DI.Setup(nameof(Composition))
            .Bind().To<EmploymentService>()
            .Bind().To<MySimpleService>()
            .Bind().To<CurrencyRateProvider>()
            .Bind().To<SecurityChecker>()
            .Bind().To<Cashier>()
            .Bind().To<GovernmentService>()
            .Bind().To<WeatherService>()
            .Bind().To<EmailSender>()
            .Bind().To<EmailRegistry>()
            .Bind().To<TimeService>()
            .Bind().To<Cache>()
            .Bind().To<Database>()
            .Bind().To<UserRegistry>()
            .Bind().To<EmployeeBasedUserSalaryCalculator>()
            .Bind().To<CurrencyBasedUserSalaryCalculator>()
            .Bind().To<SafeUserSalaryCalculator>()
            .Bind().To<WeatherBasedUserSalaryCalculator>()
            .Bind().To<NotifyingUserSalaryCalculator>()
            .Bind().To<CachingUserSalaryCalculator>()
            .RootBind<MySuperService>(nameof(MySuperService)).To<MySuperService>();   
}