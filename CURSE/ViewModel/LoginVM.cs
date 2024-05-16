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
                Human trylogin = HumanRepository.Instance.LoginHuman(loginEmail, Md5.HashPassword(loginPassword));
                HumAuthorization.Instance.loginHuman = trylogin;
               string sql = "SELECT h.ID, h.Surname, h.Name, h.Middle_Name, h.Email, h.PHONE_NUMBER, h.PASSWORD, h.Birthday, h.Photo, a.id_human  from Human h, CURSE.Applicant a  WHERE h.ID = " + HumAuthorization.Instance.loginHuman.Id + " and h.ID =a.id_human ;";
               var contains= HumanRepository.Instance.GetPeople(sql);
            if (contains.Count()!=0)
                {
                    ApplAuthorized.Instance.loginAppl=ApplicantRepository.Instance.LoginApplicant(trylogin.Id);
                   // string sql = "SELECT a.ID, a.id_human, a.XP, a.id_Education, a.id_city, a.Salary , a.IsFullEmployment , a.IsFlexibleSchedule, a.Description, a.ResumeTitle, foa.ID AS foaId, foa.Field_name AS foaTitle, h.Name, h.Middle_name , h.Surname, h.EMAIL, h.PHONE_NUMBER, h.Birthday FROM CURSE.`Cross_Applicant_Field-of-activity` cafoa, CURSE.Applicant a, Curse.Human h , CURSE.`Fields-of-activity` foa, CURSE.Education e WHERE cafoa.id_applicant = a.ID AND cafoa.id_field = foa.ID AND a.id_Education=e.ID and a.id_human =" + login.Id + ";";
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
                        EmpAuthorized.Instance.loginEmp=EmployerRepository.Instance.LoginEmployer(trylogin.Id);
                        close?.Invoke();
                        MessageBox.Show("Вы успешно вошли в аккаунт!");
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
