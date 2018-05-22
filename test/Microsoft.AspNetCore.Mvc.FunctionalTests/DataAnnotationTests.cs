using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DataAnnotationsWebSite;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Microsoft.AspNetCore.Mvc.FunctionalTests
{
    public class DataAnnotationTests : IClassFixture<MvcTestFixture<DataAnnotationsWebSite.Startup>>
    {
        private MvcTestFixture<DataAnnotationsWebSite.Startup> _fixture;

        public DataAnnotationTests(MvcTestFixture<DataAnnotationsWebSite.Startup> fixture)
        {
            _fixture = fixture;
            Client = _fixture.CreateClient();
        }

        public HttpClient Client { get; set; }

        private const string EnumUrl = "http://localhost/Enum/Enum";

        [Fact]
        public async Task DataAnnotationLocalizationEnum_CompatSwitch()
        {
            // Arrange
            var factory = _fixture.WithWebHostBuilder(builder => builder.ConfigureServices(
                services => services.Configure<MvcCompatibilityOptions>(
                    options => options.CompatibilityVersion = CompatibilityVersion.Version_2_1)));
            var client = factory.CreateDefaultClient();

            // Act
            var response = await client.GetAsync(EnumUrl);
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Contains("first option display from localizer", content);
        }

        [Fact]
        public async Task DataAnnotationLocalizionOfEnums()
        {
            // Arrange & Act
            var response = await Client.GetAsync(EnumUrl);
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("first option display from localizer", content);
        }
    }

    public class DataAnnotationsOverriddenTests : IClassFixture<MvcTestFixture<DataAnnotationsWebSite.OverriddenStartup>>
    {
        private MvcTestFixture<DataAnnotationsWebSite.OverriddenStartup> _fixture;

        public DataAnnotationsOverriddenTests(MvcTestFixture<DataAnnotationsWebSite.OverriddenStartup> fixture)
        {
            _fixture = fixture;
        }

        private const string EnumUrl = "http://localhost/Enum/Enum";

        private void ConfigureServices(IWebHostBuilder builder, CompatibilityVersion version)
        {
            builder.ConfigureServices(
                services =>
                {
                    services.AddMvc()
                        .AddViewLocalization()
                        .AddDataAnnotationsLocalization((options) => {
                            options.DataAnnotationLocalizerProvider = (modelType, slf) => slf.Create(typeof(SingleType));
                        });
                    services.Configure<MvcCompatibilityOptions>(options => options.CompatibilityVersion = version);
                });
        }


        [Fact]
        public async Task DataAnnotationLocalizationEnum_CompatSwitch_OverrideProvider()
        {
            // Arrange
            var factory = _fixture.WithWebHostBuilder(builder => ConfigureServices(builder, CompatibilityVersion.Version_2_1));
            var client = factory.CreateDefaultClient();

            // Act
            var response = await client.GetAsync(EnumUrl);
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.DoesNotContain("FirstOptionDisplay from singletype", content);
            Assert.Contains("first option display from localizer", content);
        }

        [Fact]
        public async Task DataAnnotationLocalizionOfEnums_FromDataAnnotationLocalizerProvider()
        {
            // Arrange
            var factory = _fixture.WithWebHostBuilder(builder => ConfigureServices(builder, CompatibilityVersion.Latest));
            var client = factory.CreateDefaultClient();

            // Act
            var response = await client.GetAsync(EnumUrl);
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("FirstOptionDisplay from singletype", content);
        }
    }
}
