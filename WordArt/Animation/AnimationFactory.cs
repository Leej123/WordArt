using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordArt.Config;

namespace WordArt.Animation
{
    public class AnimationFactory
    {
        private Graphics g;

        public Graphics Graphiscs
        {
            set { g = value; }
        }

        public AnimationFactory(Graphics g)
        {
            this.g = g;
        }

        public List<LinkedList<AnimationChar>> CreateAnimationChars(ref bool isParallelAnimation)
        {
            List<LinkedList<AnimationChar>> screens = new List<LinkedList<AnimationChar>>();
            isParallelAnimation = false;
            switch (AnimationConfig.AnimationType)
            {
                case AnimationType.RIGHT_IN:
                    //CreateRightInAnimation(screens);
                    SlideInAnimation slideInAnimation = new SlideInAnimation(g);
                    slideInAnimation.CreateSlideInAnimation(screens, SlideInAnimation.SlieInDirection.RIGHT);
                    break;
                case AnimationType.LEFT_IN:
                    slideInAnimation = new SlideInAnimation(g);
                    slideInAnimation.CreateSlideInAnimation(screens, SlideInAnimation.SlieInDirection.LEFT);
                    break;
                case AnimationType.BOTTOM_IN:
                    slideInAnimation = new SlideInAnimation(g);
                    slideInAnimation.CreateSlideInAnimation(screens, SlideInAnimation.SlieInDirection.BOTTOM);
                    break;
                case AnimationType.TOP_IN:
                    slideInAnimation = new SlideInAnimation(g);
                    slideInAnimation.CreateSlideInAnimation(screens, SlideInAnimation.SlieInDirection.TOP);
                    break;
                case AnimationType.VERBATIM:
                    slideInAnimation = new SlideInAnimation(g);
                    slideInAnimation.CreateSlideInAnimation(screens, SlideInAnimation.SlieInDirection.VERBATIM);
                    break;
                case AnimationType.WAVE:
                    CreateWaveAnimation(screens);
                    isParallelAnimation = true;
                    break;
                case AnimationType.SWAY:
                    CreateSwayAnimation(screens);
                    isParallelAnimation = true;
                    break;
                case AnimationType.ZOOM_IN_OUT:
                    CreateZoomInOutAnimation(screens);
                    isParallelAnimation = true;
                    break;
                case AnimationType.VERBATIM_AMPLIFICATION:
                    CreateScaleAnimation(screens, 0.0f, 1.0f);
                    break;
                case AnimationType.VERBATIM_NARROW:
                    CreateScaleAnimation(screens, 2.0f, 1.0f);
                    break;
                case AnimationType.UPWARD_ZOOM_IN_OUT:
                    CreateUpwardZoomInOutAnimation(screens);
                    isParallelAnimation = true;
                    break;
                case AnimationType.VERBATIM_FLIP:
                    CreateFilpAnimation(screens);
                    isParallelAnimation = true;
                    break;
                case AnimationType.SCALE_TO_RIGHT:
                    CreateScale2RightAnimation(screens, 0.02f, 1.0f);
                    isParallelAnimation = true;
                    break;
                case AnimationType.TREMBLE:
                    CreateTrembleAnimation(screens);
                    isParallelAnimation = true;
                    break;
                case AnimationType.ROTATE:
                    CreateRotateAnimation(screens, 0, 360);
                    isParallelAnimation = true;
                    break;
                case AnimationType.ROLLING:
                    CreateRollingAnimation(screens);
                    isParallelAnimation = true;
                    break;
                case AnimationType.ROTATE_AT_ANGLE:
                    CreateRotateAtAngleAnimation(screens, 0, 360);
                    isParallelAnimation = true;
                    break;
                case AnimationType.FALL_IN_TURN:
                    CreateFallInAnimation(screens, 2, false);
                    isParallelAnimation = true;
                    break;
                case AnimationType.FALL_IN_TURN_BOUNCE:
                    CreateFallInAnimation(screens, 2, true);
                    isParallelAnimation = true;
                    break;
                case AnimationType.SWAY_LEFT_RIGHT:
                    CreateSwayLeftRightAnimation(screens);
                    isParallelAnimation = true;
                    break;
                case AnimationType.REPEAT_FALL:
                    CreateRepeatFallAnimation(screens);
                    isParallelAnimation = true;
                    break;
                case AnimationType.TOP_RIGHT_MOVE_IN:
                    CreateTopRightInAnimation(screens, 0.5f, false);
                    isParallelAnimation = true;
                    break;
                case AnimationType.TOP_RIGHT_MOVE_IN_WIDTH_SCALE:
                    CreateTopRightInAnimation(screens, 0.5f, true);
                    isParallelAnimation = true;
                    break;
                case AnimationType.BLACK_HOLE:
                    CreateBlackHoleAnimation(screens);
                    isParallelAnimation = true;
                    break;
                case AnimationType.HORIZONTAL_ROTATION:
                    CreateHorizontalRotationAnimation(screens);
                    isParallelAnimation = true;
                    break;
                case AnimationType.TURN_INTO:
                    CreateTurnIntoAnimation(screens);
                    isParallelAnimation = true;
                    break;
                case AnimationType.ROTATION_INTO:
                    CreateRotationIntoAnimation(screens);
                    isParallelAnimation = true;
                    break;
                case AnimationType.TEXT_SCALING:
                    CreateTextScalingAnimation(screens);
                    isParallelAnimation = true;
                    break;
                case AnimationType.FROG_CARRY_OUT:
                    CreateFrogAnimation(screens, false);
                    break;
                case AnimationType.FROG_COMPRESS:
                    CreateFrogAnimation(screens, true);
                    break;
                case AnimationType.CONTINUOUSE_MOVE_LEFT:
                    CreateContinousMovementLeftAnimation(screens);
                    isParallelAnimation = true;
                    break;
                case AnimationType.CONTINUOUSE_MOVE_RIGHT:
                    CreateContinousMovementRightAnimation(screens);
                    isParallelAnimation = true;
                    break;
                case AnimationType.CONTINUOUSE_MOVE_BOTTOM:
                    CreateContinousMovementBottomAnimation(screens);
                    isParallelAnimation = true;
                    break;
                case AnimationType.CONTINUOUSE_MOVE_TOP:
                    CreateContinousMovementTopAnimation(screens);
                    isParallelAnimation = true;
                    break;

                default://AnimationType.RIGHT_IN
                    slideInAnimation = new SlideInAnimation(g);
                    slideInAnimation.CreateSlideInAnimation(screens, SlideInAnimation.SlieInDirection.RIGHT);
                    break;
            }

            return screens;
        }

        private void CreateWaveAnimation(List<LinkedList<AnimationChar>> screens)
        {
            screens.Clear();
            LinkedList<AnimationChar> linkedChars = new LinkedList<AnimationChar>();//一屏中显示的字符

            string text = AnimationConfig.Text;
            int textSize = text.Length;

            int length = 0;//字符显示的长度
            int charWidth = 0;//单个字符的宽度
            float omega = (float)Math.PI / (AnimationConfig.Font.Size * 20);
            float phase = 0;
            for (int i = 0; i < textSize; i++)
            {
                string character = text.Substring(i, 1);
                AnimationChar animChar = AnimationUtil.CreatAnimationChar(character, g);
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
                ///////////////////////////////////////////
                //计算与字体相关的参数
                int ascent = animChar.Font.FontFamily.GetCellAscent(animChar.Font.Style);
                int descent = animChar.Font.FontFamily.GetCellDescent(animChar.Font.Style);
                int emHeight = animChar.Font.FontFamily.GetEmHeight(animChar.Font.Style);

                float ascentF = ascent * animChar.Font.Size / emHeight;
                float descentF = descent * animChar.Font.Size / emHeight;

                //计算绘制文字的开始位置
                float locY = (AnimationConfig.Height - (ascentF + descentF)) / 2;

                phase = (float) ((length * omega) % (2 * Math.PI));

                float value = (float) (locY * Math.Sin(phase));
                WaveAnimation animation = new WaveAnimation(value, (AnimationConfig.Height - ascentF) / 2.0f, phase);
                animChar.SetLocation(length, locY);
                animChar.CalcPath();
                animChar.Animation = animation;
                //////////////////////////////////////////
                length += (charWidth + AnimationConfig.Space);
                LinkedListNode<AnimationChar> node = new LinkedListNode<AnimationChar>(animChar);
                linkedChars.AddLast(node);
            }

            if (linkedChars.Count > 0)
                screens.Add(linkedChars);
        }

