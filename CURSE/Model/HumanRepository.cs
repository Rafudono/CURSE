using CURSE.ViewModel;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CURSE.Model
{
    public class HumanRepository
    {
        private HumanRepository()
        {
        }

        static HumanRepository instance;
        public static HumanRepository Instance
        {
            get
            {
                if (instance == null)
                    instance = new HumanRepository();
                return instance;
            }
        }

        internal IEnumerable<Human> GetPeople(string sql)
        {
            var result = new List<Human>();
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return result;
            using (var mc = new MySqlCommand(sql, connect))
            using (var reader = mc.ExecuteReader())
            {
                Human hum = new Human();
                int id;
                while (reader.Read())
                {
                    id = reader.GetInt32("id");
                    if (hum.Id != id)
                    {
                        hum = new Human();
                        result.Add(hum);
                        hum.Id = id;
                        hum.Surname = reader.GetString("Surname");
                        hum.Name = reader.GetString("Name");
                        hum.Middle_Name = reader.GetString("Middle_Name");
                        hum.Email = reader.GetString("Email");
                        hum.PhoneNumber = reader.GetString("PHONE_NUMBER");
                        hum.Password = reader.GetString("PASSWORD");
                        hum.Birthday = reader.GetDateTime("Birthday");
                        int index = reader.GetOrdinal("Photo");
                        using (var stream = reader.GetStream(index))
                        {
                            hum.Photo = new byte[stream.Length];
                            stream.Read(hum.Photo, 0, (int)stream.Length);
                        }
                    }
                }
            }
            return result;
        }
        internal int AddHuman(Human hum)
        {
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return 0;

            int id = MySqlDB.Instance.GetAutoID("Human");                 //всегда 25
              
            string sql = "INSERT INTO Human VALUES (0, @Password, @Email, @PhoneNumber, @Name, " +
                "@Middle_Name, @Surname, @Birthday, @Photo)";
            using (var mc = new MySqlCommand(sql, connect))
            {
                mc.Parameters.Add(new MySqlParameter("id", hum.Id));
                mc.Parameters.Add(new MySqlParameter("Password", hum.Password));
                mc.Parameters.Add(new MySqlParameter("Email", hum.Email));
                mc.Parameters.Add(new MySqlParameter("PhoneNumber", hum.PhoneNumber));
                mc.Parameters.Add(new MySqlParameter("Name", hum.Name));
                mc.Parameters.Add(new MySqlParameter("Middle_Name", hum.Middle_Name));
                mc.Parameters.Add(new MySqlParameter("Surname", hum.Surname));
                mc.Parameters.Add(new MySqlParameter("Birthday", hum.Birthday));
                mc.Parameters.Add(new MySqlParameter("Photo", hum.Photo));
                mc.ExecuteNonQuery();
            }
            return id;
        }
        internal int GetTrueId(Human hum)
        {
            List<Human> list = new List<Human>();
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                MessageBox.Show("нет подключения к БД");
            string sql = "SELECT h.ID  from CURSE.Human h Where h.PASSWORD= '" + hum.Password + "' and h.EMAIL ='" + hum.Email + "' and h.PHONE_NUMBER=" + hum.PhoneNumber + " and h.Name='" + hum.Name + "' and h.Middle_name = '" + hum.Middle_Name + "' and h.Surname = '" + hum.Surname + "';";
            using (var mc = new MySqlCommand(sql, connect))
            using (var reader = mc.ExecuteReader())
            {
                Human human = new Human();
                while (reader.Read())
                {
                    human.Id = reader.GetInt32("id");
                }
                list.Add(human);
            }
            if (list.Count == 1)
                return list[0].Id;
            return -1;
        }
        internal Human LoginHuman(string email, string password)
        {

            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                MessageBox.Show("нет соединения с бд");
            Human hum = new Human();
            string sql = "SELECT * FROM Human h WHERE (h.EMAIL ='" + email + "') and (h.PASSWORD ='" + password + "');";


            using (var mc = new MySqlCommand(sql, connect))
            using (var reader = mc.ExecuteReader())
            {

                if (reader.Read())
                {
                    hum.Id = reader.GetInt32("id");
                    hum.Password = reader.GetString("Password");
                    hum.Email = reader.GetString("Email");
                    hum.PhoneNumber = reader.GetString("PHONE_NUMBER");
                    hum.Name = reader.GetString("Name");
                    hum.Middle_Name = reader.GetString("Middle_Name");
                    hum.Surname = reader.GetString("Surname");
                    hum.Birthday = reader.GetDateTime("Birthday");
                    int index = reader.GetOrdinal("Photo");
                    using (var stream = reader.GetStream(index))
                    {
                        hum.Photo = new byte[stream.Length];
                        stream.Read(hum.Photo, 0, (int)stream.Length);
                    }
                }
                return hum;
            }
        }
        internal Human FindHum(int id_emp)
        {

            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                MessageBox.Show("нет соединения с бд");

            Human hum = new Human();
            string sql = "Select h.ID ,h.PASSWORD, h.EMAIL, h.PHONE_NUMBER, h.Name, h.Middle_name, h.Surname, h.Birthday, h.Photo from CURSE.Human h, CURSE.Employer e WHERE e.ID = "+id_emp+" and e.id_human = h.ID;";
            using (var mc = new MySqlCommand(sql, connect))
            using (var reader = mc.ExecuteReader())
            {
                if (reader.Read())
                {
                    hum.Id = reader.GetInt32("id");
                    hum.Password = reader.GetString("Password");
                    hum.Email = reader.GetString("Email");
                    hum.PhoneNumber = reader.GetString("PHONE_NUMBER");
                    hum.Name = reader.GetString("Name");
                    hum.Middle_Name = reader.GetString("Middle_Name");
                    hum.Surname = reader.GetString("Surname");
                    hum.Birthday = reader.GetDateTime("Birthday");
                    int index = reader.GetOrdinal("Photo");
                    using (var stream = reader.GetStream(index))
                    {
                        hum.Photo = new byte[stream.Length];
                        stream.Read(hum.Photo, 0, (int)stream.Length);
                    }
                }
                return hum;
            }
        }
        internal void Delete(Human hum)
        {
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return;
            string sql = "SELECT h.ID, h.Surname, h.Name, h.Middle_Name, h.Email, h.PHONE_NUMBER, h.PASSWORD, h.Birthday, h.Photo, a.id_human  from Human h, CURSE.Applicant a  WHERE h.ID = " + HumAuthorization.Instance.loginHuman.Id + " and h.ID =a.id_human ;";
            var contains = GetPeople(sql);
            if (contains.Count() != 0)
            {
                ApplicantRepository.Instance.Delete(ApplAuthorized.Instance.loginAppl);
            }

            else
            {
                sql = "SELECT h.ID, h.Surname, h.Name, h.Middle_Name, h.Email, h.PHONE_NUMBER, h.Password, h.Birthday, h.Photo, e.id_human  from Human h, CURSE.Employer e  WHERE h.ID = " + HumAuthorization.Instance.loginHuman.Id + " and h.ID =e.id_human ;";
                var econtains = GetPeople(sql);
                if (econtains.Count() != 0)
                {
                    EmployerRepository.Instance.Delete(EmpAuthorized.Instance.loginEmp);
                }
                else { MessageBox.Show("ошибка"); }
            }

            sql = "DELETE FROM Human WHERE ID = '" + hum.Id + "';";

            using (var mc = new MySqlCommand(sql, connect))
                mc.ExecuteNonQuery();
        }

        internal void UpdateData(Human hum)
        {
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return;


           string sql = "UPDATE Human SET Name = @name, Surname = @surname, Middle_Name = @middle_Name, Email = @email, PHONE_NUMBER=@phoneNumber, Password=@password, Birthday=@birthday, Photo=@photo WHERE ID = " + hum.Id+";";
            using (var mc = new MySqlCommand(sql, connect))
            {
                mc.Parameters.Add(new MySqlParameter("id", hum.Id));
                mc.Parameters.Add(new MySqlParameter("surname", hum.Surname));
                mc.Parameters.Add(new MySqlParameter("name", hum.Name));
                mc.Parameters.Add(new MySqlParameter("middle_Name", hum.Middle_Name));
                mc.Parameters.Add(new MySqlParameter("email", hum.Email));
                mc.Parameters.Add(new MySqlParameter("phoneNumber", hum.PhoneNumber));
                mc.Parameters.Add(new MySqlParameter("password", hum.Password));
                mc.Parameters.Add(new MySqlParameter("birthday", hum.Birthday));
                mc.Parameters.Add(new MySqlParameter("photo", hum.Photo));
                mc.ExecuteNonQuery();
            }
        }
    }
}
