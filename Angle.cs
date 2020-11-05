using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

class Angle
{
    private Point[] anglePoints = new Point[3];
    private int pointsnum = 0;

    public void getPoints(Figure f)
    {
        double average_X = 0.0f;
        double average_Y = 0.0f;
        int nTotalPoints = 0;

        if (pointsnum == 3)
        {
            pointsnum = 0;
            for (int i = 0; i < 3; i++)
            {
                anglePoints[i].X = 0;
                anglePoints[i].Y = 0;
            }
        }

        foreach (var stroke in f.Strokes)
        {
            if (stroke.Selected != true)
                continue;
            var anglePoints = stroke.GetInkPoints();

            foreach (var pt in anglePoints)
            {
                average_X += pt.Position.X;
                average_Y += pt.Position.Y;
            }
            nTotalPoints += anglePoints.Count;
        }
        average_X /= nTotalPoints;
        average_Y /= nTotalPoints;
        anglePoints[pointsnum].X = average_X;
        anglePoints[pointsnum++].Y = average_Y;

        if (pointsnum == 2)
        {
            drawLine(anglePoints[0], anglePoints[1]);
        }
        if (pointsnum == 3)
        {
            drawLine(anglePoints[0], anglePoints[2]);
            getAngle(anglePoints[1], anglePoints[0], anglePoints[2]);
            pointsnum = 0;
        }
    }

    //자동 각도 측정 : 가장 위, 왼쪽, 아래의 세 점을 이어서 각도를 구함
    Point[] tempoints = new Point[3];
    public void autoAngle(Figure f)
    {
        tempoints[0].X = f.BoundingRect.Right;
        tempoints[1].Y = f.BoundingRect.Bottom;
        tempoints[2].Y = f.BoundingRect.Top;

        foreach (var stroke in f.Strokes)
        {
            if (stroke.Selected != true)
                continue;
            var anglePoints = stroke.GetInkPoints();

            foreach (var pt in anglePoints)
            {
                if (tempoints[0].X > pt.Position.X)
                    tempoints[0] = pt.Position;
                if (tempoints[1].Y > pt.Position.Y)
                    tempoints[1] = pt.Position;
                if (tempoints[2].Y < pt.Position.Y)
                    tempoints[2] = pt.Position;
            }
        }

        for (int i = 0; i < 3; i++)
        {
            anglePoints[i] = tempoints[i];
        }

        drawLine(anglePoints[0], anglePoints[1]);
        drawLine(anglePoints[0], anglePoints[2]);
        getAngle(anglePoints[1], anglePoints[0], anglePoints[2]);
    }

    private void drawLine(Point pt1, Point pt2)
    {
        Line line = new Line();
        var brush = new SolidColorBrush(Windows.UI.Colors.DarkSeaGreen);
        line.Stroke = brush;
        line.StrokeThickness = 2;

        line.X1 = pt1.X;
        line.Y1 = pt1.Y;

        line.X2 = pt2.X;
        line.Y2 = pt2.Y;

        //BGTviewer.MainPage.selectionCanvas.Children.Add(line);
    }

    //각도 계산 함수
    public double getAngle(Point p1, Point p2, Point p3)
    {
        double o1 = Math.Atan((p1.Y - p2.Y) / (p1.X - p2.X));
        double o2 = Math.Atan((p3.Y - p2.Y) / (p3.X - p2.X));

        double degree = Math.Abs((o1 - o2) * 180 / Math.PI);

        return degree;
    }

}

