using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

// 빈 페이지 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=234238에 나와 있습니다.

namespace BGTviewer
{
    public class AnotherPagePayload
    {
        public List<Pressure> parameter1 { get; set; }
        public List<Pressure> parameter2 { get; set; }
    }
       
    public sealed partial class NewView : Page
    {
        /*
        List<Pressure> pressurelist1;//전체 그래프의 값
        List<Pressure> pressurelist2;//선택된 그래프의 값
        */
        public NewView()
        {
            this.InitializeComponent();
        }

        private void RunIfSelected(UIElement element, Action action)
        {
            action.Invoke();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            AnotherPagePayload payload = (AnotherPagePayload) e.Parameter;

            // pressurelist1 = payload.parameter1;
            // pressurelist2 = payload.parameter2;

           this.RunIfSelected(this.LineChart, () => ((StackedLineSeries)this.LineChart.Series[0]).SeriesDefinitions[0].ItemsSource = payload.parameter1);
           this.RunIfSelected(this.LineChart, () => ((StackedLineSeries)this.LineChart.Series[0]).SeriesDefinitions[1].ItemsSource = payload.parameter2);
           //응용 프로그램이 다른 스레드를 위해 배열된 인터페이스를 호출했습니다. (Exception from HRESULT: 0x8001010E(RPC_E_WRONG_THREAD))'
        }

    }
}
