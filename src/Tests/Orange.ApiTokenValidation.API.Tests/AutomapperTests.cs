using AutoMapper;
using NUnit.Framework;

namespace Orange.ApiTokenValidation.API.Tests
{
    public class AutoMapperTests
    {
        [Test]
        public void MapperConfigurationTest()
        {
            // Arrange
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<Registration.AutoMapperProfile>();
            });

            // Act & Assert
            mapperConfiguration.AssertConfigurationIsValid();
        }
    }
}