        private void CreateSwayAnimation(List<LinkedList<AnimationChar>> screens)
        {
            screens.Clear();
            LinkedList<AnimationChar> linkedChars = new LinkedList<AnimationChar>();//一屏中显示的字符

            string text = AnimationConfig.Text;
            int textSize = text.Length;

            int length = 0;//字符显示的长度
            int charWidth = 0;//单个字符的宽度
            for (int i = 0; i < textSize; i++)
            {
                string character = text.Substring(i, 1);
                AnimationChar animChar = AnimationUtil.CreatAnimationChar(character, g);
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
                ///////////////////////////////////////////
                //计算与字体相关的参数
                int ascent = animChar.Font.FontFamily.GetCellAscent(animChar.Font.Style);
                int descent = animChar.Font.FontFamily.GetCellDescent(animChar.Font.Style);
                int emHeight = animChar.Font.FontFamily.GetEmHeight(animChar.Font.Style);

                float ascentF = ascent * animChar.Font.Size / emHeight;
                float descentF = descent * animChar.Font.Size / emHeight;

                //计算绘制文字的开始位置
                float locY = (AnimationConfig.Height - (ascentF + descentF)) / 2;

                SwayAnimation swayAnimation = new SwayAnimation(-1f, 1f, AnimationConfig.Height / 2.0f);                   

                //animChar.SetLocation(length, locY);
                animChar.SetLocation(length, locY);
                animChar.CalcPath();
                animChar.Animation = swayAnimation;
                //////////////////////////////////////////
                length += (charWidth + AnimationConfig.Space);
                LinkedListNode<AnimationChar> node = new LinkedListNode<AnimationChar>(animChar);
                linkedChars.AddLast(node);
            }

            if (linkedChars.Count > 0)
                screens.Add(linkedChars);
        }

        private void CreateZoomInOutAnimation(List<LinkedList<AnimationChar>> screens)
        {
            screens.Clear();
            LinkedList<AnimationChar> linkedChars = new LinkedList<AnimationChar>();//一屏中显示的字符

            string text = AnimationConfig.Text;
            int textSize = text.Length;

            int length = 0;//字符显示的长度
            int charWidth = 0;//单个字符的宽度
            for (int i = 0; i < textSize; i++)
            {
                string character = text.Substring(i, 1);
                AnimationChar animChar = AnimationUtil.CreatAnimationChar(character, g);
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
                ///////////////////////////////////////////
                //计算与字体相关的参数
                int ascent = animChar.Font.FontFamily.GetCellAscent(animChar.Font.Style);
                int descent = animChar.Font.FontFamily.GetCellDescent(animChar.Font.Style);
                int emHeight = animChar.Font.FontFamily.GetEmHeight(animChar.Font.Style);

                float ascentF = ascent * animChar.Font.Size / emHeight;
                float descentF = descent * animChar.Font.Size / emHeight;
                //计算绘制文字的开始位置
                float locY = (AnimationConfig.Height - (ascentF + descentF)) / 2;

                ZoomInOutAnimation animation = new ZoomInOutAnimation(1.0f, 1.5f, 1.0f, length, locY + ascentF);

                animChar.SetLocation(length, locY);
                animChar.CalcPath();
                animChar.Animation = animation;
                //////////////////////////////////////////
                length += (charWidth + AnimationConfig.Space);
                LinkedListNode<AnimationChar> node = new LinkedListNode<AnimationChar>(animChar);
                linkedChars.AddLast(node);
            }

            if (linkedChars.Count > 0)
                screens.Add(linkedChars);
        }

        private void CreateScaleAnimation(List<LinkedList<AnimationChar>> screens, float fromScale, float toScale)
        {
            screens.Clear();
            LinkedList<AnimationChar> linkedChars = new LinkedList<AnimationChar>();//一屏中显示的字符

            string text = AnimationConfig.Text;
            int textSize = text.Length;

            int length = 0;//字符显示的长度
            int charWidth = 0;//单个字符的宽度
            for (int i = 0; i < textSize; i++)
            {
                string character = text.Substring(i, 1);
                AnimationChar animChar = AnimationUtil.CreatAnimationChar(character, g);
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
                ///////////////////////////////////////////
                //计算与字体相关的参数
                int ascent = animChar.Font.FontFamily.GetCellAscent(animChar.Font.Style);
                int descent = animChar.Font.FontFamily.GetCellDescent(animChar.Font.Style);
                int emHeight = animChar.Font.FontFamily.GetEmHeight(animChar.Font.Style);

                float ascentF = ascent * animChar.Font.Size / emHeight;
                float descentF = descent * animChar.Font.Size / emHeight;
                //计算绘制文字的开始位置
                float locY = (AnimationConfig.Height - (ascentF + descentF)) / 2;

                ScaleAnimation animation = new ScaleAnimation(fromScale, toScale, length, locY + ascentF);

                animChar.SetLocation(length, locY);
                animChar.CalcPath();
                animChar.Animation = animation;
                //////////////////////////////////////////
                length += (charWidth + AnimationConfig.Space);
                LinkedListNode<AnimationChar> node = new LinkedListNode<AnimationChar>(animChar);
                linkedChars.AddLast(node);
            }

            if (linkedChars.Count > 0)
                screens.Add(linkedChars);
        }

        private void CreateUpwardZoomInOutAnimation(List<LinkedList<AnimationChar>> screens)
        {
            screens.Clear();
            LinkedList<AnimationChar> linkedChars = new LinkedList<AnimationChar>();//一屏中显示的字符

            string text = AnimationConfig.Text;
            int textSize = text.Length;

            int length = 0;//字符显示的长度
            int charWidth = 0;//单个字符的宽度
            float omega = (float)Math.PI / (AnimationConfig.Font.Size * 40);
            if (AnimationConfig.AnimationSpeedFactor >= 1)
                omega *= AnimationConfig.AnimationSpeedFactor;
            float phase = (float) Math.PI / 4;
            float maxScale = AnimationConfig.Height / (AnimationConfig.Font.GetHeight() * 2);
            int delay = 0;
            for (int i = 0; i < textSize; i++)
            {
                string character = text.Substring(i, 1);
                AnimationChar animChar = AnimationUtil.CreatAnimationChar(character, g);
                charWidth = Convert.ToInt32(animChar.Size.Width);
                if (length + charWidth > AnimationConfig.Width) //需要分屏
                {
                    length = 0;
                    delay = 0;
                    if (linkedChars.Count > 0)
                    {
                        screens.Add(linkedChars);
                        linkedChars = new LinkedList<AnimationChar>();//创建新的列表
                    }
                }
                ///////////////////////////////////////////
                //计算与字体相关的参数
                int ascent = animChar.Font.FontFamily.GetCellAscent(animChar.Font.Style);
                int descent = animChar.Font.FontFamily.GetCellDescent(animChar.Font.Style);
                int emHeight = animChar.Font.FontFamily.GetEmHeight(animChar.Font.Style);

                float ascentF = ascent * animChar.Font.Size / emHeight;
                float descentF = descent * animChar.Font.Size / emHeight;

                //计算绘制文字的开始位置
                float locY = (AnimationConfig.Height - (ascentF + descentF)) / 2;

                phase = (float)((length * omega) % (2 * Math.PI));

                SineZoomInOutAnimation animation = new SineZoomInOutAnimation(phase, maxScale, 0.05f, locY + ascentF, delay ++);
                animChar.SetLocation(length, locY);
                animChar.CalcPath();
                animChar.Animation = animation;
                //////////////////////////////////////////
                length += (charWidth + AnimationConfig.Space);
                LinkedListNode<AnimationChar> node = new LinkedListNode<AnimationChar>(animChar);
                linkedChars.AddLast(node);
            }

            if (linkedChars.Count > 0)
                screens.Add(linkedChars);
        }

