using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace HexGridSharp
{
    public class HexGrid
    {
        #region Fields

        private int _hexCountX;
        private int _hexCountY;
        private float _hexWidth;

        #endregion Fields

        #region Properties

        public List<HexCell> Cells { get; set; }

        #endregion Properties

        #region Constructor

        public HexGrid(int hexCountX, int hexCountY, float outerRadius)
        {
            _hexCountX = hexCountX;
            _hexCountY = hexCountY;
            _hexWidth = outerRadius * 2;
            HexCell._outerRadius = outerRadius;
            HexCell._innerRadius = 0.866025404f * outerRadius;

            Cells = new List<HexCell>();
            for (int z = 0; z < hexCountY; z++)
            {
                for (int x = 0; x < hexCountX; x++)
                {
                    float offsetX = (x + (z * 0.5f) - (z / 2)) * (HexCell._innerRadius * 2f);
                    float offsetZ = z * (HexCell._outerRadius * 1.5f) + (outerRadius);
                    HexCell cell = new HexCell(offsetX, offsetZ, z, x);
                    Cells.Add(cell);
                }
            }
        }

        #endregion Constructor

        #region Methods

        public void ToTexture(string filePath)
        {
            var imageWidth = (int)Math.Ceiling(_hexCountX * _hexWidth);
            var imageHeight = (int)Math.Ceiling(_hexCountY * _hexWidth);
            var blackPen = SixLabors.ImageSharp.Processing.Pens.Solid(Rgba32.Black, 1f);
            using (var im = new Image<Rgba32>(imageWidth, imageHeight))
            {
                for (int i = 0; i < Convert.ToInt32(_hexCountX * _hexWidth); i++)
                {
                    for (int j = 0; j < Convert.ToInt32(_hexCountY * _hexWidth); j++)
                    {
                        im[i, j] = Rgba32.Transparent;
                    }
                }
                GraphicsOptions o = new GraphicsOptions()
                {
                    Antialias = true,
                    AntialiasSubpixelDepth = 16
                };
                foreach (var cell in Cells)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        var vec1 = cell.Corners[i % 6];
                        var vec2 = cell.Corners[(i + 1) % 6];
                        float x1 = vec1.X;
                        float x2 = vec2.X;
                        float z1 = vec1.Y;
                        float z2 = vec2.Y;
                        var p = new PointF(x1, z1);
                        var p2 = new PointF(x2, z2);
                        im.Mutate(a => a.DrawLines<Rgba32>(o, blackPen, p, p2));
                    }
                }
                using (var fs = File.OpenWrite(filePath))
                {
                    im.SaveAsPng(fs);
                }
            }
        }

        #endregion Methods
    }

    public class HexCell
    {
        #region Fields

        public static float _outerRadius;
        public static float _innerRadius;

        #endregion Fields

        #region Properties

        public List<Vector2> Corners { get; set; }
        public Vector2 Center { get { return new Vector2(Corners[3].X, Corners[3].Y - _outerRadius); } }

        public int Row { get; set; }
        public int Column { get; set; }

        #endregion Properties

        #region Constructor

        public HexCell(float offsetX, float offsetZ, int row, int column)
        {
            Row = row;
            Column = column;
            Corners = new List<Vector2>
            {
                new Vector2(0f + offsetX,_outerRadius + offsetZ),
                new Vector2(_innerRadius + offsetX, (0.5f * _outerRadius) + offsetZ),
                new Vector2(_innerRadius + offsetX, (-0.5f * _outerRadius) + offsetZ),
                new Vector2(0f + offsetX, -_outerRadius + offsetZ),
                new Vector2(-_innerRadius + offsetX, (-0.5f * _outerRadius) + offsetZ),
                new Vector2(-_innerRadius + offsetX, (0.5f * _outerRadius) + offsetZ)
            };
        }

        #endregion Constructor
    }

    public struct Vector2
    {
        #region Fields

        public float X;
        public float Y;

        #endregion Fields

        #region Constructor

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        #endregion Constructor

        #region Methods

        public override string ToString()
        {
            return $"({X} , {Y})";
        }

        #endregion Methods
    }
}