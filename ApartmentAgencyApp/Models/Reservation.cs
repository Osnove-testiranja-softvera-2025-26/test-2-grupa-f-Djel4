

using System;

namespace ApartmentAgencyApp.Models
{
    public class Reservation
    {
        public Guid ReservationRequestId { get; set; }
        public Guid ApartmentId { get; set; }
        public ApartmentComplex ApartmentComplex {get; set;}

    }
}
