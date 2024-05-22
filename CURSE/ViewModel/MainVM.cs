using CURSE.Model;
using CURSE.View;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.Cryptography.Pkcs;
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
        public VmCommand LogOut { get; }
        public VmCommand MyDoc {  get; }
        private Page vacancyPage;
        private Page resumePage;
        private Page myVacPage;
        private Page infoResPage;

        private Visibility resume = Visibility.Hidden;
        private Visibility settings = Visibility.Hidden;
        private Visibility documents = Visibility.Hidden;
        private Visibility visMyVac = Visibility.Hidden;
        private Visibility authVis = Visibility.Visible;
        private Visibility reg = Visibility.Visible;
        private Visibility guest = Visibility.Visible;
        private Visibility roleEmp = Visibility.Hidden;
        private Visibility roleAppl = Visibility.Hidden;
        private Visibility logOut = Visibility.Hidden;
        public Visibility Resume { get => resume; set { resume = value; Signal(); } }
        public Visibility Settings { get => settings; set { settings = value; Signal(); } }
        public Visibility Documents { get => documents; set { documents = value; Signal(); } }
        public Visibility VisMyVac { get => visMyVac; set { visMyVac = value; Signal(); } }
        public Visibility AuthVis { get => authVis; set { authVis = value; Signal(); } }
        public Visibility Reg { get => reg; set { reg = value; Signal(); } }
        public Visibility Guest { get => guest; set { guest = value; Signal(); } }
        public Visibility RoleEmp { get => roleEmp; set { roleEmp = value; Signal(); } }
        public Visibility RoleAppl { get => roleAppl; set { roleAppl = value; Signal(); } }
        public Visibility LogOutVis { get => logOut; set { logOut = value; Signal(); } }
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
                    Guest=Visibility.Hidden;
                    RoleAppl = Visibility.Visible;
                    LogOutVis = Visibility.Visible;
                }
                if(EmpAuthorized.Instance.loginEmp.id_human != 0)
                {
                    VisMyVac = Visibility.Visible;
                    Documents = Visibility.Visible;
                    Settings = Visibility.Visible;
                    AuthVis = Visibility.Hidden;
                    Reg = Visibility.Hidden;
                    Guest = Visibility.Hidden;
                    RoleEmp = Visibility.Visible;
                    LogOutVis = Visibility.Visible;

                    MyVacVM myVacVM = new MyVacVM();
                    MyVacPage.DataContext = myVacVM;
                }
                Signal();
            });
            LogOut = new VmCommand(() =>
            {
                HumAuthorization.Instance.loginHuman=new Human();
                ApplAuthorized.Instance.loginAppl=new Applicant();
                EmpAuthorized.Instance.loginEmp=new Employer(); //текущую страницу поменять
                Resume = Visibility.Hidden;
                VisMyVac = Visibility.Hidden;
                Documents = Visibility.Hidden;
                Settings = Visibility.Hidden;
                AuthVis = Visibility.Visible;
                Reg = Visibility.Visible;
                Guest = Visibility.Visible;
                RoleEmp = Visibility.Hidden;
                RoleAppl = Visibility.Hidden;
                LogOutVis = Visibility.Hidden;
                Signal();
                
            });
            Search = new VmCommand(() =>
            {
                OpenSearch();
            });
            SettingsWindow = new VmCommand(() =>
            {
                if (HumAuthorization.Instance.loginHuman.Id != 0&& HumAuthorization.Instance.loginHuman.Password!=null)
                {
                    ChangeData taskwindow = new ChangeData();
                    taskwindow.ShowDialog();

                    ListResume res = new ListResume();
                    ResumePage.DataContext = res;
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

                        ListResume res = new ListResume();
                        ResumePage.DataContext = res;
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

                        HumanRepository.Instance.Delete(HumAuthorization.Instance.loginHuman);
                        HumAuthorization.Instance.loginHuman = new();
                        ApplAuthorized.Instance.loginAppl = new();
                        EmpAuthorized.Instance.loginEmp = new();
                        if (HumAuthorization.Instance.loginHuman.Id==0) 
                        {
                            Resume = Visibility.Hidden;
                            VisMyVac = Visibility.Hidden;
                            Documents = Visibility.Hidden;
                            Settings = Visibility.Hidden;
                            AuthVis = Visibility.Visible;
                            Reg = Visibility.Visible;
                            Guest = Visibility.Visible;
                            RoleEmp = Visibility.Hidden;
                            RoleAppl = Visibility.Hidden;
                            LogOutVis = Visibility.Hidden;

                            ListResume res = new ListResume();
                            ResumePage.DataContext = res;

                            ListVacancy vac = new ListVacancy();
                            VacancyPage.DataContext = vac;
                        }
                    }
                }
                else
                    MessageBox.Show("Войдите в аккаунт");
            });
            MyDoc = new VmCommand(() =>
            {
                MyDocuments taskwindow = new MyDocuments();
                taskwindow.ShowDialog();
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
        }
    }
}
