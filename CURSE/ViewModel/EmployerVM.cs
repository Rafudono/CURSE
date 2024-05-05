using CURSE.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CURSE.ViewModel
{
    public class EmployerVM:BaseVM
    {
        private MainVM mainVM;
        private ObservableCollection<Employer> employers;

        public ObservableCollection<Employer> Employers
        {
            get => employers;
            set
            {
                employers = value;
                Signal();
            }
        }

        public EmployerVM()
        {
            string sql = "SELECT a.ID, a.id_human, a.XP, a.id_Education, a.id_city, a.Salary , a.IsFullEmployment , a.IsFlexibleSchedule , foa.ID AS foaId, foa.Field_name AS foaTitle FROM CURSE.`Cross_Applicant_Field-of-activity` cafoa, CURSE.Applicant a, CURSE.`Fields-of-activity` foa WHERE cafoa.id_applicant = a.ID AND cafoa.id_field = foa.ID;";
            Employers = new ObservableCollection<Employer>(EmployerRepository.Instance.GetAllEmployers(sql));
        }
        internal void SetMainVM(MainVM mainVM)
        {
            this.mainVM = mainVM;
        }
    }
}
