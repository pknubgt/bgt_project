using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class UseSpace
{
    private double psv = 1.0;

    public double PSV
    {
        get { return psv; }
        set { psv = value; }
    }

    private List<string> usespaceinfo = new List<string>();
    int count = 0;

    public List<string> UseOfSpaceReport()
    {
        return usespaceinfo;
    }

    public void UseOfSpace(Figure[] f)//공간사용
    {
        double xmin, ymin;
        Figure minV1 = null, minV2 = null, minH1 = null, minH2 = null;

        xmin = float.MaxValue;
        ymin = float.MaxValue;

        for (int i = 0; i < f.Length - 1; i++)
        {
            for (int j = i + 1; j < f.Length; j++)
            {
                if (i == j)
                {
                    continue;
                }
                else
                {
                    if (xmin > Math.Abs(f[i].BoundingRect.X - f[j].BoundingRect.X))
                    {
                        xmin = Math.Abs(f[i].BoundingRect.X - f[j].BoundingRect.X);
                        minH1 = f[i]; minH2 = f[j];
                    }
                    if (ymin > Math.Abs(f[i].BoundingRect.Y - f[j].BoundingRect.Y))
                    {
                        ymin = Math.Abs(f[i].BoundingRect.Y - f[j].BoundingRect.Y);
                        minV1 = f[i]; minV2 = f[j];
                    }
                }
            }
            UseOfSpace(minH1, minH2, minV1, minV2, xmin, ymin);
        }

        if (count >= 2)
            psv = 10;
        else
            psv = 1;
    }

    private void UseOfSpace(Figure mh1, Figure mh2, Figure mv1, Figure mv2, double xm, double ym)
    {
        if (xm > ym)
        {
            //X값의 차이가 Y값의 차이보다 클 때 => 수평방향
            if (mh1.BoundingRect.X < mh2.BoundingRect.X)//minH1이 왼쪽에 있을 때
            {
                double Ssize = Math.Abs((mh1.BoundingRect.X + mh1.BoundingRect.Width) - mh2.BoundingRect.X);
                if (mh1.BoundingRect.Width / 2 <= Ssize || mh1.BoundingRect.Width / 4 >= Ssize)//BoundingRect.Width : 도형을 감싸는 사각형의 너비
                {
                    count++;
                    usespaceinfo.Add(mh1.Name + "&" + mh2.Name);
                }
            }
            else
            {
                double Ssize = Math.Abs((mh2.BoundingRect.X + mh2.BoundingRect.Width) - mh1.BoundingRect.X);
                if (mh2.BoundingRect.Width / 2 <= Ssize && mh2.BoundingRect.Width / 4 >= Ssize)
                {
                    count++;
                    usespaceinfo.Add(mh1.Name + "&" + mh2.Name);
                }
            }
        }
        else
        {
            //수직방향
            if (mv1.BoundingRect.Y < mv2.BoundingRect.Y) //mv1이 위쪽에 있을 때
            {
                double Sszie = Math.Abs((mv1.BoundingRect.Y + mv1.BoundingRect.Height) - mv2.BoundingRect.Y);
                if (mv1.BoundingRect.Height / 2 <= Sszie || mv1.BoundingRect.Height / 4 >= Sszie)//BoundingRect.Height : 도형을 감싸는 사각형의 높이
                {
                    count++;
                    usespaceinfo.Add(mv1.Name + "&" + mv2.Name);
                }
            }
            else //mv2가 위쪽에 있을 때
            {
                double Sszie = Math.Abs((mv2.BoundingRect.Y + mv2.BoundingRect.Height) - mv1.BoundingRect.Y);
                if (mv2.BoundingRect.Height / 2 <= Sszie || mv2.BoundingRect.Height / 4 >= Sszie)
                {
                    count++;
                    usespaceinfo.Add(mv1.Name + "&" + mv2.Name);
                }
            }
        }
    }
}
