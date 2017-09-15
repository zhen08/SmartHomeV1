using System;
using System.Collections.Concurrent;
using SmartVideo.Transport;

namespace SmartVideoHub
{
    public class MediaQueue
    {
        private const int MAX_DEPTH = 5;

        private static MediaQueue _instance;
        public static MediaQueue Instance {
            get {
                if (_instance == null) {
                    _instance = new MediaQueue();
                }
                return _instance;
            }
        }

        private ConcurrentQueue<MediaStruct> queue;
        private object lockObject = new object();

        public MediaQueue()
        {
            queue = new ConcurrentQueue<MediaStruct>();
        }

        public void Enqueue(MediaStruct media)
        {
            lock(lockObject) {
                if (queue.Count>MAX_DEPTH) {
					Console.WriteLine("Media Queue Overflow.");
					return;
                }
            }
            queue.Enqueue(media);
        }

        public MediaStruct Dequeue()
        {
            MediaStruct result;
            if (queue.TryDequeue(out result)) {
                return result;
            }
            return null;
        }
    }
}
