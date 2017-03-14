using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordArt.Animation;
using WordArt.Config;

namespace WordArt
{
    class BrushTool
    {
        private const int TRANSLATE_VALUE = -10;
        private Color[] colors = new Color[5];
        private float[] positions;
        private ColorBlend blend = new ColorBlend();
        private Brush brush;

        public Brush Brush
        {
            get { SetBrush(); return brush; }
        }

        private Pen pen;

        public Pen Pen
        {
            get { return pen; }
        }

        public BrushTool()
        {
            
        }

        public void InitBrush()
        {
            colors[0] = Color.Red;
            colors[1] = Color.Yellow;
            colors[2] = Color.Green;
            colors[3] = Color.Blue;
            colors[4] = Color.Red;

            positions = new float[] { 0.0f, 0.2f, 0.5f, 0.8f, 1.0f };

            blend.Positions = positions;
            blend.Colors = colors;

            if (!AnimationConfig.ColorfulEnabled)//炫彩为选中
            {
                brush = Brushes.White;
                pen = new Pen(brush);
                return;
            }

            if (AnimationConfig.IsColorGradient)
            {
                switch (AnimationConfig.ColorGradientType)
                {
                    case ColorGradientType.LINEAR_GRADIENT:
                        CreateLinearGradientBrush(blend);
                        break;
                    case ColorGradientType.RADIAL_GRADIENT:
                        CreatePathGradientBrush(blend);
                        break;
                    case ColorGradientType.SWEEP_GRADIENT:
                        CreateSweepGradient(blend);
                        break;
                }
            }
            else
            {
                Image srcImg = null;

                try
                {
                    srcImg = Bitmap.FromFile(AnimationConfig.FillBitmapPath);
                }
                catch (System.IO.FileNotFoundException){ }
                catch (System.OutOfMemoryException){ }
                catch (System.ArgumentException){ }

                if (srcImg != null)
                {
                    Bitmap bitmap = ResizeBitmap(srcImg);
                    srcImg.Dispose();
                    brush = new TextureBrush(bitmap, WrapMode.Tile);
                    pen = new Pen(brush);
                }
                else
                {
                    brush = Brushes.White;
                    pen = new Pen(brush);   
                }
            }

        }

        private Bitmap ResizeBitmap(Image srcImg)
        {
            Bitmap bitmap = new Bitmap(AnimationConfig.Width, AnimationConfig.Height);
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(Color.Transparent);
            g.DrawImage(srcImg, 0, 0, AnimationConfig.Width, AnimationConfig.Height);
            g.Dispose();
            return bitmap;
        }

        private void SetBrush()
        {
            if (!AnimationConfig.IsColorGradient)
                return;

            if (brush is LinearGradientBrush)
            {
                LinearGradientBrush br = brush as LinearGradientBrush;
                br.TranslateTransform(-TRANSLATE_VALUE, 0);
            }
            else if (brush is PathGradientBrush)
            {
                if (AnimationConfig.ColorGradientType == ColorGradientType.RADIAL_GRADIENT)
                {
                    Color c = colors[colors.Length - 1];
                    for (int i = colors.Length - 1; i > 0; i--)
                    {
                        colors[i] = colors[i - 1];
                    }

                    colors[0] = c;
                    blend.Colors = colors;
                    blend.Positions = positions;
                    PathGradientBrush br = brush as PathGradientBrush;
                    br.InterpolationColors = blend;
                }
            }
        }

        public void Dispose()
        {
            if (brush != null)
            {
                brush.Dispose();
                brush = null;
            }

            if (pen != null)
            {
                pen.Dispose();
                pen = null;
            }
        }

        /// <summary>
        /// 创建线性画刷
        /// </summary>
        private void CreateLinearGradientBrush(ColorBlend blend)
        {
            Rectangle rect = new Rectangle(0, 0, AnimationConfig.Width, AnimationConfig.Height);
            LinearGradientBrush tempBrush = new LinearGradientBrush(rect, Color.Transparent, Color.Transparent, LinearGradientMode.Horizontal);

            tempBrush.InterpolationColors = blend;

            brush = tempBrush;
            pen = new Pen(brush);
        }

        private void CreatePathGradientBrush(ColorBlend blend)
        {
            GraphicsPath path = new GraphicsPath();
            float halfW = AnimationConfig.Width / 2;
            float halfH = AnimationConfig.Height / 2;
            float halfSize = (float) Math.Sqrt(halfW * halfW + halfH * halfH);
            float x = 0 - (halfSize - halfW);
            float y = 0 - (halfSize - halfH);
            path.AddEllipse(x, y, halfSize * 2, halfSize * 2);
            PathGradientBrush tempBrush = new PathGradientBrush(path);

            tempBrush.InterpolationColors = blend;

            //tempBrush.CenterColor = Color.Red;
            //tempBrush.SurroundColors = new Color[]{Color.Green, Color.Blue};

            brush = tempBrush;
            pen = new Pen(brush);
        }

        private void CreateSweepGradient(ColorBlend blend)
        {
            GraphicsPath path = new GraphicsPath();
            //Point[] points = new Point[] {
            //    new Point(0, 0), new Point(AnimationConfig.Width, 0),
            //    new Point(AnimationConfig.Width, AnimationConfig.Height), new Point(0, AnimationConfig.Height)
            //};
            //path.AddPolygon(points);

            //PathGradientBrush tempBrush = new PathGradientBrush(path);
            //tempBrush.CenterPoint = new PointF(AnimationConfig.Width / 2, AnimationConfig.Height);
            //tempBrush.CenterColor = Color.Red;
            //tempBrush.SurroundColors = new Color[]{Color.Pink, Color.Blue, Color.Yellow, Color.Cyan};

            path.AddPie(0, 0, AnimationConfig.Width, AnimationConfig.Height, 0, 45);
            path.AddPie(0, 0, AnimationConfig.Width, AnimationConfig.Height, 0, -45);
           
            PathGradientBrush tempBrush = new PathGradientBrush(path);
            tempBrush.CenterPoint = new PointF(AnimationConfig.Width / 2, AnimationConfig.Height / 2);
            tempBrush.CenterColor = Color.Red;
            tempBrush.SurroundColors = new Color[] { Color.Blue, Color.Yellow};

            brush = tempBrush;
            pen = new Pen(brush);
        }
    }
}
