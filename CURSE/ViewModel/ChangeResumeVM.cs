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
    public class ChangeResumeVM:BaseVM
    {
        MainVM mainVM;

        Action close;
        ListBox listFoa;
        private Human hum = new();
        private Applicant applicant = new Applicant();
        private Education selectedEducation;
        private City selectedCity;
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
        public Education SelectedEducation
        {
            get => selectedEducation;
            set
            {
                selectedEducation = value;
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
        public VmCommand Save { get; set; }
        public List<Field_of_Activity> Fields { get; set; }
        public ObservableCollection<City> Citys { get; set; } 
        public ObservableCollection<Education> Educations { get; set; }

        public ChangeResumeVM()
        {
            Hum = HumAuthorization.Instance.loginHuman;
            Appl = ApplAuthorized.Instance.loginAppl;
            Citys = new ObservableCollection<City>(CityRepository.Instance.GetAllCity());
            Educations = new ObservableCollection<Education>(EducationRepository.Instance.GetEducation());
            Fields = FoaRepository.Instance.GetFields();
            foreach (City city in Citys)
            {
                if (city.Id == Appl.Id_City)
                    SelectedCity = city;
            }
            foreach (Education ed in Educations)
            {
                if (ed.Id == Appl.IdEducation)
                    SelectedEducation = ed;
            }
            Save = new VmCommand(() =>
            {
                if (Appl.RTitle != null && Appl.RTitle != "")
                {
                    if (SelectedCity != null)
                    {

                        if (SelectedEducation == null)
                            foreach (Education ed in Educations)
                            {
                                if (ed.Id == 5)
                                    SelectedEducation = ed;
                            }
                        Appl.FieldofActivity.Clear();
                        foreach (Field_of_Activity foa in listFoa.SelectedItems)
                            Appl.FieldofActivity.Add(foa);
                        if (Appl.FieldofActivity.Count > 0)
                        {
                            Appl.Id_City = SelectedCity.Id;
                            Appl.IdEducation = SelectedEducation.Id;
                            ApplicantRepository.Instance.UpdateResume(Appl);
                            close?.Invoke();
                        }
                        else MessageBox.Show("Укажите сферу деятельности");
                    }
                    else MessageBox.Show("Выберите город из списка");
                }
                else MessageBox.Show("Заполните все обязательные поля");

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
        internal void SetEditResume(Applicant selectedAppl, System.Windows.Threading.Dispatcher dispatcher)
        {
            Appl = selectedAppl;
            foreach (var foa in Appl.FieldofActivity)
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
