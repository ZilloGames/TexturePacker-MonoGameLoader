using System.Collections.Generic;

namespace TexturePackerLoader
{
    public class SpriteSheet
    {
        private readonly IDictionary<string, SpriteFrame> _spriteList;

        public SpriteSheet()
        {
            _spriteList = new Dictionary<string, SpriteFrame>();
        }

        public void Add(string name, SpriteFrame sprite)
        {
            _spriteList.Add(name, sprite);
        }

        public void Add(SpriteSheet otherSheet)
        {
            foreach (var sprite in otherSheet._spriteList)
            {
                _spriteList.Add(sprite);
            }
        }

        public SpriteFrame GetSpriteFrame(string sprite)
        {
            return _spriteList[sprite];
        }

        public List<string> GetAvailableSprites()
        {
            return _spriteList.Keys.ToList();
        }

        internal void Unload()
        {
            foreach(var sprite in _spriteList)
            {
                sprite.Value.Texture.Dispose();
            }
            _spriteList.Clear();
        }
    }
}