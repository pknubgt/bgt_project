using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;


class OverlappingDifficulty
{
    /*
     * 중복곤란 도형7의 두 부분을 겹쳐 그리지 못하고,
     * 도형A와 4에서 내부 도형끼리 접촉하지 못하는 현상
     */
    private List<string> overlappinginfo = new List<string>();
    private double psv = 1.0;

    public double PSV
    {
        get { return psv; }
        set { psv = value; }
    }

    public void Overlapping_difficulty(Figure[] figure)
    {
        Boolean result_figureA = false; // default_normal
        Boolean result_figure4 = false; // default_normal
        Boolean result_figure7 = false; // default_normal

        int count = 0;

        if (figure[0].Points != null)
        {
            result_figureA = OD_figure(figure[0].Points);
            Debug.WriteLine("figure A result = " + result_figureA);
            overlappinginfo.Add("figure A result = " + result_figureA);
        }

        if (figure[4].Points != null)
        {

            result_figure4 = OD_figure(figure[4].Points);
            Debug.WriteLine("figure 4 result = " + result_figure4);
            overlappinginfo.Add("figure 4 result = " + result_figure4);

        }

        if (figure[7].Points != null)
        {
            result_figure7 = check_crossLine(figure[7].Points);
            Debug.WriteLine("figure 7 result = " + result_figure7);
            overlappinginfo.Add("figure 7 result = " + result_figure7);

        }

        if (result_figureA == true)
        {
            count = count + 1;
        }
        if (result_figure4 == true)
        {
            count = count + 1;
        }
        if (result_figure7 == true)
        {
            count = count + 1;
        }

        if (count == 1) { psv = 5.5; }
        else if (count >= 2) { psv = 10.0; }

    }



    public bool check_crossLine(List<Point> hit) // y축과 평행한 직선에 겹치는 도형의 좌표 갯수 측정
    {

        List<Point> hit1 = hit.OrderBy(p => p.X).ThenBy(p => p.Y).ToList();//X크기순으로 정렬, 중복되면 Y크기순으로 정렬
        List<Point> hit2 = hit.OrderBy(p => p.Y).ThenBy(p => p.X).ToList();//Y크기순으로 정렬, 중복되면 X크기순으로 정렬

        Boolean result = false;
        int firstX = (int)hit1[0].X; //시작 숫자
        int firstY = (int)hit1[0].Y;
        int min_x = 10, min_y = 10;// 겹친 횟수 최대 최소
        int max_x = 0, max_y = 0;
        int checknum = 1; //체크하기 위한 임시 변수

        List<int> total_x = new List<int>();
        List<int> total_y = new List<int>();


        foreach (var hit_element in hit1) //x축 수직(y축 평행 교차)
        {
            if (firstX == hit_element.X) //x값이 같을 때 갯수 구함
            {
                if (firstY + 10 > hit_element.Y || firstY - 10 < hit_element.Y) // 오차 10 => y값도 같다면
                {
                    firstY = (int)hit_element.Y; //다음 비교할 y 값 집어넣음
                    checknum = checknum + 1; // 갯수 세기
                    //Debug.WriteLine("x=> " + firstX + "y=> " + firstY);
                }

            }
            else //하나의 x값의 비교가 끝나면
            {
                if (hit_element.X - firstX > 4) //x값 좌표 사이의 거리가 4 이상일 때
                {
                    min_x = 0;//최소값을 0
                }
                firstX = (int)hit_element.X;//다음 비교할 x값을 집어넣음
                firstY = (int)hit_element.Y;
                total_x.Add(checknum);
                checknum = 1;
            }
        }

        firstX = (int)hit2[0].X; //시작 숫자
        firstY = (int)hit2[0].Y;
        checknum = 1;

        foreach (var hit_element in hit2) //y축 수직(x축 평행 교차)
        {
            if (firstY == hit_element.Y)
            {
                if (firstX + 10 > hit_element.X || firstX - 10 < hit_element.X)
                {
                    firstX = (int)hit_element.X;
                    checknum = checknum + 1;
                    //Debug.WriteLine("x=> " + firstX + "y=> " + firstY);
                }

            }
            else
            {
                if (hit_element.Y - firstY > 4)
                {
                    min_y = 0;
                }
                firstX = (int)hit_element.X;
                firstY = (int)hit_element.Y;
                total_y.Add(checknum);
                checknum = 1;
            }
        }

        foreach (var total in total_x)
        {
            if (max_x < total) max_x = total; //각 x값에 대한 y값의 개수 중 최대 구함
            if (min_x > total) min_x = total; // 최소 구함
            //Debug.WriteLine("total=> x: " + total);
        }
        //Debug.WriteLine("max = " + max_x + "min= " + min_x);

        foreach (var total in total_y)
        {
            if (max_y < total) max_y = total;
            if (min_y > total) min_y = total;
            //Debug.WriteLine("total=> y: " + total);
        }
        //Debug.WriteLine("max = " + max_y + "min= " + min_y);

        if (min_x != 0 && max_x >= 4)
        {
            if (min_y != 0 && min_y >= 4)
            {
                result = true;
            }
        }

        return result;
    }


    public bool OD_figure(List<Point> points)
    {
        Point arraypt;
        List<Point> hit1 = new List<Point>();
        List<Point> hit2 = new List<Point>();
        bool hit_bool = false;//list 변경 변수
        Boolean res = true;


        foreach (var pt in points)
        {
            if (hit1 == null)
            {
                arraypt.X = Math.Truncate(pt.X);
                arraypt.Y = Math.Truncate(pt.Y);
                hit1.Add(arraypt);

                continue;
            }
            if (arraypt.X - Math.Truncate(pt.X) > 10 || Math.Truncate(pt.X) - arraypt.X > 10)
                //이전point와 현재point사이에 10 이상 차이가 나면 연속된 좌표가 끊긴 것
                //하나의 도형을 이루는 분리된 그림임을 판단 ex) 도형A의 경우 원과 마름모로 나눔
            {
                hit_bool = true;//연속된 좌표가 끊기면 true => 다음 list에 다음 도형의 좌표 삽입
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

        /* Point tem_hit = hit1[0];

         foreach (var hit_element in hit1)
         {
             drawLine(tem_hit, hit_element);
             tem_hit = hit_element;
         }*/

        res = compare(hit1, hit2);
        return res;
    }


    public bool compare(List<Point> a, List<Point> b)
    {
        Boolean res = false;

        foreach (var a_element in a)
        {
            foreach (var b_element in b)
            {
                if ((a_element.X + 3 > b_element.X && a_element.X - 3 < b_element.X) && a_element.Y == b_element.Y)
                { // 도형의 오차 3 두고 좌표가 겹쳤는지 비교
                    res = true;
                    //sum = sum + 1;
                    Debug.WriteLine("A element= (x)" + a_element.X + "(y)" + a_element.Y);
                    Debug.WriteLine("B element= (x)" + b_element.X + "(y)" + b_element.Y);

                }
            }
        }
        return res;
    }

    public List<string> OverlappingReport()
    {
        return overlappinginfo;
    }
}
