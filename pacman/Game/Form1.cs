using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game
{
    public partial class Form1 : Form
    {
        String imagePath = @"C:\Users\konto\Documents\Visual Studio 2013\Projects\Game\Game\Images\pacman.bmp";
        String imagePath1 = @"C:\Users\konto\Documents\Visual Studio 2013\Projects\Game\Game\Images\pacman1.bmp";
        String imagePath2 = @"C:\Users\konto\Documents\Visual Studio 2013\Projects\Game\Game\Images\pacman2.bmp";
        int index = 0, movedPixels = 25;
        List<String> images = new List<String>();
        Boolean rightMoveBool = true;
        Boolean downMoveBool = false;
        Boolean upMoveBool = false;
        Boolean leftMoveBool = false;
        Boolean exit = false;
        Thread eatingThread;
        RotateFlipType rotate = RotateFlipType.RotateNoneFlipNone;
        List<Boolean> backup = new List<Boolean>();

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Bitmap pacman_layout = new Bitmap(@"C:\Users\konto\Documents\Visual Studio 2013\Projects\Game\Game\Images\map.png");
            Bitmap pacman_layout_Thumbnail = new Bitmap(pacman_layout, new Size(panel1.Width, panel1.Height));
            panel1.BackgroundImage = pacman_layout_Thumbnail;
            KeyPreview = true;
            images.Add(imagePath);
            images.Add(imagePath1);
            images.Add(imagePath2);
            nextFrame();
        }
        private void nextFrame() 
        {
            Bitmap currentImage = new Bitmap(images[index]);
            currentImage.RotateFlip(rotate);
            pictureBox1.Image = currentImage;
            if (index < images.Count - 1)
            {
                index++;
            }
            else 
            {
                index = 0;
            }
        }
        private void startGame() 
        {
            while (!exit)
            {
                Thread.Sleep(100);
                nextFrame();
                pacmanMove();
            }
        }
        private void pacmanMove()
        {
            Point newPoint = pictureBox1.Location;
            if (rightMoveBool)
            {
                newPoint = new Point(pictureBox1.Location.X + movedPixels, pictureBox1.Location.Y);
            }
            if (leftMoveBool)
            {
                newPoint = new Point(pictureBox1.Location.X - movedPixels, pictureBox1.Location.Y);
            }
            if (downMoveBool) 
            {
                newPoint = new Point(pictureBox1.Location.X, pictureBox1.Location.Y + movedPixels);
            }
            if (upMoveBool) 
            {
                newPoint = new Point(pictureBox1.Location.X, pictureBox1.Location.Y - movedPixels);
            }
            if (newPoint.X + pictureBox1.Width >= panel1.Width && rightMoveBool)
            {
                newPoint = new Point(0, pictureBox1.Location.Y);
            }
            if (newPoint.X < - pictureBox1.Width) 
            {
                newPoint = new Point(panel1.Size.Width , pictureBox1.Location.Y);
            }
            if (newPoint.Y >= panel1.Height)
            {
                newPoint = new Point(pictureBox1.Location.X, 0);
            }
            if (newPoint.Y <= (0 - (pictureBox1.Size.Height/3))) 
            {
                newPoint = new Point(pictureBox1.Location.X, panel1.Height);
            }
            if (pictureBox1.InvokeRequired)
            {
                pictureBox1.Invoke(new MethodInvoker(delegate { pictureBox1.Location = newPoint; }));
            }            
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.D1) 
            {
                startMove();
            }
            if (e.KeyCode == Keys.D0)
            {
                stopMove();
            }
            if (e.KeyCode == Keys.Right)
            {
                if (leftMoveBool) 
                {
                    rotate = RotateFlipType.Rotate180FlipXY;
                }
                if (upMoveBool) 
                {
                    rotate = RotateFlipType.Rotate180FlipXY;
                }
                if (downMoveBool) 
                {
                    rotate = RotateFlipType.Rotate180FlipXY;
                }
                rightMoveBool = true;
                downMoveBool = false;
                upMoveBool = false;
                leftMoveBool = false;
                return;
            }
            if (e.KeyCode == Keys.Left)
            {
                if (rightMoveBool)
                {
                    rotate = RotateFlipType.Rotate180FlipY;
                }
                if (upMoveBool) 
                {
                    rotate = RotateFlipType.Rotate180FlipY;
                }
                if (downMoveBool) 
                {
                    rotate = RotateFlipType.Rotate180FlipY;
                }
                downMoveBool = false;
                rightMoveBool = false;
                upMoveBool = false;
                leftMoveBool = true;
                return;
            }
            if (e.KeyCode == Keys.Down)
            {
                if (upMoveBool) 
                {
                    rotate = RotateFlipType.Rotate90FlipX;
                }
                if (leftMoveBool) 
                {
                    rotate = RotateFlipType.Rotate270FlipY;
                }
                if (rightMoveBool) 
                {
                    rotate = RotateFlipType.Rotate90FlipNone;
                }
                downMoveBool = true;
                rightMoveBool = false;
                upMoveBool = false;
                leftMoveBool = false;
                return;
            }
            if (e.KeyCode == Keys.Up)
            {
                if (leftMoveBool) 
                {
                    rotate = RotateFlipType.Rotate90FlipY;
                }
                if (rightMoveBool) 
                {
                    rotate = RotateFlipType.Rotate90FlipXY;
                }
                if (downMoveBool) 
                {
                    rotate = RotateFlipType.Rotate90FlipY;
                }
                downMoveBool = false;
                rightMoveBool = false;
                upMoveBool = true;
                leftMoveBool = false;
                return;
            }
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            stopMove();
            if (eatingThread != null)
            {
                eatingThread.DisableComObjectEagerCleanup();
            }
        }
        private void stopMove()
        {
            backup.Clear();
            backup.Add(rightMoveBool); backup.Add(downMoveBool); backup.Add(upMoveBool); backup.Add(leftMoveBool);
            rightMoveBool = false;
            downMoveBool = false;
            upMoveBool = false;
            leftMoveBool = false;
            exit = true;
        }
        private void startMove()
        {
            if (backup.Count > 0)
            {
                rightMoveBool = backup[0];
                downMoveBool = backup[1];
                upMoveBool = backup[2];
                leftMoveBool = backup[3];
                exit = false;
            }
            eatingThread = new Thread(startGame);
            eatingThread.Start();
            return;
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            /*List<Line> lines = new List<Line>() 
            {
                new Line(new Point(pictureBox1.Width + 5, pictureBox1.Height + 5), new Point(pictureBox1.Width + 5, panel1.Height / 2)),
                new Line(new Point(pictureBox1.Width + 5, panel1.Height / 2), new Point(pictureBox1.Width*2, panel1.Height / 2)),
                new Line(new Point(pictureBox1.Width*2, panel1.Height / 2), new Point(pictureBox1.Width + pictureBox1.Width, pictureBox1.Height + 5)),
                new Line(new Point(pictureBox1.Width + 5, pictureBox1.Height + 5), new Point(pictureBox1.Width + pictureBox1.Width, pictureBox1.Height + 5)),
                new Line(new Point(pictureBox1.Width + 5, panel1.Height / 2 + pictureBox1.Height), new Point(pictureBox1.Width + 5, panel1.Height - pictureBox1.Height)),
                new Line(new Point(pictureBox1.Width + 5, panel1.Height / 2 + pictureBox1.Height), new Point(pictureBox1.Width*2, panel1.Height / 2 + pictureBox1.Height)),
                new Line(new Point(0,0), new Point(0, panel1.Height)),
                new Line(new Point(0, 0), new Point(panel1.Width, 0)),
                new Line(new Point(0, panel1.Height), new Point(panel1.Width, panel1.Height)),
                new Line(new Point(panel1.Width, 0), new Point(panel1.Width, panel1.Height))
            };
            base.OnPaint(e);
            using (Graphics g = e.Graphics)
            {
                var p = new Pen(Color.White, 3);
                for (int i = 0; i < lines.Count; i++)
                {
                    g.DrawLine(p, lines[i].startPoint, lines[i].entPoint);
                }
            }*/
        }

    }
}
