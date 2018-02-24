using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace WebApi.CUI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var pokemons = new List<Pokemon>();

            var url = new Uri(@"http://localhost:53580/");

            var client = new HttpClient();
            client.BaseAddress = url;
            var result = await client.GetAsync(@"api/Pokemon");

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = await result.Content.ReadAsStringAsync();
                pokemons = JsonConvert.DeserializeObject<List<Pokemon>>(content);

                foreach (var pokemon in pokemons)
                {
                    Console.WriteLine($"{pokemon.Id} {pokemon.Name} {pokemon.Type}");
                }
            }

            client.Dispose();

            var upd = pokemons.First();

            client = new HttpClient();
            client.BaseAddress = url;

            result = await client.PostAsync(@"api/Post/" + upd.Id, new StringContent(JsonConvert.SerializeObject(upd)));

            if (result.IsSuccessStatusCode)
            {
                Console.WriteLine("Pokemon updated!");
            }

            client.Dispose();

            Console.ReadLine();
        }
    }

    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
