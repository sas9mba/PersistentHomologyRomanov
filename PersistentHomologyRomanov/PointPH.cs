using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Threading;
using OpenCvSharp.CPlusPlus;



namespace PersistentHomologyRomanov
{

    /// <summary>
    /// класс описывающий точки
    /// </summary>
    public class PointPH
    {
        /// <summary>
        /// имя точки (изначальный порядковый номер)
        /// </summary>
        public int Name;
        /// <summary>
        /// координата Х
        /// </summary>
        public int CoordX;
        /// <summary>
        /// высота (неактивно)
        /// </summary>
        public int Height;
        /// <summary>
        /// Ширина (неактивно)
        /// </summary>
        public int Width;
        /// <summary>
        /// координата У
        /// </summary>
        public int CoordY;

        /// <summary>
        /// координата сектора Х (неактивно)
        /// </summary>
        public int CoordXCell;
        /// <summary>
        /// координата ячейки У (Н)
        /// </summary>
        public int CoordYCell;


        public List<int>[,] PointUnfamiliar;
        /// <summary>
        /// Точки незнакомые для этой позиции
        /// </summary>
        public List<int> PointNew;
        /// <summary>
        /// точки соединенные с этой позицией
        /// </summary>
        public List<int> PointFriend;

        public PointPH (int name, int x, int y )
        {
            Name = name; CoordX = x; CoordY = y;
      
        //this.Height=0;
        //    this.Width = 0;


        //    this.CoordXCell = 0;
        //    this.CoordYCell = 0;


        //    this.PointUnfamiliar = new List<int>[2,3];
        //    this.PointNew = new List<int>();
        //    this.PointFriend = new List<int>();
    }



        #region Функционал не действителен
        public void PaddingAllFriend(List<int> CountPoint)
        {
            PointFriend = new List<int>(CountPoint);
            PointNew = new List<int>(CountPoint);

            PointFriend.Remove(Name);
        }



        //заполнение массива точек
        public void PaddingCels(List<int>[,] Mass, int D , int MaxX, int MaxY)
        {
            Height = MaxY; Width = MaxX;
            PointUnfamiliar = new List<int>[MaxY,MaxX];
            PointNew = new List<int>();
            PointFriend = new List<int>();
            for (int y = 0; y<MaxY;y++)
            {
                for(int x=0; x<MaxX; x++)
                {
                    if (Mass[y, x] != null)
                    {
                        PointUnfamiliar[y, x] = new List<int>(Mass[y, x]);
                        
                    }
                   
                }
            }
            CoordYCell = CoordY / D;
            CoordXCell = CoordX / D;
            PointUnfamiliar[CoordYCell, CoordXCell].Remove(Name);
        }

        public void NewFrendlyToOld()
        {
            PointFriend.AddRange(PointNew);
            PointNew.Clear();
        }


        //Поиск общих точек ребра
        public List<int> SershCommonPoints(int a1, int a2, PointPH[] mass)
        {
            List<int> temp = new List<int>();
            foreach (int i in mass[a1].PointFriend)
            {
                if (mass[a2].PointFriend.Contains(i))
                {
                    temp.Add(i);
                }
            }
            return temp;
        }





