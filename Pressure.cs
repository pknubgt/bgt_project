using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Input.Inking;

namespace BGTviewer
{
    class Pressure
    {
        float totalPressure;
        float partPressure;

        public float TotalPressure
        {
            get { return totalPressure; }
            set { totalPressure = value; }
        }

        public float PartPressure
        {
            get { return partPressure; }
            set { partPressure = value; }
        }

        public void CalcTotalPressure(Figure f, IReadOnlyList<InkStroke> strokes)
        {
            var nTotalPoints = 0;

            foreach (var stroke in strokes)
            {
                if (stroke.Selected != true)
                    continue;
                var points = stroke.GetInkPoints();

                foreach (var pt in points)
                {
                    totalPressure += pt.Pressure;
                }
                nTotalPoints += points.Count;
            }
            totalPressure /= nTotalPoints;

            //text1.Text = "전체 필압의 평균 : " + pressure.ToString();
        }

        public void CalcPartPressure(Figure f, IReadOnlyList<InkStroke> strokes, Point partStartPosition, Point partLastPosition)
        {
            var nTotalPoints = 0;

            foreach (var stroke in strokes)
            {
                if (stroke.Selected != true)
                    continue;
                var points = stroke.GetInkPoints();
                foreach (var pt in points)
                {
                    if (partStartPosition.X <= pt.Position.X && partStartPosition.Y <= pt.Position.Y &&
                        partLastPosition.X >= pt.Position.X && partLastPosition.Y >= pt.Position.Y)
                    {
                        partPressure += pt.Pressure;
                        nTotalPoints++;
                    }

                }
            }
            partPressure /= nTotalPoints;//선택영역의 평균

           //text2.Text = "교차점영역 필압의 평균 : " + partPressure.ToString();

        }
    }
}
