using System.Collections.Generic;

namespace System.MachineLearning.ClusterPlaceRecognition
{
    public class Cluster
    { 
        public int ID { get; set; }

        public List<int> PointIds { get; set; }

        public List<Point> Points { get; set; }

        public double Size { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
