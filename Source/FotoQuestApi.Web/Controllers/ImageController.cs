using Microsoft.AspNetCore.Mvc;

namespace FotoQuestApi.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class ImageController : ControllerBase
{
    private readonly ILogger<ImageController> _logger;
    private readonly IImageCoordinator imageCoordinator;
    private readonly ImageConfigration imageConfigration;

    public ImageController(ILogger<ImageController> logger,
                            IImageCoordinator imageCoordinator,
                            ImageConfigration imageConfigration)
    {
        _logger = logger;
        this.imageCoordinator = imageCoordinator;
        this.imageConfigration = imageConfigration;
    }

    [HttpPost("UploadFile")]
    [ProducesResponseType(typeof(FilePersistanceSuccessResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(FilePersistanceFailedResponse), StatusCodes.Status500InternalServerError)]
    public async Task<FilePersistanceSuccessResponse> UploadFile([FromForm] FileUpload fileUpload)
        => await imageCoordinator.PersistImage(fileUpload);

    [HttpGet("Thumbnail/{fileId}")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(FilePersistanceFailedResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetThumbnail([FromRoute] string fileId)
        => await Response(imageConfigration.Thumbnail, fileId);

    [HttpGet("Small/{fileId}")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(FilePersistanceFailedResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetSmall([FromRoute] string fileId)
        => await Response(imageConfigration.Small, fileId);

    [HttpGet("Large/{fileId}")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(FilePersistanceFailedResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetLarge([FromRoute] string fileId)
        => await Response(imageConfigration.Large, fileId);

    [HttpGet("{customSize}/{fileId}")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(FilePersistanceFailedResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCustom([FromRoute] int customSize, string fileId)
    {
        return await Response(customSize, fileId);
    }

    private new async Task<IActionResult> Response(int size, string fileId)
    {
        return File(await imageCoordinator.GetImage(fileId, size), "image/png");

    }
}
