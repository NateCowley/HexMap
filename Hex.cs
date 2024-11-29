using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexMap
{
    internal class Hex
    {
        public Point center;
        public Point[] points;
        public float radius;
        public Color borderColor;
        public Color backgroundColor;

        public Hex() : this(0, 0, 1.0f, Color.Black, Color.Green) { }

        public Hex(int centerX, int centerY) : this(centerX, centerY, 1.0f, Color.Black, Color.Green) { }

        public Hex(int centerX, int centerY, float radius) : this(centerX, centerY, radius, Color.Black, Color.Green) { }

        public Hex(int centerX, int centerY, float radius, Color border) : this(centerX, centerY, radius, Color.Black, Color.Green) { }

        public Hex(int centerX, int centerY, float radius, Color border, Color background)
        {
            center = new Point(centerX, centerY);

            borderColor = border;
            backgroundColor = background;

            this.radius = radius;

            setPoints();
        }

        private void setPoints()
        {
            points = new Point[6];

            for (int i = 0; i < 6; i++)
            {
                points[i] = new Point((int)(center.X + HexMap.RADIUS * Math.Cos(i * 60 * Math.PI/180f)), (int)(center.Y + HexMap.RADIUS * Math.Sin(i * 60 * Math.PI / 180f)));
            }
        }

        public void draw(Graphics canvas)
        {
            canvas.FillPolygon(new SolidBrush(backgroundColor), points);
            for(int i = 0; i < points.Length-1; i++)
            {
                canvas.DrawLine(new Pen(new SolidBrush(borderColor), 3), points[i], points[i + 1]);
            }

            canvas.DrawLine(new Pen(new SolidBrush(borderColor), 1), points[5], points[0]);
        }
    }
}
