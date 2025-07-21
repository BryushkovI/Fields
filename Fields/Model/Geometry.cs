using Aspose.Gis.Common;
using Aspose.Gis.Common.Formats.MapInfo.GraphicalObjects;
using Aspose.Gis.Geometries;
using Aspose.Gis.SpatialReferencing;
using System.Runtime.CompilerServices;

namespace Fields.Model
{
    public class Geometry
    {
        static int earthRinMetrs = 6371009;

        static decimal DeltaAngle(decimal angle1, decimal angle2)
        {
            return angle2 - angle1;
        }


        /// <summary>
        /// Получение списка азимутов в формате от 0 до 360 градусов
        /// </summary>
        /// <param name="coordinates"></param>
        /// <returns></returns>
        static public IList<double> GetAzimuts360(IList<Point> coordinates)
        {
            //coordinates.Add(coordinates[0]);
            IList<double> azimuts = [];
            for (int i = 0; i < coordinates.Count - 1; i++)
            {
                
                var azimutRad =
                Math.Atan2(
                    Math.Sin(double.DegreesToRadians((double)DeltaAngle(coordinates[i].Lng, coordinates[i + 1].Lng)))
                  * Math.Cos(double.DegreesToRadians((double)coordinates[i + 1].Lat)),

                    Math.Cos(double.DegreesToRadians((double)coordinates[i].Lat))
                  * Math.Sin(double.DegreesToRadians((double)coordinates[i + 1].Lat))
            -
                    Math.Sin(double.DegreesToRadians((double)coordinates[i].Lat))
                  * Math.Cos(double.DegreesToRadians((double)coordinates[i + 1].Lat))
                  * Math.Cos(double.DegreesToRadians((double)DeltaAngle(coordinates[i].Lng, coordinates[i + 1].Lng))
                ));

                var azimutDeg = double.RadiansToDegrees(azimutRad);
                if (azimutDeg < 0)
                {
                    azimutDeg += 360;
                }
                azimuts.Add(azimutDeg);
            }

            return azimuts;
        }


        /// <summary>
        /// Возвращает список (360-градусный азимут и идет ли после него угол > 180 градусов)
        /// </summary>
        /// <param name="angles"></param>
        /// <returns></returns>
        static public List<(double deg, bool over180)> FindOver180Angles(IList<double> angles)
        {
            List<(double, bool)> processedAngles = [];
            angles.Add(angles[0]);
            double maxAngle = 0;
            for (int i = 0; i < angles.Count - 1; i++)
            {

                if (angles[i] > angles[i+1] && angles[i] != angles.Max())
                {
                    processedAngles.Add((angles[i], true));
                }
                else
                {
                    processedAngles.Add((angles[i], false));
                }
            }
            return processedAngles;
        }

        static public bool IsInnerPoint(IList<Point> coordinates, decimal lat, decimal lng)
        {
            Aspose.Gis.Geometries.Polygon polygon = new ();
            polygon.SpatialReferenceSystem = SpatialReferenceSystem.Wgs84;

            LinearRing ring = new ();
            foreach (var item in coordinates)
            {
                ring.AddPoint((double)item.Lat, (double)item.Lng);
            }

            polygon.ExteriorRing = ring;

            Aspose.Gis.Geometries.LineString points = new();

            points.SpatialReferenceSystem = SpatialReferenceSystem.Wgs84;
            points.AddPoint((double)lat, (double)lng);
            points.AddPoint(90, 0);


            Aspose.Gis.Geometries.Point point = new Aspose.Gis.Geometries.Point((double) lat, (double) lng);
            point.SpatialReferenceSystem = SpatialReferenceSystem.Wgs84;

            return polygon.Covers(point);
        }



