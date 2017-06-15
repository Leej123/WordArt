using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordArt.Config;

namespace WordArt.Animation
{
    public class SlideInAnimation
    {
        public enum SlieInDirection
        { 
            /// <summary>
            /// 右边进入
            /// </summary>
            RIGHT,

            /// <summary>
            /// 左边进入
            /// </summary>
            LEFT,

            /// <summary>
            /// 下边进入
            /// </summary>
            BOTTOM,

            /// <summary>
            /// 上边进入
            /// </summary>
            TOP,

            /// <summary>
            /// 逐字
            /// </summary>
            VERBATIM
        }

        private Graphics g;

        public SlideInAnimation(Graphics g)
        {
            this.g = g;
        }

        public void CreateSlideInAnimation(List<LinkedList<AnimationChar>> screens, SlieInDirection direction)
        {
            screens.Clear();
            LinkedList<AnimationChar> linkedChars = new LinkedList<AnimationChar>();//一屏中显示的字符

            string text = AnimationConfig.Text;
            int textSize = text.Length;

            int length = 0;//字符显示的长度
            int charWidth = 0;//单个字符的宽度

            if (direction == SlieInDirection.LEFT)
            {
                length = AnimationConfig.Width;
                for (int i = textSize - 1; i >= 0; i--)
                {
                    string character = text.Substring(i, 1);
                    AnimationChar animChar = AnimationUtil.CreatAnimationChar(i, character, g);
                    charWidth = Convert.ToInt32(animChar.Size.Width);

                    if ((length - charWidth) < 0) //需要分屏
                    {
                        length = AnimationConfig.Width;
                        if (linkedChars.Count > 0)
                        {
                            screens.Add(linkedChars);
                            linkedChars = new LinkedList<AnimationChar>();//创建新的列表
                        }
                    }

                    CreateAnimation(direction, animChar, length - charWidth);
                    length -= (charWidth + AnimationConfig.Space);
                    LinkedListNode<AnimationChar> node = new LinkedListNode<AnimationChar>(animChar);
                    linkedChars.AddLast(node);
                }
            }
            else
            {
                for (int i = 0; i < textSize; i++)
                {
                    string character = text.Substring(i, 1);
                    AnimationChar animChar = AnimationUtil.CreatAnimationChar(i, character, g);
                    charWidth = Convert.ToInt32(animChar.Size.Width);
                    if (length + charWidth > AnimationConfig.Width) //需要分屏
                    {
                        length = 0;
                        if (linkedChars.Count > 0)
                        {
                            screens.Add(linkedChars);
                            linkedChars = new LinkedList<AnimationChar>();//创建新的列表
                        }
                    }

                    CreateAnimation(direction, animChar, length);
                    length += (charWidth + AnimationConfig.Space);
                    LinkedListNode<AnimationChar> node = new LinkedListNode<AnimationChar>(animChar);
                    linkedChars.AddLast(node);
                }
            }

            if (linkedChars.Count > 0)
                screens.Add(linkedChars);
        }

        private void CreateAnimation(SlideInAnimation.SlieInDirection direction, AnimationChar animChar, float finalPosX)
        {
            //计算与字体相关的参数
            int ascent = animChar.Font.FontFamily.GetCellAscent(animChar.Font.Style);
            int descent = animChar.Font.FontFamily.GetCellDescent(animChar.Font.Style);
            int emHeight = animChar.Font.FontFamily.GetEmHeight(animChar.Font.Style);

            float ascentF = ascent * animChar.Font.Size / emHeight;
            float descentF = descent * animChar.Font.Size / emHeight;

            //计算绘制文字的开始位置
            float height = ascentF + descentF;
            float locY = (AnimationConfig.Height - height) / 2;

            switch (direction)
            {
                case SlieInDirection.RIGHT:
                    animChar.SetLocation(AnimationConfig.Width, locY);
                    animChar.CalcPath();//计算字体的路径
                    animChar.Animation = AnimationUtil.CreatePostionAnimation(AnimationConfig.Width, finalPosX, locY, locY);
                    break;
                case SlieInDirection.LEFT:
                    int width = Convert.ToInt32(animChar.Size.Width);
                    animChar.SetLocation(-width, locY);
                    animChar.CalcPath();
                    animChar.Animation = AnimationUtil.CreatePostionAnimation(-width, finalPosX, locY, locY);
                    break;
                case SlieInDirection.BOTTOM:
                    animChar.SetLocation(finalPosX, AnimationConfig.Height);
                    animChar.CalcPath();
                    animChar.Animation = AnimationUtil.CreatePostionAnimation(finalPosX, finalPosX, AnimationConfig.Height, locY);
                    break;
                case SlieInDirection.TOP:
                    animChar.SetLocation(finalPosX, -height);
                    animChar.CalcPath();
                    animChar.Animation = AnimationUtil.CreatePostionAnimation(finalPosX, finalPosX, -height, locY);
                    break;
                case SlieInDirection.VERBATIM:
                    animChar.SetLocation(finalPosX, AnimationConfig.Height);
                    animChar.CalcPath();
                    animChar.Animation = AnimationUtil.CreateTranslateAnimation(finalPosX, finalPosX, AnimationConfig.Height, locY);
                    break;

                default://SlieInDirection.RIGHT
                    animChar.SetLocation(AnimationConfig.Width, locY);
                    animChar.CalcPath();//计算字体的路径
                    animChar.Animation = AnimationUtil.CreatePostionAnimation(AnimationConfig.Width, finalPosX, locY, locY);
                    break;
            }
        }
    }
}
