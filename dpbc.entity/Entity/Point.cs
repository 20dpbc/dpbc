using Dapper.Contrib.Extensions;

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
        public User user { get; private set; }

        #pragma warning disable CS8618
        public Point(long id, long user_id, long message_id, DateTime started, DateTime? stoped) 
        {
            this.id = id;
            this.user_id = user_id;
            this.message_id = message_id;
            this.started = started;
            this.stoped = stoped;
        }
        #pragma warning restore CS8618

        public Point(User user)
        {
            this.user_id = user.id;
            this.user = user;
            this.started = DateTime.Now;
        }

        public void SetMessageId(ulong message_id)
        {
            this.message_id = (long)message_id;
        }

        public void Stoped()
        {
            this.stoped = this.IsValid() ? DateTime.Now : this.started;
        }

        public void SetUser(User user)
        {
            this.user = user;
        }

        public string GetOpenMessage()
        {
            return this.GetMessage();
        }    

        public string GetCloseMessage()
        {
            var closeMessage = this.IsValid() ? this.stoped?.ToString("HH:mm") : "Ponto inválido";     
            return this.GetMessage(closeMessage);
        }
        
        private string GetMessage(string? closeMessage = null) 
        {
            var message = string.Format("👮🏻‍♂️ QRA: {0}{1}📥 Entrada: {2}{3}📤 Saída: {4}{5}💳 ID: {6}",
                this.user?.mention,
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
            if (this.user == null)
                return string.Empty;

            return this.user.name.ToLower()
                .Replace(" i ", " | ")
                .Replace(" l ", " | ")
                .Split("| ")
                .LastOrDefault() ?? string.Empty;
        }

        public bool IsValid() 
        {
            if ((DateTime.Now - this.started).TotalHours >= 16) 
            {
                return false;
            }

            return true;
        }
    }
}
