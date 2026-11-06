using PersonManagement.DAL;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;



namespace PersonManagement.BLL
{
    public class People
    {
        public int PersonID { get; set; }
        public string FirstName     { get; set; }
        public string LastName { get; set; }
        public int CountryID { get; set; }

        public PersonDTO PDTO { get { return new PersonDTO(this.PersonID, this.FirstName, this.LastName, this.CountryID); } }
       public  enum  enMode { AddNew,Update};
        public enMode _Mode;

        public People(PersonDTO personDTO , enMode mode)
        {
            this.PersonID = personDTO.PersonID; 
            this.FirstName = personDTO.FirstName;   
            this.LastName = personDTO.LastName;
            this.CountryID = personDTO.CountryID;
            this._Mode = mode;
        }

       People(PersonDTO personDTO)
        {
            this.PersonID = personDTO.PersonID;
            this.FirstName = personDTO.FirstName;
            this.LastName = personDTO.LastName;
            this.CountryID = personDTO.CountryID;
            this._Mode = enMode.Update;
        }

        public static async Task <People>  Find(int personId)
        {
            PersonDTO personDTO = await PeopleData.FindPersonById(personId);

            if (personDTO != null)
            {
                return  new  People(personDTO);
            }
            else
            {
                return null;
            }
        }

        public static async Task <List<PersonDTO>>GetAll()
        {
            return  await PeopleData.GetAllPeople(); 
        }
       async Task< bool> _AddNew()
        {
            this.PersonID = await PeopleData.AddNewPerson(PDTO);
            if (PersonID != -1)
            {
                this._Mode = enMode.Update;
                return true;
            }
            return false;
        }

        async Task<bool> update()
        {
            return await    PeopleData.UpdatePerson(PDTO);  
        }

        public async Task<bool> Save()
        {
            return _Mode == enMode.AddNew ? await _AddNew() : await update();

        }

        public static async Task<bool> delete(int personID)
        {
            return await PeopleData.DeletePeeson(personID);
        }

    }
}