        private void CreateFilpAnimation(List<LinkedList<AnimationChar>> screens)
        {
            screens.Clear();
            LinkedList<AnimationChar> linkedChars = new LinkedList<AnimationChar>();//一屏中显示的字符

            string text = AnimationConfig.Text;
            int textSize = text.Length;

            int length = 0;//字符显示的长度
            int charWidth = 0;//单个字符的宽度
            float omega = (float)Math.PI / (AnimationConfig.Font.Size * 120);
            if (AnimationConfig.AnimationSpeedFactor >= 1)
                omega *= AnimationConfig.AnimationSpeedFactor;
            float phase = (float) (-Math.PI / 2);
            int delay = 0;
            FlipAnimation preAnimation = null;
            for (int i = 0; i < textSize; i++)
            {
                string character = text.Substring(i, 1);
                AnimationChar animChar = AnimationUtil.CreatAnimationChar(character, g);
                charWidth = Convert.ToInt32(animChar.Size.Width);
                if (length + charWidth > AnimationConfig.Width) //需要分屏
                {
                    length = 0;
                    delay = 0;
                    preAnimation = null;
                    if (linkedChars.Count > 0)
                    {
                        screens.Add(linkedChars);
                        linkedChars = new LinkedList<AnimationChar>();//创建新的列表
                    }
                }
                ///////////////////////////////////////////
                //计算与字体相关的参数
                int ascent = animChar.Font.FontFamily.GetCellAscent(animChar.Font.Style);
                int descent = animChar.Font.FontFamily.GetCellDescent(animChar.Font.Style);
                int emHeight = animChar.Font.FontFamily.GetEmHeight(animChar.Font.Style);

                float ascentF = ascent * animChar.Font.Size / emHeight;
                float descentF = descent * animChar.Font.Size / emHeight;

                //计算绘制文字的开始位置
                float locY = (AnimationConfig.Height - (ascentF + descentF)) / 2;

                phase = (float)((length * omega) % (2 * Math.PI));

                FlipAnimation animation = new FlipAnimation(phase, locY + ascentF, delay++);
                if (preAnimation != null)
                {
                    preAnimation.NextAnimation = animation;
                }
                preAnimation = animation;
                
                animChar.SetLocation(length, locY);
                animChar.CalcPath();
                animChar.Animation = animation;
                //////////////////////////////////////////
                length += (charWidth + AnimationConfig.Space);
                LinkedListNode<AnimationChar> node = new LinkedListNode<AnimationChar>(animChar);
                linkedChars.AddLast(node);
            }

            if (linkedChars.Count > 0)
                screens.Add(linkedChars);
        }

        private void CreateScale2RightAnimation(List<LinkedList<AnimationChar>> screens, float fromScale, float toScale)
        {
            screens.Clear();
            LinkedList<AnimationChar> linkedChars = new LinkedList<AnimationChar>();//一屏中显示的字符

            string text = AnimationConfig.Text;
            int textSize = text.Length;

            int length = 0;//字符显示的长度
            int charWidth = 0;//单个字符的宽度
            for (int i = 0; i < textSize; i++)
            {
                string character = text.Substring(i, 1);
                AnimationChar animChar = AnimationUtil.CreatAnimationChar(character, g);
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
                ///////////////////////////////////////////
                //计算与字体相关的参数
                int ascent = animChar.Font.FontFamily.GetCellAscent(animChar.Font.Style);
                int descent = animChar.Font.FontFamily.GetCellDescent(animChar.Font.Style);
                int emHeight = animChar.Font.FontFamily.GetEmHeight(animChar.Font.Style);

                float ascentF = ascent * animChar.Font.Size / emHeight;
                float descentF = descent * animChar.Font.Size / emHeight;
                //计算绘制文字的开始位置
                float locY = (AnimationConfig.Height - (ascentF + descentF)) / 2;

                ScaleToRightAnimation animation = new ScaleToRightAnimation(fromScale, toScale, length);

                animChar.SetLocation(length, locY);
                animChar.CalcPath();
                animChar.Animation = animation;
                //////////////////////////////////////////
                length += (charWidth + AnimationConfig.Space);
                LinkedListNode<AnimationChar> node = new LinkedListNode<AnimationChar>(animChar);
                linkedChars.AddLast(node);
            }

            if (linkedChars.Count > 0)
                screens.Add(linkedChars);
        }

        private void CreateTrembleAnimation(List<LinkedList<AnimationChar>> screens)
        {
            screens.Clear();
            LinkedList<AnimationChar> linkedChars = new LinkedList<AnimationChar>();//一屏中显示的字符

            string text = AnimationConfig.Text;
            int textSize = text.Length;

            int length = 0;//字符显示的长度
            int charWidth = 0;//单个字符的宽度
            Random random = new Random();
            for (int i = 0; i < textSize; i++)
            {
                string character = text.Substring(i, 1);
                AnimationChar animChar = AnimationUtil.CreatAnimationChar(character, g);
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
                ///////////////////////////////////////////
                //计算与字体相关的参数
                int ascent = animChar.Font.FontFamily.GetCellAscent(animChar.Font.Style);
                int descent = animChar.Font.FontFamily.GetCellDescent(animChar.Font.Style);
                int emHeight = animChar.Font.FontFamily.GetEmHeight(animChar.Font.Style);

                float ascentF = ascent * animChar.Font.Size / emHeight;
                float descentF = descent * animChar.Font.Size / emHeight;

                //计算绘制文字的开始位置
                float locY = (AnimationConfig.Height - (ascentF + descentF)) / 2;

                TrembleAnimation animation = new TrembleAnimation(random);

                //animChar.SetLocation(length, locY);
                animChar.SetLocation(length, locY);
                animChar.CalcPath();
                animChar.Animation = animation;
                //////////////////////////////////////////
                length += (charWidth + AnimationConfig.Space);
                LinkedListNode<AnimationChar> node = new LinkedListNode<AnimationChar>(animChar);
                linkedChars.AddLast(node);
            }

            if (linkedChars.Count > 0)
                screens.Add(linkedChars);
        }

        private void CreateRotateAnimation(List<LinkedList<AnimationChar>> screens, float fromAngle, float toAngle)
        {
            screens.Clear();
            LinkedList<AnimationChar> linkedChars = new LinkedList<AnimationChar>();//一屏中显示的字符

            string text = AnimationConfig.Text;
            int textSize = text.Length;

            int length = 0;//字符显示的长度
            int charWidth = 0;//单个字符的宽度
            for (int i = 0; i < textSize; i++)
            {
                string character = text.Substring(i, 1);
                AnimationChar animChar = AnimationUtil.CreatAnimationChar(character, g);
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
                ///////////////////////////////////////////
                //计算与字体相关的参数
                int ascent = animChar.Font.FontFamily.GetCellAscent(animChar.Font.Style);
                int descent = animChar.Font.FontFamily.GetCellDescent(animChar.Font.Style);
                int emHeight = animChar.Font.FontFamily.GetEmHeight(animChar.Font.Style);

                float ascentF = ascent * animChar.Font.Size / emHeight;
                float descentF = descent * animChar.Font.Size / emHeight;
                //计算绘制文字的开始位置
                float locY = (AnimationConfig.Height - (ascentF + descentF)) / 2;

                RotateAnimation animation = new RotateAnimation(fromAngle, toAngle, length + charWidth / 2, locY + ascentF / 2);
                
                animChar.SetLocation(length, locY);
                animChar.CalcPath();
                animChar.Animation = animation;
                //////////////////////////////////////////
                length += (charWidth + AnimationConfig.Space);
                LinkedListNode<AnimationChar> node = new LinkedListNode<AnimationChar>(animChar);
                linkedChars.AddLast(node);
            }

            if (linkedChars.Count > 0)
                screens.Add(linkedChars);
        }

        private void CreateRollingAnimation(List<LinkedList<AnimationChar>> screens)
        {
            screens.Clear();
            LinkedList<AnimationChar> linkedChars = new LinkedList<AnimationChar>();//一屏中显示的字符

            string text = AnimationConfig.Text;
            int textSize = text.Length;

            int length = 0;//字符显示的长度
            int charWidth = 0;//单个字符的宽度
            for (int i = 0; i < textSize; i++)
            {
                string character = text.Substring(i, 1);
                AnimationChar animChar = AnimationUtil.CreatAnimationChar(character, g);
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
                ///////////////////////////////////////////
                //计算与字体相关的参数
                int ascent = animChar.Font.FontFamily.GetCellAscent(animChar.Font.Style);
                int descent = animChar.Font.FontFamily.GetCellDescent(animChar.Font.Style);
                int emHeight = animChar.Font.FontFamily.GetEmHeight(animChar.Font.Style);

                float ascentF = ascent * animChar.Font.Size / emHeight;
                float descentF = descent * animChar.Font.Size / emHeight;
                //计算绘制文字的开始位置
                float locY = (AnimationConfig.Height - (ascentF + descentF)) / 2;

                RollingAnimation animation = new RollingAnimation(length + AnimationConfig.Width, length,
                    length + AnimationConfig.Width + charWidth / 2, locY + ascentF / 2);

                animChar.SetLocation(length + AnimationConfig.Width, locY);
                animChar.CalcPath();
                animChar.Animation = animation;
                //////////////////////////////////////////
                length += (charWidth + AnimationConfig.Space);
                LinkedListNode<AnimationChar> node = new LinkedListNode<AnimationChar>(animChar);
                linkedChars.AddLast(node);
            }

            if (linkedChars.Count > 0)
                screens.Add(linkedChars);
        }

