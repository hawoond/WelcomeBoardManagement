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

namespace DisplayAdmin.View.Control
{
    /// <summary>
    /// UcConversationItem.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UcConversationItem : UserControl
    {
        int mIndex;
        public UcConversationItem()
        {
            InitializeComponent();
        }
        public void SetData(string sDate, string sEnglish, string sKorean, int nIndex)
        {
            tbDate.Text = sDate;
            tbEnglish.Text = sEnglish;
            tbKorean.Text = sKorean;
            mIndex = nIndex;
        }

        public int GetIndex()
        {
            return mIndex;
        }
    }
}
