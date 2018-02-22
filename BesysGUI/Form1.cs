using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//EMGU
using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
using OpenCvSharp.Extensions;
using Point = OpenCvSharp.CPlusPlus.Point;
using Size = System.Drawing.Size;

namespace BesysGUI
{
    public partial class Form1 : Form
    {
        /*ImageViewer viewer = new ImageViewer();*/

        public Form1()
        {
            InitializeComponent();
        }

        private Mat GetSceleton(Mat grayImage)
        {
            var binary = new Mat();

            Cv2.Threshold(grayImage, binary, 240, 255, ThresholdType.BinaryInv);

            var skel = Mat.Zeros(binary.Rows, binary.Cols, MatType.CV_8UC1).ToMat();
            var temp = new Mat(binary.Cols, binary.Rows, MatType.CV_8UC1);


            var elem = Cv2.GetStructuringElement(StructuringElementShape.Cross,
                new OpenCvSharp.CPlusPlus.Size(3, 3));

            bool done;
            do
            {
                Cv2.MorphologyEx(binary, temp, MorphologyOperation.Open, elem);
                Cv2.BitwiseNot(temp, temp);
                Cv2.BitwiseAnd(binary, temp, temp);
                Cv2.BitwiseOr(skel, temp, skel);
                Cv2.Erode(binary, binary, elem);

                double max;
                double min;
                Cv2.MinMaxLoc(binary, out min, out max);
                done = (max == 0);
            } while (!done);

            return skel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ShowVideo();

            OpenFileDialog OF = new OpenFileDialog();
            OF.InitialDirectory = Application.StartupPath;
            OF.Filter = "Image files (*.jpg, *.jpeg, *.bmap, *.bmp, *.png, *.gif) | *.jpg; *.jpeg; *.bmap; *.bmp; *.png; *.gif";//TODO set filters...
            if (OF.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //filename = OF.FileName;
                var frame = new Mat(OF.FileName);
                var prevgrayframe = new Mat();
                Cv2.CvtColor(frame, prevgrayframe, ColorConversion.BgrToGray);

                var elem = Cv2.GetStructuringElement(StructuringElementShape.Ellipse,
                    new OpenCvSharp.CPlusPlus.Size(3, 3));

                //Cv2.MedianBlur(prevgrayframe, prevgrayframe, 5);
                //Cv2.GaussianBlur(prevgrayframe, prevgrayframe, new OpenCvSharp.CPlusPlus.Size(3,3), 2, 2);
                var edges = new Mat();
                //Cv2.Threshold(prevgrayframe, edges, 240, 255, ThresholdType.BinaryInv);
                //Cv2.MorphologyEx(edges, edges, MorphologyOperation.Open, elem);

                //var skel = GetSceleton(prevgrayframe);
                //edges = skel;

                Cv2.Canny(prevgrayframe, edges, 100, 200, 3);
                /*var elem = Cv2.GetStructuringElement(StructuringElementShape.Ellipse,
                    new OpenCvSharp.CPlusPlus.Size(2 * 3 + 1, 2 * 3 + 1),
                    new Point(3, 3));*/

                edges = edges.Dilate(elem).Erode(elem);
                //edges = edges.Erode(new Mat());
                //CvInvoke.MedianBlur(prevgrayframe, prevgrayframe, 5);
                //CvInvoke.AdaptiveThreshold(prevgrayframe, prevgrayframe, 255, AdaptiveThresholdType.GaussianC, ThresholdType.Binary, 11, 3.5);

                var hsvFrame = new Mat();
                Cv2.CvtColor(frame, hsvFrame, ColorConversion.BgrToHsv);

                var mask1 = hsvFrame.InRange(new Scalar(0, 70, 50), new Scalar(10, 255, 255));
                var mask2 = hsvFrame.InRange(new Scalar(170, 70, 50), new Scalar(180, 255, 255));

                var mask = mask1 | mask2;
                var maskBgr = new Mat();
                Cv2.CvtColor(mask, maskBgr, ColorConversion.GrayToBgr);

                var maskedImage = frame.Clone();
                Cv2.BitwiseAnd(maskedImage, maskBgr, maskedImage);

                using (CvWindow window = new CvWindow(maskedImage.ToIplImage()))
                {
                    CvWindow.WaitKey();
                }

               /* using (CvWindow window = new CvWindow(prevgrayframe.ToIplImage()))
                {
                    CvWindow.WaitKey();
                }*/

                using (CvWindow window = new CvWindow(edges.ToIplImage()))
                {
                    CvWindow.WaitKey();
                }

                var circles = Cv2.HoughCircles(prevgrayframe, HoughCirclesMethod.Gradient, 1.0, frame.Height/8, 200, 80, 0, 0);

                var lines = Cv2.HoughLinesP(edges, 1, Cv.PI / 180, 60, 30, 2);

                //IOutputArray countours = new VectorOfVectorOfPoint();
               // IOutputArray hierarchy;
                //var contours = new List<C>();


                //frame.Convert<Hsv, byte>().InRange()
                //inRange(hsv, Scalar(0, 70, 50), Scalar(10, 255, 255), mask1);
                //inRange(hsv, Scalar(170, 70, 50), Scalar(180, 255, 255), mask2);



                var frameCopy = frame.Clone();
                foreach (var circle in circles)
                {
                    Cv2.Circle(frameCopy, (int)circle.Center.X, (int)circle.Center.Y, (int)circle.Radius, Scalar.Green, 3);
                    //Cv2.Circle(frameCopy, circle.Center, circle.Radius);
                    //frameCopy.(circle, new Bgr(Color.GreenYellow), 3);
                }

                for (var i = 0; i < lines.Length; i++)
                {
                    var line = lines[i];
                    Cv2.Line(frameCopy, line.P1, line.P2, Scalar.Red, 2);
                }

                var switchedChannels = new Mat();
                var channels = frame.Split();
                Cv2.Merge(channels.Reverse().ToArray(), switchedChannels);
                /*
                using (CvWindow window = new CvWindow(switchedChannels.ToIplImage()))
                {
                    CvWindow.WaitKey();
                }*/

                using (CvWindow window = new CvWindow(frameCopy.ToIplImage()))
                {
                    CvWindow.WaitKey();
                }

                var bw = prevgrayframe.Canny(100, 2 * 100);
                Point[][] countours;
                HierarchyIndex[] indices;
                Cv2.FindContours(bw, out countours, out indices, ContourRetrieval.External, ContourChain.ApproxSimple );
                frameCopy.DrawContours(countours, -1, Scalar.Blue, 2);

                /*using (CvWindow window = new CvWindow(frameCopy.ToIplImage()))
                {
                    CvWindow.WaitKey();
                }
                */
                //CascadeClassifier a = new CascadeClassifier();
                //a.DetectMultiScale() 

                //DisplayImage(prevgrayframe.ToBitmap(), pictureBox1); //thread safe display for camera cross thread errors
            }



        }

