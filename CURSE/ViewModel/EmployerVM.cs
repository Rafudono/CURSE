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
        {//переделать
            string sql = "SELECT v.ID, v.employer_id, v.`Required-XP`, v.`id_Required-Education`, v.id_city, v.MINSalary, v.MAXSalary, v.Responsibilities, v.Requirements, v.Conditions, v.Description, v.IsFlexibleSchedule, v.Title, foa.ID AS foaId, foa.Field_name AS foaTitle FROM `Cross_Vacancy_Field-of-activity` cvfoa, Vacancy v, `Fields-of-activity` foa WHERE cvfoa.id_vac = v.ID AND cvfoa.id_field = foa.ID ORDER by v.ID;";
            Employers = new ObservableCollection<Employer>(EmployerRepository.Instance.GetAllEmployers(sql));
        }
        internal void SetMainVM(MainVM mainVM)
        {
            this.mainVM = mainVM;
        }
    }
}
