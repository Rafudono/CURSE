using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CURSE.Model
{
    public class SelectedVacancy
    {
        public Vacancy selectedVac { get; set; } = new();
        public SelectedVacancy() { }
        static SelectedVacancy instance;
        public static SelectedVacancy Instance
        {
            get
            {
                if (instance == null)
                    instance = new SelectedVacancy();
                return instance;
            }
        }
    }
}

