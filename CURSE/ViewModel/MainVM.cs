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
        public VmCommand SettingsWindow { get; }
        public VmCommand ChangeResWindow { get; }
        private Page vacancyPage;
        private Page resumePage;
        private Page myVacPage;

        private Visibility resume = Visibility.Visible;
        private Visibility vacancy = Visibility.Visible;
        private Visibility settings = Visibility.Visible;
        public Visibility Resume { get => resume; set { resume = value; Signal(); } }
        public Visibility Vacancy { get => vacancy; set { vacancy = value; Signal(); } }
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
        public Page MyVacPage
        {
            get => myVacPage;
            set
            {
                myVacPage = value;
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
                Signal(); //visibility не меняется!!! 
            });
            Search = new VmCommand(() =>
            {
                OpenSearch();
            });
            SettingsWindow = new VmCommand(() =>
            {
                if (HumAuthorization.Instance.loginHuman.Name != null&& HumAuthorization.Instance.loginHuman.Password!=null)
                {
                    ChangeData taskwindow = new ChangeData();
                    taskwindow.ShowDialog();
                }
                else
                    MessageBox.Show("Войдите в аккаунт");
            });

            ChangeResWindow = new VmCommand(() =>
            {
                if (HumAuthorization.Instance.loginHuman.Name != null && HumAuthorization.Instance.loginHuman.Password != null)
                {
                    ChangeResume taskwindow = new ChangeResume(ApplAuthorized.Instance.loginAppl);
                     taskwindow.ShowDialog();
                }
                else
                    MessageBox.Show("Войдите в аккаунт");
            });
            OpenSearch();
        }
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
