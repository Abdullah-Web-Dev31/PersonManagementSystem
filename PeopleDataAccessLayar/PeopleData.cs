using PersonManagement.DAL.Repositories;
using Microsoft.Data.SqlClient;
namespace PersonManagement.DAL
{
    public class PeopleData
    {
        private readonly PersonRepository _repository;

        public PeopleData(PersonRepository repository)
        {
            _repository = repository;
        }

    }
}
