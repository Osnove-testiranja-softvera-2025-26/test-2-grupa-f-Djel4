using ApartmentAgencyApp.Models;



namespace ApartmentAgencyApp.Services
{
    public interface IReservationService
    {
        void MakeReservationInComplex(Reservation reservation);
    }
}
