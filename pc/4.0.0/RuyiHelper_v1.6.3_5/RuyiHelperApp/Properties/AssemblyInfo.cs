﻿using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// 有关程序集的常规信息通过以下
// 特性集控制。更改这些特性值可修改
// 与程序集关联的信息。
[assembly: AssemblyTitle("互动课堂")]
[assembly: AssemblyDescription("如e教育")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("北京天地群网科技发展有限公司")]
[assembly: AssemblyProduct("互动课堂")]
[assembly: AssemblyCopyright("Copyright ©  2016")]
[assembly: AssemblyTrademark("如e教育")]
[assembly: AssemblyCulture("")]

// 将 ComVisible 设置为 false 使此程序集中的类型
// 对 COM 组件不可见。  如果需要从 COM 访问此程序集中的类型，
// 则将该类型上的 ComVisible 特性设置为 true。
[assembly: ComVisible(false)]

// 如果此项目向 COM 公开，则下列 GUID 用于类型库的 ID
[assembly: Guid("1a457c9c-eba8-46fd-81ca-216f069cd4e6")]

// 程序集的版本信息由下面四个值组成: 
//Major.Minor.Build.Revision
//      主版本
//      次版本 
//      生成号
//      修订号
//
// 可以指定所有这些值，也可以使用“生成号”和“修订号”的默认值，
// 方法是按如下所示使用“*”: 
[assembly: AssemblyVersion("4.0.0.*")]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]  

//0818
//1.从消息队列获取举手\习题结果等指令增加异常判断
//2.PPT部分缩略图片不显示
//3.点击下一页，下方页码进度条不增加/减少
//4.过滤掉临时文件（以“.”开头的隐藏文件）


//0601 举手窗口 没有新的按键就自动隐藏
//0521 PPT返回直接关闭进程

//1、当程序集为了维护而更新时，为了向后兼容，一定不要修改Assembly Version。
//2、在程序集有重大修改时，一定要修改Assembly Version。

//添加：
//1. FormSelectTeacher和FormSelectTeacherTop，实现form半透明，控件不透明
//    FormSelectTeacher使用opacity设置半透明
//    FormSelectTeacherTop 使用背景色和透明值实现透明，控件不透明
//2. FormNotifyToStart 启动后开始上课的小窗口
//3. 