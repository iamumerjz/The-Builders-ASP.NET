using ClassLibraryDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using ClassLibraryModel;
using System.Xml.Linq;

namespace ClassLibraryDal
{
    public class DalConstruction
    {
        public static void SaveUserInformation(User mu)
        {
            SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
                using (SqlCommand cmd = new SqlCommand("InsertUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", mu.Email);
                    cmd.Parameters.AddWithValue("@Password", mu.Password);
                    cmd.Parameters.AddWithValue("@UserType", mu.UserType);
                    cmd.Parameters.AddWithValue("@isProfileComplete", mu.isProfileComplete);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            
        }

        public static (bool isAuthenticated, int role) AuthenticateUser(string username, string password)
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("AuthenticateUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", username);
                    cmd.Parameters.AddWithValue("@Password", password);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int result = Convert.ToInt32(reader["Result"]); // Assuming your stored procedure returns 'Result' column with 1 or 0 directly
                            bool isAuthenticated = result == 1;
                            int role = Convert.ToInt32(reader["UserType"]); // Assuming your stored procedure returns 'Role' column

                            return (isAuthenticated, role);
                        }
                    }
                }
                conn.Close();
            }

            return (false, -1); // Default values if authentication fails
        }

        public static bool UserExists(string email)
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("UserExists", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", email);

                    // Use ExecuteScalar to check if any record exists with the given email
                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    conn.Close();

                    // If count is greater than 0, it means the user exists
                    return count > 0;
                }
            }
        }

        public static int FetchProfileCompleteness(string email)
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("FetchProfileCompleteness", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", email);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    conn.Close();

                    return count;
                }
            }
        }

        public static void SaveCustomerInformation(CustomerReg cr)
        {
            SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            using (SqlCommand cmd = new SqlCommand("InsertCustomerData", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FName", cr.FName);  
                cmd.Parameters.AddWithValue("@LName", cr.LName);  
                cmd.Parameters.AddWithValue("@Address", cr.Address);
                cmd.Parameters.AddWithValue("@City", cr.City);
                cmd.Parameters.AddWithValue("@State", cr.State);
                cmd.Parameters.AddWithValue("@Zip", cr.Zip);
                cmd.Parameters.AddWithValue("@Phone", cr.Phone);
                cmd.Parameters.AddWithValue("@LoggedInUserEmail", cr.LoggedInUserEmail); 
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public static void SaveLabourerInformation(LabourerReg lr)
        {
            SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            using (SqlCommand cmd = new SqlCommand("InsertLabourerData", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FName", lr.FName);
                cmd.Parameters.AddWithValue("@LName", lr.LName);
                cmd.Parameters.AddWithValue("@City", lr.City);
                cmd.Parameters.AddWithValue("@Skill", lr.Skill);
                cmd.Parameters.AddWithValue("@Experience", lr.Experience);
                cmd.Parameters.AddWithValue("@Phone", lr.Phone);
                cmd.Parameters.AddWithValue("@Price", lr.Price);
                cmd.Parameters.AddWithValue("@LoggedInUserEmail", lr.LoggedInUserEmail);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public static LabourerReg GetLabourerDetailsByEmail(string email)
        {
            LabourerReg labourer = null;
            if (GlobalVar.labemail != null)
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetLabourerDetailsByEmail", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LabourerEmail", email);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            labourer = new LabourerReg
                            {
                                FName = reader["FName"].ToString(),
                                LName = reader["LName"].ToString(),
                                City = reader["City"].ToString(),
                                Phone = reader["Phone"].ToString(),
                                Skill = reader["Skill"].ToString(),
                                Experience = reader["Experience"].ToString(),
                                Price = reader["Price"].ToString()
                            };
                        }
                    }
                }
            }
            return labourer;
        }

        public static void UpdateLabourerInformation(string email, LabourerReg lr)
        {
            SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("UpdateLabourerInformation", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@LabourerEmail", email);
            cmd.Parameters.AddWithValue("@FName", lr.FName);
            cmd.Parameters.AddWithValue("@LName", lr.LName);
            cmd.Parameters.AddWithValue("@City", lr.City);
            cmd.Parameters.AddWithValue("@Skill", lr.Skill);
            cmd.Parameters.AddWithValue("@Experience", lr.Experience);
            cmd.Parameters.AddWithValue("@Phone", lr.Phone);
            cmd.Parameters.AddWithValue("@Price", lr.Price);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static CustomerReg GetCustomerDetailsByEmail(string email)
        {
            CustomerReg customer = null;
            if (GlobalVar.custemail != null)
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetCustomerDetailsByEmail", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CustomerEmail", email);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            customer = new CustomerReg
                            {
                                FName = reader["FName"].ToString(),
                                LName = reader["LName"].ToString(),
                                Address = reader["Address"].ToString(),
                                City = reader["City"].ToString(),
                                State = reader["State"].ToString(),
                                Zip = reader["Zip"].ToString(),
                                Phone = reader["Phone"].ToString()
                            };
                        }
                    }
                }
            }
            return customer;
        }

        public static void UpdateCustomerInformation(string email, CustomerReg cr)
        {
            SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("UpdateCustomerInformation", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustomerEmail", email);
            cmd.Parameters.AddWithValue("@FName", cr.FName);
            cmd.Parameters.AddWithValue("@LName", cr.LName);
            cmd.Parameters.AddWithValue("@Address", cr.Address);
            cmd.Parameters.AddWithValue("@City", cr.City);
            cmd.Parameters.AddWithValue("@State", cr.State);
            cmd.Parameters.AddWithValue("@Zip", cr.Zip);
            cmd.Parameters.AddWithValue("@Phone", cr.Phone);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static List<LabourerReg> GetAllLabourersWithReviews()
        {
            SqlConnection conn = DbHelper.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("GetAllLabourersWithReviews", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = cmd.ExecuteReader();

            Dictionary<int, LabourerReg> labourersDict = new Dictionary<int, LabourerReg>();
            int labourerId = 0; // Move the labourerId initialization outside the loop

            while (reader.Read())
            {
                LabourerReg labourer = new LabourerReg
                {
                    LabourerId = Convert.ToInt32(reader["LabourerId"]),
                    FName = reader["FName"].ToString(),
                    LName = reader["LName"].ToString(),
                    City = reader["City"].ToString(),
                    Phone = reader["Phone"].ToString(),
                    Skill = reader["Skill"].ToString(),
                    Experience = reader["Experience"].ToString(),
                    Price = reader["Price"].ToString(),
                    Reviews = new List<Review>()
                };

                labourerId++; // Increment labourerId for each new labourer

                if (!reader.IsDBNull(reader.GetOrdinal("Rating")))
                {
                    Review review = new Review
                    {
                        Rating = Convert.ToInt32(reader["Rating"]),
                        Comment = reader["Comment"].ToString()
                    };

                    labourer.Reviews.Add(review);
                }

                labourersDict.Add(labourerId, labourer);
            }

            conn.Close();
            return labourersDict.Values.ToList();
        }

        public static void InsertAssignedWork(AssignWork aw)
        {
            
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("InsertAssignedWork", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LoggedInUserEmail", aw.loggedInUserEmail);
                    cmd.Parameters.AddWithValue("@LabourerId", aw.LabourerId);
                    cmd.ExecuteNonQuery();
                }
                conn.Close ();
            }
        }

        public static CustomerReg GetAssignedWork(string email)
        {
            CustomerReg customer = null;
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                if (email != null)
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetAssignedWork", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EmailId", email);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                customer = new CustomerReg
                                {
                                    FName = reader["CustomerFirstName"].ToString(),
                                    LName = reader["CustomerLastName"].ToString(),
                                    Address = reader["CustomerAddress"].ToString(),
                                    City = reader["CustomerCity"].ToString(),
                                    Zip = reader["CustomerZip"].ToString(),
                                    Done = (bool)reader["isAvailable"]
                                };
                            }
                        }
                    }
                    conn.Close();
                }
            }
            return customer;
        }

        public static void AddSubscriber(string email)
        {

            using (SqlConnection conn = DbHelper.GetConnection())
            {
                if (GlobalVar.Semail != null)
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("AddSubscriber", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }
            }
        }

        public static void AddBusiness(ContactUs cs)
        {
            if (cs != null && cs.Name != null && cs.BName != null && cs.Phone != null && cs.Email != null && cs.Message != null)
            {
                SqlConnection conn = DbHelper.GetConnection();
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("AddAppointment", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Name", cs.Name);
                    cmd.Parameters.AddWithValue("@BName", cs.BName);
                    cmd.Parameters.AddWithValue("@Phone", cs.Phone);
                    cmd.Parameters.AddWithValue("@Email", cs.Email);
                    cmd.Parameters.AddWithValue("@Message", cs.Message);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

        }

        public static void AddPayment(string labourerEmail)
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("AddPayment", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LabourerEmail", labourerEmail);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static LabourerStats GetLabourerStatsByEmail(string email)
        {
            LabourerStats stats = null;
            if (email != null)
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetLabourerStats", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@LabourerEmail", email);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                stats = new LabourerStats
                                {
                                    TotalOrders = reader["TotalOrders"] != DBNull.Value ? (int)reader["TotalOrders"] : 0,
                                    CancelledOrders = reader["CancelledOrders"] != DBNull.Value ? (int)reader["CancelledOrders"] : 0,
                                    CompletedOrders = reader["CompletedOrders"] != DBNull.Value ? (int)reader["CompletedOrders"] : 0,
                                    EarnedAmount = reader["EarnedAmount"] != DBNull.Value ? (decimal)reader["EarnedAmount"] : 0m
                                };
                            }
                        }
                    }
                }
            }
            return stats;
        }


        public static void UpdateAssignedWorkStatus(string email)
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand("UpdateAssignedWorkStatus", conn))
            {
                conn.Open();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmailId", email);
                cmd.ExecuteNonQuery();
                conn.Close();

            }
        }

        public static void InsertReview(string email, int rating, string comment, int labid)
        {

            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("InsertReview", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LoggedInUserEmail", email);
                    cmd.Parameters.AddWithValue("@Rating", rating);
                    cmd.Parameters.AddWithValue("@Comment", comment);
                    cmd.Parameters.AddWithValue("@LabourerId", labid);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }
        public static LabourerReg GetLabourerById(int id)
        {
            LabourerReg stats = null;

            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("GetLabourerById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LabourerId", id); // removed extra '@' sign

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            stats = new LabourerReg
                            {
                                FName = reader["FName"].ToString(),
                                LName = reader["LName"].ToString()
                            };
                        }
                    }
                }
            }

            return stats;
        }

        public static void AddCustomerStats(string customerEmail, string orderAmount)
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("AddCustomerStats", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CustomerEmail", customerEmail);
                    cmd.Parameters.AddWithValue("@OrderAmountStr", orderAmount);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

        public static CustomerStats GetCustomerStatsByEmail(string email)
        {
            CustomerStats customerStats = null;

            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("GetCustomerStats", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CustomerEmail", email);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            customerStats = new CustomerStats
                            {
                                TotalOrders = Convert.ToInt32(reader["TotalOrders"]),
                                TotalSpent = (reader["TotalSpent"].ToString())
                            };
                        }
                    }
                }
            }

            return customerStats;
        }

        public static void InsertLabourerStats(string email, int totalOrders, int cancelledOrders, int completedOrders, decimal earnedAmount)
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("InsertLabourerStats", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LabourerEmail", email);
                    cmd.Parameters.AddWithValue("@TotalOrders", totalOrders);
                    cmd.Parameters.AddWithValue("@CancelledOrders", cancelledOrders);
                    cmd.Parameters.AddWithValue("@CompletedOrders", completedOrders);
                    cmd.Parameters.AddWithValue("@Earned", earnedAmount);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

        public static List<LabourerReg> GetAssignedWorkHistory(string email)
        {
            List<LabourerReg> labourers = new List<LabourerReg>();

            using (SqlConnection conn = DbHelper.GetConnection())
            {
                if (email != null)
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetAssignedWorkHistory", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EmailId", email);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                LabourerReg labourer = new LabourerReg
                                {
                                    FName = reader["LabourerrFirstName"].ToString(),
                                    LName = reader["LabourerLastName"].ToString(),
                                    City = reader["LabourerCity"].ToString(),
                                    Price = reader["LabourerPrice"].ToString(),
                                    Skill = reader["LabourerSkill"].ToString(),
                                };
                                labourers.Add(labourer);
                            }
                        }
                    }
                    conn.Close();
                }
            }

            return labourers;
        }


    }
}
