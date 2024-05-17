using CURSE.Model;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CURSE.ViewModel
{        //открывается по кнопочке "добавить организацию-партнера"
         //имя организации и текстбокс -> OrgRepo.Inctanse.NewOrg()
    public class NewOrg:BaseVM
    {
        public VmCommand Save;
        public string OrgName { get; set; }
        public Organization Org;
        public NewOrg()
        {
            Save = new VmCommand(() =>
            {
                if (OrgName != null)
                {
                    Org = new Organization();
                    Org.Org_Name = OrgName;
                    OrgRepository.Instance.NewOrg(Org);
                }
            });
        }

    }
}
