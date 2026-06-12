using ApartmentAgencyApp.Models;
using ApartmentAgencyApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentAgencyApp.Fakes
{
        public class FakeReservationService : IReservationService
        {
            public bool WasCalled { get; set; }
            public Reservation LastReservation { get; set; }

            public void MakeReservationInComplex(Reservation reservation)
            {
                WasCalled = true;
                LastReservation = reservation;
            }
        }
}
