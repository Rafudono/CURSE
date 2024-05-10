using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CURSE.Model
{
    public class CityRepository
    {

        private CityRepository()
        {
        }

        static CityRepository instance;
        public static CityRepository Instance
        {
            get
            {
                if (instance == null)
                    instance = new CityRepository();
                return instance;
            }
        }

        internal IEnumerable<City> GetAllCity()
        {
            var result = new List<City>();
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return result;
            string sql = "SELECT * FROM City;";
            using (var mc = new MySqlCommand(sql, connect))
            using (var reader = mc.ExecuteReader())
            {
                int id;
                while (reader.Read())
                {

                    City city = new City();
                    {
                        city.Id = reader.GetInt32("ID");
                        city.City_Name = reader.GetString("City_Name");
                    }
                    result.Add(city);
                }
            }
            return result;
        }
    }
    
}

