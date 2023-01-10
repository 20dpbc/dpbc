namespace dpbc.entity.Entity
{
    public class PointView
    {
        public List<Tuple<string, int>> registers { get; set; }

        public PointView(List<Tuple<string, int>> registers)
        {
            this.registers = registers;
        }

        public string? GetTopMinutesMessage(string responsibleUser, int total)
        {
            var regs = registers.Where(x => x.Item2 > 0).ToList();

            if (regs.Count() == 0)
                return null;

            string message = string.Format("Secretaria de Segurança Pública\r\nPolicia Militar de Brazuca City\r\nCorregedoria Do 20º DPBC\r\n\r\nEu {0}, no uso de minhas atribuições, venho por meio desta informar os soldados mais dedicados da corporação nos últimos 7 dias.\r\n\r\n", responsibleUser);

            total = regs.Count() < total || total == 0 ? regs.Count() : total;
            for (var i = 1; i <= total; i++)
            {
                message += string.Format("{0} - {1} Minutos\r\n", regs[i - 1].Item1, regs[i - 1].Item2);
            }

            return message;
        }

        public string? GetInactiveMessage(string byUser, int days) 
        {
            var regs = registers.Where(x => x.Item2 == 0).ToList();
            if (regs.Count == 0)
                return null;

            string message = string.Format("Secretaria de Segurança Pública\r\nPolicia Militar de Brazuca City\r\nCorregedoria Do 20º DPBC\r\n\r\nEu {0}, no uso de minhas atribuições, venho por meio desta informar o desligamento dos seguintes policiais abaixo por inatividade de mais de {1} dias.\r\n\r\n", byUser, days);                        
            
            foreach (var reg in regs)
            {
                message += string.Format("{0}\r\n", reg.Item1);                                
            }

            message += "\r\nAtenciosamente,\r\nTribunal de Justiça Militar do Estado de Brazuca.";
            return message;
        }
    }
}
