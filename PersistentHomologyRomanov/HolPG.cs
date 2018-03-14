using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PersistentHomologyRomanov
{
    public class HolPG
    {
        public List<int> allPoint;
        public int startedIteration;
        public int endIteration;
        public List<int> allEdge;

        public HolPG(List<int> a, int b, List<int> c)
        {
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


        //время существования дыры
        public int Live()
        {
            return endIteration-startedIteration;
        }


        public bool PointInHole(int a)
        {
            return allPoint.Contains(a);
        }

        //проверка существования на текущей итерации
        public bool TestIn(int i)
        {
            if (i >= startedIteration && i <= endIteration)
                return true;
            return false;
        }

        // проверка существования двух точек в контуре дыры
        public bool TwoPointMakeHole(int a1, int a2)
        {
            return allPoint.Contains(a1) && allPoint.Contains(a2);
        }


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

        //объединение дыр
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
