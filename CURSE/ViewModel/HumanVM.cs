using CURSE.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CURSE.ViewModel
{
    public class HumanVM:BaseVM
    {
        private MainVM mainVM;
        private string searchText = "";
        private ObservableCollection<Human> people;
        private Human selectedHuman;
        //public VmCommand Create { get; set; }
        //public VmCommand Edit { get; set; }
        //public VmCommand Delete { get; set; }

        public Human SelectedHuamn
        {
            get => selectedHuman;
            set
            {
                selectedHuman = value;
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
        public Human SelectedHuman
        {
            get => selectedHuman;
            set
            {
                selectedHuman = value;
                Signal();
                //   Search();
            }
        }
        public ObservableCollection<Human> People
        {
            get => people;
            set
            {
                people = value;
                Signal();
            }
        }

        public HumanVM()
        {
            //Fields = new ObservableCollection<Field_of_Activity>(FoaRepository.Instance.GetFields());
            //Fields.Insert(0, new Field_of_Activity { Id = 0, Title = "Все виды!!!!!" });     //!!!!
            //SelectedField = Fields[0];
            string sql = "SELECT a.ID, a.id_human, a.XP, a.id_Education, a.id_city, a.Salary , a.IsFullEmployment , a.IsFlexibleSchedule , foa.ID AS foaId, foa.Field_name AS foaTitle FROM CURSE.`Cross_Applicant_Field-of-activity` cafoa, CURSE.Applicant a, CURSE.`Fields-of-activity` foa WHERE cafoa.id_applicant = a.ID AND cafoa.id_field = foa.ID;";
            People = new ObservableCollection<Human>(HumanRepository.Instance.GetPeople(sql));
        }
        internal void SetMainVM(MainVM mainVM)
        {
            this.mainVM = mainVM;
        }
        private void SearchVacancy()
        {
            People = new ObservableCollection<Human>(
                   HumanRepository.Instance.Search(SearchText));
        }
    }
}
