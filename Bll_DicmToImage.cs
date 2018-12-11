using DicomImageViewer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunc_web_api.BLL
{
    public class Bll_DicmToImage
    {

        public enum ImageBitsPerPixel { Eight, Sixteen, TwentyFour };
        public enum ViewSettings { Zoom1_1, ZoomToFit };
        public int Width;   //控件宽度
        public int Height;  //控件高度


        const double DBLNULL = -999999999F;

        private double winCD = DBLNULL;
        private double winWD = DBLNULL;

        private int _mousetypei = 0;
        /// <summary>
        /// 鼠标滚轮当前状态
        /// </summary>
        public int MouseTypeI
        {
            set { _mousetypei = value; }
            get { return _mousetypei; }
        }


        public Bll_DicmToImage()
        {
  
            //dd = new DicomDecoder();
            pixels8 = new List<byte>();
            pixels16 = new List<ushort>();
            pixels24 = new List<byte>();
            imageOpened = false;
            signedImage = false;
            maxPixelValue = 0;
            minPixelValue = 65535;
            /*--------------------------*/

            pix8 = new List<byte>();
            pix16 = new List<ushort>();
            pix24 = new List<byte>();

            //this.hScrollBar.Visible = false;
            //this.vScrollBar.Visible = false;

            winMin = 0;
            winMax = 65535;

            ptWLDown = new Point();
            changeValWidth = 0.5;
            changeValCentre = 20.0;
            rightMouseDown = false;
            imageAvailable = false;
            signed16Image = false;

            lut8 = new byte[256];
            lut16 = new byte[65536];


            viewSettingsChanged = false;///**************

        }

        #region 变量 -------------------------------------------------------


        Sunc_web_api.BLL.Bll_ImageWriter bImagewriter = new Sunc_web_api.BLL.Bll_ImageWriter();

        int imageWidth;
        int imageHeight;
        int bitDepth;
        double winCentreD = DBLNULL;    //窗宽
        double winWidthD = DBLNULL;     //窗位
        int samplesPerPixel;
        bool signedImage;

        List<byte> pixels8;
        List<ushort> pixels16;
        List<byte> pixels24;

        bool imageOpened;
        
        //double winCentre;
        //double winWidth;
        int maxPixelValue;
        int minPixelValue;


        DicomDecoder dd = new DicomDecoder();
        public List<string> FileListFullName = new List<string>();  //图片列表（全名称）
        public List<string> FileList = new List<string>();  //图片列表（文件名）
        int _FileNowIndex = 0;
        #endregion

        #region 接口--------------------------------------------------

        #region 图像处理事件-----------------------------

        /// <summary>
        /// 旋转
        /// </summary>
        /// <param name="angleI">角度</param>
        /// <returns></returns>
        public bool fB_SetImageRotate(RotateFlipType RotateNow)
        {
            viewSettingsChanged = true;
            newImage = true;
            wheel = true;

            bmp.RotateFlip(RotateNow);

            return true;
        }
        #endregion


        /// <summary>
        /// 更换显示图片
        /// </summary>
        /// <param name="filefullnamimage"></param>
        /// <returns></returns>
        public Bitmap fB_ChangeImage(string filefullnamimage)
        {
            dd.DicomFileName = filefullnamimage;

            TypeOfDicomFile typeOfDicomFile = dd.typeofDicomFile;

            if (typeOfDicomFile == TypeOfDicomFile.Dicom3File ||
                typeOfDicomFile == TypeOfDicomFile.DicomOldTypeFile)
            {
                imageWidth = dd.width;
                imageHeight = dd.height;
                bitDepth = dd.bitsAllocated;

                if (winCD == DBLNULL) winCD = dd.windowCentre;
                if (winWD == DBLNULL) winWD = dd.windowWidth;



                //if (winCentreD == DBLNULL) winCentreD = dd.windowCentre;
                //if (winWidthD == DBLNULL) winWidthD = dd.windowWidth;

                winCentreD = winCD;
                winWidthD = winWD;


                samplesPerPixel = dd.samplesPerPixel;
                signedImage = dd.signedImage;

                this.NewImage = true;

                if (samplesPerPixel == 1 && bitDepth == 8)
                {
                    pixels8.Clear();
                    pixels16.Clear();
                    pixels24.Clear();
                    dd.GetPixels8(ref pixels8);

                    minPixelValue = pixels8.Min();
                    maxPixelValue = pixels8.Max();

                    // Bug fix dated 24 Aug 2013 - for proper window/level of signed images
                    // Thanks to Matias Montroull from Argentina for pointing this out.
                    if (dd.signedImage)
                    {
                        winCentreD -= char.MinValue;
                    }

                    if (Math.Abs(winWidthD) < 0.001)
                    {
                        winWidthD = maxPixelValue - minPixelValue;
                    }

                    if ((winCentreD == 0) ||
                        (minPixelValue > winCentreD) || (maxPixelValue < winCentreD))
                    {
                        winCentreD = (maxPixelValue + minPixelValue) / 2;
                    }

                    SetParameters(ref pixels8, imageWidth, imageHeight,
                        winWidthD, winCentreD, samplesPerPixel, true);
                }

                if (samplesPerPixel == 1 && bitDepth == 16)
                {
                    pixels16.Clear();
                    pixels8.Clear();
                    pixels24.Clear();
                    dd.GetPixels16(ref pixels16);

                    minPixelValue = pixels16.Min();
                    maxPixelValue = pixels16.Max();

                    // Bug fix dated 24 Aug 2013 - for proper window/level of signed images
                    // Thanks to Matias Montroull from Argentina for pointing this out.
                    if (dd.signedImage)
                    {
                        winCentreD -= short.MinValue;
                    }

                    if (Math.Abs(winWidthD) < 0.001)
                    {
                        winWidthD = maxPixelValue - minPixelValue;
                    }

                    if ((winCentreD == 0) ||
                        (minPixelValue > winCentreD) || (maxPixelValue < winCentreD))
                    {
                        winCentreD = (maxPixelValue + minPixelValue) / 2;
                    }

                    Signed16Image = dd.signedImage;

                    SetParameters(ref pixels16, imageWidth, imageHeight,
                        winWidthD, winCentreD, true);
                }

                if (samplesPerPixel == 3 && bitDepth == 8)
                {
                    // This is an RGB colour image
                    pixels8.Clear();
                    pixels16.Clear();
                    pixels24.Clear();
                    dd.GetPixels24(ref pixels24);

                    SetParameters(ref pixels24, imageWidth, imageHeight,
                        winWidthD, winCentreD, samplesPerPixel, true);
                }
            }
            else
            {
                if (typeOfDicomFile == TypeOfDicomFile.DicomUnknownTransferSyntax)
                {
                    //MessageBox.Show("Sorry, I can't read a DICOM file with this Transfer Syntax.",
                    //    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    //MessageBox.Show("Sorry, I can't open this file. " +
                    //    "This file does not appear to contain a DICOM image.",
                    //    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                //Text = "DICOM Image Viewer: ";
                // Show a plain grayscale image instead
                try
                {
                    pixels8.Clear();
                    pixels16.Clear();
                    pixels24.Clear();
                    samplesPerPixel = 1;

                    imageWidth = this.Width - 25;   // 25 is a magic number
                    imageHeight = this.Height - 25; // Same magic number
                    int iNoPix = imageWidth * imageHeight;

                    for (int i = 0; i < iNoPix; ++i)
                    {
                        pixels8.Add(240);// 240 is the grayvalue corresponding to the Control colour
                    }
                    winWidthD = 256;
                    winCentreD = 127;
                    this.SetParameters(ref pixels8, imageWidth, imageHeight,
                        winWidthD, winCentreD, samplesPerPixel, true);

                }
                catch (Exception)
                { }

                //this.Invalidate();
            }

            //fv_BitMapCopy(ref btmtmp, bmp); //复制图片
            //fv_ImageWriter(ref btmtmp,ddlist.Count-1); //添加文字
            fv_ImageWriter(ref bmp);

            ComputeScrollBarParameters();//显示位置
            return bmp;

        }


        /// <summary>
        /// 设置窗位、窗宽
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void fV_SetWindows(int winC, int winW)
        {
            //winC = winWidth - winC;
            //winW = winCentre - winW;
            winCD = winC;     //保存窗宽
            winWD = winW;       //保存窗位
            int w = 0;
            int c = 0;

            w = winWidth - winW;
            c = WinCenterI - winC;

            //do
            //{
            DetermineMouseSensitivity();

            winWidthBy2 = winWidth / 2;
            winWidth = winMax - winMin;
            winCentre = winMin + winWidthBy2;

            //if (winWidth == winW)
            //{
            //    deltaX = 0;
            //}
            //else
            //{
            //    deltaX = w;
            //}
            //if (WinCenterI == winC)
            //{
            //    deltaY = 0;
            //}
            //else
            //{
            //    deltaY = c;// (int)(c * changeValCentre);
            //}
            deltaY = c;// (int)(c * changeValCentre);
            deltaX = w;// (int)(w * changeValCentre);
            //deltaY = c;

            winCentre -= deltaY;
            winWidth -= deltaX;
            WinCenterI -= deltaY;

            if (winWidth < 2) winWidth = 2;
            winWidthBy2 = winWidth / 2;

            winMax = winCentre + winWidthBy2;
            winMin = winCentre - winWidthBy2;

            if (winMin >= winMax) winMin = winMax - 1;
            if (winMax <= winMin) winMax = winMin + 1;

            //ptWLDown.X = e.X;
            //ptWLDown.Y = e.Y;

            UpdateMainForm();
            if (bpp == ImageBitsPerPixel.Eight)
            {
                ComputeLookUpTable8();
                CreateImage8();
            }
            else if (bpp == ImageBitsPerPixel.Sixteen)
            {

                //for (int i = 0; i < _bitmapdicmList.Count; i++)
                //{
                //    //if (_fileNowIndex != i)
                //    //{

                //        ComputeLookUpTable16();
                //        Bitmap t = new Bitmap(_bitmapdicmList[i]);
                //        CreateImage16(ref t, _pix16Now);
                //        _bitmapdicmList[_fileNowIndex] = new Bitmap(t);
                //    //}

                //}
                ComputeLookUpTable16();

                //int dddd = _fileNowIndex;
                //_bmpNow = _bitmapdicmList[_fileNowIndex];

                CreateImage16(ref bmp, pix16);

                fv_ImageWriter(ref bmp);


            }
            else // (bpp == ImageBitsPerPixel.TwentyFour)
            {
                ComputeLookUpTable8();
                CreateImage24();
            }
            
        }
        /*
        /// <summary>
        /// 图片复位时，调用对应文件
        /// </summary>
        /// <param name="filefullnamimage"></param>
        /// <returns></returns>
        public bool fB_ChangeImageReset()
        {
            winCD = DBLNULL;
            winWD = DBLNULL;
            wlreset = 1;
            mouseMoveX = mouseMoveY = 0;

            fB_ChangeImage(FileListFullName[_FileNowIndex]);

            //this.Invalidate();
            return true;
        }
        */
        int WinCenterI = 0;

        /// <summary>
        /// 向图片添加文字
        /// </summary>
        /// <param name="btmtmp"></param>
        /// <param name="ddlistI"></param>
        private void fv_ImageWriter(ref Bitmap btmtmp)
        {

            Sunc_web_api.BLL.DicomTags.str = dd.dicomInfo;
            string tagname, tagvalue;

            //左上
            string leftotpText = "";

            Sunc_web_api.BLL.DicomTags.SetString("00100010", out tagname, out tagvalue);
            leftotpText += tagvalue.Trim() + ",";

            Sunc_web_api.BLL.DicomTags.SetString("00101010", out tagname, out tagvalue);
            leftotpText += tagvalue.Trim() + ",";
            Sunc_web_api.BLL.DicomTags.SetString("00100040", out tagname, out tagvalue);
            // leftotpText +=tagvalue.Substring(0,4) +"-"+ tagvalue.Substring(4, 2) + "-" +tagvalue.Substring(6, 2) + "\n\r";
            leftotpText += tagvalue.Trim() + ",";
            Sunc_web_api.BLL.DicomTags.SetString("00100020", out tagname, out tagvalue);
            leftotpText += tagvalue.Trim() + "\n\r";

            Sunc_web_api.BLL.DicomTags.SetString("0008103E", out tagname, out tagvalue);
            leftotpText += tagvalue.Trim() + ",";
            Sunc_web_api.BLL.DicomTags.SetString("00080012", out tagname, out tagvalue);
            leftotpText += tagvalue.Trim() + " ";
            Sunc_web_api.BLL.DicomTags.SetString("00080013", out tagname, out tagvalue);
            leftotpText += tagvalue.Trim() + "\n\r";
            // leftotpText += "Fr: " + _fileNowIndex;

            //窗宽、窗位
            Sunc_web_api.BLL.DicomTags.SetString("00281050", out tagname, out tagvalue);
            try
            {
                if (WinCenterI == 0) WinCenterI = Convert.ToInt32(tagvalue);
                //if (wlreset == 1)
                //{
                //    WinCenterI = Convert.ToInt32(tagvalue);
                //    wlreset = 0;
                //}
            }
            catch (Exception)
            {
                WinCenterI = winCentre;
            }

            leftotpText += " WL: " + WinCenterI + ",";
            Sunc_web_api.BLL.DicomTags.SetString("00281051", out tagname, out tagvalue);
            leftotpText += " WW: " + winWidth;
            Font font = null;
            SizeF sizef = new SizeF();

            bImagewriter.fB_SetFontSize(ref font, ref sizef, btmtmp, leftotpText, btmtmp.Width);

            bImagewriter.DrawWords(
                ref btmtmp
                , leftotpText
                , 1F
                , Color.FromArgb(255, 255, 255, 0)
                , StringAlignment.Near
                , Sunc_web_api.BLL.ImagePosition.LeftTop
                , font
                );

            //上中
            string topMiddle = "";
            topMiddle = "A";
            bImagewriter.DrawWords(
               ref btmtmp
               , topMiddle
               , 1F
               , Color.FromArgb(255, 255, 255, 0)
               , StringAlignment.Near
               , Sunc_web_api.BLL.ImagePosition.TopMiddle
                , font
                );

            //下中
            string bottomMiddle = "";
            bottomMiddle = "P";
            bImagewriter.DrawWords(
               ref btmtmp
               , bottomMiddle
               , 1F
               , Color.FromArgb(255, 255, 255, 0)
               , StringAlignment.Near
               , Sunc_web_api.BLL.ImagePosition.BottomMiddle
               , font

               );

            //右中
            string leftMiddle = "";
            leftMiddle = "L";
            bImagewriter.DrawWords(
               ref btmtmp
               , leftMiddle
               , 1F
               , Color.FromArgb(255, 255, 255, 0)
               , StringAlignment.Near
               , Sunc_web_api.BLL.ImagePosition.LeftMiddle
               , font

               );

            //左中
            string rightMiddle = "";
            rightMiddle = "R";
            bImagewriter.DrawWords(
               ref btmtmp
               , rightMiddle
               , 1F
               , Color.FromArgb(255, 255, 255, 0)
               , StringAlignment.Near
               , Sunc_web_api.BLL.ImagePosition.RightMiddle
               , font

               );
            //btmtmp = new Bitmap(btmtmp);
            //左下
            string leftbttomText = "";
            bImagewriter = new Sunc_web_api.BLL.Bll_ImageWriter();

            Sunc_web_api.BLL.DicomTags.SetString("00180060", out tagname, out tagvalue);
            leftbttomText += "kv: " + tagvalue.Trim() + "\n\r";

            Sunc_web_api.BLL.DicomTags.SetString("00181151", out tagname, out tagvalue);
            leftbttomText += "mA: " + tagvalue.Trim() + "\n\r";
            Sunc_web_api.BLL.DicomTags.SetString("00181120", out tagname, out tagvalue);
            // leftotpText +=tagvalue.Substring(0,4) +"-"+ tagvalue.Substring(4, 2) + "-" +tagvalue.Substring(6, 2) + "\n\r";
            try
            {
                leftbttomText += "Gantry/Detector Tilt：" + Math.Round(float.Parse(tagvalue), 2).ToString().Trim() + " mm\n\r";
            }
            catch (Exception)
            { }


            bImagewriter.DrawWords(
            ref btmtmp
            , leftbttomText
            , 1F
            , Color.FromArgb(255, 255, 255, 0)
            , StringAlignment.Near
            , Sunc_web_api.BLL.ImagePosition.LeftBottom
            , font

            );



            //右上
            string righttopText = "";
            bImagewriter = new Sunc_web_api.BLL.Bll_ImageWriter();

            Sunc_web_api.BLL.DicomTags.SetString("00080080", out tagname, out tagvalue);
            righttopText += tagvalue.Trim() + "\n\r";

            Sunc_web_api.BLL.DicomTags.SetString("00081090", out tagname, out tagvalue);
            righttopText += "GE MEDICAL SYSTEMS " + tagvalue.Trim() + "\n\r";

            Sunc_web_api.BLL.DicomTags.SetString("00200011", out tagname, out tagvalue);
            righttopText += "Se:" + tagvalue.Trim() + ",";
            Sunc_web_api.BLL.DicomTags.SetString("00200013", out tagname, out tagvalue);
            righttopText += "Im: " + tagvalue.Trim();

            bImagewriter.DrawWords(
            ref btmtmp
            , righttopText
            , 1F
            , Color.FromArgb(255, 255, 255, 0)
            , StringAlignment.Far
            , Sunc_web_api.BLL.ImagePosition.RightTop
            , font

            );

            //右下
            string rightbottomText = "";
            bImagewriter = new Sunc_web_api.BLL.Bll_ImageWriter();

            Sunc_web_api.BLL.DicomTags.SetString("00181150", out tagname, out tagvalue);
            rightbottomText += tagvalue.Trim() + " ms" + "\n\r";

            Sunc_web_api.BLL.DicomTags.SetString("00181100", out tagname, out tagvalue);
            try
            {
                rightbottomText += " DFOV: " + Math.Round(float.Parse(tagvalue), 2) + " mm\n\r";
            }
            catch (Exception)
            { }


            bImagewriter.DrawWords(
            ref btmtmp
            , rightbottomText
            , 1F
            , Color.FromArgb(255, 255, 255, 0)
            , StringAlignment.Far
            , Sunc_web_api.BLL.ImagePosition.RigthBottom
            , font

            );

        }

        //static System.Threading.Thread thrLoad;
        ///// <summary>
        ///// 停止导入图片线程
        ///// </summary>
        ///// <returns></returns>
        //public bool fB_SetThreadAbort()
        //{
        //    try
        //    {
        //        thrLoad.Abort();
        //        _bitmapdicmList = new List<Bitmap>();
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //    return true;
        //}
        /// <summary>
        /// 更换显示图片
        /// </summary>
        /// <param name="filefullnamimage"></param>
        /// <returns></returns>
        //public bool fB_ChangeImageList(List<string> filefullnamimage,int i=0)
        //{
        //    try
        //    {
        //        thrLoad.Abort();
        //        _bitmapdicmList = new List<Bitmap>();
        //    }
        //    catch (Exception)
        //    {
        //    }

        //    System.Threading.Thread.Sleep(50);
        //    _fileNowIndex = i;
        //    _filefullnamimage = filefullnamimage;
        //    //添加图片列表中第一个图片
        //    fB_ChangeImage(_filefullnamimage[0]);
        //    //_bitmapdicmList.Add(bmp);

        //    thrLoad = new System.Threading.Thread(fv_getbitmap);
        //    thrLoad.Start();

        //    //RotateImage(dicomhandler.gdiImg, 90);
        //    //bitmapdicm = RotateImage(dicomhandler.gdiImg, 90);
        //    //this.Text = "DicomViewer-" + fileName;


        //    return true;
        //}

        //当前显示文件位置

        /*
        public bool fB_ChageImageIndex(int i)
        {
            _FileNowIndex = i;
            fB_ChangeImage(FileListFullName[i]);
            this.Invalidate();

            return true;
        }

        int wlreset = 0;

        /// <summary>
        /// bitmap2 复制数据 bitmap1
        /// </summary>
        /// <param name="bitmap1"></param>
        /// <param name="bitmap2"></param>
        private void fv_BitMapCopy(ref Bitmap bitmap1, Bitmap bitmap2)
        {
            for (int i = 0; i < bitmap2.Width; i++)
            {
                for (int j = 0; j < bitmap2.Height; j++)
                {
                    bitmap1.SetPixel(i, j, bitmap2.GetPixel(i, j));
                }
            }
        }
        */
        #endregion






        List<byte> pix8;
        List<ushort> pix16;
        List<byte> pix24;
        public Bitmap bmp;
        public int hOffset;
        public int vOffset;
        int hMax;
        int vMax;
        int imgWidth;
        int imgHeight;
        int panWidth;
        int panHeight;
        bool newImage;

        // 用于窗位
        int winMin;
        int winMax;
        int winCentre;
        int winWidth;
        int winWidthBy2;
        int deltaX;
        int deltaY;

        public Point ptWLDown;
        double changeValWidth;
        double changeValCentre;
        bool rightMouseDown;
        bool imageAvailable;
        bool signed16Image;

        byte[] lut8;
        byte[] lut16;
        byte[] imagePixels8;
        byte[] imagePixels16;
        byte[] imagePixels24;
        int sizeImg;
        int sizeImg3;
        //MainForm mf;
        public bool wheel;
        bool scale;
        public SizeF bsizef;
        float tempa;//**********
        float tempb;//**********

        ImageBitsPerPixel bpp;

        public ViewSettings viewSettings;///**设置图片显示大小**/////
        public bool viewSettingsChanged; ///**记录是否显示这个尺寸下的图片***////


        public bool NewImage
        {
            set
            {
                newImage = value;
            }
        }
        public bool Wheel
        {
            set
            {
                wheel = value;
            }
        }
        public bool Scale
        {
            set
            {
                scale = value;
            }
        }
        public bool Signed16Image
        {
            set { signed16Image = value; }
        }

        public void SetParameters(ref List<byte> arr, int wid, int hei, double windowWidth,
            double windowCentre, int samplesPerPixel, bool resetScroll)
        {
            if (samplesPerPixel == 1)
            {
                bpp = ImageBitsPerPixel.Eight;
                imgWidth = wid;
                imgHeight = hei;
                winWidth = Convert.ToInt32(windowWidth);
                winCentre = Convert.ToInt32(windowCentre);
                changeValWidth = 0.1;
                changeValCentre = 20.0;
                sizeImg = imgWidth * imgHeight;
                sizeImg3 = sizeImg * 3;

                pix8 = arr;
                imagePixels8 = new byte[sizeImg3];

                //mf = mainFrm;
                imageAvailable = true;
                if (bmp != null)
                    bmp.Dispose();
                ResetValues();
                ComputeLookUpTable8();
                bmp = new Bitmap(imgWidth, imgHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                CreateImage8();
            }

            if (samplesPerPixel == 3)
            {
                bpp = ImageBitsPerPixel.TwentyFour;
                imgWidth = wid;
                imgHeight = hei;
                winWidth = Convert.ToInt32(windowWidth);
                winCentre = Convert.ToInt32(windowCentre);
                changeValWidth = 0.1;
                changeValCentre = 0.1;
                sizeImg = imgWidth * imgHeight;
                sizeImg3 = sizeImg * 3;

                pix24 = arr;
                imagePixels24 = new byte[sizeImg3];

                //mf = mainFrm;
                imageAvailable = true;
                if (bmp != null)
                    bmp.Dispose();
                ResetValues();
                ComputeLookUpTable8();
                bmp = new Bitmap(imgWidth, imgHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                CreateImage24();
            }
            //if (resetScroll == true) ComputeScrollBarParameters();
            //Invalidate();
        }

        void DetermineMouseSensitivity()
        {
            // 根据窗宽，设置滑动鼠标时图像的改变大小
            if (winWidth < 10)
            {
                changeValWidth = 0.1;
            }
            else if (winWidth >= 20000)
            {
                changeValWidth = 40;
            }
            else
            {
                changeValWidth = 0.1 + (winWidth - 10) / 500.0;
            }

            changeValCentre = changeValWidth;
        }

        public void SetParametersReset(ref List<ushort> arr, int wid, int hei, double windowWidth,
            double windowCentre, bool resetScroll)
        {
            bpp = ImageBitsPerPixel.Sixteen;
            imgWidth = wid;
            imgHeight = hei;
            winWidth = Convert.ToInt32(windowWidth);
            winCentre = Convert.ToInt32(windowCentre);

            sizeImg = imgWidth * imgHeight;
            sizeImg3 = sizeImg * 3;
            double sizeImg3By4 = sizeImg3 / 4.0;

            DetermineMouseSensitivity();

            pix16 = arr;
            imagePixels16 = new byte[sizeImg3];

            //mf = mainFrm;
            imageAvailable = true;
            if (bmp != null)
                bmp.Dispose();
            ResetValues();
            ComputeLookUpTable16();
            bmp = new Bitmap(imgWidth, imgHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            CreateImage16();

            //Invalidate();
        }
        public void SetParameters(ref List<ushort> arr, int wid, int hei, double windowWidth,
            double windowCentre, bool resetScroll)
        {
            bpp = ImageBitsPerPixel.Sixteen;
            imgWidth = wid;
            imgHeight = hei;
            winWidth = Convert.ToInt32(windowWidth);
            winCentre = Convert.ToInt32(windowCentre);

            sizeImg = imgWidth * imgHeight;
            sizeImg3 = sizeImg * 3;
            double sizeImg3By4 = sizeImg3 / 4.0;

            DetermineMouseSensitivity();

            pix16 = arr;
            imagePixels16 = new byte[sizeImg3];

            //mf = mainFrm;
            imageAvailable = true;
            if (bmp != null)
                bmp.Dispose();
            ResetValues();
            ComputeLookUpTable16();
            bmp = new Bitmap(imgWidth, imgHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            CreateImage16();

            //Invalidate();
        }

        // 使用8位灰度像素数据创建位图
        private void CreateImage8()
        {
            if (bmp == null) return;

            BitmapData bmd = bmp.LockBits(new Rectangle(0, 0, imgWidth, imgHeight),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat);

            unsafe
            {
                int pixelSize = 3;
                int i, j, j1, i1;
                byte b;

                for (i = 0; i < bmd.Height; ++i)
                {
                    byte* row = (byte*)bmd.Scan0 + (i * bmd.Stride);
                    i1 = i * bmd.Width;

                    for (j = 0; j < bmd.Width; ++j)
                    {
                        b = lut8[pix8[i * bmd.Width + j]];
                        j1 = j * pixelSize;
                        row[j1] = b;            // R
                        row[j1 + 1] = b;        // G
                        row[j1 + 2] = b;        // B
                    }
                }
            }
            bmp.UnlockBits(bmd);
        }

        // 使用24位RGB像素数据，创建一个位图
        private void CreateImage24()
        {
            if (bmp == null) return;
            {
                int numBytes = imgWidth * imgHeight * 3;
                int j;
                int i, i1;

                BitmapData bmd = bmp.LockBits(new Rectangle(0, 0, bmp.Width,
                    bmp.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);

                int width3 = bmd.Width * 3;

                unsafe
                {
                    for (i = 0; i < bmd.Height; ++i)
                    {
                        byte* row = (byte*)bmd.Scan0 + (i * bmd.Stride);
                        i1 = i * bmd.Width * 3;

                        for (j = 0; j < width3; j += 3)
                        {
                            // 由于Windows使用低字节序（little-endian）所以RGB实际存储为BGR 
                            row[j + 2] = lut8[pix24[i1 + j]];     // B
                            row[j + 1] = lut8[pix24[i1 + j + 1]]; // G
                            row[j] = lut8[pix24[i1 + j + 2]];     // R
                        }
                    }
                }
                bmp.UnlockBits(bmd);
            }
        }

        // 使用8位灰度像素数据创建位图
        private void CreateImage16()
        {
            if (bmp == null) return;
            BitmapData bmd = bmp.LockBits(new Rectangle(0, 0, imgWidth, imgHeight),
               System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat);

            unsafe
            {
                int pixelSize = 3;
                int i, j, j1, i1;
                byte b;

                for (i = 0; i < bmd.Height; ++i)
                {
                    byte* row = (byte*)bmd.Scan0 + (i * bmd.Stride);
                    i1 = i * bmd.Width;

                    for (j = 0; j < bmd.Width; ++j)
                    {
                        b = lut16[pix16[i * bmd.Width + j]];
                        j1 = j * pixelSize;
                        row[j1] = b;            // R
                        row[j1 + 1] = b;        // G
                        row[j1 + 2] = b;        // B
                    }
                }
            }

            bmp.UnlockBits(bmd);
        }

        private void CreateImage16(ref Bitmap bitmapchange, List<ushort> pix16now)
        {
            if (bitmapchange == null) return;
            BitmapData bmd = bitmapchange.LockBits(new Rectangle(0, 0, imgWidth, imgHeight),
               System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmapchange.PixelFormat);

            unsafe
            {
                int pixelSize = 3;
                int i, j, j1, i1;
                byte b;

                for (i = 0; i < bmd.Height; ++i)
                {
                    byte* row = (byte*)bmd.Scan0 + (i * bmd.Stride);
                    i1 = i * bmd.Width;

                    for (j = 0; j < bmd.Width; ++j)
                    {
                        //b = lut16[pix16[i * bmd.Width + j]];
                        b = lut16[pix16now[i * bmd.Width + j]];
                        j1 = j * pixelSize;
                        row[j1] = b;            // R
                        row[j1 + 1] = b;        // G
                        row[j1 + 2] = b;        // B
                    }
                }
            }
            bitmapchange.UnlockBits(bmd);
        }

        private void fv_SetNewUshort(ref List<ushort> pix1, List<ushort> pix2)
        {
            for (int i = 0; i < pix2.Count; i++)
            {
                ushort p1 = new ushort();
                pix1.Add(p1);
                pix1[i] = pix2[i];
            }
        }

        private void ComputeScrollBarParameters()
        {
            if (bmp == null) return;

            int widthi = bmp.Width;
            int heighti = bmp.Height;
            int pnlwidthi = this.Width;
            int pnlheighti = this.Height;

            float hf = (float)heighti / pnlheighti;
            float wf = (float)widthi / pnlwidthi;

            int bsizefheight = Convert.ToInt32(heighti / wf);
            int bsizefwidth = pnlwidthi;

            if (bsizefheight > pnlheighti)
            {
                bsizefwidth = Convert.ToInt32(widthi / hf);
                bsizefheight = pnlheighti;
            }

            bsizef = new SizeF(bsizefwidth, bsizefheight);


            panWidth = this.Width;
            panHeight = this.Height;

            hOffset = (panWidth - (int)bsizef.Width) / 2;
            vOffset = (panHeight - (int)bsizef.Height) / 2;
            if (hOffset < 0) hOffset = 0;
            if (vOffset < 0) vOffset = 0;

        }

        int mouseMoveX = 0;
        int mouseMoveY = 0;
        public double imageEnlargeI = 30;  //图片放大倍数

        /// <summary>
        /// 放大
        /// </summary>
        /// <returns></returns>
        public bool fB_imageZoomEnlarye()
        {
            double heightF = (bsizef.Width + imageEnlargeI) / bsizef.Width * bsizef.Height;

            bsizef = new SizeF(
                Convert.ToInt32(bsizef.Width + imageEnlargeI)
                , Convert.ToInt32(heightF)
                );
            hOffset = (panWidth - (int)bsizef.Width) / 2;
            vOffset = (panHeight - (int)bsizef.Height) / 2;

            //this.Invalidate();


            return true;
        }
        /// <summary>
        /// 缩小
        /// </summary>
        /// <returns></returns>
        public bool fB_imageZoomNarrow()
        {
            double heightF = (bsizef.Width - imageEnlargeI) / bsizef.Width * bsizef.Height;

            bsizef = new SizeF(
                Convert.ToInt32(bsizef.Width - imageEnlargeI)
                , Convert.ToInt32(heightF)
                );
            hOffset = (panWidth - (int)bsizef.Width) / 2;
            vOffset = (panHeight - (int)bsizef.Height) / 2;

            //this.Invalidate();
            return true;
        }
        //private void ComputeScrollBarParametersEnlargel()
        //{
        //    if (bmp == null) return;

        //    int widthi = bmp.Width;
        //    int heighti = bmp.Height;
        //    int pnlwidthi = bsizef.Width;
        //    int pnlheighti = bsizef.Height;

        //    float hf = (float)heighti / pnlheighti;
        //    float wf = (float)widthi / pnlwidthi;

        //    int bsizefheight = Convert.ToInt32(heighti / wf);
        //    int bsizefwidth = pnlwidthi;

        //    if (bsizefheight > pnlheighti)
        //    {
        //        bsizefwidth = Convert.ToInt32(widthi / hf);
        //        bsizefheight = pnlheighti;
        //    }

        //    bsizef = new SizeF(bsizefwidth, bsizefheight);


        //    panWidth = this.Width;
        //    panHeight = this.Height;

        //    hOffset = (panWidth - (int)bsizef.Width) / 2;
        //    vOffset = (panHeight - (int)bsizef.Height) / 2;
        //    if (hOffset < 0) hOffset = 0;
        //    if (vOffset < 0) vOffset = 0;

        //}

            

        //void ScaleImageKeepingAspectRatio(/*ref Graphics grfx, Bitmap image*/)//*************
        //{
        //    //grfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        //    //grfx.DrawImage(image, (panWidth - bsizef.Width) / 2,
        //    //                      (panHeight - bsizef.Height) / 2, bsizef.Width, bsizef.Height);
        //    Bitmap bp = new Bitmap(600, 600);

        //    //2、获取这块内存画布的Graphics引用：

        //    Graphics w = Graphics.FromImage(bp);

        //    //3、在这块内存画布上绘图：

        //    w.DrawImage(bmp, (panWidth - bsizef.Width) / 2,
        //                          (panHeight - bsizef.Height) / 2, bsizef.Width, bsizef.Height);

        //    //4、将内存画布画到窗口中
        //    this.CreateGraphics().DrawImage(bmp, (panWidth - bsizef.Width) / 2,
        //                          (panHeight - bsizef.Height) / 2, bsizef.Width, bsizef.Height);
        //}

        // 此方法用于将当前文件存储为PNG格式..
        public void SaveImage(String fileName)
        {
            if (bmp != null)
                bmp.Save(fileName, ImageFormat.Png);
        }

        // 使用线性插值方法（Linear interpolation methods）
        private void ComputeLookUpTable8()
        {
            if (winMax == 0)
                winMax = 255;

            int range = winMax - winMin;
            if (range < 1) range = 1;
            double factor = 255.0 / range;

            for (int i = 0; i < 256; ++i)
            {
                if (i <= winMin)
                    lut8[i] = 0;
                else if (i >= winMax)
                    lut8[i] = 255;
                else
                {
                    lut8[i] = (byte)((i - winMin) * factor);
                }
            }
        }

        // 线性插值方法
        private void ComputeLookUpTable16()
        {
            int range = winMax - winMin;
            if (range < 1) range = 1;
            double factor = 255.0 / range;
            int i;

            for (i = 0; i < 65536; ++i)
            {
                if (i <= winMin)
                    lut16[i] = 0;
                else if (i >= winMax)
                    lut16[i] = 255;
                else
                {
                    lut16[i] = (byte)((i - winMin) * factor);
                }
            }
        }
        Point mouseDownPoint;
        bool mouseDownI;
        int mouseMoveXI;
        int mouseMoveYI;

        int mouseMoveXIOld;
        int mouseMoveYIOld;
        


        ////public void Draw(System.Windows.Forms.Panel _panel)
        ////{
        ////    Graphics g = Graphics.FromHwnd(_panel.Handle);
        ////    try
        ////    {
        ////        fv_ImageWriter(ref bmp); //添加文字

        ////        //在内存创建一块和panel一大小的区域
        ////        Bitmap bitmap = new Bitmap(_panel.ClientSize.Width, _panel.ClientSize.Height);
        ////        using (Graphics buffer = Graphics.FromImage(bitmap))
        ////        {
        ////            //buffer中绘图
        ////            buffer.Clear(_panel.BackColor); //用背景色填充画面
        ////            buffer.Transform = new System.Drawing.Drawing2D.Matrix();
        ////            buffer.DrawImage(bmp, (panWidth - bsizef.Width) / 2,
        ////                          (panHeight - bsizef.Height) / 2, bsizef.Width, bsizef.Height); //绘制新画面

        ////            //屏幕绘图
        ////            g.DrawImage(bitmap, 0, 0); //将buffer绘制到屏幕上
        ////        }
        ////    }
        ////    finally
        ////    {
        ////        g.Dispose();
        ////    }
        ////}
        // 在主窗体上更新图形控件
        private void UpdateMainForm()
        {
            //mf.UpdateWindowLevel(winWidth, winCentre, bpp);
        }

        // 重置窗宽窗位为初始值
        public void ResetValues()
        {
            winMax = Convert.ToInt32(winCentre + 0.5 * winWidth);
            winMin = winMax - winWidth;
            UpdateMainForm();
        }
        

        /// <summary>
        /// 旋转与翻转
        /// </summary>
        public void Rotate()
        {
            viewSettingsChanged = true;
            newImage = true;
            wheel = true;
            bmp.RotateFlip(RotateFlipType.Rotate180FlipX);
           
        }
        public void Rotate1()
        {
            //MessageBox.Show("Rotate1");
            viewSettingsChanged = true;
            newImage = true;
            wheel = true;
            bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
            int bank;
            bank = hOffset;
            hOffset = vOffset;
            vOffset = bank;

        }
    }
}
