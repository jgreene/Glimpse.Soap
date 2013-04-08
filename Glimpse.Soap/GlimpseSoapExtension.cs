using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

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
                case SoapMessageStage.BeforeSerialize:
                    _result.RequestArgs = GetRequestArgs(message);
                    break;
                case SoapMessageStage.AfterSerialize:
                    _result.RequestXml = GetXml(_newStream);
                    
                    CopyStream(_newStream, _oldStream);
                    break;
                case SoapMessageStage.BeforeDeserialize:
                    CopyStream(_oldStream, _newStream);
                    _result.ResponseXml = GetXml(_newStream);
                    break;
                case SoapMessageStage.AfterDeserialize:
                    _result.ResponseResult = message.GetReturnValue();
                    _result.Duration = ((int)(DateTime.Now - _start).TotalMilliseconds).ToString() + "ms";
                    _result.Stacktrace = new StackTrace(4, true).ToString();
                    GlimpseManager.LogMessage(_result);
                    break;
            }
        }

        private static object[] GetRequestArgs(SoapMessage message)
        {
            var keepGoing = true;
            var i = 0;
            var objects = new List<object>();
            while (keepGoing)
            {
                try
                {
                    objects.Add(message.GetInParameterValue(i));
                    i++;
                }
                catch
                {
                    keepGoing = false;
                }
            }
            return objects.ToArray();
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
