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

        internal IEnumerable<Applicant> GetAllResume (string sql)
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
                        applicant.HumanId = reader.GetInt32("id_human");
                        applicant.XP = reader.GetInt32("XP");
                        applicant.Id_City = reader.GetInt32("id_city");
                        applicant.Salary = reader.GetDouble("Salary");
                        applicant.IsFullEmployment = reader.GetBoolean("IsFullEmployment");
                        applicant.IsFlexibleSchedule = reader.GetBoolean("IsFlexibleSchedule");
                        applicant.IdEducation = reader.GetInt32("id_Education");
                        applicant.Name = reader.GetString("Name");
                        applicant.Middle_Name = reader.GetString("Middle_name");
                        applicant.Surname = reader.GetString("Surname");
                        applicant.Email = reader.GetString("EMAIL");
                        applicant.PhoneNumber = reader.GetString("PHONE_NUMBER");
                        applicant.Birthday = reader.GetDateTime("Birthday");
                    }
                    applicant.FieldofActivity.Add(new Field_of_Activity
                    {
                        Id = reader.GetInt32("id"),
                        Title = reader.GetString("foaTitle"),
                    });
                }
            }
            return result;
        }

        internal void AddApplicant(Applicant applicant)
        {
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return;

            int id = MySqlDB.Instance.GetAutoID("Applicant");

            string sql = "INSERT INTO Applicant VALUES (0, @h_id, @xp, @id_foa, @id_education, @id_city, " +
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
                if (mc.ExecuteNonQuery() > 0)
                {
                    sql = "";
                    foreach (var field in applicant.FieldofActivity)
                        sql += "INSERT INTO `Cross_Vacancy_Field-of-activity` VALUES (" + id + "," + field.Id + ");";
                    using (var mcCross = new MySqlCommand(sql, connect))
                        mcCross.ExecuteNonQuery();
                }
            }
        }

        internal void Remove(Applicant applicant)
        {
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return;

            string sql = "DELETE FROM `Cross_Applicant_Field-of-activity` WHERE id_applicant = '" + applicant.Id + "';";
            sql += "DELETE FROM Applicant WHERE ID = '" + applicant.Id + "';";

            using (var mc = new MySqlCommand(sql, connect))
                mc.ExecuteNonQuery();
        }

        internal IEnumerable<Applicant> Search(string searchText)
        {
            string sql = "SELECT a.ID, a.id_human, a.XP, a.id_Education, a.id_city, a.Salary , a.IsFullEmployment , a.IsFlexibleSchedule , foa.ID AS foaId, foa.Field_name AS foaTitle FROM CURSE.`Cross_Applicant_Field-of-activity` cafoa, CURSE.Applicant a, CURSE.`Fields-of-activity` foa WHERE cafoa.id_applicant = a.ID AND cafoa.id_field = foa.ID;";
            return GetAllResume(sql);
        }

        internal void UpdateResume(Applicant applicant)
        {
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return;

            string sql = "DELETE FROM `Cross_Applicant_Field-of-activity` WHERE id_applicant = '" + applicant.Id + "';";
            using (var mc = new MySqlCommand(sql, connect))
                mc.ExecuteNonQuery();

            sql = "";
            foreach (var foa in applicant.FieldofActivity)
                sql += "INSERT INTO `Cross_Applicant_Field-of-activity` VALUES (" + applicant.Id + "," + foa.Id + ");";
            using (var mcCross = new MySqlCommand(sql, connect))
                mcCross.ExecuteNonQuery();

            sql = "UPDATE Drink SET Title = @title, Capacity = @capacity, Price = @price, Description = @description WHERE Id = " + applicant.Id;
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
                mc.ExecuteNonQuery();
            }
        }
    }
}

