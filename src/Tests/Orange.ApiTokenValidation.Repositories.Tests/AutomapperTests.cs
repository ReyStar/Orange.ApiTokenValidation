using AutoMapper;
using NUnit.Framework;

namespace Orange.ApiTokenValidation.Repositories.Tests
{
    public class AutoMapperTests
    {
        [Test]
        public void MapperConfigurationTest()
        {
            // Arrange
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });

            // Act & Assert
            mapperConfiguration.AssertConfigurationIsValid();
        }
    }
}
