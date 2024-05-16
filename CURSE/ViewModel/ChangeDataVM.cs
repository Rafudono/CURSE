using CURSE.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CURSE.ViewModel
{
    public class ChangeDataVM:BaseVM
    {
        MainVM mainVM;
        Action close;
        private Human hum = new();
        public string CurPassword { get; set; }
        public string NewPassword { get; set; }
        public Human Hum
        {
            get => hum;
            set
            {
                hum = value;
                Signal();
            }
        }

        public VmCommand Save { get; set; }
        public VmCommand PickImage { get; set; }

        public ChangeDataVM()
        {
            Hum = HumAuthorization.Instance.loginHuman;
            Save = new VmCommand(() =>
            {
                if (Md5.HashPassword(CurPassword) == Hum.Password)
                {
                    if (NewPassword != null) 
                        Hum.Password = Md5.HashPassword(NewPassword);
                    HumanRepository.Instance.UpdateData(Hum);
                    close?.Invoke();
                }
                else { MessageBox.Show("Необходимо указать текущий пароль"); }
            });

            PickImage = new VmCommand(() => {
                var ofd = new OpenFileDialog();
                ofd.Filter = "Изображения|*.jpg;*.png";
                if (ofd.ShowDialog() == true && ofd.FileName != null)
                {
                    Hum.Photo = File.ReadAllBytes(ofd.FileName);
                    Signal(nameof(Hum));
                }
            });
        }
        internal void SetClose(Action close)
        {
            this.close = close;
        }
    }
}
