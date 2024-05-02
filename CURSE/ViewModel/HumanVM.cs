using CURSE.Model;
using CURSE.View;
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
        public VmCommand Save { get; set; }
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
            string sql = " ";
            People = new ObservableCollection<Human>(HumanRepository.Instance.GetPeople(sql));
            Save = new VmCommand(() =>
            {
                new Registration().Show();
            });
        }
        internal void SetMainVM(MainVM mainVM)
        {
            this.mainVM = mainVM;
        }
        //private void SearchVacancy()
        //{
        //    People = new ObservableCollection<Human>(
        //           HumanRepository.Instance.Search(SearchText));
        //}
    }
}
// где-то надо создать human заполнить его свойства при регистрации, внести в таблицу 