        // определение ячеек поиска и запуск поиска друзей
        public void FrendlySersh(int D, int DMin, PointPH[] Mass)
        {

            int R = D / 2;
            int Xstart = (CoordX - D) / DMin; int Xend = (CoordX + D) / DMin;
            int Ystart = (CoordY - D) / DMin; int Yend = (CoordY + D) / DMin;
            if (Xstart < 0)
                Xstart = 0;
            if (Ystart < 0)
                Ystart = 0;
            if (Xend >= Width)
                Xend = Width-1;
            if (Yend >= Height)
                Yend = Height-1;






            int i = 0;
            for (; Ystart <= Yend; Ystart++)
            {
                for (int TempXstart = Xstart; TempXstart <= Xend; TempXstart++)
                {
                    if (PointUnfamiliar[Ystart, TempXstart] != null)
                        for (int y = 0; y < PointUnfamiliar[Ystart, TempXstart].Count; y++)
                        {
                            i = PointUnfamiliar[Ystart, TempXstart][y];
                            if (Frendly(Mass[Name], Mass[i], D))
                            {

                                PointNew.Add(i);
                                PointUnfamiliar[Ystart, TempXstart].RemoveAt(y);
                                Mass[i].PointNew.Add(Name);
                                Mass[i].PointUnfamiliar[CoordYCell, CoordXCell].Remove(Name);
                                y--;
                            }
                        }
                }
            }





            //for (int TempXstart = Xstart; TempXstart <= Xend; TempXstart++)
            //{
            //    Frendles(Ystart, TempXstart, Mass, D);
            //    Frendles(Yend, TempXstart, Mass, D);
            //    Frendles(Ystart+1, TempXstart, Mass, D);
            //    Frendles(Yend-1, TempXstart, Mass, D);
            //}

            //Ystart+=2;
            //Yend-=2;
            //for (; Ystart <= Yend; Ystart++)
            //{

            //    Frendles(Ystart, Xstart, Mass, D);
            //    Frendles(Ystart, Xend, Mass, D);
            //    Frendles(Ystart, Xstart+1, Mass, D);
            //    Frendles(Ystart, Xend-1, Mass, D);

            //}
        }

        //поиск друзей в конкретной ячейке
        public void Frendles(int Ystart, int TempXstart, PointPH[] Mass, int D)
        {
            int i = 0;
            if (PointUnfamiliar[Ystart, TempXstart] != null)
                for (int y = 0; y < PointUnfamiliar[Ystart, TempXstart].Count; y++)
                {
                    i = PointUnfamiliar[Ystart, TempXstart][y];
                    if (Frendly(Mass[Name], Mass[i], D))
                    {


                        PointNew.Add(i);
                        PointUnfamiliar[Ystart, TempXstart].RemoveAt(y);
                        Mass[i].PointNew.Add(Name);
                        Mass[i].PointUnfamiliar[CoordYCell, CoordXCell].Remove(Name);
                        y--;
                    }
                }
        }



        ////Поиск симплексов
        //public bool SearchSimplecs (List<int> ListAllPoint, PointPH[] Mass )
        //{
        //    bool Trye = false;
        //    ListAllPoint.Remove(Name);
        //    foreach (int i in PointFriend)
        //    {
        //        if (ListAllPoint.Remove(i))
        //        {
        //            Trye = true;
        //            Mass[i].SearchSimplecs(ListAllPoint, Mass);
        //        }
        //    }
        //    return Trye;
        //}


            //удаление лишних точек
        public void DellExtraPoints(int i, List<PointPH> mass)
        {
            int X = 0;
            int Y = 0;
            int XY;
            for (int j = i+1; j < mass.Count; j++)
            {
                X = Math.Abs(CoordX - mass[j].CoordX);
                Y = Math.Abs(CoordY - mass[j].CoordY);
                XY = X + Y;
                if ((XY)>0 && X<2 && Y<5)
                {
                    mass[j].DellExtraPoints(j, mass);
                    mass.RemoveAt(j);
                    j--;

                }
            }

        }

        public void DellExtraPoints1(int x, int y, List<PointPH> mass)
        {
            int X = 0;
            int Y = 0;
            for (int j = 0; j < mass.Count; j++)
            {
                if (CoordX== mass[j].CoordX && CoordY== mass[j].CoordY)
                    continue;
                X = Math.Abs(x - mass[j].CoordX);
                Y = Math.Abs(y - mass[j].CoordY);
                if ( X < 2 && Y < 2)
                {
                    X = mass[j].CoordX;
                    Y = mass[j].CoordY;
                    mass.RemoveAt(j);
                    DellExtraPoints1(X,Y, mass);
                    j--;

                }
            }

        }




