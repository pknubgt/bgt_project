using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;


class Reiteration
{
    private double psv=1.0;

    public double PSV
    {
        get { return psv; }
        set { psv = value; }
    }
    public void reiteration(Figure[] figure) //중첩
    {
        int dir_RR = direct_RR(figure);
        Debug.WriteLine("극단적 중첩 횟수: " + dir_RR);

        int indir_RR = 0;
        if (dir_RR == 0) { indir_RR = indirect_RR(figure); }
        Debug.WriteLine("극단적 중첩 경향 횟수: " + indir_RR);

        if (dir_RR >= 3) { psv = 10.0; }
        else if (dir_RR == 2) { psv = 8.5; }
        else if (dir_RR == 1) { psv = 7.0; }
        else if (indir_RR >= 3) { psv = 5.5; }
        else if (indir_RR == 2) { psv = 4.0; }
        else if (indir_RR == 1) { psv = 2.5; }

        /*
        int i = 0, j = 0;
        var strokesA = canvasA.InkPresenter.StrokeContainer.GetStrokes();
        foreach(var stroke in strokesA)
        {
            var points = stroke.GetInkPoints();
            foreach(var pt in points)
            {
                if (i <= 2)
                {
                    Debug.WriteLine(pt.Position.X);
                    i = i + 1;
                }
            }
        }

        var strokes1 = canvas1.InkPresenter.StrokeContainer.GetStrokes();
        foreach (var stroke in strokes1)
        {
            var points = stroke.GetInkPoints();
            foreach (var pt in points)
            {
                if (j <= 2)
                {
                    Debug.WriteLine(pt.Position.X);
                    j = j + 1;
                }
            }
        }*/
    }

    /*public List<Point> RR_figure(List<Point> strokes) //각 figure의 stroke에서 중복되는 좌표 정리
    {
        Point arraypt = new Point((int)(strokes[0].X), (int)(strokes[0].Y));
        List<Point> RR_list = new List<Point>();
        RR_list.Add(arraypt);
        //Debug.WriteLine("RR_figure");

        foreach (var pt in strokes)
        {
            //Debug.WriteLine("X: " + (int)(pt.Position.X) + "Y: " + (int)(pt.Position.Y));
            if ((arraypt.X != (int)pt.X) || (arraypt.Y != (int)pt.Y))
            {
                arraypt.X = (int)(pt.X);
                arraypt.Y = (int)(pt.Y);
                RR_list.Add(arraypt);
            }
        }
        return RR_list;

    }*/


    public bool RR_compare(List<Point> a, List<Point> b) // 도형이 중복되었는지 확인하는 함수
    {

        Boolean res = false;
        foreach (var a_element in a)
        {
            foreach (var b_element in b)
            {
                if ((a_element.X + 3 > b_element.X && a_element.X - 3 < b_element.X) && (a_element.Y + 3 > b_element.Y && a_element.Y - 3 < b_element.Y))
                {

                    res = true;
                    Debug.WriteLine("A element= (x)" + a_element.X + "(y)" + a_element.Y);
                    Debug.WriteLine("B element= (x)" + b_element.X + "(y)" + b_element.Y);
                }
            }
        }
        return res;
    }

    public int direct_RR(Figure[] figure) // 극단적 중첩 확인
    {
        int dir_RR = 0;

        for (int i = 0; i < 9; i++)
        {
            if (figure[i].Points == null)//figure 정보란이 비어있으면
                continue;

            for (int j = i + 1; j < 9; j++)
            {
                if (figure[j].Points == null)
                    continue;

                if (RR_compare(figure[i].Points, figure[j].Points) == true) // 두 도형 중복하는지 확인
                {
                    Debug.WriteLine("i= " + i + "j= " + j);
                    dir_RR = dir_RR + 1;//중복횟수  
                }
            }
        }

        return dir_RR;
    }

    

    public int indirect_RR(Figure[] figure) //극단적 중첩경향
    {
        int indir_RR = 0;

        int ar1_xmax, ar1_xmin, ar1_ymax, ar1_ymin;
        int ar2_xmax, ar2_xmin, ar2_ymax, ar2_ymin;

        for (int i = 0; i < 9; i++)
        {
            if (figure[i].Points == null)// figure 정보란이 비어있으면
                continue;

            ar1_xmin = (int)figure[i].BoundingRect.X;
            ar1_ymin = (int)figure[i].BoundingRect.Y;
            ar1_xmax = (int)figure[i].BoundingRect.X + (int)figure[i].BoundingRect.Width;
            ar1_ymax = (int)figure[i].BoundingRect.Y + (int)figure[i].BoundingRect.Height;


            for (int j = i + 1; j < 9; j++)
            {
                if (figure[j].Points == null)
                    continue;

                ar2_xmin = (int)figure[j].BoundingRect.X;
                ar2_ymin = (int)figure[j].BoundingRect.Y;
                ar2_xmax = (int)figure[j].BoundingRect.X + (int)figure[j].BoundingRect.Width;
                ar2_ymax = (int)figure[j].BoundingRect.Y + (int)figure[j].BoundingRect.Height;


                if ((ar1_xmin < ar2_xmin && ar2_xmin < ar1_xmax) || (ar1_xmin < ar2_xmax && ar2_xmax < ar1_xmax)) // 두 도형의 최대 최소 값을 이용해서 겹치는지 확인
                {
                    if ((ar1_ymin < ar2_ymin && ar2_ymin < ar1_ymax) || (ar1_ymin < ar2_ymax && ar2_ymax < ar1_ymax))
                    {
                        Debug.WriteLine("i= " + i + "j= " + j);
                        indir_RR = indir_RR + 1;//중복횟수  
                    }

                }
            }
        }
        return indir_RR;
    }
}


