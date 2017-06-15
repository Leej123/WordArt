using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordArt.Animation;

namespace WordArt.Config
{
    /// <summary>
    /// 动画字的配置
    /// </summary>
    public class AnimationConfig
    {
        /// <summary>
        /// 动画字使用的字体
        /// </summary>
        public static Font Font = new Font(new FontFamily("宋体"), 9, FontStyle.Regular, GraphicsUnit.Pixel);

        /// <summary>
        /// 字头簇名
        /// </summary>
        public static FontFamily FontFamily = new FontFamily("宋体");

        /// <summary>
        /// 字体大小
        /// </summary>
        public static int EmSize = 9;

        /// <summary>
        /// 
        /// </summary>
        public static FontStyle FontStyle = FontStyle.Regular;

        /// <summary>
        /// 视图的宽
        /// </summary>
        public static int Width = 32;

        /// <summary>
        /// 视图的高度
        /// </summary>
        public static int Height = 32;

        /// <summary>
        /// 显示的文字
        /// </summary>
        public static string Text;

        /// <summary>
        /// 字间距
        /// </summary>
        public static int Space = 0;

        /// <summary>
        /// 填充还是空心，true为填充模式。
        /// </summary>
        public static bool IsFillMode = true;

        /// <summary>
        /// 动画类型
        /// </summary>
        public static AnimationType AnimationType = AnimationType.RIGHT_IN;

        public static MoveType MoveType = MoveType.CONTINUOUS_LEFT_SHIFT;

        /// <summary>
        /// 动画特效和连续移动。true：动画特效；false：连续移动
        /// </summary>
        public static bool AnimationEffectsEnabled = true;

        /// <summary>
        /// 每屏停留的时间，设置为100ms的倍数
        /// </summary>
        public static int ResidenceTime = 100;

        /// <summary>
        /// 动画速度
        /// </summary>
        public static float AnimationSpeedFactor = 1;

        /// <summary>
        /// 移动速度
        /// </summary>
        public static float MoveSpeedFactor = 1;

        /// <summary>
        /// 播放次数
        /// </summary>
        public static int PlayTimes = 1;

        /// <summary>
        /// 颜色渐变类型
        /// </summary>
        public static ColorGradientType ColorGradientType = ColorGradientType.LINEAR_GRADIENT;

        /// <summary>
        /// 图片填充。与颜色渐变，二选其一
        /// </summary>
        public static string FillBitmapPath="Res/bg1.jpg";

        /// <summary>
        /// true：使用颜色渐变；flase：使用图片填充
        /// </summary>
        public static bool IsColorGradient = true;

        /// <summary>
        /// 炫彩是否打开，如果打开才能使用颜色渐变或者图片填充
        /// </summary>
        public static bool ColorfulEnabled = true;

        /// <summary>
        /// 霓虹背景是否可用
        /// </summary>
        public static bool NeonBackgroundEnabled = true;

        /// <summary>
        /// 霓虹背景图片路径
        /// </summary>
        public static string NenoBackgroundPath = "neon_bg/neon_gif_0.gif";

        /// <summary>
        /// 是否使用GIF作为最终的结果
        /// </summary>
        public static bool IsUseGif = true;

        /// <summary>
        /// 在没设置炫彩时起作用
        /// </summary>
        public static Color TextColor = Color.Blue;

        /// <summary>
        /// 需要显示的文本
        /// </summary>
        public static List<TextEntity> TextList = new List<TextEntity>();

        /// <summary>
        /// 显示文本的字体
        /// </summary>
        public static Dictionary<int, Font> FontTable = new Dictionary<int, Font>();

        /// <summary>
        /// bmp保存的路径
        /// </summary>
        public static string BitmapSavePath = "./";

        /// <summary>
        /// bmp保存的名字
        /// </summary>
        public static string BitmapSaveName = "test";

        /// <summary>
        /// 动画循环时间（ms）
        /// </summary>
        public static long AnimateCycleTime = 0;
    }
}
