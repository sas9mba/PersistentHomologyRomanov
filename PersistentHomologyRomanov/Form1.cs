using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Threading;
using OpenCvSharp.CPlusPlus;
using AForge;
using AForge.Imaging;
using System.Diagnostics;


namespace PersistentHomologyRomanov
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        Color[] Calci = new Color[10] { Color.Red, Color.Blue, Color.Green, Color.Indigo, Color.Black, Color.Brown, Color.Orange, Color.Lime, Color.Tomato, Color.Gray };

        CvColor[] colorMass = new CvColor[5] { new CvColor(255, 255, 0), new CvColor(0, 255, 0),
             new CvColor(255, 0, 255), new CvColor(0, 255, 255), new CvColor(100, 255, 100) };
        int minParamLive = 0; //минимальная продолжительность жизни
        int countPoint = 0;
        int D = 10; //минимальный диаметр 
        int CountThread = 5;
        int MaxX = 0; //Граница массива поиска по Х
        int MaxY = 0; // Граница массива поиска по У
        PointPH[] PointAll; //Массив всех точек
        List<int>[,] PointSearch; //Массив Поиска
        int PointCount = 0; //Количество точек
        int PointNext = 10; //Количество через которое учитывается каждая точка
        int CountIteration = 10;
        List<HolPG> massHole = new List<HolPG>();
        List<SimplexPG> massSimplex = new List<SimplexPG>();
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        string s = @"D:\Ушеба\Семестр 8\Мисчета\Картынки\1.jpg";
        IplImage img;



        #region выделение контура и заполнение массивов
        private void OpenImage_Click(object sender, EventArgs e)
        {
            massHole.Clear();
            massSimplex.Clear();
            D = 10; //минимальный диаметр 
            CountThread = 5;
            MaxX = 0; //Граница массива поиска по Х
            MaxY = 0; // Граница массива поиска по У

            PointCount = 0; //Количество точек
            PointNext = 10; //Количество через которое учитывается каждая точка
            CountIteration = Convert.ToInt32(textBoxIteration.Text);
            massHole = new List<HolPG>();
            img = Cv.LoadImage(s, LoadMode.GrayScale);

            Cv.Threshold(img, img, 0, 255, ThresholdType.Otsu);
            Cv.ShowImage("asd", img);

            PointNext = Convert.ToInt32(TextboxNextPoint.Text);
            textBoxRadius.Text = (Convert.ToInt32(Math.Sqrt(Math.Pow(img.Height, 2) + Math.Pow(img.Width, 2))) / CountIteration).ToString();

            D = Convert.ToInt32(textBoxRadius.Text);





            MaxX = img.Width / D + 1; MaxY = img.Height / D + 1;
            PointSearch = new List<int>[MaxY, MaxX];


            List<PointPH> TempAllPoint = new List<PointPH>();

            int x = 0, y = 0;
            int[,] X = { { 1, 1, 0 }, { 0, 0, 0 }, { 0, -1, -1 } };
            int[,] Y = { { 0, 0, 1 }, { -1, 0, 1 }, { -1, 0, 0 } };

            CvScalar a = new CvScalar(0, 0, 0, 0);


            int TempNextPoint = 0;
            for (y = 0; y < img.Height; y++)
            {
                for (x = 0; x < img.Width; x++)
                {
                    if (img[y, x] == a)
                    {

                        TempAllPoint.Add(new PointPH(PointCount, x, y));
                        if (PointSearch[y / D, x / D] == null)
                        {
                            PointSearch[y / D, x / D] = new List<int>();
                        }
                        PointSearch[y / D, x / D].Add(PointCount);
                        PointCount++;
                        TempNextPoint = PointNext;
                        goto to_Out;
                    }
                }
            }
        to_Out:

            int xs = x;
            int ys = y;
            int xp = x - 1;
            int yp = y;
            int xo = 0;
            int yo = 0;
            while (true)
            {
                xo = xp - x + 1;
                yo = yp - y + 1;
                xp += X[yo, xo];
                yp += Y[yo, xo];
                if (img[yp, xp] == a)
                {
                    if (TempNextPoint == 0)
                    {
                        TempAllPoint.Add(new PointPH(PointCount, xp, yp));
                        if (PointSearch[yp / D, xp / D] == null)
                        {
                            PointSearch[yp / D, xp / D] = new List<int>();
                        }
                        PointSearch[yp / D, xp / D].Add(PointCount);
                        PointCount++;
                        TempNextPoint = PointNext;

                    }
                    else
                    {
                        TempNextPoint--;
                    }

                    xo = x; yo = y;
                    x = xp; y = yp;
                    xp = xo; yp = yo;
                    if (x == xs && y == ys)
                    {
                        break;
                    }
                }
            }

            PointAll = TempAllPoint.ToArray();
            Thread[] myThread = new Thread[CountThread];
            GetOut Go = new GetOut();
            int start = 0;
            int end = start + PointCount / (CountThread - 1);

            for (int i = 0; i < PointCount; i++)
            {
                PointAll[i].PaddingCels(PointSearch, D, MaxX, MaxY);
            }
            /*
            for (int i = 0;i< CountThread-1; i+= 1)
            {
                myThread[i] = new Thread(new ParameterizedThreadStart(PaddingPointPH));
                Go.a = start; Go.b = end;
                start = end; end = end + PointCount / (CountThread - 1);
                myThread[i].Start(Go);

            }

            myThread[CountThread-1] = new Thread(new ParameterizedThreadStart(PaddingPointPH));
            Go.a = start; Go.b = PointCount-1;
            myThread[CountThread - 1].Start(Go);
            */

        }
#endregion

        //класс для передачи в поток
        void PaddingPointPH(object xz)
        {
            GetOut a = (GetOut)xz;
            for (int i = a.a; i < a.b; i++)
            {
                PointAll[i].PaddingCels(PointSearch, D, MaxX, MaxY);
            }

        }


#region кнопка последовательного поиска друзей
        private void SershFriend_Click(object sender, EventArgs e)
        {
            CountIteration = Convert.ToInt32(textBoxIteration.Text);
            int DTemp = D;


            for (int y = 0; y < CountIteration; y++)
            {
                DTemp = D * (y + 1);
                label4.Text = y + "";


                for (int i = 0; i < PointCount; i++)
                {
                    PointAll[i].FrendlySersh(DTemp, D, PointAll);
                }



                //Новые други теперь други
                for (int i = 0; i < PointCount; i++)
                {
                    PointAll[i].NewFrendlyToOld();
                }



                img = Cv.CreateImage(img.Size, BitDepth.F32, 0);
                for (int i = 0; i < PointCount; i++)
                {
                    Cv.Circle(img, new CvPoint(PointAll[i].CoordX, PointAll[i].CoordY), 2, new CvScalar(255));
                }
                //for (int i = 0; i < PointCount; i++)
                //{
                //    Cv.Circle(img, new CvPoint(PointAll[i].CoordX, PointAll[i].CoordY), DTemp/2, new CvScalar(15));
                //}
                for (int i = 0; i < PointCount; i++)
                {
                    foreach (int f in PointAll[i].PointFriend)
                    {
                        Cv.Line(img, new CvPoint(PointAll[f].CoordX, PointAll[f].CoordY), new CvPoint(PointAll[i].CoordX, PointAll[i].CoordY), new CvScalar(122, 122, 0, 0));
                    }
                }

                Cv.ShowImage("asd", img);

                while (true)
                {
                    int c = Cv.WaitKey(66);
                    if (c == 27) break;
                    Thread.Sleep(50);
                }









            }

        }
