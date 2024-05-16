using CURSE.View;
using CURSE.ViewModel;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
                    if (applicant.ID != id)
                    {
                        applicant = new Applicant();
                        result.Add(applicant);
                        applicant.ID = id;
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
                        int index = reader.GetOrdinal("Photo");
                        using (var stream = reader.GetStream(index))
                        {
                            applicant.Photo = new byte[stream.Length];
                            stream.Read(applicant.Photo, 0, (int)stream.Length);
                        }
                        applicant.Description = reader.GetString("Description");
                        applicant.RTitle = reader.GetString("ResumeTitle");
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

            string sql = "INSERT INTO Applicant VALUES (0, @h_id, @xp, @id_education, @id_city, " +
                "@salary, @isfullEmployment, @isflexSchedule, @descr, @rtitle)";
            using (var mc = new MySqlCommand(sql, connect))
            {
                mc.Parameters.Add(new MySqlParameter("id", applicant.ID));
                mc.Parameters.Add(new MySqlParameter("h_id", applicant.HumanId));
                mc.Parameters.Add(new MySqlParameter("xp", applicant.XP));
                mc.Parameters.Add(new MySqlParameter("id_education", applicant.IdEducation));
                mc.Parameters.Add(new MySqlParameter("id_city", applicant.Id_City));
                mc.Parameters.Add(new MySqlParameter("salary", applicant.Salary));
                mc.Parameters.Add(new MySqlParameter("isfullEmployment", applicant.IsFullEmployment));
                mc.Parameters.Add(new MySqlParameter("isflexSchedule", applicant.IsFlexibleSchedule));
                mc.Parameters.Add(new MySqlParameter("descr", applicant.Description));
                mc.Parameters.Add(new MySqlParameter("rtitle", applicant.RTitle));
                if (mc.ExecuteNonQuery() > 0)
                {
                    sql = "";
                    if (applicant.FieldofActivity.Count > 0)
                    {
                        foreach (var field in applicant.FieldofActivity)
                            sql += "INSERT INTO `Cross_Vacancy_Field-of-activity` VALUES (" + id + "," + field.Id + ");";
                        using (var mcCross = new MySqlCommand(sql, connect))
                            mcCross.ExecuteNonQuery();
                    }
                }

            }
        }

        internal Applicant LoginApplicant(int id_hum)
        {

            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                MessageBox.Show("нет соединения с бд");

            Applicant applicant = new Applicant();
            string sql = "SELECT * FROM CURSE.Applicant a Where (a.id_human = " + id_hum + ");";
            using (var mc = new MySqlCommand(sql, connect))
            using (var reader = mc.ExecuteReader())
            {
                int id;
                while (reader.Read())
                {
                    id = reader.GetInt32("id");
                    if (applicant.ID != id)
                    {
                        applicant.ID = id;
                        applicant.HumanId = reader.GetInt32("id_human");
                        applicant.XP = reader.GetInt32("XP");
                        applicant.Id_City = reader.GetInt32("id_city");
                        applicant.Salary = reader.GetDouble("Salary");
                        applicant.IsFullEmployment = reader.GetBoolean("IsFullEmployment");
                        applicant.IsFlexibleSchedule = reader.GetBoolean("IsFlexibleSchedule");
                        applicant.IdEducation = reader.GetInt32("id_Education");
                        applicant.Description = reader.GetString("Description");
                        applicant.RTitle = reader.GetString("ResumeTitle");
                    }
                }
            }
                    string cross = "SELECT * from CURSE.`Cross_Applicant_Field-of-activity` cafoa Where id_applicant = "+applicant.ID +";";
                    var listcross =CrossApplFieldRepository.Instance.GetCross(cross);
                if (listcross != null && listcross.Count != 0)
                {
                    applicant = new Applicant();
                    sql = "";
                    sql = "SELECT a.ID, a.id_human, a.XP, a.id_Education, a.id_city, a.Salary , a.IsFullEmployment , a.IsFlexibleSchedule,  a.Description, a.ResumeTitle, foa.ID AS foaId, foa.Field_name AS foaTitle, h.Name, h.Middle_name , h.Surname, h.EMAIL, h.PHONE_NUMBER, h.Birthday, e.Title FROM CURSE.`Cross_Applicant_Field-of-activity` cafoa, CURSE.Applicant a, Curse.Human h, CURSE.`Fields-of-activity` foa, CURSE.Education e WHERE cafoa.id_applicant = a.ID AND cafoa.id_field = foa.ID and a.id_human = "+id_hum+" and a.id_Education =e.ID;";
                    using (var mc = new MySqlCommand(sql, connect))
                    using (var reader = mc.ExecuteReader())
                    {
                    int id;
                    while (reader.Read())
                        {
                            id = reader.GetInt32("id");
                            if (applicant.ID != id)
                            {
                                applicant.ID = id;
                                applicant.HumanId = reader.GetInt32("id_human");
                                applicant.XP = reader.GetInt32("XP");
                                applicant.Id_City = reader.GetInt32("id_city");
                                applicant.Salary = reader.GetDouble("Salary");
                                applicant.IsFullEmployment = reader.GetBoolean("IsFullEmployment");
                                applicant.IsFlexibleSchedule = reader.GetBoolean("IsFlexibleSchedule");
                                applicant.IdEducation = reader.GetInt32("id_Education");
                                applicant.Description = reader.GetString("Description");
                                applicant.RTitle = reader.GetString("ResumeTitle");
                            }
                            applicant.FieldofActivity.Add(new Field_of_Activity
                            {
                                Id = reader.GetInt32("id"),
                                Title = reader.GetString("foaTitle"),
                            });
                        }
                    }
                
            }
           
            return applicant;
            
        }
           internal void Delete(Applicant applicant)
        {
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return;

            string sql = "DELETE FROM `Cross_Applicant_Field-of-activity` WHERE id_applicant = '" + applicant.ID + "';";
            sql += "DELETE FROM Applicant WHERE ID = '" + applicant.ID + "';";

            using (var mc = new MySqlCommand(sql, connect))
                mc.ExecuteNonQuery();
        }

        internal IEnumerable<Applicant> Search(string searchText, int minSalary, int xp, bool isflex, bool isfull, Field_of_Activity selectedField, Education selectedEd, City selectedCity)
        {
            string sql = "SELECT a.ID, a.id_human, a.XP, a.id_Education, a.id_city, a.Salary , a.IsFullEmployment , a.IsFlexibleSchedule, a.Description, a.ResumeTitle, foa.ID AS foaId, foa.Field_name AS foaTitle, h.Name, h.Middle_name , h.Surname, h.EMAIL, h.PHONE_NUMBER, h.Birthday, h.Photo FROM CURSE.`Cross_Applicant_Field-of-activity` cafoa, CURSE.Applicant a, Curse.Human h , CURSE.`Fields-of-activity` foa, CURSE.Education e WHERE cafoa.id_applicant = a.ID AND cafoa.id_field = foa.ID and a.id_human = h.ID AND a.id_Education=e.ID";
            sql += " AND (a.ResumeTitle LIKE '%" + searchText + "%')";
            //if (selectedField.Id != 0)
            //{
            //    var result = GetAllResume(sql).Where(s => s.FieldofActivity.FirstOrDefault(s => s.Id == selectedField.Id) != null);
            //    return result;
            //}
            sql += " AND (a.Salary <=" + minSalary + ")";
            sql += " AND (a.XP <=" + xp + ")";
            if(!isflex)
            sql += " AND (a.IsFlexibleSchedule =" + isflex + ")";
            if(isfull)
            sql += " AND(a.IsFullEmployment=" + isfull + ")";
            if (selectedField != null && selectedField.Id != 0)
            {
                var list = GetAllResume(sql).Where(s => s.FieldofActivity.FirstOrDefault(s => s.Title == selectedField.Title) != null);
                return list;
            }
            if (selectedField != null && selectedField.Id != 0 && selectedCity != null && selectedCity.Id != 0 && selectedEd != null && selectedEd.Id != 0)
                sql += "AND foa.ID =" + selectedField.Id + " AND a.id_city =" + selectedCity.Id + " and a.id_Education=" + selectedEd.Id + "  ORDER by a.ID;";

            else if (selectedField != null && selectedField.Id != 0 && selectedCity != null && selectedCity.Id != 0)
                sql += " AND foa.ID =" + selectedField.Id + " AND a.id_city =" + selectedCity.Id + " ORDER by a.ID;";
            else if (selectedField != null && selectedField.Id != 0 && selectedEd != null && selectedEd.Id != 0)
                sql += " AND foa.ID =" + selectedField.Id + " AND  a.id_Education =" + selectedEd.Id + " ORDER by a.ID;";
            else if (selectedCity != null && selectedCity.Id != 0 && selectedEd != null && selectedEd.Id != 0)
                sql += " AND a.id_city =" + selectedCity.Id + " and a.id_Education=" + selectedEd.Id + " ORDER by a.ID;";

            else if (selectedCity != null && selectedCity.Id != 0)
                sql += " AND a.id_city =" + selectedCity.Id + " ORDER by a.ID;";
            else if (selectedField != null && selectedField.Id != 0)
                sql += " AND foa.ID =" + selectedField.Id +  " ORDER by a.ID;";
            else if (selectedEd != null && selectedEd.Id != 0)
                sql += " and a.id_Education =" + selectedEd.Id + " ORDER by a.ID;";

            else
                sql += " ORDER by a.ID;";
            return GetAllResume(sql);
        }

        internal void UpdateResume(Applicant applicant)
        {
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return;

            string sql = "DELETE FROM `Cross_Applicant_Field-of-activity` WHERE id_applicant = '" + applicant.ID + "';";
            using (var mc = new MySqlCommand(sql, connect))
                mc.ExecuteNonQuery();

            sql = "";
            foreach (var foa in applicant.FieldofActivity)
                sql += "INSERT INTO `Cross_Applicant_Field-of-activity` VALUES (" + applicant.ID + "," + foa.Id + ");";
            using (var mcCross = new MySqlCommand(sql, connect))
                mcCross.ExecuteNonQuery();

            sql = "UPDATE Applicant SET id_human = @h_id, XP = @xp, id_education = @id_education, id_city=@id_city, Salary = @salary, IsFullEmployment=@isfullEmployment, IsFlexibleSchedule=@isflexSchedule, Description =@descr, ResumeTitle=@RTitle WHERE Id = " + applicant.ID;
            using (var mc = new MySqlCommand(sql, connect))
            {
                mc.Parameters.Add(new MySqlParameter("id", applicant.ID));
                mc.Parameters.Add(new MySqlParameter("h_id", applicant.HumanId));
                mc.Parameters.Add(new MySqlParameter("xp", applicant.XP));
                mc.Parameters.Add(new MySqlParameter("id_education", applicant.IdEducation));
                mc.Parameters.Add(new MySqlParameter("id_city", applicant.Id_City));
                mc.Parameters.Add(new MySqlParameter("salary", applicant.Salary));
                mc.Parameters.Add(new MySqlParameter("isfullEmployment", applicant.IsFullEmployment));
                mc.Parameters.Add(new MySqlParameter("isflexSchedule", applicant.IsFlexibleSchedule));
                mc.Parameters.Add(new MySqlParameter("descr", applicant.Description));
                mc.Parameters.Add(new MySqlParameter("RTitle", applicant.RTitle));
                mc.ExecuteNonQuery();
            }
        }
    }
}

