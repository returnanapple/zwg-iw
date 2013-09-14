using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
using System.Net;
using System.IO;
using System.Data.Entity;
using IWorld.Model;
using IWorld.Setting;

namespace IWorld.BLL
{
    /// <summary>
    /// 数据采集的管理者对象
    /// </summary>
    public class CollectionManager
    {
        #region 公共方法

        /// <summary>
        /// 初始化数据采集（默认网站启动即开始采集）
        /// </summary>
        public static void Initialize()
        {
            CQSSCEventhandler += InsertCQSSC;
            JXSSCEventhandler += InsertJXSSC;
            XJSSCEventhandler += InsertXJSSC;
            GD11X5Eventhandler += InsertGD11X5;
            SYDYJEventhandler += InsertSYDYJ;
            SHSSLEventhandler += InsertSHSSL;
            FC3DEventhandler += InsertFC3D;
            PLSEventhandler += InsertPLS;

            System.Threading.Thread thread = new System.Threading.Thread((obj) =>
                {
                    Timer timer = new Timer(10000);
                    timer.Elapsed += (sender, e) =>
                        {
                            #region 采集数据
                            WebSetting webSetting = new WebSetting();
                            if (webSetting.CollectionRunning == true)
                            {
                                List<NcDelegate> ecentHandlers = new List<NcDelegate>();
                                ecentHandlers.Add(CQSSCEventhandler);   //重庆时时彩
                                ecentHandlers.Add(JXSSCEventhandler);   //江西时时彩
                                ecentHandlers.Add(XJSSCEventhandler);   //新疆时时彩
                                ecentHandlers.Add(GD11X5Eventhandler);  //广东十一选五
                                ecentHandlers.Add(SYDYJEventhandler);   //十一夺运金
                                ecentHandlers.Add(SHSSLEventhandler);   //上海时时乐
                                ecentHandlers.Add(FC3DEventhandler);    //福彩3D
                                ecentHandlers.Add(PLSEventhandler); //排列三

                                ecentHandlers.ForEach(x =>
                                    {
                                        if (x != null)
                                        {
                                            WebMapContext db = new WebMapContext();
                                            x.BeginInvoke(db, null, null);
                                        }
                                    });
                            }
                            #endregion
                        };
                    timer.Start();
                });
            thread.Start();
        }

        #region 事件和委托

        /// <summary>
        /// 默认的实例委托
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        private delegate void NcDelegate(DbContext db);

        /// <summary>
        /// 采集重庆时时彩数据的实例委托
        /// </summary>
        private static event NcDelegate CQSSCEventhandler;

        /// <summary>
        /// 采集江西时时彩数据的实例委托
        /// </summary>
        private static event NcDelegate JXSSCEventhandler;

        /// <summary>
        /// 采集新疆时时彩数据的实例委托
        /// </summary>
        private static event NcDelegate XJSSCEventhandler;

        /// <summary>
        /// 采集广东十一选五数据的实例委托
        /// </summary>
        private static event NcDelegate GD11X5Eventhandler;

        /// <summary>
        /// 采集十一夺运金数据的实例委托
        /// </summary>
        private static event NcDelegate SYDYJEventhandler;

        /// <summary>
        /// 采集上海时时乐数据的实例委托
        /// </summary>
        private static event NcDelegate SHSSLEventhandler;

        /// <summary>
        /// 采集福彩3D数据的实例委托
        /// </summary>
        private static event NcDelegate FC3DEventhandler;

        /// <summary>
        /// 采集排列三数据的实例委托
        /// </summary>
        private static event NcDelegate PLSEventhandler;

        #endregion

        #endregion

        #region 采集数据

