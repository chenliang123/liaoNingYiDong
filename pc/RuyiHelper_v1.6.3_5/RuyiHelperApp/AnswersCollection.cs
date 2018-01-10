using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace RueHelper
{
    public class AnswersCollection
    {
        private const string DLL_NAME = "AnswersCollection.dll";

        public enum CALLBACK_MSG
        {
            MSG_PULLEDOUT = 1,
            MSG_TEST_DATA = 2,
            MSG_ANSWER_DATA = 3,
            MSG_ATTENCE_DATA = 4,
            MSG_REGISTER_DATA = 5,
            MSG_ERROR = 6,
            MSG_VRITIME = 7,
            MSG_DOUTE_DATA = 9
        };

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void CallbackDelegate(int device, CALLBACK_MSG msg, int param1, string param2);

        [DllImport(DLL_NAME, EntryPoint = "TB_Init", CallingConvention = CallingConvention.StdCall)]
        internal static extern int TB_Init();

        [DllImport(DLL_NAME, EntryPoint = "TB_QueryReaderID", CallingConvention = CallingConvention.StdCall)]
        internal static extern int TB_QueryReaderID(int device, ref int ReaderID, int timeout);

        [DllImport(DLL_NAME, EntryPoint = "TB_Release", CallingConvention = CallingConvention.StdCall)]
        internal static extern int TB_Release();

        [DllImport(DLL_NAME, EntryPoint = "TB_EnumDevices", CallingConvention = CallingConvention.StdCall)]
        internal static extern int TB_EnumDevices(StringBuilder sComs);

        [DllImport(DLL_NAME, EntryPoint = "TB_OpenDevice", CallingConvention = CallingConvention.StdCall)]
        internal static extern int TB_OpenDevice(string com);

        [DllImport(DLL_NAME, EntryPoint = "TB_CloseDevice", CallingConvention = CallingConvention.StdCall)]
        internal static extern int TB_CloseDevice();

        [DllImport(DLL_NAME, EntryPoint = "TB_SetCallbackAddr", CallingConvention = CallingConvention.StdCall)]
        internal static extern int TB_SetCallbackAddr(CallbackDelegate callback_addr);

        [DllImport(DLL_NAME, EntryPoint = "TB_UpdateTime", CallingConvention = CallingConvention.StdCall)]
        internal static extern int TB_UpdateTime(int device, int timeout);

        [DllImport(DLL_NAME, EntryPoint = "TB_Start", CallingConvention = CallingConvention.StdCall)]
        internal static extern int TB_Start(int device);


        [DllImport(DLL_NAME, EntryPoint = "TB_Stop", CallingConvention = CallingConvention.StdCall)]
        internal static extern int TB_Stop(int device);

        [DllImport(DLL_NAME, EntryPoint = "TB_GetFirmwareVer", CallingConvention = CallingConvention.StdCall)]
        internal static extern int TB_GetFirmwareVer(int device, out byte major, out byte minor);

        [DllImport(DLL_NAME, EntryPoint = "TB_GetMiddlewareVer", CallingConvention = CallingConvention.StdCall)]
        internal static extern int TB_GetMiddlewareVer(out byte major, out byte minor);

        [DllImport(DLL_NAME, EntryPoint = "TB_SetWorkMode", CallingConvention = CallingConvention.StdCall)]
        internal static extern int TB_SetWorkMode(int device, TBModeDef mode, string param, int timeout);

        [DllImport(DLL_NAME, EntryPoint = "TB_EnableWhitelist", CallingConvention = CallingConvention.StdCall)]
        internal static extern int TB_EnableWhitelist(int device, int bEnable, int timeout);

        [DllImport(DLL_NAME, EntryPoint = "TB_AddtoWhitelist", CallingConvention = CallingConvention.StdCall)]
        internal static extern int TB_AddtoWhitelist(string cardid);

        [DllImport(DLL_NAME, EntryPoint = "TB_RemovefromWhitelist", CallingConvention = CallingConvention.StdCall)]
        internal static extern int TB_RemovefromWhitelist(string cardid);

        [DllImport(DLL_NAME, EntryPoint = "TB_GetWhitelist", CallingConvention = CallingConvention.StdCall)]
        internal static extern int TB_GetWhitelist(ulong[] TagID);

        [DllImport(DLL_NAME, EntryPoint = "HX_UnlockRegister", CallingConvention = CallingConvention.StdCall)]
        internal static extern int HX_UnlockRegister(string cardid);

        //[DllImport(DLL_NAME, EntryPoint = "HX_StartRegister", CallingConvention = CallingConvention.StdCall)]
        //internal static extern int HX_StartRegister();

        //[DllImport(DLL_NAME, EntryPoint = "HX_StopRegister", CallingConvention = CallingConvention.StdCall)]
        //internal static extern int HX_StopRegister();

        //[DllImport(DLL_NAME, EntryPoint = "HX_StartAttence", CallingConvention = CallingConvention.StdCall)]
        //internal static extern int HX_StartAttence();

        //[DllImport(DLL_NAME, EntryPoint = "HX_StopAttence", CallingConvention = CallingConvention.StdCall)]
        //internal static extern int HX_StopAttence();

        //[DllImport(DLL_NAME, EntryPoint = "HX_ConcurrentTest", CallingConvention = CallingConvention.StdCall)]
        //internal static extern int HX_ConcurrentTest(int status);

    }

    public enum TBModeDef
    {
        HX_MODE_NONE = 0, 	//待机模式
        HX_MODE_SINGLE = 1,	//单题模式
        HX_MODE_SINGLE_judge = 0x81, //单题模式判断
        HX_MODE_SINGLE_MUL = 0x41,	//单题模式多选
        HX_MODE_RAISE = 0xC1,	//单题模式抢答（举手）
        HX_MODE_MULTI = 2,	//多题模式
        HX_MODE_PAPER = 3		//套卷模式
    }
}