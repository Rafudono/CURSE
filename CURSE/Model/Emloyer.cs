using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CURSE.Model
{
    public class Emloyer : Human
    {
        public int ID { get; set; }
        public int id_human { get; set; }
        public int id_organization { get; set; } //я хотел string
    } 
}
