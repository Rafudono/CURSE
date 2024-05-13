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
    public class ChangeResumeVM:BaseVM
    {
        Action close;
        private Human hum = new();
        private Applicant applicant = new Applicant();
        private City selectedCity;
        public string CurPassword { get; set; }
        public Human Hum
        {
            get => hum;
            set
            {
                hum = value;
                Signal();
            }
        }
        public Applicant Appl
        {
            get => applicant;
            set
            {
                applicant = value;
                Signal();
            }
        }
        public City SelectedCity
        {
            get => selectedCity;
            set
            {
                selectedCity = value;
                Signal();
            }
        }
        public VmCommand Save { get; set; }
        public ObservableCollection<City> Citys { get; set; } = new ObservableCollection<City>();

        public ChangeResumeVM()
        {
            Hum = HumAuthorization.Instance.loginHuman;
            Appl = ApplAuthorized.Instance.loginAppl;
            Citys = new ObservableCollection<City>(CityRepository.Instance.GetAllCity());
            int i;
            foreach (City city in Citys) 
            { 
                if (city.Id == Appl.Id_City) 
                    SelectedCity = city; 
            }
            string sql = "";
            Save = new VmCommand(() =>
            {
                if (CurPassword == Hum.Password)
                {
                    Appl.Id_City=SelectedCity.Id;
                    ApplicantRepository.Instance.UpdateResume(Appl);
                    close?.Invoke();
                }
                else { MessageBox.Show("Необходимо указать текущий пароль"); }
            });
        }
        internal void SetClose(Action close)
        {
            this.close = close;
        }
        
    }
}
