

using ApartmentAgencyApp.Models;
using System.Collections.Generic;

namespace ApartmentAgencyApp.Services
{
    public interface IApartmentService
    {
        List<Apartment> GetAvailableApartments(ReservationRequest request);
    }
}
