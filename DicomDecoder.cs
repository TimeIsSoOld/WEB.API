using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Linq;

//解码DICOM图像

namespace DicomImageViewer
{
    public enum TypeOfDicomFile
    {
        NotDicom,
        Dicom3File,
        DicomOldTypeFile,
        DicomUnknownTransferSyntax
    };

    class DicomDecoder
    {
        const uint PIXEL_REPRESENTATION       = 0x00280103;
        const uint TRANSFER_SYNTAX_UID        = 0x00020010;
        const uint MODALITY                   = 0x00080060;
        const uint SLICE_THICKNESS            = 0x00180050;
        const uint SLICE_SPACING              = 0x00180088;
        const uint SAMPLES_PER_PIXEL          = 0x00280002;
        const uint PHOTOMETRIC_INTERPRETATION = 0x00280004;
        const uint PLANAR_CONFIGURATION       = 0x00280006;
        const uint NUMBER_OF_FRAMES           = 0x00280008;
        const uint ROWS                       = 0x00280010;
        const uint COLUMNS                    = 0x00280011;
        const uint PIXEL_SPACING              = 0x00280030;
        const uint BITS_ALLOCATED             = 0x00280100;
        const uint WINDOW_CENTER              = 0x00281050;
        const uint WINDOW_WIDTH               = 0x00281051;
        const uint RESCALE_INTERCEPT          = 0x00281052;
        const uint RESCALE_SLOPE              = 0x00281053;
        const uint RED_PALETTE                = 0x00281201;
        const uint GREEN_PALETTE              = 0x00281202;
        const uint BLUE_PALETTE               = 0x00281203;
        const uint ICON_IMAGE_SEQUENCE        = 0x00880200;
        const uint PIXEL_DATA                 = 0x7FE00010;

        const string ITEM                     = "FFFEE000";
        const string ITEM_DELIMITATION        = "FFFEE00D";
        const string SEQUENCE_DELIMITATION    = "FFFEE0DD";

        const int
            AE = 0x4145,
            AS = 0x4153,
            AT = 0x4154,
            CS = 0x4353,
            DA = 0x4441,
            DS = 0x4453,
            DT = 0x4454,
            FD = 0x4644,
            FL = 0x464C,
            IS = 0x4953,
            LO = 0x4C4F,
            LT = 0x4C54,
            PN = 0x504E,
            SH = 0x5348,
            SL = 0x534C,
            SS = 0x5353,
            ST = 0x5354,
            TM = 0x544D,
            UI = 0x5549,
            UL = 0x554C,
            US = 0x5553,
            UT = 0x5554,
            OB = 0x4F42,
            OW = 0x4F57,
            SQ = 0x5351,
            UN = 0x554E,
            QQ = 0x3F3F,
            RT = 0x5254;
        const int ID_OFFSET = 128;  // "DICM"位置
        const int IMPLICIT_VR = 0x2D2D; // '--' 
        const String DICM   = "DICM";

        public int bitsAllocated;
        public int width;
        public int height;
        public int offset;
        public int nImages;
        public int samplesPerPixel;
        public double pixelDepth = 1.0;
        public double pixelWidth = 1.0;
        public double pixelHeight = 1.0;
        public string unit;
        public double windowCentre, windowWidth;
        public bool signedImage;
        public TypeOfDicomFile typeofDicomFile;
        public List<string> dicomInfo;
        public bool dicmFound; // “DICM”在偏移128处找到

