using ApartmentAgencyApp.Exceptions;
using ApartmentAgencyApp.Models;
using ApartmentAgencyApp.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using ApartmentAgencyApp.Fakes;
namespace ApartmentAgencyApp.Test
{
    [TestFixture]
    public class ApartmentAgencyServiceTest
    {
        private FakeApartmentService fakeApartmentService; 
        private FakeDateCalculationService fakeDateCalculationService;
        private FakeReservationService fakeReservationService;
        private ApartmentAgencyService apartmentAgencyService;

        [SetUp]
        public void SetUp()
        {
            fakeApartmentService = new FakeApartmentService();
            fakeDateCalculationService = new FakeDateCalculationService();
            fakeReservationService = new FakeReservationService();
            apartmentAgencyService = new ApartmentAgencyService(fakeDateCalculationService, fakeApartmentService, fakeReservationService);
        }

        // Testcase putem PICT modela uveden za f3
        [TestCaseSource(typeof(PICTParser), nameof(PICTParser.GetTestCases))]
         public ApartmentRank CalculateApartmentRank_PICT(
            double distanceFromTheBeach,
            int percentOfPositiveReviews,
            ApartmentType apartmentType,
            bool renovatedInTheLastYear) 
        {
            return apartmentAgencyService.CalculateApartmentRank(
                distanceFromTheBeach,
                percentOfPositiveReviews,
                apartmentType,
                renovatedInTheLastYear);
        }

        // Pomoćna metoda za kreiranje apartmana sa Guid-om
        private Apartment CreateApartment(string guid = "56269134-6ef2-453d-b402-ca44f699e3a9")
        {
            return new Apartment { Id = Guid.Parse(guid) };
        }

        [Test]//kada baca exception kada nema slobodnih soba 
        public void MakeApartmentReservation_ExceptionThrown_WhenNoAvailableApartments()
        {
            ReservationRequest request = new ReservationRequest
            {
                Id = Guid.Parse("56269134-6ef2-453d-b402-ca44f699e3a9"),
                DateOfArrival = new DateTime(2024, 7, 1),
                DateOfDeparture = new DateTime(2024, 7, 10),
                ApartmentType = ApartmentType.Studio,
                DistanceFromTheBeach = 300,
                NumberOfBeds = 2
            };
            fakeApartmentService.ApartmentsToReturn.Clear();

            Assert.Throws<NoAvailableApartmentsException>((TestDelegate)(() =>
                apartmentAgencyService.MakeApartmentReservation(request)));
        }

        [Test]//kada baca exception kada servis nije pozvan 
        public void MakeApartmentReservation_ReservationServiceNotCalled_WhenNoAvailableApartments()
        {
            ReservationRequest request = new ReservationRequest
            {
                Id = Guid.Parse("56269134-6ef2-453d-b402-ca44f699e3a9"),
                DateOfArrival = new DateTime(2024, 7, 1),
                DateOfDeparture = new DateTime(2024, 7, 10),
                ApartmentType = ApartmentType.Studio,
                DistanceFromTheBeach = 300,
                NumberOfBeds = 2
            };
            fakeApartmentService.ApartmentsToReturn.Clear();

            try { apartmentAgencyService.MakeApartmentReservation(request); } catch { }

            Assert.That(fakeReservationService.WasCalled, Is.False);
        }

        [Test]//testiram ukoliko je broj lezaja jednak 3
        public void MakeApartmentReservation_BedOnly_CloseBeach_ManyBedsEqual3_ReservesComplexA()
        {
            fakeApartmentService.ApartmentsToReturn = new List<Apartment> { CreateApartment() };

            ReservationRequest request = new ReservationRequest
            {
                Id = Guid.Parse("56269134-6ef2-453d-b402-ca44f699e3a9"),
                DateOfArrival = new DateTime(2024, 7, 1),
                DateOfDeparture = new DateTime(2024, 7, 5),
                ApartmentType = ApartmentType.BedOnly,
                DistanceFromTheBeach = 300,
                NumberOfBeds = 3
            };

            apartmentAgencyService.MakeApartmentReservation(request);

            Assert.That(fakeReservationService.LastReservation.ApartmentComplex, Is.EqualTo(ApartmentComplex.ComplexA));
        }

        [Test]//testiram ukoliko je broj lezaja veci od 3
        public void MakeApartmentReservation_BedOnly_CloseBeach_ManyBedsGreaterThan3_ReservesComplexA()
        {
            fakeApartmentService.ApartmentsToReturn = new List<Apartment> { CreateApartment() };

            ReservationRequest request = new ReservationRequest
            {
                Id = Guid.Parse("56269134-6ef2-453d-b402-ca44f699e3a9"),
                DateOfArrival = new DateTime(2024, 7, 1),
                DateOfDeparture = new DateTime(2024, 7, 5),
                ApartmentType = ApartmentType.BedOnly,
                DistanceFromTheBeach = 300,
                NumberOfBeds = 5
            };

            apartmentAgencyService.MakeApartmentReservation(request);

            Assert.That(fakeReservationService.LastReservation.ApartmentComplex, Is.EqualTo(ApartmentComplex.ComplexA));
        }