        private void CreateRotateAtAngleAnimation(List<LinkedList<AnimationChar>> screens, float fromAngle, float toAngle)
        {
            screens.Clear();
            LinkedList<AnimationChar> linkedChars = new LinkedList<AnimationChar>();//一屏中显示的字符

            string text = AnimationConfig.Text;
            int textSize = text.Length;

            int length = 0;//字符显示的长度
            int charWidth = 0;//单个字符的宽度
            for (int i = 0; i < textSize; i++)
            {
                string character = text.Substring(i, 1);
                AnimationChar animChar = AnimationUtil.CreatAnimationChar(character, g);
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
                ///////////////////////////////////////////
                //计算与字体相关的参数
                int ascent = animChar.Font.FontFamily.GetCellAscent(animChar.Font.Style);
                int descent = animChar.Font.FontFamily.GetCellDescent(animChar.Font.Style);
                int emHeight = animChar.Font.FontFamily.GetEmHeight(animChar.Font.Style);

                float ascentF = ascent * animChar.Font.Size / emHeight;
                float descentF = descent * animChar.Font.Size / emHeight;
                //计算绘制文字的开始位置
                float locY = (AnimationConfig.Height - (ascentF + descentF)) / 2;

                RotateAnimation animation = new RotateAnimation(fromAngle, toAngle, length + charWidth / 2, locY + ascentF);

                animChar.SetLocation(length, locY);
                animChar.CalcPath();
                animChar.Animation = animation;
                //////////////////////////////////////////
                length += (charWidth + AnimationConfig.Space);
                LinkedListNode<AnimationChar> node = new LinkedListNode<AnimationChar>(animChar);
                linkedChars.AddLast(node);
            }

            if (linkedChars.Count > 0)
                screens.Add(linkedChars);
        }

        private void CreateFallInAnimation(List<LinkedList<AnimationChar>> screens, float delta, bool bounceBack)
        {
            screens.Clear();
            LinkedList<AnimationChar> linkedChars = new LinkedList<AnimationChar>();//一屏中显示的字符

            string text = AnimationConfig.Text;
            int textSize = text.Length;

            FallInTurnAnimation preAnimation = null;
            float fromY = delta;
            float offsetY = -delta;
            PointF pivot = new PointF();
            float bounceY = 0;
            int length = 0;//字符显示的长度
            int charWidth = 0;//单个字符的宽度
            for (int i = 0; i < textSize; i++)
            {
                string character = text.Substring(i, 1);
                AnimationChar animChar = AnimationUtil.CreatAnimationChar(character, g);
                charWidth = Convert.ToInt32(animChar.Size.Width);
                if (length + charWidth > AnimationConfig.Width) //需要分屏
                {
                    length = 0;
                    fromY = 0;
                    offsetY = -delta;
                    if (linkedChars.Count > 0)
                    {
                        screens.Add(linkedChars);
                        linkedChars = new LinkedList<AnimationChar>();//创建新的列表
                    }
                }
                ///////////////////////////////////////////
                //计算与字体相关的参数
                int ascent = animChar.Font.FontFamily.GetCellAscent(animChar.Font.Style);
                int descent = animChar.Font.FontFamily.GetCellDescent(animChar.Font.Style);
                int emHeight = animChar.Font.FontFamily.GetEmHeight(animChar.Font.Style);

                float ascentF = ascent * animChar.Font.Size / emHeight;
                float descentF = descent * animChar.Font.Size / emHeight;
                //计算绘制文字的开始位置
                float locY = (AnimationConfig.Height - (ascentF + descentF)) / 2;

                fromY = -(ascentF + descentF + offsetY);
                offsetY += delta;

                if (bounceBack)
                {
                    pivot.X = length + charWidth / 2;
                    pivot.Y = fromY + ascentF / 2;
                    bounceY = AnimationConfig.Height / 4 - ascentF;
                }

                FallInTurnAnimation animation = new FallInTurnAnimation(fromY, locY, bounceBack,
                    45, pivot, bounceY);
                if (preAnimation != null)
                    preAnimation.NextAnimation = animation;
                preAnimation = animation;

                animChar.SetLocation(length, fromY);
                animChar.CalcPath();
                animChar.Animation = animation;
                //////////////////////////////////////////
                length += (charWidth + AnimationConfig.Space);
                LinkedListNode<AnimationChar> node = new LinkedListNode<AnimationChar>(animChar);
                linkedChars.AddLast(node);
            }

            if (linkedChars.Count > 0)
                screens.Add(linkedChars);
        }

        private void CreateSwayLeftRightAnimation(List<LinkedList<AnimationChar>> screens)
        {
            screens.Clear();
            LinkedList<AnimationChar> linkedChars = new LinkedList<AnimationChar>();//一屏中显示的字符

            string text = AnimationConfig.Text;
            int textSize = text.Length;

            int length = 0;//字符显示的长度
            int charWidth = 0;//单个字符的宽度
            for (int i = 0; i < textSize; i++)
            {
                string character = text.Substring(i, 1);
                AnimationChar animChar = AnimationUtil.CreatAnimationChar(character, g);
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
                ///////////////////////////////////////////
                //计算与字体相关的参数
                int ascent = animChar.Font.FontFamily.GetCellAscent(animChar.Font.Style);
                int descent = animChar.Font.FontFamily.GetCellDescent(animChar.Font.Style);
                int emHeight = animChar.Font.FontFamily.GetEmHeight(animChar.Font.Style);

                float ascentF = ascent * animChar.Font.Size / emHeight;
                float descentF = descent * animChar.Font.Size / emHeight;
                //计算绘制文字的开始位置
                float locY = (AnimationConfig.Height - (ascentF + descentF)) / 2;

                PointF rightPivot = new PointF();
                rightPivot.X = length + charWidth;
                rightPivot.Y = locY + ascentF;

                PointF leftPivot = new PointF();
                leftPivot.X = length;
                leftPivot.Y = locY + ascentF;

                SwayLeftRightAnimation animation = new SwayLeftRightAnimation(rightPivot, leftPivot, 45);

                animChar.SetLocation(length, locY);
                animChar.CalcPath();
                animChar.Animation = animation;
                //////////////////////////////////////////
                length += (charWidth + AnimationConfig.Space);
                LinkedListNode<AnimationChar> node = new LinkedListNode<AnimationChar>(animChar);
                linkedChars.AddLast(node);
            }

            if (linkedChars.Count > 0)
                screens.Add(linkedChars);
        }

