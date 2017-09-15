using System;
using System.Collections.Concurrent;

namespace SmartVideoHub
{
    public class DetectionResultQueue
    {
		private const int MAX_DEPTH = 5;

		private static DetectionResultQueue _instance;
		public static DetectionResultQueue Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new DetectionResultQueue();
				}
				return _instance;
			}
		}

        private ConcurrentQueue<DetectionResult> queue;
		private object lockObject = new object();

		public DetectionResultQueue()
        {
            queue = new ConcurrentQueue<DetectionResult>();
        }

		public void Enqueue(DetectionResult media)
		{
			lock (lockObject)
			{
				if (queue.Count > MAX_DEPTH)
				{
                    Console.WriteLine("Detection Result Queue Overflow.");
					return;
				}
			}
			queue.Enqueue(media);
		}

		public DetectionResult Dequeue()
		{
			DetectionResult result;
			if (queue.TryDequeue(out result))
			{
				return result;
			}
			return null;
		}
	}
}
