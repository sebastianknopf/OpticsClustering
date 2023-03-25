using System;

namespace System.Windows.MachineLearning.Optics
{
    public class PointReachability
    {
        public PointReachability(UInt32 pointId, double reachability, Int32 clusterId)
        {
            PointId = pointId;
            Reachability = reachability;
            ClusterId = clusterId;
        }

        public readonly UInt32 PointId;
        public readonly double Reachability;
        public Int32 ClusterId;
    }
}
