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
    /// UcTeachersDay.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UcTeachersDay : UserControl
    {
        private ExcuteQuery mExcuteQuery;
        private List<UdtTeachersDay> BindingData;

        public UcTeachersDay()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            mExcuteQuery = ExcuteQuery.GetInstance();
            // 저장된 데이터 가져오기
            OTCSTCDY data = mExcuteQuery.SelectTeachersDayData();

            if (null != data)
            {
                tbMessage.Text = data.CELE_MSG;
            }
            else
            {
                mExcuteQuery.InsertTeachersDayData("선생님 감사합니다.");
                SaveJson();
                tbMessage.Text = "선생님 감사합니다.";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbMessage.Text))
            {
                string message = tbMessage.Text;
                mExcuteQuery.InsertTeachersDayData(message);
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
            List<UdtTeachersDay> arrUdtTeachersDay = new List<UdtTeachersDay>();

            // 저장된 데이터 가져오기
            OTCSTCDY data = mExcuteQuery.SelectTeachersDayData();

            arrUdtTeachersDay.Add(new UdtTeachersDay()
            {
                DESC = data.CELE_MSG
            });


            foreach (UdtTeachersDay udtTeachersDay in arrUdtTeachersDay)
            {
                arrTempResult.Add(udtTeachersDay);
            }

            // 책 예약 정보 xml에 저장 -> 타이머로 하지 않을까?
            Common.StaticUtils.SaveJsonFile(Common.Constants.FILE_KEY.TEACHERS_DAY, arrTempResult, Common.Constants.CelebrationCode.NONE);
        }
    }
}
