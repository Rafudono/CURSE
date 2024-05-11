using CURSE.Model;
using CURSE.View;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CURSE.ViewModel
{
    public class MainVM : BaseVM
    {
        public VmCommand Login { get; }
        public VmCommand Registration { get; }
        public VmCommand Search { get; }

        private Page vacancyPage;
        private Page resumePage;
        private Page authorizationPage;


        private Visibility resume = Visibility.Hidden;
        private Visibility vacancy = Visibility.Hidden;
        private Visibility notice = Visibility.Hidden;
        private Visibility settings = Visibility.Hidden;
        public Visibility Resume { get => resume; set { resume = value; Signal(); } }
        public Visibility Vacancy { get => vacancy; set { vacancy = value; Signal(); } }
        public Visibility Notice { get => notice; set { notice = value; Signal(); } }
        public Visibility Settings { get => settings; set { settings = value; Signal(); } }
        public Page VacancyPage
        {
            get => vacancyPage;
            set
            {
                vacancyPage = value;
                Signal();
            }
        }
        public Page ResumePage
        {
            get => resumePage;
            set
            {
                resumePage = value;
                Signal();
            }
        }
        public Page AuthorizationPage
        {
            get => authorizationPage;
            set
            {
                authorizationPage = value;
                Signal();
            }
        }
        public MainVM()
        {
            Registration = new VmCommand(() =>
            {
                Registration taskwindow = new Registration();
                taskwindow.ShowDialog();
            });
            Login = new VmCommand(() =>
            {
                LogIn taskwindow = new LogIn();
                taskwindow.ShowDialog();
            });
            Search = new VmCommand(() =>
            {
                OpenSearch();
            });

            OpenSearch();
        }
        //static MainVM instance;
        //public static MainVM Instance
        //{
        //    get
        //    {
        //        if (instance == null)

        //            instance = new MainVM();
        //        return instance;
        //    }
        //}
        //public Page applicantView = new ApplicantView(); //не сигнал
        //public Page employerView = new EmployerView();
        private void OpenSearch()
        {
            VacancyPage = new ApplicantView(this);
            ResumePage = new EmployerView(this); 
           // AuthorizationPage = new LogIn(this);
        }
        //public void ChangeVisibilityVac()
        //{
        //    if (HumAuthorization.Instance.loginHuman.Name != null)
        //    {
        //        Vacancy = Visibility.Visible;
        //        Notice = Visibility.Visible;
        //        Settings = Visibility.Visible;
        //        Signal();
        //    }
        //}
    }
}
