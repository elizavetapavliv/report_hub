using AutoFixture;
using AutoMapper;
using Exadel.ReportHub.Host.Mapping.Profiles;

namespace Exadel.ReportHub.Tests.Abstracts
{
    public abstract class BaseTestFixture
    {
        protected static IMapper Mapper { get; }

        protected Fixture Fixture { get; }

        static BaseTestFixture()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile<UserProfile>());
            Mapper = configuration.CreateMapper();
        }

        protected BaseTestFixture()
        {
            Fixture = new Fixture();
        }
    }
}
