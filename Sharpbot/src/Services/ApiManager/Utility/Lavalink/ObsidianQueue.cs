using System;
using System.Collections.Generic;
using System.Linq;
using Victoria;

namespace Sharpbot.Services.ApiManager.Utility.Lavalink
{
    public static class ObsidianQueue
    {
        public static List<LavaTrack> GetTop(this DefaultQueue<LavaTrack> queue)
        {
            var top = new List<LavaTrack>();
            var enumQueue = queue.ToList();
            var i = 0;
            foreach (var track in enumQueue)
            {
                var q = track as LavaTrack;
                if (i >= 4)
                    break;
                top.Add(track as LavaTrack);
                i++;
            }

            return top;
        }

        public static TimeSpan GetLength(this DefaultQueue<LavaTrack> queue)
        {
            var top = new List<LavaTrack>();
            var length = new TimeSpan();
            var enumQueue = queue.ToList();

            return enumQueue.Aggregate(length, (current, track) => current.Add((track as LavaTrack).Duration));

        }
    }
}