        //определение области и запуск поиска разрыва связей и поиска дыр
        public void UnfamiliarSersh(int D, int DMin, PointPH[] Mass, List<HolPG> massHole, int iteration)
        {
            int FalseHole = 1; // если дыра существует менее этого числа, то удаляем
            int R = D / 2;
            int Xstart = (CoordX - R) / DMin - 1; int Xend = (CoordX + R) / DMin + 1;
            int Ystart = (CoordY - R) / DMin - 1; int Yend = (CoordY + R) / DMin + 1;
            if (Xstart < 0)
                Xstart = 0;
            if (Ystart < 0)
                Ystart = 0;
            if (Xend >= Width)
                Xend = Width - 1;
            if (Yend >= Height)
                Yend = Height - 1;

            List<int> commonPoint;
            int i = 0;
            for (int y = 0; y < PointFriend.Count; y++)
            {

                i = PointFriend[y];
                //ребро пропало
                if (!Frendly(Mass[Name], Mass[i], D))
                {


                    PointNew.Add(i);
                    PointFriend.RemoveAt(y);

                    Mass[i].PointFriend.Remove(Name);
                    y--;


                    commonPoint = new List<int>(SershCommonPoints(i, Name, Mass));


                    int t = 0;
                    for (int count = 0; count < massHole.Count; count++)
                    {
                        if (massHole[count].TwoPointMakeHole(i, Name))
                        {
                            t++;
                        }
                    }


                    //Проверяем принадлежит ли ребро дыре

                    for (int count = 0; count < massHole.Count; count++)
                    {
                        if (massHole[count].TwoPointMakeHole(i, Name))
                        {
                            //проверка на симплекс
                            if (commonPoint.Count > 0)
                            {

                                foreach (var it in commonPoint)

                                    if (!massHole[count].allPoint.Contains(it))
                                    {
                                        massHole[count].allPoint.Add(it);
                                    }

                                continue;
                            }

                            bool fack = false;

                            //проверяем есть ли с другой стороны еще одна дыра
                            for (int count2 = count + 1; count2 < massHole.Count; count2++)
                            {

                                if (massHole[count2].TwoPointMakeHole(i, Name))
                                {
                                    if (t > 2 && massHole[count].HoleInHole(massHole[count2]))
                                    {
                                        t--;
                                        break;

                                    }
                                    fack = true;
                                    if (massHole[count2].allPoint.Count < massHole[count].allPoint.Count)
                                    {
                                        massHole[count].CombineHole(massHole[count2]);
                                        massHole[count2].startedIteration = iteration + 1;
                                        if (massHole[count].endIteration < massHole[count2].endIteration)
                                        {
                                            massHole[count].endIteration = massHole[count2].endIteration;
                                        }
                                    }
                                    else
                                    {
                                        massHole[count2].CombineHole(massHole[count]);
                                        massHole[count].startedIteration = iteration + 1;
                                        if (massHole[count].endIteration < massHole[count2].endIteration)
                                        {
                                            massHole[count2].endIteration = massHole[count].endIteration;
                                        }

                                        break;
                                    }
                                    if (massHole[count2].endIteration - massHole[count2].startedIteration < FalseHole)
                                        massHole.RemoveAt(count2);

                                    continue;

                                }
                            }
                            if (fack)
                                goto to_Out;

                            if (Mass[i].PointFriend.Count > 0 && PointFriend.Count > 0)
                            {
                                //проверка на симплекс
                                if (commonPoint.Count == 0 && PointFriend.Count < 2 && Mass[i].PointFriend.Count < 2)
                                {
                                    massHole[count].allPoint.Clear();
                                    massHole[count].startedIteration = iteration + 1;

                                    if (massHole[count].endIteration - massHole[count].startedIteration < FalseHole)
                                        massHole.RemoveAt(count);
                                    continue;
                                }

                            }
                            else
                            {
                                massHole[count].allPoint.Remove(Mass[i].PointFriend.Count < 1 ? i : Name);
                                continue;
                            }
                        to_Out:;
                        }
                    }


                    if (commonPoint.Count > 1)
                    {
                        if (InspectionConnectionPointsTrue(Mass, new List<int>(commonPoint)))
                        {
                            commonPoint.Add(Name);
                            commonPoint.Add(i);
                            massHole.Add(new HolPG(commonPoint, iteration + 1));
                        }
                    }
                    commonPoint.Clear();




                }
            }




            //for (int TempXstart = Xstart; TempXstart <= Xend; TempXstart++)
            //{
            //    Unfamiliars(Ystart, TempXstart, Mass, D);
            //    Unfamiliars(Yend, TempXstart, Mass, D);
            //    Unfamiliars(Ystart + 1, TempXstart, Mass, D);
            //    Unfamiliars(Yend - 1, TempXstart, Mass, D);
            //}

            //Ystart+=2;
            //Yend-=2;
            //for (; Ystart <= Yend; Ystart++)
            //{

            //    Unfamiliars(Ystart, Xstart, Mass, D);
            //    Unfamiliars(Ystart, Xend, Mass, D);
            //    Unfamiliars(Ystart, Xstart + 1, Mass, D);
            //    Unfamiliars(Ystart, Xend - 1, Mass, D);

            //}
        }

