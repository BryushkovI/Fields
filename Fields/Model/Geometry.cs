using Aspose.Gis.Common;

namespace Fields.Model
{
    public class Geometry
    {
        static int earthRinMetrs = 6371009;
        #region Старое решение
        static double ConvertToX(float lat, float lng)
        {
            return earthRinMetrs * Math.Cos(float.DegreesToRadians(lat)) * Math.Cos(float.DegreesToRadians(lng));
        }
        static double ConvertToY(float lat, float lng)
        {
            return earthRinMetrs * Math.Cos(float.DegreesToRadians(lat)) * Math.Sin(float.DegreesToRadians(lng));
        }
        static double ConvertToZ(float lat)
        {
            return earthRinMetrs * Math.Sin(float.DegreesToRadians(lat));
        }
        /// <summary>
        /// Конвертирует координаты широты и долготы в декартовы координаты
        /// </summary>
        /// <param name="latLngCoord">Координаты широты и долготы</param>
        /// <returns>Декартовы координаты</returns>
        static public (double, double, double) ConvertToDecart((float, float) latLngCoord)
        {
            return new(ConvertToX(latLngCoord.Item1, latLngCoord.Item2), ConvertToY(latLngCoord.Item1, latLngCoord.Item2), ConvertToZ(latLngCoord.Item1));
        }

        static public (double, double, double) GetVectorAB((double, double, double) pointA, (double, double, double) pointB)
        {
            return (pointB.Item1 - pointA.Item1, pointB.Item2 - pointA.Item2, pointB.Item3 - pointA.Item3);
        } 
        #endregion


        static (double radlat, double raflng) ConvertToRad((float lat, float lng) coordinate)
        {
            return (Math.PI * coordinate.lat / 180, Math.PI * coordinate.lng / 180);
        }

        static decimal DeltaAngle(decimal angle1, decimal angle2)
        {
            return angle2 - angle1;
        }

        static public IList<double> GetAzimuts(IList<(decimal lat, decimal lng)> coordinates)
        {
            //coordinates.Add(coordinates[0]);
            IList<double> azimuts = [];
            for (int i = 0; i < coordinates.Count - 1; i++)
            {
                
                var azimutRad =
                Math.Atan2(
                    Math.Sin(double.DegreesToRadians((double)DeltaAngle(coordinates[i].lng, coordinates[i + 1].lng)))
                  * Math.Cos(double.DegreesToRadians((double)coordinates[i + 1].lat)),

                    Math.Cos(double.DegreesToRadians((double)coordinates[i].lat))
                  * Math.Sin(double.DegreesToRadians((double)coordinates[i + 1].lat))
            -
                    Math.Sin(double.DegreesToRadians((double)coordinates[i].lat))
                  * Math.Cos(double.DegreesToRadians((double)coordinates[i + 1].lat))
                  * Math.Cos(double.DegreesToRadians((double)DeltaAngle(coordinates[i].lng, coordinates[i + 1].lng))
                ));

                azimuts.Add(azimutRad * 180 / Math.PI);
            }

            return azimuts;
        }

        static public IList<(int angleNumber, double azimutNext, double azimutPrev)> GetListAz(IList<(decimal lat, decimal lng)> coordinates)
        {
            //coordinates.Add(coordinates[0]);
            coordinates.Insert(0, coordinates[coordinates.Count - 2]);
            
            IList<(int angleNumber, double azimutNext, double azimutPrev)> azimuts = [];
            for (int i = 1; i < coordinates.Count - 1; i++)
            {

                var azimutNextRad =
                Math.Atan2(
                    Math.Sin(double.DegreesToRadians((double)DeltaAngle(coordinates[i].lng, coordinates[i + 1].lng)))
                  * Math.Cos(double.DegreesToRadians((double)coordinates[i + 1].lat)),

                    Math.Cos(double.DegreesToRadians((double)coordinates[i].lat))
                  * Math.Sin(double.DegreesToRadians((double)coordinates[i + 1].lat))
            -
                    Math.Sin(double.DegreesToRadians((double)coordinates[i].lat))
                  * Math.Cos(double.DegreesToRadians((double)coordinates[i + 1].lat))
                  * Math.Cos(double.DegreesToRadians((double)DeltaAngle(coordinates[i].lng, coordinates[i + 1].lng))
                ));

                var azimutPrevRad =
                Math.Atan2(
                    Math.Sin(double.DegreesToRadians((double)DeltaAngle(coordinates[i].lng, coordinates[i - 1].lng)))
                  * Math.Cos(double.DegreesToRadians((double)coordinates[i - 1].lat)),

                    Math.Cos(double.DegreesToRadians((double)coordinates[i].lat))
                  * Math.Sin(double.DegreesToRadians((double)coordinates[i - 1].lat))
            -
                    Math.Sin(double.DegreesToRadians((double)coordinates[i].lat))
                  * Math.Cos(double.DegreesToRadians((double)coordinates[i - 1].lat))
                  * Math.Cos(double.DegreesToRadians((double)DeltaAngle(coordinates[i].lng, coordinates[i - 1].lng))
                ));

                azimuts.Add((i,azimutNextRad * 180 / Math.PI, azimutPrevRad * 180 / Math.PI));
            }

            return azimuts;
        }


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

        static public double AreaInMetrs(IList<(decimal lat, decimal lng)> coordinates)
        {
            //var azimuts = GetAzimuts(coordinates);
            var az = GetListAz(coordinates);
            return double.DegreesToRadians(MathSphericalEpsilon(az)) * Math.Pow(earthRinMetrs, 2);
        }


        static float ConvertToLat(double z)
        {
            return (float) Math.Asin(z / earthRinMetrs);
        }
        static float ConvertToLng(double x, double y)
        {
            return (float)Math.Atan2(y, x);
        }
    }
}
