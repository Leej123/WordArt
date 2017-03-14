using WordArt.Config;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordArt.Animation;

namespace WordArt
{
    /// <summary>
    /// 单个动画字符
    /// </summary>
    public class AnimationChar
    {
        //字符
        private string character;

        /// <summary>
        /// 设置动画字符
        /// </summary>
        public string Character
        {
            get { return character; }
            set { character = value; CalcPath(); }
        }

        //该字符使用的字体
        private Font font;
        /// <summary>
        /// 该字符使用的字体
        /// </summary>
        public Font Font
        {
            get { return font; }
            set { font = value; }
        }

        //开始位置
        private PointF loc = new PointF();

        public PointF Location
        {
            get { return loc; }
        }

        /// <summary>
        /// 字符的原始路径
        /// </summary>
        private GraphicsPath path;

        //字符的大小
        private SizeF size;

        /// <summary>
        /// 字符的大小，此大小为绘制字符需要的空间。不等于字符路径的大小。
        /// </summary>
        public SizeF Size
        {
            get { return size; }
            set { size = value; }
        }

        /// <summary>
        /// 变化矩阵，实现动画的根本
        /// </summary>
        private Matrix transform = new Matrix();

        // 动画
        private TextAnimation animation;

        public TextAnimation Animation
        {
            get { return animation; }
            set { animation = value; }
        }

        public AnimationChar(string character)
        {
            this.character = character;
        }

        public GraphicsPath GetDrawPath()
        {
            GraphicsPath p;
            if (null != animation) //使用了动画
            {
                bool stillRun = false;
                transform = animation.GetTransformation(ref stillRun);//获得变换

                p = new GraphicsPath(path.PathPoints, path.PathTypes);
                p.Transform(transform);

            }
            else
            {
                p = path;
            }

            return p;
        }

        /// <summary>
        /// 绘制字符
        /// </summary>
        /// <param name="brush">填充模式时使用</param>
        /// <param name="pen">空心模式时使用</param>
        /// <param name="g"></param>
        public void Draw(Brush brush, Pen pen, Graphics g)
        {
            GraphicsPath p;
            if (null != animation) //使用了动画
            {
                bool stillRun = false;
                transform = animation.GetTransformation(ref stillRun);//获得变换

                p = new GraphicsPath(path.PathPoints, path.PathTypes);
                p.Transform(transform);

            }
            else
            {
                p = path; 
            }

            RectangleF bounds = p.GetBounds();
            if ( bounds.X >= AnimationConfig.Width
                || bounds.Y > AnimationConfig.Height
                || (bounds.X + bounds.Width <= 0)
                || (bounds.Y + bounds.Height <= 0))//超过视图范围，不进行绘制
            {
                return;
            }

            if (AnimationConfig.IsFillMode)
                g.FillPath(brush, p);
            else
                g.DrawPath(pen, p);

            //Pen pen2 = new Pen(Brushes.White);
            //g.DrawLine(pen2, 0, AnimationConfig.Height / 2, AnimationConfig.Width, AnimationConfig.Height / 2);
            //g.DrawLine(pen2, 0, loc.Y, AnimationConfig.Width, loc.Y);

            ////计算与字体相关的参数
            //int ascent = Font.FontFamily.GetCellAscent(Font.Style);
            //int descent = Font.FontFamily.GetCellDescent(Font.Style);
            ////int emHeight = animChar.Font.FontFamily.GetEmHeight(animChar.Font.Style);
            //int space = Font.FontFamily.GetLineSpacing(Font.Style);

            //float height = Font.GetHeight();
            //float ascentF = ascent * height / space;
            //float descentF = descent * height / space;
            //float dHeight = ascentF + descentF;

            //g.DrawLine(new Pen(Brushes.Red), 0, loc.Y + ascentF, AnimationConfig.Width, loc.Y + ascentF);
            //g.DrawLine(new Pen(Brushes.Pink), 0, loc.Y + dHeight, AnimationConfig.Width, loc.Y + dHeight);

            //g.Transform = transform;
            //g.DrawRectangle(new Pen(Brushes.Purple), loc.X, loc.Y, size.Width, size.Height);
            //g.ResetTransform();
        }

        /// <summary>
        /// 计算字体的路径
        /// </summary>
        public void CalcPath()
        {
            if (null == path)
                path = new GraphicsPath();
            path.Reset();
            path.AddString(character, Font.FontFamily, (int)Font.Style, (int)Font.Size, loc, StringFormat.GenericTypographic);
        }

        /// <summary>
        /// 设置字符绘制的位置
        /// </summary>
        /// <param name="locX"></param>
        /// <param name="locY"></param>
        public void SetLocation(float locX, float locY)
        {
            loc.X = locX;
            loc.Y = locY;
        }
    }
}
