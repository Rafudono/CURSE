using CURSE.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CURSE.ViewModel
{
    public class EmpInfoVM:BaseVM
    {
        private Human foundHum = new();
        private Employer foundEmp = new();
        Action close;
        public Employer FoundEmp
        {
            get => foundEmp;
            set
            {
                foundEmp = value;
                Signal();
            }
        }

        public Human FoundHum
        {
            get => foundHum;
            set
            {
                foundHum = value;
                Signal();
            }
        }
        public EmpInfoVM()
        {
            FoundEmp = EmployerRepository.Instance.FindEmp(SelectedVacancy.Instance.selectedVac.Id);
            FoundHum = HumanRepository.Instance.FindHum(FoundEmp.ID);
        }
        internal void SetClose(Action close)
        {
            this.close = close;
        }
    }
}
