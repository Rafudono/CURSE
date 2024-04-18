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
    internal class MainVM : BaseVM
    {
        public VmCommand Login { get; }
        public VmCommand Registration { get; }
        private Page currentPage;

        public Page CurrentPage
        {
            get => currentPage;
            set
            {
                currentPage = value;
                Signal();
            }
        }
        public MainVM()
        {
            Login = new VmCommand(() =>
            {
                LogIn taskwindow = new LogIn();
                taskwindow.ShowDialog();
            });
            Registration = new VmCommand(() =>
            {
                Registration taskwindow = new Registration();
                taskwindow.ShowDialog();
            });
        }
        public Page applicantView = new ApplicantView(); //не сигнал

    }
}
