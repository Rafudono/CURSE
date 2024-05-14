using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CURSE.Model
{
    public class EmployerRepository
    {
        private EmployerRepository()
        {
        }

        static EmployerRepository instance;
        public static EmployerRepository Instance
        {
            get
            {
                if (instance == null)
                    instance = new EmployerRepository();
                return instance;
            }
        }

        internal IEnumerable<Employer> GetAllEmployers(string sql)
        {
            var result = new List<Employer>();
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return result;
            using (var mc = new MySqlCommand(sql, connect))
            using (var reader = mc.ExecuteReader())
            {
                Employer emp = new Employer();
                int id;
                while (reader.Read())
                {
                    id = reader.GetInt32("id");
                    if (emp.Id != id)
                    {
                        emp = new Employer();
                        result.Add(emp);
                        emp.ID = id;
                        emp.id_human = reader.GetInt32("id_human");
                        emp.id_organization = reader.GetInt32("XP");
                    }
                }
            }
            return result;
        }

        internal void AddEmployer(Employer employer)
        {
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return;

            int id = MySqlDB.Instance.GetAutoID("Applicant");

            string sql = "INSERT INTO Applicant VALUES (0, @h_id, @xp, @id_foa, @id_education, @id_city, " +
                "@salary, @isfull-employment, @isflex-schedule)";
            using (var mc = new MySqlCommand(sql, connect))
            {
                mc.Parameters.Add(new MySqlParameter("id", employer.ID));
                mc.Parameters.Add(new MySqlParameter("h_id", employer.id_human));
                mc.Parameters.Add(new MySqlParameter("xp", employer.id_organization));
                mc.ExecuteNonQuery();
            }
        }
        internal Employer LoginEmployer(int id_hum)
        {

            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                MessageBox.Show("нет соединения с бд");

            Employer employer = new Employer();
            string sql = "SELECT * FROM CURSE.Employer e Where (e.id_human = " + id_hum + ");";
            using (var mc = new MySqlCommand(sql, connect))
            using (var reader = mc.ExecuteReader())
            {
                int id;
                while (reader.Read())
                {
                    id = reader.GetInt32("id");
                    if (employer.Id != id)
                    {
                        employer.Id = id;
                        employer.id_human = reader.GetInt32("id_human");
                        employer.id_organization = reader.GetInt32("id_organization");
                    }
                }
            }
            return employer;

        }
        internal void UpdateEmployer(Employer employer)
        {
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return;
            string sql = "";

            

            sql = "";
            using (var mc = new MySqlCommand(sql, connect))
            {
                mc.Parameters.Add(new MySqlParameter("id", employer.ID));
                mc.Parameters.Add(new MySqlParameter("id_human", employer.id_human));
                mc.Parameters.Add(new MySqlParameter("id_organization", employer.id_organization));
                mc.ExecuteNonQuery();
            }
        }
    }
}

