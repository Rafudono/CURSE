using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CURSE.Model
{
    public class ApplicantRepository
    {
        private ApplicantRepository()
        {
        }

        static ApplicantRepository instance;
        public static ApplicantRepository Instance
        {
            get
            {
                if (instance == null)
                    instance = new ApplicantRepository();
                return instance;
            }
        }

        internal IEnumerable<Applicant> WatchAllResume (string sql)
        {
            var result = new List<Applicant>();
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return result;
            using (var mc = new MySqlCommand(sql, connect))
            using (var reader = mc.ExecuteReader())
            {
                Applicant applicant = new Applicant();
                int id;
                while (reader.Read())
                {
                    id = reader.GetInt32("id");
                    if (applicant.Id != id)
                    {
                        applicant = new Applicant();
                        result.Add(applicant);
                        applicant.Id = id;
                        applicant.HumanId = reader.GetInt32("HumanId");
                        applicant.XP = reader.GetInt32("XP");
                        applicant.Id_City = reader.GetInt32("Id_City");
                        applicant.Salary = reader.GetInt32("Salary");
                        applicant.IsFullEmployment = reader.GetBoolean("IsFullEmployment");
                        applicant.IsFlexibleSchedule = reader.GetBoolean("IsFlexibleSchedule");
                        applicant.IdEducation = reader.GetInt32("IdEducation");
                    }
                    applicant.FieldofActivity.Add(new Field_of_Activity
                    {
                        Id = reader.GetInt32("foaId"),
                        Title = reader.GetString("foaTitle"),
                    });
                }
            }
            return result;
        }

        internal void AddVacancy(Applicant applicant)
        {
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return;

            int id = MySqlDB.Instance.GetAutoID("Vacancy");

            string sql = "INSERT INTO Applicant VALUES (0, @xp, @id_foa, @id_education, @id_city, " +
                "@salary, @isfull-employment, @isflex-schedule)";
            using (var mc = new MySqlCommand(sql, connect))
            {
                mc.Parameters.Add(new MySqlParameter("id", applicant.Id));
                mc.Parameters.Add(new MySqlParameter("h_id", applicant.HumanId));
                mc.Parameters.Add(new MySqlParameter("xp", applicant.XP));
                mc.Parameters.Add(new MySqlParameter("id_foa", applicant.FieldofActivity));
                mc.Parameters.Add(new MySqlParameter("id_education", applicant.IdEducation));
                mc.Parameters.Add(new MySqlParameter("id_city", applicant.Id_City));
                mc.Parameters.Add(new MySqlParameter("salary", applicant.Salary));
                mc.Parameters.Add(new MySqlParameter("isfull-employment", applicant.IsFullEmployment));
                mc.Parameters.Add(new MySqlParameter("isflex-schedule", applicant.IsFlexibleSchedule));
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

        internal void Remove(Applicant applicant)
        {
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return;

            string sql = "DELETE FROM Cross_Applicant_Field-of-activity WHERE id_applicant = '" + applicant.Id + "';";
            sql += "DELETE FROM Applicant WHERE ID = '" + applicant.Id + "';";

            using (var mc = new MySqlCommand(sql, connect))
                mc.ExecuteNonQuery();
        }

        internal IEnumerable<Applicant> Search(string searchText)
        {
            string sql = "SELECT a.ID, a.id_human, a.XP, a.id_Education, a.id_city, a.Salary , a.IsFullEmployment , a.IsFlexibleSchedule , foa.ID AS foaId, foa.Field_name AS foaTitle FROM CURSE.`Cross_Applicant_Field-of-activity` cafoa, CURSE.Applicant a, CURSE.`Fields-of-activity` foa WHERE cafoa.id_applicant = a.ID AND cafoa.id_field = foa.ID;";
            //sql += " AND (d.Title LIKE '%" + searchText + "%'";
            //sql += " OR d.Description LIKE '%" + searchText + "%')";

            //if (selectedTag.ID != 0)
            //{
            //    var result = WatchAllVacancy(sql).Where(s => s.Tags.FirstOrDefault(s => s.ID == selectedTag.ID) != null);
            //    return result;                
            //}                                       сложный способ?
            return WatchAllResume (sql);
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

