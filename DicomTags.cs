using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//

namespace Sunc_web_api.BLL
{
    public static  class DicomTags
    {
        public static List<string> str;

        /// <summary>
        /// 获得对应TAG的文本
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="tagname"></param>
        /// <param name="tagvalue"></param>
        public static void SetString(string tag, out string tagname, out string tagvalue)
        {
            tagname = "";
            tagvalue = "";
            //str = strg;
            string s1, s4, s5, s11, s12;

            // 向列表视图控件添加项
            for (int i = 0; i < str.Count; ++i)
            {
                
                if (str[i].IndexOf(tag) > -1)
                {
                    s1 = str[i];
                    ExtractStrings(s1, out s4, out s5, out s11, out s12);
                    if ((s11 + s12).IndexOf(tag) > -1)
                    {
                        tagvalue += s5;
                        tagname += s4;
                        return;
                    }
                }
                
            }
        }

        // 提取DICOM标记中的子字符串，以填充列表框
        public static void SetString(ImagePosition iposition,out string tagname,out string tagvalue)
        {
            tagname = "";
            tagvalue = "";
            //str = strg;
            string s1, s4, s5, s11, s12;

            // 向列表视图控件添加项
            for (int i = 0; i < str.Count; ++i)
            {
                s1 = str[i];
                ExtractStrings(s1, out s4, out s5, out s11, out s12);

                switch (iposition)
                {
                    case ImagePosition.LeftTop:
                        if ("00100010,00100030,00100040,0008103E,00080020 ,00281050,00281051".IndexOf(s11 + s12) > -1)
                            tagvalue +=  s5 + " , ";

                            

                        break;
                    case ImagePosition.LeftBottom:
                        if ("00180060 ,00181151,00181120".IndexOf(s11 + s12) > -1)
                            tagvalue +=s4+":"+ s5 + "\n\r";

                        break;
                    case ImagePosition.RightTop:
                        if ("00080080 ,00081090".IndexOf(s11 + s12) > -1)
                            tagvalue += s5 + "\n\r";

                        break;
                    case ImagePosition.RigthBottom:
                        if ("00181150 ,00181100".IndexOf(s11 + s12) > -1)
                            tagvalue += s4 + ":" + s5 + "\n\r";

                        break;

                    case ImagePosition.TopMiddle:
                        break;
                    case ImagePosition.BottomMiddle:
                        break;
                    case ImagePosition.Center:
                        break;
                    default:
                        break;
                }
                //ListViewItem lvi = new ListViewItem(s11);
                //lvi.SubItems.Add(s12);
                //lvi.SubItems.Add(s4);
                //lvi.SubItems.Add(s5);
                //listView.Items.Add(lvi);
            }
        }


        // 该方法是在Visual Studio中的重构工具中提取的
        static void ExtractStrings(string s1, out string s4, out string s5, out string s11, out string s12)
        {
            int ind;
            string s2, s3;
            ind = s1.IndexOf("//");
            s2 = s1.Substring(0, ind);
            s11 = s1.Substring(0, 4);
            s12 = s1.Substring(4, 4);
            s3 = s1.Substring(ind + 2);
            ind = s3.IndexOf(":");
            s4 = s3.Substring(0, ind);
            s5 = s3.Substring(ind + 1);
        }



    }
}
