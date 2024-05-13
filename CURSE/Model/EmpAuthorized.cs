using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CURSE.Model
{
    public class EmpAuthorized
    {
        public Employer loginEmp { get; set; } = new();
        public EmpAuthorized() { }
        static EmpAuthorized instance;
        public static EmpAuthorized Instance
        {
            get
            {
                if (instance == null)
                    instance = new EmpAuthorized();
                return instance;
            }
        }
    }
}

