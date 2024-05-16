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
        public VmCommand DelUser { get; }
        public VmCommand VisMyVacPage { get; }
        public VmCommand GoBack { get; }
        private Page vacancyPage;
        private Page resumePage;
        private Page myVacPage;

        private Visibility resume = Visibility.Hidden;
        private Visibility settings = Visibility.Hidden;
        private Visibility documents = Visibility.Hidden;
        private Visibility visMyVac = Visibility.Hidden;
        private Visibility authVis = Visibility.Visible;
        private Visibility reg = Visibility.Visible;
        public Visibility Resume { get => resume; set { resume = value; Signal(); } }
        public Visibility Settings { get => settings; set { settings = value; Signal(); } }
        public Visibility Documents { get => documents; set { documents = value; Signal(); } }
        public Visibility VisMyVac { get => visMyVac; set { visMyVac = value; Signal(); } }
        public Visibility AuthVis { get => authVis; set { authVis = value; Signal(); } }
        public Visibility Reg { get => reg; set { reg = value; Signal(); } }
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
                if (ApplAuthorized.Instance.loginAppl.HumanId != 0)
                {
                    Resume = Visibility.Visible;
                    Settings = Visibility.Visible;
                    AuthVis = Visibility.Hidden;
                    Reg = Visibility.Hidden;
                }
                if(EmpAuthorized.Instance.loginEmp.id_human != 0)
                {
                    VisMyVac = Visibility.Visible;
                    Documents = Visibility.Visible;
                    Settings = Visibility.Visible;
                    AuthVis = Visibility.Hidden;
                    Reg = Visibility.Hidden;
                    MyVacVM myVacVM = new MyVacVM();
                    MyVacPage.DataContext = myVacVM;
                }
                Signal();
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
                    if (ApplAuthorized.Instance.loginAppl.HumanId != 0)
                    {
                        ChangeResume taskwindow = new ChangeResume(ApplAuthorized.Instance.loginAppl);
                        taskwindow.ShowDialog();
                    }
                    else { MessageBox.Show("У вас нет доступа к этой функции"); }
                }
                else
                    MessageBox.Show("Войдите в аккаунт");
            });
            DelUser = new VmCommand(() =>
            {
                if (HumAuthorization.Instance.loginHuman.Name != null && HumAuthorization.Instance.loginHuman.Password != null)
                {

                    var result= MessageBox.Show("Вы правда хотите удалить аккаунт?", "Удаление", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        DeleteUser taskwindow = new DeleteUser();
                        taskwindow.ShowDialog();
                    }
                }
                else
                    MessageBox.Show("Войдите в аккаунт");
            });
            VisMyVacPage = new VmCommand(() =>
            {

            if (HumAuthorization.Instance.loginHuman.Name != null && HumAuthorization.Instance.loginHuman.Password != null)
            {
                    if (EmpAuthorized.Instance.loginEmp.id_human != 0)
                    {
                        Resume = Visibility.Hidden;
                        Documents = Visibility.Visible;
                    }
                    else { MessageBox.Show("У вас нет доступа к этой функции"); }
                }
                else
                    MessageBox.Show("Войдите в аккаунт");
            });
            OpenSearch();
            GoBack = new VmCommand(() =>
            {

                if (HumAuthorization.Instance.loginHuman.Name != null && HumAuthorization.Instance.loginHuman.Password != null)
                {
                    if (EmpAuthorized.Instance.loginEmp.id_human != 0)
                    {
                        Resume = Visibility.Visible;
                        Documents = Visibility.Hidden;
                    }
                    else { MessageBox.Show("У вас нет доступа к этой функции"); }
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
            MyVacPage = new MyVac(this);
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
