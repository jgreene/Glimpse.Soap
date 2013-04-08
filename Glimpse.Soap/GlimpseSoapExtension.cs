using System;
using System.IO;
using System.Web.Services.Protocols;
using System.Xml.Linq;

namespace Glimpse.Soap
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class GlimpseSoapExtensionAttribute : SoapExtensionAttribute
    {
        public override Type ExtensionType
        {
            get
            {
                return typeof(GlimpseSoapExtension);
            }
        }
        public override int Priority
        {
            get;
            set;
        }
    }

    public class GlimpseSoapExtension : SoapExtension
    {
        #region ignored
        public override object GetInitializer(Type serviceType)
        {
            return null;
        }
        public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute)
        {
            return null;
        }
        public override void Initialize(object initializer)
        {
        }
        #endregion

        private readonly DateTime _start = DateTime.Now;
        private Stream _oldStream;
        private Stream _newStream;
        private readonly SoapResult _result = new SoapResult();

        public override void ProcessMessage(SoapMessage message)
        {
            if (GlimpseManager.IsGlimpseActive() == false)
                return;

            _result.Url = message.Url;
            _result.Method = message.MethodInfo.Name;

            switch (message.Stage)
            {
                case SoapMessageStage.AfterSerialize:
                    _result.Request = GetXml(_newStream);
                    CopyStream(_newStream, _oldStream);
                    break;
                case SoapMessageStage.BeforeDeserialize:
                    CopyStream(_oldStream, _newStream);
                    _result.Response = GetXml(_newStream);
                    _result.Time = DateTime.Now - _start;
                    GlimpseManager.LogMessage(_result);
                    break;
            }
        }
        private static void CopyStream(Stream from, Stream to)
        {
            var buffer = new byte[4096];
            int count;
            while ((count = from.Read(buffer, 0, 4096)) > 0)
            {
                to.Write(buffer, 0, count);
            }
        }
        public override Stream ChainStream(Stream stream)
        {
            if (GlimpseManager.IsGlimpseActive() == false)
                return stream;

            _oldStream = stream;
            _newStream = new MemoryStream();
            return _newStream;
        }
        private static string GetXml(Stream stream)
        {
            try
            {
                stream.Position = 0;
                return XDocument.Parse(new StreamReader(stream).ReadToEnd()).ToString();
            }
            finally
            {
                stream.Position = 0;
            }
        }
    }
}
