using CURSE.Model;
using CURSE.View;
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
    public class ListVacancy : BaseVM
    {
        private MainVM mainVM;
        private string searchText = "";
        private string isFlex = "";
        private string isFull = "";
        private string selVacEd = "";
        private string selVacCity = "";
        private int minSalary = 0;
        private int xp = 100;
        private bool isFullEmp = true;
        private bool isFlexShedule;
        private ObservableCollection<Vacancy> all_vac;
        private City selectedCity;
        private Education selectedEducation;    
        private Field_of_Activity selectedField;
        private Vacancy selectedVac;
        public VmCommand EmpInformation { get; }
        private Visibility infoVac = Visibility.Hidden;
        public Visibility InfoVac { get => infoVac; set { infoVac = value; Signal(); } }
        public string IsFlex
        {
            get => isFlex;
            set
            {
                isFlex = value;
                Signal();
            }
        }
        public string IsFull
        {
            get => isFull;
            set
            {
                isFull = value;
                Signal();
            }
        }
        public string SelVacEd
        {
            get => selVacEd;
            set
            {
                selVacEd = value;
                Signal();
            }
        }
        public string SelVacCity
        {
            get => selVacCity;
            set
            {
                selVacCity = value;
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
                SearchVacancy();
            }
        }
        public Education SelectedEducation
        {
            get => selectedEducation;
            set
            {
                selectedEducation = value;
                Signal();
                SearchVacancy();
            }
        }
        public City SelectedCity
        {
            get => selectedCity;
            set
            {
                selectedCity = value;
                Signal();
                SearchVacancy();
            }
        }
        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                SearchVacancy();
            }
        }
        public int MinSalary
        {
            get => minSalary;
            set
            {
                minSalary = value;
                SearchVacancy();
            }
        }
        public int XP
        {
            get => xp;
            set
            {
                xp = value;
                SearchVacancy();
            }
        }
        public bool IsFullEmp
        {
            get => isFullEmp;
            set
            {
                isFullEmp = value;
                SearchVacancy();
            }
        }
        public bool IsFlexShedule
        {
            get => isFlexShedule;
            set
            {
                isFlexShedule = value;
                SearchVacancy();
            }
        }
       
        public ObservableCollection<Education > Educations { get; set; } = new ObservableCollection<Education>();
        public ObservableCollection<City> Citys { get; set; } = new ObservableCollection<City>();
        public ObservableCollection<Field_of_Activity> Fields { get; set; }
        public Vacancy SelectedVac
        {
            get => selectedVac;
            set
            {
                selectedVac = value;
               
                WatchInfo();
                Signal();
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
                string sql = "SELECT v.ID, v.employer_id, v.`Required-XP`, v.`id_Required-Education`, v.id_city, v.MINSalary, v.MAXSalary, v.Responsibilities, v.Requirements, v.Conditions, v.Description, v.IsFlexibleSchedule, v.Title, foa.ID AS foaId, foa.Field_name AS foaTitle FROM `Cross_Vacancy_Field-of-activity` cvfoa, Vacancy v, `Fields-of-activity` foa WHERE cvfoa.id_vac = v.ID AND cvfoa.id_field = foa.ID ORDER by v.ID;";
                All_vac = new ObservableCollection<Vacancy>(VacancyRepository.Instance.GetAllVacancy(sql));
                Educations = new ObservableCollection<Education>(EducationRepository.Instance.GetEducation());
                Fields = new ObservableCollection<Field_of_Activity>(FoaRepository.Instance.GetFields());
                Citys = new ObservableCollection<City>(CityRepository.Instance.GetAllCity());
                Fields.Insert(0, new Field_of_Activity { Id = 0, Title = "Все" });
                SelectedField = Fields[0];
                Educations.Insert(0, new Education { Id = 0, Title = "Любое" });
                SelectedEducation = Educations[0];
                Citys.Insert(0, new City { Id = 0, City_Name = "Город" });
                SelectedCity = Citys[0];
            EmpInformation = new VmCommand(() =>
            {
                SelectedVacancy.Instance.selectedVac = SelectedVac;
                EmpInfo empInfo = new EmpInfo();
                empInfo.ShowDialog();
            });
        }
        internal void SetMainVM(MainVM mainVM)
        {
            this.mainVM = mainVM;
        }
        private void WatchInfo()
        {
            if(SelectedVac!=null&& SelectedVac.Id!=0) 
            {
                if(SelectedVac.IsFlexibleSchedule) 
                {
                    IsFlex = "Гибкий график работы";
                }
                else { IsFlex = "Гибкий график не поддерживается"; }
                if (SelectedVac.IsFullEmployment)
                {
                    IsFull = "Полная занятость";
                }
                else { IsFull = "Неполная занятость"; }
                foreach (var ed in Educations)
                {
                    if (SelectedVac.IdRequiredEducation == ed.Id)
                    {
                        SelVacEd = ed.Title;
                    }
                }
                foreach (var city in Citys)
                {
                    if (SelectedVac.Id_City == city.Id)
                    {
                        SelVacCity = city.City_Name;
                    }
                }
                InfoVac = Visibility.Visible; 
            }
            else InfoVac = Visibility.Hidden;
        }
        private void SearchVacancy()
        {
            All_vac = new ObservableCollection<Vacancy>(
                    VacancyRepository.Instance.Search(SearchText, MinSalary, XP, IsFlexShedule, IsFullEmp, SelectedField, SelectedEducation, SelectedCity));

        }
    }

 }
    
