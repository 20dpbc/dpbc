using Dapper.Contrib.Extensions;
using Discord;
using Discord.WebSocket;

namespace dpbc.entity.Entity
{
    [Table("Point")]
    public class Point
    {
        [Key]
        public long id { get; private set; }
        public long user_id { get; private set; }
        public long message_id { get; private set; }
        public DateTime started { get; private set; }
        public DateTime? stoped { get; private set; }

        [Write(false)]
        public IUser user { get; private set; }

        public Point(long id, long user_id, long message_id, DateTime started, DateTime? stoped) 
        {
            this.id = id;
            this.user_id = user_id;
            this.message_id = message_id;
            this.started = started;
            this.stoped = stoped;
        }

        public Point(IUser user) 
        {
            this.user_id = (long)user.Id;
            this.user = user;
            this.started = DateTime.Now;
        }

        public void SetMessageId(ulong message_id)
        {
            this.message_id = (long)message_id;
        }

        public void SetUser(IUser user)
        {
            this.user = user;
        }

        public string GetOpenMessage()
        {
            string id = ((SocketGuildUser)this.user).Nickname.ToLower()
                .Replace(" i ", " | ")
                .Replace(" l ", " | ")
                .Split("| ")
                .LastOrDefault() ?? string.Empty;

            var message = string.Format("👮🏻‍♂️ QRA: {0}{1}📥 Entrada: {2}{3}📤 Saída:{4}💳 ID: {5}",
                this.user.Mention,
                Environment.NewLine,
                this.started.ToString("HH:mm"),
                Environment.NewLine,
                Environment.NewLine,
                id);

            return message;
        }

        public string GetCloseMessage()
        {
            this.stoped = DateTime.Now;
            var msgOut = (DateTime.Now - this.started).TotalHours >= 24 ? "Ponto inválido, mais de 24 horas sem fechar" : this.stoped?.ToString("HH:mm");
            string id = ((SocketGuildUser)this.user).Nickname.ToLower()
                .Replace(" i ", " | ")
                .Replace(" l ", " | ")
                .Split("| ")
                .LastOrDefault() ?? string.Empty;

            var message = string.Format("👮🏻‍♂️ QRA: {0}{1}📥 Entrada: {2}{3}📤 Saída: {4}{5}💳 ID: {6}",
                this.user.Mention,
                Environment.NewLine,
                this.started.ToString("HH:mm"),
                Environment.NewLine,
                msgOut,
                Environment.NewLine,
                id);

            return message;
        }
    }
}
