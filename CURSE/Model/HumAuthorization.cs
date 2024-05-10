using CURSE.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CURSE.Model
{
   public class HumAuthorization
    {
        public Human loginHuman { get; set; } = new();
        public HumAuthorization() { }
        static HumAuthorization instance;
        public static HumAuthorization Instance
        {
            get
            {
                if (instance == null)
                    instance = new HumAuthorization();
                return instance;
            }
        }
    }
}
