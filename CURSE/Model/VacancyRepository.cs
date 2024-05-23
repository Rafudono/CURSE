using CURSE.Model;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
            

            string sql = "INSERT INTO Vacancy VALUES (0, @empl_id, @xp, @id_reqeducation, @id_city, " +
                "@minsalary, @maxsalary, @resp, @require, @cond, @descr, @isflex_schedule, @title, @isfullempl)";
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
                mc.Parameters.Add(new MySqlParameter("isfullempl", vacancy.IsFullEmployment));
                if (mc.ExecuteNonQuery() > 0)
                {
                    sql = "";
                    if (vacancy.FieldofActivity.Count > 0)
                    {
                        int trueid = GetTrueId(vacancy);
                        foreach (var field in vacancy.FieldofActivity)
                            sql += "INSERT INTO `Cross_Vacancy_Field-of-activity` VALUES (" + trueid + "," + field.Id + ");";
                        using (var mcCross = new MySqlCommand(sql, connect))
                            mcCross.ExecuteNonQuery();
                    }
                }
            }
        }
        internal int GetTrueId(Vacancy vac)
        {
            List<Vacancy> list = new List<Vacancy>();
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                MessageBox.Show("нет подключения к БД");
            string sql = "SELECT v.ID  from CURSE.Vacancy v where v.employer_id ='" + vac.EmployerId+"' and v.`Required-XP` = '"+vac.XP+"' and v.`id_Required-Education` = '"+vac.IdRequiredEducation+"' and v.id_city = '"+vac.Id_City+"' and v.MINSalary = '"+vac.MINSalary+"' and v.MAXSalary = '"+ vac.MAXSalary+"' and v.Responsibilities = '"+vac.Responsibilities+"' and v.Requirements = '"+ vac.Requirements+"'and v.Conditions = '"+vac.Conditions+"' and v.Description = '"+vac.Description+"' and v.IsFlexibleSchedule = '"+(vac.IsFlexibleSchedule ? 1:0)+"' and v.Title = '"+vac.Title+"' and v.IsFullEmployment = '"+(vac.IsFullEmployment ? 1 :0)+"';";
            using (var mc = new MySqlCommand(sql, connect)) 
            using (var reader = mc.ExecuteReader())
            {
                Vacancy vacancy = new Vacancy();
                while (reader.Read())
                {
                    vacancy.Id = reader.GetInt32("id");
                }
                list.Add(vacancy);
            }
            if (list.Count == 1)
                return list[0].Id;
            return -1;
        }
        internal void DeleteVac(Vacancy Vacancy)
        {
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return;

            string sql = "DELETE FROM `Cross_Vacancy_Field-of-activity` WHERE id_vac = '" + Vacancy.Id + "';";
            sql += "DELETE FROM Vacancy WHERE ID = '" + Vacancy.Id + "';";

            using (var mc = new MySqlCommand(sql, connect))
                mc.ExecuteNonQuery();
        }
        internal IEnumerable<Vacancy> Search(string searchText, int minSalary, int xp, bool isflex, bool isfull, Field_of_Activity selectedField, Education selectedEd, City selectedCity)
        {
            string sql = "SELECT v.ID, v.employer_id, v.`Required-XP`, v.`id_Required-Education`, v.id_city, v.MINSalary, v.MAXSalary, v.Responsibilities, v.Requirements, v.Conditions, v.Description, v.IsFlexibleSchedule, v.Title, v.IsFullEmployment, foa.ID AS foaId, foa.Field_name AS foaTitle FROM CURSE.`Fields-of-activity` foa, CURSE.Education e, CURSE.Employer emp, CURSE.Vacancy v join `Cross_Vacancy_Field-of-activity` cvfoa on (cvfoa.id_vac  = v.ID ) WHERE cvfoa.id_vac = v.ID AND cvfoa.id_field = foa.ID AND v.`id_Required-Education` =e.ID and v.employer_id = emp.ID ";
            sql += " AND (v.Title LIKE '%" + searchText + "%')";
            sql+=" AND (v.MAXSalary >="+ minSalary + ")";
            sql += " AND (v.`Required-XP` <=" + xp + ")";
            if(isflex)
            sql+= " AND (v.IsFlexibleSchedule ="+ isflex+ ")";
            if(!isfull)
            sql += " AND(v.IsFullEmployment=" + isfull + ")";

            if (selectedField != null && selectedField.Id != 0 && selectedCity != null && selectedCity.Id != 0 && selectedEd != null && selectedEd.Id != 0)
                sql += " AND  cvfoa.id_field in (SELECT DISTINCT cvfoa.id_field  from `Cross_Vacancy_Field-of-activity` cvfoa WHERE cvfoa.id_vac in (select v.ID from `Vacancy` v join `Cross_Vacancy_Field-of-activity` cvfoa on cvfoa.id_vac = v.ID WHERE cvfoa.id_field = " + selectedField.Id + ")) AND v.id_city =" + selectedCity.Id + " and v.`id_Required-Education` >=" + selectedEd.Id + "  ORDER by v.ID;";

            else if (selectedField != null && selectedField.Id != 0 && selectedCity != null && selectedCity.Id != 0)
                sql += " AND  cvfoa.id_field in (SELECT DISTINCT cvfoa.id_field  from `Cross_Vacancy_Field-of-activity` cvfoa WHERE cvfoa.id_vac in (select v.ID from `Vacancy` v join `Cross_Vacancy_Field-of-activity` cvfoa on cvfoa.id_vac = v.ID WHERE cvfoa.id_field = " + selectedField.Id + ")) AND v.id_city =" + selectedCity.Id + " ORDER by v.ID;";
            else if (selectedField != null && selectedField.Id != 0 && selectedEd != null && selectedEd.Id != 0)
                sql += " AND  cvfoa.id_field in (SELECT DISTINCT cvfoa.id_field  from `Cross_Vacancy_Field-of-activity` cvfoa WHERE cvfoa.id_vac in (select v.ID from `Vacancy` v join `Cross_Vacancy_Field-of-activity` cvfoa on cvfoa.id_vac = v.ID WHERE cvfoa.id_field = " + selectedField.Id + ")) AND  v.`id_Required-Education` >=" + selectedEd.Id + " ORDER by v.ID;";
            else if (selectedCity != null && selectedCity.Id != 0 && selectedEd != null && selectedEd.Id != 0)
                sql += " AND v.id_city =" + selectedCity.Id + " and v.`id_Required-Education` >=" + selectedEd.Id + " ORDER by v.ID;";

            else if (selectedCity != null && selectedCity.Id != 0)
                sql += " AND v.id_city =" + selectedCity.Id + " ORDER by v.ID;";
            else if (selectedField != null && selectedField.Id != 0)
                sql += " AND cvfoa.id_field in (SELECT DISTINCT cvfoa.id_field  from `Cross_Vacancy_Field-of-activity` cvfoa WHERE cvfoa.id_vac in (select v.ID from `Vacancy` v join `Cross_Vacancy_Field-of-activity` cvfoa on cvfoa.id_vac = v.ID WHERE cvfoa.id_field = "+ selectedField.Id + "))";
            else if (selectedEd != null && selectedEd.Id != 0)
                sql += "and v.`id_Required-Education` >=" + selectedEd.Id + " ORDER by v.ID;";

            else
                sql += " ORDER by v.ID;";
            string result=sql;
            return GetAllVacancy(result);

            //return drinks.Where(s => 
            //    s.Title.Contains(searchText));
            //else
            //    return drinks.
        }
        //if (selectedField != null && selectedField.Id != 0)
        //{
        //    var list = GetAllVacancy(sql).Where(s => s.FieldofActivity.FirstOrDefault(s => s.Title == selectedField.Title) != null);
        //    return list;
        //}
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