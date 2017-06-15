using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using WordArt.Animation;
using WordArt.Config;
using WordArt.Gif;

namespace WordArt
{
    public class BitmapTool
    {
        private string savePath;
        private string filename;
        private int index = 1;
        private int screenIndex = 0;

        public string Path
        {
            set { savePath = value; }
        }

        public string Name
        {
            set { filename = value; }
        }

        public BitmapTool()
        {
            Reset();
        }

        public void Reset()
        {
            Path = AnimationConfig.BitmapSavePath;
            filename = AnimationConfig.BitmapSaveName;
            index = 1;
            screenIndex = 0;
            Prepare();
        }

        public void SaveBitmap(int screenIndex, Bitmap bitmap)
        {
            string saveName = string.Format("{0:s}_{1:D4}.bmp", filename, index);
            string p = savePath + "/" + saveName;
            bitmap.Save(p, ImageFormat.Bmp);
            index++;
        }

        public void SaveGif()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(savePath);
            if (!dirInfo.Exists)
                return;

            FileInfo[] fileInfos = dirInfo.GetFiles("*.bmp");
            if (fileInfos.Length <= 0) return;

            String gifFile = AnimationConfig.BitmapSavePath + "/" + AnimationConfig.BitmapSaveName + ".gif";

            AnimatedGifEncoder gifEncoder = new AnimatedGifEncoder();

            int deylay = (int)(100 / AnimationConfig.AnimationSpeedFactor);
            //if (deylay > 500) deylay = 500;
            //if (deylay < 20) deylay = 20;

            gifEncoder.SetDelay(deylay);
            gifEncoder.SetRepeat(0);

            bool ok = gifEncoder.Start(gifFile);
            if (!ok) return;
            
            foreach (FileInfo fileInfo in fileInfos)
            {
                string filename = fileInfo.FullName;
                try
                {
                    Image img = Image.FromFile(filename);
                    gifEncoder.AddFrame(img);
                    img.Dispose();
                }
                catch (OutOfMemoryException) { }
                catch (FileNotFoundException) { }
                catch (ArgumentException) { }
            }

            gifEncoder.Finish();
        }

        private void Prepare()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(savePath);
            if (dirInfo.Exists)
            {
                FileSystemInfo[] fileInfos = dirInfo.GetFileSystemInfos();
                foreach (FileSystemInfo info in fileInfos)
                { 
                    if (!(info is DirectoryInfo) && info.Name.EndsWith(".bmp"))
                    {
                        File.Delete(info.FullName);
                    }
                }
            }
            else
            {
                dirInfo.Create();
            }
        }
    }
}
