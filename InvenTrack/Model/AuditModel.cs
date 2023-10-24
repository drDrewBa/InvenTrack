using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvenTrack.Model
{
    internal class AuditModel
    {
        public DateTime auditDate { get; set; }
        public string message { get; set; }
    }
}
