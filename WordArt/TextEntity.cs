using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace WordArt
{
    public class TextEntity
    {
        private string text = "";
        /// <summary>
        /// 单个文本
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        private Font font = new Font(new FontFamily("宋体"), 9, FontStyle.Regular);
        /// <summary>
        /// 获取应用到该文本的字体
        /// </summary>
        public Font Font
        {
            get { return font; }
        }

        /// <summary>
        /// 设置应用到该文本的字体
        /// </summary>
        /// <param name="fontName"></param>
        /// <param name="emsize"></param>
        /// <param name="fontStyle"></param>
        public void SetFont(string fontName, int emsize, FontStyle fontStyle)
        {
            font = new Font(new FontFamily(fontName), emsize, fontStyle);
        }
    }
}
