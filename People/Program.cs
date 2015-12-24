using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

using Newtonsoft.Json;

namespace People
{
    class Program
    {
        private static readonly string peopleUrl = ConfigurationManager.AppSettings["PeopleUrl"];

        static void Main(string[] args)
        {
            HttpClient httpClient = new HttpClient();

            // Load JSON
            string peopleJson = httpClient.GetStringAsync(peopleUrl).Result;

            var people = JsonConvert.DeserializeObject<Person[]>(peopleJson);

            // Get lists of cats by gender of owner
            var catsByGender = (from person in people
                                where person.Pets != null
                                from pet in person.Pets
                                where pet.Type == "Cat"
                                group pet by person.Gender into g
                                select new { Gender = g.Key, Pets = g.OrderBy(p => p.Name).ToList() }).ToList();

            // Output cats by gender to the console
            foreach (var group in catsByGender)
            {
                Console.WriteLine(group.Gender);

                foreach (var pet in group.Pets.OrderBy(p => p.Name))
                {
                    Console.WriteLine("- " + pet.Name);
                }
            }
        }
    }
}
