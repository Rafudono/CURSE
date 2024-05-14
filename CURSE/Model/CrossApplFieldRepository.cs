using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CURSE.Model
{
    public class CrossApplFieldRepository
    {
        private CrossApplFieldRepository()
        {
            
        }
        static CrossApplFieldRepository instance;
        public static CrossApplFieldRepository Instance
        {
            get
            {
                if (instance == null)
                    instance = new CrossApplFieldRepository();
                return instance;
            }
        }
        internal List<CrossApplField> GetCross(string sql)
        {
            List<CrossApplField> result = new List<CrossApplField>();
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return result;

            using (var mc = new MySqlCommand(sql, connect))
            using (var reader = mc.ExecuteReader())
            {
                while (reader.Read())
                {
                    var cross = new CrossApplField
                    {
                        id_applicant = reader.GetInt32("id_applicant"),
                        id_field = reader.GetInt32("id_field")
                    };
                    result.Add(cross);
                }
            }
            return result;
        }
    }
}
