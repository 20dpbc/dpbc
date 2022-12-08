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

        [Command("topminutos")]
        public async Task GetTopMinutes(string totalRaw)
        {
            if (Context.Message.Channel.Name != "🚨・ᴀᴠɪꜱᴏꜱ" || !int.TryParse(totalRaw, out int total))
            {
                return;
            }

            await Context.Channel.DeleteMessageAsync(Context.Message.Id);

            var users = Context.Guild.Users.Where(x => !x.IsBot).ToList();
            var pointView = await _pointService.GetTotalMinutes();
            var message = pointView.GetTopMinutesMessage(Context.User.Mention, users, total);

            if (message == null)
                return;

            await ReplyAsync(message);
        }

        [Command("inativos")]
        public async Task GetAllInactive()
        {
            if (Context.Message.Channel.Name != "🚨・ᴀᴠɪꜱᴏꜱ")
            {
                return;
            }

            await Context.Channel.DeleteMessageAsync(Context.Message.Id);

            var users = Context.Guild.Users.Where(x => !x.IsBot).ToList();
            var pointView = await _pointService.GetTotalMinutes();
            var message = pointView.GetInactiveMessage(Context.User.Mention, users);

            if (message == null)
                return;

            await ReplyAsync(message);
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
            point.SetStoped();

            if (point.IsValid())
            {
                await _pointService.UpdateAsync(point);
            }
            else
            {
                await _pointService.DeleteAsync(point);
                await this.OpenPoint(point.user);
            }

            await Context.Channel.ModifyMessageAsync((ulong)point.message_id, m => m.Content = point.GetCloseMessage());
        }
    }
}