        #endregion



        /// <summary>
        /// Появление отслеживание удаление дыр.
        /// </summary>
        /// <param name="D">Диаметр измененияя (неактивен)</param>
        /// <param name="DMin">минимальный Диаметр измененияя (неактивен)</param>
        /// <param name="Mass">Массив точек</param>
        /// <param name="massHole">Массив дыр</param>
        /// <param name="iteration">Номер итерации</param>
        /// <param name="img">изображение (неактивно)</param>
        /// <param name="PointCount">Позиция точки 1 в списке</param>
        /// <param name="point2">позиция точки 2 в списке</param>
        /// <param name="listEdge">список ребер</param>
        /// <param name="countEdge">позиция ребра в списке</param>
        public void CheckHole(int D, int DMin, PointPH[] Mass, List<HolPG> massHole, int iteration, IplImage img, int PointCount, int point2, List<EdgePH> listEdge, int countEdge)
        {

            int FalseHole = -1; // если дыра существует менее этого числа, то удаляем
            int R = D / 2;

            
            List<int> commonPoint;
            int i = 0;

            i = point2;
            //ребро пропало
            PointNew.Add(i);
            PointFriend.Remove(point2);
            Mass[i].PointFriend.Remove(Name);


            commonPoint = new List<int>(SershCommonPoints(i, Name, Mass));
            int edge1 = 0, edge2 = 0;
            string obraz = "";




            //Проверяем принадлежит ли ребро дыре

            for (int count = 0; count < massHole.Count; count++)
            {
                if (massHole[count].allEdge.Contains(countEdge))
                {
                    massHole[count].allEdge.Remove(countEdge);
                    //проверка на симплекс
                    if (commonPoint.Count > 0)
                    {

                        foreach (var it in commonPoint)
                        {
                            if (!massHole[count].allPoint.Contains(it))
                            {
                                massHole[count].allPoint.Add(it);
                            }




                            if (i<it)
                            {
                                obraz = i + "-" + it;
                            }
                            else
                            {
                                obraz = it + "-" + i;
                            }
                            edge1 = listEdge.FindIndex(p => p.obraz == obraz);
                            if (!massHole[count].allEdge.Contains(edge1))
                            {
                                massHole[count].allEdge.Add(edge1);
                            }
                            else
                            {
                                listEdge[edge1].inEdge = true;
                            }



                            if (Name < it)
                            {
                                obraz = Name + "-" + it;
                            }
                            else
                            {
                                obraz = it + "-" + Name;
                            }
                            edge2 = listEdge.FindIndex(p => p.obraz == obraz);
                            if (!massHole[count].allEdge.Contains(edge2))
                            {
                                massHole[count].allEdge.Add(edge2);
                            }
                            else
                            {
                                listEdge[edge2].inEdge = true;
                            }


                        }

                        goto to_Out;
                    }



                    //проверяем есть ли с другой стороны еще одна дыра
                    for (int count2 = count + 1; count2 < massHole.Count; count2++)
                    {

                        if (massHole[count2].allEdge.Contains(countEdge))
                        {
                            massHole[count].allEdge.Remove(countEdge);

                            if (massHole[count2].allPoint.Count < massHole[count].allPoint.Count)
                            {
                                massHole[count].CombineHole(massHole[count2], listEdge);
                                massHole[count2].startedIteration = iteration + 1;
                                //if (massHole[count].endIteration < massHole[count2].endIteration)
                                //{
                                //    massHole[count].endIteration = massHole[count2].endIteration;
                                //}

                            }
                            else
                            {
                                massHole[count2].CombineHole(massHole[count], listEdge);
                                massHole[count].startedIteration = iteration + 1;
                                //if (massHole[count].endIteration < massHole[count2].endIteration)
                                //{
                                //    massHole[count2].endIteration = massHole[count].endIteration;
                                //}


                            }
                            if (massHole[count2].endIteration - massHole[count2].startedIteration < FalseHole)
                                massHole.RemoveAt(count2);

                            goto to_Out;

                        }
                    }



                    if (Mass[i].PointFriend.Count > 0 && PointFriend.Count > 0)
                    {
                        //if (Mass[i].PointFriend.Count < 2 && PointFriend.Count < 2)
                        //{
                        //проверка на симплекс
                        if (commonPoint.Count == 0 && !listEdge[countEdge].inEdge)
                            {
                                massHole[count].allPoint.Clear();
                                massHole[count].allEdge.Clear();
                                massHole[count].startedIteration = iteration + 1;

                                if (massHole[count].endIteration - massHole[count].startedIteration < FalseHole)
                                    massHole.RemoveAt(count);
                                continue;
                            }
                        //}
                        //else
                        //{
                        //    int schet1 = 0, schet2 = 0;
                        //    foreach (var b in Mass[i].PointFriend)
                        //    {
                        //        if (massHole[count].allPoint.Contains(b))
                        //        {
                        //            schet1++;
                        //        }
                        //    }
                        //    foreach (var b in PointFriend)
                        //    {
                        //        if (massHole[count].allPoint.Contains(b))
                        //        {
                        //            schet2++;
                        //        }
                        //    }
                        //    if (schet1<2 && schet2<2)
                        //    {
                        //        //проверка на симплекс
                        //        if (commonPoint.Count == 0)
                        //        {
                        //            massHole[count].allPoint.Clear();
                        //            massHole[count].allEdge.Clear();
                        //            massHole[count].startedIteration = iteration + 1;

                        //            if (massHole[count].endIteration - massHole[count].startedIteration < FalseHole)
                        //                massHole.RemoveAt(count);
                        //            continue;
                        //        }
                        //    }
                        //}


                    }
                    else
                    {
                        massHole[count].allPoint.Remove(Mass[i].PointFriend.Count < 1 ? i : Name);
                        continue;
                    }

                }
            }


            if (commonPoint.Count > 1)
            {
                //if (InspectionConnectionPointsTrue(Mass, new List<int>(commonPoint))) //смысл проверки пропал после триангуляции
                {
                    commonPoint.Add(Name);
                    commonPoint.Add(i);
                    List<int> edges = new List<int>();


                    foreach (var it in commonPoint)
                    {

                        if (i < it)
                        {
                            obraz = i + "-" + it;
                        }
                        else
                        {
                            obraz = it + "-" + i;
                        }
                        edge1 = listEdge.FindIndex(p => p.obraz == obraz);
                        edges.Add(edge1);
                        if (Name < it)
                        {
                            obraz = Name + "-" + it;
                        }
                        else
                        {
                            obraz = it + "-" + Name;
                        }
                        edge1 = listEdge.FindIndex(p => p.obraz == obraz);
                        edges.Add(edge1);
                    }




                        massHole.Add(new HolPG(commonPoint, iteration + 1, edges, listEdge[countEdge].GetCentrEdgeX(Mass), listEdge[countEdge].GetCentrEdgeY(Mass)));
                }
            }
            to_Out:;
            commonPoint.Clear();




        }




