using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CURSE.Model
{
   public class Applicant : Human
    {
        public int XP { get; set; }
        public int Salary { get; set; }
        public List<Field_of_Activity> FieldofActivity { get; set; }
        public string Comment { get; set; }
        public bool IsFlexibleSchedule { get; set; } //график
       public bool FullEmployment {  get; set; }//занятость  

    }
}
