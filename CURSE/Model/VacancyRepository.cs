using CURSE.Model;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Intrinsics.X86;
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
                        vacancy.Id_City = reader.GetInt32("Id_City");
                        vacancy.MINSalary = reader.GetInt32("MINSalary");
                        vacancy.MAXSalary = reader.GetInt32("MAXSalary");
                        vacancy.DescriptionResponsibilities = reader.GetString("DescriptionResp");
                        vacancy.DescriptionRequirements = reader.GetString("DescriptionRequire");
                        vacancy.DescriptionConditions = reader.GetString("DescriptionCond");
                        vacancy.IsFlexibleSchedule = reader.GetString("IsFlexibleSchedule");
                        vacancy.IdRequiredEducation = reader.GetInt32("IDRequireEducation");
                    }
                    vacancy.FieldofActivity.Add(new Field_of_Activity {
                     Id = reader.GetInt32("foaId"),
                     Title = reader.GetString("foaTitle"),
                });

                  
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

            string sql = "INSERT INTO Vacancy VALUES (0, @empl_id, @xp, @id_foa, @id_req-education, @id_city, " +
                "@minsalary, @maxsalary, @descr-resp, @descr-require, @descr-cond, @isflex-schedule, @title)";
            using (var mc = new MySqlCommand(sql, connect))
            {
                mc.Parameters.Add(new MySqlParameter("empl_id", vacancy.EmployerId));
                mc.Parameters.Add(new MySqlParameter("xp", vacancy.XP));
                mc.Parameters.Add(new MySqlParameter("id_foa", vacancy.FieldofActivity));
                mc.Parameters.Add(new MySqlParameter("id_req-education", vacancy.IdRequiredEducation));
                mc.Parameters.Add(new MySqlParameter("id_city", vacancy.Id_City));
                mc.Parameters.Add(new MySqlParameter("minsalary", vacancy.MINSalary));
                mc.Parameters.Add(new MySqlParameter("maxsalary", vacancy.MAXSalary));
                mc.Parameters.Add(new MySqlParameter("descr-resp", vacancy.DescriptionResponsibilities));
                mc.Parameters.Add(new MySqlParameter("descr-require", vacancy.DescriptionRequirements));
                mc.Parameters.Add(new MySqlParameter("descr-cond", vacancy.DescriptionConditions));
                mc.Parameters.Add(new MySqlParameter("isflex-schedule", vacancy.IsFlexibleSchedule));
                mc.Parameters.Add(new MySqlParameter("title", vacancy.Title));
                //if (mc.ExecuteNonQuery() > 0)
                //{
                //    sql = "";
                //    foreach (var tag in drink.Tags)
                //        sql += "INSERT INTO CrossDrinkTag VALUES (" + id + "," + tag.ID + ");";
                //    using (var mcCross = new MySqlCommand(sql, connect))
                //        mcCross.ExecuteNonQuery();
                //}            сложный способ?
            }
        }

        internal void Remove(Vacancy Vacancy)
        {
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return;

            string sql = "DELETE FROM Cross_Vacancy_Field-of-activity WHERE id_vac = '" + Vacancy.Id + "';";
            sql += "DELETE FROM Vacancy WHERE ID = '" + Vacancy.Id + "';";

            using (var mc = new MySqlCommand(sql, connect))
                mc.ExecuteNonQuery();
        }

        internal IEnumerable<Vacancy> Search(string searchText)
        {
            string sql = "SELECT v.ID, v.employer_id, v.`Required-XP`, v.`id_Required-Education`, v.id_city, v.MINSalary, v.MAXSalary, v.DescriptionResponsibilities, v.DescriptionRequirements, v.DescriptionConditions, v.IsFlexibleSchedule, v.Title, foa.ID AS foaId, foa.Field_name AS foaTitle FROM `Cross_Vacancy_Field-of-activity` cvfoa, Vacancy v, `Fields-of-activity` foa WHERE cvfoa.id_vac = v.ID AND cvfoa.id_field = foa.ID";
            //sql += " AND (d.Title LIKE '%" + searchText + "%'";
            //sql += " OR d.Description LIKE '%" + searchText + "%')";

            //if (selectedTag.ID != 0)
            //{
            //    var result = WatchAllVacancy(sql).Where(s => s.Tags.FirstOrDefault(s => s.ID == selectedTag.ID) != null);
            //    return result;                
            //}                                       сложный способ?
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

        internal void UpdateVacancy(Vacancy vacancy)
        {
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return;

            string sql = "DELETE FROM CrossDrinkTag WHERE idDrink = '" + vacancy.Id + "';";
            using (var mc = new MySqlCommand(sql, connect))
                mc.ExecuteNonQuery();

            sql = "";
            foreach (var foa in vacancy.FieldofActivity)
                sql += "INSERT INTO CrossDrinkTag VALUES (" + vacancy.Id + "," + foa.Id + ");";
            using (var mcCross = new MySqlCommand(sql, connect))
                mcCross.ExecuteNonQuery();

            sql = "UPDATE Drink SET Title = @title, Capacity = @capacity, Price = @price, Description = @description WHERE Id = " + vacancy.Id;
            using (var mc = new MySqlCommand(sql, connect))
            {
                mc.Parameters.Add(new MySqlParameter("empl_id", vacancy.EmployerId));
                mc.Parameters.Add(new MySqlParameter("xp", vacancy.XP));
                mc.Parameters.Add(new MySqlParameter("id_foa", vacancy.FieldofActivity));
                mc.Parameters.Add(new MySqlParameter("id_req-education", vacancy.IdRequiredEducation));
                mc.Parameters.Add(new MySqlParameter("id_city", vacancy.Id_City));
                mc.Parameters.Add(new MySqlParameter("minsalary", vacancy.MINSalary));
                mc.Parameters.Add(new MySqlParameter("maxsalary", vacancy.MAXSalary));
                mc.Parameters.Add(new MySqlParameter("descr-resp", vacancy.DescriptionResponsibilities));
                mc.Parameters.Add(new MySqlParameter("descr-require", vacancy.DescriptionRequirements));
                mc.Parameters.Add(new MySqlParameter("descr-cond", vacancy.DescriptionConditions));
                mc.Parameters.Add(new MySqlParameter("isflex-schedule", vacancy.IsFlexibleSchedule));
                mc.Parameters.Add(new MySqlParameter("title", vacancy.Title));
                mc.ExecuteNonQuery();
            }
        }
    }
}