        public void CheckHole(int D, int DMin, PointPH[] Mass, List<HolPG> massHole, int iteration, IplImage img, int PointCount, int point2)
        {
           
            int FalseHole = 1; // если дыра существует менее этого числа, то удаляем
            int R = D / 2;

            List<int> commonPoint;
            int i = 0;
          
                i = point2;
                //ребро пропало
                    PointNew.Add(i);
                    PointFriend.Remove(point2);
                    Mass[i].PointFriend.Remove(Name);


                    commonPoint = new List<int>(SershCommonPoints(i, Name, Mass));


               


            //Проверяем принадлежит ли ребро дыре

            for (int count = 0; count < massHole.Count; count++)
            {
                if (massHole[count].TwoPointMakeHole(i, Name))
                {
                    //проверка на симплекс
                    if (commonPoint.Count > 0)
                    {

                        foreach (var it in commonPoint)
                            if (!massHole[count].allPoint.Contains(it))
                            {
                                massHole[count].allPoint.Add(it);
                            }

                        goto to_Out;
                    }

                  

                    //проверяем есть ли с другой стороны еще одна дыра
                    for (int count2 = count + 1; count2 < massHole.Count; count2++)
                    {

                        if (massHole[count2].TwoPointMakeHole(i, Name))
                        {
                           
                            if (massHole[count2].allPoint.Count < massHole[count].allPoint.Count)
                            {
                                massHole[count].CombineHole(massHole[count2]);
                                massHole[count2].startedIteration = iteration + 1;
                                if (massHole[count].endIteration < massHole[count2].endIteration)
                                {
                                    massHole[count].endIteration = massHole[count2].endIteration;
                                }

                            }
                            else
                            {
                                massHole[count2].CombineHole(massHole[count]);
                                massHole[count].startedIteration = iteration + 1;
                                if (massHole[count].endIteration < massHole[count2].endIteration)
                                {
                                    massHole[count2].endIteration = massHole[count].endIteration;
                                }
                                

                            }
                            if (massHole[count2].endIteration - massHole[count2].startedIteration < FalseHole)
                                massHole.RemoveAt(count2);

                            goto to_Out;

                        }
                    }
                   
                        

                    if (Mass[i].PointFriend.Count > 0 && PointFriend.Count > 0)
                    {
                        //проверка на симплекс
                        if (commonPoint.Count == 0)
                        {
                            massHole[count].allPoint.Clear();
                            massHole[count].startedIteration = iteration + 1;

                            if (massHole[count].endIteration - massHole[count].startedIteration < FalseHole)
                                massHole.RemoveAt(count);
                            continue;
                        }


                    }
                    else
                    {
                        massHole[count].allPoint.Remove(Mass[i].PointFriend.Count < 1 ? i : Name);
                        continue;
                    }
                    
                }
            }
            

            if (commonPoint.Count > 1)
            {
                //if (InspectionConnectionPointsTrue(Mass, new List<int>(commonPoint))) //смысл проверки пропал после триангуляции
                {
                    commonPoint.Add(Name);
                    commonPoint.Add(i);
                    massHole.Add(new HolPG(commonPoint, iteration + 1));
                }
            }
            to_Out:;
            commonPoint.Clear();
            



        }















