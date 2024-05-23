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
        public MyDocumentsVM()
        {
            Emp = EmpAuthorized.Instance.loginEmp;
            Hum = HumAuthorization.Instance.loginHuman;
            Save = new VmCommand(() =>
            {
                if (Emp.Org_Name != null && Emp.Org_Name != "" && Emp.Org_Name != "-")
                {
                    EmployerRepository.Instance.UpdateEmployer(Emp);
                    EmpAuthorized.Instance.loginEmp = Emp;
                    MessageBox.Show("Данные об организации изменены");
                    close?.Invoke();
                }
                else MessageBox.Show("Укажите имя организации");
            });
        }
        internal void SetClose(Action close)
        {
            this.close = close;
        }
    }
}