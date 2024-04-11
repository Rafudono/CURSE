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
        public string Title { get; set; }
        public List<Field_of_Activity> FieldofActivity { get; set; }
        public string Employer { get; set; } //нужно из id работодателя (которая в таблице) получить органиацию
        public int XP { get; set; }
        public string City { get; set; } //получить из id
        public int  MINSalary { get; set; }
        public int MAXSalary { get; set; }
        public string Country { get; set; }
        public string DescriptionResponsibilities { get; set; }
        public string DescriptionRequirements { get; set; }
        public string DescriptionConditions { get; set; }
        public string Schedule { get; set; }
        public int RequiredEducation { get; set; }
    }
}
