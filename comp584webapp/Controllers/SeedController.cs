﻿using comp584webapp.Data;
using CsvHelper;
using CsvHelper.Configuration;
using datamodel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;

namespace comp584webapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController(MycitiesContext db,IHostEnvironment environment,
        UserManager<AppUser> userManager) : ControllerBase
    {
        private readonly string _pathName = Path.Combine(environment.ContentRootPath, "./Data/worldcities.csv");
        [HttpPost ("Countries")]
        
        public async Task<IActionResult> ImportCountiriesAsync()
        {
            // create a lookup dictionary containing all the countries already existing 
            // into the Database (it will be empty on first run).
            Dictionary<string, Country> countriesByName = db.Countries
                .AsNoTracking().ToDictionary(x => x.Name, StringComparer.OrdinalIgnoreCase);

            CsvConfiguration config = new(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                HeaderValidated = null
            };

            using StreamReader reader = new(_pathName);
            using CsvReader csv = new(reader, config);

            List<Worldcities> records = csv.GetRecords<Worldcities>().ToList();
            foreach (Worldcities record in records)
            {
                if (countriesByName.ContainsKey(record.country))
                {
                    continue;
                }

                Country country = new()
                {
                    Name = record.country,
                    Iso2 = record.iso2,
                    Iso3 = record.iso3
                };
                await db.Countries.AddAsync(country);
                countriesByName.Add(record.country, country);
            }

            await db.SaveChangesAsync();

            return new JsonResult(countriesByName.Count);

        }

        [HttpPost("Cities")]
        public async Task<IActionResult> ImportCitiesAsync()
        {
            Dictionary<string, Country> countries = await db.Countries//.AsNoTracking()
            .ToDictionaryAsync(c => c.Name);

            CsvConfiguration config = new(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                HeaderValidated = null
            };
            int cityCount = 0;
            using (StreamReader reader = new(_pathName))
            using (CsvReader csv = new(reader, config))
            {
                IEnumerable<Worldcities>? records = csv.GetRecords<Worldcities>();
                foreach (Worldcities record in records)
                {
                    if (!countries.TryGetValue(record.country, out Country? value))
                    {
                        Console.WriteLine($"Not found country for {record.city}");
                        return NotFound(record);
                    }

                    if (!record.population.HasValue || string.IsNullOrEmpty(record.city_ascii))
                    {
                        Console.WriteLine($"Skipping {record.city}");
                        continue;
                    }
                    City city = new()
                    {
                        Name = record.city,
                        Lat = record.lat,
                        Long = record.lng,
                        Population = (int)record.population.Value,
                        CountryId = value.Id
                    };
                    db.Cities.Add(city);
                    cityCount++;
                }
                await db.SaveChangesAsync();
            }
            return new JsonResult(cityCount);

        }
        [HttpPost("Users")]
        public async Task<IActionResult> ImportUsersAsync() {
            (string name, string email) = ("nick", "nick66@example.com");
            AppUser user = new AppUser()
            {
                UserName = name,
                Email = email,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            if (await userManager.FindByEmailAsync(email) is not null)
            {

                return Ok(user);
            }

            IdentityResult dbUser = await userManager.CreateAsync(user, "Ahdjg_12");

            user.EmailConfirmed = true;
            user.LockoutEnabled = false;
            await db.SaveChangesAsync();
            return Ok(user);
        } 
    }
}
