using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordArt.Config;

namespace WordArt.Animation
{
    public class AnimationUtil
    {

        /// <summary>
        /// 文字由向左进入动画
        /// </summary>
        /// <param name="animChar"></param>
        public static void CreateRightInAnimation(AnimationChar animChar, float toX)
        {
            //计算与字体相关的参数
            int ascent = animChar.Font.FontFamily.GetCellAscent(animChar.Font.Style);
            int descent = animChar.Font.FontFamily.GetCellDescent(animChar.Font.Style);
            int emHeight = animChar.Font.FontFamily.GetEmHeight(animChar.Font.Style);

            float ascentF = ascent * animChar.Font.Size / emHeight;
            float descentF = descent * animChar.Font.Size / emHeight;

            //计算绘制文字的开始位置
            float locY = (AnimationConfig.Height - (ascentF + descentF)) / 2;

            animChar.SetLocation(AnimationConfig.Width, locY);
            animChar.CalcPath();//计算字体的路径
            animChar.Animation = CreatePostionAnimation(AnimationConfig.Width, toX, locY, locY);
        }

        public static PositionAnimation CreatePostionAnimation(float fromX, float toX, float fromY, float toY)
        {
            PointF from = new PointF(fromX, fromY);
            PointF to = new PointF(toX, toY);

            return new PositionAnimation(from, to);
        }

        public static TranslateAnimation CreateTranslateAnimation(float fromX, float toX, float fromY, float toY)
        {
            PointF from = new PointF(fromX, fromY);
            PointF to = new PointF(toX, toY);
            return new TranslateAnimation(from, to);
        }

        /// <summary>
        /// 创建一个动画字符。每个字符具有的字体，字体样式，以及动画都有可能不同。
        /// </summary>
        /// <param name="character"></param>
        /// <param name="g"></param>
        /// <returns></returns>
        public static AnimationChar CreatAnimationChar(string character, Graphics g)
        {
            AnimationChar animChar = new AnimationChar(character);
            animChar.Font = AnimationConfig.Font;
            animChar.Size = g.MeasureString(character, animChar.Font, new Point(0, 0), StringFormat.GenericTypographic);
            return animChar;
        }
    }
}
