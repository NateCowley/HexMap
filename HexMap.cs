using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HexMap
{
    public partial class HexMap : Form
    {
        public static float RADIUS = 20.0f;
        public static float DIAMETER = 40.0f;
        public static int ROWS = 16;
        public static int COLUMNS = 16;

        private int centerMapX;
        private int centerMapY;
        private int hexDistanceX;
        private int hexDistanceY;

        private Graphics canvas;
        private Hex[] hexes;

        private Bitmap bmp;

        private bool isFirstPaint = true;

        public HexMap()
        {
            InitializeComponent();

            initialSetup();
        }

        private void initialSetup()
        {
            bmp = new Bitmap(pb.Width, pb.Height);
            canvas = Graphics.FromImage(bmp);

            setParameters();

            buildHexes();
        }

        public void drawMap()
        {
            Brush b = new SolidBrush(Color.White);
            canvas.FillRectangle(b, new Rectangle(0, 0, pb.Width, pb.Height));

            for(int i = 0; i < hexes.Length; i++)
            {
                hexes[i].draw(canvas);
            }

            pb.Image = bmp;
        }

        public void setParameters()
        {
            try
            {
                float prevRadius = RADIUS;
                int prevRows = ROWS;
                int prevColumns = COLUMNS;

                float.TryParse(tbRadius.Text, out RADIUS);
                Int32.TryParse(tbRows.Text, out ROWS);
                Int32.TryParse(tbColumns.Text, out COLUMNS);

                if (prevRadius != RADIUS || prevRows != ROWS || prevColumns != COLUMNS)
                {
                    buildHexes();
                }

                DIAMETER = 2 * RADIUS;

                int hexHeight = (int)(RADIUS * Math.Sqrt(3));
                hexDistanceX = (int)Math.Sqrt(hexHeight * hexHeight - hexDistanceY * hexDistanceY);

                hexDistanceY = (int)(Math.Sqrt(RADIUS * RADIUS - (RADIUS/2)*(RADIUS/2)));

                centerMapX = (pb.Width / 2) - (hexDistanceX * COLUMNS) / 2;
                centerMapY = (pb.Height / 2) - (hexDistanceY * 2 * ROWS) / 2;
            }
            catch (Exception e) 
            {
                MessageBox.Show("Error");
            }
            
        }

        public void buildHexes()
        {
            hexes = new Hex[ROWS * COLUMNS];

            for (int i = 0; i < COLUMNS; i++)
            {
                for (int j = 0; j < ROWS; j++)
                {
                    hexes[i * ROWS + j] = new Hex((i * hexDistanceX) + centerMapX, (j * (hexDistanceY*2)) + (hexDistanceY * (i % 2) + centerMapY));
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            setParameters();
            drawMap();
        }

        private void pb_MouseClick(object sender, MouseEventArgs e)
        {
            int target = findNearestHexIndex(e.Location);

            if(e.Button == MouseButtons.Left)
            {
                hexes[target].backgroundColor = Color.Red;
            }
            else if(e.Button == MouseButtons.Right)
            {
                hexes[target].backgroundColor = Color.Green;
            }
            else if(e.Button == MouseButtons.Middle)
            {
                hexes[target].backgroundColor = Color.Black;
            }

            drawMap();
        }

        private float distance(Point a, Point b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        private int findNearestHexIndex(Point p)
        {
            Hex closest = hexes[0];
            float shortestDistance = distance(p, closest.center);
            int index = -1;

            for(int i = 0; i < hexes.Length; i++)
            {
                float dist = distance(p, hexes[i].center);
                if(dist < shortestDistance)
                {
                    closest = hexes[i];
                    shortestDistance = dist;
                    index = i;
                }
            }

            return index;
        }

        private void HexMap_Activated(object sender, EventArgs e)
        {
            //drawMap();
        }

        private void HexMap_Paint(object sender, PaintEventArgs e)
        {

        }

        private void HexMap_Shown(object sender, EventArgs e)
        {
            initialSetup();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Clipboard.SetImage(bmp);
        }
    }
}
