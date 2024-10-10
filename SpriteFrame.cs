using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TexturePackerLoader
{ 
    public class SpriteFrame
    {
        public SpriteFrame(Texture2D texture, Texture2D normalMap, Rectangle sourceRect, Vector2 size, Vector2 pivotPoint, bool isRotated)
        {

            NormalMap = normalMap;
            this.Texture = texture;
            this.SourceRectangle = sourceRect;
            this.Size = size;
            this.Origin = isRotated ? new Vector2(sourceRect.Width * (1 - pivotPoint.Y), sourceRect.Height * pivotPoint.X)
                                    : new Vector2(sourceRect.Width * pivotPoint.X, sourceRect.Height * pivotPoint.Y);
            this.IsRotated = isRotated;
        }

        public bool HasNormalMap => NormalMap != null;
        public Texture2D NormalMap { get; }
        public Texture2D Texture { get; private set; }

        public Rectangle SourceRectangle { get; private set; }

        public Vector2 Size { get; private set; }

        public bool IsRotated { get; private set; }

        public Vector2 Origin { get; private set; }
    }
}