        private void CreateRepeatFallAnimation(List<LinkedList<AnimationChar>> screens)
        {
            screens.Clear();
            LinkedList<AnimationChar> linkedChars = new LinkedList<AnimationChar>();//一屏中显示的字符

            string text = AnimationConfig.Text;
            int textSize = text.Length;

            int length = 0;//字符显示的长度
            int charWidth = 0;//单个字符的宽度
            float initY = 0;
            float maxFontHeight = 0;//当前屏中，高度最高的字体
            for (int i = 0; i < textSize; i++)
            {
                string character = text.Substring(i, 1);
                AnimationChar animChar = AnimationUtil.CreatAnimationChar(character, g);
                charWidth = Convert.ToInt32(animChar.Size.Width);
                if (length + charWidth > AnimationConfig.Width) //需要分屏
                {
                    length = 0;
                    if (linkedChars.Count > 0)
                    {
                        /////调整文字的位置/////
                        int count = linkedChars.Count;
                        float delta = (AnimationConfig.Height - maxFontHeight) / (count + 1);
                        int j = 0;
                        
                        LinkedListNode<AnimationChar> charNode = linkedChars.First;
                        while (charNode != null)
                        {
                            initY = AnimationConfig.Height - (charNode.Value.Size.Height + j * delta);
                            charNode.Value.SetLocation(charNode.Value.Location.X, initY);
                            charNode.Value.CalcPath();

                            RepeatFallAnimation animation = new RepeatFallAnimation(AnimationConfig.Height - charNode.Value.Size.Height, 0, initY);
                            charNode.Value.Animation = animation;

                            j++;
                            charNode = charNode.Next;
                        }
                        //////////////////////
                        screens.Add(linkedChars);
                        linkedChars = new LinkedList<AnimationChar>();//创建新的列表
                    }
                    maxFontHeight = 0;
                }
                ///////////////////////////////////////////
                //计算与字体相关的参数
                int ascent = animChar.Font.FontFamily.GetCellAscent(animChar.Font.Style);
                int descent = animChar.Font.FontFamily.GetCellDescent(animChar.Font.Style);
                int emHeight = animChar.Font.FontFamily.GetEmHeight(animChar.Font.Style);

                float ascentF = ascent * animChar.Font.Size / emHeight;
                float descentF = descent * animChar.Font.Size / emHeight;
                //计算绘制文字的开始位置
                float locY = (AnimationConfig.Height - (ascentF + descentF)) / 2;

                //RepeatFallAnimation animation = new RepeatFallAnimation(AnimationConfig.Height - (ascentF + descentF), 0, initY);

                animChar.SetLocation(length, locY);
                //animChar.CalcPath();
                //animChar.Animation = animation;

                if (ascentF + descentF > maxFontHeight)
                    maxFontHeight = ascentF + descentF;

                //////////////////////////////////////////
                length += (charWidth + AnimationConfig.Space);
                LinkedListNode<AnimationChar> node = new LinkedListNode<AnimationChar>(animChar);
                linkedChars.AddLast(node);
            }

            if (linkedChars.Count > 0)
            {
                /////调整文字的位置/////
                int count = linkedChars.Count;
                float delta = (AnimationConfig.Height - maxFontHeight) / (count + 1);
                int j = 0;

                LinkedListNode<AnimationChar> charNode = linkedChars.First;
                while (charNode != null)
                {
                    initY = AnimationConfig.Height - (charNode.Value.Size.Height + j * delta);
                    charNode.Value.SetLocation(charNode.Value.Location.X, initY);
                    charNode.Value.CalcPath();

                    RepeatFallAnimation animation = new RepeatFallAnimation(AnimationConfig.Height - charNode.Value.Size.Height, 0, initY);
                    charNode.Value.Animation = animation;

                    j++;
                    charNode = charNode.Next;
                }
                //////////////////////
                screens.Add(linkedChars);
            }   
        }

        private void CreateTopRightInAnimation(List<LinkedList<AnimationChar>> screens, float delta, bool scale)
        {
            screens.Clear();
            LinkedList<AnimationChar> linkedChars = new LinkedList<AnimationChar>();//一屏中显示的字符

            string text = AnimationConfig.Text;
            int textSize = text.Length;

            TopRightMoveInAnimation preAnimation = null;
            float fromY = 0;
            int length = 0;//字符显示的长度
            int charWidth = 0;//单个字符的宽度
            for (int i = 0; i < textSize; i++)
            {
                string character = text.Substring(i, 1);
                AnimationChar animChar = AnimationUtil.CreatAnimationChar(character, g);
                charWidth = Convert.ToInt32(animChar.Size.Width);
                if (length + charWidth > AnimationConfig.Width) //需要分屏
                {
                    length = 0;
                    fromY = 0;
                    if (linkedChars.Count > 0)
                    {
                        screens.Add(linkedChars);
                        linkedChars = new LinkedList<AnimationChar>();//创建新的列表
                    }
                }
                ///////////////////////////////////////////
                //计算与字体相关的参数
                int ascent = animChar.Font.FontFamily.GetCellAscent(animChar.Font.Style);
                int descent = animChar.Font.FontFamily.GetCellDescent(animChar.Font.Style);
                int emHeight = animChar.Font.FontFamily.GetEmHeight(animChar.Font.Style);

                float ascentF = ascent * animChar.Font.Size / emHeight;
                float descentF = descent * animChar.Font.Size / emHeight;
                //计算绘制文字的开始位置
                float locY = (AnimationConfig.Height - (ascentF + descentF)) / 2;


                TopRightMoveInAnimation animation;
                if (!scale)
                {
                    animation = new TopRightMoveInAnimation(AnimationConfig.Width + length, length, fromY, locY,
                        AnimationConfig.Width, locY);
                }
                else
                {
                    animation = new TopRighMoveInWithScaleAnimation(AnimationConfig.Width + length, length, fromY, locY,
                        AnimationConfig.Width, locY, 0.01f, 1.0f);
                }
                

                if (preAnimation != null)
                    preAnimation.NextAnimation = animation;
                preAnimation = animation;

                animChar.SetLocation(AnimationConfig.Width + length, fromY);
                animChar.CalcPath();
                animChar.Animation = animation;

                fromY -= delta;
                //////////////////////////////////////////
                length += (charWidth + AnimationConfig.Space);
                LinkedListNode<AnimationChar> node = new LinkedListNode<AnimationChar>(animChar);
                linkedChars.AddLast(node);
            }

            if (linkedChars.Count > 0)
                screens.Add(linkedChars);
        }

        private void CreateBlackHoleAnimation(List<LinkedList<AnimationChar>> screens)
        {
            screens.Clear();
            LinkedList<AnimationChar> linkedChars = new LinkedList<AnimationChar>();//一屏中显示的字符

            string text = AnimationConfig.Text;
            int textSize = text.Length;

            int length = 0;//字符显示的长度
            int charWidth = 0;//单个字符的宽度
            for (int i = 0; i < textSize; i++)
            {
                string character = text.Substring(i, 1);
                AnimationChar animChar = AnimationUtil.CreatAnimationChar(character, g);
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
                ///////////////////////////////////////////
                //计算与字体相关的参数
                int ascent = animChar.Font.FontFamily.GetCellAscent(animChar.Font.Style);
                int descent = animChar.Font.FontFamily.GetCellDescent(animChar.Font.Style);
                int emHeight = animChar.Font.FontFamily.GetEmHeight(animChar.Font.Style);

                float ascentF = ascent * animChar.Font.Size / emHeight;
                float descentF = descent * animChar.Font.Size / emHeight;
                //计算绘制文字的开始位置
                float locY = (AnimationConfig.Height - (ascentF + descentF)) / 2;

                animChar.SetLocation(length, locY);
                animChar.CalcPath();
                //////////////////////////////////////////
                length += (charWidth + AnimationConfig.Space);
                LinkedListNode<AnimationChar> node = new LinkedListNode<AnimationChar>(animChar);
                linkedChars.AddLast(node);
            }

            if (linkedChars.Count > 0)
                screens.Add(linkedChars);

            /////Add annimation////
            if (screens.Count > 0)
            {
                int count = 0;
                float mid = 0;
                int index = 0;
                BlackHoleAnimation preAnimation = null;
                float fromScale = 0.05f;
                int ascent;
                float ascentf;
                float emHeight;
                foreach (LinkedList<AnimationChar> chars in screens)
                {
                    count = chars.Count;
                    mid = (count - 1) / 2.0f;
                    index = 0;
                    preAnimation = null;
                    fromScale = 0.05f;

                    LinkedListNode<AnimationChar> aNode = chars.First;
                    while (aNode != null)
                    { 
                        ascent = aNode.Value.Font.FontFamily.GetCellAscent(aNode.Value.Font.Style);
                        emHeight = aNode.Value.Font.FontFamily.GetEmHeight(aNode.Value.Font.Style);
                        ascentf = ascent * aNode.Value.Font.Size / emHeight;
                        BlackHoleAnimation animation = new BlackHoleAnimation(fromScale, 1.0f, aNode.Value.Location.X, aNode.Value.Location.Y + ascentf);
                        if (preAnimation != null)
                        {
                            preAnimation.NextAnimation = animation;
                        }
                        preAnimation = animation;
                        aNode.Value.Animation = animation;

                        index++;
                        if (index <= mid)
                        {
                            fromScale -= 0.05f;
                        }
                        else if (index > (int)(mid + 0.5f))
                        {
                            fromScale += 0.05f;
                        }
                        aNode = aNode.Next;
                    }
                }
            }
        }

