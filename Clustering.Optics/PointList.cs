using System;
using System.Collections.Generic;

namespace System.Windows.MachineLearning.Optics
{
    public class PointList
    {
        internal List<Point> _points;

        public PointList()
        {
            _points = new List<Point>();
        }

        public void AddPoint(UInt32 id, double[] vector)
        {
            var newPoint = new Point((UInt32)_points.Count, id, vector);
            _points.Add(newPoint);
        }
    }
}
