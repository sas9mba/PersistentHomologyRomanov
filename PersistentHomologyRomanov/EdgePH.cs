using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistentHomologyRomanov
{
    public class EdgePH
    {
        public int point1;
        public int point2;
        public double leght;
        public string obraz;
        public bool inEdge;

        public  EdgePH (int point1, int point2, double leght)
        {
            this.point1 = point1;
            this.point2 = point2;
            this.leght = leght;
            inEdge = false;

            if (point1<point2)
            {
                obraz = point1 + "-" + point2;
            }
            else
            {
                obraz = point2 + "-" + point1;
            }
        }


    }
}
