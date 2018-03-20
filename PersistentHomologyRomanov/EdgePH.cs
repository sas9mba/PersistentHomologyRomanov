using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistentHomologyRomanov
{
    /// <summary>
    /// Класс ребро
    /// </summary>
    public class EdgePH
    {
        /// <summary>
        /// позиция первой точки в списке точек
        /// </summary>
        public int point1;
        /// <summary>
        /// позиция второй точке в списке точек
        /// </summary>
        public int point2;
        /// <summary>
        /// длинна ребра
        /// </summary>
        public double leght;
        /// <summary>
        /// имя ребра складывается из имен точек "младшая - старшая"
        /// </summary>
        public string obraz;
        /// <summary>
        /// Двойное ребро, тру если дважды принадлежит одной дыре (размыкание бублика)
        /// </summary>
        public bool inEdge;

        public EdgePH(int point1, int point2, double leght)
        {
            this.point1 = point1;
            this.point2 = point2;
            this.leght = leght;
            inEdge = false;

            if (point1 < point2)
            {
                obraz = point1 + "-" + point2;
            }
            else
            {
                obraz = point2 + "-" + point1;
            }
        }

        /// <summary>
        /// получаем координату середины ребра по Х
        /// </summary>
        /// <param name="MassPoints"></param>
        /// <returns>координата Х </returns>
        public int GetCentrEdgeX(PointPH[] MassPoints)
        {
            return (MassPoints[point1].CoordX+ MassPoints[point2].CoordX)/2;
        }
        /// <summary>
        /// получаем координату середины ребра по У
        /// </summary>
        /// <param name="MassPoints"></param>
        /// <returns>координата У </returns>
        public int GetCentrEdgeY(PointPH[] MassPoints)
        {
            return (MassPoints[point1].CoordY + MassPoints[point2].CoordY) / 2;
        }

    }

    
}
