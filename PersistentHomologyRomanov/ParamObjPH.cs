using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistentHomologyRomanov
{
    /// <summary>
    /// класс содержит параметры объекта
    /// </summary>
   
    public class ParamObjPH
    {
        /// <summary>Имя объекта</summary>
        public string nameObject;

        /// <summary>точка отсчета начала линии жизни дыры</summary>
        public double[] paramHole; //точки относительного начала дыр
        /// <summary>длина линии жизни дыры</summary>
        public double[] paramHole2;//точки относительной длины дыр
        /// <summary>координата х дыры</summary>
        public double[] paramHoleX; //координата Х дыр
        /// <summary>Координата у дыры</summary>
        public double[] paramHoleY;//координата У дыр
        /// <summary>точка отсчета начала линии жизни компоненты</summary>
        public double[] paramSimplex; //точки относительного начала компонент
        /// <summary>длина линии жизни компоненты</summary>
        public double[] paramSimplex2;//точки относительной длины компонент
        /// <summary>координата х компоненты</summary>
        public double[] paramSimplexX;//координата Х Компонент
        /// <summary>координата у компоненты</summary>
        public double[] paramSimplexY;//координата У Компонент
        /// <summary>сумма параметров дыр</summary>
        public double sumParamHole = 0;
        /// <summary>сумма параметров компонент</summary>
        public double sumParamSimplex = 0;
        /// <summary>проценты соответствия по дырам</summary>
        public double[] procentParamHole;
        /// <summary>проценты соответствия по компонентам</summary>
        public double[] procentParamSimplex;
        public ParamObjPH(string name, double[] hole, double[] simplex, int met = 0)
        {
            if (met == 0)
            {
                paramHole = hole;
                paramSimplex = simplex;
            }
            if (met == 1)
            {
                int i ;
                double tempSum;
                tempSum = 0;
                i = 0;
                for (; i<hole.Length; i++)
                {
                    tempSum += hole[i];
                }
                tempSum = tempSum / i;
                List<double> tempList = new List<double>(hole);
                for(i=0;i< tempList.Count; i++)
                {
                    if(tempList[i]<tempSum)
                    {
                        tempList.RemoveAt(i);
                        i--;
                    }
                }
                paramHole = tempList.ToArray();



                tempSum = 0;
                i = 0;
                for (; i < simplex.Length; i++)
                {
                    tempSum += simplex[i];
                }
                tempSum = tempSum / i;
               tempList = new List<double>(simplex);
                for (i = 0; i < tempList.Count; i++)
                {
                    if (tempList[i] < tempSum)
                    {
                        tempList.RemoveAt(i);
                        i--;
                    }
                }
                paramSimplex = tempList.ToArray();
            }
            if (met == 2)
            {
                int i;
                double tempSum;
                tempSum = 0;
                i = 0;
                for (; i < hole.Length; i++)
                {
                    tempSum += hole[i];
                }
                tempSum = tempSum / i;
                List<double> tempList = new List<double>(hole);
                for (i = 0; i < tempList.Count; i++)
                {
                    if (tempList[i] < tempSum)
                    {
                        tempList.RemoveAt(i);
                        i--;
                    }
                }
                paramHole = new double[1] { tempList.Count };



                tempSum = 0;
                i = 0;
                for (; i < simplex.Length; i++)
                {
                    tempSum += simplex[i];
                }
                tempSum = tempSum / i;
                tempList = new List<double>(simplex);
                for (i = 0; i < tempList.Count; i++)
                {
                    if (tempList[i] < tempSum)
                    {
                        tempList.RemoveAt(i);
                        i--;
                    }
                }
                paramSimplex = new double[1] { tempList.Count };
            }
            procentParamHole = new double[paramHole.Length];
            procentParamSimplex = new double[paramSimplex.Length];
            for (int i = 0; i < paramHole.Length; i++)
                sumParamHole += paramHole[i];

            for (int i = 0; i < paramSimplex.Length; i++)
                sumParamSimplex += paramSimplex[i];

            for (int i = 0; i < paramHole.Length; i++)
                procentParamHole[i] = Otnoshenie(sumParamHole, paramHole[i]);

            for (int i = 0; i < paramSimplex.Length; i++)
                procentParamSimplex[i] = Otnoshenie(sumParamSimplex, paramSimplex[i]);




        }


        public ParamObjPH(string name, double[] hole1, double[] hole2, double[] holeX, double[] holeY, double[] simplex1, double[] simplex2, double[] simplexX, double[] simplexY)
        {
            nameObject = name;
            paramHole = hole1;
            paramHole2 = hole2;
            paramHoleX = holeX;
            paramHoleY = holeY;
            paramSimplex = simplex1;
            paramSimplex2 = simplex2;
            paramSimplexX = simplexX;
            paramSimplexY = simplexY;
    }
        /// <summary>
        /// Возвращает наилучший процент по схожести дыр
        /// </summary>
        /// <param name="param">Объект поступивший в систему</param>
        /// <returns></returns>
        public double ComparisonBetti(ParamObjPH param)
        {
            double outProcent = 0, tempOutProcent = 0;
            int indexBest = 0;
            double prov = 0;
            double[] outDebag = new double[paramHole.Length];
            for (int i = 0; i<param.paramHole2.Length;i++)
            {
                prov += param.paramHole2[i];
            }

            for (int i =0; i< paramHole.Length; i++)
            {
                List<double> tempParamHole1 = new List<double>();
                List<double> tempParamHole2 = new List<double>();
                for (int y =0; y<paramHole.Length;y++)
                {
                    tempParamHole1.Add((paramHole[y] - paramHole[i]) / paramHole2[i]);
                }
                for (int y = 0; y < paramHole2.Length; y++)
                {
                    tempParamHole2.Add(paramHole2[y] / paramHole2[i]);
                }

                //если хоть в одном дыра 1 а в другом от одной то 100%
                if ((param.paramHole.Length == 1 && tempParamHole2.Count >= 1))
                    return 100;

                if ((param.paramHole.Length >= 1 && tempParamHole2.Count == 0))
                    return 0;

                

                tempOutProcent = CalculetRecursion(tempParamHole1, tempParamHole2, param.paramHole, param.paramHole2);

                tempParamHole1.RemoveAt(i);
                tempParamHole2.RemoveAt(i);

                outDebag[i] = tempOutProcent;
                if (outProcent< tempOutProcent)
                {
                    outProcent = tempOutProcent;
                    indexBest = i;
                }
            }

            return outProcent/prov*100;
        }


        /// <summary>Возвращает наилучший процент по схожести компонент (нужно было сделать универсальную функцию но я ленив)</summary>
        public double ComparisonBettiKomponenta(ParamObjPH param)
        {
            double outProcent = 0, tempOutProcent = 0;
            int indexBest = 0;
            double prov = 0;
            double[] outDebag = new double[paramSimplex.Length];
            for (int i = 1; i < param.paramSimplex2.Length; i++)
            {
                prov += param.paramSimplex2[i];
            }

            for (int i = 0; i < paramSimplex.Length; i++)
            {
                List<double> tempparamSimplex1 = new List<double>();
                List<double> tempparamSimplex2 = new List<double>();
                for (int y = 0; y < paramSimplex.Length; y++)
                {
                    tempparamSimplex1.Add((paramSimplex[y] - paramSimplex[i]) / paramSimplex2[i]);
                }
                for (int y = 0; y < paramSimplex2.Length; y++)
                {
                    tempparamSimplex2.Add(paramSimplex2[y] / paramSimplex2[i]);
                }

                //если хоть в одном дыра 1 а в другом от одной то 100%
                //if ((param.paramSimplex.Length == 1 && tempparamSimplex2.Count >= 1))
                //    return 100;

                tempparamSimplex1.RemoveAt(i);
                tempparamSimplex2.RemoveAt(i);

                tempOutProcent = CalculetRecursion(tempparamSimplex1, tempparamSimplex2, param.paramSimplex, param.paramSimplex2);
                outDebag[i] = tempOutProcent;
                if (outProcent < tempOutProcent)
                {
                    outProcent = tempOutProcent;
                    indexBest = i;
                }
            }

            return outProcent / prov * 100;
        }


        //меньшее на большее
        double DivPar(double i, double j)
        {
            double a = 0;
            if (j > i)
                a = (i / j);
            else
                a = (j / i);
            return a;
        }

        /// <summary>
        /// Метод сравнивает векторы параметров числа бети (два вектора с каждого объекта)
        /// </summary>
        /// <param name="ListParamBase1">начало линии жизни 1</param>
        /// <param name="LlistParamBase2">длина линии жизни 1</param>
        /// <param name="LlistParamIn1">начало линии жизни 2 искомый</param>
        /// <param name="LlistParamIn2">длина линии жизни 2 искомый</param>
        /// <returns></returns>
        public double CalculetRecursion(List<double> ListParamBase1, List<double> LlistParamBase2, double[] LlistParamIn1, double [] LlistParamIn2)
        {
            List<double> TempLlistParamIn1 = new List<double>(LlistParamIn1);
            List<double> TempLlistParamIn2 = new List<double>(LlistParamIn2);
            List<double> TempListParamBase1 = new List<double>(ListParamBase1);
            List<double> TempListParamBase2 = new List<double>(LlistParamBase2);


            double Procent = 0, tempProcent = 0, outProcent = 0;
            int indexBest = 0;

            for (int i =0; i< TempLlistParamIn1.Count; i++)
            {
                tempProcent = 0; Procent = 0;

                for (int y = 0; y < TempListParamBase1.Count; y++)
                {
                    tempProcent = DivPar((Math.Abs(TempListParamBase1[y]- TempLlistParamIn1[i])) + (Math.Abs(TempListParamBase2[y]- TempLlistParamIn2[i]))+1,1);
                    if (tempProcent>Procent)
                    {
                        Procent = tempProcent;
                        indexBest = y;
                    }
                }
                outProcent += Procent* TempLlistParamIn2[i];
                if (TempListParamBase1.Count > 0)
                {
                    TempListParamBase1.RemoveAt(indexBest);
                    TempListParamBase2.RemoveAt(indexBest);
                }
                indexBest = 0;

            }





            return outProcent;
        }

        //поиск кратчайшего пути с полным обходом
        public double CalculetFullRecursion(List<double> listParamBase1, List<double> listParamBase2, List<double> listParamIn1, List<double> listParamIn2, int indexInParam = 1)
        {
            if (indexInParam == listParamIn1.Count || listParamBase1.Count < 1)
                return 0;




            return 0;
        }








        public void Calculet()
        {
            sumParamHole = 0;
            sumParamSimplex = 0;

            procentParamHole = new double[paramHole.Length];
            procentParamSimplex = new double[paramSimplex.Length];
            for (int i = 0; i < paramHole.Length; i++)
                sumParamHole += paramHole[i];

            for (int i = 0; i < paramSimplex.Length; i++)
                sumParamSimplex += paramSimplex[i];

            for (int i = 0; i < paramHole.Length; i++)
                procentParamHole[i] = Otnoshenie(sumParamHole, paramHole[i]);

            for (int i = 0; i < paramSimplex.Length; i++)
                procentParamSimplex[i] = Otnoshenie(sumParamSimplex, paramSimplex[i]);
        }

        public static double Otnoshenie(double par1, double par2)
        {
            if (par1 < par2)
            {
                return par1 / par2;
            }
            else
            {
                if (par1 == par2)
                {
                    return 1;
                }
                else
                    return par2 / par1;
            }
        }

        public void Normalized(ParamObjPH param)
        {
            double a = param.paramHole[0]/ paramHole[0];
            double b = param.paramSimplex[0]/paramSimplex[0];

            for (int i = 0; i< paramHole.Length; i++)
            {
                paramHole[i] *=a;
            }
            for (int i = 0; i < paramSimplex.Length; i++)
            {
                paramSimplex[i] *= b;
            }
        }





            public double DifParamHole(ParamObjPH param)
        {
            double outProc = 0;
            double[] llk;
            double[] llk1;
            int countIteration = 0;
            if (param.paramHole.Length > paramHole.Length)
            {
                countIteration = paramHole.Length;
                llk = param.procentParamHole;
                llk1 = param.paramHole;
            }
            else
            {
                countIteration = param.paramHole.Length;
                llk = procentParamHole;
                llk1 = paramHole;
            }
            if (llk1.Length == 1 && llk1[0]==0)
                return 0.5;

            for (int i = 0; i < countIteration; i++)
                {

                    outProc += Otnoshenie(paramHole[i], param.paramHole[i]) * llk[i];

                }
            
           


            return outProc*75;
        }



        public double DifParamHoleEps(ParamObjPH param)
        {
            double outProc = 0;
            int countIteration = 0;
            if (param.paramHole.Length > paramHole.Length)
            {
                countIteration = paramHole.Length;
               
            }
            else
            {
                countIteration = param.paramHole.Length;
                
            }


            for (int i = 0; i < countIteration; i++)
            {

                outProc += Math.Pow(Math.Abs(paramHole[i]-param.paramHole[i]),3);

            }

            outProc = Math.Pow(outProc, 1.0 / 3.0);




            return outProc;
        }




        public double DifParamSimplex(ParamObjPH param)
        {
            double[] llk;
            double outProc = 0;
            int countIteration = 0;
            if (param.paramSimplex.Length > paramSimplex.Length)
            {
                countIteration = paramSimplex.Length;
                llk = param.procentParamSimplex;
            }
            else
            {
                countIteration = param.paramSimplex.Length;
                llk = procentParamSimplex;
            }

            for (int i = 0; i < countIteration; i++)
                {

                    outProc += Otnoshenie(paramSimplex[i], param.paramSimplex[i]) * llk[i];

                }
     


            return outProc*25;
        }




    }
}
