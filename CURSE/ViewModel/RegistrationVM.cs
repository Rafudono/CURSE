using CURSE.View;
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

        private Visibility step0 = Visibility.Visible;
        private Visibility step1 = Visibility.Hidden;
        private Visibility step2 = Visibility.Hidden;
        private Visibility step3 = Visibility.Hidden;
        public Visibility Step0 { get => step0; set { step0 = value; Signal(); } } 

        public Visibility Step1 { get => step1; set { step1 = value; Signal(); } }
        public Visibility Step2 { get => step2; set { step2 = value; Signal(); } } 
        public Visibility Step3 { get => step3; set { step3 = value; Signal(); } }
        public VmCommand ChangeNEXTVisibility {  get; set; }

        public RegistrationVM()
        {

            ChangeNEXTVisibility = new VmCommand(() =>
            {
                if (Step0 == Visibility.Visible)
                {
                    Step0 = Visibility.Hidden;
                    Step1 = Visibility.Visible;
                }
                else if (Step1 == Visibility.Visible)
                {
                    Step1 = Visibility.Hidden;
                    Step2 = Visibility.Visible;
                }
                else if (Step2 == Visibility.Visible)
                {
                    Step2 = Visibility.Hidden;
                    Step3 = Visibility.Visible;
                }
                else
                {
                    registration.Close();
                    MessageBox.Show("Вы успешно зарегистрировались!");
                }
            });
        }
        Registration registration;
        internal void RegisterWindow(Registration registration)
        {
            this.registration = registration;   
        }
    }
}
