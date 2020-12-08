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
        public double PSV
        {
            get { return psv; }
            set { psv = value; }
        }

        //도형 A의 위치
        public void checkPositionA(Rect rect, Figure fA)
        {

            Debug.WriteLine("도형버튼눌림");
            double inch = rect.Width * (1 / 8);
            float psv = 0;
            Point rectStart = new Point(rect.Left, rect.Top);
            Point rectCenter = new Point((rect.Left + rect.Right) / 2, (rect.Top + rect.Bottom) / 2);
            Point rectEnd = new Point(rect.Right, rect.Bottom);

            Point positionA = fA.Center;
            Debug.WriteLine(positionA);

            if ((rectCenter.X - 3 * inch) <= fA.Start.X && fA.Start.X <= (rectCenter.X + 3 * inch) &&
                ((rectCenter.Y - 3 * inch) <= fA.Start.Y && fA.Start.Y <= (rectCenter.Y + 3 * inch)) &&
                (rectCenter.X - 3 * inch) <= fA.End.X && fA.End.X <= (rectCenter.X + 3 * inch) &&
                ((rectCenter.Y - 3 * inch) <= fA.End.Y && fA.End.Y <= (rectCenter.Y + 3 * inch)))
            {   //자기중심적 위치
                psv = 5.0f;
            }
            else if ((rect.Right - inch) <= fA.Start.X && fA.End.X <= (rect.Right) ||
                (rect.Left <= fA.Start.X) && (fA.End.X <= rect.Left + inch) ||
                (rect.Top - inch) <= fA.End.Y && fA.Start.Y <= (rect.Top) ||
                (rect.Bottom <= fA.End.Y) && (fA.Start.Y <= rect.Bottom + inch))
            {   //비정상적 위치
                psv = 10.0f;
            }
            else if (fA.End.Y <= rect.Top + rect.Height / 3)
            {   //정상적 위치
                psv = 1.0f;
            }
            else { psv = 10.0f; }

        }
    }
}