        /// <summary>
        /// Получение списка азимутов из вершины i к вершинам i+1 и i-1
        /// </summary>
        /// <param name="coordinates"></param>
        /// <returns></returns>
        static public IList<(int angleNumber, double azimutNext, double azimutPrev)> GetListAz(IList<Point> coordinates)
        {
            //coordinates.Add(coordinates[0]);
            coordinates.Insert(0, coordinates[coordinates.Count - 2]);
            
            IList<(int angleNumber, double azimutNext, double azimutPrev)> azimuts = [];
            for (int i = 1; i < coordinates.Count - 1; i++)
            {

                var azimutNextRad =
                Math.Atan2(
                    Math.Sin(double.DegreesToRadians((double)DeltaAngle(coordinates[i].Lng, coordinates[i + 1].Lng)))
                  * Math.Cos(double.DegreesToRadians((double)coordinates[i + 1].Lat)),

                    Math.Cos(double.DegreesToRadians((double)coordinates[i].Lat))
                  * Math.Sin(double.DegreesToRadians((double)coordinates[i + 1].Lat))
            -
                    Math.Sin(double.DegreesToRadians((double)coordinates[i].Lat))
                  * Math.Cos(double.DegreesToRadians((double)coordinates[i + 1].Lat))
                  * Math.Cos(double.DegreesToRadians((double)DeltaAngle(coordinates[i].Lng, coordinates[i + 1].Lng))
                ));

                var azimutPrevRad =
                Math.Atan2(
                    Math.Sin(double.DegreesToRadians((double)DeltaAngle(coordinates[i].Lng, coordinates[i - 1].Lng)))
                  * Math.Cos(double.DegreesToRadians((double)coordinates[i - 1].Lat)),

                    Math.Cos(double.DegreesToRadians((double)coordinates[i].Lat))
                  * Math.Sin(double.DegreesToRadians((double)coordinates[i - 1].Lat))
            -
                    Math.Sin(double.DegreesToRadians((double)coordinates[i].Lat))
                  * Math.Cos(double.DegreesToRadians((double)coordinates[i - 1].Lat))
                  * Math.Cos(double.DegreesToRadians((double)DeltaAngle(coordinates[i].Lng, coordinates[i - 1].Lng))
                ));

                azimuts.Add((i,azimutNextRad * 180 / Math.PI, azimutPrevRad * 180 / Math.PI));
            }

            return azimuts;
        }

        /// <summary>
        /// Получение сферического избытка (эксцесса)
        /// </summary>
        /// <param name="azimuts"></param>
        /// <returns></returns>
        static public double MathSphericalEpsilon(IList<(int angleNumber, double azimutNext, double azimutPrev)> azimuts)
        {
            //azimuts.Add(azimuts[0]);
            IList<double> thetas = [];
            double thetaSum = 0;
            double azimutSum = 0;
            int i = 0;
            for (i = 0; i < azimuts.Count; i++)
            {
                thetaSum +=  ( azimuts[i].azimutPrev - azimuts[i].azimutNext);
            }
            double rem = thetaSum % 180;
            if (rem < 0)
            {
                rem += 180;
            }
            return rem;
        }

        /// <summary>
        /// Расчет площади поверхности многоугольника на сфере
        /// </summary>
        /// <param name="coordinates"></param>
        /// <returns></returns>
        static public double AreaInMetrs(IList<Point> coordinates)
        {
            //var azimuts = GetAzimuts(coordinates);
            var az = GetListAz(coordinates);
            return double.DegreesToRadians(MathSphericalEpsilon(az)) * Math.Pow(earthRinMetrs, 2);
        }

        private static double InnerAngleRad(Point pointA, Point pointB)
        {
            double havdeltaLat = MathExtension.Hav(double.DegreesToRadians((double)DeltaAngle(pointA.Lat, pointB.Lat)));

            double havdeltaLng = MathExtension.Hav(double.DegreesToRadians((double)DeltaAngle(pointA.Lng, pointB.Lng)));

            double cosfi1 = Math.Cos(double.DegreesToRadians((double)pointA.Lat));

            double cosfi2 = Math.Cos(double.DegreesToRadians((double)pointB.Lat));

            return havdeltaLat + cosfi1 * cosfi2 * havdeltaLng;
        }

        static public double Distance2Points(Point pointA, Point pointB)
        {
            double dist = Math.Round(2 * earthRinMetrs * Math.Asin(Math.Sqrt(InnerAngleRad(pointA, pointB))));
            
            return dist;
        }

        
    }

    public static class MathExtension
    {
        public static double Hav(double angleRad)
        {
            return Math.Pow(double.Sin(angleRad / 2), 2);
        }
    }
    
}