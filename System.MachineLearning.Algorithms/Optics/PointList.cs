using System;
using System.Collections.Generic;

namespace System.MachineLearning.Optics
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

        public void AddPoint(UInt32 id, DateTime timestamp, double[] vector)
        {
            var newPoint = new Point((UInt32)_points.Count, id, timestamp, vector);
            _points.Add(newPoint);
        }
    }
}
