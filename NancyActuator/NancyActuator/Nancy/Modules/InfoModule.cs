using System.Collections.Generic;
using System.Linq;
using Nancy;
using NancyActuator.Core.Info;

namespace NancyActuator.Nancy.Modules
{
    public class InfoModule : NancyModule
    {
        public InfoModule(IEnumerable<IInfoContributor> infoContributors)
        {
            Get["/info"] = parameters =>
            {
                var infoBuilder = new Info.Builder();
                foreach (var infoContributor in infoContributors)
                    infoContributor.Contribute(infoBuilder);

                return infoBuilder.Build();
            };
        }
    }
}