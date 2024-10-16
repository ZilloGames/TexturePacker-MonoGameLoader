﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
#if NETFX_CORE
    using System.Threading.Tasks;
#endif
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TexturePackerLoader
{
    public class SpriteSheetLoader
    {
        private readonly ContentManager _contentManager;
        private readonly GraphicsDevice _graphicsDevice;

        private Dictionary<string, SpriteSheet> _spriteSheets = new Dictionary<string, SpriteSheet>();

        public SpriteSheetLoader(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            this._contentManager = contentManager;
            this._graphicsDevice = graphicsDevice;
        }

        //public SpriteSheet MultiLoad(string imageResourceFormat, int numSheets)
        //{
        //    SpriteSheet result = new SpriteSheet();
        //    for (int i = 0; i < numSheets; i++)
        //    {
        //        string imageResource = string.Format(imageResourceFormat, i);

        //        SpriteSheet tmp = Load(imageResource);
        //        result.Add(tmp);
        //    }
        //    return result;
        //}


        public SpriteSheet Load(string imageResource)
        {
            if (_spriteSheets.ContainsKey(imageResource))
            {
                return _spriteSheets[imageResource];
            }

            var imageFile = Path.Combine(_contentManager.RootDirectory, imageResource + ".png");
            var dataFile = Path.ChangeExtension(imageFile, "txt");
            var normalMapFile = Path.Combine(_contentManager.RootDirectory, imageResource + "_n.png");

            FileStream fileStream = new FileStream(imageFile, FileMode.Open);
            var texture = Texture2D.FromStream(_graphicsDevice, fileStream);
            fileStream.Dispose();

            Texture2D normalMapTexture = null;

            if(File.Exists(normalMapFile))
            {
                FileStream normalMapFileStream = new FileStream(normalMapFile, FileMode.Open);
                normalMapTexture = Texture2D.FromStream(_graphicsDevice, normalMapFileStream);
                normalMapFileStream.Dispose();
            }


            var dataFileLines = ReadDataFile(dataFile);

            var sheet = new SpriteSheet();

            foreach (
                var cols in
                    from row in dataFileLines
                    where !string.IsNullOrEmpty(row) && !row.StartsWith("#")
                    select row.Split(';'))
            {
                if (cols.Length != 10)
                {
                    throw new InvalidDataException("Incorrect format data in spritesheet data file");
                }

                var isRotated = int.Parse (cols [1]) == 1;
                var name = cols[0];
                var sourceRectangle = new Rectangle(
                    int.Parse(cols[2]),
                    int.Parse(cols[3]),
                    int.Parse(cols[4]),
                    int.Parse(cols[5]));
                var size = new Vector2(
                    int.Parse(cols[6]),
                    int.Parse(cols[7]));
                var pivotPoint = new Vector2(
                    float.Parse(cols[8], CultureInfo.InvariantCulture),
                    float.Parse(cols[9], CultureInfo.InvariantCulture));
                var sprite = new SpriteFrame(texture, normalMapTexture, sourceRectangle, size, pivotPoint, isRotated);

                sheet.Add(name, sprite);
            }

            _spriteSheets.Add(imageResource, sheet);

            return sheet;
        }
        public void Unload()
        {
            foreach (var sheet in _spriteSheets)
            {
                sheet.Value.Unload();
            }
            _spriteSheets.Clear();
        }

#if NETFX_CORE
        private string[] ReadDataFile(string dataFile)
        {
            var dataFileLines = ReadDataFileLines(dataFile);

            return dataFileLines.Result.ToArray();
        }

        private async Task<string[]> ReadDataFileLines(string dataFile)
        {
            var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;

            var file = await folder.GetFileAsync(dataFile).AsTask().ConfigureAwait(false);
            var fileContents = await Windows.Storage.FileIO.ReadLinesAsync(file).AsTask().ConfigureAwait(false);

            return fileContents.ToArray();
        }
#elif __ANDROID__
		private string[] ReadDataFile(string dataFile) {
			using(var ms = new MemoryStream()) {
				using (var s = Game.Activity.Assets.Open (dataFile)) {
					s.CopyTo (ms);
					return System.Text.Encoding.Default.GetString (ms.ToArray()).Split (new char[] { '\n'});
				}
			}
		}
#else
        private string[] ReadDataFile(string dataFile)
        {
            return File.ReadAllLines(dataFile);
        }

      
#endif

    }
}