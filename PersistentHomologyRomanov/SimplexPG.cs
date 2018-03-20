using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PersistentHomologyRomanov
{
    /// <summary>
    /// Компонента связанности
    /// </summary>
    class SimplexPG
    {
        /// <summary>
        /// имя компоненты (порядковый номер)
        /// </summary>
        public int name;
        /// <summary>
        /// Все точки компоненты
        /// </summary>
        public List<int> allPoint;
        /// <summary>
        /// итерация появления
        /// </summary>
        public int startedIteration;
        /// <summary>
        /// Итерация исчезновения
        /// </summary>
        public int endIteration;

        public int shape;
        public int absorption=-1;



        public SimplexPG(List<int> a, int EndIteration, int Name, int Shape)
        {
            name = Name;
            endIteration =EndIteration+1;
            allPoint = new List<int>(a);
            shape = Shape;
            

        }

        /// <summary>
        /// Тру если существует на текущей итерации
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public bool TestIn(int i)
        {
            if (i>=startedIteration && i<=endIteration)
            return true;
            return false;
        }

        /// <summary>
        /// Возвращает время жизни компоненты
        /// </summary>
        /// <returns></returns>
        public int Live()
        {
            return endIteration - startedIteration;
        }


        /// <summary>
        /// Поиск компонент связанности
        /// </summary>
        /// <param name="mass"></param>
        /// <param name="massSimplex"></param>
        /// <param name="iteration"></param>
        /// <param name="Name"></param>
        public void SearchSimplecsForSimplex(PointPH[] mass, List<SimplexPG> massSimplex, int iteration, int Name)
        {
            List<int> newAllPoint = new List<int>();
            List<SimplexPG> tempSimplexList = new List<SimplexPG>();
            while (allPoint.Count != 0)
            {
                newAllPoint.Clear();
                newAllPoint.Add(allPoint[0]);
                if (SearchSimplecs(mass, allPoint, newAllPoint, allPoint[0]))
                {
                    
                    tempSimplexList.Add(new SimplexPG(newAllPoint, iteration, Name, name));
                    Name++;
                   
                }
            }
            if (tempSimplexList.Count > 0)
            {
                if (tempSimplexList.Count == 1)
                {
                    allPoint = new List<int>(tempSimplexList[0].allPoint);
                }
                else
                {
                    int tempCountPointSimplex = 0;
                    int tempMaxPointSimplex = 0;
                    for (int i=0; i< tempSimplexList.Count; i++)
                    {
                        if (tempSimplexList[i].allPoint.Count>tempMaxPointSimplex)
                        {
                            tempCountPointSimplex = i;
                            tempMaxPointSimplex = tempSimplexList[i].allPoint.Count;
                        }
                    }
                    allPoint = new List<int>(tempSimplexList[tempCountPointSimplex].allPoint);
                    tempSimplexList.RemoveAt(tempCountPointSimplex);
                    massSimplex.AddRange(tempSimplexList);
                    //startedIteration = iteration+1;
                    //allPoint.Clear();
                }
            }
            else
            {
                startedIteration = iteration+1;
                allPoint.Clear();
            }

        }

        

        //проход по всем точкам компоненты
        public bool SearchSimplecs(PointPH[] Mass, List<int> allPoints, List<int> newallPoint, int Name)
        {
             bool Trye = false;
        allPoints.Remove(Name);
            foreach (int i in Mass[Name].PointFriend)
            {
                if (allPoints.Remove(i))
                {
                    newallPoint.Add(i);
                    Trye = true;
                    SearchSimplecs(Mass, allPoints, newallPoint, i);
    }
}
            return Trye;
        }

    }
}
