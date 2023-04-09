using Google.Api.Gax.Grpc.Rest;
using Google.Cloud.Vision.V1;
using Microsoft.AspNetCore.Mvc;
using OCRApiModels;

namespace OCRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OCRController : ControllerBase
    {
        private readonly ILogger<OCRController> _logger;

        public OCRController(ILogger<OCRController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TextFromHandWritingDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post(TextFromHandWritingDto textFromHandWritingDto)
        {
            try
            {
                var client = new ImageAnnotatorClientBuilder
                {
                    GrpcAdapter = RestGrpcAdapter.Default
                }.Build();

                var image = Image.FetchFromUri(textFromHandWritingDto.ImageUrl);

                var responseText = "";

                IReadOnlyList<EntityAnnotation> textAnnotations = await client.DetectTextAsync(image);
                foreach (EntityAnnotation text in textAnnotations)
                {
                    Console.WriteLine($"Description: {text.Description}");
                    responseText += " " + text.Description;
                }

                return Ok(responseText);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }
    }
}
