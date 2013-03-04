using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacManShared.Util.TimeStamps
{
    public class TimeStampManager
    {
        private LinkedList<TimeStamp> timeStamps;
        private int max;

        public TimeStampManager(int max)
        {
            this.max = max;
            this.timeStamps = new LinkedList<TimeStamp>();
        }

        public TimeStampManager():this(100){}

        public void Push(TimeStamp timeStamp)
        {
            if(timeStamps.Count > max)
            {
                timeStamps.RemoveLast();
            }

            timeStamps.AddFirst(timeStamp);
        }

        public TimeStamp Pop()
        {

                TimeStamp timeStamp = timeStamps.First.Value;
                timeStamps.RemoveFirst();

                return timeStamp;
            
        }

        public TimeStamp Peek()
        {
            return timeStamps.First.Value;
        }

        public void AddFirstBefore(TimeStamp timeStamp, int time)
        {
            this.BatchPop(time);
            Push(timeStamp);
        }

        public TimeStamp BatchPop(int time)
        {
            TimeStamp timeStamp = this.Pop();

            while(timeStamp.time <= time)
            {
                timeStamp = this.Pop();
            }

            return timeStamp;
        }

        public TimeStamp ElementAt(int index)
        {
            return timeStamps.ElementAt(index);
        }

        public TimeStamp ReplaceTimeStamp(TimeStamp timeStamp, int time)
        {
            LinkedListNode<TimeStamp> toReplace = FindTimeStamp(time);

            if(toReplace != null)
            {
                timeStamps.AddBefore(toReplace, timeStamp);

                if (timeStamps.Remove(toReplace.Value))
                {
                    return toReplace.Value;
                }
            }
            

            throw new Exception("Could not replace timestamp");

        }

        public LinkedListNode<TimeStamp> FindTimeStamp(int time)
        {
            LinkedListNode<TimeStamp> timeStamp = timeStamps.First;
            int index = 0;

            while (timeStamp.Value.time <= time)
            {
                index++;
                timeStamp = new LinkedListNode<TimeStamp>(timeStamps.ElementAt(index));
            }

            return timeStamp;
        }

        public void SwapFirstTimeStamp(TimeStamp timeStamp)
        {
            if(timeStamps.Count > 0)
            {
                TimeStamp returnStamp = timeStamps.First.Value;
                timeStamps.RemoveFirst();


            }

            timeStamps.AddFirst(timeStamp);
        }
    }
}
