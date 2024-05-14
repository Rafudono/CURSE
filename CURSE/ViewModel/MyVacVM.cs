using CURSE.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CURSE.ViewModel
{
    public class MyVacVM:BaseVM
    {
        MainVM mainVM;
        private ObservableCollection<Vacancy> all_vac;
        private Vacancy selectedVac;
        Action close;
        public VmCommand NewVac { get; }
        public VmCommand EditVac { get; }
        public VmCommand DeleteVac { get; }
        public Vacancy SelectedVac
        {
            get => selectedVac;
            set
            {
                selectedVac = value;
                Signal();
            }
        }
        public ObservableCollection<Vacancy> All_vac
        {
            get => all_vac;
            set
            {
                all_vac = value;
                Signal();
            }
        }

        public MyVacVM()
        {
            string sql = "SELECT v.ID, v.employer_id, v.`Required-XP`, v.`id_Required-Education`, v.id_city, v.MINSalary, v.MAXSalary, v.Responsibilities, v.Requirements, v.Conditions, v.Description, v.IsFlexibleSchedule, v.Title, foa.ID AS foaId, foa.Field_name AS foaTitle FROM `Cross_Vacancy_Field-of-activity` cvfoa, Vacancy v, `Fields-of-activity` foa WHERE cvfoa.id_vac = v.ID AND cvfoa.id_field = foa.ID and employer_id = "+EmpAuthorized.Instance.loginEmp.Id+" ORDER by v.ID;";
            All_vac = new ObservableCollection<Vacancy>(VacancyRepository.Instance.GetAllVacancy(sql));
        }
        internal void SetMainVM(MainVM mainVM)
        {
            this.mainVM = mainVM;
        }
    }
}
