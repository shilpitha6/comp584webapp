using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace comp584webapp.DTO
{
    public class cityDTO
    {
        
        public int Id { get; set; }

        
        public string Name { get; set; } = null!;

        
        public double Lat { get; set; }

        
        public double Long { get; set; }

        
        public int Population { get; set; }
        public required string CountryName { get; set; }
    }
}
