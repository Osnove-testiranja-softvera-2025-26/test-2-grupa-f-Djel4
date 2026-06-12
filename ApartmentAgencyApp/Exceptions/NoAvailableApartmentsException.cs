using System;

namespace ApartmentAgencyApp.Exceptions
{
    public class NoAvailableApartmentsException: Exception
    {
        public NoAvailableApartmentsException()
        {

        }

        public NoAvailableApartmentsException(string message): base(message)
        {

        }
    }
}
