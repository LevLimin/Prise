using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Prise.IntegrationTests
{
    // These tests do not succeed (System.PlatformNotSupportedException: Named maps are not supported)
#if NETCORE3_0 || NETCORE3_1
    public class Translation2Tests : TranslationTestsBase
    {
        public Translation2Tests() : base(AppHostWebApplicationFactory.Default()) { }

        [Fact]
        public async Task PluginG_DE_Works()
        {
            // Arrange, Act
            var results = await GetTranslations(_client, "de-DE", "/translation2?&input=dog");

            // Assert
            Assert.Equal(2, results.Count());
        }

        [Fact]
        public async Task PluginG_FR_Works()
        {
            // Arrange, Act
            var results = await GetTranslations(_client, "fr-FR", "/translation2?&input=dog");

            // Assert
            Assert.Equal(2, results.Count());
        }

        [Fact]
        public async Task PluginG_NL_Works()
        {
            // Arrange, Act
            var results = await GetTranslations(_client, "nl-BE", "/translation2?&input=dog");

            // Assert
            Assert.Equal(2, results.Count());
        }

        [Fact]
        public async Task PluginG_EN_Returns_Empty_List()
        {
            // Arrange, Act
            var results = await GetTranslations(_client, "en-GB", "/translation2?&input=dog");

            // Assert
            Assert.Equal(0, results.Count());
        }

        [Theory]
        [InlineData("fr-FR", "cat", "Chat")]
        [InlineData("fr-FR", "hello", "Bonjour")]
        [InlineData("fr-FR", "goodbye", "Au revoir")]
        [InlineData("de-DE", "cat", "Katze")]
        [InlineData("de-DE", "hello", "Guten Tag")]
        [InlineData("de-DE", "goodbye", "Auf Wiedersehen")]
        [InlineData("nl-BE", "cat", "Kat")]
        [InlineData("nl-BE", "hello", "Hallo")]
        [InlineData("nl-BE", "goodbye", "Tot ziens")]
        public async Task PluginG_Works(string culture, string input, string result)
        {
            // Arrange, Act
            var results = await GetTranslations(_client, culture, $"/translation2?&input={input}");

            // Assert
            Assert.Equal(1, results.Count());
            Assert.Equal(result, results.First().Translation);
        }
    }
#endif
}
