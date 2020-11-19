using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;

class Simplification
{
    private List<string> simplificationinfo = new List<string>();
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

    private DataTable readDB()
    {
        DataTable dt = new DataTable();
        string dbpath = @"Data Source=" + System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "database.sqlite");

        string sql = "SELECT * FROM Figureinfo";
        var adpt = new SQLiteDataAdapter(sql, dbpath);
        adpt.Fill(dt);

        return dt;
    }

    private float[] ColumnsAvg()
    {
        DataTable dt = readDB();
        float[] pointAvg = new float[9];

        pointAvg[0] = (float)Convert.ToDouble(dt.Compute("avg(pointsnum)"," "));
        pointAvg[1] = (float)Convert.ToDouble(dt.Compute("avg(PointNum_1)", " "));
        pointAvg[2] = (float)Convert.ToDouble(dt.Compute("avg(PointNum_2)", " "));
        pointAvg[3] = (float)Convert.ToDouble(dt.Compute("avg(PointNum_3)", " "));
        pointAvg[4] = (float)Convert.ToDouble(dt.Compute("avg(PointNum_4)", " "));
        pointAvg[5] = (float)Convert.ToDouble(dt.Compute("avg(PointNum_5)", " "));
        pointAvg[6] = (float)Convert.ToDouble(dt.Compute("avg(PointNum_6)", " "));
        pointAvg[7] = (float)Convert.ToDouble(dt.Compute("avg(PointNum_7)", " "));
        pointAvg[8] = (float)Convert.ToDouble(dt.Compute("avg(PointNum_8)", " "));
        
        return pointAvg;
    }

    public void simplification(Figure[] f)
    {
        simplification_1(f);
        simplification_2(f[6]);

        SimplificationScore(f);
    }
    
    private void simplification_1(Figure[] f)
    {
        float[] pointAvg = new float[9];
        float tmp = 0.0f;

        pointAvg = ColumnsAvg();

        /*
         * 도형1 : 12개의 점
         * 도형2 : 11개의 원이 3열로 구성 => 33개
         * 도형3 : 16개의 점
         * 도형5 : 19개의 점으로 된 곡선 + 7개의 점으로 된 선 => 26개
         */

        for(int i = 0; i <= 5; i++)
        {
            if(i == 1)
            {
                tmp = pointAvg[i] / 12;
            }
            else if(i == 2)
            {
                tmp = pointAvg[i] / 33;
            }
            else if(i == 3)
            {
                tmp = pointAvg[i] / 16;
            }
            else if(i == 5)
            {
                tmp = pointAvg[i] / 26;
            }
            else
            {
                continue;
            }

            if (f[i].Points.ToArray().Length <= pointAvg[i] - (3 * tmp)) //3 * tmp 값은 조정(오차)
                f[i].is_simplification = true;
        }
        
    }
    
    //도형 6
    private void simplification_2(Figure f6)
    {
        Point top, left, bottom, right;
        List<Point> pts = new List<Point>();
        //bool condition1, condition2;

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

    private void simplification_3(Figure f7, Figure f8)
    {

    }

    private void SimplificationScore(Figure[] f)
    {
        simplification(f);

        int count = 0;
        for(int i = 0; i < 9; i++)
        {
            if (f[i].is_simplification == true)
            {
                count++;
                simplificationinfo.Add("도형" + i + " 단순화");
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
    
    public List<string> SimplificationReport()
    {
        return simplificationinfo;
    }
}
