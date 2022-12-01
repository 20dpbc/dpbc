using Dapper.Contrib.Extensions;
using System.Globalization;

namespace dpbc.entity.Entity
{
    [Table("Point")]
    public class Point
    {
        [Key]
        public long id { get; private set; }
        public long user_id { get; private set; }
        public long message_id { get; private set; }
        public DateTime registerAt { get; private set; }

        public Point(long id, long user_id, long message_id, string registerAt) 
        {
            this.id = id;
            this.user_id = user_id;
            this.message_id = message_id;
        }

        public Point(ulong user_id, ulong message_id) 
        {
            this.user_id = (long)user_id;
            this.message_id = (long)message_id;
            this.registerAt = DateTime.Now;
        }
    }
}
