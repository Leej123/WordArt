using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordArt.Animation;
using WordArt.Config;
using System.Runtime.InteropServices;

namespace WordArt
{
    /// <summary>
    /// 提供对外接口
    /// </summary>
    [Guid("5EBA6FB6-54BB-4033-97B2-D3609FE0E5FD"), ProgId("WordArt.WordArtApi"), ComVisible(true)]
    public class WordArtApi : IObjectSafety
    {
        private AnimationString animStr;
        private AnimationConfig config;

        private bool drawEnd = false;
        private DrawEvent userDrawEvent;
        private bool isBegin = false;

        public WordArtApi()
        {
            config = new AnimationConfig();
            animStr = new AnimationString(config);
            animStr.DrawBitmap += DrawBitmap;
        }

        #region 设置配置

        /// <summary>
        /// 设置动画字的宽
        /// </summary>
        /// <param name="width"></param>
        public void SetWidth(int width)
        {
            AnimationConfig.Width = width;
        }

        /// <summary>
        /// 设置动画字的高
        /// </summary>
        /// <param name="height"></param>
        public void SetHeight(int height)
        {
            AnimationConfig.Height = height;
        }

        /// <summary>
        /// 使能特效种类
        /// </summary>
        /// <param name="enable">true：动画特效；false：连续移动</param>
        public void EnableAnimationEffects(bool enable)
        {
            AnimationConfig.AnimationEffectsEnabled = enable;
        }

        /// <summary>
        /// 动画类型
        /// </summary>
        /// <param name="type"></param>
        public void SetAnimationType(int type)
        {
            if (type < 0) type = 0;
            AnimationConfig.AnimationType = (AnimationType)type;
        }

        /// <summary>
        /// 设置动画速度
        /// </summary>
        /// <param name="speed"></param>
        public void SetAnimationSpeed(float speed)
        {
            AnimationConfig.AnimationSpeedFactor = speed;
        }

        /// <summary>
        /// 设置连续移动速度
        /// </summary>
        /// <param name="speed"></param>
        public void SetContinuousMoveSpeed(float speed)
        {
            AnimationConfig.MoveSpeedFactor = speed;
        }

        /// <summary>
        /// 设置播放次数
        /// </summary>
        /// <param name="times"></param>
        public void SetPlayTimes(int times)
        {
            if (times <= 0) times = 1;
            AnimationConfig.PlayTimes = times;
        }

        /// <summary>
        /// 停留时间
        /// </summary>
        /// <param name="millisecond">毫秒单位，设置为100ms的倍数</param>
        public void SetResidenceTime(int millisecond)
        {
            if (millisecond < 100) millisecond = 100;
            AnimationConfig.ResidenceTime = millisecond;
        }

        /// <summary>
        /// 打开/关闭 炫彩
        /// </summary>
        /// <param name="enable"></param>
        public void EnableColorful(bool enable)
        {
            AnimationConfig.ColorfulEnabled = enable;
        }

        /// <summary>
        /// 此设置只有在炫彩打开下才有效
        /// </summary>
        /// <param name="enable">true：使用颜色渐变；false：使用图片填充</param>
        public void EnableColorGradient(bool enable)
        {
            AnimationConfig.IsColorGradient = enable;
        }

        /// <summary>
        /// 颜色渐变类型。只有在设置了颜色渐变下才有用
        /// </summary>
        /// <param name="type"></param>
        public void SetColorGradientType(int type)
        {
            if (type < 0) type = 0;
            AnimationConfig.ColorGradientType = (ColorGradientType)type;
        }

        /// <summary>
        /// 设置填充图的路径
        /// </summary>
        /// <param name="path">图片路径</param>
        public void SetFillPicturePath(string path)
        {
            AnimationConfig.FillBitmapPath = path;
        }

        /// <summary>
        /// 打开/关闭 霓虹背景
        /// </summary>
        /// <param name="enable"></param>
        public void EnableNeonBackground(bool enable)
        {
            AnimationConfig.NeonBackgroundEnabled = enable;
        }

        /// <summary>
        /// 设置霓虹背景图片的路径。只有在打开霓虹背景下有效。
        /// </summary>
        /// <param name="path">霓虹背景图片的路径</param>
        public void SetNeonBackgroundGifPath(string path)
        {
            AnimationConfig.NenoBackgroundPath = path;
        }

        /// <summary>
        /// 设置字体簇名
        /// </summary>
        /// <param name="fontName">字体名称</param>
        public void SetFontFamilyName(string fontName)
        {
            AnimationConfig.FontFamily = new FontFamily(fontName);
        }

        /// <summary>
        /// 设置字体大小
        /// </summary>
        /// <param name="fontSize"></param>
        public void SetFontSize(int fontSize)
        {
            if (fontSize < 9) fontSize = 9;
            AnimationConfig.EmSize = fontSize;
        }

        /// <summary>
        /// 是否使用粗体
        /// </summary>
        /// <param name="enalbe"></param>
        public void EnableBold(bool enable)
        {
            if (enable) AnimationConfig.FontStyle |= FontStyle.Bold;
            else AnimationConfig.FontStyle -= FontStyle.Bold;

        }

        /// <summary>
        /// 是否使用斜体
        /// </summary>
        /// <param name="enable"></param>
        public void EnableItalic(bool enable)
        {
            if (enable) AnimationConfig.FontStyle |= FontStyle.Italic;
            else AnimationConfig.FontStyle -= FontStyle.Italic;
        }

