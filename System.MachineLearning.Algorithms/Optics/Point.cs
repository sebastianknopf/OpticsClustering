namespace System.MachineLearning.Optics
{
    internal class Point : PriorityQueueNode
    {
        public Point(UInt32 index, UInt32 id, double[] vector)
        {
            Index = index;
            Id = id;
            Timestamp = DateTime.MinValue;
            Vector = vector;

            WasProcessed = false;
            ReachabilityDistance = double.NaN;
        }

        public Point(UInt32 index, UInt32 id, DateTime timestamp, double[] vector)
        {
            Index = index;
            Id = id;
            Timestamp = timestamp;
            Vector = vector;

            WasProcessed = false;
            ReachabilityDistance = double.NaN;
        }

        public readonly UInt32 Id;
        public readonly DateTime Timestamp;
        public readonly double[] Vector;
        public readonly UInt32 Index;

        internal double ReachabilityDistance;
        internal bool WasProcessed;
    }
}
