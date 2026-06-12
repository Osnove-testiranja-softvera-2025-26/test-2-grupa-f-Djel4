using System;


namespace ApartmentAgencyApp.Models
{
    public class Apartment
    {
        public Guid Id { get; set; }
        public ApartmentType Type { get; set; }
        public int NumberOfBeds { get; set; }
        public double PricePerNight { get; set; }
    }
}
