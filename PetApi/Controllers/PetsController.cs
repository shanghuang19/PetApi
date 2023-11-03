using Microsoft.AspNetCore.Mvc;

namespace PetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetsController : ControllerBase
    {
        private static Dictionary<string, Pet> _pets = new Dictionary<string, Pet>();

        [HttpPost]
        public ActionResult<Pet> Create(Pet requst)
        {
            if (_pets.ContainsKey(requst.Name)) {
                return BadRequest();
            }

            var petCreated = new Pet
            (
                requst.Name,
                requst.Type,
                requst.Color,
                requst.Price
            );

            _pets.Add(petCreated.Name, petCreated);
            return StatusCode(StatusCodes.Status201Created, petCreated);
        }

        [HttpGet("{name}")]
        public ActionResult<Pet> Get(string name)
        {
            if (_pets.ContainsKey(name))
            {
                return _pets[name];
            }
            return NotFound();
        }

        [HttpGet]
        public List<Pet> GetAll()
        {
            return _pets.Values.ToList();
        }

        [HttpDelete("{name}")]
        public ActionResult Delete (string name) 
        { 
            if (!_pets.ContainsKey(name))
            {
                return NotFound();
            }
            _pets.Remove(name);
            return NoContent();
        }

        [HttpPut("{name}")]
        public ActionResult<Pet> UpdatePetPrice(string name, UpdatePriceRequest updatePriceRequest)
        {
            if (!_pets.ContainsKey(name))
            {
                return NotFound();
            }
            var petToUpdate = _pets[name];
            petToUpdate.Price = updatePriceRequest.Price;
            return petToUpdate;
        }

        [HttpDelete]
        public ActionResult DeleteAll()
        {
            _pets.Clear();
            return NoContent();
        }
    }
}
