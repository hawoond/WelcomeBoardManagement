using DisplayAdmin.Model;
using DisplayAdmin.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DisplayAdmin
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private ExcuteQuery mExcuteQuery;
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            mExcuteQuery = ExcuteQuery.GetInstance();

            Common.StaticUtils.SaveAllJson();
            Common.StaticUtils.SaveWeatherJson();
            DispatcherTimer timerJsonCreate = new DispatcherTimer();
            timerJsonCreate.Interval = TimeSpan.FromTicks(100);
            timerJsonCreate.Tick += TimerJsonCreate_Tick;
            // test
            Common.StaticUtils.SaveWeatherJson();
        }


        private void TimerJsonCreate_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now.ToString("HHmmss").Equals("000000"))
            {
                // 날씨 날짜 상태값 변경
                ExcuteQuery.GetInstance().UpdatePrevWeatherData();
                // Json 새로 저장
                Common.StaticUtils.SaveAllJson();
                
            }

            if (DateTime.Now.ToString("HHmmss").Equals("070000") || DateTime.Now.ToString("HHmmss").Equals("073000") || DateTime.Now.ToString("HHmmss").Equals("080000") || DateTime.Now.ToString("HHmmss").Equals("083000") || DateTime.Now.ToString("HHmmss").Equals("090000") || DateTime.Now.ToString("HHmmss").Equals("093000") || DateTime.Now.ToString("HHmmss").Equals("100000") || DateTime.Now.ToString("HHmmss").Equals("113000") || DateTime.Now.ToString("120000").Equals("120000") || DateTime.Now.ToString("HHmmss").Equals("123000") || DateTime.Now.ToString("HHmmss").Equals("130000") || DateTime.Now.ToString("HHmmss").Equals("150000") || DateTime.Now.ToString("HHmmss").Equals("180000") || DateTime.Now.ToString("HHmmss").Equals("210000") || DateTime.Now.ToString("HHmmss").Equals("220000"))
            {
                // 날씨 갱신
                Common.StaticUtils.SaveWeatherJson();
            }
        }
    }
}
