


using System;
using System.IO;
using System.Threading.Tasks;
using Itminus.Areas.DirectLine.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Itminus.InDirectLine.Controllers
{

    [ApiController]
    public class ConversationsController : Controller
    {
        private readonly ILogger<ConversationsController> _logger;
        private readonly IOptions<InDirectLineOptions> _opt;
        private readonly DirectLineHelper _helper;

        public ConversationsController(ILogger<ConversationsController> logger, IOptions<InDirectLineOptions> opt, DirectLineHelper helper)
        {
            this._logger = logger;
            this._opt = opt;
            this._helper = helper;
        }

        [HttpGet("v3/[controller]")]
        public async Task<IActionResult> Index()
        {
            // todo
            return Ok();
        }

        [HttpPost("v3/[controller]/{conversationId}/activities")]
        public async Task<IActionResult> ReceiveActivityFromBotAsync([FromRoute] string conversationId,[FromBody]Activity activity)
        {
            activity.Id = Guid.NewGuid().ToString();
            activity.From = new ChannelAccount{
                Id = "id",
                Name = "Bot",
            };
            var conversationExists = await this._helper.ConversationHistoryExistsAsync(conversationId);
            if(!conversationExists){
                return BadRequest(new{
                    Message = $"Conversation with id={conversationId} doesn't exist!"
                });
            }
            await this._helper.AddActivityToConversationHistoryAsync(conversationId,activity);
            this._logger.LogInformation("message from bot received: \r\nConversationId={0}\r\nActivity.Id={1}\tActivityType={2}\tMessageText={3}",conversationId,activity.Id,activity.Type,activity.Text);
            return new OkResult();
        }

        [HttpPost("v3/[controller]/{conversationId}/activities/{replyTo}")]
        public async Task<IActionResult> ReceiveActiviyFromBotAsync([FromRoute] string conversationId,[FromRoute] string replyTo, Activity activity)
        {
            activity.Id = Guid.NewGuid().ToString();
            activity.From = new ChannelAccount{
                Id = "id",
                Name = "Bot",
            };
            var conversationExists = await this._helper.ConversationHistoryExistsAsync(conversationId);
            if(!conversationExists){
                return BadRequest(new{
                    Message = $"Conversation with id={conversationId} doesn't exist!"
                });
            }
            await this._helper.AddActivityToConversationHistoryAsync(conversationId,activity);
            this._logger.LogInformation("message from bot received: \r\nConversationId={0}\r\n ActivityId={1}\r\nActivity.Id={2}\tActivityType={3}\tMessageText={4}",conversationId,replyTo,activity.Id,activity.Type,activity.Text);
            return new OkResult();
        }

        [HttpPost("v3/[controller]/{conversationId}/members")]
        public async Task<IActionResult> ConversationMembers([FromRoute] string conversationId)
        {
            // not implemented
            return new OkResult();
        }

        [HttpPost("v3/[controller]/{conversationId}/activities/{activityId}/members")]
        public async Task<IActionResult> ActivityMembers([FromRoute] string conversationId)
        {
            // not implemented
            return new OkResult();
        }


    }

}