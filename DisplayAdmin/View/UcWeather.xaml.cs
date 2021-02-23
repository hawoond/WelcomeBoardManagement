using DisplayAdmin.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
using System.Xml.Linq;

namespace DisplayAdmin.View
{
    /// <summary>
    /// UcWeather.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UcWeather : UserControl
    {
        private List<UdtWeather> BindingData;
        public UcWeather()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            //IMRUtils.CallRestFul callRestFul = new IMRUtils.CallRestFul();
            //IMRUtils.Model.UdtWeather udtWeather = callRestFul.CallWeatherApi();

            //rtbRestFulResult.AppendText("온도 :");
            //rtbRestFulResult.AppendText(udtWeather.Temp);
            //rtbRestFulResult.AppendText("\n체감온도 :");
            //rtbRestFulResult.AppendText(udtWeather.SensTemp);
            //rtbRestFulResult.AppendText("\n강수확률 :");
            //rtbRestFulResult.AppendText(udtWeather.Pop);
            //rtbRestFulResult.AppendText("\n미세먼지 :");
            //rtbRestFulResult.AppendText(udtWeather.FDust);
            //rtbRestFulResult.AppendText("\n초미세먼지 :");
            //rtbRestFulResult.AppendText(udtWeather.UfDust);

            //IMRUtils.CallRestFul callRestFul = new IMRUtils.CallRestFul();
            //List<IMRUtils.Model.UdtBook> arrBook = callRestFul.CallBookApi(tbBookSearch.Text);
        }

    }
}