        //определение области и запуск поиска разрыва связей и поиска дыр
        public void UnfamiliarSershStep(int D, int DMin, PointPH[] Mass, List<HolPG> massHole, int iteration, IplImage img, int PointCount)
        {
            CvColor[] colorMass = new CvColor[6] { new CvColor(255, 255, 0), new CvColor(0, 255, 0),
            new CvColor(255, 0, 0), new CvColor(255, 0, 255), new CvColor(255, 255, 255), new CvColor(100, 255, 100) };
            int FalseHole = 1; // если дыра существует менее этого числа, то удаляем
            int R = D / 2;
            int Xstart = (CoordX - R) / DMin - 1; int Xend = (CoordX + R) / DMin + 1;
            int Ystart = (CoordY - R) / DMin - 1; int Yend = (CoordY + R) / DMin + 1;
            if (Xstart < 0)
                Xstart = 0;
            if (Ystart < 0)
                Ystart = 0;
            if (Xend >= Width)
                Xend = Width - 1;
            if (Yend >= Height)
                Yend = Height - 1;

            List<int> commonPoint;
            int i = 0;
            for (int y = 0; y < PointFriend.Count; y++)
            {

                i = PointFriend[y];
                //ребро пропало
                if (!Frendly(Mass[Name], Mass[i], D))
                {
                    

                    PointNew.Add(i);
                    PointFriend.RemoveAt(y);

                    Mass[i].PointFriend.Remove(Name);
                    y--;


                    commonPoint = new List<int>(SershCommonPoints(i, Name, Mass));


                    int t = 0;
                    for (int count = 0; count < massHole.Count; count++)
                    {
                        if (massHole[count].TwoPointMakeHole(i, Name))
                        {
                            t++;
                        }
                    }


                            //Проверяем принадлежит ли ребро дыре

                            for (int count = 0; count < massHole.Count; count++)
                    {
                        if (massHole[count].TwoPointMakeHole(i, Name))
                        {
                            //проверка на симплекс
                            if (commonPoint.Count > 0)
                            {

                                foreach (var it in commonPoint)

                                    if (!massHole[count].allPoint.Contains(it))
                                    {
                                        massHole[count].allPoint.Add(it);
                                    }

                                continue;
                            }

                            bool fack = false;
                            
                            //проверяем есть ли с другой стороны еще одна дыра
                            for (int count2 = count + 1; count2 < massHole.Count; count2++)
                            {

                                if (massHole[count2].TwoPointMakeHole(i, Name))
                                {
                                    if (t > 2)
                                    {
                                        t--;
                                        break;
                                       
                                    }
                                    fack = true;
                                    if (massHole[count2].allPoint.Count < massHole[count].allPoint.Count)
                                    {
                                        massHole[count].CombineHole(massHole[count2]);
                                        massHole[count2].startedIteration = iteration + 1;
                                        if (massHole[count].endIteration < massHole[count2].endIteration)
                                        {
                                            massHole[count].endIteration = massHole[count2].endIteration;
                                        }
                                    }
                                    else
                                    {
                                        massHole[count2].CombineHole(massHole[count]);
                                        massHole[count].startedIteration = iteration + 1;
                                        if (massHole[count].endIteration < massHole[count2].endIteration)
                                        {
                                            massHole[count2].endIteration = massHole[count].endIteration;
                                        }

                                        break;
                                    }
                                    if (massHole[count2].endIteration - massHole[count2].startedIteration < FalseHole)
                                        massHole.RemoveAt(count2);

                                    continue;

                                }
                            }
                            if (fack)
                                goto to_Out;

                            if (Mass[i].PointFriend.Count > 0 && PointFriend.Count > 0)
                            {
                                //проверка на симплекс
                                if (commonPoint.Count == 0 && PointFriend.Count<2 && Mass[i].PointFriend.Count<2)
                                {
                                    massHole[count].allPoint.Clear();
                                    massHole[count].startedIteration = iteration + 1;

                                    if (massHole[count].endIteration - massHole[count].startedIteration < FalseHole)
                                        massHole.RemoveAt(count);
                                    continue;
                                }
                              
                            }
                            else
                            {
                                massHole[count].allPoint.Remove(Mass[i].PointFriend.Count < 1 ? i : Name);
                                continue;
                            }
                        to_Out:;
                        }
                    }


                    //визуализация детальная
                    {
                        img = Cv.CreateImage(img.Size, BitDepth.F32, 4);
                        //точки
                        for (int i1 = 0; i1 < PointCount; i1++)
                        {
                            Cv.Circle(img, new CvPoint(Mass[i1].CoordX, Mass[i1].CoordY), 2, new CvScalar(0, 0, 255, 0));
                        }


                        //ребра
                        for (int i1 = 0; i1 < PointCount; i1++)
                        {
                            foreach (int f in Mass[i1].PointFriend)
                            {
                                Cv.Line(img, new CvPoint(Mass[f].CoordX, Mass[f].CoordY), new CvPoint(Mass[i1].CoordX, Mass[i1].CoordY), new CvScalar(100, 0, 0, 0));
                                for (int f1 = 0; f1 < massHole.Count; f1++)
                                {
                                    if (massHole[f1].allPoint.Contains(f) && massHole[f1].allPoint.Contains(i))
                                    {
                                        if (f1 < 5)
                                            Cv.Line(img, new CvPoint(Mass[f].CoordX, Mass[f].CoordY), new CvPoint(Mass[i1].CoordX, Mass[i1].CoordY), colorMass[f1]);
                                        else
                                            Cv.Line(img, new CvPoint(Mass[f].CoordX, Mass[f].CoordY), new CvPoint(Mass[i1].CoordX, Mass[i1].CoordY), colorMass[4]);

                                    }
                                }


                            }
                        }
                        //точки дыр
                        for (int i1 = 0; i1 < PointCount; i1++)
                        {
                            for (int g = 0; g < massHole.Count; g++)
                            {
                                if (massHole[g].PointInHole(i1))
                                {
                                    Cv.Circle(img, new CvPoint(Mass[i1].CoordX, Mass[i1].CoordY), 2, colorMass[g]);
                                    goto asd;
                                }
                            }
                            Cv.Circle(img, new CvPoint(Mass[i1].CoordX, Mass[i1].CoordY), 2, new CvScalar(0, 0, 255, 0));
                        asd:;
                        }






                        Cv.ShowImage("asd", img);

                        while (true)
                        {
                            int c = Cv.WaitKey(66);
                            if (c == 27) break;
                            Thread.Sleep(50);
                        }
                        //<-визуализация
                    }

                    if (commonPoint.Count > 1)
                    {
                        if (InspectionConnectionPointsTrue(Mass, new List<int>(commonPoint)))
                        {
                            commonPoint.Add(Name);
                            commonPoint.Add(i);
                            massHole.Add(new HolPG(commonPoint, iteration + 1));
                        }
                    }
                    commonPoint.Clear();



                   
                }
            }
        }