        /// <summary>
        /// 采集重庆时时彩数据
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        private static void InsertCQSSC(DbContext db)
        {
            string ticiketName = "重庆时时彩";
            if (!InsertingCQSSC)
            {
                InsertingCQSSC = true;
                #region 插入

                try
                {
                    int maxP = 120;
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://caipiao.163.com/award/ssc/");
                    request.Method = "GET";
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream stream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    string html = reader.ReadToEnd();
                    stream.Close();

                    Regex reg1 = new Regex("<span id=\"date_no\">(\\d+)</span>");
                    string phases = reg1.Match(html).Groups[1].Value;

                    bool hadLottery = db.Set<Lottery>().Any(x => x.Ticket.Name == ticiketName && x.Phases == phases);
                    if (!hadLottery)
                    {
                        List<string> bs = new List<string>();
                        Regex reg2 = new Regex("<span class=\"red_ball\">(\\d+)</span>");
                        Match m = reg2.Match(html);
                        while (m.Success)
                        {
                            bs.Add(m.Groups[1].Value);
                            m = m.NextMatch();
                        }
                        int nextP = Convert.ToInt32(phases.Substring(6, 3)) + 1;
                        int tYear = Convert.ToInt32(phases.Substring(0, 2)) + 2000;
                        int tMonth = Convert.ToInt32(phases.Substring(2, 2));
                        int tDay = Convert.ToInt32(phases.Substring(4, 2));
                        DateTime tTime = new DateTime(tYear, tMonth, tDay);
                        if (nextP > maxP)
                        {
                            nextP = 1;
                            tTime = tTime.AddDays(1);
                        }
                        string n = tTime.Year.ToString().Substring(2, 2) + tTime.Month.ToString("00")
                            + tTime.Day.ToString("00") + nextP.ToString("000");

                        AddLottery(db, ticiketName, phases, bs, n);
                        CacheManager.SetCollectionResultIn(string.Format("{0} 第{1}期 {2} 下期:{3}"
                            , ticiketName, phases, string.Join(" | ", bs), n));
                    }
                }
                catch (Exception)
                {
                }

                #endregion
                InsertingCQSSC = false;
            }
        }

        /// <summary>
        /// 采集江西时时彩数据
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        private static void InsertJXSSC(DbContext db)
        {
            string ticiketName = "江西时时彩";
            if (!InsertingJXSSC)
            {
                InsertingJXSSC = true;
                #region 插入

                try
                {
                    int maxP = 84;
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://caipiao.163.com/award/jxssc/");
                    request.Method = "GET";
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream stream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    string html = reader.ReadToEnd();
                    stream.Close();

                    Regex reg1 = new Regex("<span id=\"date_no\">(\\d+)</span>");
                    string phases = reg1.Match(html).Groups[1].Value;

                    bool hadLottery = db.Set<Lottery>().Any(x => x.Ticket.Name == ticiketName && x.Phases == phases);
                    if (!hadLottery)
                    {
                        List<string> bs = new List<string>();
                        Regex reg2 = new Regex("<span class=\"red_ball\">(\\d+)</span>");
                        Match m = reg2.Match(html);
                        while (m.Success)
                        {
                            bs.Add(m.Groups[1].Value);
                            m = m.NextMatch();
                        }
                        int nextP = Convert.ToInt32(phases.Substring(8, 3)) + 1;
                        int tYear = Convert.ToInt32(phases.Substring(0, 4));
                        int tMonth = Convert.ToInt32(phases.Substring(4, 2));
                        int tDay = Convert.ToInt32(phases.Substring(6, 2));
                        DateTime tTime = new DateTime(tYear, tMonth, tDay);
                        if (nextP > maxP)
                        {
                            nextP = 1;
                            tTime = tTime.AddDays(1);
                        }
                        string n = tTime.Year.ToString().Substring(0, 4) + tTime.Month.ToString("00")
                            + tTime.Day.ToString("00") + nextP.ToString("000");

                        AddLottery(db, ticiketName, phases, bs, n);
                        CacheManager.SetCollectionResultIn(string.Format("{0} 第{1}期 {2} 下期:{3}"
                            , ticiketName, phases, string.Join(" | ", bs), n));
                    }
                }
                catch (Exception)
                {
                }

                #endregion
                InsertingJXSSC = false;
            }
        }

