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
        public string Employer { get; set; }
        public int XP { get; set; }
        public string FieldofActivity { get; set; }
        public int RequiredEducation {get; set; }
        public string City { get; set; }
    }
}
