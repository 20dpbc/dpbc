using Discord.Commands;
using dpbc.entity.Entity;
using dpbc.service.Service;

namespace dpbc.app.Modules
{
    public class HumanResourcesModule : ModuleBase<SocketCommandContext>
    {
        private readonly IPointService _pointService;
        private readonly IUserService _userService;

        public HumanResourcesModule(IPointService pointService, IUserService userService)
        {
            _pointService = pointService;
            _userService = userService;
        }

        [Command("minutes")]
        public async Task GetTopMinutes(string totalRaw)
        {
            if (Context.Message.Channel.Name != "🚨・ᴀᴠɪꜱᴏꜱ" || !int.TryParse(totalRaw, out int total))
                return;

            await Context.Channel.DeleteMessageAsync(Context.Message.Id);

            var pointView = await _pointService.GetTotalMinutes(7);
            var message = pointView.GetTopMinutesMessage(Context.User.Mention, total);

            if (message == null)
                return;

            await ReplyAsync(message);
        }

        [Command("inactives")]
        public async Task GetAllInactive(string daysRaw)
        {
            if (Context.Message.Channel.Name != "🚨・ᴀᴠɪꜱᴏꜱ")
                return;

            int.TryParse(daysRaw, out int days);
            days = days == 0 ? 7 : days;

            await Context.Channel.DeleteMessageAsync(Context.Message.Id);
            var pointView = await _pointService.GetTotalMinutes(days);
            var message = pointView.GetInactiveMessage(Context.User.Mention, days);

            if (message == null)
                return;

            await ReplyAsync(message);
        }

        [Command("p")]
        public async Task Point()
        {
            if (Context.Message.Channel.Name != "🅿・ʙᴀᴛᴇʀ-ᴘᴏɴᴛᴏ")
                return;

            await Context.Channel.DeleteMessageAsync(Context.Message.Id);
            var user = await _userService.GetByUUID(Context.User.Id.ToString());

            if (user == null)
            {
                await ReplyAsync(string.Format("Policial {0} não cadastrado", Context.User.Mention));
                return;
            }

            var point = await _pointService.GetByUserIdAsync(user.id);

            if (point == null)
                await this.OpenPoint(user); 
            else
            {
                point.SetUser(user);
                await this.ClosePoint(point);
            }
        }

        private async Task OpenPoint(User user)
        {
            Point point = new(user);
            var message = await ReplyAsync(point.GetOpenMessage());
            point.SetMessageId(message.Id);
            await _pointService.InsertAsync(point);
        }

        private async Task ClosePoint(Point point) 
        {
            point.Stoped();
            await _pointService.UpdateAsync(point);
            await Context.Channel.ModifyMessageAsync((ulong)point.message_id, m => m.Content = point.GetCloseMessage());

            if (!point.IsValid())
                await this.OpenPoint(point.user);
        }
    }
}
