using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PersistentHomologyRomanov
{
    /// <summary>
    /// Класс Дыр
    /// </summary>
    public class HolPG
    {
        /// <summary>
        /// список точек дыры
        /// </summary>
        public List<int> allPoint;
        /// <summary>
        /// Итерация на которой дыра появилась
        /// </summary>
        public int startedIteration;
        /// <summary>
        /// Итерация на которой дыра пропала
        /// </summary>
        public int endIteration;
        /// <summary>
        /// Все ребра дыры
        /// </summary>
        public List<int> allEdge;
        /// <summary>
        /// Координата Х
        /// </summary>
        public int coordX;
        /// <summary>
        /// Координата Y
        /// </summary>
        public int coordY;


        public HolPG(List<int> a, int b, List<int> c, int x, int y)
        {
            coordX = x;
            coordY = y;
            endIteration = b;
            allPoint = new List<int>(a);
            allEdge = new List<int>(c);
        } 

        public HolPG(List<int> a, int b)
        {
            endIteration = b;
            allPoint = new List<int>(a);
            allEdge = new List<int>();
        }

        public HolPG(HolPG a)
        {
            allPoint = new List<int>(a.allPoint);
            startedIteration = a.startedIteration;
            endIteration = a.endIteration;
            allPoint = new List<int>(a.allPoint);
        }


        /// <summary>
        /// сколько итераций существует дыра
        /// </summary>
        /// <returns></returns>
        public int Live()
        {
            return endIteration-startedIteration;
        }

        /// <summary>
        /// тру если дыра содержит точку
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public bool PointInHole(int a)
        {
            return allPoint.Contains(a);
        }

        /// <summary>
        /// Тру если дыра есть на текущей итерации
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public bool TestIn(int i)
        {
            if (i >= startedIteration && i <= endIteration)
                return true;
            return false;
        }

        /// <summary>
        /// тру если обе точки принадлежат дыре
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        public bool TwoPointMakeHole(int a1, int a2)
        {
            return allPoint.Contains(a1) && allPoint.Contains(a2);
        }

        /// <summary>
        /// Если все точки входной дыры находятся в текущей то Тру
        /// </summary>
        /// <param name="hole2"></param>
        /// <returns></returns>
        public bool HoleInHole(HolPG hole2)
        {
            foreach(var i in hole2.allPoint)
            {
                if (!allPoint.Contains(i))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Объединяем дыры
        /// </summary>
        /// <param name="a"></param>
        /// <param name="edges"></param>
        public void CombineHole(HolPG a, List<EdgePH> edges = null)
        {
            foreach (var i in a.allPoint)
            {
                if (!allPoint.Contains(i))
                {
                    allPoint.Add(i);
                }
            }

            for (int i = 0; i < a.allEdge.Count; i++)
            {
                if(!(a.allEdge[i]<0))
                if (!allEdge.Contains(a.allEdge[i]))
                {
                    allEdge.Add(a.allEdge[i]);
                }
                else
                {
                    edges[a.allEdge[i]].inEdge = true;
                }
            }

           
            a.allEdge.Clear();
            a.allPoint.Clear();
        }
    }
}
