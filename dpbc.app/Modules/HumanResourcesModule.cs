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
            var point = await _pointService.GetByUserIdAsync(Context.User.Id);            

            if (point == null)
            {
                await ReplyAsync(String.Format("!start_point {0}", Context.User.Mention));
            }
            else
            {
                await this.StopPoint(point);
            }
        }

        [Command("start_point")]
        public async Task StartPoint(IUser? user = null)
        {
            if (Context.Message.Channel.Name != "🅿・ʙᴀᴛᴇʀ-ᴘᴏɴᴛᴏ" || !Context.User.IsBot)
            {
                return;
            }

            var message = "👮🏻‍♂️ QRA: " + user.Mention
                + Environment.NewLine
                + "📥 Entrada: " + DateTime.Now.ToString("HH:mm")
                + Environment.NewLine
                + "📤 Saída:"
                + Environment.NewLine
                + "💳 ID: " + ((Discord.WebSocket.SocketGuildUser)user).Nickname.Split("| ").LastOrDefault();

            await Context.Channel.ModifyMessageAsync(Context.Message.Id, m => m.Content = message);
            await _pointService.InsertAsync(user.Id, Context.Message.Id);
        }

        private async Task StopPoint(Point point) 
        {
            var message = await Context.Channel.GetMessageAsync((ulong)point.message_id);
            if (message == null)
            {
                return;
            }

            await Context.Channel.ModifyMessageAsync((ulong)point.message_id, m => m.Content = message.Content.Replace("Saída:", "Saída: " + DateTime.Now.ToString("HH:mm")));
            await _pointService.DeleteAsync(point);
        }
    }
}
