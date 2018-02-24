using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.SPA.Controllers
{
    [Produces("application/json")]
    [Route("api/Pokemon")]
    public class PokemonController : Controller
    {
        private ICollection<Pokemon> _pokemons;

        public PokemonController()
        {
            _pokemons = new List<Pokemon>(new Pokemon[]
            {
                new Pokemon { Id = 1, Name = "Jigglypuff", Type = "Fairy" },
                new Pokemon { Id = 2, Name = "Pikachu", Type = "Electric" },
                new Pokemon { Id = 3, Name = "Charizard", Type = "Fire" },
                new Pokemon { Id = 4, Name = "Bulbasaur", Type = "Grass" }
            });
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(_pokemons);

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var item = _pokemons.FirstOrDefault(x => x.Id == id);

            if (item is null)
                return BadRequest("Pokemon not found");
            else
                return Ok(item);
        }

        [HttpPut("[action]")]
        public IActionResult Put([FromBody]Pokemon model)
        {
            if (ModelState.IsValid)
            {
                _pokemons.Add(model);
                return Created("/api/Pokemon/Put", model);
            }

            return BadRequest(ModelState.ValidationState);
        }

        [HttpPost("[action]/{id}")]
        public IActionResult Post([FromRoute]int id, [FromBody]Pokemon model)
        {
            var pokemon = _pokemons.FirstOrDefault(x => x.Id == id);

            if (pokemon is null)
                return BadRequest("Wrong id");

            if (ModelState.IsValid)
            {
                pokemon.Id = model.Id;
                pokemon.Name = model.Name;
                pokemon.Type = model.Type;

                return Ok(pokemon);
            }

            return BadRequest(ModelState.ValidationState);
        }

        [HttpDelete("[action]/{id}")]
        public IActionResult Delete([FromRoute]int id)
        {
            var pokemon = _pokemons.FirstOrDefault(x => x.Id == id);

            if (pokemon is null)
                return BadRequest("Pokemon not found");

            _pokemons.Remove(pokemon);

            return Ok("Removed");
        }
    }

    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}