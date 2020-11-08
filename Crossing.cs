using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Crossing
{
    private double psv = 1.0;

    public double PSV
    {
        get { return psv; }
        set { psv = value; }
    }
    public void checkCrossing(Figure f6, Figure f7)
    {
        float m = 0;
        float s = 0;
        float compare = 0;
        int[] result = new int[2];

        for (int i = 0; i < 2; i++)
        {
            if (i == 0)
            {
                m = f6.TotalPressure;
                s = f6.StandardDeviation;
                compare = f6.PartPressure;
            }
            else if (i == 1)
            {
                m = f7.TotalPressure;
                s = f7.StandardDeviation;
                compare = f7.PartPressure;
            }

            if (compare > m + s) //심하다
                result[i] = 10;
            else if (compare <= m + s && compare > m + (s / 2))//보통이다
                result[i] = 7;
            else if (compare <= m + (s / 2) && compare > m + (s / 4))//경미하다
                result[i] = 4;
            else //없다
                result[i] = 1;
        }

        setCheckCrossingScore(result);
    }

    private void setCheckCrossingScore(int[] r)
    {
        if (r.Average() > 0 && r.Average() < 4)
            psv = 1;
        else if (r.Average() >= 4 && r.Average() < 7)
            psv = 4;
        else if (r.Average() >= 7 && r.Average() < 10)
            psv = 7;
        else
            psv = 10;
    }
}
