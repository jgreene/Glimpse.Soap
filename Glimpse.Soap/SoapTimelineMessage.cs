using System;
using Glimpse.Core.Message;

namespace Glimpse.Soap
{
	internal class SoapTimelineMessage : ITimelineMessage
	{
		public SoapTimelineMessage()
		{
			Id = Guid.NewGuid();
		}

		public Guid Id { get; private set; }
		public TimeSpan Offset { get; set; }
		public TimeSpan Duration { get; set; }
		public DateTime StartTime { get; set; }
		public string EventName { get; set; }
		public TimelineCategoryItem EventCategory { get; set; }
		public string EventSubText { get; set; }
	}
}