        DicomDictionary dic;
        BinaryReader file;
        String dicomFileName;
        String photoInterpretation;
        bool littleEndian = true;
        bool oddLocations;  // 奇数位置
        bool bigEndianTransferSyntax = false;
        bool inSequence;
        bool widthTagFound;
        bool heightTagFound;
        bool pixelDataTagFound;
        int location = 0;///当前字节位置记录符
        int elementLength;
        int vr;
        int min8 = Byte.MinValue;
        int max8 = Byte.MaxValue;
        int min16 = short.MinValue;
        int max16 = ushort.MaxValue;
        int pixelRepresentation;
        double rescaleIntercept;
        double rescaleSlope;
        byte[] reds;
        byte[] greens;
        byte[] blues;
        byte[] vrLetters = new byte[2];
        List<byte> pixels8;///单通道 8bit 的像素
        List<byte> pixels24; ///3通道 24bit 的像素
        List<ushort> pixels16;///单通道 16bit 的像素
        List<int> pixels16Int;///？？？？？？？


        public DicomDecoder()
        {
            dic = new DicomDictionary();
            signedImage = false;
            dicomInfo = new List<string>();
            InitializeDicom();
        }

        void InitializeDicom()
        {
            bitsAllocated = 0;
            width = 1;
            height = 1;
            offset = 1;//DICOM像素点开始的地方
            nImages = 1;
            samplesPerPixel = 1;
            photoInterpretation = "";
            unit = "mm";
            windowCentre = 0;
            windowWidth = 0;
            signedImage = false;
            widthTagFound = false;
            heightTagFound = false;
            pixelDataTagFound = false;
            rescaleIntercept = 0.0; // 默认值
            rescaleSlope = 1.0; // 默认值
            typeofDicomFile = TypeOfDicomFile.NotDicom;
        }

        public string DicomFileName
        {
            set
            {
                dicomFileName = value;//赋值文件名
                InitializeDicom();

                file = new BinaryReader(File.Open(dicomFileName, FileMode.Open, FileAccess.Read));//以二进制读取DICOM文件
                location = 0; // 定义全局变量，记录读取到的二进制的位置
                dicomInfo.Clear();
                try
                {
                    bool readResult = ReadFileInfo();///读取DICOM文件各个DataElement的VF，并将其保存起来
                    if (readResult && widthTagFound && heightTagFound && pixelDataTagFound)///成功读到DICOM数据后
                    {
                        ReadPixels();//读取像素
                        if (dicmFound == true)
                            typeofDicomFile = TypeOfDicomFile.Dicom3File;
                        else
                            typeofDicomFile = TypeOfDicomFile.DicomOldTypeFile;
                    }
                }
                catch 
                {
                    
                }
                finally
                {
                    file.Close();
                }
            }
        }

        public void GetPixels8(ref List<byte> pixels)
        {
            pixels = pixels8;
        }

        public void GetPixels16(ref List<ushort> pixels)
        {
            pixels = pixels16;
        }

        public void GetPixels24(ref List<byte> pixels)
        {
            pixels = pixels24;
        }

        String GetString(int length)
        {
            byte[] buf = new byte[length];
            file.BaseStream.Position = location;
            int count = file.Read(buf, 0, length);////从location处开始读取
            ///将 index 0 作为字节数组中的起始点，从流中读取 length 个字节,存放到buf中。
            location += length;
            string s = System.Text.ASCIIEncoding.ASCII.GetString(buf);//将二进制转化为字符串
            return s;
        }

        byte GetByte() //读取当前位置上的一个字节
        {
            file.BaseStream.Position = location;//设置当前位置
            byte b = file.ReadByte();//读取当前位置的下一个字节
            ++location;
            return b;
        }

        ushort GetShort() // 返回16bit无符号短类型
        {
            byte b0 = GetByte();///读取下一个字节
            byte b1 = GetByte();///读取下一个字节
            ushort s;
            if (littleEndian)////Litte字节序
                s = Convert.ToUInt16((b1 << 8) + b0);///
            else//Big字节序
                s = Convert.ToUInt16((b0 << 8) + b1);///？？？？
            return s;
        }

        int GetInt()//4Byte
        {
            byte b0 = GetByte();
            byte b1 = GetByte();
            byte b2 = GetByte();
            byte b3 = GetByte();
            int i;
            if (littleEndian)
                i = (b3 << 24) + (b2 << 16) + (b1 << 8) + b0;
            else
                i = (b0 << 24) + (b1 << 16) + (b2 << 8) + b3;
            return i;
        }