        private void CreateHorizontalRotationAnimation(List<LinkedList<AnimationChar>> screens)
        {
            screens.Clear();
            LinkedList<AnimationChar> linkedChars = new LinkedList<AnimationChar>();//一屏中显示的字符

            string text = AnimationConfig.Text;
            int textSize = text.Length;

            int length = 0;//字符显示的长度
            int charWidth = 0;//单个字符的宽度
            for (int i = 0; i < textSize; i++)
            {
                string character = text.Substring(i, 1);
                AnimationChar animChar = AnimationUtil.CreatAnimationChar(character, g);
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
                ///////////////////////////////////////////
                //计算与字体相关的参数
                int ascent = animChar.Font.FontFamily.GetCellAscent(animChar.Font.Style);
                int descent = animChar.Font.FontFamily.GetCellDescent(animChar.Font.Style);
                int emHeight = animChar.Font.FontFamily.GetEmHeight(animChar.Font.Style);

                float ascentF = ascent * animChar.Font.Size / emHeight;
                float descentF = descent * animChar.Font.Size / emHeight;
                //计算绘制文字的开始位置
                float locY = (AnimationConfig.Height - (ascentF + descentF)) / 2;

                animChar.SetLocation(length, locY);
                animChar.CalcPath();
                //////////////////////////////////////////
                length += (charWidth + AnimationConfig.Space);
                LinkedListNode<AnimationChar> node = new LinkedListNode<AnimationChar>(animChar);
                linkedChars.AddLast(node);
            }

            if (linkedChars.Count > 0)
                screens.Add(linkedChars);

            if (screens.Count > 0)
            {
                float totalWidth;
                float mid;
                float fromX, toX;
                int ascent;
                float ascentf;
                float emHeight;

                foreach (LinkedList<AnimationChar> chars in screens)
                {
                    totalWidth = chars.Last.Value.Location.X + chars.Last.Value.Size.Width;
                    mid = totalWidth / 2.0f;
                    LinkedListNode<AnimationChar> aNode = chars.First;
                    while (aNode != null)
                    {
                        fromX = aNode.Value.Location.X;
                        toX = mid + (mid - (aNode.Value.Location.X + aNode.Value.Size.Width));
                        ascent = aNode.Value.Font.FontFamily.GetCellAscent(aNode.Value.Font.Style);
                        emHeight = aNode.Value.Font.FontFamily.GetEmHeight(aNode.Value.Font.Style);
                        ascentf = ascent * aNode.Value.Font.Size / emHeight;
                        HorizontalRotationAnimation animation = new HorizontalRotationAnimation(fromX, toX, 1.0f, 0.01f, aNode.Value.Location.Y + ascentf, 8);
                        aNode.Value.Animation = animation;
                        aNode = aNode.Next;
                    }
                }
            }
        }

        private void CreateTurnIntoAnimation(List<LinkedList<AnimationChar>> screens)
        {
            screens.Clear();
            LinkedList<AnimationChar> linkedChars = new LinkedList<AnimationChar>();//一屏中显示的字符

            string text = AnimationConfig.Text;
            int textSize = text.Length;

            int length = 0;//字符显示的长度
            int charWidth = 0;//单个字符的宽度
            for (int i = 0; i < textSize; i++)
            {
                string character = text.Substring(i, 1);
                AnimationChar animChar = AnimationUtil.CreatAnimationChar(character, g);
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
                ///////////////////////////////////////////
                //计算与字体相关的参数
                int ascent = animChar.Font.FontFamily.GetCellAscent(animChar.Font.Style);
                int descent = animChar.Font.FontFamily.GetCellDescent(animChar.Font.Style);
                int emHeight = animChar.Font.FontFamily.GetEmHeight(animChar.Font.Style);

                float ascentF = ascent * animChar.Font.Size / emHeight;
                float descentF = descent * animChar.Font.Size / emHeight;
                //计算绘制文字的开始位置
                float locY = (AnimationConfig.Height - (ascentF + descentF)) / 2;

                animChar.SetLocation(length, locY);
                animChar.CalcPath();
                //////////////////////////////////////////
                length += (charWidth + AnimationConfig.Space);
                LinkedListNode<AnimationChar> node = new LinkedListNode<AnimationChar>(animChar);
                linkedChars.AddLast(node);
            }

            if (linkedChars.Count > 0)
                screens.Add(linkedChars);

            if (screens.Count > 0)
            {
                float totalWidth;
                float mid;

                float newWidth;
                float newMid;

                float posX = 0;

                float fromX, toX;
                int index = 0;

                foreach (LinkedList<AnimationChar> chars in screens)
                {
                    totalWidth = chars.Last.Value.Location.X + chars.Last.Value.Size.Width;
                    mid = totalWidth / 2.0f;
                    
                    posX = 0;
                    index = 0;
                    float[] newLocationX = new float[chars.Count];
                    LinkedListNode<AnimationChar> aNode = chars.First;
                    while (aNode != null)
                    {
                        if (aNode.Previous != null)
                        {
                            posX = posX + (aNode.Value.Location.X - aNode.Previous.Value.Location.X) * 5;
                        }
                        newLocationX[index] = posX;

                        index++;
                        aNode = aNode.Next;
                    }

                    newWidth = 0;
                    newMid = 0;

                    newWidth = newLocationX[newLocationX.Length - 1] + chars.Last.Value.Size.Width;
                    newMid = newWidth / 2.0f;

                    float offset = newMid - mid;
                    if (offset == 0)
                        offset = chars.First.Value.Size.Width * 5;
                    index = 0;

                    aNode = chars.First;
                    while (aNode != null)
                    {
                        fromX = newLocationX[index] - offset;
                        toX = aNode.Value.Location.X;

                        aNode.Value.SetLocation(fromX, aNode.Value.Location.Y);
                        aNode.Value.CalcPath();
                        TurnIntoAnimation animation = new TurnIntoAnimation(fromX, toX, 0.8f, fromX, 0.05f, fromX, 65);
                        aNode.Value.Animation = animation;

                        aNode = aNode.Next;
                        index++;
                    }
                }
            }
        }

        private void CreateRotationIntoAnimation(List<LinkedList<AnimationChar>> screens)
        {
            screens.Clear();
            LinkedList<AnimationChar> linkedChars = new LinkedList<AnimationChar>();//一屏中显示的字符

            string text = AnimationConfig.Text;
            int textSize = text.Length;

            int length = 0;//字符显示的长度
            int charWidth = 0;//单个字符的宽度
            for (int i = 0; i < textSize; i++)
            {
                string character = text.Substring(i, 1);
                AnimationChar animChar = AnimationUtil.CreatAnimationChar(character, g);
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
                ///////////////////////////////////////////
                //计算与字体相关的参数
                int ascent = animChar.Font.FontFamily.GetCellAscent(animChar.Font.Style);
                int descent = animChar.Font.FontFamily.GetCellDescent(animChar.Font.Style);
                int emHeight = animChar.Font.FontFamily.GetEmHeight(animChar.Font.Style);

                float ascentF = ascent * animChar.Font.Size / emHeight;
                float descentF = descent * animChar.Font.Size / emHeight;
                //计算绘制文字的开始位置
                float locY = (AnimationConfig.Height - (ascentF + descentF)) / 2;

                animChar.SetLocation(length, locY);
                animChar.CalcPath();
                //////////////////////////////////////////
                length += (charWidth + AnimationConfig.Space);
                LinkedListNode<AnimationChar> node = new LinkedListNode<AnimationChar>(animChar);
                linkedChars.AddLast(node);
            }

            if (linkedChars.Count > 0)
                screens.Add(linkedChars);

            if (screens.Count > 0)
            {
                float totalWidth;
                float mid;

                float newWidth;
                float newMid;

                float posX = 0;

                float fromX, toX;
                int index = 0;

                int ascent;
                float ascentf;
                float emHeight;

                foreach (LinkedList<AnimationChar> chars in screens)
                {
                    totalWidth = chars.Last.Value.Location.X + chars.Last.Value.Size.Width;
                    mid = totalWidth / 2.0f;

                    posX = 0;
                    index = 0;
                    float[] newLocationX = new float[chars.Count];
                    LinkedListNode<AnimationChar> aNode = chars.First;
                    while (aNode != null)
                    {
                        if (aNode.Previous != null)
                        {
                            posX = posX + (aNode.Value.Location.X - aNode.Previous.Value.Location.X) * 5;
                        }
                        newLocationX[index] = posX;

                        index++;
                        aNode = aNode.Next;
                    }

                    newWidth = 0;
                    newMid = 0;

                    newWidth = newLocationX[newLocationX.Length - 1] + chars.Last.Value.Size.Width;
                    newMid = newWidth / 2.0f;

                    float offset = newMid - mid;
                    if (offset == 0)
                        offset = chars.First.Value.Size.Width * 5;
                    index = 0;

                    aNode = chars.First;
                    while (aNode != null)
                    {
                        fromX = newLocationX[index] - offset;
                        toX = aNode.Value.Location.X;

                        ascent = aNode.Value.Font.FontFamily.GetCellAscent(aNode.Value.Font.Style);
                        emHeight = aNode.Value.Font.FontFamily.GetEmHeight(aNode.Value.Font.Style);
                        ascentf = ascent * aNode.Value.Font.Size / emHeight;

                        PointF pivot = new PointF();
                        pivot.X = fromX + aNode.Value.Size.Width / 2;
                        pivot.Y = aNode.Value.Location.Y + ascentf / 2;

                        aNode.Value.SetLocation(fromX, aNode.Value.Location.Y);
                        aNode.Value.CalcPath();
                        RotationIntoAnimation animation = new RotationIntoAnimation(fromX, toX, pivot, 80);
                        aNode.Value.Animation = animation;

                        aNode = aNode.Next;
                        index++;
                    }
                }
            }
        }

