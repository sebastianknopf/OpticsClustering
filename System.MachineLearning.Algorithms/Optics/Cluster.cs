using System.Collections.Generic;

namespace System.MachineLearning.Optics
{
    public class Cluster
    {
        public int ID { get; set; }

        public List<UInt32> PointIds { get; set; }

        public List<double[]> Points { get; set; }


        public double Size { get; set; }
        public double[] Center { get; set; }
    }
}
