using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace BGTviewer
{
    class PositionA
    {
        //도형 A의 위치
        public void CheckPositionA(Drawing drawing, Figure[] figure)
        {

            Debug.WriteLine("도형버튼눌림");
            float inch = 2.54f;
            float psv = 0;

            if (figure[0] == null)
            {
                return;
            }
            Figure fA = figure[0];

            Point positionA = figure[0].Center;
            Debug.WriteLine(positionA);
            /*
            if ((drawing - 3 * inch) <= fA.figureStart.X && fA.figureStart.X <= (fileInfo.center.X + 3 * inch) &&
                ((fileInfo.center.Y - 3 * inch) <= fA.figureStart.Y && fA.figureStart.Y <= (fileInfo.center.Y + 3 * inch)) &&
                (fileInfo.center.X - 3 * inch) <= fA.EndPoint.X && fA.EndPoint.X <= (fileInfo.center.X + 3 * inch) &&
                ((fileInfo.center.Y - 3 * inch) <= fA.EndPoint.Y && fA.EndPoint.Y <= (fileInfo.center.Y + 3 * inch)))
            {   //자기중심적 위치
                psv = 5.0f;
                Debug.WriteLine(psv);
            }
            else if ((fileInfo.size.X - inch) <= fA.CenterPoint.X && fA.CenterPoint.X <= (fileInfo.size.X) ||
                (0 <= fA.CenterPoint.X) && (fA.CenterPoint.X <= inch) ||
                (fileInfo.size.Y - inch) <= fA.CenterPoint.Y && fA.CenterPoint.Y <= (fileInfo.size.Y) ||
                (0 <= fA.CenterPoint.Y) && (fA.CenterPoint.Y <= inch))
            {   //비정상적 위치
                psv = 10.0f;
                Debug.WriteLine(psv);
            }
            else if (fA.figureStart.Y <= fileInfo.size.Y / 3 && fA.EndPoint.Y <= fileInfo.size.Y / 3)
            {   //정상적 위치
                psv = 1.0f;
                Debug.WriteLine(fA.figureStart.Y);
                Debug.WriteLine(fA.EndPoint.Y);
                Debug.WriteLine(fileInfo.size.Y / 3);
            }


        }*/
    }
}

}
