using Discord.WebSocket;

namespace dpbc.entity.Entity
{
    public class PointView
    {
        public List<Tuple<long, int>> registers { get; set; }

        public PointView(List<Tuple<long, int>> registers)
        {
            this.registers = registers;
        }

        public string? GetTopMinutesMessage(string responsibleUser, List<SocketGuildUser> users, int total)
        {
            var query = (
                from us in users
                from re in registers
                .Where(register => (long)us.Id == register.Item1).DefaultIfEmpty()
                select new
                {
                    mention = us.Mention,
                    total_minutes = re?.Item2 ?? 0,
                }
            )
            .Where(x => x.total_minutes > 0)
            .OrderByDescending(c => c.total_minutes)
            .ToArray();

            if (query.Count() == 0)
                return null;

            string message = string.Format("Secretaria de Segurança Pública\r\nPolicia Militar de Brazuca City\r\nCorregedoria Do 20º DPBC\r\n\r\nEu {0}, no uso de minhas atribuições, venho por meio desta informar os soldados mais dedicados da corporação nos últimos 6 dias.\r\n", responsibleUser);

            total = query.Count() < total || total == 0 ? query.Count() : total;
            for (var i = 1; i <= total; i++)
            {
                message += string.Format("{0} - {1} Minutos\r\n", query[i - 1].mention, query[i - 1].total_minutes);
            }

            return message;
        }

        public string? GetInactiveMessage(string responsibleUser, List<SocketGuildUser> users, int days) 
        {
            var query = (
                from us in users
                from re in registers
                .Where(register => (long)us.Id == register.Item1).DefaultIfEmpty()
                select new
                {
                    mention = us.Mention,
                    total_minutes = re?.Item2 ?? 0,
                }
            ).Where(x => x.total_minutes == 0);

            if (query.Count() == 0)
                return null;

            string message = string.Format("Secretaria de Segurança Pública\r\nPolicia Militar de Brazuca City\r\nCorregedoria Do 20º DPBC\r\n\r\nEu {0}, no uso de minhas atribuições, venho por meio desta informar o desligamento dos seguintes policiais abaixo por inatividade de mais de {1} dias.\r\n", responsibleUser, days);

            foreach(var user in query)
            {
                message += string.Format("{0}\r\n", user.mention);                                
            }

            message += "\r\nAtenciosamente,\r\nTribunal de Justiça Militar do Estado de Brazuca.";

            return message;
        }
    }
}
