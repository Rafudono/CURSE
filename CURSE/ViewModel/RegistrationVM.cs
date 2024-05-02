using CURSE.Model;
using CURSE.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xaml;

namespace CURSE.ViewModel
{
    public class RegistrationVM : BaseVM
    {
        private MainVM mainVM;


        private Visibility step0 = Visibility.Visible;
        private Visibility step1 = Visibility.Hidden;
        private Visibility step2 = Visibility.Hidden;
        private Visibility step3 = Visibility.Hidden;
        public Visibility Step0 { get => step0; set { step0 = value; Signal(); } } 

        public Visibility Step1 { get => step1; set { step1 = value; Signal(); } }
        public Visibility Step2 { get => step2; set { step2 = value; Signal(); } } 
        public Visibility Step3 { get => step3; set { step3 = value; Signal(); } }
        public VmCommand ChangeNEXTVisibility {  get; set; }
        public VmCommand ChangePrevVisibility { get; set; }
        public Human registrateHuman { get; set; }
        public bool regLikeApplicant {  get; set; }
        public bool regLikeEmp {  get; set; }
        public string password1 {  get; set; }
        public string password2 { get; set; }

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
                    if (password1 != password2)
                    {
                        MessageBox.Show("Введите одинаковые пароли в поля");
                        return;
                    }
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
                    registrateHuman.Password = password2;
                    int id = HumanRepository.Instance.AddHuman(registrateHuman);
                    if (id != 0)
                    {
                        if (regLikeApplicant)
                        { 
                        ApplicantRepository.Instance.AddApplicant(new Applicant {  HumanId = id });
                        }
                        if (regLikeEmp)
                        {
                            EmployerRepository.Instance.AddEmployer(new Employer { id_human = id});
                        }
                        registration.Close();
                        MessageBox.Show("Вы успешно зарегистрировались!");
                    }

                }
            });
            ChangePrevVisibility = new VmCommand(() =>
            {
                if (Step1 == Visibility.Visible)
                {
                    Step1 = Visibility.Hidden;
                    Step0 = Visibility.Visible;
                }
                else if (Step2 == Visibility.Visible)
                {
                    Step2 = Visibility.Hidden;
                    Step1 = Visibility.Visible;
                }
                else if (Step3 == Visibility.Visible)
                {
                    Step3 = Visibility.Hidden;
                    Step2 = Visibility.Visible;
                }
            });
        }
        Registration registration;
        internal void RegisterWindow(Registration registration)
        {
            this.registration = registration;   
        }
        internal void SetMainVM(MainVM mainVM)
        {
            this.mainVM = mainVM;
        }
    }
}
