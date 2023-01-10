using Discord;
using Discord.Commands;
using Newtonsoft.Json;

namespace dpbc.app.Modules
{
    public class SpawnerModule : ModuleBase<SocketCommandContext>
    {
        public enum Button { ponto }

        [Command("botao")]
        public async Task SpawnButton(string name, string type, string style, string url)
        {
            await Context.Channel.DeleteMessageAsync(Context.Message.Id);

            if (!Enum.TryParse(type, out Button button))
            {
                await ReplyAsync("tipo de botão não existe!");
                return;
            }

            if (!Enum.TryParse(style, out ButtonStyle buttonStyle))
            {
                await ReplyAsync("style de botão não existe!");
                return;
            }

            var builder = new ComponentBuilder()
                .WithButton(label: name, customId: button.ToString(), style: buttonStyle, url: url);

            await ReplyAsync(" ", components: builder.Build());
        }

        [Command("user")]
        public async Task User(IUser mention)
        {
            await Context.Channel.DeleteMessageAsync(Context.Message.Id);
            
            var user = mention;
        }
    }
}
