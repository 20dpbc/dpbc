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
        public IUser? user { get; private set; }

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

        public void SetStoped()
        {
            this.stoped = DateTime.Now;
        }

        public void SetUser(IUser user)
        {
            this.user = user;
        }

        public string GetOpenMessage()
        {
            return this.GetMessage();
        }    

        public string GetCloseMessage()
        {
            var closeMessage = this.IsValid() ? this.stoped?.ToString("HH:mm") : "Ponto inválido, mais de 12 horas sem fechar";
     
            return this.GetMessage(closeMessage);
        }
        
        private string GetMessage(string? closeMessage = null) 
        {
            var message = string.Format("👮🏻‍♂️ QRA: {0}{1}📥 Entrada: {2}{3}📤 Saída: {4}{5}💳 ID: {6}",
                this.user?.Mention,
                Environment.NewLine,
                this.started.ToString("HH:mm"),
                Environment.NewLine,
                closeMessage,
                Environment.NewLine,
                GetIdFromUser());

            return message;
        }

        private string GetIdFromUser()
        {
            return ((SocketGuildUser)this.user).Nickname.ToLower()
                .Replace(" i ", " | ")
                .Replace(" l ", " | ")
                .Split("| ")
                .LastOrDefault() ?? string.Empty;
        }

        public bool IsValid() 
        {
            if ((DateTime.Now - this.started).TotalHours >= 12) 
            {
                return false;
            }

            return true;
        }
    }
}
