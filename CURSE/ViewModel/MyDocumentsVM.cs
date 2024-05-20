using CURSE.Model;
using CURSE.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CURSE.ViewModel
{
    public class MyDocumentsVM:BaseVM
    {
        public VmCommand Save { get; }
        private Employer emp = new();
        public string OrgName = "";
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

        // список всех организаций-партнёров
        //кнопочка добавить организацию-партнера
        //переключение на окно с формой для добавления организации-партнера     // Моя организация: выбрать название организации работодателя по id_org в работодателе
        // updateEmp
        public MyDocumentsVM()
        {
            Emp = EmpAuthorized.Instance.loginEmp;
            Save = new VmCommand(() => 
            {
                Emp.Org_Name = OrgName;
                EmployerRepository.Instance.UpdateEmployer(Emp);
                EmpAuthorized.Instance.loginEmp=Emp;
                close?.Invoke();
            });
        }
        internal void SetClose(Action close)
        {
            this.close = close;
        }
    }
}