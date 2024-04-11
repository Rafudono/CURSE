using CURSE.Model;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CURSE.Model
{
    public class DrinkRepository
    {
        private DrinkRepository()
        {
        }

        static DrinkRepository instance;
        public static DrinkRepository Instance
        {
            get
            {
                if (instance == null)
                    instance = new DrinkRepository();
                return instance;
            }
        }

        internal IEnumerable<Vacancy> WatchAllVacancy(string sql)
        {
            var result = new List<Vacancy>();
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return result;
            using (var mc = new MySqlCommand(sql, connect))
            using (var reader = mc.ExecuteReader())
            {
                Vacancy vacancy = new Vacancy();
                int id;
                while (reader.Read())
                {
                    id = reader.GetInt32("id");
                    if (vacancy.Id != id)
                    {
                        vacancy = new Vacancy();
                        result.Add(vacancy);
                        vacancy.Id = id;
                        vacancy.Title = reader.GetString("Title");
                        vacancy.XP= reader.GetInt32("XP");
                        vacancy.City = reader.GetString("City");
                        vacancy.MINSalary = reader.GetInt32("MINSalary");
                        vacancy.MAXSalary = reader.GetInt32("MAXSalary");
                        vacancy.DescriptionResponsibilities = reader.GetString("DescriptionResp");
                        vacancy.DescriptionRequirements = reader.GetString("DescriptionRequire");
                        vacancy.DescriptionConditions = reader.GetString("DescriptionCond");
                        vacancy.IsFlexibleSchedule = reader.GetString("Schedule");
                        vacancy.RequiredEducation = reader.GetString("RequireEducation");
                    }
                  
                }
            }

            return result;
        }

        internal void AddVacancy(Vacancy vacancy)
        {
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return;

            int id = MySqlDB.Instance.GetAutoID("Vacancy");

            string sql = "INSERT INTO Drink VALUES (0, @title, @capacity, @price, @description)";
            using (var mc = new MySqlCommand(sql, connect))
            {
                mc.Parameters.Add(new MySqlParameter("title", vacancy.Title));
                mc.Parameters.Add(new MySqlParameter("capacity", vacancy.Capacity));
                mc.Parameters.Add(new MySqlParameter("price", vacancy.Price));
                mc.Parameters.Add(new MySqlParameter("description", vacancy.Description));
                if (mc.ExecuteNonQuery() > 0)
                {
                    sql = "";
                    foreach (var tag in drink.Tags)
                        sql += "INSERT INTO CrossDrinkTag VALUES (" + id + "," + tag.ID + ");";
                    using (var mcCross = new MySqlCommand(sql, connect))
                        mcCross.ExecuteNonQuery();
                }
            }
        }

        internal void Remove(Vacancy Vacancy)
        {
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return;

            string sql = "DELETE FROM CrossDrinkTag WHERE idDrink = '" + Vacancy.Id + "';";
            sql += "DELETE FROM Drink WHERE id = '" + Vacancy.Id + "';";

            using (var mc = new MySqlCommand(sql, connect))
                mc.ExecuteNonQuery();
        }

        internal IEnumerable<Vacancy> Search(string searchText, Tag selectedTag)
        {
            string sql = "SELECT d.id, d.Title, d.Capacity, d.Price, d.Description, tt.id AS tagId, tt.Title AS tagTitle FROM CrossDrinkTag cdt, Drink d, TagsTable tt WHERE cdt.idDrink = d.id AND cdt.idTag = tt.id";
            sql += " AND (d.Title LIKE '%" + searchText + "%'";
            sql += " OR d.Description LIKE '%" + searchText + "%')";

            if (selectedTag.ID != 0)
            {
                var result = WatchAllVacancy(sql).Where(s => s.Tags.FirstOrDefault(s => s.ID == selectedTag.ID) != null);
                return result;
            }
            return WatchAllVacancy(sql);
            //return drinks.Where(s => 
            //    s.Title.Contains(searchText) ||
            //    s.Description.Contains(searchText));
            //else
            //    return drinks.Where(s =>
            //    (s.Title.Contains(searchText) ||
            //    s.Description.Contains(searchText)) &&
            //    s.Tags.Contains(selectedTag));
        }

        internal void UpdateDrink(Vacancy vacancy)
        {
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return;

            string sql = "DELETE FROM CrossDrinkTag WHERE idDrink = '" + vacancy.Id + "';";
            using (var mc = new MySqlCommand(sql, connect))
                mc.ExecuteNonQuery();

            sql = "";
            foreach (var tag in vacancy.Tags)
                sql += "INSERT INTO CrossDrinkTag VALUES (" + vacancy.Id + "," + tag.ID + ");";
            using (var mcCross = new MySqlCommand(sql, connect))
                mcCross.ExecuteNonQuery();

            sql = "UPDATE Drink SET Title = @title, Capacity = @capacity, Price = @price, Description = @description WHERE Id = " + drink.ID;
            using (var mc = new MySqlCommand(sql, connect))
            {
                mc.Parameters.Add(new MySqlParameter("title", vacancy.Title));
                mc.Parameters.Add(new MySqlParameter("capacity", vacancy.Capacity));
                mc.Parameters.Add(new MySqlParameter("price", vacancy.XP));
                mc.Parameters.Add(new MySqlParameter("description", vacancy.Description));
                mc.ExecuteNonQuery();
            }
        }
    }
}