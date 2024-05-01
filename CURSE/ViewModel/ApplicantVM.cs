using CURSE.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CURSE.ViewModel
{
    public class ApplicantVM : BaseVM
    {
        private MainVM mainVM;
        private string searchText = "";
        private ObservableCollection<Applicant> applicants;
        private Applicant selectedApplicant;
        //public VmCommand Create { get; set; }
        //public VmCommand Edit { get; set; }
        //public VmCommand Delete { get; set; }

        public Applicant SelectedApplicant
        {
            get => selectedApplicant;
            set
            {
                selectedApplicant = value;
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
        public ObservableCollection<Applicant> Applicants
        {
            get => applicants;
            set
            {
                applicants = value;
                Signal();
            }
        }

        public ApplicantVM()
        {
            //Fields = new ObservableCollection<Field_of_Activity>(FoaRepository.Instance.GetFields());
            //Fields.Insert(0, new Field_of_Activity { Id = 0, Title = "Все виды!!!!!" });     //!!!!
            //SelectedField = Fields[0];
            string sql = "SELECT a.ID, a.id_human, a.XP, a.id_Education, a.id_city, a.Salary , a.IsFullEmployment , a.IsFlexibleSchedule , foa.ID AS foaId, foa.Field_name AS foaTitle FROM CURSE.`Cross_Applicant_Field-of-activity` cafoa, CURSE.Applicant a, CURSE.`Fields-of-activity` foa WHERE cafoa.id_applicant = a.ID AND cafoa.id_field = foa.ID;";
            Applicants = new ObservableCollection<Applicant>(ApplicantRepository.Instance.GetAllResume(sql));
        }
        internal void SetMainVM(MainVM mainVM)
        {
            this.mainVM = mainVM;
        }
    }
}
