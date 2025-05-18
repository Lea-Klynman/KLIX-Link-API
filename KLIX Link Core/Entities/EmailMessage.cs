using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLIX_Link_Core.Entities
{
    public class EmailMessage
    {
      
            public int Id { get; set; }
            public string From { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }
        public bool IsRead { get; set; } 

        public DateTime Date { get; set; }
    }
}
