using ApartmentAgencyApp.Models;
using ApartmentAgencyApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentAgencyApp.Fakes
{
  
        public class FakeApartmentService : IApartmentService
        {

            public ReservationRequest LastRequest { get; set; }
            public List<Apartment> ApartmentsToReturn { get; set; } = new List<Apartment>();

            public List<Apartment> GetAvailableApartments(ReservationRequest request)
            {
                LastRequest = request;
                return ApartmentsToReturn;
            }
        }
    
}
