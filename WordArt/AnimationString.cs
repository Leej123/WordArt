using WordArt.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using WordArt.Animation;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using WordArt.Gif;
using System.Drawing.Imaging;
using System.IO;

namespace WordArt
{
    public class DrawEventArgs : EventArgs
    {
        private Bitmap bitmap;
        public Bitmap Bitmap
        {
            get { return bitmap; }
        }

        public DrawEventArgs(Bitmap bitmap)
        {
            this.bitmap = bitmap;
        }

        public byte[] GetBitmapData()
        {
            if (null == bitmap)
                return null;
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);
                byte[] data = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(data, 0, Convert.ToInt32(stream.Length));
                return data;
            }
        }

        public int GetBitmapData(byte[] data)
        {
            if (null == bitmap || data == null)
                return 0;
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);
                stream.Seek(0, SeekOrigin.Begin);

                int length = (int) Math.Min(data.Length, stream.Length);
                stream.Read(data, 0, length);
                return length;
            }
        }
    }

    public delegate void DrawEvent(object sender, DrawEventArgs args);

    /// <summary>
    /// 绘制文本的动画
    /// </summary>
    public class AnimationString
    {
        //动画配置
        private AnimationConfig config;
        /// <summary>
        /// 应用到动画字的配置
        /// </summary>
        public AnimationConfig AnimationConfig
        {
            get { return config; }
        }

        private List<LinkedList<AnimationChar>> screens;// = new List<LinkedList<AnimationChar>>();//文字长度超过视图的宽度是，需要分屏显示。

        /// <summary>
        /// 分屏索引
        /// </summary>
        private int screenindex = 0;
        private Object obj = new Object();

        private Bitmap bitmap;
        private bool isDisposed = false;

        //文本输出质量：呈现模式和平滑效果
        private TextRenderingHint textRenderingHint = TextRenderingHint.ClearTypeGridFit;
        private SmoothingMode smoothingMode = SmoothingMode.AntiAlias;

        /// <summary>
        /// 动画工厂，复杂动画的创建
        /// </summary>
        private AnimationFactory animationFactory;

        /// <summary>
        /// 是并行动画还是串行动画。true为平行动画，false为串行动画
        /// </summary>
        private bool isParallelAnimation = false;

        private Image[] frog = null;
        private int frogIndex = 0;

        public event DrawEvent DrawBitmap;

        private StringAnimator animator;
        private AnimatedGifEncoder gifEncoder;
        private GifDecoder gifDecoder;
        private int gifFrameIndex = -1;
        private string neonGifPath;
        public string NenoGifPath
        {
            set 
            {
                gifFrameIndex = -1;
                neonGifPath = value;
                if (gifDecoder == null)
                {
                    gifDecoder = new GifDecoder();
                }
                else
                {
                    int size = gifDecoder.GetFrameCount();
                    for (int i = 0; i < size; i++)
                    {
                        gifDecoder.GetFrame(i).Dispose();
                    }
                }
                gifDecoder.Read(value);
                gifFrameIndex = 0;
            }
        }

        /// <summary>
        /// 霓虹gif背景
        /// </summary>
        private Image nenoGifImg;
        private FrameDimension fd;

        //画刷和画笔
        private BrushTool brushTool;

        private Graphics g;

        /// <summary>
        /// 绘制的区域
        /// </summary>
        private Region region = new Region();

        /// <summary>
        /// 根据动画配置创建文本动画。
        /// </summary>
        /// <param name="config"></param>
        public AnimationString(AnimationConfig config)
        {
            this.config = config;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (animator != null)
            {
                animator.Transformed -= TransformedEvent;
                animator.Stop();
                animator = null;
            }

            if (gifDecoder != null)
            {
                int size = gifDecoder.GetFrameCount();
                for (int i = 0; i < size; i++)
                {
                    gifDecoder.GetFrame(i).Dispose();
                }
                gifDecoder = null;
            }

            if (gifEncoder != null)
            {
                gifEncoder.Finish();
                isStart = false;
                gifEncoder = null;
            }

            if (brushTool != null)
            {
                brushTool.Dispose();
                brushTool = null;
            }

            if (bitmap != null)
            {
                isDisposed = true;
                bitmap.Dispose();
                bitmap = null;
            }

            if (nenoGifImg != null)
            {
                nenoGifImg.Dispose();
                nenoGifImg = null;
            }
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        { 
        
        }

        /// <summary>
        /// 根据动画配置创建动画效果
        /// </summary>
        /// <param name="g"></param>
        public void CreateAnimation(Graphics g1)
        {
            if (animator != null)
            {
                animator.Transformed -= TransformedEvent;
                animator.Stop();
            }

            if (screens != null)
            {
                screens.Clear();
            }
            
            //List<AnimationChar> chars = new List<AnimationChar>();

            bitmap = new Bitmap(AnimationConfig.Width, AnimationConfig.Height);
            isDisposed = false;
            g = Graphics.FromImage(bitmap);
            g.TextRenderingHint = textRenderingHint;
            g.SmoothingMode = smoothingMode;

            //SetAnimation(g);
            if (animationFactory == null)
                animationFactory = new AnimationFactory(g);
            else
                animationFactory.Graphiscs = g;

            isParallelAnimation = false;
            screens = animationFactory.CreateAnimationChars(ref isParallelAnimation);
            screenindex = 0;

            animator = new StringAnimator(config, isParallelAnimation);
            animator.Screens = screens;
            animator.Transformed += TransformedEvent;
            animator.Start();

            gifEncoder = new AnimatedGifEncoder();
            gifEncoder.SetDelay(StringAnimator.TIME_INTERVAL);
            gifEncoder.SetRepeat(0);

            if (frog == null)
            {
                try
                {
                    frog = new Image[5];
                    frog[0] = Bitmap.FromFile("Res/frog1.png");
                    frog[1] = Bitmap.FromFile("Res/frog2.png");
                    frog[2] = Bitmap.FromFile("Res/frog3.png");
                    frog[3] = Bitmap.FromFile("Res/frog4.png");
                    frog[4] = Bitmap.FromFile("Res/frog5.png");
                }
                catch (System.IO.FileNotFoundException) { frog = null; }
                catch (System.OutOfMemoryException) { frog = null; }
                catch (System.ArgumentException) { frog = null; }
            }
            frogIndex = 0;

            brushTool = new BrushTool();
            brushTool.InitBrush();

            if (AnimationConfig.NeonBackgroundEnabled && AnimationConfig.NenoBackgroundPath.Length > 0)
            {
                //NenoGifPath = AnimationConfig.NenoBackgroundPath;
                nenoGifImg = null;
                try
                {
                    nenoGifImg = Image.FromFile(AnimationConfig.NenoBackgroundPath);
                    fd = new FrameDimension(nenoGifImg.FrameDimensionsList[0]);
                    gifFrameIndex = 0;
                }
                catch (System.IO.FileNotFoundException) { }
                catch (System.OutOfMemoryException) { }
                catch (System.ArgumentException) { }
            }
            else
            {
                if (nenoGifImg != null) nenoGifImg.Dispose();
                nenoGifImg = null;
                gifFrameIndex = -1;
            }
        }

        private bool isStart = false;
        private void TransformedEvent(object sender, TransformEventArgs args)
        { 
            lock (obj)
            {
                this.screenindex = args.ScreenIndex;
            }
            drawBitmap();
            if (AnimationConfig.IsUseGif)
            {
                if (args.State == TransformEventArgs.BEGIN)
                {
                    gifEncoder.Start("./test.gif");
                    gifEncoder.AddFrame(bitmap);
                    isStart = true;
                }
                else if (args.State == TransformEventArgs.RUNNING)
                {
                    gifEncoder.AddFrame(bitmap);
                }
                else if (args.State == TransformEventArgs.END)
                {
                    gifEncoder.Finish();
                    isStart = false;
                    if (DrawBitmap != null && !isDisposed)
                    {
                        DrawBitmap(this, new DrawEventArgs(bitmap));
                    }
                }
                else
                {
                    if (isStart)
                        gifEncoder.Finish();
                    isStart = false;
                }
            }
            else
            {
                if (DrawBitmap != null && !isDisposed)
                {
                    DrawBitmap(this, new DrawEventArgs(bitmap));
                }
            }
        }

        private void drawBitmap()
        {
            //g.TextRenderingHint = textRenderingHint;
            //g.SmoothingMode = smoothingMode;
            g.Clear(Color.Black);

            if (gifFrameIndex != -1)
            {
                //Image image = gifDecoder.GetFrame(gifFrameIndex);
                //if (image != null)
                //{
                //    g.DrawImage(image, 0, 0, AnimationConfig.Width, AnimationConfig.Height);
                //}
                //gifFrameIndex++;
                //if (gifFrameIndex >= gifDecoder.GetFrameCount())
                //    gifFrameIndex = 0;
                if (nenoGifImg != null)
                {
                    nenoGifImg.SelectActiveFrame(fd, gifFrameIndex);
                    g.DrawImage(nenoGifImg, 0, 0, AnimationConfig.Width, AnimationConfig.Height);
                    gifFrameIndex++;
                    if (gifFrameIndex >= nenoGifImg.GetFrameCount(fd))
                        gifFrameIndex = 0;
                }
            }

            int idx = 0;
            lock (obj)
            {
                idx = screenindex;
            }

            if (idx >= screens.Count)
                return;

            Brush brush = brushTool.Brush;
            Pen pen = brushTool.Pen;

            if (AnimationConfig.AnimationType == AnimationType.FROG_COMPRESS || AnimationConfig.AnimationType == AnimationType.FROG_CARRY_OUT)
            {
                LinkedList<AnimationChar> chars = screens[idx];
                LinkedListNode<AnimationChar> node = chars.First;
                region.MakeEmpty();
                while (node != null)
                {
                    //node.Value.Draw(brush, pen, g);
                    region.Xor(node.Value.GetDrawPath());
                    if (node.Value.Animation.IsStart() && !node.Value.Animation.IsAnimationEnd() && frog != null)
                    {
                        g.DrawImage(frog[frogIndex++], node.Value.Location.X, node.Value.Location.Y - node.Value.Size.Width, node.Value.Size.Width, node.Value.Size.Width);
                        if (frogIndex == 5) frogIndex = 0;    
                    }

                    node = node.Next;
                }
                g.FillRegion(brush, region);
            }
            else
            {
                LinkedList<AnimationChar> chars = screens[idx];
                LinkedListNode<AnimationChar> node = chars.First;
                region.MakeEmpty();
                while (node != null)
                {
                    //node.Value.Draw(brush, pen, g);
                    region.Xor(node.Value.GetDrawPath());
                    if (isParallelAnimation)
                    {
                        node = node.Next;
                    }
                    else
                    {
                        bool end = node.Value.Animation.IsAnimationEnd();
                        if (!end)
                            break;
                        node = node.Next;
                    }
                }

                g.FillRegion(brush, region);
            }

            //Bitmap b = new Bitmap(bitmap);
            //if (DrawBitmap != null)
            //{
            //    DrawBitmap(this, new DrawEventArgs(b));
            //}
            
        }
    }
}
