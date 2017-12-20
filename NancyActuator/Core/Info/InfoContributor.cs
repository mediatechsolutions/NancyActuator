using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NancyActuator.Core.Info
{
    /// <summary>
    /// Contributes additional info details.
    /// </summary>
    public interface IInfoContributor
    {
        /// <summary>
        /// Contributes additional details using the specified <see cref="Info.Builder"/>.
        /// </summary>
        /// <param name="builder">Build to use to create a new instance of info.</param>
        void Contribute(Info.Builder builder);
    }
}
