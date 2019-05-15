using System;

namespace JOS.KDTree
{
    public static class DistanceHelpers
    {
        public static double L2Norm(double[] x, double[] y)
        {
            var rlat1 = Math.PI * x[0] / 180;
            var rlat2 = Math.PI * y[0] / 180;
            var theta = x[1] - y[1];
            var rtheta = Math.PI * theta / 180;
            var distance =
                Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
                Math.Cos(rlat2) * Math.Cos(rtheta);
            distance = Math.Acos(distance);
            distance = distance * 180 / Math.PI;
            distance = distance * 60 * 1.1515;
            var distanceInKm = distance * 1.609344;
            return distanceInKm;
        }
    }
}
