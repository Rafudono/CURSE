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
    public class VacancyRepository
    {
        private VacancyRepository()
        {
        }

        static VacancyRepository instance;
        public static VacancyRepository Instance
        {
            get
            {
                if (instance == null)
                    instance = new VacancyRepository();
                return instance;
            }
        }

        internal IEnumerable<Vacancy> GetAllVacancy(string sql)
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
                        vacancy.XP= reader.GetInt32("Required-XP");
                        vacancy.Id_City = reader.GetInt32("Id_City");
                        vacancy.MINSalary = reader.GetInt32("MINSalary");
                        vacancy.MAXSalary = reader.GetInt32("MAXSalary");
                        vacancy.Responsibilities = reader.GetString("Responsibilities");
                        vacancy.Requirements = reader.GetString("Requirements");
                        vacancy.Conditions = reader.GetString("Conditions");
                        vacancy.Description = reader.GetString("Description");
                        vacancy.IsFlexibleSchedule = reader.GetBoolean("IsFlexibleSchedule");
                        vacancy.IdRequiredEducation = reader.GetInt32("id_Required-Education");
                    }
                    Field_of_Activity foa = new Field_of_Activity
                    {
                        Id = reader.GetInt32("id"),
                        Title = reader.GetString("foaTitle"),
                    };
                    vacancy.FieldofActivity.Add(foa);
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

            string sql = "INSERT INTO Vacancy VALUES (0, @empl_id, @xp, @id_foa, @id_reqeducation, @id_city, " +
                "@minsalary, @maxsalary, @resp, @require, @cond, @descr, @isflex_schedule, @title)";
            using (var mc = new MySqlCommand(sql, connect))
            {
                mc.Parameters.Add(new MySqlParameter("empl_id", vacancy.EmployerId));
                mc.Parameters.Add(new MySqlParameter("xp", vacancy.XP));
                mc.Parameters.Add(new MySqlParameter("id_reqeducation", vacancy.IdRequiredEducation));
                mc.Parameters.Add(new MySqlParameter("id_city", vacancy.Id_City));
                mc.Parameters.Add(new MySqlParameter("minsalary", vacancy.MINSalary));
                mc.Parameters.Add(new MySqlParameter("maxsalary", vacancy.MAXSalary));
                mc.Parameters.Add(new MySqlParameter("resp", vacancy.Responsibilities));
                mc.Parameters.Add(new MySqlParameter("require", vacancy.Requirements));
                mc.Parameters.Add(new MySqlParameter("cond", vacancy.Conditions));
                mc.Parameters.Add(new MySqlParameter("descr", vacancy.Description));
                mc.Parameters.Add(new MySqlParameter("isflex_schedule", vacancy.IsFlexibleSchedule));
                mc.Parameters.Add(new MySqlParameter("title", vacancy.Title));
                if (mc.ExecuteNonQuery() > 0)
                {
                    sql = "";
                    foreach (var field in vacancy.FieldofActivity)
                        sql += "INSERT INTO `Cross_Vacancy_Field-of-activity` VALUES (" + id + "," + field.Id + ");";
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

            string sql = "DELETE FROM Cross_Vacancy_Field-of-activity WHERE id_vac = '" + Vacancy.Id + "';";
            sql += "DELETE FROM Vacancy WHERE ID = '" + Vacancy.Id + "';";

            using (var mc = new MySqlCommand(sql, connect))
                mc.ExecuteNonQuery();
        }

        internal IEnumerable<Vacancy> Search(string searchText)//, Field_of_Activity field)
        {
            string sql = "SELECT v.ID, v.employer_id, v.`Required-XP`, v.`id_Required-Education`, v.id_city, v.MINSalary, v.MAXSalary, v.Responsibilities, v.Requirements, v.Conditions, v.Description, v.IsFlexibleSchedule, v.Title, foa.ID AS foaId, foa.Field_name AS foaTitle FROM `Cross_Vacancy_Field-of-activity` cvfoa, Vacancy v, `Fields-of-activity` foa WHERE cvfoa.id_vac = v.ID AND cvfoa.id_field = foa.ID ORDER by v.ID; ";
            //sql += " AND (d.Title LIKE '%" + searchText + "%'";
            //sql += " OR d.Description LIKE '%" + searchText + "%')";

            //if (selectedTag.ID != 0)
            //{
            //    var result = WatchAllVacancy(sql).Where(s => s.Tags.FirstOrDefault(s => s.ID == selectedTag.ID) != null);
            //    return result;                
            //}                                       сложный способ?
            return GetAllVacancy(sql);
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

            string sql = "DELETE FROM `Cross_Vacancy_Field-of-activity` WHERE id_vac = '" + vacancy.Id + "';";
            using (var mc = new MySqlCommand(sql, connect))
                mc.ExecuteNonQuery();

            sql = "";
            foreach (var foa in vacancy.FieldofActivity)
                sql += "INSERT INTO `Cross_Vacancy_Field-of-activity` VALUES (" + vacancy.Id + "," + foa.Id + ");";
            using (var mcCross = new MySqlCommand(sql, connect))
                mcCross.ExecuteNonQuery();

            sql = "UPDATE CURSE.Vacancy SET employer_id = @empl_id, `Required-XP` = @xp, `id_Required-Education` = @id_reqeducation, id_city = @id_city, MINSalary = @minsalary, MAXsalary = @maxsalary, Responsibilities = @resp, Requirements = @require, Conditions = @cond, Description = @descr, IsFlexibleSchedule = @isflex_schedule, Title = @title WHERE ID =" + vacancy.Id;
            using (var mc = new MySqlCommand(sql, connect))
            {
                mc.Parameters.Add(new MySqlParameter("empl_id", vacancy.EmployerId));
                mc.Parameters.Add(new MySqlParameter("xp", vacancy.XP));
                mc.Parameters.Add(new MySqlParameter("id_reqeducation", vacancy.IdRequiredEducation));
                mc.Parameters.Add(new MySqlParameter("id_city", vacancy.Id_City));
                mc.Parameters.Add(new MySqlParameter("minsalary", vacancy.MINSalary));
                mc.Parameters.Add(new MySqlParameter("maxsalary", vacancy.MAXSalary));
                mc.Parameters.Add(new MySqlParameter("resp", vacancy.Responsibilities));
                mc.Parameters.Add(new MySqlParameter("require", vacancy.Requirements));
                mc.Parameters.Add(new MySqlParameter("cond", vacancy.Conditions));
                mc.Parameters.Add(new MySqlParameter("descr", vacancy.Description));
                mc.Parameters.Add(new MySqlParameter("isflex_schedule", vacancy.IsFlexibleSchedule));
                mc.Parameters.Add(new MySqlParameter("title", vacancy.Title));
                mc.ExecuteNonQuery();
            }
        }
    }
}