            //определение области и запуск поиска разрыва связей и поиска дыр
            public void UnfamiliarSershCub(int D, int DMin, PointPH[] Mass, List<HolPG> massHole, int iteration)
        {
            int FalseHole = 1; // если дыра существует менее этого числа, то удаляем
            int Xstart = (CoordX - D) / DMin - 1; int Xend = (CoordX + D) / DMin + 1;
            int Ystart = (CoordY - D) / DMin - 1; int Yend = (CoordY + D) / DMin + 1;
            if (Xstart < 0)
                Xstart = 0;
            if (Ystart < 0)
                Ystart = 0;
            if (Xend >= Width)
                Xend = Width - 1;
            if (Yend >= Height)
                Yend = Height - 1;
            List<int> pointsOnRadius = new List<int>();
            int i = 0;
            for (int x=Xstart;x<Xend; x++ )
            {
                for (int y = Ystart; y < Yend; y++)
                {
                    if (PointUnfamiliar[x, y]!=null)
                    for (int tempI =0; tempI < PointUnfamiliar[x,y].Count; tempI++)
                    {
                        if(Frendly(Mass[Name], Mass[PointUnfamiliar[x, y][i]], D))
                        {
                            pointsOnRadius.Add(PointUnfamiliar[x, y][i]);
                        }
                    }
                }
            }
            PointNew = new List<int>(PointFriend);
            PointFriend = new List<int>(pointsOnRadius);
            foreach(var i1 in pointsOnRadius)
            {
                PointNew.Remove(i1);
            }


            
        }

