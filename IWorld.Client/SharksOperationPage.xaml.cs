using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using IWorld.Client.Class;
using IWorld.Client.FundsService;
using System.Windows.Threading;

namespace IWorld.Client
{
    public partial class SharksOperationPage : UserControl
    {

        private DispatcherTimer m_pLimitTimer = null;
        private DispatcherTimer m_pRotateTimer = null;
        private DispatcherTimer m_pSlowRotateTimer = null;
        private DispatcherTimer m_pEndSlowRotateTimer = null;
        private int m_nSlowRotateCount = 0;
        private int m_nRandom = 5;
        private DateTime m_dtBgTime;
        private DateTime m_dtEndTime;
             

        public SharksOperationPage()
        {
            InitializeComponent();

            InitRotateItems();

            InitTimer();
        }
        

        public int LimitTime
        {
            get { return (int)GetValue(LimitTimeProperty); }
            set { SetValue(LimitTimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LimitTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LimitTimeProperty =
            DependencyProperty.Register("LimitTime", typeof(int), typeof(SharksOperationPage), new PropertyMetadata(0));



        void InitRotateItems()
        {
            lstRotateItems.Items.Clear();
            for (int nIndex = 0; nIndex < 28; nIndex++)
            {
                SharkRotateItemCtrl rotItem = new SharkRotateItemCtrl();
                if (nIndex == 0)
                {
                    rotItem.IsSelected = true;
                }
                rotItem.RotateItemID = nIndex;
                rotItem.RotateItemName = "鲨鱼";
                rotItem.RotateItemImagePath = "img/shark/thumb_animal" + nIndex + ".png";
                lstRotateItems.Items.Add(rotItem);
            }

        }

        void InitTimer()
        {
            LimitTime = 10;
            if (m_pLimitTimer == null)
            {
                m_pLimitTimer = new DispatcherTimer();
                m_pLimitTimer.Interval = TimeSpan.FromSeconds(1);
                m_pLimitTimer.Tick += m_pLimitTimer_Tick;
                m_pLimitTimer.Start();
            }

            if (m_pSlowRotateTimer == null)
            {
                m_pSlowRotateTimer = new DispatcherTimer();
                m_pSlowRotateTimer.Interval = TimeSpan.FromSeconds(0.3);
                m_pSlowRotateTimer.Tick += m_pSlowRotateTimer_Tick;
            }

            if (m_pRotateTimer == null)
            {
                m_pRotateTimer = new DispatcherTimer();
                m_pRotateTimer.Interval = TimeSpan.FromSeconds(0.1);
                m_pRotateTimer.Tick += m_pRotateTimer_Tick;
            }
            if (m_pEndSlowRotateTimer == null)
            {
                m_pEndSlowRotateTimer = new DispatcherTimer();
                m_pEndSlowRotateTimer.Interval = TimeSpan.FromSeconds(0.3);
                m_pEndSlowRotateTimer.Tick += m_pEndSlowRotateTimer_Tick;
            }
        }

        void m_pLimitTimer_Tick(object sender, EventArgs e)
        {
            DispatcherTimer dt = sender as DispatcherTimer;
            LimitTime -= 1;
            if (LimitTime <= 0)
            {
                dt.Stop();
                if (m_pSlowRotateTimer != null)
                {
                    m_nSlowRotateCount = 0;
                    m_pSlowRotateTimer.Start();
                }
            }
        }

        void m_pSlowRotateTimer_Tick(object sender, EventArgs e)
        {
            ChangeSelectRotateItemToNext();
            m_nSlowRotateCount++;
            if (m_nSlowRotateCount > 10)
            {
                m_pSlowRotateTimer.Stop();
                if (m_pRotateTimer != null)
                {
                    m_pRotateTimer.Start();
                    Random rd = new Random();
                    m_nRandom = rd.Next(5, 11);
                    m_dtBgTime = DateTime.Now;
                    
                }
            }
        }

        void m_pRotateTimer_Tick(object sender, EventArgs e)
        {
            ChangeSelectRotateItemToNext();
            m_dtEndTime = DateTime.Now;
            TimeSpan ts = m_dtEndTime - m_dtBgTime;
            if (ts.TotalSeconds > m_nRandom)
            {
                m_pRotateTimer.Stop();
                m_nSlowRotateCount = 0;
                if (m_pEndSlowRotateTimer != null)
                {
                    m_pEndSlowRotateTimer.Start();
                }
            }
        }


         void m_pEndSlowRotateTimer_Tick(object sender, EventArgs e)
        {
            ChangeSelectRotateItemToNext();
            m_nSlowRotateCount++;
            if (m_nSlowRotateCount > 10)
            {
                m_pEndSlowRotateTimer.Stop();
                if (m_pLimitTimer != null)
                {
                    LimitTime = 10;
                    m_pLimitTimer.Start();
                }
            }
        }

        void ChangeSelectRotateItemToNext()
        {
            try
            {
                int nIndex = 0;
                SharkRotateItemCtrl findSelItem = (SharkRotateItemCtrl)lstRotateItems.Items.FirstOrDefault(item => ((SharkRotateItemCtrl)item).IsSelected);                
                if (findSelItem != null)
                {
                    nIndex = findSelItem.RotateItemID;
                }
                nIndex++;
                if (nIndex >= lstRotateItems.Items.Count)
                {
                    nIndex = 0;
                }
                foreach (var item in lstRotateItems.Items)
                {
                    ((SharkRotateItemCtrl)item).IsSelected=false;
                }
                ((SharkRotateItemCtrl)lstRotateItems.Items[nIndex]).IsSelected = true;
            }
            catch { }
        }


    }
}
