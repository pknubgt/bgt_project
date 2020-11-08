using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

class Simplification
{

    public void simplification(Figure f6)
    {
        Point top, left, bottom, right;
        List<Point> pts = new List<Point>();

        foreach (var stroke in f6.Strokes)
        {
            if (stroke.Selected != true)
                continue;
            var points = stroke.GetInkPoints();

            foreach (var pt in points)
            {
                pts.Add(pt.Position);

                if (f6.BoundingRect.X == pt.Position.X)
                    left = pt.Position;
                if (f6.BoundingRect.Y == pt.Position.Y)
                    top = pt.Position;
                if (f6.BoundingRect.X + f6.BoundingRect.Width == pt.Position.X)
                    right = pt.Position;
                if (f6.BoundingRect.Y + f6.BoundingRect.Height == pt.Position.Y)
                    bottom = pt.Position;
            }
        }

        //세로방향
        var m = (top.Y - bottom.Y) / (top.X - bottom.X);
        int count = 0;
        for (int i = (int)f6.BoundingRect.X; i < f6.BoundingRect.X + f6.BoundingRect.Width; i++)
        {
            for (int j = 0; j < pts.ToArray().Length; j++)
            {
                if (pts[j].Y - top.Y == m * (pts[j].X - top.X))
                {
                    count++;
                    if (count >= 8)
                        goto pass;
                }
            }
        }
        f6.is_simplification = true;
        goto end;

    pass:;

        //가로방향
        m = (right.Y - left.Y) / (right.X - left.X);
        count = 0;
        for (int i = (int)f6.BoundingRect.Y; i < f6.BoundingRect.Y + f6.BoundingRect.Height; i++)
        {
            for (int j = 0; j < pts.ToArray().Length; j++)
            {
                if (pts[j].Y - top.Y == m * (pts[j].X - top.X))
                {
                    count++;
                    if (count >= 8)
                        goto end;
                }
            }
        }
        f6.is_simplification = true;

    end:;
    }
}
