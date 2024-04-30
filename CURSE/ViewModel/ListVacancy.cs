using CURSE.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CURSE.ViewModel
{
    public class ListVacancy : BaseVM
    {
        private MainVM mainVM;
        private string searchText = "";
        private ObservableCollection<Vacancy> all_vac;
        private Field_of_Activity selectedField;
        private Vacancy selectedVac;
        //public VmCommand Create { get; set; }
        //public VmCommand Edit { get; set; }
        //public VmCommand Delete { get; set; }

        public Field_of_Activity SelectedField
        {
            get => selectedField;
            set
            {
                selectedField = value;
                Signal();
                //  Search();
            }
        }

        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                //   Search();
            }
        }

        public ObservableCollection<Field_of_Activity> Fields { get; set; }
        public Vacancy SelectedVac
        {
            get => selectedVac;
            set
            {
                selectedVac = value;
                Signal();
                //   Search();
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

        public ListVacancy()
        {

            //Fields = new ObservableCollection<Field_of_Activity>(FoaRepository.Instance.GetFields());
            //Fields.Insert(0, new Field_of_Activity { Id = 0, Title = "Все виды!!!!!" });     //!!!!
            //SelectedField = Fields[0];
            string sql = "SELECT v.ID, v.employer_id, v.`Required-XP`, v.`id_Required-Education`, v.id_city, v.MINSalary, v.MAXSalary, v.Responsibilities, v.Requirements, v.Conditions, v.Description, v.IsFlexibleSchedule, v.Title, foa.ID AS foaId, foa.Field_name AS foaTitle FROM `Cross_Vacancy_Field-of-activity` cvfoa, Vacancy v, `Fields-of-activity` foa WHERE cvfoa.id_vac = v.ID AND cvfoa.id_field = foa.ID ORDER by v.ID;";

            All_vac = new ObservableCollection<Vacancy>(VacancyRepository.Instance.GetAllVacancy(sql));

            //Create = new VmCommand(() =>
            //{
            //    mainVM.CurrentPage = new EditorDrink(mainVM);
            //});

            //Edit = new VmCommand(() => {
            //    if (SelectedDrink == null)
            //        return;
            //    mainVM.CurrentPage = new EditorDrink(mainVM, SelectedDrink);
            //});

            //Delete = new VmCommand(() => {
            //    if (SelectedDrink == null)
            //        return;

            //    if (MessageBox.Show("Удаление напитка", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            //    {
            //        DrinkRepository.Instance.Remove(SelectedDrink);
            //        Drinks.Remove(SelectedDrink);
            //    }
            //});
        }
        internal void SetMainVM(MainVM mainVM)
        {
            this.mainVM = mainVM;
        }

        private void SearchVacancy()
        {
            All_vac = new ObservableCollection<Vacancy>(
                    VacancyRepository.Instance.Search(SearchText));//, SelectedField));

        }
    }

 }
    
