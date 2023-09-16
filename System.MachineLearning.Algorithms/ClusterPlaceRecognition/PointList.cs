using System.Collections.Generic;

namespace System.MachineLearning.ClusterPlaceRecognition
{
    public class PointList
    {
        internal List<Point> Points { get; set; } = new List<Point>();

        public void AddPoint(int id, double latitude, double longitude)
        {
            this.Points.Add(new Point(id, latitude, longitude));
        }

        public void AddPoint(int id, DateTime timestamp, double latitude, double longitude)
        {
            this.Points.Add(new Point(id, timestamp, latitude, longitude));
        }
    }
}
