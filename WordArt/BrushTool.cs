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

        private Color[] sweepColors;

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

            if (!AnimationConfig.ColorfulEnabled)//炫彩未选中
            {
                //brush = Brushes.White;
                brush = new SolidBrush(AnimationConfig.TextColor);
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
                        //CreateSweepGradient(blend);
                        //CreateSweepGradient2(blend);
                        CreateSweepGradient3(blend);
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
                    //brush = Brushes.White;
                    brush = new SolidBrush(AnimationConfig.TextColor);
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
                else
                {
                    //Color c = sweepColors[sweepColors.Length - 1];
                    //for (int i = sweepColors.Length - 1; i > 0; i--)
                    //{
                    //    sweepColors[i] = sweepColors[i - 1];
                    //}
                    //sweepColors[0] = c;
                    PathGradientBrush br = brush as PathGradientBrush;
                    //br.SurroundColors = sweepColors;
                    Matrix trans = br.Transform;
                    trans.RotateAt(2, new PointF(AnimationConfig.Width / 2, AnimationConfig.Height), MatrixOrder.Append);
                    br.Transform = trans;
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

        private void CreateSweepGradient2(ColorBlend blend)
        {
            List<PointF> points = new List<PointF>();
            List<Color> colors = new List<Color>();

            PointF center = new PointF(AnimationConfig.Width / 2.0F, AnimationConfig.Height);
            double radius = Math.Sqrt(center.X * center.X + center.Y * center.Y);

            double colorStep = 10;
            for (double angle = 0; angle < 360; angle += colorStep)
            {

                double angleR = angle * (Math.PI / 180);
                PointF location = GetColorLocation(center, angleR, radius);

                points.Add(location);

//                 Color color;
//                 if (angle >= 0 && angle <= 60)
//                 {
//                     color = Color.Red;
//                 }
//                 else if (angle > 60 && angle <= 120)
//                 {
//                     color = Color.Blue;
//                 }
//                 else if (angle > 120 && angle <= 180)
//                 {
//                     color = Color.Green;
//                 }
//                 else if (angle > 180 && angle <= 240)
//                 {
//                     color = Color.Green;
//                 }
//                 else if (angle > 240 && angle <= 300)
//                 {
//                     color = Color.Blue;
//                 }
//                 else
//                 {
//                     color = Color.Red;
//                 }
//                 colors.Add(color);

                colors.Add(GetColor(angle, 1, 0.5));
                
            }

            PathGradientBrush tempBrush = new PathGradientBrush(points.ToArray(), WrapMode.Clamp);
            tempBrush.CenterPoint = center;
            tempBrush.CenterColor = Color.Transparent;
            tempBrush.SurroundColors = colors.ToArray();

            brush = tempBrush;
            pen = new Pen(tempBrush);
        }

        private PointF GetColorLocation(PointF centerPoint, double angleR, double radius)
        {
            double x, y;

            x = centerPoint.X + Math.Cos(angleR) * radius;
            y = centerPoint.Y - Math.Sin(angleR) * radius;
            return new PointF((float)x, (float) y);
        }

        private void CreateSweepGradient3(ColorBlend blend)
        {
            GraphicsPath path = new GraphicsPath();
            int width = AnimationConfig.Width;
            int height = AnimationConfig.Height;
            int halfWidth = width / 2;
            int halfHeight = height / 2;
            int quarterWidth = width / 4;
            int quarterHeight = height / 3;


            //path.AddLine(0, height, 0, halfHeight);
            //path.AddLine(0, halfHeight, 0, 0);
            //path.AddLine(0, 0, halfWidth, 0);
            //path.AddLine(halfWidth, 0, width, 0);
            //path.AddLine(width, 0, width, halfHeight);
            //path.AddLine(width, halfHeight, width, height);

            //path.AddLine(0, height, 0, height - quarterHeight);
            //path.AddLine(0, height - quarterHeight, 0, halfHeight);
            //path.AddLine(0, halfHeight, 0, quarterHeight);
            //path.AddLine(0, quarterHeight, 0, 0);

            //path.AddLine(0, 0, quarterWidth, 0);
            //path.AddLine(quarterWidth, 0, halfWidth, 0);
            //path.AddLine(halfWidth, 0, width - quarterWidth, 0);
            //path.AddLine(width - quarterWidth, 0, width, 0);

            //path.AddLine(width, 0, width, quarterHeight);
            //path.AddLine(width, quarterHeight, width, halfHeight);
            //path.AddLine(width, halfHeight, width, height - quarterHeight);
            //path.AddLine(width, height - quarterHeight, width, height);

            List<PointF> points = new List<PointF>();
            List<Color> colors = new List<Color>();

            PointF center = new PointF(AnimationConfig.Width / 2.0F, AnimationConfig.Height);
            double radius = Math.Sqrt(center.X * center.X + center.Y * center.Y);
            double colorStep = 10;
            for (double angle = 0; angle < 360; angle += colorStep)
            {

                double angleR = angle * (Math.PI / 180);
                PointF location = GetColorLocation(center, angleR, radius);
                points.Add(location);
            }
            path.AddLines(points.ToArray());


            int size = path.PointCount;
            Color[] baseColor = new Color[] { Color.Green, Color.Yellow, Color.Red, Color.Blue };
            sweepColors = new Color[size];
            int colorIndex = 0;
            int colorIndexStep = 0;
            for (int i = 0; i < size; i++)
            {
                sweepColors[i] = baseColor[colorIndex];
                colorIndexStep++;
                if (colorIndexStep == 2)
                {
                    colorIndexStep = 0;
                    colorIndex++;
                    if (colorIndex == baseColor.Length)
                    {
                        colorIndex = 0;
                    }
                }
            }

            PathGradientBrush tempBrush = new PathGradientBrush(path);
            tempBrush.WrapMode = WrapMode.Tile;
            tempBrush.SurroundColors = sweepColors;
            tempBrush.CenterColor = Color.FromArgb(50, 250, 250, 250);
            tempBrush.CenterPoint = center;
            
            brush = tempBrush;
            pen = new Pen(tempBrush);
        }

        private Color GetColor(double hue, double saturation, double lightness)
        {
            double h = Math.Min(359, hue);
            double s = Math.Min(1, saturation);
            double l = Math.Min(1, lightness);

            double q;
            if (l < 0.5)
            {
                q = l * (1 + s);
            }
            else
            {
                q = l + s - l * s;
            }
            double p = 2 * l - q;
            double hk = h / 360;

            double[] tc = { hk + 1d / 3d, hk, hk - 1d / 3d };
            double[] colors = { 0.0, 0.0, 0.0 };
            for (int color = 0; color < colors.Length; color++)
            {
                if (tc[color] < 0)
                {
                    tc[color] += 1;
                }
                if (tc[color] > 1)
                {
                    tc[color] -= 1;
                }

                if (tc[color] < 1d / 6d)
                {
                    colors[color] = p + (q - p) * 6 * tc[color];
                }
                else if (tc[color] >= 1d / 6d && tc[color] < 1d / 2d)
                {
                    colors[color] = q;
                }
                else if (tc[color] >= 1d / 2d && tc[color] < 2d / 3d)
                {
                    colors[color] = p + (q - p) * 6 * (2d / 3d - tc[color]);
                }
                else
                {
                    colors[color] = p;
                }

                colors[color] *= 255;
            }

            return Color.FromArgb(255, (int)colors[0], (int)colors[1], (int)colors[2]);
        }
    }
}
