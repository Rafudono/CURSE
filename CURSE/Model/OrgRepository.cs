using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CURSE.Model
{
    public class OrgRepository
    {
        private OrgRepository()
        {
        }

        static OrgRepository instance;
        public static OrgRepository Instance
        {
            get
            {
                if (instance == null)
                    instance = new OrgRepository();
                return instance;
            }
        }

        internal IEnumerable<Organization> GetOrg(string sql)
        {
            var result = new List<Organization>();
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return result;
            using (var mc = new MySqlCommand(sql, connect))
            using (var reader = mc.ExecuteReader())
            {
                Organization org = new Organization();
                int id;
                while (reader.Read())
                {
                    id = reader.GetInt32("id");
                    if (org.Id != id)
                    {
                        org = new Organization();
                        result.Add(org);
                        org.Id = id;
                        org.Org_Name = reader.GetString("Org_Name");
                    }
                }
            }
            return result;
        }
        internal int NewOrg(Organization org)
        {
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return 0;

            int id = MySqlDB.Instance.GetAutoID("Organization"); 

            string sql = "INSERT INTO Organization VALUES (0, @Org_Name)";
            using (var mc = new MySqlCommand(sql, connect))
            {
                mc.Parameters.Add(new MySqlParameter("id", org.Id));
                mc.Parameters.Add(new MySqlParameter("Org_Name", org.Org_Name));
                mc.ExecuteNonQuery();
            }
            return id;
        }
    }
}

