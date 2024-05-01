using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
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

        internal IEnumerable<Education> GetEducation(string sql)
        {
            var result = new List<Education>();
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return result;
            using (var mc = new MySqlCommand(sql, connect))
            using (var reader = mc.ExecuteReader())
            {
                Education ed = new Education();
                int id;
                while (reader.Read())
                {
                    id = reader.GetInt32("id");
                    if (ed.Id != id)
                    {
                        ed = new Education();
                        result.Add(ed);
                        ed.Id = id;
                        ed.Title = reader.GetString("Title");
                    }
                }
            }
            return result;
        }
    }
}
