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
        public string password { get; set; }
        public VmCommand Delete { get; }
        public DelteVacVM()
        {
            Delete = new VmCommand(() =>
            {
                if (password != null)
                {

                    if (password == HumAuthorization.Instance.loginHuman.Password)
                    {
                        var selectedVac = SelectedVacancy.Instance.selectedVac;
                        VacancyRepository.Instance.DeleteVac(selectedVac);
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
    }
}
