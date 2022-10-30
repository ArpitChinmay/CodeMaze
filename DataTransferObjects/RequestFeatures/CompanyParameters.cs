

namespace Shared.RequestFeatures
{
    public class CompanyParameters : RequestParameters
    {
        public string? country;

        public string Country
        {
            get { return country; }

            set
            {
                List<string> availableCountry = new List<string> { "india", "usa", "italy", "3" };
                if (availableCountry.Contains(value.ToLower()))
                    this.country = value;
                /*
                 * This needs to be moved someplace else.
                 */
                //else 
                //    throw new NotValidCountryBadRequestException();
            }
        }

        public string? SearchTerm { get; set; }

        public CompanyParameters() => OrderBy = "Name";
    }
}
