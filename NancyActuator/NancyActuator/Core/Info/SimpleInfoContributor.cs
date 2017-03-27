using System;

namespace NancyActuator.Core.Info
{
    public class SimpleInfoContributor: IInfoContributor
    {
        private readonly string prefix;
    	private readonly object detail;

	    public SimpleInfoContributor(string prefix, object detail)
        {
            if (prefix == null)
                throw new ArgumentNullException("prefix");
            this.prefix = prefix;
            this.detail = detail;
        }
        
        public void Contribute(Info.Builder builder)
        {
            if (this.detail != null)
            {
                builder.WithDetail(this.prefix, this.detail);
            }
        }
    }
}
