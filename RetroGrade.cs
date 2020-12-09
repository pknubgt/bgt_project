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
        private double psv = 1.0;
        private List<string> retrogradeInfo = new List<string>();


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

            Boolean result = true; //디폴트 값은 퇴영O
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

                //두 좌표의 차이가 7보다 떨어져있으면 떨어져있는 도형에 속한 좌표로 인식
                if ((arraypt.X - Math.Truncate(pt.X) > 5 || Math.Truncate(pt.X) - arraypt.X > 5) 
                    && (arraypt.Y - Math.Truncate(pt.Y) > 5 || Math.Truncate(pt.Y) - arraypt.Y > 5))
                {
                    ct2.X = arraypt.X;
                    ct2.Y = arraypt.Y;

                }

                //+-2이내일 때 퇴영이 아니다.
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

            double a, b;

            int count = 1;

            Boolean result = false; //디폴트 값 퇴영 xx
            int margin = 15;

            Point start = new Point();
            Point end = new Point();

            double x_max = 0.0, x_min = 3000.0, y_max = 0.0, y_min = 3000.0;
            foreach (var pt in list)
            {
                if (flag == false)
                {
                    arraypt.X = Math.Truncate(pt.X);
                    arraypt.Y = Math.Truncate(pt.Y);

                    start = arraypt;

                    flag = true;

                    continue;
                }

                if ((arraypt.X - Math.Truncate(pt.X) > 3 || Math.Truncate(pt.X) - arraypt.X > 3) 
                    || (arraypt.Y - Math.Truncate(pt.Y) > 3 || Math.Truncate(pt.Y) - arraypt.Y > 3))
                {
                    end = arraypt;

                    if(Math.Abs(start.X - end.X) > margin || Math.Abs(start.Y - end.Y) > margin)
                    {
                        result = true; // 시작점과 끝점이 margin 이상 차이가 나면 line 퇴영 (방법 1)
                        Debug.WriteLine("start x = " + start.X + " start y = " + start.Y + " end x = " + end.X + " end y = " + end.Y);
                    }

                    /*if (x_max - x_min > margin || y_max - y_min > margin)
                    {
                        a = x_max - x_min;
                        b = y_max - y_min;
                        Debug.WriteLine(i + "도형 점 아님");
                        Debug.WriteLine("w_width = " + a + " x_max = " + x_max +" x_min = " + x_min);
                        Debug.WriteLine("y_height = " + b+ " y_max = " + y_max + " y_min = " + y_min);
                        result = true; /////////////////////////////점의 최대 최소 값이 margin 값 이상이면 점이 아닌 선 => 퇴영 (방법 2)
                        
                    }*/


                    count = count + 1;

                    //Debug.WriteLine("i번째 fig " + i + " xmax " + x_max + " xmin " + x_min + " ymax " + y_max + " ymin " + y_min);
                    //x_max = 0.0; x_min = 3000.0; y_max = 0.0; y_min = 3000.0;

                    start = pt;
                }

                //if (arraypt.X > x_max) x_max = arraypt.X;
                //if (arraypt.X < x_min) x_min = arraypt.X;
                //if (arraypt.Y > y_max) y_max = arraypt.Y;
                //if (arraypt.Y < y_min) y_min = arraypt.Y;


                arraypt.X = (int)(pt.X);
                arraypt.Y = (int)(pt.Y);
            }

            Debug.WriteLine(i + "번째 도형 count " + count);

            /*if (i == 1 && count <=10) // 점이 몇 개 이하면 퇴영
            {
                result = true;
                Debug.WriteLine("1번째 도형 count result true" + count);
            }else if (i == 3 && count<=14)
            {
                result = true;
                Debug.WriteLine("3번째 도형 count result true" + count);
            }
            else if(i==5 && count<=24)
            {
                result = true;
                Debug.WriteLine("5번째 도형 count result true " + count);
            }*/

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
                    retrogradeInfo.Add(f[i].Name + "퇴영");
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

        public List<string> RetroGradeReport()
        {
            return retrogradeInfo;
        }
    }
}