            //Проверка на соединение между точками устаревшее
            public bool InspectionConnectionPoints(PointPH[] mass, List<int> points)
        {
            for (int i = 0; i < points.Count - 1; i++)
            {
                for (int i2 = i + 1; i2 < points.Count; i2++)
                {
                    if (!mass[points[i]].PointFriend.Contains(points[i2]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        //если из любой точки можно придти к любой то не дыра
        public bool InspectionConnectionPointsTrue(PointPH[] mass, List<int> points, int b=0)
        {
          
            int a = points[b];
            points.RemoveAt(b);
            for (int i =0; i<points.Count; i++)
            {
                if (mass[a].PointFriend.Contains(points[i]))
                {
                    InspectionConnectionPointsTrue(mass, points, i);
                    i--;
                    continue;
                }
            }

            if (points.Count >0)
                return true;
            else return false;
        }



        //разрыв связи в конкретной области
        public void Unfamiliars(int Ystart, int TempXstart, PointPH[] Mass, int D)
        {
            int i = 0;
            
            if (PointUnfamiliar[Ystart, TempXstart] != null)
                for (int y = 0; y < PointUnfamiliar[Ystart, TempXstart].Count; y++)
                {
                    i = PointUnfamiliar[Ystart, TempXstart][y];
                    if (!Frendly(Mass[Name], Mass[i], D))
                    {


                        PointUnfamiliar[Ystart, TempXstart].RemoveAt(y);
                        PointFriend.Remove(i);
                        

                        Mass[i].PointUnfamiliar[CoordYCell, CoordXCell].Remove(Name);
                        Mass[i].PointFriend.Remove(Name);
                        y--;

                        


                    }
                }
        }

        // проверка расстояния между двумя точкамии на соответствие радиусу
        public static bool Frendly(PointPH a, PointPH b, int D)
        {
            if (Math.Sqrt( Math.Pow(b.CoordX - a.CoordX,2) + Math.Pow(b.CoordY  - a.CoordY, 2)) < D)
                return true;
            return false;
        }

    }
}
