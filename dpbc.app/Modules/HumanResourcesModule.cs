using Discord;
using Discord.Commands;
using dpbc.entity.Entity;
using dpbc.service.Service;

namespace dpbc.app.Modules
{
    public class HumanResourcesModule : ModuleBase<SocketCommandContext>
    {
        private readonly IPointService _pointService;

        public HumanResourcesModule(IPointService pointService)
        {
            _pointService = pointService;
        }

        [Command("p")]
        public async Task Point()
        {
            if (Context.Message.Channel.Name != "🅿・ʙᴀᴛᴇʀ-ᴘᴏɴᴛᴏ")
            {
                return;
            }

            await Context.Channel.DeleteMessageAsync(Context.Message.Id);
            var point = await _pointService.GetByUserIdAsync((long)Context.User.Id);

            if (point == null)
            {
                await this.OpenPoint(Context.User);               
            }
            else
            {
                point.SetUser(Context.User);
                await this.ClosePoint(point);
            }
        }

        private async Task OpenPoint(IUser user)
        {
            Point point = new(user);
            var message = await ReplyAsync(point.GetOpenMessage());
            point.SetMessageId(message.Id);
            await _pointService.InsertAsync(point);
        }

        private async Task ClosePoint(Point point) 
        {
            if (point.IsValid())
            {
                await _pointService.UpdateAsync(point);
            }
            else
            {
                await _pointService.DeleteAsync(point);
            }

            await Context.Channel.ModifyMessageAsync((ulong)point.message_id, m => m.Content = point.GetCloseMessage());        
        }
    }
}
