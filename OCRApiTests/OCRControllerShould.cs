using Microsoft.Extensions.Logging;
using NUnit.Framework.Internal;
using NUnit.Framework;
using OCRApi.Controllers;
using OCRApiModels;
using Microsoft.AspNetCore.Mvc;

namespace OCRApiTests
{
    [TestFixture]
    public class OCRControllerShould
    {
        private ILogger<OCRController> _logger;

        [SetUp]
        public void Setup()
        {
            _logger = new Logger<OCRController>(new LoggerFactory());
        }

        [Test]
        public async Task ReturnTextFromImage()
        {
            var sut = new OCRController(_logger);

            var handwritingDto = new TextFromHandWritingDto()
            {
                ImageUrl = "https://i.stack.imgur.com/i1Abv.png"
            };

            var result = await sut.Post(handwritingDto);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.InstanceOf(typeof(OkObjectResult)));
                Assert.That(((OkObjectResult)result).Value, Is.EqualTo(" It was the best of\ntimes, it was the worst\nof times, it was the age\nof wisdom, it was the\nage of foolishness... It was the best of times , it was the worst of times , it was the age of wisdom , it was the age of foolishness ..."));
            });
        }
    }
}