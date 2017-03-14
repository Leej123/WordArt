using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordArt.Animation
{
    /// <summary>
    /// 右上角放大进入
    /// </summary>
    public class TopRighMoveInWithScaleAnimation : TopRightMoveInAnimation
    {
        private float fromScale;
        private float toScale;

        private float offsetX;

        public TopRighMoveInWithScaleAnimation(float fromX, float toX, float fromY, float toY, float width, float height, float fromScale, float toScale)
            : base(fromX, toX, fromY, toY, width, height)
        {
            this.fromScale = fromScale;
            this.toScale = toScale;
        }

        public override void ApplyTransformation(System.Drawing.Drawing2D.Matrix transform)
        {
            base.ApplyTransformation(transform);
            float scale = GetScale(curX);
            offsetX = width * scale - width;
            transform.Translate(-offsetX, 0);
            transform.Scale(GetScale(curX), 1);
        }

        private float GetScale(float curX)
        {
            if (fromX == toX)
                return toScale;

            return (curX - fromX) * (toScale - fromScale) / (toX - fromX) + fromScale;
        }
    }
}
