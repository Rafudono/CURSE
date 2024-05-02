using CURSE.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CURSE.ViewModel
{
    public class LoginVM : BaseVM
    {
        private MainVM mainVM;
        public Human loginHuman { get; set; } = new();
        public string loginEmail {  get; set; }
        public string loginPassword { get; set; }
        public VmCommand loginCommand { get; set; }
        private ApplicantVM AVM { get; set; }

        public LoginVM()
        {
            loginCommand=new VmCommand(()=>
            {
                var hash= Md5.HashPassword(loginEmail);
               int id=HumanRepository.Instance.LoginHuman(loginEmail,hash);
                for (int i = 0; i < AVM.Applicants.Count; i++)
                {
                  
                        }
            });
        }
    }
}
