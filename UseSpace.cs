using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        double mindistance;
        double[] compare = new double[8];
        Figure f1 = new Figure();
        Figure f2 = new Figure(); ;

        for (int i = 0; i < f.Length - 1; i++)
        {
            mindistance = float.MaxValue;
            for (int j = i + 1; j < f.Length; j++)
            {
                //i번째 도형의 네개의 꼭지점 좌표와 j번째 도형의 위의 양쪽 좌표의 거리를 구한다.
                compare[0] = Math.Sqrt(Math.Pow(f[i].BoundingRect.X - f[j].BoundingRect.X, 2)+ Math.Pow(f[i].BoundingRect.Y - f[j].BoundingRect.Y, 2)); 
                compare[1] = Math.Sqrt(Math.Pow(((f[i].BoundingRect.X + f[i].BoundingRect.Width) - (f[j].BoundingRect.X + f[j].BoundingRect.Width)), 2) + Math.Pow(f[i].BoundingRect.Y - f[j].BoundingRect.Y, 2));
                compare[2] = Math.Sqrt(Math.Pow(f[i].BoundingRect.X - f[j].BoundingRect.X, 2) + Math.Pow((f[i].BoundingRect.Y  + f[i].BoundingRect.Height) - f[j].BoundingRect.Y, 2)); 
                compare[3] = Math.Sqrt(Math.Pow(((f[i].BoundingRect.X + f[i].BoundingRect.Width) - (f[j].BoundingRect.X + f[j].BoundingRect.Width)), 2) + Math.Pow((f[i].BoundingRect.Y + f[i].BoundingRect.Height) - f[j].BoundingRect.Y, 2));
                compare[4] = Math.Sqrt(Math.Pow(f[i].BoundingRect.X - (f[j].BoundingRect.X + f[j].BoundingRect.Width), 2) + Math.Pow(f[i].BoundingRect.Y - f[j].BoundingRect.Y, 2));
                compare[5] = Math.Sqrt(Math.Pow((f[i].BoundingRect.X - (f[j].BoundingRect.X + f[j].BoundingRect.Width)), 2) + Math.Pow((f[i].BoundingRect.Y + f[i].BoundingRect.Height) - f[j].BoundingRect.Y, 2));
                compare[6] = Math.Sqrt(Math.Pow((f[i].BoundingRect.X + f[i].BoundingRect.Width) - f[j].BoundingRect.X, 2) + Math.Pow(f[i].BoundingRect.Y - f[j].BoundingRect.Y, 2)); 
                compare[7] = Math.Sqrt(Math.Pow(((f[i].BoundingRect.X + f[i].BoundingRect.Width) - f[j].BoundingRect.X), 2) + Math.Pow((f[i].BoundingRect.Y + f[i].BoundingRect.Height) - f[j].BoundingRect.Y, 2));

                if (mindistance > compare[0] || mindistance > compare[1] || mindistance > compare[2] || mindistance > compare[3] ||
                    mindistance > compare[4] || mindistance > compare[5] || mindistance > compare[6] || mindistance > compare[7]) //8개의 거리 중 하나라도 mindistance보다 작으면
                {
                    Array.Sort(compare);

                    mindistance = compare[0];   //그 중 최소값을 mindistance에 대입

                    f1 = f[i]; f2 = f[j];       //해당하는 i, j를 대입
                    Debug.WriteLine("i : " + i + ", j : " + j + ", distance : " + compare[0]);
                }
            }
            UseOfSpace(f1, f2);
        }
        
       if (count >= 2)
           psv = 10;
        else
            psv = 1;
    }

    private void UseOfSpace(Figure f1, Figure f2)
    {
        double margin;
        
        if ((f1.BoundingRect.Y <= f2.BoundingRect.Y && (f1.BoundingRect.Y + f1.BoundingRect.Height) >= f2.BoundingRect.Y) ||
            (f2.BoundingRect.Y <= f1.BoundingRect.Y && (f2.BoundingRect.Y + f2.BoundingRect.Height) >= f1.BoundingRect.Y))
        {
            //감싸는 사각형의 Y좌표 범위가 겹칠 때
            //수평하다고 판단
            if (f1.BoundingRect.X < f2.BoundingRect.X)//f1이 왼쪽에 있을 때
            {
                margin = Math.Abs((f1.BoundingRect.X + f1.BoundingRect.Width) - f2.BoundingRect.X);
                if (f1.BoundingRect.Width / 2 <= margin || f1.BoundingRect.Width / 4 >= margin)//BoundingRect.Width : 도형을 감싸는 사각형의 너비
                {
                    count++;
                    usespaceinfo.Add("수평방향 : " + f1.Name + " & " + f2.Name);
                    Debug.WriteLine("f1.BoundingRect.Width / 2 : " + f1.BoundingRect.Width / 2 + "f1.BoundingRect.Width / 4 : " + f1.BoundingRect.Width / 4);
                }
            }
            else
            {
                margin = Math.Abs((f2.BoundingRect.X + f2.BoundingRect.Width) - f1.BoundingRect.X);
                if (f2.BoundingRect.Width / 2 <= margin && f2.BoundingRect.Width / 4 >= margin)
                {
                    count++;
                    usespaceinfo.Add("수평방향 : " + f1.Name + " & " + f2.Name);
                    Debug.WriteLine("f2.BoundingRect.Width / 2 : " + f2.BoundingRect.Width / 2 + "f2.BoundingRect.Width / 4 : " + f2.BoundingRect.Width / 4);
                }
            }
        }
        else if((f1.BoundingRect.X <= f2.BoundingRect.X && (f1.BoundingRect.X + f1.BoundingRect.Width) >= f2.BoundingRect.X) || 
            (f2.BoundingRect.X <= f1.BoundingRect.X && (f2.BoundingRect.X + f2.BoundingRect.Width) >= f1.BoundingRect.X))
        {
            //감싸는 사각형의 X좌표 범위가 겹칠 때
            //수직하다고 판단

            if (f1.BoundingRect.Y < f2.BoundingRect.Y) //f1이 위쪽에 있을 때
            {
                margin = Math.Abs((f1.BoundingRect.Y + f1.BoundingRect.Height) - f2.BoundingRect.Y);
                if (f1.BoundingRect.Height / 2 <= margin || f1.BoundingRect.Height / 4 >= margin)//BoundingRect.Height : 도형을 감싸는 사각형의 높이
                {
                    count++;
                    usespaceinfo.Add("수직방향 : " + f1.Name + " & " + f2.Name);
                    Debug.WriteLine("f1.BoundingRect.Height / 2 : " + f1.BoundingRect.Height / 2 + "f1.BoundingRect.Height / 4 : " + f1.BoundingRect.Height / 4);
                }
            }
            else //f2가 위쪽에 있을 때
            {
                margin = Math.Abs((f2.BoundingRect.Y + f2.BoundingRect.Height) - f1.BoundingRect.Y);
                if (f2.BoundingRect.Height / 2 <= margin || f2.BoundingRect.Height / 4 >= margin)
                {
                    count++;
                    usespaceinfo.Add("수직방향 : " + f1.Name + " & " + f2.Name);
                    Debug.WriteLine("f2.BoundingRect.Height / 2 : " + f2.BoundingRect.Height / 2 + "f2.BoundingRect.Height / 4 : " + f2.BoundingRect.Height / 4);
                }
            }
        }
    }
    
}
