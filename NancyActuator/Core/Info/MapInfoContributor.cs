using System.Collections.Generic;

namespace NancyActuator.Core.Info
{
    public class MapInfoContributor: IInfoContributor
    {
        private readonly Dictionary<string, object> info;

        public MapInfoContributor(Dictionary<string, object> info)
        {
            this.info = new Dictionary<string, object>(info);
        }

        public void Contribute(Info.Builder builder)
        {
            builder.WithDetails(this.info);
        }
    }
}
