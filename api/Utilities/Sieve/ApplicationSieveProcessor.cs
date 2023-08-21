using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;

namespace ResellHub.Utilities.Sieve
{
    public class ApplicationSieveProcessor : SieveProcessor
    {
        public ApplicationSieveProcessor(IOptions<SieveOptions> options) : base(options)
        {
        }
    }
}
