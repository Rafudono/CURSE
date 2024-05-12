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
                    hum.Id = id;
                    if (hum.Id != 0)
                    {
                        hum = new Applicant();
                        result.Add(hum);
                        hum.Id = id;
                        hum.Surname = reader.GetString("Surname");
                        hum.Name = reader.GetString("Name");
                        hum.Middle_Name = reader.GetString("Middle_Name");
                        hum.Email = reader.GetString("Email");
                        hum.PhoneNumber = reader.GetString("PHONE_NUMBER");
                        hum.Password = reader.GetString("Password");
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

            int id = MySqlDB.Instance.GetAutoID("Human");

            string sql = "INSERT INTO Human VALUES (0, @Surname, @Name, @Middle_Name, @Email, " +
                "@PhoneNumber, @Password, @Birthday, @Photo)";
            using (var mc = new MySqlCommand(sql, connect))
            {
                mc.Parameters.Add(new MySqlParameter("id", hum.Id));
                mc.Parameters.Add(new MySqlParameter("Surname", hum.Surname));
                mc.Parameters.Add(new MySqlParameter("Name", hum.Name));
                mc.Parameters.Add(new MySqlParameter("Middle_Name", hum.Middle_Name));
                mc.Parameters.Add(new MySqlParameter("Email", hum.Email));
                mc.Parameters.Add(new MySqlParameter("PhoneNumber", hum.PhoneNumber));
                mc.Parameters.Add(new MySqlParameter("Password", hum.Password));
                mc.Parameters.Add(new MySqlParameter("Birthday", hum.Birthday));
                mc.Parameters.Add(new MySqlParameter("Photo", hum.Photo));
                mc.ExecuteNonQuery();
            }
            return id;
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
        internal void Remove(Human hum)
        {
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return;

            string sql = "DELETE FROM Human WHERE Id = '" + hum.Id + "';";

            using (var mc = new MySqlCommand(sql, connect))
                mc.ExecuteNonQuery();
        }

        //internal IEnumerable<Human> Search(string searchText)
        //{
        //    string sql = "SELECT hum.ID, hum.LOGIN, hum.PASSWORD, hum.EMAIL, hum.PHONE_NUMBER, hum.Name, hum.Middle_name, hum.Surname, hum.Birthday, hum.Photo from CURSE.Human hum;";
        //    return GetPeople(sql);
        //}

        internal void UpdateData(Human hum)
        {
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return;


           string sql = "UPDATE Human SET Name = @name, Surname = @surname, Middle_Name = @middle_Name, Email = @email, PHONE_NUMBER=@phoneNumber, Password=@password, Birthday=@birthday, Photo=@photo WHERE ID = " + hum.Id;
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
            // hum = new Applicant();
            //result.Add(hum);
            //hum.Id = id;
            //hum.Surname = reader.GetString("Surname");
            //hum.Name = reader.GetString("Name");
            //hum.Middle_Name = reader.GetString("Middle_Name");
            //hum.Email = reader.GetString("Email");
            //hum.PhoneNumber = reader.GetString("PhoneNumber");
            //hum.Password = reader.GetString("Password");
            //hum.Birthday = reader.GetDateTime("Birthday");
            //int index = reader.GetOrdinal("Photo");
            //using (var stream = reader.GetStream(index))
            //{
            //    hum.Photo = new byte[stream.Length];
            //    stream.Read(hum.Photo, 0, (int)stream.Length);
        }
    }
}
