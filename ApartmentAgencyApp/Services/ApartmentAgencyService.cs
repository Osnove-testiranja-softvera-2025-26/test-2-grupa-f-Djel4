
using ApartmentAgencyApp.Exceptions;
using ApartmentAgencyApp.Models;
using System.Collections.Generic;

namespace ApartmentAgencyApp.Services
{
    public class ApartmentAgencyService
    {
        private readonly IDateCalculationService _dateCalculationService;
        private readonly IApartmentService _apartmentService;
        private readonly IReservationService _reservatonService;


        public void MakeApartmentReservation(ReservationRequest request)
        {
            RequestDaysInfo daysInfo = _dateCalculationService.GetDaysInfo(request.DateOfArrival, request.DateOfDeparture);
            ApartmentComplex complex;

            if (request.ApartmentType.Equals(ApartmentType.BedOnly) && request.DistanceFromTheBeach < 500) 
            {
                if (request.NumberOfBeds >= 3) 
                {
                    complex = ApartmentComplex.ComplexA;
                }
                else
                {
                    complex = ApartmentComplex.ComplexB;
                }
            }
            else if (request.ApartmentType.Equals(ApartmentType.Studio)) 
            {
                if (daysInfo.NumberOfDays >= 5 || daysInfo.NumberOfSeasonDays > 2) 
                {
                    complex = ApartmentComplex.ComplexB;
                }
                else
                {
                    complex = ApartmentComplex.ComplexC;
                }
            }
            else
            {
                complex = ApartmentComplex.ComplexD;
            }

            List<Apartment> availableApartments = _apartmentService.GetAvailableApartments(request);
            if(availableApartments.Count == 0)
            {
                throw new NoAvailableApartmentsException("Cannot make a reservation");
            }
            _reservatonService.MakeReservationInComplex(new Reservation { ApartmentId = availableApartments[0].Id, ReservationRequestId = request.Id, ApartmentComplex = complex });
        }

        public ApartmentRank CalculateApartmentRank(double distanceFromTheBeach, int percentOfPositiveReviews, ApartmentType apartmentType, bool renovatedInTheLastYear)
        {
            if (apartmentType.Equals(ApartmentType.Studio))
            {
                if (percentOfPositiveReviews > 80 && distanceFromTheBeach <= 500.0)
                {
                    if (renovatedInTheLastYear)
                        return ApartmentRank.First;
                    return ApartmentRank.Second;
                }
                return ApartmentRank.Third;
            }
            else if (apartmentType.Equals(ApartmentType.StudioWithTerrace))
            {
                if (percentOfPositiveReviews <= 70 || distanceFromTheBeach > 1500.0)
                    return ApartmentRank.Second;
                return ApartmentRank.First;
            }

            return ApartmentRank.Forth;
        }
    }
}
