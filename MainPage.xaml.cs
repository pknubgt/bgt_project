using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Storage.Pickers.Provider;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Input.Inking;
using Windows.UI.ViewManagement;
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

        Figure[] figure = new Figure[9];
        Symbol LassoSelect = (Symbol)0xEF20;

        Point partStartPosition;
        Point partLastPosition;

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
                //var a = inkCanvas.InkPresenter.StrokeContainer.GetStrokes().ElementAt(0).GetInkPoints().ElementAt(0); //?
            }
            else
            {
                //this.textBlock.Text = "Operation cancelled.";
            }
        }

        ///클릭한 좌표를 얻어와 각도 측정 =>angle클래스로 옮김

        private void UnprocessedInput_PointerPressed(InkUnprocessedInput sender, PointerEventArgs args)
        {
            if (isPartPressure == true)
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

                /*
                if (angleMethod == 1)
                {
                    //클릭한 지점의 좌표를 받아 직선그려보기
                    anglePoints[pointsnum].X = args.CurrentPoint.Position.X;
                    anglePoints[pointsnum++].Y = args.CurrentPoint.Position.Y;

                    if (pointsnum == 2)//좌표를 두번째 까지 얻어왔을 때, 두개의 점을 이어서 선을 그린다.
                    {
                        drawLine(anglePoints[0], anglePoints[1]);
                    }
                    if (pointsnum == 3)//좌표를 세번째 까지 얻어왔을 때 두번째 선을 그리고, 두 선분이 이루는 각도를 계산한다.
                    {
                        drawLine(anglePoints[0], anglePoints[2]);
                        getAngle(anglePoints[1], anglePoints[0], anglePoints[2]);

                        pointsnum = 0;
                    }
                }*/
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

        /*
        /// <summary>
        /// 각도를 측정하는 방식
        /// </summary>
        //선택한 스트로크의 중심좌표끼리 선을이어 두개의 선 사이의 각도를 구함
        private void getPoints()
        {
            double average_X = 0.0f;
            double average_Y = 0.0f;
            int nTotalPoints = 0;
            var strokes = inkCanvas.InkPresenter.StrokeContainer.GetStrokes();

            if (pointsnum == 3)
            {
                pointsnum = 0;
                for (int i = 0; i < 3; i++)
                {
                    points[i].X = 0;
                    points[i].Y = 0;
                }
            }

            foreach (var stroke in strokes)
            {
                if (stroke.Selected != true)
                    continue;
                var points = stroke.GetInkPoints();

                foreach (var pt in points)
                {
                    average_X += pt.Position.X;
                    average_Y += pt.Position.Y;
                }
                nTotalPoints += points.Count;
            }
            average_X /= nTotalPoints;
            average_Y /= nTotalPoints;
            points[pointsnum].X = average_X;
            points[pointsnum++].Y = average_Y;

            if (pointsnum == 2)
            {
                drawLine(points[0], points[1]);
            }
            if (pointsnum == 3)
            {
                drawLine(points[0], points[2]);
                getAngle(points[1], points[0], points[2]);
                pointsnum = 0;
            }
        }

        //자동 각도 측정 : 가장 위, 왼쪽, 아래의 세 점을 이어서 각도를 구함
        Point[] tempoints = new Point[3];
        private void autoAngle()
        {
            var strokes = inkCanvas.InkPresenter.StrokeContainer.GetStrokes();
            tempoints[0].X = boundingRect.Right;
            tempoints[1].Y = boundingRect.Bottom;
            tempoints[2].Y = boundingRect.Top;

            foreach (var stroke in strokes)
            {
                if (stroke.Selected != true)
                    continue;
                var points = stroke.GetInkPoints();

                foreach (var pt in points)
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
                points[i] = tempoints[i];
            }

            drawLine(points[0], points[1]);
            drawLine(points[0], points[2]);
            getAngle(points[1], points[0], points[2]);
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

            selectionCanvas.Children.Add(line);
        }

        //각도 계산 함수
        private void getAngle(Point p1, Point p2, Point p3)
        {
            double o1 = Math.Atan((p1.Y - p2.Y) / (p1.X - p2.X));
            double o2 = Math.Atan((p3.Y - p2.Y) / (p3.X - p2.X));

            double degree = Math.Abs((o1 - o2) * 180 / Math.PI);

            //angle.Text = "angle : " + degree + " degree";
        }
        */

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
                if (figure[i].selected == true && isPartPressure != true)
                {
                    DrawBoundingRect(figure[i]);
                    TotalPressureGraph(figure[i]);
                    figure[i].BoundingRect = boundingRect;
                    figure[i].CalPoints(inkCanvas.InkPresenter.StrokeContainer.GetStrokes());
                    instruction.Text = figure[i].Name + " 전체 저장";
                    figure[i].selected = false;
                    Debug.WriteLine(figure[i].Name + " " + figure[i].BoundingRect.X + " " + figure[i].BoundingRect.Y);
                    Debug.WriteLine(figure[i].Points[1].X + " " + figure[i].Points[1].Y);

                    break;
                }
                else if (figure[i].selected == true && isPartPressure == true)
                {
                    PartPressureGraph(figure[i]);
                    instruction.Text = figure[i].Name + " 부분 선택 저장";
                    figure[i].selected = false;

                    break;
                }
                else if (figure[i].selected == false) //아무 버튼도 선택되지 않았을 경우  (i == figure.Length - 1 &&)
                {
                    instruction.Text = "먼저 정보를 저장할 해당하는 도형 버튼을 선택하세요";
                }
            }
        }

        private void initValue()
        {
            (LineChart.Series[0] as LineSeries).ItemsSource = null;
            (LineChart.Series[1] as LineSeries).ItemsSource = null;
        }

    }
}
