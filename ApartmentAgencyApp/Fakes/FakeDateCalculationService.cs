using ApartmentAgencyApp.Models;
using ApartmentAgencyApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentAgencyApp.Fakes
{
   
        public class FakeDateCalculationService : IDateCalculationService
        {
            public int NumberOfDaysToReturn { get; set; }
            public int NumberOfSeasonDaysToReturn { get; set; }

            public RequestDaysInfo GetDaysInfo(DateTime from, DateTime to)
            {
                return new RequestDaysInfo
                {
                    NumberOfDays = NumberOfDaysToReturn,
                    NumberOfSeasonDays = NumberOfSeasonDaysToReturn
                };
            }
        }
    }

