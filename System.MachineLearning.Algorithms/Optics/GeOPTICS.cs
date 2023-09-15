using System.Collections.Generic;

namespace System.MachineLearning.Optics
{
    public class GeOPTICS : OPTICS
    {
        public GeOPTICS(double eps, int minPts, PointList points) : base(eps, minPts, 2, points)
        {
        }

        internal override void GetNeighborhood(UInt32 p1Index, List<PointRelation> neighborhoodOut)
        {
            neighborhoodOut.Clear();

            for (UInt32 p2Index = 0; p2Index < _points.Length; p2Index++)
            {
                var distance = this.HaversineDistance(p1Index, p2Index);

                if (distance <= _eps)
                {
                    neighborhoodOut.Add(new PointRelation(p2Index, distance));
                }
            }
        }
    }
}