        double GetDouble()
        {
            byte b0 = GetByte();
            byte b1 = GetByte();
            byte b2 = GetByte();
            byte b3 = GetByte();
            byte b4 = GetByte();
            byte b5 = GetByte();
            byte b6 = GetByte();
            byte b7 = GetByte();

            long res = 0;
            if (littleEndian)
            {
                res += b0;
                res += (((long)b1) << 8);
                res += (((long)b2) << 16);
                res += (((long)b3) << 24);
                res += (((long)b4) << 32);
                res += (((long)b5) << 40);
                res += (((long)b6) << 48);
                res += (((long)b7) << 56);
            }
            else
            {
                res += b7;
                res += (((long)b6) << 8);
                res += (((long)b5) << 16);
                res += (((long)b4) << 24);
                res += (((long)b3) << 32);
                res += (((long)b2) << 40);
                res += (((long)b1) << 48);
                res += (((long)b0) << 56);
            }

            double d = Convert.ToDouble(res, new CultureInfo("en-US"));
            return d;
        }

        float GetFloat()
        {
            byte b0 = GetByte();
            byte b1 = GetByte();
            byte b2 = GetByte();
            byte b3 = GetByte();

            int res = 0;

            if (littleEndian)
            {
                res += b0;
                res += (((int)b1) << 8);
                res += (((int)b2) << 16);
                res += (((int)b3) << 24);
            }
            else
            {
                res += b3;
                res += (((int)b2) << 8);
                res += (((int)b1) << 16);
                res += (((int)b0) << 24);
            }

            float f1;
            f1 = Convert.ToSingle(res, new CultureInfo("en-US"));
            return f1;
        }

        byte[] GetLut(int length)
        {
            if ((length & 1) != 0) // odd
            {
                String dummy = GetString(length);
                return null;
            }

            length /= 2;
            byte[] lut = new byte[length];
            for (int i = 0; i < length; ++i)
                lut[i] = Convert.ToByte(GetShort() >> 8);
            return lut;
        }

        int GetLength()
        {
            byte b0 = GetByte();
            byte b1 = GetByte();
            byte b2 = GetByte();
            byte b3 = GetByte();

            // 如果没有完整的Dicom数据字典，就不可能知道VR是隐式的还是显式的 
            vr = (b0 << 8) + b1;//Tag后2Byte可能存放VR值（显式）
            //VR 2Byte

            switch (vr)////DICOM中DataElement的三种结构
            {
                case OB:
                case OW:
                case SQ:
                case UN:
                case UT:
                    // 如果其他两个字节为零，则使用32位长度的显式VR
                    //b2 b3 为预留位 空白值
                    if ((b2 == 0) || (b3 == 0)) return GetInt();//再获取4Byte//数据长度
                    // 32位的隐式VR
                    vr = IMPLICIT_VR;///？？？？？？
                    if (littleEndian)
                        return ((b3 << 24) + (b2 << 16) + (b1 << 8) + b0);
                    else
                        return ((b0 << 24) + (b1 << 16) + (b2 << 8) + b3);
                    
                case AE:
                case AS:
                case AT:
                case CS:
                case DA:
                case DS:
                case DT:
                case FD:
                case FL:
                case IS:
                case LO:
                case LT:
                case PN:
                case SH:
                case SL:
                case SS:
                case ST:
                case TM:
                case UI:
                case UL:
                case US:
                case QQ:
                case RT:
                    // 16位的显式VR
                    if (littleEndian)
                        return ((b3 << 8) + b2);
                    else
                        return ((b2 << 8) + b3);
                default:
                    // 32位隐式VR
                    vr = IMPLICIT_VR;
                    if (littleEndian)
                        return ((b3 << 24) + (b2 << 16) + (b1 << 8) + b0);
                    else
                        return ((b0 << 24) + (b1 << 16) + (b2 << 8) + b3);
            }
        }