        /// <summary>
        /// 是否使用下划线
        /// </summary>
        /// <param name="enable"></param>
        public void EnableUnderline(bool enable)
        {
            if (enable) AnimationConfig.FontStyle |= FontStyle.Underline;
            else AnimationConfig.FontStyle -= FontStyle.Underline;
        }

        /// <summary>
        /// 是否空心
        /// </summary>
        /// <param name="enable"></param>
        public void EnableOutlineFont(bool enable)
        {
            AnimationConfig.IsFillMode = !enable;
        }

        /// <summary>
        /// 设置字体间距
        /// </summary>
        /// <param name="space"></param>
        public void SetWordSpace(int space)
        {
            if (space < 0) space = 0;
            AnimationConfig.Space = space;
        }

        /// <summary>
        /// 设置选用显示的文本
        /// </summary>
        /// <param name="text"></param>
        public void SetText(string text)
        {
            AnimationConfig.Text = text;
        }

        public void UseGifAsResult(bool enable)
        {
            AnimationConfig.IsUseGif = enable;
        }
        #endregion

        public void SetDrawEvent(DrawEvent drawEvent)
        {
            animStr.DrawBitmap += drawEvent;
            userDrawEvent = drawEvent;
        }

        /// <summary>
        /// 开始展示动画字
        /// </summary>
        /// <returns></returns>
        public bool Show()
        {
            if (AnimationConfig.Text == null || AnimationConfig.Text.Length <= 0) return false;

            Font font = new Font(AnimationConfig.FontFamily, AnimationConfig.EmSize, AnimationConfig.FontStyle, GraphicsUnit.Pixel);
            AnimationConfig.Font = font;
            animStr.CreateAnimation(null);
            isBegin = true;
            return true;
        }

        /// <summary>
        /// 动画是否开始
        /// </summary>
        /// <returns></returns>
        public bool IsBegin()
        {
            return isBegin;
        }

        public bool IsDrawingEnd()
        {
            return drawEnd;
        }

        public void Dispose()
        {
            animStr.Dispose();
            animStr.DrawBitmap -= DrawBitmap;
            if (null != userDrawEvent)
                animStr.DrawBitmap -= userDrawEvent;
            isBegin = false;
        }

        private void DrawBitmap(object sender, DrawEventArgs args)
        {
            drawEnd = true;
            isBegin = false;
        }

        #region 实现IObjectSafety接口
        private const string _IID_IDispatch = "{00020400-0000-0000-C000-000000000046}";
        private const string _IID_IDispatchEx = "{a6ef9860-c720-11d0-9337-00a0c90dcaa9}";
        private const string _IID_IPersistStorage = "{0000010A-0000-0000-C000-000000000046}";
        private const string _IID_IPersistStream = "{00000109-0000-0000-C000-000000000046}";
        private const string _IID_IPersistPropertyBag = "{37D84F60-42CB-11CE-8135-00AA004BB851}";

        private const int INTERFACESAFE_FOR_UNTRUSTED_CALLER = 0x00000001;
        private const int INTERFACESAFE_FOR_UNTRUSTED_DATA = 0x00000002;
        private const int S_OK = 0;
        private const int E_FAIL = unchecked((int)0x80004005);
        private const int E_NOINTERFACE = unchecked((int)0x80004002);

        private bool _fSafeForScripting = true;
        private bool _fSafeForInitializing = true;

        public int GetInterfaceSafetyOptions(ref Guid riid, ref int pdwSupportedOptions, ref int pdwEnabledOptions)
        {
            int Rslt = E_FAIL;

            string strGUID = riid.ToString("B");
            pdwSupportedOptions = INTERFACESAFE_FOR_UNTRUSTED_CALLER | INTERFACESAFE_FOR_UNTRUSTED_DATA;
            switch (strGUID)
            {
                case _IID_IDispatch:
                case _IID_IDispatchEx:
                    Rslt = S_OK;
                    pdwEnabledOptions = 0;
                    if (_fSafeForScripting == true)
                        pdwEnabledOptions = INTERFACESAFE_FOR_UNTRUSTED_CALLER;
                    break;
                case _IID_IPersistStorage:
                case _IID_IPersistStream:
                case _IID_IPersistPropertyBag:
                    Rslt = S_OK;
                    pdwEnabledOptions = 0;
                    if (_fSafeForInitializing == true)
                        pdwEnabledOptions = INTERFACESAFE_FOR_UNTRUSTED_DATA;
                    break;
                default:
                    Rslt = E_NOINTERFACE;
                    break;
            }

            return Rslt;
        }

        public int SetInterfaceSafetyOptions(ref Guid riid, int dwOptionSetMask, int dwEnabledOptions)
        {
            int Rslt = E_FAIL;
            string strGUID = riid.ToString("B");
            switch (strGUID)
            {
                case _IID_IDispatch:
                case _IID_IDispatchEx:
                    if (((dwEnabledOptions & dwOptionSetMask) == INTERFACESAFE_FOR_UNTRUSTED_CALLER) && (_fSafeForScripting == true))
                        Rslt = S_OK;
                    break;
                case _IID_IPersistStorage:
                case _IID_IPersistStream:
                case _IID_IPersistPropertyBag:
                    if (((dwEnabledOptions & dwOptionSetMask) == INTERFACESAFE_FOR_UNTRUSTED_DATA) && (_fSafeForInitializing == true))
                        Rslt = S_OK;
                    break;
                default:
                    Rslt = E_NOINTERFACE;
                    break;
            }

            return Rslt;
        }

        #endregion
    }
}
