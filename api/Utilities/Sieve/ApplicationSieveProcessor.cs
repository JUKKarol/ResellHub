using Microsoft.Extensions.Options;
using ResellHub.Entities;
using Sieve.Models;
using Sieve.Services;

namespace ResellHub.Utilities.Sieve
{
    public class ApplicationSieveProcessor : SieveProcessor
    {
        public ApplicationSieveProcessor(IOptions<SieveOptions> options) : base(options)
        {
        }

        protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
        {
            mapper.Property<Offer>(o => o.Title)
                .CanFilter();

            mapper.Property<Offer>(o => o.Brand)
                .CanFilter();

            mapper.Property<Offer>(o => o.CategoryId)
                .CanFilter();

            mapper.Property<Offer>(o => o.Condition)
                .CanSort()
                .CanFilter();

            mapper.Property<Offer>(o => o.Price)
                .CanSort()
                .CanFilter();

            mapper.Property<Offer>(o => o.Currency)
                .CanSort()
                .CanFilter();

            mapper.Property<Offer>(o => o.ProductionYear)
               .CanSort()
               .CanFilter();

            mapper.Property<Offer>(o => o.CreatedDate)
               .CanSort()
               .CanFilter();

            return mapper;
        }
    }
}