        private void ShowVideo()
        {
            VideoCapture capture = new VideoCapture("sample3.mp4");
       
            int sleepTime = (int)Math.Round(1000 / capture.Fps);

            using (Window window = new Window("capture"))
            using (Window windowFiltered = new Window("filtered"))
            using (Window windowMask = new Window("mask"))
            using (Window windowH = new Window("1"))
            using (Window windowH2 = new Window("2"))
            using (Mat image = new Mat()) // Frame image buffer
            {
                // When the movie playback reaches end, Mat.data becomes NULL.
                while (true)
                {
                    capture.Read(image); // same as cvQueryFrame
                    if (image.Empty())
                        break;

                    var imageHsv = new Mat();
                    Cv2.CvtColor(image, imageHsv, ColorConversion.BgrToHsv);

                    var minGreen = new Scalar(0, 30, 60);
                    var maxGreeen = new Scalar(20, 255, 255);
                    var mask = imageHsv.InRange(new Scalar(0, 30, 110), new Scalar(25, 255, 255));

                    var roi = GetFaceRoiFastMat(mask);
                    if (roi != Rect.Empty)
                    {
                        image.Rectangle(roi, Scalar.Red, 2);
                    }

                    //var mask2 = imageHsv.InRange(new Scalar(160, 30, 60), new Scalar(180, 240, 180));

                    //var mask = imageHsv.InRange(new Scalar(0, 50, 50), new Scalar(20, 240, 255));
                    //var mask2 = imageHsv.InRange(new Scalar(160, 30, 60), new Scalar(180, 240, 180));

                    //Cv2.BitwiseOr(mask, mask2, mask);

                    var maskedImage = new Mat();

                    //Cv2.BitwiseNot(mask, mask);
                    imageHsv.CopyTo(maskedImage, mask);

                    var maskedImageBgr = new Mat();
                    Cv2.CvtColor(maskedImage, maskedImageBgr, ColorConversion.HsvToBgr);

                    windowMask.ShowImage(mask);
                    windowFiltered.ShowImage(maskedImageBgr);
                    window.ShowImage(image);
                    Cv2.WaitKey(sleepTime);
                }
            }
        }

