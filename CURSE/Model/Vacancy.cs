using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CURSE.Model
{
    public class Vacancy
    {
        public int Id {  get; set; }
        public string Title { get; set; } = " ";
        public int EmployerId { get; set; }
        public List<Field_of_Activity> FieldofActivity { get; set; } = new();
        //public string Organization { get; set; } 
        //нужно из id работодателя (которая в таблице) получить органиацию
        public int XP { get; set; }
        public int Id_City { get; set; } //получить из id
        public int  MINSalary { get; set; }
        public int MAXSalary { get; set; }
        public string Responsibilities { get; set; }= " ";
        public string Requirements { get; set; } = " ";
        public string Conditions { get; set; } = " ";
        public string Description { get; set; } = " ";  
        public bool IsFlexibleSchedule { get; set; }
        public bool IsFullEmployment { get; set; }
        public int IdRequiredEducation { get; set; }
    }
}
