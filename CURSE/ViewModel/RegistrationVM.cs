using CURSE.Model;
using CURSE.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
        Action close;

        private Visibility step0 = Visibility.Visible;
        private Visibility step1 = Visibility.Hidden;
        private Visibility step2 = Visibility.Hidden;
        private Visibility step3 = Visibility.Hidden;
        public Visibility Step0 { get => step0; set { step0 = value; Signal(); } }

        public Visibility Step1 { get => step1; set { step1 = value; Signal(); } }
        public Visibility Step2 { get => step2; set { step2 = value; Signal(); } }
        public Visibility Step3 { get => step3; set { step3 = value; Signal(); } }
        public VmCommand ChangeNEXTVisibility { get; set; }
        public VmCommand ChangePrevVisibility { get; set; }
        public Human registrateHuman { get; set; } = new Human { Photo=File.ReadAllBytes("user.png") };
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
                    if (registrateHuman.Name != null && registrateHuman.Name!="" && registrateHuman.Surname != null && registrateHuman.Surname !="")
                    {
                        if (registrateHuman.Middle_Name == null || registrateHuman.Middle_Name == "")
                            registrateHuman.Middle_Name = "-";
                        Step0 = Visibility.Hidden;
                        Step1 = Visibility.Visible;
                    }
                    else
                        MessageBox.Show("Заполните все необходимые поля");
                }
                else if (Step1 == Visibility.Visible)
                {
                    if (registrateHuman.Email != null && registrateHuman.Email!="" && password1 != null && password1!="")
                    {
                        string sql = "select * from Human h where h.Email ='"+registrateHuman.Email+"';";
                        var listSameEmail= new ObservableCollection<Human>( HumanRepository.Instance.GetPeople(sql));
                        if (listSameEmail.Count == 0)
                        {
                            if (registrateHuman.Email.Contains("@"))
                            {
                                if (password1 != password2)
                                {
                                    MessageBox.Show("Введите одинаковые пароли в поля");
                                    return;
                                }
                                Step1 = Visibility.Hidden;
                                Step2 = Visibility.Visible;
                            }
                            else MessageBox.Show("Неверный формат электронной почты");
                        }
                        else MessageBox.Show("Аккаунт с такой почтой уже существует");
                    }
                    else
                        MessageBox.Show("Заполните все необходимые поля");
                }
                else if (Step2 == Visibility.Visible)
                {
                    if (registrateHuman.PhoneNumber != null&&registrateHuman.PhoneNumber!="")
                    {
                        if (registrateHuman.PhoneNumber.Length == 11)
                        {
                            Step2 = Visibility.Hidden;
                            Step3 = Visibility.Visible;
                        }
                        else MessageBox.Show("Неверный формат номера телефона");
                    }
                    else
                        MessageBox.Show("Заполните все необходимые поля");
                }
                else
                {
                    if (regLikeApplicant || regLikeEmp)
                    {
                        registrateHuman.Password = Md5.HashPassword(password2);
                        HumanRepository.Instance.AddHuman(registrateHuman);
                        int trueid = HumanRepository.Instance.GetTrueId(registrateHuman);
                        if (trueid != 0)

                        {
                            if (regLikeApplicant)
                            {
                                ApplicantRepository.Instance.AddApplicant(new Applicant { HumanId = trueid, Id_City = 5 });
                            }
                            if (regLikeEmp)
                            {
                                EmployerRepository.Instance.AddEmployer(new Employer { id_human = trueid });
                                MessageBox.Show("Чтобы размещать вакансии добавьте информацию о вашей организации", "Важно!");
                            }
                            close?.Invoke();
                            MessageBox.Show("Вы успешно зарегистрировались!");
                        }
                    }
                    else
                        MessageBox.Show("Выберите роль");
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
        internal void SetClose(Action close)
        {
            this.close = close;
        }
    }
}
