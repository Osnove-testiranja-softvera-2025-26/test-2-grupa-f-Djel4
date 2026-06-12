

using ApartmentAgencyApp.Models;
using System;

namespace ApartmentAgencyApp.Services
{
    public interface IDateCalculationService
    {
        RequestDaysInfo GetDaysInfo(DateTime from, DateTime to);
    }
}
