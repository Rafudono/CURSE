using CURSE.Model;
using CURSE.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CURSE.ViewModel
{
    public class MyVacVM:BaseVM
    {
        private MainVM mainVM;
        private ObservableCollection<Vacancy> all_vac;
        private Vacancy selectedVac;

        public VmCommand NewVac { get; }
        public VmCommand EditVac { get; }
        public VmCommand DeleteVac { get; }
        public ObservableCollection<Field_of_Activity> Fields { get; set; }
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
        public ObservableCollection<Vacancy> MyVacancy
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
            if (EmpAuthorized.Instance.loginEmp.id_human != 0)
            {
                try
                {
                    string sql = "SELECT v.ID, v.employer_id, v.`Required-XP`, v.`id_Required-Education`, v.id_city, v.MINSalary, v.MAXSalary, v.Responsibilities, v.Requirements, v.Conditions, v.Description, v.IsFlexibleSchedule, v.Title, foa.ID AS foaId, foa.Field_name AS foaTitle FROM `Cross_Vacancy_Field-of-activity` cvfoa, Vacancy v, `Fields-of-activity` foa WHERE cvfoa.id_vac = v.ID AND cvfoa.id_field = foa.ID and employer_id = " + EmpAuthorized.Instance.loginEmp.ID + " ORDER by v.ID;";
                    MyVacancy = new ObservableCollection<Vacancy>(VacancyRepository.Instance.GetAllVacancy(sql));
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
            NewVac = new VmCommand(() =>
            {
                    NEWVacancy taskwindow = new NEWVacancy();
                    taskwindow.ShowDialog();
                var myvac = this;
                myvac = new MyVacVM();
            });
            EditVac = new VmCommand(() =>
            {
                if (SelectedVac != null && SelectedVac.Id != 0)
                {
                    SelectedVacancy.Instance.selectedVac=SelectedVac;
                    NewOrEditVacancy taskwindow = new NewOrEditVacancy(SelectedVac);
                    taskwindow.ShowDialog();

                    var myvac = this;
                    myvac = new MyVacVM();

                   MainVM mainVM = new MainVM();
                    ListVacancy listVacancy = new ListVacancy();
                    if(mainVM!=null)
                   mainVM.VacancyPage.DataContext = listVacancy;
                }
            });
            DeleteVac = new VmCommand(() =>
            {
                if (SelectedVac != null && SelectedVac.Id != 0)
                {
                    SelectedVacancy.Instance.selectedVac = SelectedVac;
                    var result = MessageBox.Show("Вы правда хотите удалить выбранную вакансию?", "Удаление", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        DeleteVac taskwindow = new DeleteVac();
                        taskwindow.ShowDialog();

                        var myvac = this;
                        myvac = new MyVacVM();
                    }
                }
            });
        }
        internal void SetMainVM(MainVM mainVM)
        {
            this.mainVM = mainVM;
        }
    }
}
