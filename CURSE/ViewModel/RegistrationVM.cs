using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CURSE.ViewModel
{
    public class RegistrationVM : BaseVM
    {
        private Visibility step1 = Visibility.Hidden;

        public Visibility Step1 { get => step1; set { step1 = value; Signal(); } }
        public Visibility Step2 { get; set; } = Visibility.Hidden;
        public Visibility Step3 { get; set; } = Visibility.Hidden;


    }
}
