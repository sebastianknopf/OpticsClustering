using System.Globalization;
using System.Linq;
using System.Windows.MachineLearning.Optics;

namespace System.Windows.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            DemoHaversine();
        }

        private static void DemoHaversine()
        {
            PointList points = new PointList();

            // create random clusters with random points
            points.AddPoint(0, new double[] { 48.89278443239984, 8.700974198578608 });
            points.AddPoint(1, new double[] { 48.89276513743977, 8.701028063869876 });
            points.AddPoint(2, new double[] { 48.89276049688994, 8.700929620406455 });
            points.AddPoint(3, new double[] { 48.892897515502554, 8.701080071737248 });
            points.AddPoint(14, new double[] { 48.89301743701231, 8.70116365580991 });
            points.AddPoint(5, new double[] { 48.89298593018576, 8.701109790518604 });
            points.AddPoint(6, new double[] { 48.892905575453085, 8.700989057969167 });
            points.AddPoint(7, new double[] { 48.89293757083888, 8.701161798386057 });
            points.AddPoint(8, new double[] { 48.892906063961995, 8.701085644008693 });
            points.AddPoint(9, new double[] { 48.89286722987363, 8.701035493565055 });

            points.AddPoint(13, new double[] { 48.891853174108086, 8.69676132596165 });
            points.AddPoint(4, new double[] { 48.89180501743606, 8.696740504080669 });
            points.AddPoint(15, new double[] { 48.89181806324053, 8.696839714219493 });
            points.AddPoint(16, new double[] { 48.8918311090416, 8.696747852979838 });
            points.AddPoint(17, new double[] { 48.89183771247533, 8.696712333300521 });
            points.AddPoint(18, new double[] { 48.891802440527854, 8.696720907016218 });
            points.AddPoint(19, new double[] { 48.89179938041237, 8.696829915687267 });
            points.AddPoint(20, new double[] { 48.891801152073725, 8.696789496741836 });
            points.AddPoint(21, new double[] { 48.89181741906281, 8.696685387336899 });
            points.AddPoint(22, new double[] { 48.89183529663779, 8.696728255915428 });

            points.AddPoint(26, new double[] { 48.89182876288352, 8.699227573426857 });
            points.AddPoint(27, new double[] { 48.89190345879316, 8.699161564920036 });
            points.AddPoint(28, new double[] { 48.89185957038717, 8.699194104324807 });
            points.AddPoint(29, new double[] { 48.891805901780984, 8.699172721287386 });
            points.AddPoint(30, new double[] { 48.89181947176225, 8.699229432821415 });

            points.AddPoint(10, new double[] { 48.889159765695425, 8.694999629736628 });
            points.AddPoint(11, new double[] { 48.88913243292303, 8.694950806338184 });
            points.AddPoint(12, new double[] { 48.88911060338492, 8.695080537082571 });
            points.AddPoint(34, new double[] { 48.88903741013183, 8.694992654965388 });
            points.AddPoint(35, new double[] { 48.88911830795521, 8.694938251750015 });

            points.AddPoint(36, new double[] { 48.793025704227226, 8.778409705363188 });
            points.AddPoint(37, new double[] { 48.792933732957074, 8.778487265457455 });
            points.AddPoint(53, new double[] { 48.792987747897605, 8.778328451931085 });
            points.AddPoint(39, new double[] { 48.79303689657943, 8.778442945403588 });

            points.AddPoint(40, new double[] { 50.007317566816624, 10.905508580356166 });
            points.AddPoint(41, new double[] { 50.00727313627596, 10.905432024597474 });
            points.AddPoint(42, new double[] { 50.007300271741016, 10.905527139327926 });
            points.AddPoint(43, new double[] { 50.007307428412616, 10.905499300870147 });

            points.AddPoint(47, new double[] { 48.8917883528621, 8.697177049757236 });
            points.AddPoint(48, new double[] { 48.89176910416698, 8.697202881819805 });
            points.AddPoint(49, new double[] { 48.891774118546095, 8.697149987596525 });
            points.AddPoint(50, new double[] { 48.89177589784816, 8.697188120641247 });

            points.AddPoint(51, new double[] { 48.891679977728025, 8.697067571016003 });
            points.AddPoint(52, new double[] { 48.89167690440909, 8.697127845828625 });
            points.AddPoint(38, new double[] { 48.891670596006605, 8.69722010319489 });
            points.AddPoint(54, new double[] { 48.89166752268709, 8.697058960328562 });

            // add also some noise
            points.AddPoint(23, new double[] { 48.892019916930266, 8.694548829874108 });
            points.AddPoint(24, new double[] { 48.89417576745332, 8.692966286661555 });
            points.AddPoint(25, new double[] { 48.890987739489596, 8.698047083291893 });

            points.AddPoint(31, new double[] { 48.89338770206088, 8.700071490593366 });
            points.AddPoint(32, new double[] { 48.893908168829704, 8.699289515157762 });
            points.AddPoint(33, new double[] { 48.893452986858534, 8.704117498324337 });

            points.AddPoint(44, new double[] { 50.010011913274596, 10.900348064702285 });
            points.AddPoint(45, new double[] { 50.0116361713167, 10.903826681966247 });
            points.AddPoint(46, new double[] { 48.89223329302022, 8.70002674910773 });

            double maxRadius = 7;
            int minPoints = 2;

            var optics = new OPTICS(maxRadius, minPoints, 2, points);
            optics.Metric = Metric.Haversine;

            optics.Predict();

            var reachablity = optics.GetPointReachability();

            foreach (var item in reachablity)
            {
                Console.WriteLine(item.PointId + ";" + item.Reachability + ";" + item.ClusterId);
            }

            Console.WriteLine();

            foreach(Cluster item in optics.GetClusters())
            {
                Console.WriteLine("Cluster " + item.ID +  ", " + item.Points.Count + " Points, Size: " + item.Size);
                Console.WriteLine(item.Center[0].ToString(CultureInfo.InvariantCulture));
                Console.WriteLine(item.Center[1].ToString(CultureInfo.InvariantCulture));
            }

            Console.ReadLine();
        }

        private static void DemoEuclidian()
        {
            PointList points = new PointList();

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

            double maxRadius = 0.4;
            int minPoints = 10;

            var optics = new OPTICS(maxRadius, minPoints, 2, points);
            optics.Metric = Metric.Euclidian;

            optics.Predict();

            var reachablity = optics.GetPointReachability();

            foreach (var item in reachablity)
            {
                Console.WriteLine(item.PointId + ";" + item.Reachability + ";" + item.ClusterId);
            }

            Console.ReadLine();
        }
    }
}
