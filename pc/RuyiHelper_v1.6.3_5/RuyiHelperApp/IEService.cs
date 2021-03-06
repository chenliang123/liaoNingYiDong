﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace RueHelper
{
    [ServiceContract]
    public interface IEService
    {
        //[OperationContract(Action = "UploadFile", IsOneWay = true)]
        //void UploadFile(FileUploadMessage request);

        #region 登录前初始化----Init(),SetClass()
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        Response Init();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string SetClass(int classid);
        #endregion

        #region 显示学生笔迹, 隐藏，清空学生笔迹
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]//
        Response StartPenTrail();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]//
        Response ShowPenTrail(int index);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]//
        Response HidePenTrail();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]//
        Response ClearPenTrail();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]//
        Response ClosePenTrail();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]//
        Response ShowViewPen(string filename);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]//
        Response ShowViewPenInPanel(string filename, int index);

        #endregion



        #region 录音、录像的上传和删除
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]//
        Response UploadVideo(Stream stream);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]//
        Response UploadAudio(Stream stream);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string DelMedia(string filename);
        #endregion

        #region 下载PPT缩略图
        [WebGet(UriTemplate = "Download/{filename}")]
        Stream DownloadFileStream(string filename);//{path}/{serverfile}
        #endregion

        #region 摄像头拍照
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        Response TakePicture(string callback);//打开Form3,picture
        #endregion

        #region 图片播放、截屏、旋转、缩放和关闭
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        string ShowView(Stream stream);//打开Form3,picture

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Response ShowView4_1(Stream stream);//打开Form3_4,picture
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Response ShowView4_2(Stream stream);//打开Form3_4,picture
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Response ShowView4_3(Stream stream);//打开Form3_4,picture
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Response ShowView4_4(Stream stream);//打开Form3_4,picture

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string ShowView4_switch(int index);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string CloseView4();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string EmptyView4();

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        string ShowViewAgain(Stream stream);//打开Form3,picture

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        Response ShowView_Xiti();
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        Response ShowView_PPT();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string RotateView(int right);//打开Form3,picture

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        Response ShowViewLocal(string callback, string filename);//打开Form3,picture

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        Response ShowViewWeixin(string callback, string filename);//打开微信拍照

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string DrawView(string perX, string perY, int mode, string color, int width);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string ClearView();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string CloseView();//关闭Form3,picture

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string Zoom(double ratio, double ratioX, double ratioY);
        #endregion

        #region 触摸板
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string MouseMove(double Rx, double Ry);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string MouseClick();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string MouseRightClick();
        #endregion

        #region 播放Flash，视频，音频文件

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string PlayFlash(string url);//打开Form4,Flash

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string CloseFlash();//关闭Form4,Flash

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string PlayVideo(string url);//Form5,Video

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string PlayAudio(string url);//Form5,Video

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string CloseVideo();//Form5,Video

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string CloseAudio();//Form5,Audio
        #endregion

        #region 播放URL
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string OpenWeb(string url);//Form6,Web

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string CloseWeb();//Form6,Web
        #endregion

        #region 播放PPT,PDF,DOC
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string SelectPPT(string filepath);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string PPT(string url);//Form7

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string ClosePPT();//Form7

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Response OpenPPT(string filename, string pageIndex);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Response OpenDoc(string filename);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        string CloseDoc();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        string OpenPDF(string filename);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        string ClosePDF();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string LocalOpenPPT(string filename);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string LocalClosePPT();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string DeleteFile(string filename);
        #endregion

        #region 模拟PPT鼠标单击
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        void PPTMouseMove(double x, double y);
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        void PPTMouseClick(double x, double y);
        #endregion

        #region PPT最大化 最小化
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string MinimizePPT();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string MaximizePPT();

        #endregion

        #region 回答问题的推荐学生名单
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string getRecommendStudent();
        #endregion

        #region 题库出题\PPT截屏出题，设置答案，显示答题统计
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string Projective(string courseid, string id, string rid, string classid, string lessonid);
        //http://localhost:8986/Projective?courseid=13&id=0&rid=123&classid=43&lessonid=5000

        //--------------------PPT出题- FormPPTPractise---------------------
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        int ProjectiveInPPT(string courseid, string classid, string lessonid);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        int JudgeProjectiveInPPT(string courseid, string classid, string lessonid);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        int SingleProjectiveInPPT(string courseid, string classid, string lessonid);
        //http://localhost:8986/ProjectiveInPPT?courseid=13&classid=43&lessonid=5000
        //http://localhost:8989/PracticeAnswer.get
        //之前出题后必须看统计，所以是在Statistics()的时候执行的上传。
        //现在出题后实时同步答案，PAD不一定会Statistics()，因此需要在CloseProjective()中执行上传

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string SetAnswer(string answer);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string SwitchAnswer(string selectAnswer);//Form8, xiti.result

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        Response XitiSwitchView(int index);
        //http://localhost:8986/XitiSwitchView?index=1
        //http://localhost:8986/XitiSwitchView?index=2

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string Statistics(string id, string type, string answer);//Form8, xiti.result

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string CloseStatistics();//Form8, xiti.result

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string CloseProjective();//关闭Form2

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string HidePanel(int hide);
        #endregion

        #region 结束答题
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string StopXiti();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string StopJugeXiti();
        #endregion

        #region 举手&点名&奖励
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        Response Handon(int courseid, int lessonid);
        //http://localhost:8986/Handon

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        Response HandonClose();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        Response HandonSwitchView(int index);
        //http://localhost:8986/HandonSwitchView?index=1
        //http://localhost:8986/HandonSwitchView?index=2


        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string HandonOver(string id, string name);
        //http://localhost:8986/HandonOver?id=0&name=

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string Reward(string reason, string reasonid);//Form10,CallName

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string Award(int point, int uid, string name, string reason, string reasonid);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string Criticize(int id);
        #endregion

        #region 投票
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string VoteStart(string options);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string VoteStop();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string VoteClose();
        #endregion


        #region 精彩两分钟
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string TwoMinuteStart(string options);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string TwoMinuteStop();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string TwoMinuteUpInfo(string optTea, string optStu);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string TwoMinuteClose();
        #endregion

        #region 生生互动
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string EnterInter(string courseid, string classid, string lessonid, string stuName);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string ExitInter();
        #endregion

        #region 分组竞赛
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string EnterCompete(int num, string allScore, string Rank);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string ExitCompete();
        #endregion

        #region 分组竞赛得分
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string EnterScore(string groupnum, string scorenum);
        #endregion

        #region 抢答
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string QDStart();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string QDResult(string answer);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string QDClose();
        #endregion

        #region 课堂总结
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string Summary();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string SummaryClose();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        Response getXitiStat();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        Response getStat();
        #endregion

        #region 分组教学
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string showGroup(string group, string name);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string showGroupRank();
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string closeGroupRank();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string startGroupHandon();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string startGroupCallname(string group);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string startGroupReward(string group, int result);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string startGroupXiti(string rid);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string endGroupHandon();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string endGroupXiti();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string setGroupXitiAnswer(string answer);
        #endregion

        #region 下载文件 Download
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        Stream Download(string fileName);
        #endregion

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string GetFileBase64(string fileName);

        #region 退出如e课堂
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string Exit(string accessValue, string chapter);
        #endregion

        #region 辽宁移动登录成功
        [OperationContract]
        [WebInvoke(Method = "GET")]
        string loginSuccess(string clientid, string uid, string time, string sign);
        #endregion
    
    }
}