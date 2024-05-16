using CURSE.Model;
using CURSE.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CURSE.ViewModel
{
    public class DeleteUserVM:BaseVM
    {
        public string password { get; set; }
        public VmCommand Delete {  get; set; }
        public DeleteUserVM()
        {
            Delete = new VmCommand(() =>
            {
                if (password != null)
                {
                    if (Md5.HashPassword(password) == HumAuthorization.Instance.loginHuman.Password)
                    {
                        HumanRepository.Instance.Delete(HumAuthorization.Instance.loginHuman);
                    }
                    else
                        MessageBox.Show("неверный пароль");
                }
                else
                { MessageBox.Show("для удаления аккаунта введите текущий пароль"); }
            });
            
        }
    }
}
