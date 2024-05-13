using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CURSE.Model
{
    public class ApplAuthorized
    {
        public Applicant loginAppl { get; set; } = new();
        public ApplAuthorized() { }
        static ApplAuthorized instance;
        public static ApplAuthorized Instance
        {
            get
            {
                if (instance == null)
                    instance = new ApplAuthorized();
                return instance;
            }
        }
    }
}
