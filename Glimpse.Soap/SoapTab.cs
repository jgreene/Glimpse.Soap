using Glimpse.AspNet.Extensibility;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using System;
using System.Linq;
namespace Glimpse.Soap
{
    public class SoapTab : AspNetTab, ITabSetup
    {
        public override string Name { get { return "Soap"; } }

        public override object GetData(ITabContext context)
        {
            return context.GetMessages<SoapResult>().ToArray();
        }
        public void Setup(ITabSetupContext context)
        {
            context.PersistMessages<SoapResult>();
        }
    }
}
