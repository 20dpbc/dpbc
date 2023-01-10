using Discord.Commands;
using Discord.WebSocket;
using dpbc.service.Service;

namespace dpbc.app.Modules
{
    public class UserModule : ModuleBase<SocketCommandContext>
    {
        private readonly IUserService _userService;

        public UserModule(IUserService userService)
        {
            _userService = userService;
        }

        [Command("setuser")]
        public async Task Create(SocketGuildUser socketUser, string photo)
        {
            await Context.Channel.DeleteMessageAsync(Context.Message.Id);
            var user = await _userService.GetByUUID(socketUser.Id.ToString());

            if (user != null)
            {
                user.Update(socketUser.Nickname, photo);
                await _userService.UpdateAsync(user);
                await ReplyAsync(string.Format("{0} atualizado", user.mention));
                return;
            }

            user = new(socketUser.Nickname, socketUser.Id, socketUser.Mention, photo);
            await _userService.InsertAsync(user);
            await ReplyAsync(string.Format("{0} cadastrado", user.mention));
        }

        [Command("deleteuser")]
        public async Task Delete(SocketGuildUser socketUser)
        {
            await Context.Channel.DeleteMessageAsync(Context.Message.Id);
            var user = await _userService.GetByUUID(socketUser.Id.ToString());

            if (user == null)
            {
                await ReplyAsync(string.Format("Policial {0} não cadastrado", socketUser.Mention));
                return;
            }

            await _userService.DeleteAsync(user);            
        }

        [Command("badge")]
        public async Task GetBadge(SocketGuildUser socketUser)
        {
            await Context.Channel.DeleteMessageAsync(Context.Message.Id);
            var user = await _userService.GetByUUID(socketUser.Id.ToString());

            if (user == null)
            {
                await ReplyAsync(string.Format("Policial {0} não cadastrado", socketUser.Mention));
                return;
            }

            await ReplyAsync(embed: user.GetBadge().Build());
        }
    }
}
