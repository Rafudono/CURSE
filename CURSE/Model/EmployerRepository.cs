using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                        emp.Org_Name = reader.GetString("Org_Name");
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

            int id = MySqlDB.Instance.GetAutoID("Employer");

            string sql = "INSERT INTO Employer VALUES (0, @h_id, @Org_Name)";
            using (var mc = new MySqlCommand(sql, connect))
            {
                mc.Parameters.Add(new MySqlParameter("id", employer.ID));
                mc.Parameters.Add(new MySqlParameter("h_id", employer.id_human));
                mc.Parameters.Add(new MySqlParameter("Org_Name", employer.Org_Name));
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
                        employer.ID = id;
                        employer.id_human = reader.GetInt32("id_human");
                        employer.Org_Name = reader.GetString("Org_Name");
                    }
                }
            }
            return employer;
        }
        internal Employer FindEmp(int id_vac)
        {

            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                MessageBox.Show("нет соединения с бд");

            Employer employer = new Employer();
            string sql = "SELECT e.ID, e.id_human, e.Org_Name FROM CURSE.Employer e, CURSE.Vacancy v Where v.ID = " + id_vac + " and v.employer_id = e.ID;";
            using (var mc = new MySqlCommand(sql, connect))
            using (var reader = mc.ExecuteReader())
            {
                int id;
                while (reader.Read())
                {
                    id = reader.GetInt32("id");
                    if (employer.Id != id)
                    {
                        employer.ID = id;
                        employer.id_human = reader.GetInt32("id_human");
                        employer.Org_Name = reader.GetString("Org_Name");
                    }
                }
            }
            return employer;
        }
        internal void Delete(Employer emp)
        {
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return;
            ObservableCollection<Vacancy> list;
            //получить список вакансий этого работодателя, если элементов больше 0, то удалить вакансии из репозитория вакансий, у которых id работодателя такое же, то удалить эти вакансии, а потом удалить ЭТО
            string getListVac = "SELECT v.ID, v.employer_id, v.`Required-XP`, v.`id_Required-Education`, v.id_city, v.MINSalary, v.MAXSalary, v.Responsibilities, v.Requirements, v.Conditions, v.Description, v.IsFlexibleSchedule, v.Title, foa.ID AS foaId, foa.Field_name AS foaTitle FROM `Cross_Vacancy_Field-of-activity` cvfoa, Vacancy v, `Fields-of-activity` foa WHERE cvfoa.id_vac = v.ID AND cvfoa.id_field = foa.ID AND v.employer_id ='" + emp.ID+ "'ORDER by v.ID; ";
             list = new ObservableCollection<Vacancy>( VacancyRepository.Instance.GetAllVacancy(getListVac));
            if (list.Count != 0)
            {
                foreach (Vacancy vacancy in list)
                {
                    VacancyRepository.Instance.DeleteVac(vacancy);
                }
            }
            string sql = "DELETE FROM Employer WHERE ID = '" + emp.ID + "';";
            using (var mc = new MySqlCommand(sql, connect))
                mc.ExecuteNonQuery();

        }
        internal void UpdateEmployer(Employer employer)
        {
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return;
            string sql = "Update  Employer SET id_human = @id_human, Org_Name = @Org_Name WHERE ID =" + employer.ID+";";

            using (var mc = new MySqlCommand(sql, connect))
            {
                mc.Parameters.Add(new MySqlParameter("id", employer.ID));
                mc.Parameters.Add(new MySqlParameter("id_human", employer.id_human));
                mc.Parameters.Add(new MySqlParameter("Org_Name", employer.Org_Name));
                mc.ExecuteNonQuery();
            }
        }
    }
}