        private void CreateTextScalingAnimation(List<LinkedList<AnimationChar>> screens)
        {
            screens.Clear();
            LinkedList<AnimationChar> linkedChars = new LinkedList<AnimationChar>();//一屏中显示的字符

            string text = AnimationConfig.Text;
            int textSize = text.Length;

            int length = 0;//字符显示的长度
            int charWidth = 0;//单个字符的宽度
            for (int i = 0; i < textSize; i++)
            {
                string character = text.Substring(i, 1);
                AnimationChar animChar = AnimationUtil.CreatAnimationChar(character, g);
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
                ///////////////////////////////////////////
                //计算与字体相关的参数
                int ascent = animChar.Font.FontFamily.GetCellAscent(animChar.Font.Style);
                int descent = animChar.Font.FontFamily.GetCellDescent(animChar.Font.Style);
                int emHeight = animChar.Font.FontFamily.GetEmHeight(animChar.Font.Style);

                float ascentF = ascent * animChar.Font.Size / emHeight;
                float descentF = descent * animChar.Font.Size / emHeight;
                //计算绘制文字的开始位置
                float locY = (AnimationConfig.Height - (ascentF + descentF)) / 2;

                animChar.SetLocation(length, locY);
                animChar.CalcPath();
                //////////////////////////////////////////
                length += (charWidth + AnimationConfig.Space);
                LinkedListNode<AnimationChar> node = new LinkedListNode<AnimationChar>(animChar);
                linkedChars.AddLast(node);
            }

            if (linkedChars.Count > 0)
                screens.Add(linkedChars);

            if (screens.Count > 0)
            {
                float totalWidth;
                float mid;
                float fromX;
                float toX;
                int ascent;
                float ascentf;
                float emHeight;

                foreach (LinkedList<AnimationChar> chars in screens)
                {
                    totalWidth = chars.Last.Value.Location.X + chars.Last.Value.Size.Width;
                    mid = totalWidth / 2.0f;
                    LinkedListNode<AnimationChar> aNode = chars.First;
                    while (aNode != null)
                    {
                        //fromX = aNode.Value.Location.X;
                        toX = aNode.Value.Location.X;
                        fromX = mid - aNode.Value.Size.Width / 2;

                        aNode.Value.SetLocation(fromX, aNode.Value.Location.Y);
                        aNode.Value.CalcPath();

                        ascent = aNode.Value.Font.FontFamily.GetCellAscent(aNode.Value.Font.Style);
                        emHeight = aNode.Value.Font.FontFamily.GetEmHeight(aNode.Value.Font.Style);
                        ascentf = ascent * aNode.Value.Font.Size / emHeight;

                        TextScalingAnimation animation = new TextScalingAnimation(fromX, toX, fromX, aNode.Value.Location.Y + ascentf);
                        aNode.Value.Animation = animation;
                        aNode = aNode.Next;
                    }
                }
            }
        }

        private void CreateFrogAnimation(List<LinkedList<AnimationChar>> screens, bool show)
        {
            screens.Clear();
            LinkedList<AnimationChar> linkedChars = new LinkedList<AnimationChar>();//一屏中显示的字符

            string text = AnimationConfig.Text;
            int textSize = text.Length;

            int length = 0;//字符显示的长度
            int charWidth = 0;//单个字符的宽度
            for (int i = 0; i < textSize; i++)
            {
                string character = text.Substring(i, 1);
                AnimationChar animChar = AnimationUtil.CreatAnimationChar(character, g);
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
                ///////////////////////////////////////////
                //计算与字体相关的参数
                int ascent = animChar.Font.FontFamily.GetCellAscent(animChar.Font.Style);
                int descent = animChar.Font.FontFamily.GetCellDescent(animChar.Font.Style);
                int emHeight = animChar.Font.FontFamily.GetEmHeight(animChar.Font.Style);

                float ascentF = ascent * animChar.Font.Size / emHeight;
                float descentF = descent * animChar.Font.Size / emHeight;
                //计算绘制文字的开始位置
                float locY = (AnimationConfig.Height - (ascentF + descentF)) / 2;

                FrogAnimation animation = new FrogAnimation(locY + ascentF, show);

                animChar.SetLocation(length, locY);
                animChar.CalcPath();
                animChar.Animation = animation;
                //////////////////////////////////////////
                length += (charWidth + AnimationConfig.Space);
                LinkedListNode<AnimationChar> node = new LinkedListNode<AnimationChar>(animChar);
                linkedChars.AddLast(node);
            }

            if (linkedChars.Count > 0)
                screens.Add(linkedChars);
        }

        private void CreateContinousMovementLeftAnimation(List<LinkedList<AnimationChar>> screens)
        {
            screens.Clear();
            LinkedList<AnimationChar> linkedChars = new LinkedList<AnimationChar>();//一屏中显示的字符

            string text = AnimationConfig.Text;
            int textSize = text.Length;
            int length = 0;//字符显示的长度
            int charWidth = 0;//单个字符的宽度
            for (int i = 0; i < textSize; )
            {
                string character = text.Substring(i, 1);
                AnimationChar animChar = AnimationUtil.CreatAnimationChar(character, g);
                charWidth = Convert.ToInt32(animChar.Size.Width);
                ///////////////////////////////////////////
                //计算与字体相关的参数
                int ascent = animChar.Font.FontFamily.GetCellAscent(animChar.Font.Style);
                int descent = animChar.Font.FontFamily.GetCellDescent(animChar.Font.Style);
                int emHeight = animChar.Font.FontFamily.GetEmHeight(animChar.Font.Style);

                float ascentF = ascent * animChar.Font.Size / emHeight;
                float descentF = descent * animChar.Font.Size / emHeight;
                //计算绘制文字的开始位置
                float locY = (AnimationConfig.Height - (ascentF + descentF)) / 2;

                animChar.SetLocation(length + AnimationConfig.Width, locY);
                animChar.CalcPath();
                //////////////////////////////////////////
                length += (charWidth + AnimationConfig.Space);
                LinkedListNode<AnimationChar> node = new LinkedListNode<AnimationChar>(animChar);
                linkedChars.AddLast(node);

                if (i == textSize - 1)
                {
                    i = 0;
                    if (length > AnimationConfig.Width * 1.5f)//如果字不够一屏补全一屏
                        break;
                }
                else
                {
                    i++;
                }
            }

            if (linkedChars.Count > 0)
                screens.Add(linkedChars);

            //设置动画
            if (screens.Count > 0)
            {
                LinkedListNode<AnimationChar> aNode = screens[0].First;
                while (aNode != null)
                {
                    PointF from = new PointF();
                    PointF to = new PointF();
                    PointF init = new PointF();

                    init.X = aNode.Value.Location.X;
                    init.Y = aNode.Value.Location.Y;

                    to.X = -(aNode.Value.Size.Width + AnimationConfig.Space);
                    to.Y = aNode.Value.Location.Y;

                    from.X = to.X + length;
                    from.Y = aNode.Value.Location.Y;

                    ContinuouseMovementAnimation animation = new ContinuouseMovementAnimation(init, from, to);
                    aNode.Value.Animation = animation;

                    aNode = aNode.Next;
                }
            }
        }

