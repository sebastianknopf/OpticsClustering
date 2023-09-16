using System.Collections.Generic;
using System.Linq;

namespace System.MachineLearning.ClusterPlaceRecognition
{
    public class ClusterPlaceRecognition
    {
        protected double minDurationSeconds;
        protected double maxRadius;

        protected PointList pointList;

        protected List<Cluster> clusterList;

        public ClusterPlaceRecognition(double minDurationSeconds, double maxRadius, PointList pointList)
        {
            this.minDurationSeconds = minDurationSeconds;
            this.maxRadius = maxRadius;
            this.pointList = pointList;

            this.clusterList = new List<Cluster>();
        }

        internal double HaversineDistance(Point point1, Point point2)
        {
            // extract lat/lon data for haversine calculation
            double latitudeA = point1.Latitude;
            double longitudeA = point1.Longitude;
            double latitudeB = point2.Latitude;
            double longitudeB = point2.Longitude;

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

        public void Predict()
        {
            int clusterCount = 0;
            while (this.pointList.Points.Count > 0)
            {
                Point point = this.pointList.Points[0];
                
                DateTime minTimestamp = point.Timestamp;
                DateTime maxTimestamp = point.Timestamp.AddSeconds(this.minDurationSeconds);

                bool clusterBroken = false;
                List<Point> possibleClusterPoints = this.pointList.Points.Where(p => p.Timestamp.CompareTo(minTimestamp) >= 0 && p.Timestamp.CompareTo(maxTimestamp) <= 0).ToList();
                foreach(Point candidate in possibleClusterPoints)
                {
                    if (this.HaversineDistance(point, candidate) > this.maxRadius)
                    {
                        clusterBroken = true;
                        break;
                    }
                }

                if (!clusterBroken)
                {
                    Cluster cluster = new Cluster();
                    cluster.ID = clusterCount++;
                    cluster.PointIds = possibleClusterPoints.Select(p => p.ID).ToList();
                    cluster.Points = possibleClusterPoints;

                    cluster.Latitude = possibleClusterPoints.Select(p => p.Latitude).Average();
                    cluster.Longitude = possibleClusterPoints.Select(p => p.Longitude).Average();

                    this.clusterList.Add(cluster);

                    this.pointList.Points.RemoveAll(p => p.Timestamp.CompareTo(maxTimestamp) <= 0);
                }
                else
                {
                    this.pointList.Points.RemoveAll(p => p.ID == point.ID);
                }
            }
        }

        public List<Cluster> GetClusters()
        {
            return this.clusterList;
        }
    }
}
