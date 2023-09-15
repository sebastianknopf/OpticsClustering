using System.Collections.Generic;
using System.Linq;
using System.MachineLearning.Optics.PriorityQueue;

namespace System.MachineLearning.Optics
{
    public class OPTICS
    {
        public Metric Metric { get; set; } = Metric.Euclidian;

        private static PointRelationComparison pointComparison = new PointRelationComparison();

        private class PointRelationComparison : IComparer<PointRelation>
        {
            public int Compare(PointRelation x, PointRelation y)
            {
                if (x.Distance == y.Distance)
                {
                    return 0;
                }
                return x.Distance > y.Distance ? 1 : -1;
            }
        }

        internal struct PointRelation
        {
            public readonly UInt32 To;
            public readonly double Distance;

            public PointRelation(uint to, double distance)
            {
                this.To = to;
                this.Distance = distance;
            }
        }

        internal readonly Point[] _points;
        internal readonly double _eps;
        internal readonly int _minPts;
        internal readonly int _dimensions;
        internal readonly List<UInt32> _outputIndexes;
        internal readonly HeapPriorityQueue<Point> _seeds;

        internal readonly List<PointReachability> _reachabilities = new List<PointReachability>();

        private void AddOutputIndex(UInt32 index)
        {
            _outputIndexes.Add(index);
            if (_outputIndexes.Count % 250 == 0)
            {
                // TODO : add progress reporting interface
                // Console.WriteLine("Progress {0}/{1}", _outputIndexes.Count, _outputIndexes.Capacity);
            }
        }

        public OPTICS(double eps, int minPts, int dimensions, PointList points)
        {
            _eps = eps;
            _minPts = minPts;
            _dimensions = dimensions;
            _points = points._points.ToArray();

            _outputIndexes = new List<UInt32>(_points.Length);
            _seeds = new HeapPriorityQueue<Point>(_points.Length);
        }

        internal double EuclideanDistance(UInt32 p1Index, UInt32 p2Index)
        {
            double dist = 0;
            var vec1 = _points[p1Index].Vector;
            var vec2 = _points[p2Index].Vector;

            for (int i = 0; i < vec1.Length; i++)
            {
                var diff = (vec1[i] - vec2[i]);
                dist += diff * diff;
            }

            return Math.Sqrt(dist);
        }

        internal double ManhattanDistance(UInt32 p1Index, UInt32 p2Index)
        {
            double dist = 0;
            var vec1 = _points[p1Index].Vector;
            var vec2 = _points[p2Index].Vector;

            for (int i = 0; i < vec1.Length; i++)
            {
                var diff = Math.Abs(vec1[i] - vec2[i]);
                dist += diff;
            }

            return dist;
        }

        internal double HaversineDistance(UInt32 p1Index, UInt32 p2Index)
        {
            // check for vector dimensions
            var vec1 = _points[p1Index].Vector;
            var vec2 = _points[p2Index].Vector;

            if (vec1.Length != 2 || vec2.Length != 2)
            {
                return double.MaxValue;
            }

            // extract lat/lon data for haversine calculation
            double latitudeA = vec1[0];
            double longitudeA = vec1[1];
            double latitudeB = vec2[0];
            double longitudeB = vec2[1];

            // haversine calculation
            var mpi = Math.PI / 180;

            latitudeA *= mpi;
            longitudeA *= mpi;
            latitudeB *= mpi;
            longitudeB *= mpi;

            var deltaLat = latitudeB - latitudeA;
            var deltaLon = longitudeB - longitudeA;

            var a = Math.Sin(deltaLat / 2.0) * Math.Sin(deltaLat / 2.0) + Math.Cos(latitudeB) * Math.Sin(deltaLon / 2.0) * Math.Sin(deltaLon / 2.0);
            var km = 6372.797 * 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return km * 1000;
        }

        internal int SecondsDistance(UInt32 p1Index, UInt32 p2Index)
        {
            // check for vector dimensions
            DateTime dateTime1 = _points[p1Index].Timestamp;
            DateTime dateTime2 = _points[p2Index].Timestamp;

            if (dateTime1 == DateTime.MinValue || dateTime2 == DateTime.MinValue)
            {
                return int.MaxValue;
            }

            return (int) Math.Abs((dateTime2 - dateTime1).TotalSeconds);
        }

        internal virtual void GetNeighborhood(UInt32 p1Index, List<PointRelation> neighborhoodOut)
        {
            neighborhoodOut.Clear();

            for (UInt32 p2Index = 0; p2Index < _points.Length; p2Index++)
            {
                var distance = double.MaxValue;

                switch(this.Metric)
                {
                    case Metric.Euclidian:
                        distance = this.EuclideanDistance(p1Index, p2Index);
                        break;

                    case Metric.Manhattan:
                        distance = this.ManhattanDistance(p1Index, p2Index);
                        break;

                    case Metric.Haversine:
                        distance = this.HaversineDistance(p1Index, p2Index);
                        break;
                }

                if (distance <= _eps)
                {
                    neighborhoodOut.Add(new PointRelation(p2Index, distance));
                }
            }
        }

