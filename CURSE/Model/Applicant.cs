using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CURSE.Model
{
   public class Applicant : Human
    {
        public int Id { get; set; }
        public int HumanId {  get; set; }
        public int Id_City { get; set; }
        public int XP { get; set; }
        public double Salary { get; set; }
        public List<Field_of_Activity> FieldofActivity { get; set; } = new();
        public int IdEducation { get; set; }
        public bool IsFlexibleSchedule { get; set; } //график
       public bool IsFullEmployment {  get; set; }//занятость  

    }
}