        /// <summary>
        /// 采集新疆时时彩数据
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        private static void InsertXJSSC(DbContext db)
        {
            string ticiketName = "新疆时时彩";
            if (!InsertingXJSSC)
            {
                InsertingXJSSC = true;
                #region 插入

                try
                {
                    int maxP = 96;
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://www.xjflcp.com/ssc/");
                    request.Method = "GET";
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream stream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    string html = reader.ReadToEnd();
                    stream.Close();

                    Regex reg1 = new Regex("<td><a href=\"javascript:detatilssc\\('(\\d+)'\\);");
                    string phases = reg1.Match(html).Groups[1].Value;

                    bool hadLottery = db.Set<Lottery>().Any(x => x.Ticket.Name == ticiketName && x.Phases == phases);
                    if (!hadLottery)
                    {
                        Regex reg2 = new Regex("<td class=\"red\"><p>([0-9 ]+)</p></td>");
                        string _str = reg2.Match(html).Groups[1].Value;
                        List<string> bs = _str.Split(new char[] { ' ' }).ToList();
                        int nextP = Convert.ToInt32(phases.Substring(8, 2)) + 1;
                        int tYear = Convert.ToInt32(phases.Substring(0, 4));
                        int tMonth = Convert.ToInt32(phases.Substring(4, 2));
                        int tDay = Convert.ToInt32(phases.Substring(6, 2));
                        DateTime tTime = new DateTime(tYear, tMonth, tDay);
                        if (nextP > maxP)
                        {
                            nextP = 1;
                            tTime = tTime.AddDays(1);
                        }
                        string n = tTime.Year.ToString().Substring(0, 4) + tTime.Month.ToString("00")
                            + tTime.Day.ToString("00") + nextP.ToString("00");

                        AddLottery(db, ticiketName, phases, bs, n);
                        CacheManager.SetCollectionResultIn(string.Format("{0} 第{1}期 {2} 下期:{3}"
                            , ticiketName, phases, string.Join(" | ", bs), n));
                    }
                }
                catch (Exception)
                {
                }

                #endregion
                InsertingXJSSC = false;
            }
        }

        /// <summary>
        /// 采集广东十一选五数据
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        private static void InsertGD11X5(DbContext db)
        {
            string ticiketName = "广东十一选五";
            if (!InsertingGD11X5)
            {
                InsertingGD11X5 = true;
                #region 插入

                int maxP = 84;
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://baidu.lecai.com/lottery/draw/view/23");
                request.Method = "GET";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.0)";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                string html = reader.ReadToEnd();
                stream.Close();

                Regex reg1 = new Regex("var latest_draw_phase = '(\\d+)';");
                string phases = reg1.Match(html).Groups[1].Value;

                bool hadLottery = db.Set<Lottery>().Any(x => x.Ticket.Name == ticiketName && x.Phases == phases);
                if (!hadLottery)
                {
                    List<string> bs = new List<string>();
                    Regex reg2 = new Regex("var latest_draw_result =(.*);");
                    string _str = reg2.Match(html).Groups[1].Value;
                    Regex reg3 = new Regex("\"(\\d+)\"");
                    Match m = reg3.Match(_str);
                    while (m.Success)
                    {
                        bs.Add(m.Groups[1].Value);
                        m = m.NextMatch();
                    }
                    int nextP = Convert.ToInt32(phases.Substring(6, 2)) + 1;
                    int tYear = Convert.ToInt32(phases.Substring(0, 2)) + 2000;
                    int tMonth = Convert.ToInt32(phases.Substring(2, 2));
                    int tDay = Convert.ToInt32(phases.Substring(4, 2));
                    DateTime tTime = new DateTime(tYear, tMonth, tDay);
                    if (nextP > maxP)
                    {
                        nextP = 1;
                        tTime = tTime.AddDays(1);
                    }
                    string n = tTime.Year.ToString().Substring(2, 2) + tTime.Month.ToString("00")
                        + tTime.Day.ToString("00") + nextP.ToString("00");

                    AddLottery(db, ticiketName, phases, bs.Take(5).ToList(), n);
                    CacheManager.SetCollectionResultIn(string.Format("{0} 第{1}期 {2} 下期:{3}"
                        , ticiketName, phases, string.Join(" | ", bs.Take(5)), n));
                }

                #endregion
                InsertingGD11X5 = false;
            }
        }

        /// <summary>
        /// 采集十一夺运金数据
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        private static void InsertSYDYJ(DbContext db)
        {
            string ticiketName = "十一夺运金";
            if (!InsertingSYDYJ)
            {
                InsertingSYDYJ = true;
                #region 插入

                try
                {
                    int maxP = 78;
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://caipiao.163.com/award/11x5.html");
                    request.Method = "GET";
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream stream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    string html = reader.ReadToEnd();
                    stream.Close();

                    Regex reg1 = new Regex("<span id=\"date_no\">(\\d+)</span>");
                    string phases = reg1.Match(html).Groups[1].Value;

                    bool hadLottery = db.Set<Lottery>().Any(x => x.Ticket.Name == ticiketName && x.Phases == phases);
                    if (!hadLottery)
                    {
                        List<string> bs = new List<string>();
                        Regex reg2 = new Regex("<span class=\"red_ball\">(\\d+)</span>");
                        Match m = reg2.Match(html);
                        while (m.Success)
                        {
                            bs.Add(m.Groups[1].Value);
                            m = m.NextMatch();
                        }
                        int nextP = Convert.ToInt32(phases.Substring(6, 2)) + 1;
                        int tYear = Convert.ToInt32(phases.Substring(0, 2)) + 2000;
                        int tMonth = Convert.ToInt32(phases.Substring(2, 2));
                        int tDay = Convert.ToInt32(phases.Substring(4, 2));
                        DateTime tTime = new DateTime(tYear, tMonth, tDay);
                        if (nextP > maxP)
                        {
                            nextP = 1;
                            tTime = tTime.AddDays(1);
                        }
                        string n = tTime.Year.ToString().Substring(2, 2) + tTime.Month.ToString("00")
                            + tTime.Day.ToString("00") + nextP.ToString("00");

                        AddLottery(db, ticiketName, phases, bs, n);
                        CacheManager.SetCollectionResultIn(string.Format("{0} 第{1}期 {2} 下期:{3}"
                            , ticiketName, phases, string.Join(" | ", bs), n));
                    }
                }
                catch (Exception)
                {
                }

                #endregion
                InsertingSYDYJ = false;
            }
        }

