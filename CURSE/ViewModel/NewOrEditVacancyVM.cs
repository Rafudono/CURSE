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
    public class NewOrEditVacancyVM:BaseVM
    {
        Action close;
        ListBox listFoa;
        private Human hum = new();
        private Vacancy vacancy = new Vacancy();
        private City selectedCity;
        private Education selectedEducation;
        private Field_of_Activity selectedField;
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
        public NewOrEditVacancyVM()
        {
            Vac = SelectedVacancy.Instance.selectedVac;
            Citys = new ObservableCollection<City>(CityRepository.Instance.GetAllCity());
            Educations = new ObservableCollection<Education>(EducationRepository.Instance.GetEducation());
            Fields = FoaRepository.Instance.GetFields();
            foreach (City city in Citys)
            {
                if (city.Id == Vac.Id_City)
                    SelectedCity = city;
            }
            string sql = "";
            Save = new VmCommand(() =>
            {
                if (CurPassword != null)
                {
                    if (Md5.HashPassword(CurPassword) == Hum.Password)
                    {

                        Vac.FieldofActivity.Clear();
                        foreach (Field_of_Activity foa in listFoa.SelectedItems)
                            Vac.FieldofActivity.Add(foa);
                        Vac.Id_City = SelectedCity.Id;
                        VacancyRepository.Instance.UpdateVacancy(Vac);
                        close?.Invoke();
                    }
                }
                else { MessageBox.Show("Необходимо указать текущий пароль"); }

            });
        }
    }
}