        [Test]
        public void MakeApartmentReservation_BedOnly_CloseBeach_LessBeds2_ReservesComplexB()
        {
            fakeApartmentService.ApartmentsToReturn = new List<Apartment> { CreateApartment() };

            ReservationRequest request = new ReservationRequest
            {
                Id = Guid.Parse("56269134-6ef2-453d-b402-ca44f699e3a9"),
                DateOfArrival = new DateTime(2024, 7, 1),
                DateOfDeparture = new DateTime(2024, 7, 5),
                ApartmentType = ApartmentType.BedOnly,
                DistanceFromTheBeach = 200,
                NumberOfBeds = 2
            };

            apartmentAgencyService.MakeApartmentReservation(request);

            Assert.That(fakeReservationService.LastReservation.ApartmentComplex, Is.EqualTo(ApartmentComplex.ComplexB));
        }

        [Test]
        public void MakeApartmentReservation_Studio_LongStay_ReservesComplexB()
        {
           
            fakeDateCalculationService.NumberOfDaysToReturn = 7;
            fakeDateCalculationService.NumberOfSeasonDaysToReturn = 0;
            fakeApartmentService.ApartmentsToReturn = new List<Apartment> { CreateApartment() };

            ReservationRequest request = new ReservationRequest
            {
                Id = Guid.Parse("56269134-6ef2-453d-b402-ca44f699e3a9"),
                DateOfArrival = new DateTime(2024, 7, 1),
                DateOfDeparture = new DateTime(2024, 7, 8),
                ApartmentType = ApartmentType.Studio,
                DistanceFromTheBeach = 1000,
                NumberOfBeds = 1
            };

            apartmentAgencyService.MakeApartmentReservation(request);

            Assert.That(fakeReservationService.LastReservation.ApartmentComplex, Is.EqualTo(ApartmentComplex.ComplexB));
        }

        [Test]
        public void MakeApartmentReservation_Studio_LongStayEqual5_ReservesComplexB()
        {
         
            fakeDateCalculationService.NumberOfDaysToReturn = 5;
            fakeDateCalculationService.NumberOfSeasonDaysToReturn = 0;
            fakeApartmentService.ApartmentsToReturn = new List<Apartment> { CreateApartment() };

            ReservationRequest request = new ReservationRequest
            {
                Id = Guid.Parse("56269134-6ef2-453d-b402-ca44f699e3a9"),
                DateOfArrival = new DateTime(2024, 7, 1),
                DateOfDeparture = new DateTime(2024, 7, 6), 
                ApartmentType = ApartmentType.Studio,
                DistanceFromTheBeach = 1000,
                NumberOfBeds = 1
            };

            apartmentAgencyService.MakeApartmentReservation(request);

            Assert.That(fakeReservationService.LastReservation.ApartmentComplex, Is.EqualTo(ApartmentComplex.ComplexB));
        }

        [Test]//kompleks C kada je broj odsedanja manji od 5 i u letnjoj sezoni da je 2
        public void MakeApartmentReservation_Studio_StayLessThan5_ReservesComplexC()
        {

            fakeDateCalculationService.NumberOfDaysToReturn = 4;
            fakeDateCalculationService.NumberOfSeasonDaysToReturn = 0;
            fakeApartmentService.ApartmentsToReturn = new List<Apartment> { CreateApartment() };

            ReservationRequest request = new ReservationRequest
            {
                Id = Guid.Parse("56269134-6ef2-453d-b402-ca44f699e3a9"),
                DateOfArrival = new DateTime(2024, 7, 1),
                DateOfDeparture = new DateTime(2024, 7, 2),
                ApartmentType = ApartmentType.Studio,
                DistanceFromTheBeach = 1000,
                NumberOfBeds = 1
            };

            apartmentAgencyService.MakeApartmentReservation(request);

            Assert.That(fakeReservationService.LastReservation.ApartmentComplex, Is.EqualTo(ApartmentComplex.ComplexC));
        }
        [Test]//kompleks C kada je broj odsedanja manji od 5 i u letnjoj sezoni da je 1
        public void MakeApartmentReservation_Studio_StayLessThan5_ReservesComplexC1()
        {

            fakeDateCalculationService.NumberOfDaysToReturn = 4;
            fakeDateCalculationService.NumberOfSeasonDaysToReturn = 0;
            fakeApartmentService.ApartmentsToReturn = new List<Apartment> { CreateApartment() };

            ReservationRequest request = new ReservationRequest
            {
                Id = Guid.Parse("56269134-6ef2-453d-b402-ca44f699e3a9"),
                DateOfArrival = new DateTime(2024, 7, 1),
                DateOfDeparture = new DateTime(2024, 7, 1),
                ApartmentType = ApartmentType.Studio,
                DistanceFromTheBeach = 1000,
                NumberOfBeds = 1
            };

            apartmentAgencyService.MakeApartmentReservation(request);

            Assert.That(fakeReservationService.LastReservation.ApartmentComplex, Is.EqualTo(ApartmentComplex.ComplexC));
        }


        [Test]//Komplex D
        public void MakeApartmentReservation_OtherApartmentType_ReservesComplexD()
        {
          
            fakeApartmentService.ApartmentsToReturn = new List<Apartment> { CreateApartment() };

            ReservationRequest request = new ReservationRequest
            {
                Id = Guid.Parse("56269134-6ef2-453d-b402-ca44f699e3a9"),
                DateOfArrival = new DateTime(2024, 7, 1),
                DateOfDeparture = new DateTime(2024, 7, 5),
                ApartmentType = ApartmentType.StudioWithTerrace,//izabrana terasa zbog preskakanja oba if uslova
                DistanceFromTheBeach = 300,
                NumberOfBeds = 2
            };

    
            apartmentAgencyService.MakeApartmentReservation(request);

    
            Assert.That(fakeReservationService.LastReservation.ApartmentComplex,
                Is.EqualTo(ApartmentComplex.ComplexD));
        }


    }
}