        /// <summary>
        /// 返回int Tag，赋值ElementLength
        /// </summary>
        /// <returns></returns>
        int GetNextTag()//读取下一个Tag
        {
            int groupWord = GetShort();//读取组号 2Byte
            if (groupWord == 0x0800 && bigEndianTransferSyntax)
            {
                littleEndian = false;
                groupWord = 0x0008;
            }
            int elementWord = GetShort();//读取元素号 2Byte
            int tag = groupWord << 16 | elementWord;
            
            elementLength = GetLength();///获取到数据长度（数据元素存放的数据的长度）///******

            //读取GE文件
            if (elementLength == 13 && !oddLocations)
                elementLength = 10;

            // "Undefined"元素长度.
            // 这是一个包含一系列元素的括号
            if (elementLength == -1)
            {
                elementLength = 0;
                inSequence = true;
            }
            return tag;
        }

        String GetHeaderInfo(int tag, String value)
        {
            string str = tag.ToString("X8");////？？？？
            if (str == ITEM_DELIMITATION || str == SEQUENCE_DELIMITATION)
            {
                inSequence = false;
                return null;
            }

            string id = null;
            
            if (dic.dict.ContainsKey(str))
            {
                id = dic.dict[str];
                if (id != null)
                {
                    if (vr == IMPLICIT_VR)
                        vr = (id[0] << 8) + id[1];
                    id = id.Substring(2);
                }
            }

            if (str == ITEM)
                return (id != null ? id : ":null");
            if (value != null)
                return id + ": " + value;

            switch (vr)
            {
                case FD:
                    for (int i = 0; i < elementLength; ++i)
                        GetByte();
                    break;
                case FL:
                    for (int i = 0; i < elementLength; i++)
                        GetByte();
                    break;
                case AE:
                case AS:
                case AT:
                case CS:
                case DA:
                case DS:
                case DT:
                case IS:
                case LO:
                case LT:
                case PN:
                case SH:
                case ST:
                case TM:
                case UI:
                    value = GetString(elementLength);
                    break;
                case US:
                    if (elementLength == 2)
                        value = Convert.ToString(GetShort());
                    else
                    {
                        value = "";
                        int n = elementLength / 2;
                        for (int i = 0; i < n; i++)
                            value += Convert.ToString(GetShort()) + " ";
                    }
                    break;
                case IMPLICIT_VR:
                    value = GetString(elementLength);
                    if (elementLength > 44)
                        value = null;
                    break;
                case SQ:
                    value = "";
                    bool privateTag = ((tag >> 16) & 1) != 0;
                    if (tag != ICON_IMAGE_SEQUENCE && !privateTag)
                        break;
                    goto default;
                default:
                    location += elementLength;
                    value = "";
                    break;
            }

            if (value != null && id == null && value != "")
                return "---: " + value;
            else if (id == null)
                return null;
            else
                return id + ": " + value;
        }

        void AddInfo(int tag, string value)
        {
            string info = GetHeaderInfo(tag, value);

            string str = tag.ToString("X");
            string strPadded = str.PadLeft(8, '0');
            string strInfo;
            if (inSequence && info != null && vr != SQ)
                info = ">" + info;
            if (info != null && str != ITEM)
            {
                if (info.Contains("---"))
                    strInfo = info.Replace("---", "Private Tag");
                else
                    strInfo = info;

                dicomInfo.Add(strPadded + "//" + strInfo);
            }
        }

        void AddInfo(int tag, int value)
        {
            AddInfo(tag, Convert.ToString(value));
        }

