using System.Drawing;

namespace Biceseve.Lib
{
    public class RgbArray
    {
        public Color[][] data { get; set; }

        public int Width => this.data?[0].Length ?? 0;
        public int Height => this.data?.Length ?? 0;
        public RgbArray(Color[][] data)
        {
            this.data = data;
        }

        public static implicit operator RgbArray(Color[][] data) => new RgbArray(data);
        public static implicit operator Color[][](RgbArray rgbArray) => rgbArray.data;
    }
}
