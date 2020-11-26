using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using System.Data.SQLite;
using System.Data;
using System.Diagnostics;

class Simplification
{
    private List<string> simplificationinfo = new List<string>();
    private double psv = 1.0;

    public double PSV
    {
        get { return psv; }
        set { psv = value; }
    }

    /*
     * case1 : 도형 1,2,3,5의 구성요소의 수가 최소 3개 이상 감소된 경우
     * DB에서 도형 1,2,3,5 각각의 point수 값의 평균을 구함 => 표준편차 사용하여 오차범위 설정 => m - 3x?
     */

    public void simplification(Figure[] f)
    {
        //simplification_1(f);
        simplification_2(f[6]);

        SimplificationScore(f);
    }

    private void simplification_1(Figure[] f)
    {
        int fig1 = 0, fig2 = 0, fig3 = 0, fig5 = 0;
        

        if (f[1].Points != null) //도형 1 점 10이하면 단순화
        {
            
            fig1 = Count(f[1].Points);
            if (fig1 <= 10)
            {
                Debug.WriteLine("1 Count"+fig1);
                f[1].is_simplification = true;
            }
        }

        if (f[2].Points != null) //도형 2 점 31이하면 단순화
        {
            fig2 = Count(f[2].Points);
            if (fig2 <= 31)
            {
                Debug.WriteLine("2 Count" + fig2);
                f[2].is_simplification = true;
            }
        }

        if (f[3].Points != null) //도형 3 점 14이하면 단순화
        {
            fig3 = Count(f[3].Points);
            if (fig3 <= 14)
            {
                Debug.WriteLine("3 Count" + fig3);
                f[3].is_simplification = true;
            }
        }
        if (f[5].Points != null)
        {
            fig5 = Count(f[5].Points);
            if (fig5 <= 24)
            {
                Debug.WriteLine("5 Count" + fig5);
                f[5].is_simplification = true;
            }
        }
        /*
         * 도형1 : 12개의 점
         * 도형2 : 11개의 원이 3열로 구성 => 33개
         * 도형3 : 16개의 점
         * 도형5 : 19개의 점으로 된 곡선 + 7개의 점으로 된 선 => 26개
         */
    }

    private int Count(List<Point> list)
    {
        
        int count = 0;
        Point arraypt = new Point(0, 0);
        foreach (var pt in list)
        {
            if (count == 0)
            {
                arraypt.X = Math.Truncate(pt.X);
                arraypt.Y = Math.Truncate(pt.Y);
                count = count + 1;

                continue;
            }
            if ((arraypt.X - Math.Truncate(pt.X) > 3 || Math.Truncate(pt.X) - arraypt.X > 3) || (arraypt.Y - Math.Truncate(pt.Y) > 3 || Math.Truncate(pt.Y) - arraypt.Y > 3))
            {
                count = count + 1;
            }

            arraypt.X = (int)(pt.X);
            arraypt.Y = (int)(pt.Y);
        }
        //Debug.WriteLine("count");
        return count;
    }

