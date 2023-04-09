using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Webinex.Clippo.AspNetCore.Controllers.Mappers;
using Webinex.Coded.AspNetCore;

namespace Webinex.Clippo.AspNetCore.Controllers
{
    /// <summary>
    ///     Clippo base controller which serves client lib requests on /api/clippo
    /// </summary>
    /// <typeparam name="TClip">Clip type</typeparam>
    /// <typeparam name="TClipDto">Clip DTO type</typeparam>
    [Route("/api/clippo")]
    public class ClippoControllerBase<TClip, TClipDto> : ControllerBase
    {
        private readonly IClippoAspNetCoreControllerSettings _settings;
        private readonly IClippo<TClip> _clippo;
        private readonly IClipDtoMapper<TClip, TClipDto> _mapper;
        private readonly IOptions<JsonOptions> _jsonOptions;

        public ClippoControllerBase(
            IClippoAspNetCoreControllerSettings settings,
            IClippo<TClip> clippo,
            IClipDtoMapper<TClip, TClipDto> mapper, IOptions<JsonOptions> jsonOptions)
        {
            _settings = settings;
            _clippo = clippo;
            _mapper = mapper;
            _jsonOptions = jsonOptions;
        }

        /// <summary>
        ///     Gets clip by identifier
        /// </summary>
        /// <param name="id">Clip identifier. Required.</param>
        /// <returns>Found clip or coded result failure</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> ByIdAsync([Required] string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest();

            if (!await AuthorizeAsync())
                return Forbid();

            var result = await _clippo.ByIdAsync(id);
            if (!result.Succeed)
                return new CodedActionResult(result.Failure);

            var mapped = await _mapper.MapAsync(result.Payload);
            return Ok(mapped);
        }

        /// <summary>
        ///     Get clips by owner predicate
        /// </summary>
        /// <param name="ownerType">Owner type. Required.</param>
        /// <param name="ownerId">Owner Id. Required.</param>
        /// <param name="directory">Directory. Optional.</param>
        /// <returns>Found clips or coded result failure</returns>
        [HttpGet("by-owner")]
        public async Task<IActionResult> GetByOwnerAsync(
            [FromQuery] string ownerType,
            [FromQuery] string ownerId,
            [FromQuery] string directory)
        {
            if (string.IsNullOrWhiteSpace(ownerType) || string.IsNullOrWhiteSpace(ownerId))
                return BadRequest();

            if (!await AuthorizeAsync())
                return Forbid();

            var args = GetClipsArgs.ByOwner(ownerType, ownerId, directory);
            var result = await _clippo.GetAllValuesAsync(args);
            if (!result.Succeed)
                return new CodedActionResult(result.Failure);

            var mapped = await _mapper.MapAsync(result.Payload);
            return Ok(mapped);
        }

        /// <summary>
        ///     Gets clips by directory
        /// </summary>
        /// <param name="directory">Directory. Required.</param>
        /// <returns>Found clips or coded result failure</returns>
        [HttpGet("by-directory")]
        public async Task<IActionResult> GetByDirectoryAsync([FromQuery] string directory)
        {
            if (string.IsNullOrWhiteSpace(directory))
                return BadRequest();

            if (!await AuthorizeAsync())
                return Forbid();

            var args = GetClipsArgs.ByDirectory(directory);
            var result = await _clippo.GetAllValuesAsync(args);
            if (!result.Succeed)
                return new CodedActionResult(result.Failure);

            var mapped = await _mapper.MapAsync(result.Payload);
            return Ok(mapped);
        }

        /// <summary>
        ///     Gets clip file content by identifier
        /// </summary>
        /// <param name="id">Clip identifier. Required.</param>
        /// <returns>File content or coded result failure</returns>
        [HttpGet("{id}/content")]
        public async Task<IActionResult> ContentAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest();

            if (!await AuthorizeAsync())
                return Forbid();

            var result = await _clippo.ContentAsync(id, GetClipOptions.IncludeInactive);
            if (!result.Succeed)
                return new CodedActionResult(result.Failure);

            var contentDisposition = new ContentDisposition
                { Inline = true, FileName = result.Payload.FileName, Size = result.Payload.SizeBytes }.ToString();
            Response.Headers.Add(HeaderNames.ContentDisposition, contentDisposition);

            return new FileStreamResult(result.Payload.Value, result.Payload.MimeType);
        }

        /// <summary>
        ///     Stores file content as clip
        /// </summary>
        /// <param name="file">File to store. Required.</param>
        /// <param name="payloadJson">File payload json. Optional.</param>
        /// <returns>Newly created Clip or coded result failure</returns>
        [HttpPost]
        public async Task<IActionResult> StoreAsync(
            [FromForm(Name = "file")] IFormFile file,
            [FromForm(Name = "payload")] string payloadJson)
        {
            if (file == null)
                return BadRequest();

            if (!await AuthorizeAsync())
                return Forbid();

            var payload = payloadJson != null
                ? JsonSerializer.Deserialize<StoreClipPayload>(payloadJson, _jsonOptions.Value.JsonSerializerOptions)
                : StoreClipPayload.Empty;

            var stream = file.OpenReadStream();
            HttpContext.Response.RegisterForDispose(stream);

            var content = new StoreClipContent(file.FileName, file.ContentType, stream);
            var args = new StoreClipArgs(content, payload!.Actions, payload.Values);

            HttpContext.Response.RegisterForDispose(args.Content.Value);

            var result = await _clippo.StoreAsync(args);
            if (!result.Succeed)
                return new CodedActionResult(result.Failure);

            var mapped = await _mapper.MapAsync(result.Payload);
            return Ok(mapped);
        }

        /// <summary>
        ///     Applies clippo actions.
        /// </summary>
        /// <param name="actions">Actions to apply</param>
        /// <returns>OK result or coded result failure</returns>
        [HttpPut]
        public async Task<IActionResult> ApplyAsync([FromBody] IEnumerable<IClippoAction> actions)
        {
            if (actions == null)
                return BadRequest();

            var result = await _clippo.ApplyAsync(actions);
            if (!result.Succeed) return new CodedActionResult(result.Failure);

            return Ok();
        }

        private IAuthorizationService AuthorizationService =>
            HttpContext.RequestServices.GetRequiredService<IAuthorizationService>();

        private IAuthenticationService AuthenticationService =>
            HttpContext.RequestServices.GetRequiredService<IAuthenticationService>();

        private async Task<bool> AuthorizeAsync()
        {
            if (_settings.Policy == null)
                return true;

            var authenticationResult = await AuthenticationService.AuthenticateAsync(HttpContext, _settings.Schema);
            if (!authenticationResult.Succeeded)
                return false;

            var authorizationResult =
                await AuthorizationService.AuthorizeAsync(authenticationResult.Principal!, _settings.Policy);
            return authorizationResult.Succeeded;
        }
    }
}