using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;


class Reiteration
{
    private double psv = 1.0;
    private List<string> reiterationinfo = new List<string>();

    public double PSV
    {
        get { return psv; }
        set { psv = value; }
    }
    public void reiteration(Figure[] figure) //중첩
    {
        int dir_RR = direct_RR(figure); //극단적 중첩 횟수를 판단
        reiterationinfo.Add("중첩 횟수: " + dir_RR);
        Debug.WriteLine("중첩 횟수: " + dir_RR);

        int indir_RR = 0;               //극단적 중첩이 아니라면 극단적 중첩 경향 횟수를 판단
        if (dir_RR == 0) { indir_RR = indirect_RR(figure); }
        reiterationinfo.Add("중첩 경향 횟수: " + indir_RR);
        Debug.WriteLine("중첩 경향 횟수: " + indir_RR);

        if (dir_RR >= 3) { psv = 10.0; }
        else if (dir_RR == 2) { psv = 8.5; }
        else if (dir_RR == 1) { psv = 7.0; }
        else if (indir_RR >= 3) { psv = 5.5; }
        else if (indir_RR == 2) { psv = 4.0; }
        else if (indir_RR == 1) { psv = 2.5; }

    }

   
    public bool RR_compare(List<Point> a, List<Point> b) // 도형이 중복되었는지 확인하는 함수
    {

        Boolean res = false;
        foreach (var a_element in a)
        {
            foreach (var b_element in b)
            {
                if ((a_element.X + 3 > b_element.X && a_element.X - 3 < b_element.X) && (a_element.Y + 3 > b_element.Y && a_element.Y - 3 < b_element.Y))
                {//좌표를 양 끝으로 6정도 오차를 두고, 서로 좌표가 겹치면 도형이 중복

                    res = true;
                    //Debug.WriteLine("A element= (x)" + a_element.X + "(y)" + a_element.Y);
                    //Debug.WriteLine("B element= (x)" + b_element.X + "(y)" + b_element.Y);
                }
            }
        }
        return res;
    }

    public int direct_RR(Figure[] figure) // 중첩 확인
    {
        int dir_RR = 0;

        for (int i = 0; i < 9; i++)
        {
            if (figure[i].Points == null)//figure 정보란이 비어있으면 다음 도형
                continue;

            for (int j = i + 1; j < 9; j++)
            {
                if (figure[j].Points == null)
                    continue;

                if (RR_compare(figure[i].Points, figure[j].Points) == true) // 두 도형 중복하는지 확인 (함수 호출)
                {
                    Debug.WriteLine("i= " + i + "j= " + j);
                    dir_RR = dir_RR + 1;//중복횟수  
                }
            }
        }

        return dir_RR;
    }



    public int indirect_RR(Figure[] figure) //중첩 경향
    {
        int indir_RR = 0;
        int xmax, xmin, ymax, ymin;
        
        for (int i = 0; i < 9; i++)
        {
            if (figure[i].Points == null)// figure 정보란이 비어있으면 다음 도형
                continue;

            xmin = (int)figure[i].BoundingRect.X;
            ymin = (int)figure[i].BoundingRect.Y;
            xmax = (int)figure[i].BoundingRect.X + (int)figure[i].BoundingRect.Width;
            ymax = (int)figure[i].BoundingRect.Y + (int)figure[i].BoundingRect.Height;
            
            for (int j = i + 1; j < 9; j++)
            {
                if (figure[j].Points == null)
                    continue;
                
                foreach(var pt in figure[j].Points)
                {
                    if((xmin < pt.X && pt.X < xmax) && (ymin < pt.Y && pt.Y < ymax))
                    {
                        Debug.WriteLine("i= " + i + "j= " + j);
                        indir_RR = indir_RR + 1;//중복횟수
                        continue;
                    }
                }
            }
        }
        return indir_RR;
    }

    public List<string> ReiterationReport()
    {
        return reiterationinfo;
    }
}

