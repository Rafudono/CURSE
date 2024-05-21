using CURSE.Model;
using CURSE.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CURSE.ViewModel
{
    public class MyDocumentsVM:BaseVM
    {
        public VmCommand Save { get; }
        private Employer emp = new();
        private Human hum = new();
        public string password {  get; set; }
        Action close;
        public Employer Emp
        {
            get => emp;
            set
            {
                emp = value;
                Signal();
            }
        }
        public Human Hum
        {
            get => hum;
            set
            {
                hum = value;
                Signal();
            }
        }

        // список всех организаций-партнёров
        //кнопочка добавить организацию-партнера
        //переключение на окно с формой для добавления организации-партнера     // Моя организация: выбрать название организации работодателя по id_org в работодателе
        // updateEmp
        public MyDocumentsVM()
        {
            Emp = EmpAuthorized.Instance.loginEmp;
            Hum = HumAuthorization.Instance.loginHuman;
            Save = new VmCommand(() => 
            {
                if (password != null|| password!="")
                {

                    if (Md5.HashPassword(password) == Hum.Password)
                    {
                        EmployerRepository.Instance.UpdateEmployer(Emp);
                        EmpAuthorized.Instance.loginEmp = Emp;
                        MessageBox.Show("Данные об организации изменены");
                        close?.Invoke();
                    }
                    else MessageBox.Show("Неверный пароль");
                }
                else
                    MessageBox.Show("Введите текущий пароль");
            });
        }
        internal void SetClose(Action close)
        {
            this.close = close;
        }
    }
}