#endregion

        //выбор изображения
        private void SelectImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog open_dialog = new OpenFileDialog();
            open_dialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG;*.tif)|*.BMP;*.JPG;*.GIF;*.PNG;*.tif|All files (*.*)|*.*";
            if (open_dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    s = open_dialog.FileName;
                }
                catch
                {
                    DialogResult rezult = MessageBox.Show("Невозможно открыть выбранный файл",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        #region кнопка последовательного удаления ребер, с конца
        private void SershDir_Click(object sender, EventArgs e)
        {

         
            CountIteration = Convert.ToInt32(textBoxIteration.Text);
            int DTemp = D;
            List<int> temp = new List<int>();

            int tempCountSimplex;
            for (int i = 0; i < PointCount; i++)
            {
                temp.Add(i);
            }
            massSimplex.Add(new SimplexPG(temp, CountIteration, 0, 0));
            for (int i = 0; i < PointCount; i++)
            {
                PointAll[i].PaddingAllFriend(temp);
            }

            for (int y = CountIteration - 1; y >= 0; y--)
            {
                DTemp = D * (y + 1);
                label4.Text = y + "";


                //ДыроПоиск
                for (int i = 0; i < PointCount; i++)
                {
                    PointAll[i].UnfamiliarSersh(DTemp, D, PointAll, massHole, y);
                }


                //СимплексПоиск
                tempCountSimplex = massSimplex.Count;
                for (int i = 0; i < tempCountSimplex; i++)
                {
                    if (massSimplex[i].allPoint.Count > 0)
                        massSimplex[i].SearchSimplecsForSimplex(PointAll, massSimplex, y, massSimplex.Count);
                }

                //img = Cv.CreateImage(img.Size, BitDepth.F32, 0);
                //for (int i = 0; i < PointCount; i++)
                //{
                //    Cv.Circle(img, new CvPoint(PointAll[i].CoordX, PointAll[i].CoordY), DTemp / 2, new CvScalar(15));
                //}
                //for (int i = 0; i < PointCount; i++)
                //{
                //    Cv.Circle(img, new CvPoint(PointAll[i].CoordX, PointAll[i].CoordY), 2, new CvScalar(255));
                //}
                //for (int i = 0; i < PointCount; i++)
                //{
                //    foreach (int f in PointAll[i].PointFriend)
                //    {
                //        Cv.Line(img, new CvPoint(PointAll[f].CoordX, PointAll[f].CoordY), new CvPoint(PointAll[i].CoordX, PointAll[i].CoordY), new CvScalar(122, 122, 0, 0));
                //    }
                //}
                //Cv.ShowImage("asd", img);

                //while (true)
                //{
                //    int c = Cv.WaitKey(66);
                //    if (c == 27) break;
                //    Thread.Sleep(50);
                //}

            }


            for (int i = massSimplex.Count - 1; i > 0; i--)
            {
                if (massSimplex[i].startedIteration < massSimplex[massSimplex[i].shape].startedIteration)
                {
                    massSimplex[massSimplex[i].shape].startedIteration = massSimplex[i].startedIteration;
                    massSimplex[massSimplex[i].shape].absorption = massSimplex[i].name;
                    //massSimplex.RemoveAt(i);
                }
            }
            for (int i = massSimplex.Count - 1; i > 0; i--)
            {
                if (massSimplex[i].name == massSimplex[massSimplex[i].shape].absorption)
                {
                    massSimplex.RemoveAt(i);
                }
            }
        }
        #endregion

        #region считает числа бети (кнопка)
        private void button1_Click(object sender, EventArgs e)
        {
            int maxHole = 0;
            int maxSimplex = 0;
            int maxLiveHole2 = 0;
            int maxLiveHole = 0;
            int maxLiveSimplex = 0;
            int maxLiveSimplex2 = 0;
            int tempHole = 0;
            int tempSimplex = 0;

            //считаем максимальное количество дыр и симплексов в такт.
            for (int i = 0; i < CountIteration; i++)
            {
                tempHole = 0;
                tempSimplex = 0;
                foreach (var r in massHole)
                {
                    if (r.TestIn(i))
                        tempHole++;
                }
                foreach (var r in massSimplex)
                {
                    if (r.TestIn(i))
                        tempSimplex++;
                }
                if (maxSimplex < tempSimplex)
                {

                    maxSimplex = tempSimplex;
                }
                if (maxHole < tempHole)
                {

                    maxHole = tempHole;

                }



            }
            //высчитываем две долгоживущие дыры и симплексы а так же средняя продолжительность
            double averageLiveHole = 0;
            double averageLiveSimplex = 0;
            int i1 = 0;
            foreach (var i in massHole)
            {
                i1 = i.Live();
                averageLiveHole += i1;
                if (maxLiveHole2 < i1)
                {
                    if (maxLiveHole < i1)
                    {
                        maxLiveHole2 = maxLiveHole;
                        maxLiveHole = i1;

                    }
                    else
                    {
                        maxLiveHole2 = i1;
                    }
                }
            }
            foreach (var i in massSimplex)
            {
                i1 = i.Live();
                if (maxLiveSimplex2 < i1)
                {
                    if (maxLiveSimplex < i1)
                    {
                        maxLiveSimplex2 = maxLiveSimplex;
                        maxLiveSimplex = i1;

                    }
                    else
                    {
                        maxLiveSimplex2 = i1;
                    }
                }
                averageLiveSimplex += i1;
            }

            averageLiveHole /= Convert.ToDouble(massHole.Count);
            averageLiveSimplex /= Convert.ToDouble(massSimplex.Count);




            //количество дыр и симплексов с жизнью выше средней

            int countAverageLiveHole = 0;
            int countAverageLiveSimplex = 0;
            foreach (var i in massHole)
            {
                i1 = i.Live();
                if (averageLiveHole < i1)
                {
                    countAverageLiveHole++;
                }
            }
            foreach (var i in massSimplex)
            {
                i1 = i.Live();
                if (averageLiveSimplex < i1)
                {
                    countAverageLiveSimplex++;
                }
            }
        }
        #endregion

        #region Открыть рисунок точек
        private void button2_Click(object sender, EventArgs e)
        {
            massHole.Clear();
            massSimplex.Clear();
            D = 10; //минимальный диаметр 
            CountThread = 5;
            MaxX = 0; //Граница массива поиска по Х
            MaxY = 0; // Граница массива поиска по У

            PointCount = 0; //Количество точек
            PointNext = 10; //Количество через которое учитывается каждая точка
            massHole = new List<HolPG>();
            img = Cv.LoadImage(s, LoadMode.GrayScale);

            Cv.Threshold(img, img, 0, 255, ThresholdType.Otsu);
            Cv.ShowImage("asd", img);
            CountIteration = Convert.ToInt32(textBoxIteration.Text);
            textBoxRadius.Text = (Convert.ToInt32(Math.Sqrt(Math.Pow(img.Height, 2) + Math.Pow(img.Width, 2))) / CountIteration).ToString();
            PointNext = Convert.ToInt32(TextboxNextPoint.Text);
            D = Convert.ToInt32(textBoxRadius.Text);




            MaxX = img.Width / D + 1; MaxY = img.Height / D + 1;
            PointSearch = new List<int>[MaxY, MaxX];


            List<PointPH> TempAllPoint = new List<PointPH>();
            CvScalar a = new CvScalar(0, 0, 0, 0);


            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    if (img[y, x] == a)
                    {

                        TempAllPoint.Add(new PointPH(PointCount, x, y));

                        PointCount++;
                    }
                }
            }

            //for (int i = 0; i < TempAllPoint.Count; i++)
            //{
            //    TempAllPoint[i].DellExtraPoints1(TempAllPoint[i].CoordX, TempAllPoint[i].CoordY, TempAllPoint);
            //    TempAllPoint[i].Name = i;
            //}

            PointAll = TempAllPoint.ToArray();


            PointCount = PointAll.Length;

            int tempX;
            int tempY;


            for (int i = 0; i < PointCount; i++)
            {
                tempX = PointAll[i].CoordX / D;
                tempY = PointAll[i].CoordY / D;

                if (PointSearch[tempY, tempX] == null)
                {
                    PointSearch[tempY, tempX] = new List<int>();
                }
                PointSearch[tempY, tempX].Add(i);
            }



            for (int i = 0; i < PointCount; i++)
            {
                PointAll[i].PaddingCels(PointSearch, D, MaxX, MaxY);
            }

            Cv.SaveImage(s + ".jpg", img);

        }
        #endregion



        #region Запись параметров
        private void button3_Click(object sender, EventArgs e)
        {
            var Param = GetParam();


            string bue = "";


            bue = "";

            #region запись в файл (устаревшее)
            //for (int i = 0; i < Param[0].Length - 1; i++)
            //{
            //    bue += (Param[0][i]) + ";";
            //}
            //if (Param[0].Length > 0)
            //    bue += Param[0][Param[0].Length - 1] + "\n";
            //else
            //    bue += "0\n";


            //for (int i = 0; i < Param[1].Length - 1; i++)
            //{
            //    bue += (Param[1][i]) + ";";
            //}
            //if (Param[1].Length > 0)
            //    bue += Param[1][Param[1].Length - 1];
            //else
            //    bue += "0";
            #endregion


            int y = 0;
            bue = textBoxNameObject.Text + '\n';
            for (y = 0;y<7;y++)
            {
                for (int i = 0; i < Param[y].Length - 1; i++)
                {
                    bue += (Param[y][i]) + ";";
                }
                if (Param[y].Length > 0)
                    bue += Param[y][Param[y].Length - 1] + "\n";
                else
                    bue += "0\n";
            }

            for (int i = 0; i < Param[y].Length - 1; i++)
            {
                bue += (Param[y][i]) + ";";
            }
            if (Param[y].Length > 0)
                bue += Param[y][Param[y].Length - 1];
            else
                bue += "0";



            if (WritenInFile.Checked)
                File.AppendAllText(@"param.txt", bue + "\n");


        }

        private void button4_Click(object sender, EventArgs e)
        {
            string bue = "";
            for (int i = 0; i < 10; i++)
            {
                bue += (i + "a") + ";";
            }
            for (int i = 0; i < 10; i++)
            {
                for (int j = i + 1; j < 10; j++)
                {
                    bue += (i + "a/" + j + "a") + ";";
                }
            }
            File.AppendAllText(@"D:\Ушеба\Семестр 8\Мисчета\Картынки\param.csv", bue + '\n');
        }


         void GetPointCanny( List<PointPH> mass, IplImage img2, int num, int step, int cordY, int cordX)
            {
            CvScalar blackColor = new CvScalar(0, 0, 0, 0);
            CvScalar whiteColor = new CvScalar(255, 255,255,0);
            CvSize a = img2.Size;
            img2[cordY, cordX] = whiteColor;
            
            
            if (num >= step)
            {
                num = 0;
                mass.Add(new PointPH(countPoint, cordX, cordY));
                countPoint++;
            }
                num++;
                if (img2[cordY+1,cordX+1]== blackColor)
                {
                    GetPointCanny(mass, img2, num, step, cordY + 1, cordX + 1);
                }

                if (img2[cordY, cordX + 1] == blackColor)
                {
                    GetPointCanny(mass, img2, num, step, cordY, cordX + 1);
                }

                if (img2[cordY-1, cordX + 1] == blackColor)
                {
                    GetPointCanny(mass, img2, num, step, cordY - 1, cordX + 1);
                }

                if (img2[cordY-1, cordX] == blackColor)
                {
                    GetPointCanny(mass, img2, num, step, cordY - 1, cordX);
                }

                if (img2[cordY-1, cordX - 1] == blackColor)
                {
                    GetPointCanny(mass, img2, num, step, cordY - 1, cordX - 1);
                }

                if (img2[cordY, cordX - 1] == blackColor)
                {
                    GetPointCanny(mass, img2, num, step, cordY, cordX - 1);
                }

                if (img2[cordY+1, cordX - 1] == blackColor)
                {
                    GetPointCanny(mass, img2, num, step, cordY + 1, cordX - 1);
                }

                if (img2[cordY+1, cordX ] == blackColor)
                {
                    GetPointCanny(mass, img2, num, step, cordY + 1, cordX);
                }

          
        }