        /// <summary>
        /// 采集上海时时乐数据
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        private static void InsertSHSSL(DbContext db)
        {
            string ticiketName = "上海时时乐";
            if (!InsertingSHSSL)
            {
                InsertingSHSSL = true;
                #region 插入

                try
                {
                    int maxP = 23;
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://baidu.lecai.com/lottery/draw/view/201");
                    request.Method = "GET";
                    request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.0)";
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream stream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    string html = reader.ReadToEnd();
                    stream.Close();

                    Regex reg1 = new Regex("var latest_draw_phase = '(\\d+)';");
                    string phases = reg1.Match(html).Groups[1].Value;

                    bool hadLottery = db.Set<Lottery>().Any(x => x.Ticket.Name == ticiketName && x.Phases == phases);
                    if (!hadLottery)
                    {
                        List<string> bs = new List<string>();
                        Regex reg2 = new Regex("var latest_draw_result =(.*);");
                        string _str = reg2.Match(html).Groups[1].Value;
                        Regex reg3 = new Regex("\"(\\d+)\"");
                        Match m = reg3.Match(_str);
                        while (m.Success)
                        {
                            bs.Add(m.Groups[1].Value);
                            m = m.NextMatch();
                        }
                        int nextP = Convert.ToInt32(phases.Substring(8, 2)) + 1;
                        int tYear = Convert.ToInt32(phases.Substring(0, 4));
                        int tMonth = Convert.ToInt32(phases.Substring(4, 2));
                        int tDay = Convert.ToInt32(phases.Substring(6, 2));
                        DateTime tTime = new DateTime(tYear, tMonth, tDay);
                        if (nextP > maxP)
                        {
                            nextP = 1;
                            tTime = tTime.AddDays(1);
                        }
                        string n = tTime.Year.ToString().Substring(0, 4) + tTime.Month.ToString("00")
                            + tTime.Day.ToString("00") + nextP.ToString("00");

                        AddLottery(db, ticiketName, phases, bs.Take(3).ToList(), n);
                        CacheManager.SetCollectionResultIn(string.Format("{0} 第{1}期 {2} 下期:{3}"
                            , ticiketName, phases, string.Join(" | ", bs.Take(3)), n));
                    }
                }
                catch (Exception)
                {
                }

                #endregion
                InsertingSHSSL = false;
            }
        }

        /// <summary>
        /// 采集福彩3D数据
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        private static void InsertFC3D(DbContext db)
        {
            string ticiketName = "福彩3D";
            if (!InsertingFC3D)
            {
                InsertingFC3D = true;
                #region 插入

                try
                {
                    int maxP = 365;
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://caipiao.163.com/award/3d/");
                    request.Method = "GET";
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream stream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    string html = reader.ReadToEnd();
                    stream.Close();

                    Regex reg1 = new Regex("<span id=\"date_no\">(\\d+)</span>");
                    string phases = reg1.Match(html).Groups[1].Value;

                    bool hadLottery = db.Set<Lottery>().Any(x => x.Ticket.Name == ticiketName && x.Phases == phases);
                    if (!hadLottery)
                    {
                        List<string> bs = new List<string>();
                        Regex reg2 = new Regex("<span class=\"red_ball\">(\\d+)</span>");
                        Match m = reg2.Match(html);
                        while (m.Success)
                        {
                            bs.Add(m.Groups[1].Value);
                            m = m.NextMatch();
                        }
                        int nextP = Convert.ToInt32(phases.Substring(4, 3)) + 1;
                        int tYear = Convert.ToInt32(phases.Substring(0, 4));
                        DateTime tTime = new DateTime(tYear, 1, 1);
                        if (nextP > maxP)
                        {
                            nextP = 1;
                            tTime = tTime.AddYears(1);
                        }
                        string n = tTime.Year.ToString().Substring(0, 4) + nextP.ToString("000");

                        AddLottery(db, ticiketName, phases, bs.Take(3).ToList(), n);
                        CacheManager.SetCollectionResultIn(string.Format("{0} 第{1}期 {2} 下期:{3}"
                            , ticiketName, phases, string.Join(" | ", bs.Take(3)), n));
                    }
                }
                catch (Exception)
                {
                }

                #endregion
                InsertingFC3D = false;
            }
        }

        /// <summary>
        /// 采集排列三数据
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        private static void InsertPLS(DbContext db)
        {
            string ticiketName = "排列三";
            if (!InsertingPLS)
            {
                InsertingPLS = true;
                #region 插入

                try
                {
                    int maxP = 365;
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://caipiao.163.com/award/pl3/");
                    request.Method = "GET";
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream stream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    string html = reader.ReadToEnd();
                    stream.Close();

                    Regex reg1 = new Regex("<span id=\"date_no\">(\\d+)</span>");
                    string phases = reg1.Match(html).Groups[1].Value;

                    bool hadLottery = db.Set<Lottery>().Any(x => x.Ticket.Name == ticiketName && x.Phases == phases);
                    if (!hadLottery)
                    {
                        List<string> bs = new List<string>();
                        Regex reg2 = new Regex("<span class=\"red_ball\">(\\d+)</span>");
                        Match m = reg2.Match(html);
                        while (m.Success)
                        {
                            bs.Add(m.Groups[1].Value);
                            m = m.NextMatch();
                        }
                        int nextP = Convert.ToInt32(phases.Substring(2, 3)) + 1;
                        int tYear = Convert.ToInt32(phases.Substring(0, 2)) + 2000;
                        DateTime tTime = new DateTime(tYear, 1, 1);
                        if (nextP > maxP)
                        {
                            nextP = 1;
                            tTime = tTime.AddYears(1);
                        }
                        string n = tTime.Year.ToString().Substring(2, 2) + nextP.ToString("000");

                        AddLottery(db, ticiketName, phases, bs.Take(3).ToList(), n);
                        CacheManager.SetCollectionResultIn(string.Format("{0} 第{1}期 {2} 下期:{3}"
                            , ticiketName, phases, string.Join(" | ", bs.Take(3)), n));
                    }
                }
                catch (Exception)
                {
                }

                #endregion
                InsertingPLS = false;
            }
        }

        #region 私有变量

        private static bool InsertingCQSSC = false;

        private static bool InsertingJXSSC = false;

        private static bool InsertingXJSSC = false;

        private static bool InsertingGD11X5 = false;

        private static bool InsertingSYDYJ = false;

        private static bool InsertingSHSSL = false;

        private static bool InsertingFC3D = false;

        private static bool InsertingPLS = false;

        #endregion

        #region 私有方法

        /// <summary>
        /// 添加开奖记录
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        /// <param name="ticketName">目标彩票的名称</param>
        /// <param name="phases">期号</param>
        /// <param name="values">开奖号码</param>
        /// <param name="nextPhases">下一期期号</param>
        private static void AddLottery(DbContext db, string ticketName, string phases, List<string> values, string nextPhases)
        {
            List<string> tNames = new List<string> { "万位", "千位", "百位", "十位", "个位" };
            if (values.Count == 3)
            {
                tNames = new List<string> { "百位", "十位", "个位" };
            }
            int ticketId = db.Set<LotteryTicket>().Where(x => x.Name == ticketName)
                .Select(x => x.Id).FirstOrDefault();
            List<LotteryManager.Factory.IPackageForSeat> seats = new List<LotteryManager.Factory.IPackageForSeat>();
            for (int i = 0; i < values.Count; i++)
            {
                seats.Add(LotteryManager.Factory.CreatePackageForSeat(tNames[i], Convert.ToInt32(values[i]).ToString()));
            }
            ICreatePackage<Lottery> pfc = LotteryManager.Factory
                .CreatePackageForCreate(phases, LotterySources.系统采集, ticketId, seats);
            new LotteryManager(db).Create(pfc);
            new LotteryTicketManager(db).ChangeNextPhases(ticketId, nextPhases);
        }

        #endregion

        #endregion
    }
}