        private void CreateContinousMovementRightAnimation(List<LinkedList<AnimationChar>> screens)
        {
            screens.Clear();
            LinkedList<AnimationChar> linkedChars = new LinkedList<AnimationChar>();//一屏中显示的字符

            string text = AnimationConfig.Text;
            int textSize = text.Length;
            int length = 0;//字符显示的长度
            int charWidth = 0;//单个字符的宽度
            for (int i = textSize - 1; i >= 0; )
            {
                string character = text.Substring(i, 1);
                AnimationChar animChar = AnimationUtil.CreatAnimationChar(character, g);
                charWidth = Convert.ToInt32(animChar.Size.Width);
                ///////////////////////////////////////////
                //计算与字体相关的参数
                int ascent = animChar.Font.FontFamily.GetCellAscent(animChar.Font.Style);
                int descent = animChar.Font.FontFamily.GetCellDescent(animChar.Font.Style);
                int emHeight = animChar.Font.FontFamily.GetEmHeight(animChar.Font.Style);

                float ascentF = ascent * animChar.Font.Size / emHeight;
                float descentF = descent * animChar.Font.Size / emHeight;
                //计算绘制文字的开始位置
                float locY = (AnimationConfig.Height - (ascentF + descentF)) / 2;

                animChar.SetLocation(length - charWidth, locY);
                animChar.CalcPath();
                //////////////////////////////////////////
                length -= (charWidth + AnimationConfig.Space);
                LinkedListNode<AnimationChar> node = new LinkedListNode<AnimationChar>(animChar);
                linkedChars.AddLast(node);

                if (i == 0)
                {
                    i = textSize - 1;
                    if (Math.Abs(length) > AnimationConfig.Width * 1.5f)//如果字不够一屏补全一屏
                        break;
                }
                else
                {
                    i--;
                }
            }

            if (linkedChars.Count > 0)
                screens.Add(linkedChars);

            //设置动画
            if (screens.Count > 0)
            {
                LinkedListNode<AnimationChar> aNode = screens[0].First;
                while (aNode != null)
                {
                    PointF from = new PointF();
                    PointF to = new PointF();
                    PointF init = new PointF();

                    init.X = aNode.Value.Location.X;
                    init.Y = aNode.Value.Location.Y;

                    to.X = AnimationConfig.Width;
                    to.Y = aNode.Value.Location.Y;

                    from.X = to.X + length;
                    from.Y = aNode.Value.Location.Y;

                    ContinuouseMovementAnimation animation = new ContinuouseMovementAnimation(init, from, to);
                    aNode.Value.Animation = animation;

                    aNode = aNode.Next;
                }
            }
        }

        private void CreateContinousMovementBottomAnimation(List<LinkedList<AnimationChar>> screens)
        {
            screens.Clear();
            LinkedList<AnimationChar> linkedChars = new LinkedList<AnimationChar>();//一屏中显示的字符

            string text = AnimationConfig.Text;
            int textSize = text.Length;
            int length = 0;//字符显示的长度
            int charWidth = 0;//单个字符的宽度
            for (int i = textSize - 1; i >= 0; )
            {
                string character = text.Substring(i, 1);
                AnimationChar animChar = AnimationUtil.CreatAnimationChar(character, g);
                charWidth = Convert.ToInt32(animChar.Size.Width);
                ///////////////////////////////////////////
                //计算与字体相关的参数
                int ascent = animChar.Font.FontFamily.GetCellAscent(animChar.Font.Style);
                int descent = animChar.Font.FontFamily.GetCellDescent(animChar.Font.Style);
                int emHeight = animChar.Font.FontFamily.GetEmHeight(animChar.Font.Style);

                float ascentF = ascent * animChar.Font.Size / emHeight;
                float descentF = descent * animChar.Font.Size / emHeight;

                animChar.SetLocation((AnimationConfig.Width - charWidth) / 2, length - (ascentF + descentF));
                animChar.CalcPath();
                //////////////////////////////////////////
                length -= (int)(ascentF + descentF + AnimationConfig.Space);
                LinkedListNode<AnimationChar> node = new LinkedListNode<AnimationChar>(animChar);
                linkedChars.AddLast(node);

                if (i == 0)
                {
                    i = textSize - 1;
                    if (Math.Abs(length) > AnimationConfig.Height * 1.5f)//如果字不够一屏补全一屏
                        break;
                }
                else
                {
                    i--;
                }
            }

            if (linkedChars.Count > 0)
                screens.Add(linkedChars);

            //设置动画
            if (screens.Count > 0)
            {
                LinkedListNode<AnimationChar> aNode = screens[0].First;
                while (aNode != null)
                {
                    PointF from = new PointF();
                    PointF to = new PointF();
                    PointF init = new PointF();

                    init.X = aNode.Value.Location.X;
                    init.Y = aNode.Value.Location.Y;

                    to.X = aNode.Value.Location.X;
                    to.Y = AnimationConfig.Height;

                    from.X = aNode.Value.Location.X;
                    from.Y = to.Y + length;

                    ContinuouseMovementAnimation animation = new ContinuouseMovementAnimation(init, from, to);
                    aNode.Value.Animation = animation;

                    aNode = aNode.Next;
                }
            }
        }

        private void CreateContinousMovementTopAnimation(List<LinkedList<AnimationChar>> screens)
        {
            screens.Clear();
            LinkedList<AnimationChar> linkedChars = new LinkedList<AnimationChar>();//一屏中显示的字符

            string text = AnimationConfig.Text;
            int textSize = text.Length;
            int length = 0;//字符显示的长度
            int charWidth = 0;//单个字符的宽度
            for (int i = 0; i < textSize; )
            {
                string character = text.Substring(i, 1);
                AnimationChar animChar = AnimationUtil.CreatAnimationChar(character, g);
                charWidth = Convert.ToInt32(animChar.Size.Width);
                ///////////////////////////////////////////
                //计算与字体相关的参数
                int ascent = animChar.Font.FontFamily.GetCellAscent(animChar.Font.Style);
                int descent = animChar.Font.FontFamily.GetCellDescent(animChar.Font.Style);
                int emHeight = animChar.Font.FontFamily.GetEmHeight(animChar.Font.Style);

                float ascentF = ascent * animChar.Font.Size / emHeight;
                float descentF = descent * animChar.Font.Size / emHeight;

                animChar.SetLocation((AnimationConfig.Width - charWidth) / 2, length + AnimationConfig.Height);
                animChar.CalcPath();
                //////////////////////////////////////////
                length += (int)(ascentF + descentF + AnimationConfig.Space);
                LinkedListNode<AnimationChar> node = new LinkedListNode<AnimationChar>(animChar);
                linkedChars.AddLast(node);

                if (i == textSize - 1)
                {
                    i = 0;
                    if (length > AnimationConfig.Height * 1.5f)//如果字不够一屏补全一屏
                        break;
                }
                else
                {
                    i++;
                }
            }

            if (linkedChars.Count > 0)
                screens.Add(linkedChars);

            //设置动画
            if (screens.Count > 0)
            {
                LinkedListNode<AnimationChar> aNode = screens[0].First;
                while (aNode != null)
                {
                    PointF from = new PointF();
                    PointF to = new PointF();
                    PointF init = new PointF();

                    init.X = aNode.Value.Location.X;
                    init.Y = aNode.Value.Location.Y;

                    to.X = aNode.Value.Location.X;
                    to.Y = -(aNode.Value.Size.Height + AnimationConfig.Space);

                    from.X = aNode.Value.Location.X;
                    from.Y = to.Y + length;

                    ContinuouseMovementAnimation animation = new ContinuouseMovementAnimation(init, from, to);
                    aNode.Value.Animation = animation;

                    aNode = aNode.Next;
                }
            }
        }
    }
}
