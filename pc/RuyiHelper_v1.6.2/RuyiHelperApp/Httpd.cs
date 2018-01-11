﻿using RueHelper.model;
using RueHelper.util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RueHelper
{
    class Httpd
    {
        private static System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        
        private static string g_filenames = "";
        private static long tmFilename = 0;
        private static string g_filenamesTips = "";
        private static string g_practiceAnswer = "";
        private static string g_publicMsg = "";
        private static string g_handon = "";
        private static string g_PenStatus = "";
        private static string g_groupresult = "";
        private static bool running = true;
        private static AutoResetEvent _autoResetEvent_PublicMsg = new AutoResetEvent(false);
        private static AutoResetEvent _autoResetEvent_Practice = new AutoResetEvent(false);//做题信号
        private static AutoResetEvent _autoResetEvent_Vote = new AutoResetEvent(false);//做题信号
        private static AutoResetEvent _autoResetEvent_Competitive = new AutoResetEvent(false);//抢答信号
        private static AutoResetEvent _autoResetEvent_Handon = new AutoResetEvent(false);//抢答信号
        private static AutoResetEvent _autoResetEvent_GroupResult = new AutoResetEvent(false);
        private static AutoResetEvent _autoResetEvent_PenStatus = new AutoResetEvent(false);

        private static AutoResetEvent _autoResetEvent_MQ_Xiti = new AutoResetEvent(false);
        private static AutoResetEvent _autoResetEvent_MQ_Handon = new AutoResetEvent(false);
        private HttpListener listerner;
        private static RueSqlite m_db = new RueSqlite();
        private static Dictionary<Int32, string> bookoutlineMap = new Dictionary<Int32, string>();
        private static Queue MQ_Handon = new Queue();
        private static Queue MQ_Xiti = new Queue();
        private static string Key_P1 = "05438jf=4r9f895hr78c";
        private static string Key_P2 = "ckfr84-329487-2cjfy4";

        private static AnswerCard answer_card;//全局答题卡对象
        private static FormKaoQin fkq;  //考勤

        public Httpd()
        {
            //User u = m_db.getLastlogin();
            //Log.Info("last login user: id=" + u.id + ", name=" + u.name);
        }
        public void stop()
        {
            OnPublicMsg("EXIT");
            running = false;
            try
            {
                listerner.Stop();
            }catch(Exception e)
            {
                Log.Error("stop() exception. " + e.Message);
            }
            
        }

        private static void PushMQ_Handon(string str)
        {
            if (str.Length <= 0)
                return;
            System.Diagnostics.Debug.WriteLine(str);
            //1-7:58B3A099,H|1-1:58B3A099,H
            string str2 = "";
            {
                string[] szItem = str.Replace("-","").Split('|');

                ///通过随机数解决抢答老是显示同一个学生问题
                Random ran = new Random();
                int index = 0;
                string temp = "";
                int iT = 0;
                for (int j = 0; j < szItem.Length; j++)
                {

                    index = ran.Next(0, szItem.Length - 1);
                    iT = ran.Next(0,2);
                    if (index != j && iT!=0)
                    {
                        temp = szItem[j];
                        szItem[j] = szItem[index];
                        szItem[index] = temp;
                    }
                }

                for(int i=0; i<szItem.Length;i++)
                {
                    string[] szP = szItem[i].Split(':');
                    if(szP.Length > 1)
                    {
                        int nSeat = Util.toInt(szP[0]);

                        //StudentInfo si = Global.getUserInfoBySeat(nSeat);
                        //if (si == null)
                        //    continue;

                        str2 += (str2.Length>0?"|":"") + nSeat;
                    }
                }
            }
            // 设置返回给PAD的举手信息
            setHandon(str2);

            // 设置 小助手自己用的举手信息
            MQ_Handon.Enqueue(str2);
            _autoResetEvent_MQ_Handon.Set();
        }
        public static void setSeatfn(string data){
            // 设置返回给PAD的举手信息
            setHandon(data);
            // 设置 小助手自己用的举手信息
            MQ_Handon.Enqueue(data);
            _autoResetEvent_MQ_Handon.Set();
        }
        private static void PushMQ_Xiti(string data)
        {
            if (data.Length <= 0)
                return;

            string tm = DateTime.Now.ToString("HHmmss");
            string dataToPad = "";
            for (int i = 0; i < data.Split('|').Length - 1; i++)
            {
                int nSeat = Convert.ToInt16(data.Split('|')[i].Split(':')[0].ToString().Replace("-", ""));
                StudentInfo si = Global.getUserInfoBySeat(nSeat);
                if (si == null)
                    continue;

                string answer = data.Split('|')[i].Split(':')[1];
                //57A299E5,A;57A299E5,B;57A299E6,C;57A299E6,D;57A299E6,S
                string[] szKey = answer.Split(';');
                string getAnswer = "";
                SortedSet<string> keySet = new SortedSet<string>();
                for (int k = 0; k < szKey.Length; k++)
                {
                    string _answer = szKey[k].Split(',')[1];
                    if (_answer == "S")
                        continue;
                    if (!keySet.Contains(_answer))
                    {
                        keySet.Add(_answer);
                    }
                }
                foreach (string key in keySet)
                {
                    getAnswer += key;
                }
                if (getAnswer.Length == 0)
                    continue;

                string context = nSeat + ":" + getAnswer;
                Log.Info("Push_XitiResult: " + context);

                MQ_Xiti.Enqueue(tm + "#" + context);
                //MQ_Xiti.Enqueue(context);

                dataToPad += (dataToPad.Length>0?"|":"") + context;
            }
            setPracticeResult(dataToPad);
            _autoResetEvent_MQ_Xiti.Set();
        }
        public static string PopMQ_Xiti()
        {
            string data = "";
            try
            {
                bool re = _autoResetEvent_MQ_Xiti.WaitOne(300);
                if (re || MQ_Xiti.Count > 0)
                {
                    while(MQ_Xiti.Count>0)
                    {
                        Object obj = (string)MQ_Xiti.Dequeue();
                        string str = obj.ToString();
                        string time = "", buf = "";
                        if (str.IndexOf(":") > 5)
                        {
                            time = str.Substring(0, 6);
                            buf = str.Substring(7);
                        }
                        else
                        {
                            buf = str;
                        }


                        data += (data.Length > 0 ? "|" : "") + buf;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("PopMQ_Xiti exception. " + e.Message);
            }
            return data;
        }
        public static string PopMQ_Handon()
        {
            string ret = "";
            try
            {
                _autoResetEvent_MQ_Handon.WaitOne(500);
                if (MQ_Handon.Count > 0)
                {
                    ret = (string)MQ_Handon.Dequeue();
                }
            }
            catch (Exception e)
            {
                Log.Error("PopMQ_Handon exception. " + e.Message);
            }
            return ret;
        }

        public static string ClearMQ_Xiti()
        {
            g_practiceAnswer = "";

            //0-6 : 576A3133,A ; 576A3133,B ; 576A3133,C ; 576A3133,D ; 576A3134,S | 0-7 : 576A3133,A
            int i = 0;
            string dataTotal = "";
            while (MQ_Xiti.Count > 0)
            {
                Object obj = (string)MQ_Xiti.Dequeue();
                string str = obj.ToString();
                string time="", buf="";
                if (str.IndexOf(":") > 5)
                {
                    time = str.Substring(0, 6);
                    buf = str.Substring(7);
                }
                else
                {
                    buf = str;
                }
                string strNO = buf.Split(':')[0].Replace("-", "");
                string keys = buf.Split(':')[1];
                string _data = strNO + ":" + keys + ":" + time;
                dataTotal += (dataTotal.Length > 0 ? "|" : "") + _data;
                i++;
            }
            MQ_Xiti.Clear();
            return dataTotal;
        }
        public static string ClearMQ_Handon()
        {
            string ret = g_handon;
            g_handon = "";
            MQ_Handon.Clear();
            return ret;
        }
       
        ////////////////////////////////////////////////

        
        public void run()
        {
            try
            {
                running = true;
                listerner = new HttpListener();

                ArrayList iplist = Util.GetInternalIPList();
                listerner.AuthenticationSchemes = AuthenticationSchemes.Anonymous;//指定身份验证 Anonymous匿名访问
                iplist.Add("127.0.0.1");
                iplist.Add("localhost");
                foreach (string _ip in iplist)
                {
                    listerner.Prefixes.Add("http://" + _ip + ":8989/");
                }
                listerner.Start();
                Log.Info("服务器启动成功.......");

                int maxThreadNum, portThreadNum;

                //线程池
                int minThreadNum;
                ThreadPool.GetMaxThreads(out maxThreadNum, out portThreadNum);
                ThreadPool.GetMinThreads(out minThreadNum, out portThreadNum);
                Log.Info("最大线程数："+ maxThreadNum);
                Log.Info("最小空闲线程数："+ minThreadNum);

                //ThreadPool.QueueUserWorkItem(new WaitCallback(TaskProc1), x);

                Log.Info("\n\n等待客户连接中。。。。");
                while (running)
                {
                    //等待请求连接
                    //没有请求则GetContext处于阻塞状态
                    HttpListenerContext ctx = listerner.GetContext();
                    ThreadPool.QueueUserWorkItem(new WaitCallback(TaskProc), ctx);
                }
                Log.Info("\n\nTerminated.......");
            }
            catch (Exception e)
            {
                Log.Error("----------------------WARNING!!!------------------------\n"+e.Message);
                //Console.ReadKey();
            }
            finally
            {
                try
                {
                    listerner.Stop();
                }
                catch (Exception e2)
                {
                    Log.Error(e2.Message);
                }

                try
                {
                    listerner.Close();
                }
                catch (Exception e3)
                {
                    Log.Error(e3.Message);
                }
            }
        }

        static void TaskProc(object o)
        {
            HttpListenerContext ctx = (HttpListenerContext)o;
            HttpListenerResponse response = ctx.Response;
            HttpListenerRequest request = ctx.Request; 

            IPAddress clientAddr = ctx.Request.RemoteEndPoint.Address;
            string clientIp = clientAddr.ToString();

            ctx.Response.StatusCode = 200;//设置返回给客服端http状态代码
            string url = ctx.Request.Url.ToString();
            string rawUrl = url.Substring(url.LastIndexOf('/') + 1);

            if (string.Compare(rawUrl, "UploadVideo", true) == 0)
            {
                handleUploadVideo(request, response);
                return;
            }
            int pos = rawUrl.IndexOf('?');
            string method = "";
            string param = "";
            if (pos > 0)
            {
                method = rawUrl.Substring(0,pos);
                param = rawUrl.Substring(pos+1);
            }
            else
            {
                method = rawUrl;
            }

            if (method != "GetFilenames" && method != "GetHandon")
                Log.Debug("method=" + method + ", param=" + param);
            string action = ctx.Request.QueryString["action"];
            if (action == null)
                action = "";

            string ret = "";
            if(method.IndexOf("0comet") >= 0)//Comet! http://127.0.0.1:8989/comet
            {
                handleComet(ctx.Response);
            }else{
                ret = handleGetRequest(clientIp,method, action, param);
            }

            //使用Writer输出http响应代码
            
            try
            {
                StreamWriter writer = new StreamWriter(ctx.Response.OutputStream);
                writer.Write(ret);
                writer.Close();
            }
            catch (Exception e1)
            {
                Log.Error(e1.Message);
            }
            finally
            {
                ctx.Response.Close();
            }

        }
        private static void handleUploadVideo(HttpListenerRequest request, HttpListenerResponse response)
        {
            #region 上传图片
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".mp4";
            string srcDir = Application.StartupPath + "\\" + DateTime.Now.ToString("yyyyMMdd");
            string srcFile = Path.Combine(srcDir, fileName);
            string srcDir_Relative = DateTime.Now.ToString("yyyyMMdd");
            string srcFile_Relative = Path.Combine(srcDir_Relative, fileName);

            int LEN = 1024000;
            int LenWrite = 10240000;//10M
            bool bRead = false;
            byte[] buffer = new byte[LEN];
            byte[] bufferHead = new byte[LEN];
            string _name = "";
            byte[] bufferOnce = new byte[LenWrite];
            int nBufferOnceCount = 0;
            #region recv data from stream, and write to file
            try
            {
                BinaryReader br = new BinaryReader(request.InputStream);
                FileStream fs = new FileStream(srcFile, FileMode.Create, FileAccess.Write, FileShare.None);
                BinaryWriter bw = new BinaryWriter(fs);
                {
                    int nRead = 0;
                    while ((nRead = br.Read(buffer, 0, (int)LEN)) != 0)
                    {
                        //bw.Write(bytes, 0, nRead);
                        if (bRead == false)
                        {
                            Array.ConstrainedCopy(buffer, 0, bufferHead, 0, nRead);
                            bRead = true;

                            int pos = Util.bytesIndexOf(buffer, System.Text.Encoding.ASCII.GetBytes("CALLBACK&"));
                            int len = 9;
                            if (pos == -1)
                            {
                                pos = Util.bytesIndexOf(buffer, System.Text.Encoding.ASCII.GetBytes("image/jpeg\r\n\r\n"));
                                len = 14;
                            }

                            nBufferOnceCount = (nRead - pos - len);
                            Array.ConstrainedCopy(buffer, pos + len, bufferOnce, 0, nBufferOnceCount);
                            
                            string s2 = System.Text.Encoding.UTF8.GetString(bufferHead);
                            NameValueCollection nvc = Util.ParseQueryString(s2);
                            _name = nvc["name"];
                        }
                        else
                        {
                            if (nBufferOnceCount + nRead > LenWrite)//10M，写文件
                            {
                                bw.Write(bufferOnce, 0, nBufferOnceCount);
                                Array.Clear(bufferOnce, 0, nBufferOnceCount);
                                nBufferOnceCount = 0;
                            }
                            Array.ConstrainedCopy(buffer, 0, bufferOnce, nBufferOnceCount, nRead);
                            nBufferOnceCount += nRead;
                        }
                        Array.Clear(buffer, 0, nRead);
                    }

                    bw.Write(bufferOnce, 0, nBufferOnceCount);
                    bw.Close();
                    request.InputStream.Close();
                    Log.Info("UploadVideo()...read stream over... dstfile=" + srcFile);
                    EService.selectFile(_name, srcFile);
                }
            }catch(Exception e){
                Log.Error(e.Message);
            }

            #endregion

            response.ContentType = "text/html;charset=utf-8";

            #region ffmpeg compressing-thread
            try
            {
                string ffmpegExe = Application.StartupPath + "\\ffmpeg.exe ";
                if (File.Exists(ffmpegExe))
                {
                    Thread BackThread = new Thread(delegate()
                    {
                        //var pStartInfo = new ProcessStartInfo
                        //{
                        //    WorkingDirectory = Application.StartupPath,
                        //    FileName = @"ffmpeg.exe",
                        //    Arguments = Argu,
                        //    CreateNoWindow  = true
                        //};
                        //Process p = Process.Start(pStartInfo);

                        string TargetFile = Path.Combine(srcDir_Relative, _name);
                        Log.Info("UploadVideo() recv mp4 ok. ffmpeg now..." + srcFile_Relative + ", TargetFile=" + TargetFile);

                        //string Argu = @"-i " + srcFile_Relative + " -ar 22050 -b 700k -s 800x480 " + TargetFile;
                        string Argu = @"-i " + srcFile_Relative + " -ar 22050 -b 300 -s 800x480 -vcodec mpeg4 -ab 32 -acodec aac -strict experimental -r 23 " + TargetFile;
                        //string Argu = @"-i " + srcFile_Relative + " -ar 22050 -qscale 6 -s 800x450 -vcodec mpeg4 -ab 32 -acodec aac -strict experimental -r 23 " + TargetFile;
                        StringBuilder sbExe = new StringBuilder(255);
                        Util.GetShortPathName(ffmpegExe, sbExe, 255);
                        Log.Info(sbExe.ToString() + Argu);

                        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(sbExe.ToString(), Argu);
                        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        Process p2 = System.Diagnostics.Process.Start(startInfo);
                        while (!p2.HasExited)
                        {
                            Thread.Sleep(500);
                        }

                        if (File.Exists(TargetFile))
                        {
                            string md5 = Util.GetFileMD5(TargetFile);
                            Log.Info("UploadVideo() convert mp4 ok. upload now...");
                            Common.uploadPicture(TargetFile);//upload video
                            Common.uploadRecordEvent(Path.GetFileName(TargetFile), md5);
                        }

                    });
                    BackThread.IsBackground = true;
                    BackThread.Start();
                }
            }
            catch(Exception e)
            {
                Log.Error(e.Message);
            }
            #endregion


            Response resp = new Response(0, "UploadVideo OK", fileName);

            using (StreamWriter writer = new StreamWriter(response.OutputStream, Encoding.UTF8))
            {
                writer.WriteLine(resp.toJson());
                writer.Close();
            }
            response.Close();
            return;
            #endregion  
        }
        private static string handleGetRequest(string clientIp,string method,string action,string param)
        {
            string callback = "";
            string[] szItem = param.Split('&');

            int _courseid = 0;
            int _lessonid = 0;

            Dictionary<String, String> pList = new Dictionary<String, String>();
            for(int i=0; i< szItem.Length; i++)
            {
                string pair = szItem[i];
                string[] szPair = pair.Split('=');
                if(szPair.Length==2)
                {
                    string key = szPair[0];
                    string val = szPair[1];
                    if(!pList.ContainsKey(key))
                        pList.Add(key, val);
                    if (key == "callback")
                        callback = val;
                }
            }

            try
            {
                if (pList.ContainsKey("courseid"))
                    _courseid = Util.toInt(pList["courseid"]);//防止出现undefined
                if (pList.ContainsKey("lessonid"))
                    _lessonid = Util.toInt(pList["lessonid"]);


                Global.checkLesson(_courseid, _lessonid);
            }
            catch (Exception ee)
            {

            }


            

            string ret = "";
            if (method.IndexOf("GetFiles") >= 0)//Comet! http://127.0.0.1:8989/GetFiles
            {
                string data = handleGetFiles(pList);
                string tips = popFilelistTips();
                if (tips.Length > 0)
                    return getResponse(callback, 0, "获取文件名称", data);
                else
                    return getResponse(callback, 1, "获取文件名称", data);
            }
            else if (method.IndexOf("PracticeResult.get") >= 0)//Comet! http://127.0.0.1:8989/PracticeAnswer.get
            {
                Log.Debug("PAD_PracticeResult.get ...");
                string data = handleGetPracticeResult();
                return getResponse(callback, "获取做题结果", data);
            }
            else if (method.IndexOf("getVote") >= 0)//Comet! http://127.0.0.1:8989/PracticeAnswer.get
            {
                Log.Debug("getVote ...");
                string data = handleGetVote();
                return getResponse(callback, "getVote", data);
            }
            else if (method.IndexOf("GetUpdate") >= 0)//Comet！
            {
                string data = handleGetUpdate(pList);
                return data;
            }
            else if (method.IndexOf("GetPenStatus") >= 0)//Comet！
            {
                string data = handleGetPenStatus();
                return getResponse(callback, "获取Pen信息", data);
            }
            else if (method.IndexOf("GetHandon") >= 0)//Comet！
            {
                string data = handleGetHandon();
                return getResponse(callback, "获取举手信息", data);
            }
            else if (action.IndexOf("getAwardType") >= 0)//Comet！
            {
                string data = handleGetAwardlist(pList);
                return data;
            }
            else if (action.IndexOf("login") >= 0)//本地登录
            {
                //FormDraw.ClearRecord();
                string data = handleLogin(pList, clientIp);
                //if (answer_card == null)
                //{
                //    answer_card = new AnswerCard();
                //    answer_card.Message();
                //}
                //else 
                //{
                //    answer_card.Message();
                //}
                return data;
            }
            else if (action.IndexOf("AutoChangeClass") >= 0)//
            {
                //FormDraw.ClearRecord();
                string data = handleAutoChangeClass(pList, clientIp);
                return data;
            }
            else if (action.IndexOf("students.list") >= 0)//获取学生列表
            {
                string data = handleGetStudentlist(pList);
                return data;
            }
            else if (action.IndexOf("getKQlist") >= 0)//获取打卡学生列表
            {
                string data = handleKQlist(pList);
                return data;
            }
            else if (action.IndexOf("getStudentlist") >= 0)//获取学生列表
            {
                string data = handleGetStudentlistGroupByPY(pList);
                return data;
            }
            else if (action.IndexOf("bookoutline.get") >= 0)//获取大纲
            {
                string data = handleGetBookoutline(pList);
                return data;
            }
            else if (action.IndexOf("sound.set") >= 0)// http://localhost:8989/EduApi/user.do?action=sound.get&callback=cbfunction&handon=1&callname=1&reward=0
            {
                string data = handleSetSound(pList);
                return data;
            }
            else if (action.IndexOf("sound.get") >= 0)// http://localhost:8989/EduApi/user.do?action=sound.get
            {
                string data = handleGetSound(pList);
                return data;
            }
            else if (action.IndexOf("seat.set") >= 0)//
            {
                string data = handleSetSeat(pList);// http://localhost:8989/EduApi/user.do?action=seat.set&uid=3999
                return data;
            }
            else if (action.IndexOf("xitiFromHX.list") >= 0)//获取习题列表
            {
                string data = handleGetXiti(pList);
                return data;
            }
            else if (method.IndexOf("test") >= 0)
            {
                ret = "Hi!";
            }
            else if (method.IndexOf("HD_Handon") >= 0)//接收机访问小助手
            {
                string data = pList["data"];
                if (data.Length > 1 && (data.Length - 1) == data.LastIndexOf('|'))
                    data = data.Substring(0, data.Length-1);
                Log.Debug("HD_Handon: " + data);
                PushMQ_Handon(data);
                ret = "Hi!";
            }
            else if (method.IndexOf("HD_Xiti") >= 0)//接收机访问小助手
            {
                string data = pList["data"];
                Log.Debug("HD_Xiti: "+ data);
                PushMQ_Xiti(data);
                ret = "Hi!";
            }
            else if (method.IndexOf("GetCourses") >= 0)//method
            {
                string data = handleGetCourses();
                return getResponse(callback, "科目列表", data);
            }
            else if (method.IndexOf("GetRoomCourses") >= 0)//method
            {
                string data = handleGetRoomCourses();
                return getResponse(callback, "科目列表", data);
            }
            else if (method.IndexOf("getGroup") >= 0)//http://localhost:8989/getGrouplist?callback=a
            {
                string data = handleGetGroup();
                return getResponse(callback, "分组列表", data);
            }
            else if (method.IndexOf("setGroup") >= 0)//http://localhost:8989/setGroup?groupname=B&uids=3,4,5
            {
                string data = handleSetGroup(pList);
                return getResponse(callback, "更新分组", data);
            }
            else if (method.IndexOf("createGroup") >= 0)//http://localhost:8989/createGroup?data=
            {
                string data = handleCreateGroup(pList);
                return getResponse(callback, "更新分组", data);
            }
            else if (method.IndexOf("GetGroupResult") >= 0)//Comet！
            {
                string data = handleGetGroupResult();
                return getResponse(callback, "获取结果信息", data);
            }
            else if (method.IndexOf("startSetSeat") >= 0)//Comet！
            {

                string data = startSetSeat();
                return getResponse(callback, "启动设置座位", data);
            }
            else if (method.IndexOf("closeSetSeat") >= 0)//Comet！
            {
                string data = closeSetSeat();
                return getResponse(callback, "关闭设置座位", data);
            }
            else if (method.IndexOf("GetWeixinPic") >= 0)
            {
                string data = handleGetWeixinPic();
                return getResponse(callback, "获取微信端拍照图片", data);
            }
            return ret;
        }

        //获取微端拍照图片
        private static string handleGetWeixinPic()
        {
            int teacherID = Global.getTeacherID();
            //teacherID = 4836;
            string dayfrom = DateTime.Now.AddDays(-3).ToString("yyyyMMdd");
            //dayfrom = "20170817";
            string dayto = DateTime.Now.ToString("yyyyMMdd");
            //dayto = "20170817";
            //根据教师id去后台获取近三日数据
            string strImgData = Common.getTheUploadPhotos(teacherID, dayfrom, dayto);
            //Console.WriteLine(strImgData);
            if (strImgData.Length > 0)
            {
                System.Web.Script.Serialization.JavaScriptSerializer json = new System.Web.Script.Serialization.JavaScriptSerializer();
                JObject jo = (JObject)JsonConvert.DeserializeObject(strImgData);
                // Console.WriteLine(jo);
                foreach (var item in jo)
                {
                    for (var i = 0; i < item.Value.Count(); i++)
                    {
                        for (var j = 0; j < item.Value[i]["photoMapList"].Count(); j++)
                        {
                            // Console.WriteLine(item.Value[i]["photoMapList"][j]);
                            //Console.WriteLine(item.Value[i]["photoMapList"][j]["image"]); //获取到图片
                            //Console.WriteLine(item.Key);
                            string path = "http://api.skyeducation.cn/EduApi/upload/";
                            string strDate = item.Key;
                            strDate = strDate.Insert(4, "/");
                            strDate = strDate.Insert(7, "/");
                            string picName = item.Value[i]["photoMapList"][j]["image"].ToString();
                            string tag = picName.Substring(picName.Length - 4);
                            if (item.Value[i]["photoMapList"][j]["type"].ToString() == "0")
                            {
                                picName = item.Value[i]["photoMapList"][j]["imageMd5"] + tag;
                            }
                            path += strDate + @"/" + teacherID + @"/" + picName;
                            //获取到路径后下载
                            string imgNamess = Common.HttpDownload(path, item.Value[i]["photoMapList"][j]["image"].ToString());
                            Console.WriteLine(path);
                            //Console.WriteLine(item.Value[i]["photoMapList"][j]["image"]);
                            //Console.WriteLine(path);
                        }
                    }
                }
                // Console.WriteLine(jo);
            }
            return strImgData;
        }


        private static string startSetSeat() 
        {  
            Global.setSeatBtn = true;
            AnswerCard.RaiseStart();   //启动答题卡举手           
            return "启动设置座位成功";
        }
        private static string closeSetSeat()
        {
            Global.setSeatBtn = false;
            AnswerCard.AnswerStop();   //结束答题卡举手
            return "关闭设置座位成功";
        }

        /*
        获取分组列表 : 8989/getGrouplist
        更新单个分组 : 8989/setGroup?callback=...&groupname=A&uids=3505,3506,3507,3508
         */
        private static string handleGetCourses()
        {
            Log.Info("GetCourses()");
            string ret = "{\"schoolid\":\"P1\",\"schoolname\":\"P2\",\"classid\":\"P3\",\"classname\":\"P4\",\"courses\":P5}";
            ret = ret.Replace("P1", Global.getSchoolID() + "");
            ret = ret.Replace("P2", Global.getSchoolname());
            ret = ret.Replace("P3", Global.getClassID() + "");
            ret = ret.Replace("P4", Global.getClassname());
            string p5 = "";
            HashSet<Int32> courseIdSet = new HashSet<Int32>();
            if (Global.g_TeacherArray != null)
            {
                foreach (User u in Global.g_TeacherArray)
                {
                    //string buf = u.courseid + ":" + u.coursename + ":" + u.id + ":" + u.account+":"+u.name;
                    string buf = "{\"courseid\":\"P1\",\"coursename\":\"P2\",\"uid\":\"P3\",\"acount\":\"P4\",\"name\":\"P5\"}";
                    if (courseIdSet.Contains(u.courseid) || u.courseid==0)
                    {
                        continue;
                    }
                    else
                    {
                        courseIdSet.Add(u.courseid);
                    }
                    buf = buf.Replace("P1", u.courseid + "");
                    buf = buf.Replace("P2", u.coursename);
                    buf = buf.Replace("P3", u.id + "");
                    buf = buf.Replace("P4", u.account);
                    buf = buf.Replace("P5", u.name);
                    p5 += (p5.Length > 0 ? ",\r\n" : "") + buf;
                }
            }
            if(p5.Length>0)
                ret = ret.Replace("P5", "["+p5+"]");
            else
                ret = ret.Replace("P5", "\"\"");
            string outStr = ret;

            ////声明字符集   
            //System.Text.Encoding utf8, gb2312;
            ////gb2312   
            //gb2312 = System.Text.Encoding.GetEncoding("gb2312");
            ////utf8   
            //utf8 = System.Text.Encoding.GetEncoding("utf-8");
            //for (int i = 0; i < ret.Length; i++)
            //{
            //    if (Regex.IsMatch(ret[i].ToString(), @"[\u4e00-\u9fa5]"))
            //    {
            //        outStr += "\\u" + ((int)ret[i]).ToString("x");
            //    }
            //    else
            //    {
            //        outStr += ret[i];
            //    }
            //}

            Log.Debug(ret);
            return outStr;
        }

        private static string handleGetRoomCourses()
        {
            if (Global.g_roommsg.Length > 10) 
            {          
                JObject jo = (JObject)JsonConvert.DeserializeObject(Global.g_roommsg);
                Global.roomname = jo["roomname"].ToString();
                string classStr = jo["courses"][0].ToString();
                JObject ob = (JObject)JsonConvert.DeserializeObject(classStr);
                Global.classname = ob["classname"].ToString();
                Global.teachername = ob["name"].ToString();
            }
            return Global.g_roommsg;
        }


        //获取分组
        private static string handleGetGroup()
        {
            string ret = Common.getGroup();
            Log.Info(ret);
            return ret;
        }
        
        //更新分组
        private static string handleSetGroup(Dictionary<String, String> pList)
        {
            //http://localhost:8989/setGroup?data=
            string data = "";
            if (pList.ContainsKey("data"))
                data = pList["data"];
            Log.Info("setGroup() data="+data);
            if (Global.m_grouplist==null)
            {
                Common.getGroup();
            }
            if (Global.m_grouplist != null)
            {
                //A:3919,3926,3945,4218,4224,4232,3933,3948,3918,3954,4236,4233,4225,4219,3931,3920,3946,3953,3950,4226,3936,3935;B:4234,3921,3929,3930,3938,4214,4221,4228,4235,3952,3944,3928,4229,4222,4215,3942;C:3923,4217,4223,4231,3937,3939,3924
                Common.setGroup(Global.m_grouplist.id, data);
                Global.updateGroup(data);
            }
            
            return "success";
        }


        //更新分组
        private static string handleCreateGroup(Dictionary<String, String> pList)
        {
            //http://localhost:8989/createGroup?data=
            string data = "";
            if (pList.ContainsKey("data"))
                data = pList["data"];
            Log.Info("createGroup() data=" + data);

            string ret = Common.createGroup(data);
            Global.setGroup(ret);
            return handleGetGroup();
        }
        private static string handleUsbFilenames()
        {
            int i = 0;
            long tmNow = Environment.TickCount;
            long tmDiff = tmNow - tmFilename;
            ////超过3秒就重新获取
            //if (tmDiff > 1000 * 3)
            //{
            //    g_filenames = Form12.g_filenames;
            //    tmFilename = Environment.TickCount;
            //    return g_filenames;
            //}

            //不到3秒时，没有变化就sleep 500ms
            while (i++ < 10)
            {
                if (g_filenames != Form12.g_filenames)
                {
                    g_filenames = Form12.g_filenames;
                    tmFilename = Environment.TickCount;
                    break;
                }
                else
                {
                    Thread.Sleep(500);
                }
            }
            return g_filenames;
        }
        private static string handleGetFiles(Dictionary<String, String> pList)
        {
            string callback = getParam(pList,"callback");

            string str = handleUsbFilenames();
            if (str == null)
                str = "";
            string[] szFile = str.Split('|');
            string data = "{\"filelist\":[P1],\"panelshow\":"+ Global.panelshow +"}";
            string bufs = "";
            string dir = Application.StartupPath+"\\"+DateTime.Now.ToString("yyyyMMdd")+"\\";
            for(int i=0; i<szFile.Length; i++)
            {
                if(szFile[i].Length > 0)
                {
                    string name = szFile[i];
                    string ext = name.Substring(name.LastIndexOf('.')+1).ToLower();
                    string nameWithoutExt = name.Substring(0, name.LastIndexOf('.'));
                    int thumbimg = 0;
                    string pcIP = Form1.GetRandomIP();
                    pcIP = pcIP.Substring(0, pcIP.Length - 1);
                    string url = "http://" + pcIP + ":8986/Download/" + szFile[i] + "_1.jpg";
                    string _path = dir + name+"_1.jpg";
                    if (ext.IndexOf("ppt") >= 0 || ext.IndexOf("pdf") >= 0 || ext.IndexOf("doc") >= 0 || ext.IndexOf("wmv") >= 0 || ext.IndexOf("mp4") >= 0 || ext.IndexOf("mov") >= 0)
                    {
                        if(File.Exists(_path))
                            thumbimg = 1;
                    }
                    else if (ext.IndexOf("mp3") >= 0 || ext.IndexOf("wma") >= 0)
                    {
                        thumbimg = 1;
                        url = "http://" + pcIP + ":8986/Download/music.jpg";
                    }
                    else if (ext.IndexOf("jpg") >= 0 || ext.IndexOf("jpeg") >= 0 || ext.IndexOf("png") >= 0)
                    {
                        thumbimg = 1;
                        url = "http://" + pcIP + ":8986/Download/" + name;
                    }
                    else if (ext.IndexOf("swf") >= 0)
                    {
                        thumbimg = 1;
                        url = "http://" + pcIP + ":8986/Download/flash.png";
                    }

                    string buf = "";
                    ext = ext.Substring(0, ext.Length > 3 ? 3 : ext.Length);

                    if(thumbimg==0)
                        buf = "{\"name\":\"" + name + "\",\"ext\":\"" + ext.ToUpper() + "\",\"ThumbImg\":\"0\"}";
                    else
                        buf = "{\"name\":\"" + name + "\",\"ext\":\"" + ext.ToUpper() + "\",\"ThumbImg\":\"1\", \"url\":\"" + url + "\"}";

                    bufs += (bufs.Length > 0 ? "," : "") + buf;
                }

            }

            return data.Replace("P1", bufs);
        }

        public static void setPracticeResult(string result)
        {
            string[] szItem = result.Split('|');
            result = "";
            for (int i = 0; i < szItem.Length; i++)
            {
                string item = szItem[i];
                string[] tempArr = item.Split(':');
                item = Global.getSeatByCardid(tempArr[0]) + ":" + tempArr[1];
                if (result == "")
                {
                    result = item;
                }
                else
                {
                    result += "|" + item;
                }
            }

            if (result.Length == 0)
                return;

            if (g_practiceAnswer.Length > 0)
                g_practiceAnswer += "|" + result;//如果之前没有获取，则累加
            else
                g_practiceAnswer = result;
            _autoResetEvent_Practice.Set(); 
        }
        public static void pushFilelistTips(string result)
        {
            g_filenamesTips = result;
        }
        public static string popFilelistTips()
        {
            string ret = g_filenamesTips;
            g_filenamesTips = "";
            return ret;
        }
        public static void clearPracticeResult()
        {
            g_groupresult = "";
            g_practiceAnswer = "";
        }
        private static string handleGetPracticeResult()
        {
            string ret = "";
            while (true)
            {
                //等30秒，30秒没有信号，显示无结果
                //有信号，则显示更新结果
                bool re = _autoResetEvent_Practice.WaitOne(5000);
                if (re || g_practiceAnswer.Length > 0)
                {
                    ret = g_practiceAnswer;
                    Log.Info("getPracticeResult:" + ret);
                    g_practiceAnswer = "";

                    if(ret.Length > 0)
                    {
                        string[] sz = ret.Split('|');
                        string strOut = "";
                        foreach (string str in sz)
                        {
                            string[] szPair = str.Split(':');
                            int seat = Util.toInt(szPair[0]);
                            string answer = szPair[1];
                            StudentInfo si = Global.getUserInfoBySeat(seat);
                            if (si == null)
                                continue;
                            strOut += (strOut.Length > 0 ? "|" : "") + (seat + ":" + answer + ":" + si.pinyin);
                        }
                        ret = strOut;
                    }
                    break;
                }
                else
                {
                    Log.Debug("getPracticeResult...no result, timeout");
                    break;
                }
            }
            return ret;
        }
        private static string handleComet(HttpListenerResponse response)
        {
            string ret = "";
            try
            {
                while (true)
                {
                    //等30秒，30秒没有信号，显示无结果
                    //有信号，则显示更新结果
                    //bool re = _autoResetEvent_PublicMsg.WaitOne(3000);
                    //if (re)
                    {
                        //if ("EXIT" == g_publicMsg)
                        //{
                        //    break;
                        //}

                        Thread.Sleep(1000);

                        if (g_publicMsg.Length > 0)
                            ret = g_publicMsg;
                        else
                            ret = "heartbeat";

                        g_publicMsg = "";
                        byte[] data=System.Text.Encoding.Default.GetBytes ( ret );
                        response.Headers.Add("access-control-allow-origin", "*");
                        response.Headers.Add("Content-Type", "text/event-stream");
                        //response.OutputStream.SetLength(data.Length);
                        response.OutputStream.Write(data, 0, data.Length);
                        response.OutputStream.Flush();

                        Log.Info("CometMsg:" + ret);
                    }
                }
            }
            catch (Exception e1)
            {
                Log.Error(e1.Message);
            }
            return ret;
        }
        public static void OnPublicMsg(string msg)
        {
            g_publicMsg = msg;
            _autoResetEvent_PublicMsg.Set();
        }
        public static void NotifyVoteEvent()
        {
            _autoResetEvent_Vote.Set();
        }
        private static string handleGetVote()
        {
            _autoResetEvent_Vote.WaitOne(5000);
            return FormVote.RESULT;
        }

        //--------------------------------------------------------------------
        // 设置返回给PAD的举手信息
        //--------------------------------------------------------------------
        public static void setHandon(string result)
        {
            //if(result.Length > 0)
            //{
            //    if (g_handon.Length > 0)
            //        g_handon += "|" + result;//如果之前没有获取，则累加
            //    else
            //        g_handon = result;

            //    _autoResetEvent_Handon.Set();
            //}
//将物理id转成座位号返回给Pad
            string[] szItem = result.Split('|');
            result = "";
            for (int i = 0; i < szItem.Length; i++)
            {
                string item = szItem[i];
                item = Global.getSeatByCardid(item);
                if (result == "")
                {
                    result = item;
                }
                else
                {
                    result += "|" + item;
                }
            }

            if(result.Length > 0)
            {
                if (g_handon.Length > 0)
                    g_handon += "|" + result;//如果之前没有获取，则累加
                else
                    g_handon = result;

                _autoResetEvent_Handon.Set();
            }              
        }
        public static void cancelHandon()
        {
            g_handon = "";
            _autoResetEvent_Handon.Set();
        }
        public static void setGroupResult(string result)
        {
            if (result.Length > 0)
            {
                if (g_groupresult.Length > 0)
                    g_groupresult += "|" + result;//如果之前没有获取，则累加
                else
                    g_groupresult = result;
            }
            _autoResetEvent_GroupResult.Set();
        }
        /// <summary>
        /// 获取升级信息
        /// </summary>
        /// <returns></returns>
        private static string handleGetUpdate(Dictionary<String, String> pList)
        {
            string versionC = getParam(pList, "version");
            string callback = getParam(pList, "callback");

            int code = 0;
            UpdateItem ui = Global.getRyktUpdateInfo();
            string data = "";
            if(ui!=null)
            {
                string versionS = ui.version;
                if (versionS.Length > 0)
                {
                    string[] szVerS = versionS.Split('.');
                    string[] szVerC = versionC.Split('.');
                    try
                    {
                        for (int i = 0; i < szVerS.Length; i++)
                        {
                            int nS = Util.toInt(szVerS[i]);
                            int nC = Util.toInt(szVerC[i]);
                            if (nS > nC)
                            {
                                data = ui.toJson();
                                code = 1;
                                break;
                            }
                        }
                    }
                    catch (Exception e1)
                    {
                        Log.Error("getUpdate exception. versionC=" + versionC + ", versionS=" + versionS);
                    }
                }
            }
            else
            {
                Log.Error("getRyktUpdateInfo return null");
            }

            string ret = getResponse(callback, code, "获取升级信息成功", data);
            Log.Debug("Pad.getUpdate..... ret="+ret);
            return data;
        }

        private static string handleGetPenStatus()
        {
            string ret = "";
            while (true)
            {
                bool re = _autoResetEvent_PenStatus.WaitOne(1000);
               // if (re)
                {
                    int[] szStatus = FormDraw.getStatus();
                    for (int i = 1; i <= szStatus.Length; i++ )
                    {
                        string item = i + ":" + szStatus[i-1];
                        ret += (ret.Length > 0 ? "|" : "") + item;
                    }
                    //ret = g_PenStatus;
                    break;
                }
            }
            //Log.Info(ret);
            return ret;
        }
        /// <summary>
        /// Comet_PAD获取举手信息
        /// </summary>
        /// <returns></returns>
        private static string handleGetHandon()
        {
            string ret = "";
            while (true)
            {
                bool re = _autoResetEvent_Handon.WaitOne(3000);
                if (re || g_handon.Length > 0)
                {
                    ret = g_handon;
                    g_handon = "";
                    break;
                }
            }
            if(ret.Length > 0 )
            {
                string[] sz = ret.Split('|');
                string names = "" ;
                string strOut = "";
                foreach(string str in sz)
                {
                    int seat = Util.toInt(str);
                    StudentInfo si = Global.getUserInfoBySeat(seat);
                    if (si == null)
                        continue;
                    strOut += (strOut.Length > 0 ? "|" : "") + (seat + ":" + si.pinyin + ":" + si.Name + ":" + si.ID);
                }
                Log.Info("Pad.getHandon. ret=" + ret + ", names=" + names);
                
                return strOut;
            }
            else
            {
                return "";
            }
        }
        
        /// <summary>
        /// Comet_PAD获取举手信息
        /// </summary>
        /// <returns></returns>
        private static string handleGetGroupResult()
        {
            string ret = "";
            Log.Debug("Pad.getGroupHandon.");

            while (true)
            {
                bool re = _autoResetEvent_GroupResult.WaitOne(2000);
                if (re || g_groupresult.Length > 0)
                {
                    ret = g_groupresult;
                    g_groupresult = "";
                    break;
                }
                else
                {
                    //Log.Info("getGroupHandon...no result, timeout");
                    break;
                }
            }
            Log.Debug("Pad.getGroupHandon. ret=" + ret);
            return ret;
        }

        private static string handleSetSeat(Dictionary<String, String> pList)
        {
            string ret = "";
            string uid = getParam(pList, "uid").ToLower();
            string callback = getParam(pList, "callback");

            Httpd.ClearMQ_Handon();

            string data0 =PopMQ_Handon();
            string data = "";
            string ret2 = "";
            if (data0.Length > 0)
            {
                //17154100#14:5763A98C,H|19:5763A98C,H|4:5763A98C,H|1:5763A98C,H|8:5763A98C,H|9:5763A98C,H|28:5763A98C,H"
                int pos = data0.IndexOf("#")+1;
                data0 = data0.Substring(pos > 0 ? pos : 0);
                string[] szItem = data0.Split('|');
                string seat = szItem[0].Split(':')[0];
                data = "{\"uid\":\"P1\",\"seat\":\"P2\"}".Replace("P1", uid + "").Replace("P2", seat.Replace("-",""));

                if (seat.Length == 1)
                    seat = "0" + seat;
                string seat2 = seat.Insert(1, "-");
                Log.Info("SetSeat: uid=" + uid + ", seat=" + seat2);

                //upload to Server
                Common.setSeat(uid + ":" + seat2);

                //TODO: upload local data
                data = Global.setSeat(Util.toInt(uid), seat2);
            }
            ret = getResponse(callback, 0, "SetSeat", data);
            return ret;
        }

        //{"ret":"0","msg":"登录成功！","data":{"id":"3842","type":"1","phone":"201604081438","nick":"","account":"wangqi","name":"王奇","pinyin":"","imageurl":"","age":"0","gender":"1","province":"北京","city":"北京","district":"东城区","address":"111111111111111","studentno":"","schoolid":"33","courseid":"0","teachyears":"0","intro":"1111111111111","classlist":[{"id":"1634","schoolid":"33","grade":"3","name":"三(1)班","seatxy":"1,1","orderid":"1","roomid":"1356","roomname":"三年级一班","building":"","hdid":"","hdip":"1","hdport":"80","appip":"172.18.2.105","courseid":"12","coursename":"语文"},{"id":"1638","schoolid":"33","grade":"1","name":"一(1)班","seatxy":"1,1","orderid":"1","roomid":"1360","roomname":"一年级一班","building":"","hdid":"192.168.253.201","hdip":"1","hdport":"80","appip":"172.18.2.104","courseid":"12","coursename":"语文"}],"schoolname":"测试小学","schoolcode":"3477786fefd57d7403631c839b6787d4"},"count":"1"}
        private static string handleLogin(Dictionary<String, String> pList,string clientIp)
        {
            if(pList==null)
            {
                return getResponse("", 0, "登录失败!", "");
            }
            string ret = "";
            string callback = getParam(pList, "callback");
            string user = getParam(pList, "user");
            string pwd = getParam(pList, "pwd");
            int courseid = Util.toInt(getParam(pList, "courseid"));

            if (user.Length >0 && pwd.Length > 0)
            {
                User u = m_db.getTeacher(user);
                if (u == null)
                {
                    ret = getResponse(callback, 0, "登录失败，用户不存在。", "");
                }
                else if (pwd == u.pwd || pwd == GetMD5(u.pwd))
                {
                    ////////////////////////////////////////////////
                    // 获取lesson，异步
                    ////////////////////////////////////////////////
                    Common.getLessonAsync();
                    //Common.getLesson();


                    //u.classname = Global.getClassname();
                    u.classid = Global.getClassID();
                    u.lessonid = Global.getLessonID();
                    u.pwd = "";
                    string data = "";
                    data = u.toJson();
                    ret = getResponse(callback, 1, "登录成功", data);

                    ////////////////////////////////////////////////
                    // CourseID
                    ////////////////////////////////////////////////
                    m_db.setLastlogin(u);


                    Global.setCourseID(courseid);
                    Global.setCourseName(u.coursename);
                    Global.setTeacherID(u.id);
                    Global.setPadIP(clientIp);


                    
                }
                else
                {
                    ret = getResponse(callback, 0, "登录失败,密码不正确", "");
                }
            }
            else
            {
                if (Global.g_TeacherArray != null && Global.g_TeacherArray.Length > 0)
                {
                    foreach(User u in Global.g_TeacherArray)
                    {
                        if(courseid == u.courseid)
                        {
                            //u.classname = Global.getClassname();
                            u.classid = Global.getClassID();
                            u.pwd = "";

                            ////////////////////////////////////////////////
                            // 获取lesson，异步
                            ////////////////////////////////////////////////
                            Global.setCourseID(courseid);
                            Global.setCourseName(u.coursename);
                            Global.setTeacherID(u.id);
                            Global.setPadIP(clientIp);

                            Common.getLessonWithTimeout();
                            //Common.getLessonAsync();
                            //Common.getLesson();


                            u.lessonid = Global.getLessonID();
                            Thread th1 = new Thread(delegate()
                            {
                                IntelligentRecommend.InitLesson();
                            });
                            th1.Start();
                            string data = "";
                            data = u.toJson();
                            ret = getResponse(callback, 1, "登录成功", data);
                            return ret;
                        }
                    }
                }
                ret = getResponse(callback, 0, "登录失败", "");
            }
            return ret;
        }

        private static string handleAutoChangeClass(Dictionary<String, String> pList, string clientIp)
        {
            if (pList == null)
            {
                return getResponse("", 0, "登录失败!", "");
            }
            string ret = "";
            string callback = getParam(pList, "callback");
            string user = getParam(pList, "user");
            string pwd = getParam(pList, "pwd");
            int classid = Util.toInt(getParam(pList, "classid"));
           // string _classname = getParam(pList, "classname");
            int courseid = Util.toInt(getParam(pList, "courseid"));

            Classes c = m_db.getClassById(classid);
            string _classname = c.name;
            Global.lessonindex = getParam(pList, "lessonIndex");

            Global.saveClassConfig(classid, _classname);
            Global.loadClassInfo();
            
            //给后台传考勤信息
            int schooid = Global.getSchoolID();
            Common.uploadAllKQinfor(schooid, classid, Global.lessonindex, AnswerCard.Reader, Global.stuKQlist);

            if (user.Length > 0 && pwd.Length > 0)
            {
                User u = m_db.getTeacher(user);
                if (u == null)
                {
                    ret = getResponse(callback, 0, "登录失败，用户不存在。", "");
                }
                else if (pwd == u.pwd || pwd == GetMD5(u.pwd))
                {
                    ////////////////////////////////////////////////
                    // 获取lesson，异步
                    ////////////////////////////////////////////////
                    Common.getLessonAsync();
                    //Common.getLesson();


                    //u.classname = Global.getClassname();
                    u.classid = Global.getClassID();
                    u.lessonid = Global.getLessonID();
                    u.pwd = "";
                    string data = "";
                    data = u.toJson();
                    ret = getResponse(callback, 1, "登录成功", data);

                    ////////////////////////////////////////////////
                    // CourseID
                    ////////////////////////////////////////////////
                    m_db.setLastlogin(u);


                    Global.setCourseID(courseid);
                    Global.setCourseName(u.coursename);
                    Global.setTeacherID(u.id);
                    Global.setPadIP(clientIp);



                }
                else
                {
                    ret = getResponse(callback, 0, "登录失败,密码不正确", "");
                }
            }
            else
            {
                if (Global.g_TeacherArray != null && Global.g_TeacherArray.Length > 0)
                {
                    foreach (User u in Global.g_TeacherArray)
                    {
                        if (courseid == u.courseid)
                        {
                            //u.classname = Global.getClassname();
                            u.classid = classid;
                            u.pwd = "";

                            ////////////////////////////////////////////////
                            // 获取lesson，异步
                            ////////////////////////////////////////////////
                            Global.setCourseID(courseid);
                            Global.setCourseName(u.coursename);
                            Global.setTeacherID(u.id);
                            Global.setPadIP(clientIp);

                            Common.getLessonWithTimeout();
                            //Common.getLessonAsync();
                            //Common.getLesson();


                            u.lessonid = Global.getLessonID();
                            Thread th1 = new Thread(delegate()
                            {
                                IntelligentRecommend.InitLesson();
                            });
                            th1.Start();
                            string data = "";
                            data = u.toJson();
                            ret = getResponse(callback, 1, "登录成功", data);
                            return ret;
                        }
                    }
                }
                ret = getResponse(callback, 0, "登录失败", "");
            }
            return ret;
        }

        private static string handleKQlist(Dictionary<String, String> pList)
        {
            string ret = "";
            string classid = "";
            string callback = getParam(pList, "callback");
            //{"id":"3881","type":"2","phone":"1850409041","nick":"","account":"1704081482","name":"张鑫月","pinyin":"","imageurl":"http://www.jintaimingyuan.com/product/pics/20141225/1419474642.jpg","age":"9","gender":"2","province":"天津","city":"天津","district":"武清","address":"武清区","studentno":"1704081482","schoolid":"33","seat":"0-1","classid":"1634"
           // List<User> ulist = m_db.getStudentlist(Global.getClassID() + "");
            List<User> ulist = new List<User>();
            if(Global.stuKQlist == "")
            {
                Global.stuPadall = Global.stuPadon;
            }
            else if(Global.stuPadon == "")
            {
                Global.stuPadall = Global.stuKQlist;
            }
            else
            {
                Global.stuPadall = Global.stuKQlist + ";" + Global.stuPadon;
            }
            string[] sz = Global.stuPadall.Split(';');
            if(sz[0] != ""){
                foreach (string str in sz)
                {
                    ulist.Add(m_db.getCardlist(str.Substring(0, 10)));
                }
            }
                      
            string data = new JavaScriptSerializer().Serialize(ulist);

            List<PYGroup> pylist = new List<PYGroup>();
            foreach (User u in ulist)
            {
                string ch = u.pinying.ToUpper().Substring(0, 1);
                bool toCreate = true;
                foreach (PYGroup g in pylist)
                {
                    if (g.ch == ch)
                    {
                        toCreate = false;
                        g.userlist.Add(u);
                        break;
                    }
                }
                if (toCreate)
                {
                    PYGroup g = new PYGroup();
                    g.ch = ch;
                    g.userlist = new List<User>();
                    g.userlist.Add(u);

                    pylist.Add(g);
                }
            }

            foreach (PYGroup g in pylist)
            {
                g.userlist.Sort((x, y) => x.pinying.CompareTo(y.pinying));
            }
            pylist.Sort((x, y) => x.ch.CompareTo(y.ch));
            ret = getResponse(callback, 1, "获取打卡学生列表成功", data);
            return ret;
        }

        //{"ret":"0","msg":"登录成功！","data":{"id":"3842","type":"1","phone":"201604081438","nick":"","account":"wangqi","name":"王奇","pinyin":"","imageurl":"","age":"0","gender":"1","province":"北京","city":"北京","district":"东城区","address":"111111111111111","studentno":"","schoolid":"33","courseid":"0","teachyears":"0","intro":"1111111111111","classlist":[{"id":"1634","schoolid":"33","grade":"3","name":"三(1)班","seatxy":"1,1","orderid":"1","roomid":"1356","roomname":"三年级一班","building":"","hdid":"","hdip":"1","hdport":"80","appip":"172.18.2.105","courseid":"12","coursename":"语文"},{"id":"1638","schoolid":"33","grade":"1","name":"一(1)班","seatxy":"1,1","orderid":"1","roomid":"1360","roomname":"一年级一班","building":"","hdid":"192.168.253.201","hdip":"1","hdport":"80","appip":"172.18.2.104","courseid":"12","coursename":"语文"}],"schoolname":"测试小学","schoolcode":"3477786fefd57d7403631c839b6787d4"},"count":"1"}
        private static string handleGetStudentlist(Dictionary<String, String> pList)
        {
            string ret = "";
            string classid ="";
            string callback = getParam(pList, "callback");
            //{"id":"3881","type":"2","phone":"1850409041","nick":"","account":"1704081482","name":"张鑫月","pinyin":"","imageurl":"http://www.jintaimingyuan.com/product/pics/20141225/1419474642.jpg","age":"9","gender":"2","province":"天津","city":"天津","district":"武清","address":"武清区","studentno":"1704081482","schoolid":"33","seat":"0-1","classid":"1634"
            List<User> ulist = m_db.getStudentlist(Global.getClassID()+"");
            string data = new JavaScriptSerializer().Serialize(ulist);

            List<PYGroup> pylist = new List<PYGroup>();
            foreach (User u in ulist)
            {
                string ch = u.pinying.ToUpper().Substring(0, 1);
                bool toCreate = true;
                foreach (PYGroup g in pylist)
                {
                    if (g.ch == ch)
                    {
                        toCreate = false;
                        g.userlist.Add(u);
                        break;
                    }
                }
                if (toCreate)
                {
                    PYGroup g = new PYGroup();
                    g.ch = ch;
                    g.userlist = new List<User>();
                    g.userlist.Add(u);

                    pylist.Add(g);
                }
            }

            foreach (PYGroup g in pylist)
            {
                g.userlist.Sort((x, y) => x.pinying.CompareTo(y.pinying));
            }
            pylist.Sort((x, y) => x.ch.CompareTo(y.ch));
            ret = getResponse(callback,1, "获取学生列表成功", data);
            return ret;
        }

        private static string handleGetStudentlistGroupByPY(Dictionary<String, String> pList)
        {
            string ret = "";
            string classid = "";
            string callback = getParam(pList, "callback");
            //{"id":"3881","type":"2","phone":"1850409041","nick":"","account":"1704081482","name":"张鑫月","pinyin":"","imageurl":"http://www.jintaimingyuan.com/product/pics/20141225/1419474642.jpg","age":"9","gender":"2","province":"天津","city":"天津","district":"武清","address":"武清区","studentno":"1704081482","schoolid":"33","seat":"0-1","classid":"1634"
            List<User> ulist = m_db.getStudentlist(Global.getClassID() + "");


            List<PYGroup> pylist = new List<PYGroup>();
            
            foreach(User u in ulist)
            {
                string ch = u.pinying.ToUpper().Substring(0, 1);
                bool toCreate = true;
                foreach(PYGroup g in pylist)
                {
                    if(g.ch == ch)
                    {
                        toCreate = false;

                        g.userlist.Add(u);
                    }
                }
                if(toCreate)
                {
                    PYGroup g = new PYGroup();
                    g.ch = ch;
                    g.userlist = new List<User>();
                    g.userlist.Add(u);

                    pylist.Add(g);
                }
            }
            foreach (PYGroup g in pylist)
            {
                g.userlist.Sort((x, y) => x.pinying.CompareTo(y.pinying));
            }
            pylist.Sort((x, y) => x.ch.CompareTo(y.ch));
            string data = new JavaScriptSerializer().Serialize(pylist);
            ret = getResponse(callback, 1, "获取学生列表成功", data);
            return ret;
        }

        private static string handleGetAwardlist(Dictionary<String, String> pList)
        {
            string ret = "";
            string classid = "";
            string callback = getParam(pList, "callback");
            List<AwardType> ulist = new List<AwardType>(Global.g_szAwardType);
            string data = new JavaScriptSerializer().Serialize(ulist);
            ret = getResponse(callback, 1, "handleGetAwardlist ok", data);
            return ret;
        }
        private static string handleSetSound(Dictionary<String, String> pList)
        {
            string callback = getParam(pList, "callback");
            string sound_handon = getParam(pList, "handon");
            string sound_callname = getParam(pList, "callname");
            string sound_reward = getParam(pList, "reward");

            if (sound_handon.Length != 1)
                sound_handon = "0";
            if (sound_callname.Length != 1)
                sound_callname = "0";
            if (sound_reward.Length != 1)
                sound_reward = "0";

            Global.setSoundHandon(Util.toInt(sound_handon));
            Global.setSoundCallname(Util.toInt(sound_callname));
            Global.setSoundReward(Util.toInt(sound_reward));

            //http://192.168.253.1:8989/EduApi/user.do?action=sound.set&handon=1&callname=1&reward=1
            string data = Global.getSoundConfig();
            string ret = getResponse(callback, "setSoundConfig ok", data);
            return ret;
        }
        private static string handleGetSound(Dictionary<String, String> pList)
        {
            string callback = getParam(pList, "callback");

            //http://192.168.253.1:8989/EduApi/user.do?action=sound.get
            string data = Global.getSoundConfig();
            string ret = getResponse(callback,"getSoundConfig ok",data);
            return ret;
        }

        private static string handleGetBookoutline(Dictionary<String, String> pList)
        {
            string ret = "";
            string callback = getParam(pList, "callback");
            string action = getParam(pList, "action");

            int courseid = Global.getCourseID();
            int grade = Global.getGrade();
            //根据日期取学期
            int term = 0;
            int day = Int32.Parse(DateTime.Now.ToString("MMdd"));
            if (day > 800 || day < 210)
                term = 1;
            else
                term = 2;
            string coursename = Global.getCourseName();
            int teacheid = Global.getTeacherID();

            //Get from local buffer
            string data = "";
            {
                string dir = Application.StartupPath + "\\conf\\";
                string filename = "bo."+courseid + "-" + grade + "-" + term +".conf";
                FileOper fo = new FileOper(dir, filename);
                data = fo.ReadFile();
                if (data.Length == 0)
                {
                    data = Common.getBookOutline(action);
                    //更新本地缓存
                    fo.WriteFile(data);
                }
            }

            //http://192.168.253.1:8989/EduApi/user.do?action=bookoutline.get&classid=1634&courseid=12
            string retUnicode = getResponse(data);
            if (callback != null && callback.Length > 0)
            {
                retUnicode = callback + "(" + retUnicode + ")";
            }
            return retUnicode;
        }

        private static string handleGetXiti(Dictionary<String, String> pList)
        {
            //strKey=bookid, strVal=0, strVal2=0
            //strKey=action, strVal=xitiFromHX.list, strVal2=xitiFromHX.list
            //strKey=muluid, strVal=48713, strVal2=48713
            //strKey=callback, strVal=jsonp23, strVal2=jsonp23
            //strKey=lessonid, strVal=0, strVal2=0
            //strKey=courseid, strVal=12, strVal2=12
            //strKey=classid, strVal=1634, strVal2=1634
            string ret = "";
            string callback = getParam(pList, "callback");
            string bookid = getParam(pList, "bookid");
            string muluid = getParam(pList, "muluid");
            string action = getParam(pList, "action");

            //Get from local buffer
            string data = "";
            {
                int courseid = Global.getCourseID();
                int grade = Global.getGrade();
                //根据日期取学期
                int term = 0;
                int day = Int32.Parse(DateTime.Now.ToString("MMdd"));
                if (day > 800 || day < 210)
                    term = 1;
                else
                    term = 2;
                string dir = Application.StartupPath + "\\conf\\";
                string filename = "xt." + courseid + "-" + grade + "-" + term + "_"+bookid+"-"+muluid+".conf";
                string keyTemp = GetMD5(Key_P1 + filename + Key_P2);
                FileOper fo = new FileOper(dir, filename);
                data = fo.ReadFile();
                if (data.Length == 0)
                {
                    data = Common.getXiti(action, bookid, muluid);
                    if(data.Length > 0)
                    {
                        string dataEncrypt = AesEncrypt(data, keyTemp);//跟java的substring不一样

                        //更新本地缓存
                        fo.WriteFile(dataEncrypt);
                    }
                }
                else
                {
                    string dataDecrypt = AesDecrypt(data, keyTemp);//跟java的substring不一样
                    data = dataDecrypt;
                }
            }

            if (callback!=null && callback.Length > 0)
            {
                data = callback + "(" + data + ")";
            }
            return data;
        }


        /// <summary>
        /// 封装应答
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static string getResponse(string callback, string msg,string data)
        {
            if (data == null)
                data = "";

            string str;
            if(data.IndexOf("{")>=0 && data.IndexOf("}") >=0)
                str = "{\"ret\":\"1\",\"msg\":\"P1\",\"data\":P2,\"count\":\"0\"}".Replace("P1", msg).Replace("P2", data);
            else
                str = "{\"ret\":\"1\",\"msg\":\"P1\",\"data\":\"P2\",\"count\":\"0\"}".Replace("P1", msg).Replace("P2", data);

            if (msg.IndexOf("获取文件") < 0)  
                Log.Debug(str);

            //声明字符集   
            System.Text.Encoding utf8, gb2312;
            //gb2312   
            gb2312 = System.Text.Encoding.GetEncoding("gb2312");
            //utf8   
            utf8 = System.Text.Encoding.GetEncoding("utf-8");
            string outStr = "" ;
            for (int i = 0; i < str.Length; i++)
            {
                if (Regex.IsMatch(str[i].ToString(), @"[\u4e00-\u9fa5]"))
                {
                    outStr += "\\u" + ((int)str[i]).ToString("x");
                }
                else { 
                    outStr += str[i]; 
                }
            }

            if (callback.Length > 0)
                outStr = callback + "(" + outStr + ")";
            return outStr;

        }

        private static string getResponse(string callback,int ret, string msg, string data)
        {
            if (data == null)
                data = "";

            if(data.IndexOf("{")<0)
                data = "\"" + data+"\"";

            string str = "{\"ret\":\"P0\",\"msg\":\"P1\",\"data\":P2,\"count\":\"0\"}".Replace("P0", ret+"").Replace("P1", msg).Replace("P2", data);
            if (msg.IndexOf("获取文件") < 0)
                Log.Info(str);

            //声明字符集   
            System.Text.Encoding utf8, gb2312;
            //gb2312   
            gb2312 = System.Text.Encoding.GetEncoding("gb2312");
            //utf8   
            utf8 = System.Text.Encoding.GetEncoding("utf-8");
            string outStr = "";
            for (int i = 0; i < str.Length; i++)
            {
                if (Regex.IsMatch(str[i].ToString(), @"[\u4e00-\u9fa5]"))
                {
                    outStr += "\\u" + ((int)str[i]).ToString("x");
                }
                else
                {
                    outStr += str[i];
                }
            }

            if (callback.Length > 0)
                outStr = callback + "(" + outStr + ")";
            return outStr;

        }

        private static string getResponse(string str)
        {
            //声明字符集   
            System.Text.Encoding utf8, gb2312;
            //gb2312   
            gb2312 = System.Text.Encoding.GetEncoding("gb2312");
            //utf8   
            utf8 = System.Text.Encoding.GetEncoding("utf-8");
            string outStr = "";

            for (int i = 0; i < str.Length; i++)
            {
                if (Regex.IsMatch(str[i].ToString(), @"[\u4e00-\u9fa5]"))
                {
                    outStr += "\\u" + ((int)str[i]).ToString("x");
                }
                else
                {
                    outStr += str[i];
                }
            }
            return outStr;

        }

        private static string GetMD5(string sDataIn)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bytValue, bytHash;
            bytValue = System.Text.Encoding.UTF8.GetBytes(sDataIn);
            bytHash = md5.ComputeHash(bytValue);
            md5.Clear();
            string sTemp = "";
            for (int i = 0; i < bytHash.Length; i++)
            {
                sTemp += bytHash[i].ToString("X").PadLeft(2, '0');
            }
            return sTemp.ToLower();
        } 
        private static string getParam(Dictionary<String, String> pList,string name)
        {
            string value = "";
            if (pList != null)
            {
                if (pList.ContainsKey(name))
                    value = pList[name];
            }
            return value;
        }


        /// <summary>
        /// AES加密程序，与JAVA通用
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string AesEncrypt(string str, string key)
        {
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);

            System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = System.Security.Cryptography.CipherMode.ECB,
                Padding = System.Security.Cryptography.PaddingMode.PKCS7
            };

            System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateEncryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// AES解密，与JAVA通用
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string AesDecrypt(string str, string key)
        {
            if (string.IsNullOrEmpty(str))
                return null;
            try
            {
                Byte[] toEncryptArray = Convert.FromBase64String(str);

                System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
                {
                    Key = Encoding.UTF8.GetBytes(key),
                    Mode = System.Security.Cryptography.CipherMode.ECB,
                    Padding = System.Security.Cryptography.PaddingMode.PKCS7
                };

                System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateDecryptor();
                Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                return Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return "";
            }

        }


    }


}