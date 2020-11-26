using SQLite.Net;
using SQLite.Net.Interop;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;
// 빈 페이지 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x412에 나와 있습니다.



namespace BGTviewer
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    /// 

    public sealed partial class MainPage : Page
    {
        private Polyline lasso;
        private int angleMethod = 0;
        private bool isBoundRect;
        private bool isPartPressure = false;
        private Rectangle partRect;
        private Rect boundingRect;
        private Rectangle total;

        public static Rect drawingRect;
        static Figure[] figure = new Figure[9];
        Symbol LassoSelect = (Symbol)0xEF20;

        Point partStartPosition;
        Point partLastPosition;

        Rotation rt = new Rotation();

        public MainPage()
        {
            SetFigureName();
            this.InitializeComponent();
            inkCanvas.InkPresenter.InputDeviceTypes |= CoreInputDeviceTypes.Mouse;
            toolbar.ActiveTool = toolButtonLasso;
            initValue();

            InkDrawingAttributes drawingAttributes = new InkDrawingAttributes();
            drawingAttributes.Color = Windows.UI.Colors.Black;
            drawingAttributes.IgnorePressure = false;
            drawingAttributes.FitToCurve = true;
            inkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
        }

        public void TestDatabase(object sender, RoutedEventArgs e)
        {
            // 데이터베이스 파일은 C:\Users\<사용자>\AppData\Local\Packages\<앱의 식별 ID>\LocalState에 저장될 것입니다.
            //C:\Users\borio\AppData\Local\Packages\99c2343f-8bed-4f8a-9010-d5e79a9c5980_4cyg4469rg6fr\LocalState
            string pathLocal = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "database.sqlite");
            // Universal Windows Platform에서 실행될 것이므로 플랫폼은 SQLite.Net.Platform.WinRT.SQLitePlatformWinRT를 지정합니다.
            ISQLitePlatform sqlitePlatform = new SQLitePlatformWinRT();
            FigureInfo figureinfo = null;

            int id = 0;
            using (SQLiteConnection conn = new SQLiteConnection(sqlitePlatform, pathLocal))
            {
                Point tmp = new Point(0, 0);
                conn.CreateTable<FigureInfo>();

                for (int i = 0; i < figure.Length; i++)
                {
                    if (figure[i].Points == null)
                    {
                        figure[i].Points = new List<Point>();
                        figure[i].Points.Add(tmp);
                    }
                    //if (figure[i].TotalPressure == null)
                    //   figure[i].TotalPressure = 0.0f;
                }

                figureinfo = new FigureInfo
                {
                    ID = id++,

                    PointNum_A = figure[0].Points.ToArray().Length,
                    PointNum_1 = figure[1].Points.ToArray().Length,
                    PointNum_2 = figure[2].Points.ToArray().Length,
                    PointNum_3 = figure[3].Points.ToArray().Length,
                    PointNum_4 = figure[4].Points.ToArray().Length,
                    PointNum_5 = figure[5].Points.ToArray().Length,
                    PointNum_6 = figure[6].Points.ToArray().Length,
                    PointNum_7 = figure[7].Points.ToArray().Length,
                    PointNum_8 = figure[8].Points.ToArray().Length,

                    TotalPressure_A = figure[0].TotalPressure,
                    TotalPressure_1 = figure[1].TotalPressure,
                    TotalPressure_2 = figure[2].TotalPressure,
                    TotalPressure_3 = figure[3].TotalPressure,
                    TotalPressure_4 = figure[4].TotalPressure,
                    TotalPressure_5 = figure[5].TotalPressure,
                    TotalPressure_6 = figure[6].TotalPressure,
                    TotalPressure_7 = figure[7].TotalPressure,
                    TotalPressure_8 = figure[8].TotalPressure,

                    PartPressure_6 = BGTviewer.MainPage.figure[6].PartPressure
                };
                Debug.WriteLine(string.Format("figure[0].Points = {0}, figure[0].TotalPressure = {1}", figure[0].Points.ToArray().Length, figure[0].TotalPressure));
                conn.Insert(figureinfo);
            }
        }

        private void SetFigureName()
        {
            for (int i = 0; i < 9; i++)
            {
                figure[i] = new Figure();
                if (i == 0)
                    figure[i].Name = "figureA";
                else
                    figure[i].Name = "figure" + i;
            }
        }
        private bool is_Done()
        {
            for (int i = 0; i < figure.Length; i++)
            {
                if (figure[i].complete == false)
                    goto end;
            }
            return true;

        end: return false;
        }

        private void GetResult()
        {
            Crossing crossing = new Crossing();//교차 곤란
            UseSpace useSpace = new UseSpace();//공간의 사용
            Simplification simple = new Simplification();//단순화
            Reiteration reiteration = new Reiteration();//중첩
            OverlappingDifficulty overlappingDifficulty = new OverlappingDifficulty();//중복 곤란

            crossing.checkCrossing(figure[6], figure[7]);
            //resultCrossing.Text = crossing.PSV.ToString();

            useSpace.UseOfSpace(figure);
            US.Text = useSpace.PSV.ToString();

            //simple.simplification(figure[6]);
            //SP.Text = figure[6].is_simplification.ToString();

            reiteration.reiteration(figure);
            RR.Text = reiteration.PSV.ToString();

            overlappingDifficulty.Overlapping_difficulty(figure);
            OD.Text = overlappingDifficulty.PSV.ToString();
        }

        public void Bt_US(object sender, RoutedEventArgs e)
        {
            UseSpace usespace = new UseSpace();
            usespace.UseOfSpace(figure);
            US.Text = usespace.PSV.ToString();
        }

        public void Bt_CS(object sender, RoutedEventArgs e)
        {
            Crossing crossing = new Crossing();
            crossing.checkCrossing(figure[6], figure[7]);
            CS.Text = crossing.PSV.ToString();
        }

        public void Bt_RR(object sender, RoutedEventArgs e)/////////////////////중첩
        {
            Reiteration rr = new Reiteration();
            rr.reiteration(figure);

            RR.Text = rr.PSV.ToString();
        }

        public void Bt_OD(object sender, RoutedEventArgs e)/////////////////////중복곤란
        {
            OverlappingDifficulty od = new OverlappingDifficulty();
            od.Overlapping_difficulty(figure);

            OD.Text = od.PSV.ToString();
        }

        public void Bt_RT(object sender, RoutedEventArgs e)/////////////////////지각한 회전 rotation
        {
            if (rt.Flagrt == false)
            {
                instruction.Text = "지각된 회전 점 선택";
                rt.Flagrt = true;
            }
            else
            {
                rt.Flagrt = false;
                rt.rotation();
                RT.Text = rt.PSV.ToString();
            }
        }

        public void Bt_PA(object sender, RoutedEventArgs e)/////////////////////도형A의 위치
        {
            PositionA pa = new PositionA();
            pa.checkPositionA(drawingRect, figure[0]);

            PA.Text = pa.PSV.ToString();
        }

        public void Bt_RG(object sender, RoutedEventArgs e)/////////////////////퇴영
        {
            RetroGrade rg = new RetroGrade();
            rg.retrograde(figure);

            RG.Text = rg.PSV.ToString();
        }


        public void Bt_UP(object sender, RoutedEventArgs e)
        {
            selectionCanvas.Children.Remove(total);

            float size = 1.2f;
            Debug.WriteLine("size: " + size);
            var container = inkCanvas.InkPresenter.StrokeContainer;
            var strokes = container.GetStrokes();
            var bounds = container.BoundingRect;
            var center = new Vector2((float)bounds.Left, (float)bounds.Top);
            var transform = Matrix3x2.CreateScale(size, size, center);
            foreach (var stroke in strokes)
            {
                stroke.PointTransform *= transform;
            }
            drawingRect = inkCanvas.InkPresenter.StrokeContainer.BoundingRect;

            total = new Rectangle()
            {
                Stroke = new SolidColorBrush(Windows.UI.Colors.Red),
                StrokeThickness = 1,
                StrokeDashArray = new DoubleCollection() { 5, 2 },
                Width = drawingRect.Width,
                Height = drawingRect.Height,
            };

            Canvas.SetLeft(total, bounds.Left);
            Canvas.SetTop(total, bounds.Top);

            
            selectionCanvas.Children.Add(total);
        }

        private void Bt_DOWN(object sender, RoutedEventArgs e)
        {
            selectionCanvas.Children.Remove(total);

            float size = 0.8f;
            var container = inkCanvas.InkPresenter.StrokeContainer;
            var strokes = container.GetStrokes();
            var bounds = container.BoundingRect;
            var center = new Vector2((float)bounds.Left, (float)bounds.Top);
            var transform = Matrix3x2.CreateScale(size, size, center);
            foreach (var stroke in strokes)
            {
                stroke.PointTransform *= transform;
            }

            drawingRect = inkCanvas.InkPresenter.StrokeContainer.BoundingRect;

            total = new Rectangle()
            {
                Stroke = new SolidColorBrush(Windows.UI.Colors.Red),
                StrokeThickness = 1,
                StrokeDashArray = new DoubleCollection() { 5, 2 },
                Width = drawingRect.Width,
                Height = drawingRect.Height,
            };

            Canvas.SetLeft(total, bounds.Left);
            Canvas.SetTop(total, bounds.Top);

            selectionCanvas.Children.Add(total);
            //drawing.Height = inkCanvas.InkPresenter.StrokeContainer.BoundingRect.Height; // 이거랑 bounds.Height랑 결과가 다름....
            //drawing.Width = inkCanvas.InkPresenter.StrokeContainer.BoundingRect.Width;
        }

        private void Bt_fA(object sender, RoutedEventArgs e)
        {
            figure[0].selected = true;
        }
        private void Bt_f1(object sender, RoutedEventArgs e)
        {
            figure[1].selected = true;
        }
        private void Bt_f2(object sender, RoutedEventArgs e)
        {
            figure[2].selected = true;
        }
        private void Bt_f3(object sender, RoutedEventArgs e)
        {
            figure[3].selected = true;
        }
        private void Bt_f4(object sender, RoutedEventArgs e)
        {
            figure[4].selected = true;
        }
        private void Bt_f5(object sender, RoutedEventArgs e)
        {
            figure[5].selected = true;
        }
        private void Bt_f6(object sender, RoutedEventArgs e)
        {
            figure[6].selected = true;
        }
        private void Bt_f7(object sender, RoutedEventArgs e)
        {
            figure[7].selected = true;
        }
        private void Bt_f8(object sender, RoutedEventArgs e)
        {
            figure[8].selected = true;
        }

        //angle 메뉴 
        private void Bt_useClick(object sender, RoutedEventArgs e)
        {
            angleMethod = 1;
        }
        private void Bt_useStroke(object sender, RoutedEventArgs e)
        {
            angleMethod = 2;
        }
        private void Bt_useAuto(object sender, RoutedEventArgs e)
        {
            angleMethod = 3;
        }
        private void Bt_InitAngleButton(object sender, RoutedEventArgs e)
        {
            angleMethod = 0;
        }
        private void Bt_PartPressure(object sender, TappedRoutedEventArgs e)
        {
            isPartPressure = true;
        }

        private void Bt_ImageTappedLoad(object sender, TappedRoutedEventArgs e)
        {
            Load();
        }

        private async void Load()
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".gif");
            picker.FileTypeFilter.Add(".isf");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();


            if (file != null)
            {
                // Application now has read/write access to the picked file
                // this.textBlock.Text = "Picked photo: " + file.Name;

                IInputStream stream = await file.OpenSequentialReadAsync();
                await inkCanvas.InkPresenter.StrokeContainer.LoadAsync(stream);


                //var a = inkCanvas.InkPresenter.StrokeContainer.GetStrokes().ElementAt(0).GetInkPoints().ElementAt(0); 
            }
            else
            {
                //this.textBlock.Text = "Operation cancelled.";
            }
        }

        private void UnprocessedInput_PointerPressed(InkUnprocessedInput sender, PointerEventArgs args)
        {
            if (rt.Flagrt == true) // 지각된 회전 옵션 - 양 끝점 좌표 찍기
            {

                var dot = new Ellipse()
                {
                    Stroke = new SolidColorBrush(Windows.UI.Colors.Red),
                    StrokeThickness = 3,
                    Width = 8,
                    Height = 8,
                };

                for (int i = 0; i < 9; i++)
                {
                    if (figure[i].selected == true && rt.Flagrt_in == false)
                    {
                        rt.Fignum = i;

                        rt.Vertex[0].X = args.CurrentPoint.Position.X;
                        rt.Vertex[0].Y = args.CurrentPoint.Position.Y;

                        Canvas.SetLeft(dot, rt.Vertex[0].X);
                        Canvas.SetTop(dot, rt.Vertex[0].Y);

                        selectionCanvas.Children.Remove(dot);
                        selectionCanvas.Children.Add(dot);
                        //Debug.WriteLine("x = " + rt.Vertex[0].X + " y= " + rt.Vertex[0].Y);

                        rt.Flagrt_in = true;
                    }
                }

                if (rt.Vertex[0].X != args.CurrentPoint.Position.X && rt.Vertex[0].Y != args.CurrentPoint.Position.Y && figure[rt.Fignum].selected == true)
                {
                    rt.Vertex[1].X = args.CurrentPoint.Position.X;
                    rt.Vertex[1].Y = args.CurrentPoint.Position.Y;

                    //Debug.WriteLine("x = " + rt.Vertex[1].X + " y= " + rt.Vertex[1].Y);

                    Canvas.SetLeft(dot, rt.Vertex[1].X);
                    Canvas.SetTop(dot, rt.Vertex[1].Y);

                    selectionCanvas.Children.Add(dot);
                    figure[rt.Fignum].selected = false;
                    rt.Flagrt_in = false;
                    rt.Calc_RT();

                }
            }
            else if (isPartPressure == true)
            {
                partStartPosition = args.CurrentPoint.Position;
            }
            else //도형을 감싸는 영역을 선택할 경우
            {
                lasso = new Polyline()
                {
                    Stroke = new SolidColorBrush(Windows.UI.Colors.Blue),
                    StrokeThickness = 1,
                    StrokeDashArray = new DoubleCollection() { 5, 2 },
                };

                lasso.Points.Add(args.CurrentPoint.RawPosition);
                selectionCanvas.Children.Add(lasso);

                //inkCanvas.Children.Add(lasso);
                isBoundRect = true;

            }
        }

        private void UnprocessedInput_PointerMoved(InkUnprocessedInput sender, PointerEventArgs args)
        {
            if (isPartPressure == true)
            {
                selectionCanvas.Children.Remove(partRect);
                partLastPosition = args.CurrentPoint.Position;

                try
                {
                    partRect = new Rectangle()
                    {
                        Stroke = new SolidColorBrush(Windows.UI.Colors.DarkGray),
                        StrokeThickness = 1,
                        StrokeDashArray = new DoubleCollection() { 5, 2 },
                        Width = partLastPosition.X - partStartPosition.X,
                        Height = partLastPosition.Y - partStartPosition.Y
                    };
                }
                catch
                {
                    partLastPosition = partStartPosition;

                    partRect = new Rectangle()
                    {
                        Stroke = new SolidColorBrush(Windows.UI.Colors.DarkGray),
                        StrokeThickness = 1,
                        StrokeDashArray = new DoubleCollection() { 5, 2 },
                        Width = partLastPosition.X - partStartPosition.X,
                        Height = partLastPosition.Y - partStartPosition.Y
                    };
                }
                Canvas.SetLeft(partRect, partStartPosition.X);
                Canvas.SetTop(partRect, partStartPosition.Y);

                selectionCanvas.Children.Add(partRect);
            }
            else
            {
                if (isBoundRect)
                {
                    lasso.Points.Add(args.CurrentPoint.RawPosition);
                }
            }
        }

        private void UnprocessedInput_PointerReleased(InkUnprocessedInput sender, PointerEventArgs args)
        {

            if (isPartPressure == true)
            {
                SelectedFigure();
                isPartPressure = false;
            }
            else
            {
                lasso.Points.Add(args.CurrentPoint.RawPosition);
                boundingRect = inkCanvas.InkPresenter.StrokeContainer.SelectWithPolyLine(lasso.Points);

                isBoundRect = false;


                SelectedFigure();

                /*
                //각도를 측정하는 방식을 선택
                if (angleMethod == 2)
                    getPoints();
                else if (angleMethod == 3)
                    autoAngle();
                */
            }

        }

        private void DrawBoundingRect(Figure f)
        {

            selectionCanvas.Children.Remove(lasso);

            if (boundingRect.Width <= 0 || boundingRect.Height <= 0)
            {
                return;
            }

            var rectangle = new Rectangle()
            {
                Stroke = new SolidColorBrush(Windows.UI.Colors.Blue),
                StrokeThickness = 1,
                StrokeDashArray = new DoubleCollection() { 5, 2 },
                Width = boundingRect.Width,
                Height = boundingRect.Height
            };

            Canvas.SetLeft(rectangle, boundingRect.X);
            Canvas.SetTop(rectangle, boundingRect.Y);

            selectionCanvas.Children.Add(rectangle);
        }

        private void DrawRect(Rectangle rectangle,double width, double height)
        {

            selectionCanvas.Children.Remove(rectangle);

            if (boundingRect.Width <= 0 || boundingRect.Height <= 0)
            {
                return;
            }

            rectangle = new Rectangle()
            {
                Stroke = new SolidColorBrush(Windows.UI.Colors.Coral),
                StrokeThickness = 1,
                StrokeDashArray = new DoubleCollection() { 5, 2 },
                Width = width,
                Height = height
            };

            Canvas.SetLeft(rectangle, boundingRect.X);
            Canvas.SetTop(rectangle, boundingRect.Y);

            selectionCanvas.Children.Add(rectangle);
        }

        private void Bt_ToolButtonLasso(object sender, RoutedEventArgs e)
        {
            inkCanvas.InkPresenter.InputProcessingConfiguration.RightDragAction = InkInputRightDragAction.LeaveUnprocessed;

            inkCanvas.InkPresenter.UnprocessedInput.PointerPressed += UnprocessedInput_PointerPressed;
            inkCanvas.InkPresenter.UnprocessedInput.PointerMoved += UnprocessedInput_PointerMoved;
            inkCanvas.InkPresenter.UnprocessedInput.PointerReleased += UnprocessedInput_PointerReleased;
        }

        private void Bt_ClearDrawnBoundingRect(object sender, RoutedEventArgs e)
        {
            if (selectionCanvas.Children.Count > 0)
            {
                selectionCanvas.Children.Clear();
                boundingRect = Rect.Empty;
                initValue();
            }
        }

        List<PressureGraph> TotalPressureList;
        List<PressureGraph> PartPressureList;

        public void PartPressureGraph(Figure f)
        {
            var strokes = inkCanvas.InkPresenter.StrokeContainer.GetStrokes();

            PartPressureList = new List<PressureGraph>();

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
                        if (TotalPressureList.Exists(x => x.location == pt.Position) == true
                            && PartPressureList.Exists(x => x.location == pt.Position) != true)
                        {
                            int idx = TotalPressureList.FindIndex(v => v.location == pt.Position);
                            PartPressureList.Add(TotalPressureList[idx]);
                        }
                    }
                }
            }

            (LineChart.Series[1] as LineSeries).ItemsSource = null;
            (LineChart.Series[1] as LineSeries).ItemsSource = PartPressureList;

        }

        public void TotalPressureGraph(Figure f)
        {
            var strokes = inkCanvas.InkPresenter.StrokeContainer.GetStrokes();
            int i = 0;

            TotalPressureList = new List<PressureGraph>();

            float sum = 0;

            foreach (var stroke in strokes)
            {
                if (stroke.Selected != true)
                    continue;
                var points = stroke.GetInkPoints();

                foreach (var pt in points)
                {
                    sum += pt.Pressure;
                    i++;
                    if (i % 10 == 0)
                        TotalPressureList.Add(new PressureGraph() { location = pt.Position, index = i, strength = sum / 10.0F });
                    sum = 0;
                }
            }

                (LineChart.Series[0] as LineSeries).ItemsSource = null;
            (LineChart.Series[1] as LineSeries).ItemsSource = null;
            (LineChart.Series[0] as LineSeries).ItemsSource = TotalPressureList;
        }

        private void SelectedFigure()
        {
            for (int i = 0; i < figure.Length; i++)
            {
                if (figure[i].selected == true && isPartPressure != true && rt.Flagrt != true)
                {
                    DrawBoundingRect(figure[i]);
                    TotalPressureGraph(figure[i]);
                    figure[i].BoundingRect = boundingRect;
                    figure[i].Start = new Point(boundingRect.Left, boundingRect.Top);
                    figure[i].Center = new Point((boundingRect.Left + boundingRect.Right) / 2, (boundingRect.Top + boundingRect.Bottom) / 2);
                    figure[i].End = new Point(boundingRect.Right, boundingRect.Bottom);
                    figure[i].Strokes = inkCanvas.InkPresenter.StrokeContainer.GetStrokes();
                    figure[i].CalPoints(figure[i].Strokes);
                    instruction.Text = figure[i].Name + " 전체 저장";
                    figure[i].selected = false;
                    figure[i].complete = true;
                    figure[i].CalcTotalPressure(figure[i].Strokes);
                    break;
                }
                else if (figure[i].selected == true && isPartPressure == true && rt.Flagrt != true)
                {
                    PartPressureGraph(figure[i]);
                    figure[i].CalcPartPressure(figure[i].Strokes, partStartPosition, partLastPosition);
                    instruction.Text = figure[i].Name + " 부분 선택 저장";
                    figure[i].selected = false;

                    break;
                }
                else if (figure[i].selected == false && rt.Flagrt != true) //아무 버튼도 선택되지 않았을 경우  (i == figure.Length - 1 &&)
                {
                    instruction.Text = "먼저 정보를 저장할 해당하는 도형 버튼을 선택하세요";
                }
            }
            /*
            if(is_Done()==true)
                GetResult();*/
        }

        private void initValue()
        {
            (LineChart.Series[0] as LineSeries).ItemsSource = null;
            (LineChart.Series[1] as LineSeries).ItemsSource = null;
        }


    }
}