    //도형 6
    private void simplification_2(Figure f6)
    {
        /*
        Point top, left, bottom, right;
        List<Point> pts = new List<Point>();
        //bool condition1, condition2;

        foreach (var stroke in f6.Strokes)
        {
            
            if (stroke.Selected != true)
                continue;
            var points = stroke.GetInkPoints();

            foreach (var pt in points)
            {
                pts.Add(pt.Position);

                if (f6.BoundingRect.X == pt.Position.X)
                    left = pt.Position;
                if (f6.BoundingRect.Y == pt.Position.Y)
                    top = pt.Position;
                if (f6.BoundingRect.X + f6.BoundingRect.Width == pt.Position.X)
                    right = pt.Position;
                if (f6.BoundingRect.Y + f6.BoundingRect.Height == pt.Position.Y)
                    bottom = pt.Position;
            }
        }

        //세로방향
        var m = (top.Y - bottom.Y) / (top.X - bottom.X);
        int count = 0;
        for (int i = (int)f6.BoundingRect.X; i < f6.BoundingRect.X + f6.BoundingRect.Width; i++)
        {
            for (int j = 0; j < pts.ToArray().Length; j++)
            {
                if (pts[j].Y - top.Y == m * (pts[j].X - top.X))
                {
                    count++;
                    if (count >= 8)
                        goto pass;
                }
            }
        }
        f6.is_simplification = true;
        goto end;

    pass:;

        //가로방향
        m = (right.Y - left.Y) / (right.X - left.X);
        count = 0;
        for (int i = (int)f6.BoundingRect.Y; i < f6.BoundingRect.Y + f6.BoundingRect.Height; i++)
        {
            for (int j = 0; j < pts.ToArray().Length; j++)
            {
                if (pts[j].Y - top.Y == m * (pts[j].X - top.X))
                {
                    count++;
                    if (count >= 8)
                        goto end;
                }
            }
        }
        f6.is_simplification = true;

    end:;*/
        /*
            double maxX = f6.BoundingRect.Right, minX = f6.BoundingRect.Left;
            double maxY = f6.BoundingRect.Bottom, minY = f6.BoundingRect.Top;
            int count = 0;
            int startindex = 0;

            Debug.WriteLine("maxX : " + maxX + ", minX : " + minX + ", maxY : " + maxY + ", minY : " + minY);

            foreach (Point pt in f6.Points)
            {
                if (pt.Y >= minY-5 || pt.Y <= maxY-5)//시작점이 위 또는 아래 => 세로부터
                {
                    Debug.WriteLine("condition1");
                    count = condition1(f6, maxX, minX, maxY, minY, startindex);
                    break;
                }
                else if (pt.X >= minX-5 || pt.X <= maxX-5)//시작점이 왼 또는 오 => 가로부터
                {
                    Debug.WriteLine("condition2");
                    count = condition2(f6, maxX, minX, maxY, minY, startindex);
                    break;
                }
                startindex++;
            }

            Debug.WriteLine("f6 simple count : " + count);

            if (count < 14)
                f6.is_simplification = true;
              */

        Point arraypt;
        List<Point> hit1 = new List<Point>();
        List<Point> hit2 = new List<Point>();
        bool hit_bool = false;//list 변경 변수
        int count1 = 0, count2=0;


        foreach (var pt in f6.Points)
        {
            if (hit1 == null)
            {
                arraypt.X = Math.Truncate(pt.X);
                arraypt.Y = Math.Truncate(pt.Y);
                hit1.Add(arraypt);

                continue;
            }
            if (arraypt.X - Math.Truncate(pt.X) > 10 || Math.Truncate(pt.X) - arraypt.X > 10)
            {
                hit_bool = true;
            }

            arraypt.X = (int)(pt.X);
            arraypt.Y = (int)(pt.Y);

            if (hit_bool == false)
            {
                hit1.Add(arraypt);
            }
            else
            {
                hit2.Add(arraypt);
            }
        }

        if(hit1 == null)
        {
            Debug.WriteLine("hit 1 = Null");
        }
        else Debug.WriteLine("hit 1 = Not Null");
        if (hit2 == null)
        {
            Debug.WriteLine("hit 2 = Null");
        }
        else Debug.WriteLine("hit 2 = Not Null");

        count1 = circle(hit1,1);
        count2 = circle(hit2, 2);
        if (count1 <=3 || count2 <= 3)
        {
            f6.is_simplification = true;
        }
    }

    private int circle(List<Point> list, int i)
    {
        Point arraypt;
        
        bool flag ; // false는 up, true는 down
        double x_max = 0.0, x_min = 1000.0, y_max = 0.0, y_min = 1000.0;
        
        int count = 0;

        Boolean arrayflag = false;

        if (i == 1)
        {
            if (list[0].Y < list[3].Y)
            {
                flag = false;

                foreach (var pt in list)
                {
                    if(arrayflag == false)
                    {
                        arraypt.X = pt.X;
                        arraypt.Y = pt.Y;
                        arrayflag = true;
                    }
                    
                    if (arraypt.Y < pt.Y)
                    {
                        if (flag == true && y_min != 1000.0)
                        {
                            if (y_max - y_min > 3) { count = count + 1; }
                            flag = false;
                            y_max = 0.0;
                        }
                        y_max = pt.Y;
                    }
                    if (arraypt.Y > pt.Y)
                    {
                        if (flag == false && y_max != 0.0)
                        {
                            flag = true;
                            y_min = 1000.0;
                        }
                        y_min = pt.Y;

                    }

                    arraypt.X = pt.X;
                    arraypt.Y = pt.Y;
                }
            }
            else
            {
                flag = true;

                foreach (var pt in list)
                {
                    if (arrayflag == false)
                    {
                        arraypt.X = pt.X;
                        arraypt.Y = pt.Y;
                        arrayflag = true;
                    }

                    if (arraypt.Y < pt.Y)
                    {
                        if (flag == true && y_min != 1000.0)
                        {
                            flag = false;
                            y_max = 0.0;
                        }
                        y_max = pt.Y;
                    }
                    if (arraypt.Y > pt.Y)
                    {
                        if (flag == false && y_max != 0.0)
                        {
                            if (y_max - y_min > 3) { count = count + 1; }
                            flag = true;
                            y_min = 1000.0;
                        }
                        y_min = pt.Y;

                    }
                    arraypt.X = pt.X;
                    arraypt.Y = pt.Y;
                }
            }
        }

        if (i == 2)
        {
            if (list[0].X < list[3].X)
            {
                flag = false;

                foreach (var pt in list)
                {
                    if (arrayflag == false)
                    {
                        arraypt.X = pt.X;
                        arraypt.Y = pt.Y;
                        arrayflag = true;
                    }

                    if (arraypt.X < pt.X)
                    {
                        if (flag == true && x_min != 1000.0)
                        {
                            if (x_max - x_min > 3) { count = count + 1; }
                            flag = false;
                            x_max = 0.0;
                        }
                        x_max = pt.X;
                    }
                    if (arraypt.X > pt.X)
                    {
                        if (flag == false && x_max != 0.0)
                        {
                            flag = true;
                            x_min = 1000.0;
                        }
                        x_min = pt.X;

                    }

                    arraypt.X = pt.X;
                    arraypt.Y = pt.Y;
                }
            }
            else
            {
                flag = true;

                foreach (var pt in list)
                {
                    if (arrayflag == false)
                    {
                        arraypt.X = pt.X;
                        arraypt.Y = pt.Y;
                        arrayflag = true;
                    }
                    if (arraypt.X < pt.X)
                    {
                        if (flag == true && x_min != 1000.0)
                        {
                            flag = false;
                            x_max = 0.0;
                        }
                        x_max = pt.X;
                    }
                    if (arraypt.X > pt.X)
                    {
                        if (flag == false && x_max != 0.0)
                        {
                            if (x_max - x_min > 3) { count = count + 1; }
                            flag = true;
                            x_min = 1000.0;
                        }
                        x_min = pt.X;

                    }
                    arraypt.X = pt.X;
                    arraypt.Y = pt.Y;
                }
            }
        }

        return count;
    }

