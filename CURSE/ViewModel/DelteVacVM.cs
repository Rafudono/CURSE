using CURSE.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CURSE.ViewModel
{
    public class DelteVacVM:BaseVM
    {
        Action close;
        public string password { get; set; }
        public VmCommand Delete { get; }
        private Vacancy selectedVac;
        public Vacancy SelectedVac
        {
            get => selectedVac;
            set
            {
                selectedVac = value;
                Signal();
            }
        }
        public DelteVacVM()
        {
            Delete = new VmCommand(() =>
            {
                SelectedVac = SelectedVacancy.Instance.selectedVac;
                if (password != null)
                {

                    if (Md5.HashPassword( password) == HumAuthorization.Instance.loginHuman.Password)
                    {
                        VacancyRepository.Instance.DeleteVac(SelectedVac);
                    }
                    else
                    {
                        MessageBox.Show("введён неверный пароль");
                    }
                }
                else
                { MessageBox.Show("Введите пароль"); }
            });
        }
        internal void SetClose(Action close)
        {
            this.close = close;
        }
    }
}