        private Rect GetFaceRoiFastMat(Mat mask)
        {
            var elem = Cv2.GetStructuringElement(StructuringElementShape.Ellipse,
                new OpenCvSharp.CPlusPlus.Size(7, 7));
            var elem3 = Cv2.GetStructuringElement(StructuringElementShape.Ellipse,
                new OpenCvSharp.CPlusPlus.Size(19, 19));
            var elem2 = Cv2.GetStructuringElement(StructuringElementShape.Ellipse,
                new OpenCvSharp.CPlusPlus.Size(25, 25));

            //Cv2.Erode(mask, mask, elem);
            //Cv2.Dilate(mask, mask, elem2);
            //Cv2.Erode(mask, mask, elem3);
            //Cv2.Dilate(mask, mask, elem2);

            var minRatio = 0.75;
            var stopRatio = 0.95;
            var minSize = (int)(mask.Height * 0.25);
            var maxSize = (int)(mask.Height * 0.8);

            var maxI = -1;
            var maxJ = -1;
            var max = 0.0;
            var sizeModifier = 0.8;
            var roi = new Rect(0, 0, mask.Width, mask.Height);
            var finalSize = 0;

            var integralMask = GetIntegralMaskMat(mask);

            for (var size = maxSize; size >= minSize; size = (int)(size * sizeModifier))
                {
                    double ratio;
                    var rect = GetFaceRoiFastMat(integralMask, roi, size, out ratio);
                    // remove!
                    //var rect2 = GetFaceRoi(mask, roi, size, out ratio);

                    if (rect == Rect.Empty)
                    {
                        break;
                    }

                    if (ratio < max)
                    {
                        break;
                    }

                    max = ratio;
                    roi = rect;
                    finalSize = size;

                    // remove
                    var drawn = new Mat();
                    Cv2.CvtColor(mask, drawn, ColorConversion.GrayToBgr);
                    drawn.Rectangle(rect, Scalar.Red, 1);
                    //using (var wd = new CvWindow("progress", drawn.ToIplImage()))
                    //{
                    //    //Window.WaitKey(500);
                    //}

                    if (ratio > stopRatio)
                    {
                        break;
                    }
                }


            if (max < minRatio)
            {
                return Rect.Empty;
            }

            return roi;
        }

        private Rect GetFaceRoiFast(Mat mask)
        {
            var minRatio = 0.6;
            var stopRatio = 0.8;
            var minSize = (int)(mask.Height * 0.16667);
            var maxSize = (int)(mask.Height * 0.8);

            var maxI = -1;
            var maxJ = -1;
            var max = 0.0;
            var sizeModifier = 0.8;
            var roi = new Rect(0, 0, mask.Width, mask.Height);
            var finalSize = 0;
            var integralMask = GetIntegralMask(mask);

            for (var size = maxSize; size >= minSize; size = (int)(size * sizeModifier))
            {
                double ratio;
                var rect = GetFaceRoiFast(integralMask, roi, size, out ratio);

                if (rect == Rect.Empty)
                {
                    break;
                }

                if (ratio < max)
                {
                    break;
                }

                max = ratio;
                roi = rect;
                finalSize = size;

                if (ratio > stopRatio)
                {
                    break;
                }
            }


            if (max < minRatio)
            {
                return Rect.Empty;
            }

            return roi;
        }

        private Rect GetFaceRoi(Mat mask)
        {
            var minRatio = 0.6;
            var stopRatio = 0.8;
            var minSize = (int)(mask.Height * 0.16667);
            var maxSize = (int)(mask.Height * 0.8);

            var maxI = -1;
            var maxJ = -1;
            var max = 0.0;
            var sizeModifier = 0.8;
            var roi = new Rect(0, 0, mask.Width, mask.Height);
            var finalSize = 0;

            for (var size = maxSize; size >= minSize; size = (int)(size * sizeModifier))
            {
                double ratio;
                var rect = GetFaceRoi(mask, roi, size, out ratio);

                if (rect == Rect.Empty)
                {
                    break;
                }

                if (ratio < max)
                {
                    break;
                }

                max = ratio;
                roi = rect;
                finalSize = size;

                if (ratio > stopRatio)
                {
                    break;
                }

            }


            if (max < minRatio)
            {
                return Rect.Empty;
            }

            return roi;
        }

        private Mat GetIntegralMaskMat(Mat mask)
        {
            var mat01 = new Mat();
            Cv2.Threshold(mask, mat01, 10, 1, ThresholdType.Binary);
            var sumMat = new Mat();
            Cv2.Integral(mat01, sumMat);
            return sumMat;
        }

