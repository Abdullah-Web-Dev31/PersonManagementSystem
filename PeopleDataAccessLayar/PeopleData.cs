using PersonManagement.DAL.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;
namespace PersonManagement.DAL
{

    public class PersonDTO
    {
        public int PersonID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CountryID { get; set; }

        public PersonDTO(int personID, string firstName, string lastName, int countryID)
        {
            PersonID = personID;
            FirstName = firstName;
            LastName = lastName;
            CountryID = countryID;
        }
    }

    public class PeopleData
    {


        public static async Task<List<PersonDTO>> GetAllPeople()
        {
            var people = new List<PersonDTO>();

            try
            {
                using (SqlConnection conn = DataAccessSettings.GetConnection())
                {
                    using (SqlCommand comm = new SqlCommand("GetAll", conn))
                    {
                        comm.CommandType = System.Data.CommandType.StoredProcedure;
                       await conn.OpenAsync();
                        using(SqlDataReader reader = await comm.ExecuteReaderAsync())
                        {
                            if (!reader.HasRows)
                                return null;
                            else
                            {
                                while (reader.Read())
                                {
                                    people.Add(new PersonDTO(reader.GetInt32(reader.GetOrdinal("PersonID")), reader.GetString(reader.GetOrdinal("FirstName")), reader.GetString(reader.GetOrdinal("LastName")), reader.GetInt32(reader.GetOrdinal("CountryID"))));
                                }
                                return people;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        

        }

        public static async Task<PersonDTO> FindPersonById(int personid)
        {
            

            try
            {
                using (SqlConnection conn = DataAccessSettings.GetConnection())
                {
                    using (SqlCommand comm = new SqlCommand("GetPersonByID", conn))
                    {
                        comm.CommandType = System.Data.CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("@PersonID",personid);
                        await conn.OpenAsync();
                        using (SqlDataReader reader = await comm.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                            return  new PersonDTO(reader.GetInt32(reader.GetOrdinal("PersonID")), reader.GetString(reader.GetOrdinal("FirstName")), reader.GetString(reader.GetOrdinal("LastName")), reader.GetInt32(reader.GetOrdinal("CountryID")));
                            }   
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            


            return null;
        }

        public static async Task<bool> DeletePeeson(int PersonID)
        {
            try
            {
                using(SqlConnection conn = DataAccessSettings.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("DeletePerson", conn))
                    {
                      await  conn.OpenAsync();
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        //  object obj =  await cmd.ExecuteNonQueryAsync();
                        return await cmd.ExecuteNonQueryAsync()>0;


                        
                    }
                }
            }catch(Exception ex)
            {
                throw ex;
            }

        }

        public static async Task<int> AddNewPerson(PersonDTO pdto)
        {
            try
            {
                using(SqlConnection conn = DataAccessSettings.GetConnection())
                {
                    using(SqlCommand cmd = new SqlCommand("AddNewPerson",conn))
                    {
                      await  conn.OpenAsync();
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@FirstName", pdto.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", pdto.LastName);
                        cmd.Parameters.AddWithValue("@CountryID", pdto.CountryID);
                        SqlParameter outparam = new SqlParameter("PersonID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outparam);
                        await cmd.ExecuteScalarAsync();

                        return (int)outparam.Value;

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static async Task<bool> UpdatePerson(PersonDTO pdto)
        {
            try
            {
                using (SqlConnection conn = DataAccessSettings.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("UpdatePerson", conn))
                    {
                        await conn.OpenAsync();
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@FirstName", pdto.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", pdto.LastName);
                        cmd.Parameters.AddWithValue("@CountryID", pdto.CountryID);
                        cmd.Parameters.AddWithValue("@PersonID", pdto.PersonID);

                        return  (int)await cmd.ExecuteNonQueryAsync() > 0;


                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


    }
}
