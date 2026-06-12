

using System;

namespace ApartmentAgencyApp.Models
{
    public class ReservationRequest
    {
        public Guid Id { get; set; }
        public DateTime DateOfArrival { get; set; }
        public DateTime DateOfDeparture { get; set; }
        public double DistanceFromTheBeach { get; set; }
        public int NumberOfBeds { get; set; }
        public ApartmentType ApartmentType { get; set; }
    }
}
