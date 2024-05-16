using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace CURSE.Model
{
    public class EducationRepository
    {
        private EducationRepository()
        {
        }

        static EducationRepository instance;
        public static EducationRepository Instance
        {
            get
            {
                if (instance == null)
                    instance = new EducationRepository();
                return instance;
            }
        }

        internal IEnumerable<Education> GetEducation()
        {
            var result = new List<Education>();
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return result;
            string sql= "SELECT * FROM Education";
            using (var mc = new MySqlCommand(sql, connect))
            using (var reader = mc.ExecuteReader())
            {
                int id;
                while (reader.Read())
                {

                    Education ed = new Education();
                    { 
                        ed.Id = reader.GetInt32("ID");
                        ed.Title = reader.GetString("Title");
                    }
                    result.Add(ed);
                }
            }
            return result;
        }
    }
}
