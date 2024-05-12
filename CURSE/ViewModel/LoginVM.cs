using CURSE.Model;
using CURSE.View;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection.PortableExecutable;
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
               string sql = "SELECT h.ID, h.Surname, h.Name, h.Middle_Name, h.Email, h.PHONE_NUMBER, h.Password, h.Birthday, h.Photo, a.id_human  from Human h, CURSE.Applicant a  WHERE h.ID = " + HumAuthorization.Instance.loginHuman.Id + " and h.ID =a.id_human ;";
               var contains= HumanRepository.Instance.GetPeople(sql);
            if (contains.Count()!=0)
                {
                    close?.Invoke();
                    MessageBox.Show("Вы успешно вошли в аккаунт!");
                    mainVM.Resume = Visibility.Visible; 
                    mainVM.Settings = Visibility.Visible; 
                }

                else
                {
                    sql = "";
                    sql = "SELECT h.ID, h.Surname, h.Name, h.Middle_Name, h.Email, h.PHONE_NUMBER, h.Password, h.Birthday, h.Photo, e.id_human  from Human h, CURSE.Employer e  WHERE h.ID = " + HumAuthorization.Instance.loginHuman.Id + " and h.ID =e.id_human ;";
                    var econtains = HumanRepository.Instance.GetPeople(sql);
                    if (econtains.Count() != 0)
                    {
                        close?.Invoke();
                        MessageBox.Show("Вы успешно вошли в аккаунт!");
                        mainVM.Vacancy = Visibility.Visible;
                        mainVM.Settings = Visibility.Visible;
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
