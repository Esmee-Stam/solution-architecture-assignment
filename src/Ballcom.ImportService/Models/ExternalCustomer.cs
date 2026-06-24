using CsvHelper.Configuration.Attributes;

namespace Ballcom.ImportService.Models
{
    public class ExternalCustomer
    {
        [Name("Company Name")]
        public required string CompanyName { get; set; }

        [Name("First Name")]
        public required string FirstName { get; set; }

        [Name("Last Name")]
        public required string LastName { get; set; }

        [Name("Phone Number")]
        public required string PhoneNumber { get; set; }

        [Name("Address")]
        public required string Address { get; set; }
    }
}