        /// <summary>
        /// ///赋值像素行距，列距
        /// </summary>
        /// <param name="scale"></param>
        void GetSpatialScale(String scale)
        {
            double xscale = 0, yscale = 0;
            int i = scale.IndexOf('\\');
            if (i == 1) 
            {
                yscale = Convert.ToDouble(scale.Substring(0, i), new CultureInfo("en-US"));
                xscale = Convert.ToDouble(scale.Substring(i + 1), new CultureInfo("en-US"));
            }
            if (xscale != 0.0 && yscale != 0.0)
            {
                pixelWidth = xscale;
                pixelHeight = yscale;
                unit = "mm";
            }
        }

        public bool ReadFileInfo()/////读取各个DataElement中的VF，并将其保存起来
        {
            long skipCount = Convert.ToInt32(ID_OFFSET);//数据元素起始地址128
            bitsAllocated = 16;

            file.BaseStream.Seek(skipCount, SeekOrigin.Begin);//访问skipCount二进制
            location += ID_OFFSET;

            if (GetString(4) != DICM)//128字节后的四个字节存放“DICM”
                //GetString()是自定义方法：从当前位置起，读取指定字节数，并返回为字符串类型
            {
                //用于读取3.0之前的DICOM文件
               
                // 找文件开始处
                file.BaseStream.Seek(0, SeekOrigin.Begin);
                location = 0;

                // 3.0之前的DICOM文件没有序言和前缀( preamble and prefix）
                dicmFound = false;

              
                // 继续读取信息.
                //查看宽、高、像素标签是否存在。
                //如果这些标记存在，那么我们就会得出结论，这是一个DICOM文件，
                //因为所有的DICOM文件都应该至少有这三个标记。
                //否则，它不是DICOM文件。
                
            }
            else
            {
                // true表示文件是3.0的版本
                dicmFound = true;
            }

            /////接下来读取DICOM文件信息
            bool decodingTags = true;
            samplesPerPixel = 1;///通道数为1 
            int planarConfiguration = 0;
            photoInterpretation = "";
            string modality;

            while (decodingTags)
            {
                int tag = GetNextTag();///读取下一个Tag
                if ((location & 1) != 0)
                    oddLocations = true;
                
                if (inSequence)/////未定义标签？？？？？？
                {   
                    AddInfo(tag, null);                     
                    continue;
                }

                string s;
                switch (tag)
                {
                    case (int)(TRANSFER_SYNTAX_UID):///00020010决定普通Tag的读取方式Little-Endlian还是Big-Endlian,以及VR的显式隐式
                        s = GetString(elementLength);//读取elementLength长度的数据
                        AddInfo(tag, s);
                        ////Tag为00020010时可以通过VF值判断Little/Big，VR显式/隐式
                        if (s.IndexOf("1.2.4") > -1 || s.IndexOf("1.2.5") > -1)///字符串匹配返回第一次出现的位置
                        {
                            file.Close();
                            typeofDicomFile = TypeOfDicomFile.DicomUnknownTransferSyntax;
                            // 表示无法处理
                            return false;
                        }
                        if (s.IndexOf("1.2.840.10008.1.2.2") >= 0)
                            bigEndianTransferSyntax = true;
                        break;
                    case (int)MODALITY:///检查模态(MRI/CT/CR/DR)
                        modality = GetString(elementLength);
                        AddInfo(tag, modality);
                        break;
                    case (int)(NUMBER_OF_FRAMES):
                        s = GetString(elementLength);
                        AddInfo(tag, s);
                        double frames = Convert.ToDouble(s, new CultureInfo("en-US"));
                        if (frames > 1.0)
                            nImages = (int)frames;
                        break;
                    case (int)(SAMPLES_PER_PIXEL):///图像上的采样率.
                        samplesPerPixel = GetShort();////采样率
                        AddInfo(tag, samplesPerPixel);
                        break;
                    case (int)(PHOTOMETRIC_INTERPRETATION):///光度计的解释,对于CT图像，用两个枚举值MONOCHROME1，MONOCHROME
                       //2.用来判断图像是否是彩色的，MONOCHROME1/2是灰度图， RGB则是真彩色图，还有其他.
                        photoInterpretation = GetString(elementLength);
                        photoInterpretation = photoInterpretation.Trim();
                        AddInfo(tag, photoInterpretation);
                        break;
                    case (int)(PLANAR_CONFIGURATION):
                        planarConfiguration = GetShort();
                        AddInfo(tag, planarConfiguration);
                        break;
                    case (int)(ROWS)://图像的总行数，行分辨率.
                        height = GetShort();
                        AddInfo(tag, height);
                        heightTagFound = true;
                        break;
                    case (int)(COLUMNS)://图像的总列数，列分辨率.
                        width = GetShort();
                        AddInfo(tag, width);
                        widthTagFound = true;
                        break;
                    case (int)(PIXEL_SPACING)://像素间距.像素中心之间的物理间距.
                        String scale = GetString(elementLength);
                        GetSpatialScale(scale);///赋值像素行距，列距
                        AddInfo(tag, scale);
                        break;
                    case (int)(SLICE_THICKNESS)://层厚
                    case (int)(SLICE_SPACING)://层与层之间的间距,单位为mm
                        String spacing = GetString(elementLength);
                        pixelDepth = Convert.ToDouble(spacing, new CultureInfo("en-US"));
                        AddInfo(tag, spacing);
                        break;
                    case (int)(BITS_ALLOCATED)://分配的位数:存储每一个像素值时分配的位数，每一个样本应该拥有相同的这个值.
                        bitsAllocated = GetShort();
                        AddInfo(tag, bitsAllocated);
                        break;
                    case (int)(PIXEL_REPRESENTATION)://像素数据的表现类型:这是一个枚举值，分别为十六进制数0000和0001.0000H = 无符号整数，0001H = 2的补码.
                        pixelRepresentation = GetShort();
                        AddInfo(tag, pixelRepresentation);
                        break;
                    case (int)(WINDOW_CENTER)://窗位*******
                        String center = GetString(elementLength);
                        int index = center.IndexOf('\\');
                        if (index != -1) center = center.Substring(index + 1);
                        windowCentre = Convert.ToDouble(center, new CultureInfo("en-US"));///**********
                        AddInfo(tag, center);
                        break;
                    case (int)(WINDOW_WIDTH)://窗宽*******
                        String widthS = GetString(elementLength);
                        index = widthS.IndexOf('\\');
                        if (index != -1) widthS = widthS.Substring(index + 1);
                        windowWidth = Convert.ToDouble(widthS, new CultureInfo("en-US"));////**********
                        AddInfo(tag, widthS);
                        break;
                    case (int)(RESCALE_INTERCEPT):///截距:如果表明不同模态的LUT颜色对应表不存在时,则使用方程Units = m*SV + b,计算真实的像素值到呈现像素值。其中这个值为表达式中的b。
                        String intercept = GetString(elementLength);
                        rescaleIntercept = Convert.ToDouble(intercept, new CultureInfo("en-US"));
                        AddInfo(tag, intercept);
                        break;
                    case (int)(RESCALE_SLOPE):///斜率.这个值为表达式中的m
                        String slop = GetString(elementLength);
                        rescaleSlope = Convert.ToDouble(slop, new CultureInfo("en-US"));
                        AddInfo(tag, slop);
                        break;
                    case (int)(RED_PALETTE):
                        reds = GetLut(elementLength);
                        AddInfo(tag, elementLength / 2);
                        break;
                    case (int)(GREEN_PALETTE):
                        greens = GetLut(elementLength);
                        AddInfo(tag, elementLength / 2);
                        break;
                    case (int)(BLUE_PALETTE):
                        blues = GetLut(elementLength);
                        AddInfo(tag, elementLength / 2);
                        break;
                    case (int)(PIXEL_DATA):
                        // 图像数据开始处
                        if (elementLength != 0)
                        {
                            offset = location;
                            AddInfo(tag, location);
                            decodingTags = false;
                        }
                        else
                            AddInfo(tag, null);
                        pixelDataTagFound = true;
                        break;
                    default:
                        AddInfo(tag, null);/////不用的Tag暂时赋值为空
                        break;
                }
            }
            return true;
        }

