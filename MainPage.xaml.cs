using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
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
    public class Pressure
    {
        public Point location { get; set; }
        public int index { get; set; }
        public float strength { get; set; }
    }

    public class Figure
    {
        //선택된 도형을 평가할 때 필요한 정보들을 저장할 변수들(앞으로 차차 필요한 변수를 추가할 예정)
        public Point figureStart = new Point();
        public int pointCount = 0;
        public float pressureAvg = 0.0f;
        public float pressureCrossingAvg = 0.0f;
        public float variance = 0.0f;
        public float standardDeviation = 0.0f;
        //...
    }

    public sealed partial class MainPage : Page
    {
        private Polyline lasso;
        private Rect boundingRect;
        private bool isBoundRect;
        //private Polyline line;

        private int selectAngleHow = 0;

        private bool pressure_select_mode = false;
        private Rectangle selectionRect;

        private bool crossing_difficulties = false; //교차곤란 버튼 선택

        //처음에 필요한 도형의 정보를 모두 저장해둘 클래스들
        Figure[] figure_info = new Figure[9];
        //버튼이 눌러졌을 때 true가 되도록 => 89번 줄
        bool[] f_selected = new bool[9];

        Symbol LassoSelect = (Symbol)0xEF20;

        /// ////////////////////////////////////////////////////////////////////////////////
        // 생성자 함수
        public MainPage()
        {
            /*
            for (int i = 0; i < f_selected.Length; i++)
                f_selected[i] = false;
            */

            this.InitializeComponent();
            inkCanvas.InkPresenter.InputDeviceTypes |= CoreInputDeviceTypes.Mouse;
            toolbar.ActiveTool = toolButtonLasso;   //디폴트 버튼으로 선택
            initValue();

            InkDrawingAttributes drawingAttributes = new InkDrawingAttributes();
            drawingAttributes.Color = Windows.UI.Colors.Black;
            drawingAttributes.IgnorePressure = false;
            drawingAttributes.FitToCurve = true;
            inkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);

            // Listen for button click to initiate recognition.
            //recognize.Click += RecognizeStrokes_Click;

        }

        //어떤 도형의 정보를 저장할 것인가?
        private void fA_select(object sender, RoutedEventArgs e)
        {
            f_selected[0] = true;
        }
        private void f1_select(object sender, RoutedEventArgs e)
        {
            f_selected[1] = true;
        }
        private void f2_select(object sender, RoutedEventArgs e)
        {
            f_selected[2] = true;
        }
        private void f3_select(object sender, RoutedEventArgs e)
        {
            f_selected[3] = true;
        }
        private void f4_select(object sender, RoutedEventArgs e)
        {
            f_selected[4] = true;
        }
        private void f5_select(object sender, RoutedEventArgs e)
        {
            f_selected[5] = true;
        }
        private void f6_select(object sender, RoutedEventArgs e)
        {
            f_selected[6] = true;
        }
        private void f7_select(object sender, RoutedEventArgs e)
        {
            f_selected[7] = true;
        }
        private void f8_select(object sender, RoutedEventArgs e)
        {
            f_selected[8] = true;
        }

        private void selected_figure()//어떤 도형 버튼이 선택됬는지 확인하고, 필요한 정보를 얻기위한 함수사용
        {
            int count = 0;
            for (int i = 0; i < f_selected.Length; i++)
            {
                if (f_selected[i] == true && pressure_select_mode != true)
                {
                    DrawBoundingRect(figure_info[i]);
                    CalcStrokeInfo(figure_info[i]);
                    f_selected[i] = false;
                }
                else if (f_selected[i] == true && pressure_select_mode == true)
                {
                    DrawBoundingRect(figure_info[i]);
                    f_selected[i] = false;
                }

                /*
                if (f_selected[i] == false)
                    count++;

                if(count == f_selected.Length - 1)
                    instruction.Text = "먼저 정보를 저장할 해당하는 도형 버튼을 선택하세요";
                else
                    instruction.Text = "저장";
                */
            }
        }

        //angle 메뉴
        private void use_click(object sender, RoutedEventArgs e)
        {
            selectAngleHow = 1;
        }
        private void use_stroke(object sender, RoutedEventArgs e)
        {
            selectAngleHow = 2;
        }
        private void use_auto(object sender, RoutedEventArgs e)
        {
            selectAngleHow = 3;
        }
        private void end_use(object sender, RoutedEventArgs e)
        {
            selectAngleHow = 0;
        }
        /// ////////////////////////////////////////////////////////////////////////////////
        // BGT 응답 파일(GIF)을 불러오는 함수
        private void Image_Tapped_Load(object sender, TappedRoutedEventArgs e)
        {
            Load();
        }

        private async void Load()//파일 탐색기를 사용할 수 있게 하는 함수
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".gif");
            picker.FileTypeFilter.Add(".isf");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();

            /*
            if (file == null)
            {
                return;
            }

            SoftwareBitmap softwareBitmap;

            using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read))
            {
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);

                softwareBitmap = await decoder.GetSoftwareBitmapAsync();
            }

            if (softwareBitmap.BitmapPixelFormat != BitmapPixelFormat.Bgra8 ||
             softwareBitmap.BitmapAlphaMode == BitmapAlphaMode.Straight)
            {
                softwareBitmap = SoftwareBitmap.Convert(softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
            }

            var source = new SoftwareBitmapSource();
            await source.SetBitmapAsync(softwareBitmap);

            img.Source = source;
            */


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


        /// //////////////////////////////////////////////////////////////////////////////////////////////
        /// Lasso Tool을 통해 마우스로 스트로크를 선택할 수 있도록 해 주는 관련 함수들
        /// 
        ///클릭한 좌표를 얻어와 각도 측정
        ///
        Point[] points = new Point[3];
        int pointsnum = 0;

        Point startposition;
        Point lastposition;

        private void UnprocessedInput_PointerPressed(InkUnprocessedInput sender, PointerEventArgs args)//마우스를 클릭했을 때
        {
            if (pressure_select_mode == true)//부분 필압을 측정하는 버튼이 선택됬을 경우
            {
                startposition = args.CurrentPoint.Position;
            }
            else //도형을 감싸는 영역을 선택할 경우
            {
                lasso = new Polyline()
                {
                    Stroke = new SolidColorBrush(Windows.UI.Colors.Blue),
                    StrokeThickness = 1,
                    StrokeDashArray = new DoubleCollection() { 5, 2 }, //도형의 윤곽선을 그리는 데 사용되는 대시 및 간격 패턴
                };

                lasso.Points.Add(args.CurrentPoint.RawPosition);
                selectionCanvas.Children.Add(lasso);

                //inkCanvas.Children.Add(lasso);
                isBoundRect = true;

                if (selectAngleHow == 1)
                {
                    //클릭한 지점의 좌표를 받아 직선그려보기
                    points[pointsnum].X = args.CurrentPoint.Position.X;
                    points[pointsnum++].Y = args.CurrentPoint.Position.Y;

                    if (pointsnum == 2)//좌표를 두번째 까지 얻어왔을 때, 두개의 점을 이어서 선을 그린다.
                    {
                        drawLine(points[0], points[1]);
                    }
                    if (pointsnum == 3)//좌표를 세번째 까지 얻어왔을 때 두번째 선을 그리고, 두 선분이 이루는 각도를 계산한다.
                    {
                        drawLine(points[0], points[2]);
                        getAngle(points[1], points[0], points[2]);

                        pointsnum = 0;
                    }
                }
            }
        }

        private void UnprocessedInput_PointerMoved(InkUnprocessedInput sender, PointerEventArgs args)//마우스를 움직일 경우
        {
            if (pressure_select_mode == true)
            {
                selectionCanvas.Children.Remove(selectionRect);
                lastposition = args.CurrentPoint.Position;

                try
                {
                    selectionRect = new Rectangle()
                    {
                        Stroke = new SolidColorBrush(Windows.UI.Colors.DarkGray),
                        StrokeThickness = 1,
                        StrokeDashArray = new DoubleCollection() { 5, 2 },
                        Width = lastposition.X - startposition.X,
                        Height = lastposition.Y - startposition.Y
                    };
                }
                catch
                {
                    lastposition = startposition;

                    selectionRect = new Rectangle()
                    {
                        Stroke = new SolidColorBrush(Windows.UI.Colors.DarkGray),
                        StrokeThickness = 1,
                        StrokeDashArray = new DoubleCollection() { 5, 2 },
                        Width = lastposition.X - startposition.X,
                        Height = lastposition.Y - startposition.Y
                    };
                }
                Canvas.SetLeft(selectionRect, startposition.X);
                Canvas.SetTop(selectionRect, startposition.Y);

                selectionCanvas.Children.Add(selectionRect);
            }
            else
            {
                if (isBoundRect)
                {
                    lasso.Points.Add(args.CurrentPoint.RawPosition);
                }
            }
        }

        private void UnprocessedInput_PointerReleased(InkUnprocessedInput sender, PointerEventArgs args)//마우스의 누르고 있던 버튼을 땔 경우
        {
            /*
            for (int i = 0; i < f_selected.Length; i++)
            {
                if (f_selected[i] == true)
                {
                    instruction.Text = "저장";
                    break;
                }
                else if (i == f_selected.Length - 1)
                {
                    instruction.Text = "정보를 저장할 해당하는 도형 버튼을 먼저 선택하세요";
                    //selectionCanvas.Children.Clear();
                }
            }*/

            if (pressure_select_mode == true)//도형의 부분 필압을 얻어오는 버튼을 눌렀을 경우
            {
                selected_figure();
                pressure_select_mode = false;
            }
            else
            {
                lasso.Points.Add(args.CurrentPoint.RawPosition);//-------------------------------------------------------여기?

                boundingRect = inkCanvas.InkPresenter.StrokeContainer.SelectWithPolyLine(lasso.Points);//----------------여기?

                isBoundRect = false;


                selected_figure();

                //각도를 측정하는 방식을 선택
                if (selectAngleHow == 2)
                    getPoints();
                else if (selectAngleHow == 3)
                    autoAngle();
            }

        }


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

        //////////////////////////////////////////////////////////////////////////
        /// 인식 복붙
        //////////////////////////////////////////////////////////////////////////

        //도형을 감싸는 사각형을 그리는 함수
        private void DrawBoundingRect(Figure f)
        {
            f = new Figure();

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

            f.figureStart = new Point(boundingRect.X, boundingRect.Y);
        }



        private void ToolButton_Lasso(object sender, RoutedEventArgs e)
        {
            // By default, pen barrel button or right mouse button is processed for inking
            // Set the configuration to instead allow processing these input on the UI thread
            inkCanvas.InkPresenter.InputProcessingConfiguration.RightDragAction = InkInputRightDragAction.LeaveUnprocessed;

            inkCanvas.InkPresenter.UnprocessedInput.PointerPressed += UnprocessedInput_PointerPressed;
            inkCanvas.InkPresenter.UnprocessedInput.PointerMoved += UnprocessedInput_PointerMoved;
            inkCanvas.InkPresenter.UnprocessedInput.PointerReleased += UnprocessedInput_PointerReleased;
        }

        private void ClearDrawnBoundingRect(object sender, RoutedEventArgs e)
        {
            if (selectionCanvas.Children.Count > 0)
            {
                selectionCanvas.Children.Clear();
                boundingRect = Rect.Empty;
                initValue();
            }
        }

        /// //////////////////////////////////////////////////////////////
        //스트로크 정보를 계산하고 그래프를 그림
        //////////////////////////////////////////////////////////////////
        List<Pressure> pressurelist1;//전체 그래프의 값
        List<Pressure> pressurelist2;//선택된 그래프의 값
        public void CalcStrokeInfo(Figure f)
        {
            f = new Figure();

            var pressure = 0.0f;
            var selected_pressure = 0.0f;
            var nTotalPoints = 0;
            var strokes = inkCanvas.InkPresenter.StrokeContainer.GetStrokes();
            int i = 0;
            float varTemp = 0.0f;

            if (pressure_select_mode != true)
            {
                pressurelist1 = new List<Pressure>();

                float sum = 0;

                foreach (var stroke in strokes)
                {
                    if (stroke.Selected != true)
                        continue;
                    var points = stroke.GetInkPoints();

                    foreach (var pt in points)
                    {
                        pressure += pt.Pressure;
                        sum += pt.Pressure;
                        i++;
                        if (i % 10 == 0)
                            pressurelist1.Add(new Pressure() { location = pt.Position, index = i, strength = sum / 10.0F });
                        sum = 0;
                    }
                    nTotalPoints += points.Count;
                }
                pressure /= nTotalPoints;
                f.pressureAvg = pressure;

                text1.Text = "전체 필압의 평균 : " + pressure.ToString();

                (LineChart.Series[0] as LineSeries).ItemsSource = null;
                (LineChart.Series[1] as LineSeries).ItemsSource = null;
                (LineChart.Series[0] as LineSeries).ItemsSource = pressurelist1;
            }
            else//지정한 사각형 안의 pressure 구하기
            {

                pressurelist2 = new List<Pressure>();

                foreach (var stroke in strokes)
                {
                    if (stroke.Selected != true)
                        continue;
                    var points = stroke.GetInkPoints();
                    foreach (var pt in points)
                    {
                        if (startposition.X <= pt.Position.X && startposition.Y <= pt.Position.Y &&
                            lastposition.X >= pt.Position.X && lastposition.Y >= pt.Position.Y)
                        {
                            selected_pressure += pt.Pressure;
                            varTemp += (pt.Pressure - pressure) * (pt.Pressure - pressure);//편차의 제곱의 합
                            nTotalPoints++;

                            if (pressurelist1.Exists(x => x.location == pt.Position) == true //전체 그래프에서 선택된 영역
                                && pressurelist2.Exists(x => x.location == pt.Position) != true) //선택된 영역에서 중복되지 않는 값
                            {
                                int idx = pressurelist1.FindIndex(v => v.location == pt.Position);
                                pressurelist2.Add(pressurelist1[idx]);
                            }
                        }
                    }
                }
                selected_pressure /= nTotalPoints;//선택영역의 평균
                varTemp /= nTotalPoints;//분산
                f.variance = varTemp;
                f.standardDeviation = (float)Math.Round(Math.Sqrt(varTemp), 2);

                text2.Text = "교차점영역 필압의 평균 : " + selected_pressure.ToString();

                (LineChart.Series[1] as LineSeries).ItemsSource = null;
                (LineChart.Series[1] as LineSeries).ItemsSource = pressurelist2;

                if (figure_info[6].standardDeviation != 0 && figure_info[7].standardDeviation != 0) //도형6과 도형7의 표준편차 값이 구해졌을 경우
                {
                    checkCrossing(figure_info[6], figure_info[7]);//점수를 측정한다.
                }
            }
        }

        public void checkCrossing(Figure f) //figure6과 figure7을 합쳐서 점수내야함                                                   --- 일단 테스트 용으로 간단하게 만듬
        {                                   //일단 figure6으로만 점수를 내봄
            //Debug.WriteLine("전체 평균 : " + f.pressureAvg.ToString());
            //Debug.WriteLine("분산 : " + f.variance.ToString());
            //Debug.WriteLine("표준편차 : " + f.standardDeviation.ToString());

            f = new Figure();

            float m = f.pressureAvg;
            float s = f.standardDeviation;
            float compare = f.pressureCrossingAvg;

            if (compare > m + s) //심하다
                resultCrossing.Text = "10.0";
            else if (compare <= m + s && compare > m + (s / 2))//보통이다
                resultCrossing.Text = "7.0";
            else if (compare <= m + (s / 2) && compare > m + (s / 4))//경미하다
                resultCrossing.Text = "4.0";
            else //없다                                                                                                               ------- 완전히 같을 순 없을텐데?
                resultCrossing.Text = "1.0";

            crossing_difficulties = false;
        }

        public void checkCrossing(Figure f6, Figure f7)
        {
            float m = 0;
            float s = 0;
            float compare = 0;
            int[] result = new int[2];

            f6 = new Figure();
            f7 = new Figure();

            for (int i = 0; i < 2; i++)
            {
                if (i == 0)
                {
                    m = f6.pressureAvg;
                    s = f6.standardDeviation;
                    compare = f6.pressureCrossingAvg;
                }
                else if (i == 1)
                {
                    m = f7.pressureAvg;
                    s = f7.standardDeviation;
                    compare = f7.pressureCrossingAvg;
                }

                if (compare > m + s) //심하다
                    result[i] = 10;
                else if (compare <= m + s && compare > m + (s / 2))//보통이다
                    result[i] = 7;
                else if (compare <= m + (s / 2) && compare > m + (s / 4))//경미하다
                    result[i] = 4;
                else //없다
                    result[i] = 1;
            }

            /*
            if (result[0] == result[1])
                resultCrossing.Text = result[0].ToString();
            else if()

            */
        }

        private void pressure_select(object sender, TappedRoutedEventArgs e)
        {
            pressure_select_mode = true;
        }

        private void initValue()
        {
            (LineChart.Series[0] as LineSeries).ItemsSource = null;
            (LineChart.Series[1] as LineSeries).ItemsSource = null;
        }

        private void Click_Crossing(object sender, RoutedEventArgs e)
        {
            crossing_difficulties = true;
        }
        
    }
}
