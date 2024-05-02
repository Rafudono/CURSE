using CURSE.View;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private MainVM mainVM;

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
        //public Page applicantView = new ApplicantView(); //не сигнал
        //public Page employerView = new EmployerView();
        private void OpenSearch()
        {
            VacancyPage = new ApplicantView(this);
            ResumePage = new EmployerView(this); 
           // AuthorizationPage = new LogIn(this);
        }

    }
}
