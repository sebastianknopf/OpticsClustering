using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clustering.Optics.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            DemoHaversine();
        }

        private static void DemoHaversine()
        {
            Clustering.Optics.PointsList points = new Clustering.Optics.PointsList();

            // create random clusters with random points
            points.AddPoint(0, new double[] { 48.89278443239984, 8.700974198578608 });
            points.AddPoint(1, new double[] { 48.89276513743977, 8.701028063869876 });
            points.AddPoint(2, new double[] { 48.89276049688994, 8.700929620406455 });
            points.AddPoint(3, new double[] { 48.892897515502554, 8.701080071737248 });
            points.AddPoint(4, new double[] { 48.89301743701231, 8.70116365580991 });
            points.AddPoint(5, new double[] { 48.89298593018576, 8.701109790518604 });
            points.AddPoint(6, new double[] { 48.892905575453085, 8.700989057969167 });
            points.AddPoint(7, new double[] { 48.89293757083888, 8.701161798386057 });
            points.AddPoint(8, new double[] { 48.892906063961995, 8.701085644008693 });
            points.AddPoint(9, new double[] { 48.89286722987363, 8.701035493565055 });

            points.AddPoint(13, new double[] { 48.891853174108086, 8.69676132596165 });
            points.AddPoint(14, new double[] { 48.89180501743606, 8.696740504080669 });
            points.AddPoint(15, new double[] { 48.89181806324053, 8.696839714219493 });
            points.AddPoint(16, new double[] { 48.8918311090416, 8.696747852979838 });
            points.AddPoint(17, new double[] { 48.89183771247533, 8.696712333300521 });
            points.AddPoint(18, new double[] { 48.891802440527854, 8.696720907016218 });
            points.AddPoint(19, new double[] { 48.89179938041237, 8.696829915687267 });
            points.AddPoint(20, new double[] { 48.891801152073725, 8.696789496741836 });
            points.AddPoint(21, new double[] { 48.89181741906281, 8.696685387336899 });
            points.AddPoint(22, new double[] { 48.89183529663779, 8.696728255915428 });

            points.AddPoint(23, new double[] { 48.892019916930266, 8.694548829874108 });
            points.AddPoint(24, new double[] { 48.89417576745332, 8.692966286661555 });
            points.AddPoint(25, new double[] { 48.890987739489596, 8.698047083291893 });

            points.AddPoint(10, new double[] { 48.89338770206088, 8.700071490593366 });
            points.AddPoint(11, new double[] { 48.893908168829704, 8.699289515157762 });
            points.AddPoint(12, new double[] { 48.893452986858534, 8.704117498324337 });

            double maxRadius = 3000;
            int minPoints = 4;

            var optics = new Clustering.Optics.OPTICS(maxRadius, minPoints, points);
            optics.DistanceFunction = DistanceFunction.Haversine;

            optics.BuildReachability();

            var reachablity = optics.ReachabilityPoints();

            foreach (var item in reachablity)
            {
                Console.WriteLine(item.PointId + ";" + item.Reachability);
            }

            Console.ReadLine();
        }

        private static void DemoEuclidian()
        {
            Clustering.Optics.PointsList points = new Clustering.Optics.PointsList();

            // create random clusters with random points
            const int dimensions = 10;

            Random rand = new Random(5487);

            UInt32 totalPointsIndex = 0;
            for (int clusterIndex = 0; clusterIndex < 5; clusterIndex++)
            {
                var clusterCenter = Enumerable.Range(0, dimensions).Select(x => rand.NextDouble()).ToArray();
                int clusterCount = rand.Next(100, 1000);

                for (int pointIndex = 0; pointIndex < clusterCount; pointIndex++)
                {
                    var pointVector = clusterCenter.Select(v => v + (rand.NextDouble() * 0.05) - 0.025).ToArray();
                    points.AddPoint(totalPointsIndex, pointVector);
                    totalPointsIndex++;
                }
            }

            // add random noise
            const int noisePointCount = 1000;
            for (int pointIndex = 0; pointIndex < noisePointCount; pointIndex++)
            {
                var pointVector = Enumerable.Range(0, dimensions).Select(x => rand.NextDouble()).ToArray();
                points.AddPoint(totalPointsIndex, pointVector);
                totalPointsIndex++;
            }

            double maxRadius = 5;
            int minPoints = 10;

            var optics = new Clustering.Optics.OPTICS(maxRadius, minPoints, points);
            optics.DistanceFunction = DistanceFunction.Euclidian;

            optics.BuildReachability();

            var reachablity = optics.ReachabilityPoints();

            foreach (var item in reachablity)
            {
                Console.WriteLine(item.PointId + ";" + item.Reachability);
            }

            Console.ReadLine();
        }
    }
}
