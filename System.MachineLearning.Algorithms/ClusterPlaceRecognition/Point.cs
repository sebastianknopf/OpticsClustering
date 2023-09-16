namespace System.MachineLearning.ClusterPlaceRecognition
{
    public class Point
    {
        public readonly int ID;
        public readonly DateTime Timestamp;
        public readonly double Latitude;
        public readonly double Longitude;

        public Point(int id, double latitude, double longitude)
        {
            this.ID = id;
            this.Timestamp = DateTime.MinValue;
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        public Point(int id, DateTime timestamp, double latitude, double longitude)
        {
            this.ID = id;
            this.Timestamp = timestamp;
            this.Latitude = latitude;
            this.Longitude = longitude;
        }
    }
}
