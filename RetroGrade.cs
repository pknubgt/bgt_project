using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace BGTviewer
{
    public class RetroGrade
    {
        private List<string> retrogradeInfo = new List<string>();
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

        public void retrograde(Figure[] f)
        {
            retrograde_sum(f);
            RetroGradeScore(f);
        }

        private void retrograde_sum(Figure[] f)
        {


            if (f[1].Points != null) //도형 1 점 10이하면 단순화
            {
                f[1].is_retrograde = Check_RG2(1, f[1].Points);

            }

            if (f[2].Points != null) //도형 2 점 31이하면 단순화
            {
                f[2].is_retrograde = Check_RG1(f[2].Points);
            }

            if (f[3].Points != null) //도형 3 점 14이하면 단순화
            {
                f[3].is_retrograde = Check_RG2(3, f[3].Points);
            }
            if (f[5].Points != null)
            {
                f[5].is_retrograde = Check_RG2(5, f[5].Points);
            }
            /*
             * 도형1 : 12개의 점
             * 도형2 : 11개의 원이 3열로 구성 => 33개
             * 도형3 : 16개의 점
             * 도형5 : 19개의 점으로 된 곡선 + 7개의 점으로 된 선 => 26개
             */
        }

        private Boolean Check_RG1(List<Point> list)//figure 2
        {
            Boolean flag = false;
            Point arraypt = new Point(0, 0);

            Point ct = new Point(0, 0);
            Point ct2 = new Point(0, 0);

            Boolean result = true; //디폴트 값 퇴영 ㅇㅇ
            int margin = 2;

            foreach (var pt in list)
            {
                if (flag == false)
                {
                    arraypt.X = Math.Truncate(pt.X);
                    arraypt.Y = Math.Truncate(pt.Y);

                    ct.X = Math.Truncate(pt.X);
                    ct.Y = Math.Truncate(pt.Y);

                    flag = true;

                    continue;
                }
                if ((arraypt.X - Math.Truncate(pt.X) > 7 || Math.Truncate(pt.X) - arraypt.X > 7) && (arraypt.Y - Math.Truncate(pt.Y) > 7 || Math.Truncate(pt.Y) - arraypt.Y > 7))
                {
                    ct2.X = arraypt.X;
                    ct2.Y = arraypt.Y;

                }

                if (((ct.X - margin <= ct2.X) && (ct.X + margin >= ct.X)) && ((ct.Y - margin <= ct2.Y) && (ct.Y + margin >= ct.Y)))
                {
                    result = false; //퇴영 아님 
                }

                ct.X = Math.Truncate(pt.X);
                ct.Y = Math.Truncate(pt.Y);

                arraypt.X = (int)(pt.X);
                arraypt.Y = (int)(pt.Y);
            }

            return result;
        }

        private Boolean Check_RG2(int i, List<Point> list)//figure 1,3,5
        {
            Boolean flag = false;
            Point arraypt = new Point(0, 0);

            Point ct = new Point(0, 0);
            Point ct2 = new Point(0, 0);

            Boolean result = false; //디폴트 값 퇴영 xx
            int margin = 10;

            double x_max = 0.0, x_min = 1000.0, y_max = 0.0, y_min = 1000.0;
            foreach (var pt in list)
            {
                if (flag == false)
                {
                    arraypt.X = Math.Truncate(pt.X);
                    arraypt.Y = Math.Truncate(pt.Y);

                    flag = true;

                    continue;
                }
                if ((arraypt.X - Math.Truncate(pt.X) > 10 || Math.Truncate(pt.X) - arraypt.X > 10) && (arraypt.Y - Math.Truncate(pt.Y) > 10 || Math.Truncate(pt.Y) - arraypt.Y > 10))
                {
                    if (x_max - x_min > margin || y_max - y_min > margin) result = true;
                    Debug.WriteLine("i번째 fig " + i + " xmax " + x_max + " xmin " + x_min + " ymax " + y_max + " ymin " + y_min);

                    x_max = 0.0; x_min = 1000.0; y_max = 0.0; y_min = 1000.0;
                }

                if (arraypt.X > x_max) x_max = arraypt.X;
                if (arraypt.X < x_min) x_min = arraypt.X;
                if (arraypt.Y > y_max) y_max = arraypt.Y;
                if (arraypt.Y < y_min) y_min = arraypt.Y;


                arraypt.X = (int)(pt.X);
                arraypt.Y = (int)(pt.Y);
            }

            return result;
        }

        private void RetroGradeScore(Figure[] f)
        {
            int count = 0;
            for (int i = 0; i < 9; i++)
            {
                if (f[i].is_retrograde == true)
                {
                    count++;

                    Debug.WriteLine("도형" + i + " 퇴영");
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
    }
}
