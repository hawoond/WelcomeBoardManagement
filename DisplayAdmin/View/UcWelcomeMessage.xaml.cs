using DisplayAdmin.Model;
using DisplayAdmin.Query;
using DisplayAdmin.Table;
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

namespace DisplayAdmin.View
{
    /// <summary>
    /// UcWelcomeMessage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UcWelcomeMessage : UserControl
    {
        private ExcuteQuery mExcuteQuery;
        private List<UdtWelcomeMessage> BindingData;
        private Realms.Realm realm;

        public UcWelcomeMessage()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            mExcuteQuery = ExcuteQuery.GetInstance();
            // 저장된 데이터 가져오기
            OTCSWELC data = mExcuteQuery.SelectWelcomeData();

            if (null != data)
            {
                tbMessage.Text = data.WELC_MSG;
            }
            else
            {
                mExcuteQuery.InsertWelcomeData("방문을 환영합니다.");
                SaveJson();
                tbMessage.Text = "방문을 환영합니다.";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbMessage.Text))
            {
                string message = tbMessage.Text;
                mExcuteQuery.InsertWelcomeData(message);
                SaveJson();
            }
            else
            {
                MessageBox.Show("메시지를 입력해 주세요");
            }
        }

        /// <summary>
        /// JSON 만들긔
        /// </summary>
        public void SaveJson()
        {
            List<object> arrTempResult = new List<object>();
            List<UdtWelcomeMessage> arrUdtWelcomeMessage = new List<UdtWelcomeMessage>();

            // 저장된 데이터 가져오기
            OTCSWELC data = mExcuteQuery.SelectWelcomeData();

            arrUdtWelcomeMessage.Add(new UdtWelcomeMessage()
            {
                DESC = data.WELC_MSG
            });


            foreach (UdtWelcomeMessage udtWelcomeMessage in arrUdtWelcomeMessage)
            {
                arrTempResult.Add(udtWelcomeMessage);
            }

            // 책 예약 정보 xml에 저장 -> 타이머로 하지 않을까?
            Common.StaticUtils.SaveJsonFile(Common.Constants.FILE_KEY.WELCOME_MESSAGE, arrTempResult, Common.Constants.CelebrationCode.NONE);
        }
    }
}
