using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationLayer.Classes
{
    public class ClsUtils
    {

        public static bool IsFolderThere(string SourceFolder)
        {
            try
            {
                if (!Directory.Exists( SourceFolder ))
                {
                    Directory.CreateDirectory(SourceFolder);
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                MessageBox.Show($"Error: {ex.Message} \n {SourceFolder}", "Check Folder Exists", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        public static string ChangeNameToGUID(string FileName)
        {
            string filename = FileName;
            FileInfo fi = new FileInfo(filename);
            string extn = fi.Extension;
            return Guid.NewGuid().ToString().Replace("-", "").ToLower() + extn;
        }

        public static bool MoveImageToProjectFolder(ref string SourceImage)
        {
            string destinationFolder = @"C:\DVLD-People-Images\";

            if (!IsFolderThere( destinationFolder ))
            {
                return false;
            }

            string DestinationFolder = destinationFolder + ChangeNameToGUID(SourceImage);

            try
            {
                File.Copy(SourceImage, DestinationFolder, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}.", "Error File Copy", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            SourceImage = DestinationFolder;
            return true;
        }

        public static Image ResizeImage(Image image, int _Width, int _Height)
        {
            Bitmap bitmap = new Bitmap(_Width, _Height);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;

                g.DrawImage(image, 0, 0, _Width, _Height);
            }

            return bitmap;
        }

        public static Image ResizeAndCorpToCircle(Image image, int _Width, int _Height, Color Color)
        {
            Image resizeImage = ResizeImage(image, _Width, _Height);

            int diameter = Math.Min(resizeImage.Width, resizeImage.Height);

            Bitmap CircleBitmap = new Bitmap(diameter, diameter);

            using (Graphics g = Graphics.FromImage(CircleBitmap))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddEllipse(0, 0, diameter, diameter);
                    g.SetClip(path);
                    g.DrawImage(resizeImage, 0, 0, diameter, diameter);

                    using (Pen pen1 = new Pen(Color, 2))
                    {
                        g.ResetClip();
                        g.DrawEllipse(pen1, 1, 1, diameter - 2, diameter - 2);
                    }
                }
            }

            return CircleBitmap;
        }


    }
}
