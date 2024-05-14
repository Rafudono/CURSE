using CURSE.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;

namespace CURSE.ViewModel
{
    public class ListResume : BaseVM
    {
        private MainVM mainVM;
        private string searchText = "";
        private ObservableCollection<Applicant> all_resume;
        private City selectedCity;
        private Education selectedEducation;
        private Field_of_Activity selectedField;
        private Applicant selectedResume;
        private List<Field_of_Activity> Listfoa=new();

        private int minSalary = 1000000;
        private int xp = 100;
        private bool isFullEmp;
        private bool isFlexShedule=true;
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
                SearchResume();
            }
        }
        public Education SelectedEducation
        {
            get => selectedEducation;
            set
            {
                selectedEducation = value;
                Signal();
                SearchResume();
            }
        }
        public City SelectedCity
        {
            get => selectedCity;
            set
            {
                selectedCity = value;
                Signal();
                SearchResume();
            }
        }

        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                SearchResume();
            }
        }
        public int MinSalary
        {
            get => minSalary;
            set
            {
                minSalary = value;
                SearchResume();
            }
        }
        public int XP
        {
            get => xp;
            set
            {
                xp = value;
                SearchResume();
            }
        }
        public bool IsFullEmp
        {
            get => isFullEmp;
            set
            {
                isFullEmp = value;
                SearchResume();
            }
        }
        public bool IsFlexShedule
        {
            get => isFlexShedule;
            set
            {
                isFlexShedule = value;
                SearchResume();
            }
        }

        public ObservableCollection<Field_of_Activity> Fields { get; set; }
        public ObservableCollection<Education> Educations { get; set; } = new ObservableCollection<Education>();
        public ObservableCollection<City> Citys { get; set; } = new ObservableCollection<City>();
        public Applicant SelectedResume
        {
            get => selectedResume;
            set
            {
                selectedResume = value;
                Signal();
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
            string sql = "SELECT a.ID, a.id_human, a.XP, a.id_Education, a.id_city, a.Salary , a.IsFullEmployment , a.IsFlexibleSchedule,  a.Description, a.ResumeTitle, foa.ID AS foaId, foa.Field_name AS foaTitle, h.PASSWORD, h.Name, h.Middle_name, h.Surname, h.EMAIL, h.PHONE_NUMBER, h.Birthday, h.Photo, e.Title FROM CURSE.`Cross_Applicant_Field-of-activity` cafoa, CURSE.Applicant a, Curse.Human h, CURSE.`Fields-of-activity` foa, CURSE.Education e WHERE cafoa.id_applicant = a.ID AND cafoa.id_field = foa.ID and a.id_human = h.ID and a.id_Education =e.ID;";
            All_resume = new ObservableCollection<Applicant>(ApplicantRepository.Instance.GetAllResume(sql));
            Listfoa= new List<Field_of_Activity>(FoaRepository.Instance.GetFields());
            Educations = new ObservableCollection<Education>(EducationRepository.Instance.GetEducation());
            Fields = new ObservableCollection<Field_of_Activity>(FoaRepository.Instance.GetFields());
            Citys = new ObservableCollection<City>(CityRepository.Instance.GetAllCity());
            Fields.Insert(0, new Field_of_Activity { Id = 0, Title = "Все" });
            SelectedField = Fields[0];
            Educations.Insert(0, new Education { Id = 0, Title = "Любое" });
            SelectedEducation = Educations[0];
        }
        internal void SetMainVM(MainVM mainVM)
        {
            this.mainVM = mainVM;
        }
        private void SearchResume()
        {
            All_resume = new ObservableCollection<Applicant>(
            ApplicantRepository.Instance.Search(SearchText, MinSalary, XP, IsFlexShedule, IsFullEmp, SelectedField, SelectedEducation, SelectedCity));
        }
    }

}
