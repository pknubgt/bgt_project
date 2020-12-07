using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Input.Inking;

namespace BGTviewer
{
    class PositionA
    {
        private double psv = 1.0;
        private List<string> positionAinfo = new List<string>();

        public double PSV
        {
            get { return psv; }
            set { psv = value; }
        }

        //도형 A의 위치
        public void checkPositionA(Rect rect, Figure fA)
        {

            Debug.WriteLine("도형버튼눌림");
            double inch = rect.Height * (1 / 8); //용지의 세로 길이의 1/8을 1 inch로 함 (실제 인치 수를 대입하니까 용지 크기가 제각각이라서 결과가 다르게 나옴) 
            float psv = 0;
            Point rectStart = new Point(rect.Left, rect.Top);
            Point rectCenter = new Point((rect.Left + rect.Right) / 2, (rect.Top + rect.Bottom) / 2);
            Point rectEnd = new Point(rect.Right, rect.Bottom);

            Point positionA = fA.Center; //도형A의 중점을 기준으로 위치를 잡음
            Debug.WriteLine(positionA);

            //fA의 시작점과 끝점이 용지의 중앙 3인치 평방 내에 속하는지 비교
            if ((rectCenter.X - 3 * inch) <= fA.Start.X && fA.Start.X <= (rectCenter.X + 3 * inch) &&
                ((rectCenter.Y - 3 * inch) <= fA.Start.Y && fA.Start.Y <= (rectCenter.Y + 3 * inch)) &&
                (rectCenter.X - 3 * inch) <= fA.End.X && fA.End.X <= (rectCenter.X + 3 * inch) &&
                ((rectCenter.Y - 3 * inch) <= fA.End.Y && fA.End.Y <= (rectCenter.Y + 3 * inch)))
            {   //자기중심적 위치
                psv = 5.0f;
            }//용지 테두리 1인치 내에 fA의 시작점과 끝점이 모두 있는지 판단
            else if ((rect.Right - inch) <= fA.Start.X && fA.End.X <= (rect.Right) ||
                (rect.Left <= fA.Start.X) && (fA.End.X <= rect.Left + inch) ||
                (rect.Top - inch) <= fA.End.Y && fA.Start.Y <= (rect.Top) ||
                (rect.Bottom <= fA.End.Y) && (fA.Start.Y <= rect.Bottom + inch))
            {   //비정상적 위치
                psv = 10.0f;
            }//fA의 끝점이 용지 상부 1/3에 속하는지 확인
            else if (fA.End.Y <= rect.Top + rect.Height / 3)
            {   //정상적 위치
                psv = 1.0f;
            }//나머지 경우는 해당하는 점수가 없어서 일단 예외로 빼두고 비정상적 위치 처리함. 
            else { psv = 10.0f; }

        }
        public List<string> PositionAReport()
        {
            return positionAinfo;
        }
    }
}

