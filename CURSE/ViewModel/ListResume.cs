using CURSE.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CURSE.ViewModel
{
    public class ListResume : BaseVM
    {
        private MainVM mainVM;
        private string searchText = "";
        private ObservableCollection<Applicant> all_resume;
        private Field_of_Activity selectedField;
        private Applicant selectedResume;
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
        public Applicant SelectedResume
        {
            get => selectedResume;
            set
            {
                selectedResume = value;
                Signal();
                //   Search();
            }
        }
        public ObservableCollection<Applicant> All_resume
        {
            get => all_resume;
            set
            {
                all_resume = value;
                Signal();
            }
        }

        public ListResume()
        {
            //Fields = new ObservableCollection<Field_of_Activity>(FoaRepository.Instance.GetFields());
            //Fields.Insert(0, new Field_of_Activity { Id = 0, Title = "Все виды!!!!!" });     //!!!!
            //SelectedField = Fields[0];
            //string sql = "SELECT a.ID, a.id_human, a.XP, a.id_Education, a.id_city, a.Salary , a.IsFullEmployment , a.IsFlexibleSchedule , foa.ID AS foaId, foa.Field_name AS foaTitle FROM CURSE.`Cross_Applicant_Field-of-activity` cafoa, CURSE.Applicant a, CURSE.`Fields-of-activity` foa WHERE cafoa.id_applicant = a.ID AND cafoa.id_field = foa.ID;";
           
            string sql = "SELECT a.ID, a.id_human, a.XP, a.id_Education, a.id_city, a.Salary , a.IsFullEmployment , a.IsFlexibleSchedule , foa.ID AS foaId, foa.Field_name AS foaTitle, h.Name, h.Middle_name , h.Surname, h.EMAIL, h.PHONE_NUMBER, h.Birthday, e.Title \r\nFROM CURSE.`Cross_Applicant_Field-of-activity` cafoa, CURSE.Applicant a, Curse.Human h, CURSE.`Fields-of-activity` foa, CURSE.Education e \r\nWHERE cafoa.id_applicant = a.ID AND cafoa.id_field = foa.ID and a.id_human = h.ID and a.id_Education =e.ID;";
            All_resume = new ObservableCollection<Applicant>(ApplicantRepository.Instance.GetAllResume(sql));
        }
        internal void SetMainVM(MainVM mainVM)
        {
            this.mainVM = mainVM;
        }
        private void SearchResume()
        {
            All_resume = new ObservableCollection<Applicant>(
                   ApplicantRepository.Instance.Search(SearchText));
        }
    }

}
