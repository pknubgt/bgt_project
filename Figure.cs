using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Input.Inking;

public class Figure
{
    private string name;
    private List<Point> points;
    public bool selected;
    //스트로크 수 (보류)

    private Rect boundingRect;
    //+시작점 +센터 + 끝점

    public string Name
    {
        get { return name; }  set{ name = value; }
    }
    public List<Point> Points
    {
        get { return points; }  set { points = value; }
    }
    public Rect BoundingRect
    {
        get { return boundingRect; }
        set { boundingRect = value; }
    }
    public void CalPoints(IReadOnlyList<InkStroke> strokes)
    {
        points = new List<Point>();
        foreach (var stroke in strokes)
        {
            if (stroke.Selected != true)
                continue;
            var selectedPoints = stroke.GetInkPoints();

            foreach (var pt in selectedPoints)
            {
                if (points == null)
                {
                    points.Add(pt.Position);
                    continue;
                }
                if (points.Exists(p => p == pt.Position) != true)
                { 
                    points.Add(pt.Position);
                }
            }
        }
    }



}

