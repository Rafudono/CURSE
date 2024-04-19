using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CURSE.Model
{
    internal class FoaRepository
    {
        private FoaRepository()
        {

        }

           static FoaRepository instance;
        public static FoaRepository Instance
        {
            get
            {
                if (instance == null)
                    instance = new FoaRepository();
                return instance;
            }
        }

        internal List<Field_of_Activity> GetFields()
        {
            List<Field_of_Activity> result = new List<Field_of_Activity>();
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return result;

            string sql = "SELECT * FROM `Fields-of-activity`";
            using (var mc = new MySqlCommand(sql, connect))
            using (var reader = mc.ExecuteReader())
            {
                while (reader.Read())
                {
                    var field = new Field_of_Activity
                    {
                        Id = reader.GetInt32("ID"),
                        Title = reader.GetString("Field_name")
                    };
                    result.Add(field);
                }
            }
            return result;
        }
    }
}