        void ReadPixels()
        {
            if (samplesPerPixel == 1 &&  bitsAllocated == 8)///单通道 8bit
            {
                if (pixels8 != null)
                    pixels8.Clear();
                pixels8 = new List<byte>();
                int numPixels = width * height;
                byte[] buf = new byte[numPixels];////存放读取到的像素点
                file.BaseStream.Position = offset;////DICOM像素点开始的地方
                file.Read(buf, 0, numPixels);

                for (int i = 0; i < numPixels; ++i)
                {
                    int pixVal = (int)(buf[i] * rescaleSlope + rescaleIntercept);
                    // 将8位图像转换到0-255
                    //if (photoInterpretation.Equals("MONOCHROME1", StringComparison.OrdinalIgnoreCase))
                    //    pixVal = 65535 - pixVal;
                    if (photoInterpretation == "MONOCHROME1")
                        pixVal = max8 - pixVal;

                    pixels8.Add((byte)(pixelRepresentation == 1 ? pixVal : (pixVal - min8)));
                }
            }

            if (samplesPerPixel == 1 &&  bitsAllocated == 16)//单通道 16bit
            {
                if (pixels16 != null)
                    pixels16.Clear();
                if (pixels16Int != null)
                    pixels16Int.Clear();

                pixels16 = new List<ushort>();
                pixels16Int = new List<int>();
                int numPixels = width * height;
                byte[] bufByte = new byte[numPixels * 2];
                byte[] signedData = new byte[2];
                file.BaseStream.Position = offset;
                file.Read(bufByte, 0, numPixels * 2);
                ushort unsignedS;
                int i, i1, pixVal;
                byte b0, b1;               

                for (i = 0; i < numPixels; ++i)
                {
                    i1 = i * 2;
                    b0 = bufByte[i1];
                    b1 = bufByte[i1 + 1];
                    unsignedS = Convert.ToUInt16((b1 << 8) + b0);
                    if (pixelRepresentation == 0) // Unsigned
                    {
                        pixVal = (int)(unsignedS * rescaleSlope + rescaleIntercept);
                        if (photoInterpretation == "MONOCHROME1")
                            pixVal = max16 - pixVal;
                    }
                    else  // 像素表示为1，表示2s补码图像
                    {                        
                        signedData[0] = b0;
                        signedData[1] = b1;
                        short sVal = System.BitConverter.ToInt16(signedData, 0);

                        // 需要考虑重新调整斜率和截取以计算最终像素值
                        pixVal = (int)(sVal * rescaleSlope + rescaleIntercept);
                        if (photoInterpretation == "MONOCHROME1")
                            pixVal = max16 - pixVal;
                    }
                    pixels16Int.Add(pixVal);
                }

                int minPixVal = pixels16Int.Min();
                signedImage = false;
                if (minPixVal < 0) signedImage = true;

                // 使用上面的像素数据填充列表pixels16
                foreach (int pixel in pixels16Int)
                {
                    // 将所有16位图像转换到0 - 65535
                    if (signedImage)
                        pixels16.Add((ushort)(pixel - min16));
                    else
                        pixels16.Add((ushort)(pixel));
                }

                pixels16Int.Clear();
            }
            
            // Ultrasound images处理
            if (samplesPerPixel == 3 && bitsAllocated == 8)///RGB 3*8=24bit
            {
                signedImage = false;
                if (pixels24 != null)
                    pixels24.Clear();
                pixels24 = new List<byte>();
                int numPixels = width * height;
                int numBytes = numPixels * samplesPerPixel;
                byte[] buf = new byte[numBytes];
                file.BaseStream.Position = offset;
                file.Read(buf, 0, numBytes);

                for (int i = 0; i < numBytes; ++i)
                {
                    pixels24.Add(buf[i]);
                }
            }
        }
    }
}
