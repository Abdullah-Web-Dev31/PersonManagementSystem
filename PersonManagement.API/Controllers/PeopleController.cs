using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PersonManagement.BLL;
using PersonManagement.DAL;

namespace PersonManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        [HttpGet("GetAllPeople",Name ="GetAllPeople")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task< ActionResult<IEnumerable<PersonDTO>>>GetAllPeople()
        {
            var people = await People.GetAll();
            if (people == null)
                return  NotFound("Not found any people");
            return  Ok(people);  
        }

        [HttpDelete("DeletePerson/{PersonID}",Name ="DeletePerson")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<bool>>DeletePerson(int PersonID)
        {
            if (PersonID <= 0) return BadRequest("Bad request");
            if ( await People.delete(PersonID)) return Ok("Deleted successfully");
            return NotFound($"Not found any id with this {PersonID}");

        }

        [HttpGet("GetPersonByID/{PersonID}",Name ="GetPersonByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<PersonDTO>>GetPersonByID(int PersonID)
        {
            if (PersonID <= 0) return BadRequest("Type id grater than 0 ");
            People person = await People.Find(PersonID);
            if (person == null) return NotFound("Not found a y person with this id ");
            return Ok(person.PDTO);

        }

        [HttpPost("AddNewPerson", Name = "AddNew")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PersonDTO>> AddNew(PersonDTO dTO)
        {
            if (dTO.CountryID.ToString() == "" || string.IsNullOrWhiteSpace(dTO.FirstName) || string.IsNullOrWhiteSpace(dTO.LastName))
            {
                return BadRequest("any data is required");
            }
            People people = new People(new PersonDTO(dTO.PersonID, dTO.FirstName, dTO.LastName, dTO.CountryID),People.enMode.AddNew);
            if (await people.Save())
                return CreatedAtRoute("GetPersonByID", new {PersonID = people.PersonID},people.PDTO);
            return StatusCode(500, "Server error");
        }

        [HttpPut("Update/{personID}", Name = "Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PersonDTO>> Update(int personID,PersonDTO dTO)
        {
            if (personID <= 0) return NotFound("type id grater than 0 ");


            if (dTO.CountryID.ToString() == "" || string.IsNullOrWhiteSpace(dTO.FirstName) || string.IsNullOrWhiteSpace(dTO.LastName))
            {
                return BadRequest("any data is required");
            }
            People people = await People.Find(personID);

            if (people == null) return NotFound("Not found any person with this id ");

            people.FirstName = dTO.FirstName;
            people.LastName = dTO.LastName;
            people.CountryID = dTO.CountryID;

            if (await people.Save())
                return Ok(people.PDTO);
            return StatusCode(500, "Server Error");
        }


    }
}