#endregion

        public void Triangulation(List<PointPH> pointsPH)
        {
           List<Vertex> points = new List<Vertex>();
            //List<EdgePH> outEdge = new List<EdgePH>();
            for (int i =0; i<pointsPH.Count; i++)
           points.Add(new Vertex(pointsPH[i].CoordX , pointsPH[i].CoordY));


            // Write out the data set we're actually going to triangulate
            Triangulator angulator = new Triangulator();

            Stopwatch watch = new Stopwatch();
            watch.Start();
            List<Triad> triangles = angulator.Triangulation(points, true);
            watch.Stop();

            Debug.WriteLine(watch.ElapsedMilliseconds + " ms");
           
            // Write the triangulation results in the format suitable for DTT
            for (int i = 0; i < triangles.Count; i++)
            {
                Triad t = triangles[i];
                //outEdge.Add(new EdgePH(t.a, t.b, Math.Sqrt(Math.Pow(pointsPH[t.a].CoordX - pointsPH[t.b].CoordX, 2) + Math.Pow(pointsPH[t.a].CoordY - pointsPH[t.b].CoordY, 2))));
                //outEdge.Add(new EdgePH(t.b, t.c, Math.Sqrt(Math.Pow(pointsPH[t.b].CoordX - pointsPH[t.c].CoordX, 2) + Math.Pow(pointsPH[t.b].CoordY - pointsPH[t.c].CoordY, 2))));
                //outEdge.Add(new EdgePH(t.a, t.c, Math.Sqrt(Math.Pow(pointsPH[t.a].CoordX - pointsPH[t.c].CoordX, 2) + Math.Pow(pointsPH[t.a].CoordY - pointsPH[t.c].CoordY, 2))));

                if (!pointsPH[t.a].PointFriend.Contains(t.b))
                pointsPH[t.a].PointFriend.Add(t.b);
                if (!pointsPH[t.a].PointFriend.Contains(t.c))
                    pointsPH[t.a].PointFriend.Add(t.c);

                if (!pointsPH[t.b].PointFriend.Contains(t.a))
                    pointsPH[t.b].PointFriend.Add(t.a);
                if (!pointsPH[t.b].PointFriend.Contains(t.c))
                    pointsPH[t.b].PointFriend.Add(t.c);

                if (!pointsPH[t.c].PointFriend.Contains(t.a))
                    pointsPH[t.c].PointFriend.Add(t.a);
                if (!pointsPH[t.c].PointFriend.Contains(t.b))
                    pointsPH[t.c].PointFriend.Add(t.b);
            }


            //return outEdge;
        }




        public double[][] GetParam() //функция получения всех параметров
        {


            #region Объявление переменных
            OpenFileDialog open_dialog = new OpenFileDialog();
            open_dialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG;*.tif)|*.BMP;*.JPG;*.GIF;*.PNG;*.tif|All files (*.*)|*.*";
            if (open_dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    s = open_dialog.FileName;
                }
                catch
                {
                    DialogResult rezult = MessageBox.Show("Невозможно открыть выбранный файл",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            
            massHole.Clear();
            massSimplex.Clear();
            D = 10; //минимальный диаметр 
            CountThread = 5;
            MaxX = 0; //Граница массива поиска по Х
            MaxY = 0; // Граница массива поиска по У

            PointCount = 0; //Количество точек
            PointNext = 10; //Количество через которое учитывается каждая точка
            CountIteration = Convert.ToInt32(textBoxIteration.Text); 
           
            massHole = new List<HolPG>();
            IplImage img2;

            img = Cv.LoadImage(s, LoadMode.GrayScale);
            img2 = Cv.LoadImage(s, LoadMode.GrayScale);
            //Cv.ShowImage("оригинал", img);
            Cv.Smooth(img, img, SmoothType.Gaussian, 3);
            img.SaveImage(s + "Bloor.png");
            // Cv.ShowImage("гаус", img);
            Cv.Threshold(img, img, 0, 255, ThresholdType.Otsu);
            //img.SaveImage(s + "Thresh.png");
            Cv.AdaptiveThreshold(img,img2,255, AdaptiveThresholdType.GaussianC);
            //Cv.ShowImage("бинар", img);
            //Cv.Canny(img,img2, 255, 0);
            //Cv.ShowImage("контур", img);
            //Cv.Not(img, img);

            //Cv.ShowImage("инверсия", img);
            //Cv.SaveImage(s+"contur.png", img2);
            //Cv.NamedWindow("asd", WindowMode.StretchImage);

            //Cv.ShowImage("asd", img2);

            //while (true)
            //{
            //    int c = Cv.WaitKey(66);
            //    if (c == 27) break;
            //    Thread.Sleep(50);
            //}



            textBoxRadius.Text = (Convert.ToInt32(Math.Sqrt(Math.Pow(img.Height, 2) + Math.Pow(img.Width, 2))) / CountIteration).ToString();
            if (Convert.ToInt32(textBoxRadius.Text) < 2)
            {
                CountIteration = Convert.ToInt32(Math.Sqrt(Math.Pow(img.Height, 2) + Math.Pow(img.Width, 2)))/2 ;
                textBoxRadius.Text = 2.ToString();
                textBoxIteration.Text = CountIteration.ToString();
            }
           

            PointNext = Convert.ToInt32(TextboxNextPoint.Text);
            D = Convert.ToInt32(textBoxRadius.Text);




            MaxX = img.Width / D + 1; MaxY = img.Height / D + 1;
            PointSearch = new List<int>[MaxY, MaxX];


            List<PointPH> TempAllPoint = new List<PointPH>();
            CvScalar a = new CvScalar(0, 0, 0, 0);
            #endregion

            #region способы выделения точек (устарело)
            //тут у нас точки получаются с верхней и потом через шаг ->
            //int TempNextPoint = 0;
            //for (int y = 0; y < img.Height; y++)
            //{
            //    for (int x = 0; x < img.Width; x++)
            //    {
            //        if (img[y, x] == a)
            //        {

            //            TempAllPoint.Add(new PointPH(PointCount, x, y));

            //            PointCount++;
            //        }
            //    }
            //}
            //for (int i = 0; i < TempAllPoint.Count; i++)
            //{
            //    TempAllPoint[i].DellExtraPoints(i, TempAllPoint);
            //    TempAllPoint[i].Name = i;
            //}
            //тут у нас точки получаются с верхней и потом через шаг <-


            //новый способ-->
            //KeyPoint[] allKeyPoints = new KeyPoint[1];
            //Mat img1 = new Mat(img);

            //Cv2.AdaptiveThreshold(img1, img, 255, AdaptiveThresholdType.GaussianC, ThresholdType.Otsu, 11, 2);
            //Cv2.AdaptiveThreshold(img1, img1, 255, AdaptiveThresholdType.GaussianC, ThresholdType.Binary, 3, 11);
            //Cv2.ImShow("asdf", img1);

            // Mat Mask = new Mat ()






            ////тут у нас точки получаются surf ->
            //Cv.SaveImage("temp.png", img);
            //Mat img12 = Cv2.ImRead("temp.png", 0);
            //List<KeyPoint> pz;
            //SURF surfCPU = new SURF(3000);
            //pz = surfCPU.Detect(img12, null).ToList();


            //for (int i = 0; i < pz.Count; i++)
            //{
            //    GetPointCanny(TempAllPoint, img2, 0, 20, (int)pz[i].Pt.Y, (int)pz[i].Pt.X); //Накидывание точек с шагом
            //    TempAllPoint.Add(new PointPH(countPoint, (int)pz[i].Pt.X, (int)pz[i].Pt.Y));
            //    Cv.AdaptiveThreshold(img, img2, 255, AdaptiveThresholdType.GaussianC);
            //}


            //for (int i = 0; i < TempAllPoint.Count; i++)
            //{
            //    //удаление точек расположенных слишком близко друк к другу
            //    for (int y = (i + 1); y < TempAllPoint.Count; y++)
            //    {
            //        if (Math.Sqrt(Math.Pow(TempAllPoint[i].CoordX - TempAllPoint[y].CoordX, 2) + Math.Pow(TempAllPoint[i].CoordY - TempAllPoint[y].CoordY, 2)) < 20)
            //        {
            //            TempAllPoint.RemoveAt(y);
            //            y--;
            //        }
            //    }
            //}
            ////тут у нас точки получаются surf <-
            #endregion
           
            
            #region тут у нас точки получаются Susan ->
            SusanCornersDetector scd = new SusanCornersDetector();
            List<IntPoint> corners = scd.ProcessImage(img.ToBitmap());




            countPoint = 0;
            //добавление угловых и шаговых точек

            for (int i = 0; i < corners.Count; i++)
            {
                GetPointCanny(TempAllPoint, img2, 0, 5, corners[i].Y, corners[i].X); //Накидывание точек с шагом
                TempAllPoint.Add(new PointPH(countPoint, corners[i].X, corners[i].Y));
                countPoint++;
                //Cv.AdaptiveThreshold(img, img2, 255, AdaptiveThresholdType.GaussianC);
            }


            for (int i = 0; i < TempAllPoint.Count; i++)
            {
                //удаление точек расположенных слишком близко друк к другу
                for (int y = (i + 1); y < TempAllPoint.Count; y++)
                {
                    if (Math.Sqrt(Math.Pow(TempAllPoint[i].CoordX - TempAllPoint[y].CoordX, 2) + Math.Pow(TempAllPoint[i].CoordY - TempAllPoint[y].CoordY, 2)) < 5)
                    {
                        TempAllPoint.RemoveAt(y);
                        y--;
                    }
                }
            }

            List<EdgePH> edges = new List<EdgePH>();
            for (int i = 0; i < TempAllPoint.Count; i++)
            {
                //TempAllPoint[i] = new PointPH(i, TempAllPoint[i].CoordX, TempAllPoint[i].CoordY);
                TempAllPoint[i].Name = i;
            }


           



                    corners = null;
            #endregion

            #region триангуляция ->
            List<int> temp = new List<int>();
            for (int i = 0; i < TempAllPoint.Count; i++)
            {
                TempAllPoint[i].PaddingAllFriend(temp);
            }

           Triangulation(TempAllPoint);
            
            
            // все ребра ->
            for (int i = 0; i < TempAllPoint.Count - 1; i++)
            {
                for (int y = 0; y < TempAllPoint[i].PointFriend.Count; y++)
                {
                    if (TempAllPoint[i].PointFriend[y]>i)
                    edges.Add(new EdgePH(i, TempAllPoint[i].PointFriend[y], Math.Sqrt(Math.Pow(TempAllPoint[TempAllPoint[i].PointFriend[y]].CoordX - TempAllPoint[i].CoordX, 2) + Math.Pow(TempAllPoint[TempAllPoint[i].PointFriend[y]].CoordY - TempAllPoint[i].CoordY, 2))));
                }
            }
            edges.Sort((a1, b1) => -a1.leght.CompareTo(b1.leght));
            //все ребра <-
#endregion






            //edges.Sort((a1, b1) => -a1.leght.CompareTo(b1.leght));

            PointAll = TempAllPoint.ToArray();
            PointCount = PointAll.Length;
            int tempX;
            int tempY;

            #region раньше заполняло поисковую сетку (устарело)
            //for (int i = 0; i < PointCount; i++)
            //{
            //    tempX = PointAll[i].CoordX / D;
            //    tempY = PointAll[i].CoordY / D;

            //    if (PointSearch[tempY, tempX] == null)
            //    {
            //        PointSearch[tempY, tempX] = new List<int>();
            //    }
            //    PointSearch[tempY, tempX].Add(i);
            //}



            //for (int i = 0; i < PointCount; i++)
            //{
            //    PointAll[i].PaddingCels(PointSearch, D, MaxX, MaxY);
            //}
            //раньше заполняло поисковую сетку <-
            #endregion



            CountIteration = Convert.ToInt32(textBoxIteration.Text);
            int DTemp = D;
            
            int tempCountSimplex;
            img = Cv.LoadImage(s, LoadMode.Color);
            for (int i = 0; i < PointCount; i++)
            {
                temp.Add(i);
            }
            massSimplex.Add(new SimplexPG(temp, CountIteration, 0, 0));


            #region  все всем друзья (устарело)
            //for (int i = 0; i < PointCount; i++)
            //{
            //    PointAll[i].PaddingAllFriend(temp);
            //}
            //все всем друзья <-



            //IplImage img1 = Cv.CreateImage(new CvSize(CountIteration+10, 12), BitDepth.F32, 3);
            //IplImage img2 = Cv.CreateImage(new CvSize(CountIteration+10, 12), BitDepth.F32, 3); 
            //for (int i = 0; i < PointCount; i++)
            //{
            //    PointAll[i].UnfamiliarSershCub(D * (101 + 1), D, PointAll, massHole, 101);
            //}
            #endregion

            int tempc = 0;


            #region Создание графика
            chart1.ChartAreas[0].AxisX.Minimum = 0;

            chart1.ChartAreas[0].AxisX.Maximum = img.Width;


            chart1.ChartAreas[0].AxisY.IsInterlaced = false;
            chart1.ChartAreas[0].AxisX.IsInterlaced = false;

            int iv = 0;
            CountIteration = (int)edges[0].leght / D + 1;


            if (Visualisers.Checked || checkBoxVievStep.Checked)
            for (int i1 = 0; i1 < edges.Count; i1++)
            {
                chart1.Series.Add("Ser" + chart1.Series.Count);
                chart1.Series["Ser" + (chart1.Series.Count - 1)].Color = Color.Gray;
                chart1.Series["Ser" + (chart1.Series.Count - 1)].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart1.Series["Ser" + (chart1.Series.Count - 1)].ChartArea = "ChartArea1";
                chart1.Series[(chart1.Series.Count - 1)].Points.DataBindXY(new double[] { PointAll[edges[i1].point1].CoordX, PointAll[edges[i1].point2].CoordX }, new double[] { -PointAll[edges[i1].point1].CoordY, -PointAll[edges[i1].point2].CoordY });
            }
            #endregion


            #region Процесс выделения параметров
            for (int y = CountIteration - 1; y >= 0; y--)
                {
                DTemp = D * (y + 1);
                label4.Text = y + "";


                #region ДыроПоиск

                for (; iv < edges.Count; iv++)
                {
                    if (edges[iv].leght > DTemp)
                    {
                        PointAll[edges[iv].point1].CheckHole(DTemp, D, PointAll, massHole, y, img, PointCount, edges[iv].point2,edges, iv);
                        #region визуализация пошагово
                        if (checkBoxVievStep.Checked)
                        {
                            img = Cv.CreateImage(img.Size, BitDepth.F32, 3);

                            //img
                            //радиусы
                            //for (int i = 0; i < PointCount; i++)
                            //{
                            //    Cv.Circle(img, new CvPoint(PointAll[i].CoordX, PointAll[i].CoordY), DTemp / 2, new CvScalar(0, 100, 0, 0));
                            //}

                            //точки
                            //for (int i = 0; i < PointCount; i++)
                            //{
                            //    Cv.Circle(img, new CvPoint(PointAll[i].CoordX, PointAll[i].CoordY), 2, new CvScalar(255, 255, 0, 0));
                            //}


                            //ребра
                            for (int i1 = CountIteration; i1 < iv+1; i1++)
                            {
                                chart1.Series[i1].Enabled = false;
                            }
                            for (int i1 = iv+1; i1 < edges.Count; i1++)
                            {
                                chart1.Series[i1].Color = Color.Gray;
                                Cv.Line(img, new CvPoint(PointAll[edges[i1].point1].CoordX, PointAll[edges[i1].point1].CoordY), new CvPoint(PointAll[edges[i1].point2].CoordX, PointAll[edges[i1].point2].CoordY), new CvScalar(100, 0, 0, 0));
                                for (int f1 = 0; f1 < massHole.Count; f1++)
                                {
                                    if (massHole[f1].allEdge.Contains(i1))
                                    {
                                        if (f1 < 5)
                                            Cv.Line(img, new CvPoint(PointAll[edges[i1].point1].CoordX, PointAll[edges[i1].point1].CoordY), new CvPoint(PointAll[edges[i1].point2].CoordX, PointAll[edges[i1].point2].CoordY), colorMass[f1]);
                                        else
                                            Cv.Line(img, new CvPoint(PointAll[edges[i1].point1].CoordX, PointAll[edges[i1].point1].CoordY), new CvPoint(PointAll[edges[i1].point2].CoordX, PointAll[edges[i1].point2].CoordY), colorMass[4]);
                                        chart1.Series[i1].Color = Calci[f1 % 10];
                                    }
                                }
                               
                            }
                            CountIteration = iv;
                            chart1.Series[iv+1].Color = Color.OrangeRed;
                            Cv.Line(img, new CvPoint(PointAll[edges[iv+1].point1].CoordX, PointAll[edges[iv + 1].point1].CoordY), new CvPoint(PointAll[edges[iv + 1].point2].CoordX, PointAll[edges[iv + 1].point2].CoordY), new CvScalar(0, 0, 255, 0));







                            //for (int i = 0; i < PointCount; i++)
                            //{
                            //    foreach (int f in PointAll[i].PointFriend)
                            //    {
                            //        Cv.Line(img, new CvPoint(PointAll[f].CoordX, PointAll[f].CoordY), new CvPoint(PointAll[i].CoordX, PointAll[i].CoordY), new CvScalar(100, 0, 0, 0));
                            //        for (int f1 = 0; f1 < massHole.Count; f1++)
                            //        {
                            //            if (massHole[f1].allPoint.Contains(f) && massHole[f1].allPoint.Contains(i))
                            //            {
                            //                if (f1 < 5)
                            //                    Cv.Line(img, new CvPoint(PointAll[f].CoordX, PointAll[f].CoordY), new CvPoint(PointAll[i].CoordX, PointAll[i].CoordY), colorMass[f1]);
                            //                else
                            //                    Cv.Line(img, new CvPoint(PointAll[f].CoordX, PointAll[f].CoordY), new CvPoint(PointAll[i].CoordX, PointAll[i].CoordY), colorMass[4]);

                            //            }
                            //        }


                            //    }
                            //}



                            //точки дыр
                            for (int i = 0; i < PointCount; i++)
                            {
                                for (int g = 0; g < massHole.Count; g++)
                                {
                                    if (massHole[g].PointInHole(i))
                                    {
                                        if (g < 5)
                                            Cv.Circle(img, new CvPoint(PointAll[i].CoordX, PointAll[i].CoordY), 2, colorMass[g]);
                                        else
                                            Cv.Circle(img, new CvPoint(PointAll[i].CoordX, PointAll[i].CoordY), 2, colorMass[4]);
                                        goto asd;
                                    }
                                }
                                Cv.Circle(img, new CvPoint(PointAll[i].CoordX, PointAll[i].CoordY), 2, new CvScalar(255, 255, 0, 0));
                                asd:;
                            }






                            Cv.ShowImage("asd", img);

                            while (true)
                            {
                                int c = Cv.WaitKey(66);
                                if (c == 27) break;
                                Thread.Sleep(50);
                            }
                        }
#endregion
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                #region Поиск дыр (устарело)
                //for (int i1 = 0; i1 < PointCount; i1++)
                //{
                //    if (checkBoxVievStep.Checked)
                //        PointAll[i1].UnfamiliarSershStep(DTemp, D, PointAll, massHole, y, img, PointCount);
                //    else
                //        PointAll[i1].UnfamiliarSersh(DTemp, D, PointAll, massHole, y);
                //}




                //for (int i1 = 0; i1 < massHole.Count; i1++)
                //{
                //    if (massHole[i1].allPoint.Count>0)
                //    for (int i2 = i1+1; i2 < massHole.Count; i2++)
                //    {

                //        if (new HashSet<int>(massHole[i1].allPoint).SetEquals(massHole[i2].allPoint))
                //        {
                //            massHole[i2].startedIteration=y+1;
                //            massHole[i2].allPoint.Clear();
                //        }
                //    }
                //}
                #endregion
                tempc = 0;
                foreach (var d in massHole)
                {
                    if (d.startedIteration < y+1)
                        tempc++;
                }
                label5.Text = massHole.Count+"-"+ tempc;
                #endregion


                #region СимплексПоиск
                tempCountSimplex = massSimplex.Count;
                for (int i = 0; i < tempCountSimplex; i++)
                {
                    if (massSimplex[i].allPoint.Count > 0)
                        massSimplex[i].SearchSimplecsForSimplex(PointAll, massSimplex, y, massSimplex.Count);
                }


                tempc = 0;
                foreach (var d in massSimplex)
                {
                    if (d.startedIteration < y+1)
                        tempc++;
                }
                label6.Text = massSimplex.Count + "-" + tempc;
                #endregion
                #region визуализация 
                if (Visualisers.Checked)
                {
                    img = Cv.CreateImage(img.Size, BitDepth.F64, 4);

                    //img
                    //радиусы
                    //for (int i = 0; i < PointCount; i++)
                    //{
                    //    Cv.Circle(img, new CvPoint(PointAll[i].CoordX, PointAll[i].CoordY), DTemp / 2, new CvScalar(0, 100, 0, 0));
                    //}

                    //точки
                    //for (int i = 0; i < PointCount; i++)
                    //{
                    //    Cv.Circle(img, new CvPoint(PointAll[i].CoordX, PointAll[i].CoordY), 2, new CvScalar(255, 255, 0, 0));
                    //}


                    //ребра

                    //chart1.Series.Clear();
                    if(Visualisers2.Checked)
                    for(int i1 = CountIteration; i1<iv; i1++)
                    {
                        chart1.Series[i1].Enabled = false;
                    }
                    for (int i1 = iv + 1; i1 < edges.Count; i1++)
                    {
                        if (Visualisers2.Checked)
                            chart1.Series[i1].Color = Color.Gray;
                        Cv.Line(img, new CvPoint(PointAll[edges[i1].point1].CoordX, PointAll[edges[i1].point1].CoordY), new CvPoint(PointAll[edges[i1].point2].CoordX, PointAll[edges[i1].point2].CoordY), new CvScalar(100, 0, 0, 0));
                        for (int f1 = 0; f1 < massHole.Count; f1++)
                        {
                            if (massHole[f1].allEdge.Contains(i1))
                            {
                                // Cv.Line(img, new CvPoint(PointAll[edges[i1].point1].CoordX, PointAll[edges[i1].point1].CoordY), new CvPoint(PointAll[edges[i1].point2].CoordX, PointAll[edges[i1].point2].CoordY), new CvScalar(30*(f1+1), 0* (f1 + 1), 0*(f1 + 1), 100));
                                if (f1 < 5)
                                    Cv.Line(img, new CvPoint(PointAll[edges[i1].point1].CoordX, PointAll[edges[i1].point1].CoordY), new CvPoint(PointAll[edges[i1].point2].CoordX, PointAll[edges[i1].point2].CoordY), colorMass[f1]);
                                else
                                    Cv.Line(img, new CvPoint(PointAll[edges[i1].point1].CoordX, PointAll[edges[i1].point1].CoordY), new CvPoint(PointAll[edges[i1].point2].CoordX, PointAll[edges[i1].point2].CoordY), colorMass[4]);
                                //chart1.Series.Add("Ser" + chart1.Series.Count);
                                if (Visualisers2.Checked)
                                    chart1.Series[i1].Color = Calci[f1 % 10];
                                 //CountIteration++;
                            }
                            
                                

                        }
                    }
                    CountIteration = iv;







                    //for (int i = 0; i < PointCount; i++)
                    //{
                    //    foreach (int f in PointAll[i].PointFriend)
                    //    {
                    //        Cv.Line(img, new CvPoint(PointAll[f].CoordX, PointAll[f].CoordY), new CvPoint(PointAll[i].CoordX, PointAll[i].CoordY), new CvScalar(100, 0, 0, 0));
                    //        for (int f1 = 0; f1 < massHole.Count; f1++)
                    //        {
                    //            if (massHole[f1].allPoint.Contains(f) && massHole[f1].allPoint.Contains(i))
                    //            {
                    //                if (f1 < 5)
                    //                    Cv.Line(img, new CvPoint(PointAll[f].CoordX, PointAll[f].CoordY), new CvPoint(PointAll[i].CoordX, PointAll[i].CoordY), colorMass[f1]);
                    //                else
                    //                    Cv.Line(img, new CvPoint(PointAll[f].CoordX, PointAll[f].CoordY), new CvPoint(PointAll[i].CoordX, PointAll[i].CoordY), colorMass[4]);

                    //            }
                    //        }


                    //    }
                    //}



                    //точки дыр
                    for (int i = 0; i < PointCount; i++)
                    {
                        for (int g = 0; g < massHole.Count; g++)
                        {
                            if (massHole[g].PointInHole(i))
                            {
                                if (g < 5)
                                    Cv.Circle(img, new CvPoint(PointAll[i].CoordX, PointAll[i].CoordY), 2, colorMass[g]);
                                else
                                    Cv.Circle(img, new CvPoint(PointAll[i].CoordX, PointAll[i].CoordY), 2, colorMass[4]);
                                goto asd;
                            }
                        }
                        Cv.Circle(img, new CvPoint(PointAll[i].CoordX, PointAll[i].CoordY), 2, new CvScalar(255, 255, 0, 0));
                    asd:;
                    }






                    Cv.ShowImage("asd", img);

                    while (true)
                    {
                        int c = Cv.WaitKey(66);
                        if (c == 27) break;
                        Thread.Sleep(50);
                    }
                }
                #endregion
                #region  визуализация - Дикая , отдельно мониторит дыры



                if (checkBoxWildViev.Checked)
                {
                    img = Cv.CreateImage(img.Size, BitDepth.F32, 3);
                    int ass = 5;
                    IplImage[] img1 = new IplImage[ass];
                    for (int i = 0; i < ass; i++)
                        img1[i] = Cv.CreateImage(img.Size, BitDepth.F32, 3);
                    //радиусы
                    //for (int i = 0; i < PointCount; i++)
                    //{
                    //    Cv.Circle(img, new CvPoint(PointAll[i].CoordX, PointAll[i].CoordY), DTemp / 2, new CvScalar(0, 100, 0, 0));
                    //}

                    


                    //ребра
                    for (int i = 0; i < PointCount; i++)
                    {
                        foreach (int f in PointAll[i].PointFriend)
                        {
                            for (int i1 = 0; i1 < ass; i1++)
                                //Cv.Line(img1[i1], new CvPoint(PointAll[f].CoordX, PointAll[f].CoordY), new CvPoint(PointAll[i].CoordX, PointAll[i].CoordY), new CvScalar(100, 0, 0, 0));
                            Cv.Line(img, new CvPoint(PointAll[f].CoordX, PointAll[f].CoordY), new CvPoint(PointAll[i].CoordX, PointAll[i].CoordY), new CvScalar(100, 0, 0, 0));
                            for (int f1 = 0; f1 < massHole.Count; f1++)
                            {
                                if (massHole[f1].allPoint.Contains(f) && massHole[f1].allPoint.Contains(i))
                                {
                                    if (f1 < 5)
                                    {
                                        Cv.Line(img, new CvPoint(PointAll[f].CoordX, PointAll[f].CoordY), new CvPoint(PointAll[i].CoordX, PointAll[i].CoordY), colorMass[f1]);
                                        Cv.Line(img1[f1], new CvPoint(PointAll[f].CoordX, PointAll[f].CoordY), new CvPoint(PointAll[i].CoordX, PointAll[i].CoordY), colorMass[f1]);
                                    }
                                    else
                                    {
                                        Cv.Line(img, new CvPoint(PointAll[f].CoordX, PointAll[f].CoordY), new CvPoint(PointAll[i].CoordX, PointAll[i].CoordY), colorMass[4]);
                                        Cv.Line(img1[f1], new CvPoint(PointAll[f].CoordX, PointAll[f].CoordY), new CvPoint(PointAll[i].CoordX, PointAll[i].CoordY), colorMass[4]);
                                    }
                                }
                            }


                        }
                    }
                    //точки
                    for (int i = 0; i < PointCount; i++)
                    {
                        Cv.Circle(img, new CvPoint(PointAll[i].CoordX, PointAll[i].CoordY), 2, new CvScalar(0, 0, 255, 0));
                        for (int i1 = 0; i1 < ass; i1++)
                            Cv.Circle(img1[i1], new CvPoint(PointAll[i].CoordX, PointAll[i].CoordY), 2, new CvScalar(0, 0, 255, 0));
                    }
                    //точки дыр
                    for (int i = 0; i < PointCount; i++)
                    {
                        for (int g = 0; g < massHole.Count; g++)
                        {
                            if (massHole[g].PointInHole(i))
                            {
                                Cv.Circle(img, new CvPoint(PointAll[i].CoordX, PointAll[i].CoordY), 2, colorMass[g]);
                                goto asd;
                            }
                        }
                        Cv.Circle(img, new CvPoint(PointAll[i].CoordX, PointAll[i].CoordY), 2, new CvScalar(0, 0, 255, 0));
                    asd:;
                    }





                    for (int i1 = 0; i1 < ass; i1++)
                        Cv.ShowImage("asd" + i1, img1[i1]);
                    Cv.ShowImage("asd", img);

                    while (true)
                    {
                        int c = Cv.WaitKey(66);
                        if (c == 27) break;
                        Thread.Sleep(50);
                    }
                }
                #endregion

                #region визуализация  (устарело)
                //визуализация графика количества дыр от итерации
                //int a1 = 0;

                //for (int g = 0; g < massSimplex.Count; g++)
                //{
                //    if (massSimplex[g].startedIteration <= y)
                //    {
                //        a1++;
                //    }
                //}
                //Cv.Line(img1, new CvPoint((y + 5), img1.Height), new CvPoint((y + 5), img1.Height - a1 ), new CvScalar(0, 100, 0, 0));
                //a1 = 0;
                //for (int g = 0; g < massHole.Count; g++)
                //{
                //    if (massHole[g].startedIteration <= y)
                //    {
                //        a1++;
                //    }
                //}
                //Cv.Line(img2, new CvPoint((y + 5), img1.Height), new CvPoint((y + 5), img1.Height - a1), new CvScalar(100, 0, 0, 0));
                //a1 = 0;
#endregion

            }
            #endregion
            //img1.SaveImage("C:\\Users\\sas9mba\\Desktop\\Тести\\Pict\\" + open_dialog.SafeFileName.Split('.')[0] + "sim.png");
            //img2.SaveImage("C:\\Users\\sas9mba\\Desktop\\Тести\\Pict\\" + open_dialog.SafeFileName.Split('.')[0] + "hole.png");

            #region Подготовка параметров к сохранению


            #region удаление дыр и компонент с жизнью меньше чем minParamLive и сортировка
            for (int i =0; i<massHole.Count; i++)
            {
                if (massHole[i].Live()<minParamLive)
                {
                    massHole.RemoveAt(i);
                    i--;
                }
            }
            for (int i = 0; i < massSimplex.Count; i++)
            {
                if (massSimplex[i].Live() < minParamLive)
                {
                    massSimplex.RemoveAt(i);
                    i--;
                }
            }

            HolPG tempHole;
            for (int i =0; i<massHole.Count; i++)
            {
                for (int y = i+1; y < massHole.Count; y++)
                {
                    if(massHole[i].Live()< massHole[y].Live())
                    {
                        tempHole = massHole[i];
                        massHole[i] = massHole[y];
                        massHole[y] = tempHole;
                    }
                }
            }
            SimplexPG tempSimplex;
            for (int i = 0; i < massSimplex.Count; i++)
            {
                for (int y = i + 1; y < massSimplex.Count; y++)
                {
                    if (massSimplex[i].Live() < massSimplex[y].Live())
                    {
                        tempSimplex = massSimplex[i];
                        massSimplex[i] = massSimplex[y];
                        massSimplex[y] = tempSimplex;
                    }
                }
            }



            #endregion

            double[][] OutMass = new double[8][];
            OutMass[0] = new double[massHole.Count];
            OutMass[1] = new double[massHole.Count];
            OutMass[2] = new double[massHole.Count];
            OutMass[3] = new double[massHole.Count];
            OutMass[4] = new double[massSimplex.Count];
            OutMass[5] = new double[massSimplex.Count];
            OutMass[6] = new double[massSimplex.Count];
            OutMass[7] = new double[massSimplex.Count];


            double liveBaseLine = massHole[0].Live();

            for (int i =0; i<massHole.Count; i++)
            {
                OutMass[0][i] = (massHole[i].startedIteration- massHole[0].startedIteration)/liveBaseLine;//относительная точка начала
                OutMass[1][i] = massHole[i].Live() / liveBaseLine;//относительная длина
                OutMass[2][i] = massHole[i].coordX;//координата x
                OutMass[3][i] = massHole[i].coordY;//координата y

            }

            liveBaseLine = massSimplex[0].Live();
            for (int i = 0; i < massSimplex.Count; i++)
            {
                OutMass[4][i] = (massSimplex[i].startedIteration - massSimplex[0].startedIteration) / liveBaseLine;//относительная точка начала
                OutMass[5][i] = massSimplex[i].Live() / liveBaseLine;//относительная длина
                OutMass[6][i] = 0;//координата x
                OutMass[7][i] = 0;//координата y
            }



            #region Тут сортировка массивов дыр и строк. Вывод их в баркод (отключено)
            //Array.Sort(OutMass[0], (x, y) => -x.CompareTo(y));
            //Array.Sort(OutMass[1], (x, y) => -x.CompareTo(y));

            //HolPG asdd;
            //for (int i = 0; i < massHole.Count; i++)
            //{
            //    for (int i1 = i+1; i1 < massHole.Count; i1++)
            //    {
            //        if (massHole[i].startedIteration>massHole[i1].startedIteration)
            //        {
            //            asdd = new HolPG(massHole[i1]);
            //            massHole[i1] = new HolPG(massHole[i]);
            //            massHole[i] = new HolPG(asdd);
            //        }
            //    }


            //}

            //    int b = 2;
            //IplImage imgSave = Cv.CreateImage(new CvSize(100, 200), BitDepth.F32, 0);
            //for (int i = 0; i < massHole.Count; i++)
            //{
            //    Cv.Line(imgSave, new CvPoint(1+ (massHole[i].endIteration + (massHole[i].endIteration - massHole[i].startedIteration) * 2), imgSave.Height - b), new CvPoint(massHole[i].startedIteration, imgSave.Height - b), colorMass[4]);
            //    b += 2;

            //}
            //imgSave.SaveImage(s + ".png");
            //b = 2;
            //imgSave = Cv.CreateImage(new CvSize(100, 200), BitDepth.F32, 0);
            //for (int i = 0; i < massHole.Count; i++)
            //{
            //    if (massHole[i].Live() > 3)
            //    {
            //        Cv.Line(imgSave, new CvPoint(1 + (massHole[i].endIteration+(massHole[i].endIteration- massHole[i].startedIteration)*2), imgSave.Height - b), new CvPoint(massHole[i].startedIteration, imgSave.Height - b), colorMass[4]);

            //        b += 2;
            //    }


            //}



            ////if (massHole[i].Live() > 3)

            //    imgSave.SaveImage(s + "NoNoiz.png");

            ////график распределения длин жизни по убыванию
            ////IplImage img3 = Cv.CreateImage(new CvSize(CountIteration + 10, 100), BitDepth.F32, 3);
            ////IplImage img4 = Cv.CreateImage(new CvSize(CountIteration + 10, 100), BitDepth.F32, 3);

            ////for (int g = 0; g < OutMass[1].Length; g++)
            ////{
            ////    Cv.Line(img3, new CvPoint((g + 5), img3.Height), new CvPoint((g + 5), img3.Height - (int)OutMass[1][g]), new CvScalar(0, 100, 0, 0));
            ////}


            ////for (int g = 0; g < OutMass[0].Length; g++)
            ////{
            ////    Cv.Line(img4, new CvPoint((g + 5), img3.Height), new CvPoint((g + 5), img3.Height - (int)OutMass[0][g]), new CvScalar(100, 0, 0, 0));
            ////}

            ////img3.SaveImage( @"C:\Users\sas9mba\Desktop\Тести\Pict\"+ open_dialog.SafeFileName.Split('.')[0] + "LivSim.png");
            ////img4.SaveImage(@"C:\Users\sas9mba\Desktop\Тести\Pict\" + open_dialog.SafeFileName.Split('.')[0] + "LivHol.png");
            #endregion



            return OutMass;

#endregion

        }


        //Делить меньшее на большее
        double FanDivPar(int i, int j, double[] Param)
        {
            double a = 0;
            if (Param[j] > Param[i])
                a = (Param[i] / Param[j]);
            else
                a = (Param[j] / Param[i]);
            return a;
        }


        private void Poisk_Click(object sender, EventArgs e)
        {


            int vin = 0;
            double max = 0;
            
            var bue = GetParam();
           
            ParamObjPH paramObj = new ParamObjPH("", bue[0], bue[1], bue[2], bue[3], bue[4], bue[5], bue[6], bue[7]);
            int countTemp = 0;
            string [] allClass = File.ReadAllLines("param.txt");
            ParamObjPH [] allParamObj = new ParamObjPH[allClass.Length/9];
            double[] allProcent = new double[allClass.Length];
            string ext = "";

            for (int i = 0; i < allClass.Length; i+=9)
            {
                allParamObj[countTemp] = new ParamObjPH(allClass[i],
                    Array.ConvertAll(allClass[i+1].Split(';'), new Converter<string, double>(PointFToPoint)),
                    Array.ConvertAll(allClass[i+2].Split(';'), new Converter<string, double>(PointFToPoint)),
                    Array.ConvertAll(allClass[i+3].Split(';'), new Converter<string, double>(PointFToPoint)),
                    Array.ConvertAll(allClass[i+4].Split(';'), new Converter<string, double>(PointFToPoint)),
                    Array.ConvertAll(allClass[i+5].Split(';'), new Converter<string, double>(PointFToPoint)),
                    Array.ConvertAll(allClass[i+6].Split(';'), new Converter<string, double>(PointFToPoint)),
                    Array.ConvertAll(allClass[i+7].Split(';'), new Converter<string, double>(PointFToPoint)),
                    Array.ConvertAll(allClass[i+8].Split(';'), new Converter<string, double>(PointFToPoint)));
                countTemp++;
            }

            for (int i =0; i< allParamObj.Length; i++)
            {
               ext += allParamObj[i].ComparisonBetti(paramObj)+" - "+allParamObj[i].nameObject+"\n\r";
            }
            MessageBox.Show(ext);

            #region поиск  (устарел 18/09/17)

            //for (int i = 0; i < allClass.Length; i += 2)
            //{
            //    allParam[countTemp] = new ParamObjPH(Array.ConvertAll(allClass[i].Split(';'), new Converter<string, double>(PointFToPoint)), Array.ConvertAll(allClass[i + 1].Split(';'), new Converter<string, double>(PointFToPoint)), Convert.ToInt32(textBoxClaster.Text));
            //    countTemp++;
            //}
            //string exite = "";
            //double temp = 0;


            //for (int i=0; i<allParam.Length;i++)
            //{
            //    allProcent[i] = new double[2];

            //    //нормализация ->
            //    allParam[i].Normalized(paramObj);
            //    allParam[i].Calculet();
            //    //нормализация <-


            //    temp = allParam[i].DifParamHole(paramObj);
            //    allProcent[i][0] = temp;

            //    temp += allParam[i].DifParamSimplex(paramObj);
            //    allProcent[i][1] = allParam[i].DifParamSimplex(paramObj);


            //    exite += (i + 1) + "-" + allProcent[i][0] + "+"+ allProcent[i][1]+"="+ (allProcent[i][0]  + allProcent[i][1])+"\n";

            //    if (temp>max)
            //    {
            //        max = temp;
            //        vin = i + 1;
            //    }
            //}



            //MessageBox.Show((vin)+"\n"+exite);
            #endregion


            #region самый первый поиск (устарел)
            //double [] precentParam = new double[allClass.Length];
            //int max = 0;
            //double temp = 0;
            //double[,] tempPr = new double[allClass.Length, bue.Length];
            //for (int i = 0; i < allClass.Length; i++)
            //{
            //    for (int j = 0; j < bue.Length; j++)
            //    {
            //        if (Math.Abs(bue[j]) < Math.Abs(allParam[i][j]))
            //        {
            //            temp = (bue[j] / allParam[i][j]);

            //        }
            //        else
            //        {
            //            if (bue[j] == allParam[i][j])
            //                temp = 1;
            //            else
            //            temp = ( allParam[i][j]/ bue[j]);
            //        }


            //        tempPr[i, j] = temp;

            //        if(j==10)
            //        precentParam[i] += Math.Abs(temp) * 20;
            //        else 
            //        if (j == 12 || j == 11 || j == 4)
            //            precentParam[i] += Math.Abs(temp) * 15;
            //        else 
            //        if (j == 5 || j == 6)
            //            precentParam[i] += Math.Abs(temp) * 10;
            //        else 
            //        if (j == 0 || j == 7)
            //            precentParam[i] += Math.Abs(temp) * 3;
            //        else
            //            precentParam[i] += Math.Abs(temp) * 2;




            //    }
            //    if (precentParam[max] < precentParam[i])
            //    {
            //        max = i;
            //    }
            //}

            //for(int i =0; i<precentParam.Length;i++)
            //{
            //    exite += (i + 1) + "-" + precentParam[i] + "%\r\n";
            //}

            //MessageBox.Show(exite);
            //MessageBox.Show((max+1)+"");
            #endregion
        }


        public static double PointFToPoint(string pf)
        {
            return Convert.ToDouble(pf);
        }
    }
}
