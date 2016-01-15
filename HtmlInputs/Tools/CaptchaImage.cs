using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
namespace HtmlInputs.Tools
{
    public class CaptchaImage
    {
        public const string CaptchaValueKey = "CaptchaImageText";

        private string text;
        private int width;
        private int height;
        private string familyName;
        private Bitmap image;

        //Генерация случайных чисел
        private Random random = new Random();
        public string Text
        {
            get { return text; }
        }

        public Bitmap Image
        {
            get { return image; }
        }

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        public CaptchaImage(string s, int width, int height, string familyName)
        {
            text = s;
            SetDimensions(width, height);
            SetFamilyName(familyName);
            GenerateImage();
        }

        public CaptchaImage(string s, int width, int height)
        {
            text = s;
            SetDimensions(width, height);
            GenerateImage();
        }
        ~CaptchaImage()
        {
            Dispose(false);
        }
        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                image.Dispose();
            }
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        private void SetDimensions(int aWidth, int aHeight)
        {
            if (aWidth <= 0)
                throw new ArgumentOutOfRangeException("aWidth", aWidth, "Argument out of range, must be greater than zero");
            if (aHeight <= 0)
                throw new ArgumentOutOfRangeException("aHeight", aHeight, "Argument out ou range, must be greater than zero");
            width = aWidth;
            height = aHeight;
        }

        private void SetFamilyName(string aFamilyName)
        {
            try
            {
                Font font = new Font(aFamilyName, 12F);
                familyName = aFamilyName;
                font.Dispose();
            }
            catch
            {
                familyName = FontFamily.GenericSerif.Name;
            }
        }

        private void GenerateImage()
        {
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, width, height);

            HatchBrush hatchBrush = new HatchBrush(HatchStyle.SmallConfetti, Color.LightGray, Color.White);
            g.FillRectangle(hatchBrush, rect);

            SizeF size;
            float fontSize = rect.Height + 1;
            Font font;

            do
            {
                fontSize--;
                font = new Font(familyName, fontSize, FontStyle.Bold);
                size = g.MeasureString(text, font);
            } while (size.Width > rect.Width);

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            GraphicsPath path = new GraphicsPath();
            path.AddString(text, font.FontFamily, (int)font.Style, font.Size, rect, format);
            float v = 4F;
            PointF[] points = {
                                  new PointF(random.Next(rect.Width)/v,random.Next(rect.Height)/v),
                                  new PointF(rect.Width-random.Next(rect.Width)/v, random.Next(rect.Height)/v),
                                  new PointF(random.Next(rect.Width)/v,rect.Height-random.Next(rect.Height)/v),
                                  new PointF(rect.Width - random.Next(rect.Width)/v, rect.Height - random.Next(rect.Height)/v)
                              };
            Matrix matrix = new Matrix();
            matrix.Translate(0F, 0F);
            path.Warp(points, rect, matrix, WarpMode.Perspective, 0F);
            hatchBrush = new HatchBrush(HatchStyle.LargeConfetti, Color.LightGray, Color.DarkGray);
            g.FillPath(hatchBrush, path);

            int m = Math.Max(rect.Width, rect.Height);
            for(int i = 0; i < (int)(rect.Width * rect.Height / 30F); i++)
            {
                int x = random.Next(rect.Width);
                int y = random.Next(rect.Height);
                int w = random.Next(m / 50);
                int h = random.Next(m / 50);
                g.FillEllipse(hatchBrush, x, y, w, h);
            }

            font.Dispose();
            hatchBrush.Dispose();
            g.Dispose();

            image = bitmap;
        }
    }
}