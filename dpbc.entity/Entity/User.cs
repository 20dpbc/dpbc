using Dapper.Contrib.Extensions;
using Discord;

namespace dpbc.entity.Entity
{
    [Table("[User]")]
    public class User
    {
        [Key]
        public long id { get; private set; }
        public string name { get; private set; }
        public string uuid { get; private set; }
        public string mention { get; private set; }
        public string ra { get; private set; }
        public string url { get; private set; }
        public DateTime created { get; private set; }

        public User(long id, string name, string uuid, string mention, string ra, string url, DateTime created)
        {
            this.id = id;
            this.name = name;
            this.uuid = uuid;
            this.mention = mention; 
            this.ra = ra;
            this.url = url;
            this.created = created;
        }

        public User(string name, ulong uuid, string mention, string url)
        {
            this.name = name;
            this.uuid = uuid.ToString();
            this.mention = mention;
            this.url = url;
            ra = this.GetRaFromName(name);
            created = DateTime.Now;
        }

        public void Update(string name, string url)
        {
            this.name = name;
            this.url = !string.IsNullOrEmpty(url) ? url : this.url;
        }

        public EmbedBuilder GetBadge()
        {
            var embed = new EmbedBuilder
            {
                Title = "CARTEIRA DE IDENTIDADE MILITAR",
                Description = "20º Batalhão"
            };

            embed.AddField("Nome", this.name, true)
                .AddField("Cargo", "POLICIAL", true)
                .AddField("\u200b", "\u200b")
                .AddField("R.A", this.ra, true)
                .AddField("Matrícula", this.uuid, true)                
                .WithTitle("CARTEIRA DE IDENTIDADE MILITAR")
                .WithDescription("POLÍCIA MLITAR DO ESTADO DE BRAZUCA CITY\nSUPERINTENDÊNCIA DO VIGÉSIMO DPBC")
                .WithFooter(footer => footer.Text = string.Format("Desde: {0}", created.ToString("dd/MM/yyyy")))
                .WithThumbnailUrl(this.url)
                .WithCurrentTimestamp();

            return embed;
        }

        private string GetRaFromName(string name)
        {
            if (name == null)
                return string.Empty;

            return name.ToLower()
                .Replace(" i ", " | ")
                .Replace(" l ", " | ")
                .Split("| ")
                .LastOrDefault() ?? string.Empty;
        }
    }
}
