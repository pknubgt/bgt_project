using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

class Rotation
{
    private Boolean flagrt = false;
    private Boolean flagrt_in = false;
    private double psv = 1.0;
    private Point[] vertex = new Point[2];
    private double[] angle = new double[9];
    private double max_angle = 0;
    private int fignum = 0;
    private List<string> rotationinfo = new List<string>();


    public double PSV
    {
        get { return psv; }
        set { psv = value; }
    }

    public Boolean Flagrt
    {
        get { return flagrt; }
        set { flagrt = value; }
    }
    public Boolean Flagrt_in
    {
        get { return flagrt_in; }
        set { flagrt_in = value; }
    }

    public Point[] Vertex
    {
        get { return vertex; }
        set { vertex = value; }
    }

    public int Fignum
    {
        get { return fignum; }
        set { fignum = value; }
    }

    public void rotation()
    {
        foreach (var a in angle)
        {
            if (max_angle < a) max_angle = a; //회전된 각도 중 제일 큰 각도로 점수 판별
        }

        if (max_angle <= 180 && max_angle >= 80) { psv = 10.0; }
        else if (max_angle <= 79 && max_angle >= 15) { psv = 7.0; }
        else if (max_angle <= 14 && max_angle >= 5) { psv = 4.0; }
        else if (max_angle <= 4) { }

    }
    public void Calc_RT()
    {
        angle[fignum] = Math.Abs(GetAngle(vertex[0], vertex[1]));//각 도형의 회전 각도 구한 다음 절댓값 구하여 배열에 값 입력
        Debug.WriteLine("angle [" + fignum + "] = " + angle[fignum]);

    }

    public double GetAngle(Point ver1, Point ver2) //x축과 선택한 두점(직선) 사이의 각도 구하기

    {
        double deltaX = ver2.X - ver1.X;
        double deltaY = ver2.Y - ver1.Y;

        return Math.Atan2(deltaY, deltaX) * (180d / Math.PI);
    }

    public List<string> RotationReport()
    {
        return rotationinfo;
    }
}
