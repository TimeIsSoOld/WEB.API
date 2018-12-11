using Sunc_web_api.DAL;
using Sunc_web_api.MDL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Sunc_web_api.BLL
{
    public class Bll_BIsCardLegal
    {
        private bool legal = false;//用于记录就诊卡是否非法
        Dal_DIsCardLegal dIsCardLegal = new Dal_DIsCardLegal();

        /// <summary>
        /// 判断就诊卡是否是非法卡
        /// 参数是就诊卡号
        /// 返回值是判断结果，true:合法卡  false：非法卡
        /// </summary>
        /// <param name="cardCode"></param>
        /// <returns></returns>
        public bool fB_IsCardLegal(string CardCode)
        {
            List<Mdl_MPatientInfo> templist = fB_SelectPatientInfoByCardCode(CardCode);
            if (templist.Count > 0) return true;
            return false;
        }

        /// <summary>
        /// 通过就诊卡查询病人信息
        /// 赋值PI_V_CardCode，PI_V_Name，PI_V_HpNO
        /// </summary>
        /// <param name="CardCode"></param>
        /// <returns></returns>
        public List<Mdl_MPatientInfo> fB_SelectPatientInfoByCardCode(string CardCode)
        {
            List<Mdl_MPatientInfo> patientInfoList = new List<Mdl_MPatientInfo>();

            DataTable dt = dIsCardLegal.fD_SelectPatientInfoByCardCode(CardCode);

            foreach (DataRow dr in dt.Rows)
            {
                Mdl_MPatientInfo patientInfo = new Mdl_MPatientInfo();
                patientInfo.PI_V_CardCode = dr["PI_V_CardCode"].ToString();
                patientInfo.PI_V_Name = dr["PI_V_Name"].ToString();
                patientInfo.VL_V_StudyBodyPart = dr["VL_V_StudyBodyPart"].ToString();
                patientInfoList.Add(patientInfo);
            }
            return patientInfoList;
        }

        /// <summary>
        /// 获取就诊卡号
        /// </summary>
        /// <param name="SerialNumber"></param>
        /// <param name="ProcessNumber"></param>
        /// <returns></returns>
        public string Fb_getBCR(string SerialNumber, string ProcessNumber)
        {
            
            DataTable dt = dIsCardLegal.fD_getBCR(SerialNumber, ProcessNumber);

          string   visitlist = dt.Rows[0]["VL_PI_V_CardCode"].ToString();
            return visitlist;

        }

        /// <summary>
        /// 获取就诊卡号
        /// </summary>
        /// <param name="SerialNumber">LIST类型</param>
        /// <param name="ProcessNumber"></param>
        /// <returns></returns>
        public string Fb_getBCR(List<string> SerialNumber, string ProcessNumber)
        {       
            DataTable dt = dIsCardLegal.fD_getBCR(SerialNumber, ProcessNumber);
            string visitlist = dt.Rows[0]["VL_PI_V_CardCode"].ToString();
            return visitlist;
        }

        /// <summary>
        /// 获取就诊卡号-----------ago
        /// </summary>
        /// <param name="SerialNumber">LIST类型</param>
        /// <param name="ProcessNumber"></param>
        /// <returns></returns>
        public string Fb_getBCR_twoago(List<string> SerialNumber, string ProcessNumber)
        {
            DataTable dt = dIsCardLegal.fD_getBCR_agotwo(SerialNumber, ProcessNumber);
            string visitlist = dt.Rows[0]["OV_PI_V_CardCode"].ToString();
            return visitlist;
        }
    }
}