    private int condition1(Figure f6, double maxx, double minx, double maxy, double miny, int index)//수직방향
    {
        int countV = 0, countH = 0;

        for (int i = index; i < f6.Points.ToArray().Length - 2; i++)
        {
            if (f6.Points[i].X <= f6.Points[i+1].X && f6.Points[i + 1].X >= f6.Points[i + 2].X)
            {
                countV++;
            }
            else if (f6.Points[i].X >= f6.Points[i + 1].X && f6.Points[i + 1].X <= f6.Points[i + 2].X)
            {
                countV++;
            }
            Debug.WriteLine("condition1 countV : " + countV);

            if (f6.Points[i].X >= minx - 5 || f6.Points[i].X <= maxx - 5)
            {
                Debug.WriteLine("condition2 in condition1");
                countH = condition2(f6, maxx, minx, maxy, miny, i);
                goto outside;
            }
        }
    outside:;
        Debug.WriteLine("condition1 countV : " + countV + ", countH : " + countH);


        return countV + countH;
    }

    private int condition2(Figure f6, double maxx, double minx, double maxy, double miny, int index)//수평방향
    {
        int countV = 0, countH = 0;

        for (int i = index; i < f6.Points.ToArray().Length - 2; i++)
        {
            if (f6.Points[i].Y <= f6.Points[i + 5].Y && f6.Points[i + 1].Y >= f6.Points[i + 2].Y)
            {
                countH++;
            }
            else if (f6.Points[i].Y >= f6.Points[i + 1].Y && f6.Points[i + 1].Y <= f6.Points[i + 2].Y)
            {
                countH++;
            }

            if (f6.Points[i].Y >= miny-5 || f6.Points[i].Y <= maxy - 5)
            {
                countV = condition1(f6, maxx, minx, maxy, miny, i);
                goto outside;
            }
        }
    outside:;
        Debug.WriteLine("condition2 countV : " + countV + ", countH : " + countH);

        return countV + countH;
    }

    private void simplification_3(Figure f7, Figure f8)
    {
        Point LT = new Point(f7.BoundingRect.Left, f7.BoundingRect.Top);
        Point LB = new Point(LT.X, LT.Y + f7.BoundingRect.Height);
        double gap = LB.Y - LT.Y;
        int count = 0;

        for (double i = LT.Y; i < LB.Y; i = i + (gap / 127))
        {
            count = 0;
            foreach (Point pt in f7.Points)
            {
                if (f7.Points.Exists(p => p.X == i))
                {
                    count++;
                }
            }

            if (count == 4)
                goto f7NotSimple;
        }
        f7.is_simplification = true;

    f7NotSimple:;

        LT = new Point(f8.BoundingRect.Left, f8.BoundingRect.Top);
        Point RT = new Point(f8.BoundingRect.Right, f8.BoundingRect.Top);
        gap = RT.X - LT.X;
        count = 0;

        for (double i = LT.X; i < RT.X; i = i + (gap / 128))
        {
            count = 0;
            foreach (Point pt in f8.Points)
            {
                if (f8.Points.Exists(p => p.X == i))
                {
                    count++;
                }
            }

            if (count == 4)
                goto f8NotSimple;
        }
        f8.is_simplification = true;

    f8NotSimple:;
    }

    private void SimplificationScore(Figure[] f)
    {
        //simplification(f);

        int count = 0;
        for (int i = 0; i < 9; i++)
        {
            if (f[i].is_simplification == true)
            {
                count++;
                Debug.WriteLine("단순화 " + i + "번째");
                //simplificationinfo.Add("도형" + i + " 단순화");
            }
        }

        if (count >= 3)     //3개 이상의 도형에서 일어날 때
            psv = 10;
        else if (count == 2)//2개 도형에서 일어날 때
            psv = 7;
        else if (count == 1)//1개 도형에서 일어날 때
            psv = 4;
        else                //없다
            psv = 1;
    }

    public List<string> SimplificationReport()
    {
        return simplificationinfo;
    }
}