        private int[,] GetIntegralMask(Mat mask)
        {
            var integral = new int[mask.Rows, mask.Cols];
            var mat3 = new MatOfByte(mask);
            var indexer = mat3.GetGenericIndexer<int>();

            for (var i = 0; i < mask.Rows; i++)
            {
                for (var j = 0; j < mask.Cols; j++)
                {
                    var sum = 0;
                    var substr = false;

                    if (i != 0)
                    {
                        if (j != 0)
                        {
                            substr = true;
                        }

                        sum += integral[i - 1, j];
                    }

                    if (j != 0)
                    {
                        sum += integral[i, j - 1];
                    }

                    if (substr)
                    {
                        sum -= integral[i - 1, j - 1];
                    }

                    sum += indexer[i,j] > 0 ? 1 : 0;

                    integral[i, j] = sum;
                }
            }

            return integral;
        }

        private Rect GetFaceRoiFast(int[,] integralMask, Rect roi, int size, out double faceRatio)
        {
            var maxI = -1;
            var maxJ = -1;
            var max = 0.0;

            for (var i = roi.Y; i < roi.Y + roi.Height - size; i += 4)
            {
                for (var j = roi.X; j < roi.X + roi.Width - size; j += 4)
                {
                    var count = integralMask[i + size-1, j + size-1];
                    var add = false;
                    if (i > 0)
                    {
                        if (j > 0)
                        {
                            add = true;
                        }
 
                        count -= integralMask[i - 1, j];                       
                    }
                    if (j > 0)
                    {
                        count -= integralMask[i, j - 1];
                    }

                    if (add)
                    {
                        count += integralMask[i - 1, j - 1];
                    }

                    var ratio = count * 1.0 / (size * size);

                    if (ratio > max)
                    {
                        max = ratio;
                        maxI = i;
                        maxJ = j;
                    }
                }
            }

            faceRatio = max;

            if (maxI == -1)
            {
                return Rect.Empty;
            }

            return new Rect(maxJ, maxI, size, size);
        }

        private Rect GetFaceRoiFastMat(Mat integralMask, Rect roi, int size, out double faceRatio)
        {
            var maxI = -1;
            var maxJ = -1;
            var max = 0.0;

            var mat3 = new MatOfInt(integralMask);
            var indexer = mat3.GetGenericIndexer<int>();

            for (var i = roi.Y; i < roi.Y + roi.Height - size; i += 4)
            {
                for (var j = roi.X; j < roi.X + roi.Width - size; j += 4)
                {
                    var maskI = i + 1;
                    var maskJ = j + 1;

                    var count = indexer[maskI + size - 1, maskJ + size - 1];
                    count -= indexer[maskI - 1, maskJ + size - 1];
                    count -= indexer[maskI + size - 1, maskJ - 1];
                    count += indexer[maskI - 1, maskJ - 1];

                    var ratio = count * 1.0 / (size * size);

                    if (ratio > max)
                    {
                        max = ratio;
                        maxI = i;
                        maxJ = j;
                    }
                }
            }

            faceRatio = max;

            if (maxI == -1)
            {
                return Rect.Empty;
            }

            return new Rect(maxJ, maxI, size, size);
        }

        private Rect GetFaceRoi(Mat mask, Rect roi, int size, out double faceRatio)
        {
            var maxI = -1;
            var maxJ = -1;
            var max = 0.0;

            for (var i = roi.Y; i < roi.Y + roi.Height - size; i += 4)
            {
                for (var j = roi.X; j < roi.X + roi.Width - size; j += 4)
                {
                    var newIm = new Mat(mask, new Rect(j, i, size, size));
                    var count = Cv2.CountNonZero(newIm);
                    var ratio = count * 1.0 / (size * size);

                    if (ratio > max)
                    {
                        max = ratio;
                        maxI = i;
                        maxJ = j;
                    }
                }
            }

            faceRatio = max;

            if (maxI == -1)
            {
                return Rect.Empty;
            }

            return new Rect(maxJ, maxI, size, size);
        }

        /// <summary>
        /// Thread safe method to display image in a picturebox that is set to automatic sizing
        /// </summary>
        /// <param name="Image"></param>
        private delegate void DisplayImageDelegate(Bitmap Image, PictureBox Target);

        private void DisplayImage(Bitmap Image, PictureBox Target)
        {
            if (Target.InvokeRequired)
            {
                try
                {
                    DisplayImageDelegate DI = new DisplayImageDelegate(DisplayImage);
                    this.BeginInvoke(DI, new object[] { Image, Target });
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                Target.Image = Image;
            }
        }

        private void hueUpDown_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
