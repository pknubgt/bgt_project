using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Crossing
{
    private List<string> crossinginfo = new List<string>();
    private double psv = 1.0;

    public double PSV
    {
        get { return psv; }
        set { psv = value; }
    }

    public void checkCrossing(Figure f6, Figure f7)
    {
        float n = 0, s = 0; ;
        float tmp = 0;
        int[] count = new int[2];
        int[] result = new int[2];

        for (int i = 0; i < 2; i++)
        {

            if (i == 0)
            {
                n = f6.PartPoints.ToArray().Length;

                for (int j = 0; j < n; j++)
                {
                    tmp += f6.PartPoints[j];
                    s = f6.StandardDeviation;

                    if (j % (n / 3) == 0)
                    {
                        float compare = tmp / (n / 3);
                        if (compare > f6.TotalPressure + 2 * s || compare < f6.TotalPressure - 2 * s)
                            count[0]++;
                    }
                }
            }
            else if (i == 1)
            {
                n = f7.PartPoints.ToArray().Length;

                for (int j = 0; j < n; j++)
                {
                    tmp += f7.PartPoints[j];
                    s = f7.StandardDeviation;

                    float compare = tmp / (n / 3);
                    if (compare > f7.TotalPressure + 2 * s || compare < f7.TotalPressure - 2 * s)
                        count[0]++;
                }
            }

            if (count[i] >= 3) //심하다
            {
                result[i] = 10;
                crossinginfo.Add("도형" + i + " - 심하다");
            }
            else if (count[i] == 2)//보통이다
            {
                result[i] = 7;
                crossinginfo.Add("도형" + i + " - 보통이다");
            }
            else if (count[i] == 1)//경미하다
            {
                result[i] = 4;
                crossinginfo.Add("도형" + i + " - 경미하다");
            }
            else //없다
            {
                result[i] = 1;
                crossinginfo.Add("도형" + i + " - 이상없음");
            }
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

    public List<string> CrossingReport()
    {
        return crossinginfo;
    }
}