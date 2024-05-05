using CURSE.Model;
using CURSE.View;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CURSE.ViewModel
{
    public class LoginVM : BaseVM
    {
        private MainVM mainVM;
        public Human loginHuman { get; set; } = new();
        public string loginEmail {  get; set; }
        public string loginPassword { get; set; }
        public VmCommand loginCommand { get; set; }
        private ApplicantVM AVM { get; set; }
        private EmployerVM EVM { get; set; }
        LogIn logIn;
        public LoginVM()
        {
            loginCommand = new VmCommand(() =>
            {
            var hash = Md5.HashPassword(loginEmail);
            int id = HumanRepository.Instance.LoginHuman(loginEmail, hash);
            loginHuman.Id = id;
            for (int i = 0; i < AVM.Applicants.Count; i++)
            {
                    if (AVM.Applicants[i].HumanId == loginHuman.Id)
                    {
                        logIn.Close();
                        MessageBox.Show("Вы успешно вошли в аккаунт!");
                        mainVM.Resume = Visibility.Visible;
                        mainVM.Notice = Visibility.Visible;
                        mainVM.Settings = Visibility.Visible;
                    }

                    else if (EVM.Employers[i].id_human == loginHuman.Id)
                    {
                        logIn.Close();
                        MessageBox.Show("Вы успешно вошли в аккаунт!");
                        mainVM.Vacancy= Visibility.Visible;
                        mainVM.Notice = Visibility.Visible;
                        mainVM.Settings = Visibility.Visible;
                    }
                    else
                        MessageBox.Show("Непредвиденная ошибка)");
                }
            
            });
        }
    }
}
