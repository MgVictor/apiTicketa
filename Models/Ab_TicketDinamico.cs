using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiTicket.Models
{
    public class Ab_TicketDinamico
    {
        public string system { get; set; }
        public IList<Field> fields { get; set; }
        public IList<Ab_Attachment> attachments { get; set; }
    }
}
