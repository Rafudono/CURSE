using CURSE.Model;
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
   public class NEWVacancyVM:BaseVM
    {
        Action close;
        ListBox listFoa;
        private Human hum = new();
        private Vacancy vacancy = new Vacancy();
        private City selectedCity;
        private Education selectedEducation;
        private Field_of_Activity selectedField;
        public Human Hum
        {
            get => hum;
            set
            {
                hum = value;
                Signal();
            }
        }
        public Vacancy Vac
        {
            get => vacancy;
            set
            {
                vacancy = value;
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
        public Field_of_Activity SelectedField
        {
            get => selectedField;
            set
            {
                selectedField = value;
                Signal();
            }
        }
        public Education SelectedEducation
        {
            get => selectedEducation;
            set
            {
                selectedEducation = value;
                Signal();
            }
        }
        public VmCommand Save { get; set; }
        public List<Field_of_Activity> Fields { get; set; }
        public ObservableCollection<City> Citys { get; set; }
        public ObservableCollection<Education> Educations { get; set; }
        public NEWVacancyVM()
        {
            Vac = new Vacancy();
            Citys = new ObservableCollection<City>(CityRepository.Instance.GetAllCity());
            Educations = new ObservableCollection<Education>(EducationRepository.Instance.GetEducation());
            Fields = FoaRepository.Instance.GetFields();
            Hum = HumAuthorization.Instance.loginHuman;
            foreach (City city in Citys)
            {
                if (city.Id == Vac.Id_City)
                    SelectedCity = city;
            }
            foreach (Education ed in Educations)
            {
                if (ed.Id == Vac.IdRequiredEducation)
                    SelectedEducation = ed;
            }
            string sql = "";
            Save = new VmCommand(() =>
            {
                Vac.EmployerId = EmpAuthorized.Instance.loginEmp.ID;
                Vac.FieldofActivity.Clear();
                foreach (Field_of_Activity foa in listFoa.SelectedItems)
                    Vac.FieldofActivity.Add(foa);

                Vac.Id_City = SelectedCity.Id;
                Vac.IdRequiredEducation = SelectedEducation.Id;
                VacancyRepository.Instance.AddVacancy(Vac);
                close?.Invoke();
            });
        }
        internal void SetClose(Action close)
        {
            this.close = close;
        }
        internal void SetMainVM(ListBox listTags)
        {
            this.listFoa = listTags;
        }
        internal void SetEditVacancy(Vacancy selectedVac, System.Windows.Threading.Dispatcher dispatcher)
        {
            Vac = selectedVac;
            foreach (var foa in Vac.FieldofActivity)
            {
                var search = Fields.FirstOrDefault(s => s.Title == foa.Title);
                if (search != null)
                {
                    search.Selected = true;
                }
            }
        }
    }
}
