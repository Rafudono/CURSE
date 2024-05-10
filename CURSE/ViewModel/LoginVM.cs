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
        private MainVM mainVM=new();
        public string loginEmail {  get; set; }
        public string loginPassword { get; set; }
        public VmCommand loginCommand { get; }
        Action close;
        public LoginVM()
        {
            
            loginCommand = new VmCommand(() =>
            {
                // var hash = Md5.HashPassword(loginPassword);
                Human login = HumanRepository.Instance.LoginHuman(loginEmail, loginPassword);
                HumAuthorization.Instance.loginHuman = login;
                string sql = "SELECT h.ID, a.id_human  from Human h, CURSE.Applicant a  WHERE a.id_human = " + HumAuthorization.Instance.loginHuman.Id + ";";
               var contains= HumanRepository.Instance.GetPeople(sql);
                if (contains != null)
                {
                    close?.Invoke();
                    MessageBox.Show("Вы успешно вошли в аккаунт!");
                    mainVM.Resume = Visibility.Visible; //< !--Visibility = "{Binding Resume}"-- >
                    mainVM.Settings = Visibility.Visible; //<!--Visibility="{Binding Vacancy}"-->
                    mainVM.Notice= Visibility.Visible;// <!--Visibility="{Binding Notice}"-->
                }                                     // <!--Visibility="{Binding Settings}"-->

                else
                {                
                    sql = "SELECT h.ID, e.id_human  from Human h,CURSE.Employer e WHERE e.id_human = " + HumAuthorization.Instance.loginHuman.Id + ";";
                    contains = HumanRepository.Instance.GetPeople(sql);
                    if (contains != null)
                    {
                        close?.Invoke();
                        MessageBox.Show("Вы успешно вошли в аккаунт!");
                        mainVM.Vacancy = Visibility.Visible;
                        mainVM.Settings = Visibility.Visible;
                        mainVM.Notice = Visibility.Visible;
                    }
                    else { MessageBox.Show("Непредвиденная ошибка)"); }
                }
            
            });
        }
        internal void SetClose(Action close)
        {
            this.close = close;
        }
    }
}