        private double CoreDistance(List<PointRelation> neighbors)
        {
            if (neighbors.Count < _minPts)
                return double.NaN;

            neighbors.Sort(pointComparison);
            return neighbors[_minPts-1].Distance;
        }

        private void Update(UInt32 pIndex, List<PointRelation> neighbors, double coreDistance)
        {
            for (int i = 0; i < neighbors.Count; i++)
            {
                UInt32 p2Index = neighbors[i].To;

                if (_points[p2Index].WasProcessed)
                    continue;

                double newReachabilityDistance = Math.Max(coreDistance, neighbors[i].Distance);

                if (double.IsNaN(_points[p2Index].ReachabilityDistance))
                {
                    _points[p2Index].ReachabilityDistance = newReachabilityDistance;
                    _seeds.Enqueue(_points[p2Index], newReachabilityDistance);
                }
                else if (newReachabilityDistance < _points[p2Index].ReachabilityDistance)
                {
                    _points[p2Index].ReachabilityDistance = newReachabilityDistance;
                    _seeds.UpdatePriority(_points[p2Index], newReachabilityDistance);
                }
            }
        }

        public void Predict()
        {
            for (UInt32 pIndex = 0; pIndex < _points.Length; pIndex++)
            {
                if (_points[pIndex].WasProcessed)
                    continue;

                List<PointRelation> neighborOfPoint = new List<PointRelation>();
                GetNeighborhood(pIndex, neighborOfPoint);

                _points[pIndex].WasProcessed = true;

                AddOutputIndex(pIndex);

                double coreDistance = CoreDistance(neighborOfPoint);

                if (!double.IsNaN(coreDistance))
                {
                    _seeds.Clear();
                    Update(pIndex, neighborOfPoint, coreDistance);

                    List<PointRelation> neighborInner = new List<PointRelation>();
                    while (_seeds.Count > 0)
                    {
                        UInt32 pInnerIndex = _seeds.Dequeue().Index;

                        GetNeighborhood(pInnerIndex, neighborInner);

                        _points[pInnerIndex].WasProcessed = true;

                        AddOutputIndex(pInnerIndex);

                        double coreDistanceInner = CoreDistance(neighborInner);

                        if (!double.IsNaN(coreDistanceInner))
                        {
                            Update(pInnerIndex, neighborInner, coreDistanceInner);
                        }
                    }
                }
            }

            // store point reachabilities
            foreach (var item in _outputIndexes)
            {
                this._reachabilities.Add(new PointReachability(_points[item].Id, _points[item].ReachabilityDistance, -1));
            }

            // apply clusters on ordered points
            Int32 clusterId = 0;
            bool lastWasClustered = true;

            for (int i = 1; i < this._reachabilities.Count; i++)
            {
                var previousItem = this._reachabilities[i - 1];
                var item = this._reachabilities[i];

                if (!double.IsNaN(item.Reachability) && item.Reachability < this._eps)
                {
                    previousItem.ClusterId = clusterId;
                    item.ClusterId = clusterId;

                    lastWasClustered = true;
                }
                else if (lastWasClustered)
                {
                    clusterId++;

                    lastWasClustered = false;
                }
            }
        }

        public List<PointReachability> GetPointReachability()
        {
            return this._reachabilities;
        }

        public List<Cluster> GetClusters()
        {            
            List<Cluster> clusters = new List<Cluster>();

            foreach(int clusterId in this._reachabilities.Where(r => r.ClusterId >= 0).OrderBy(r => r.ClusterId).Select(r => r.ClusterId).Distinct())
            {
                // select required data
                List<UInt32> clusterPointIds = this._reachabilities.Where(r => r.ClusterId == clusterId).Select(r => r.PointId).ToList();
                List<double[]> clusterPoints = this._points.Where(p => clusterPointIds.Contains(p.Id)).Select(p => p.Vector).ToList();

                // build cluster object
                Cluster cluster = new Cluster();
                cluster.ID = clusterId;

                cluster.PointIds = clusterPointIds;
                cluster.Points = clusterPoints;

                cluster.Size = this._reachabilities.Where(r => clusterPointIds.Contains(r.PointId)).Select(r => r.Reachability).Max();
                cluster.Center = Enumerable.Range(0, this._dimensions).Select(i => clusterPoints.Average(c => c[i])).ToArray();

                clusters.Add(cluster);
            }
            
            return clusters;
        }
    }
}
