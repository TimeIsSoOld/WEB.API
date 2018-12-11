using Sunc_web_api.DAL;
using Sunc_web_api.MDL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Sunc_web_api.BLL
{
    public class Bll_BCanSendCard
    {
        /// <summary>
        /// 此方法可以查询指定就诊卡号的
        /// 发卡状态VL_I_State，审核医生状态VL_I_DocState
        /// 返回一个就诊表Mdl_MVisitList对象
        /// </summary>
        /// <returns></returns>
        public  List<Mdl_VisitList> fB_VisitListInfo(List<string> SerialNumber)
        {
            List<Mdl_VisitList> visitlistList = new List<MDL.Mdl_VisitList>();
            Dal_DCanSendCard canSendCard = new DAL.Dal_DCanSendCard();
            DataTable dt = canSendCard.fD_CanSendCard(SerialNumber);

            foreach (DataRow dr in dt.Rows)
            {
                Mdl_VisitList visitlist = new Mdl_VisitList();
                try
                {
                    visitlist.VL_I_State = int.Parse(dr["VL_I_State"].ToString());     
                    if (Convert.ToInt32(dr["DD_FLAG_CHECK"]) == 0)
                    {
                        visitlist.DD_FLAG_CHECK = false;
                    }
                    else
                    {
                        visitlist.DD_FLAG_CHECK = true ;
                    }
                }
                catch (Exception ex)
                {
                }
                visitlistList.Add(visitlist);
            }
            return visitlistList;
        }



        /// <summary>
        /// 此方法可以查询指定就诊卡号的------twoago
        /// 发卡状态VL_I_State，审核医生状态VL_I_DocState
        /// 返回一个就诊表Mdl_MVisitList对象
        /// </summary>
        /// <returns></returns>
        public List<Mdl_OFFLINE_VISITLIST> fB_VisitListInfo_Tago(List<string> SerialNumber)
        {
            List<Mdl_OFFLINE_VISITLIST> visitlistList = new List<MDL.Mdl_OFFLINE_VISITLIST>();
            Dal_DCanSendCard canSendCard = new DAL.Dal_DCanSendCard();
            DataTable dt = canSendCard.fD_CanSendCard_Tago(SerialNumber);

            foreach (DataRow dr in dt.Rows)
            {
                Mdl_OFFLINE_VISITLIST visitlist = new Mdl_OFFLINE_VISITLIST();
                try
                {
                    visitlist.OV_I_State = int.Parse(dr["OV_I_State"].ToString());
                    if (Convert.ToInt32(dr["OD_FLAG_CHECK"]) == 0)
                    {
                        visitlist.OD_FLAG_CHECK = false;
                    }
                    else
                    {
                        visitlist.OD_FLAG_CHECK = true;
                    }
                }
                catch (Exception ex)
                {
                }
                visitlistList.Add(visitlist);
            }
            return visitlistList;
        }

        /// <summary>
        /// 获取指定的流水号下的报告打印状态并返回bool值
        /// </summary>
        /// <param name="SerialNumber"></param>
        /// <returns></returns>
        public bool fB_CanMedicalState(List<string> SerialNumber)
        {
            bool CanMedicalState = false;
            List<Mdl_VisitList> visitlistList = new List<MDL.Mdl_VisitList>();
            Dal_DCanSendCard canMedicalState = new DAL.Dal_DCanSendCard();
            DataTable dt = canMedicalState.fD_CanMedicalState(SerialNumber);
            foreach (DataRow dr in dt.Rows)
            {
                Mdl_VisitList visitlist = new Mdl_VisitList();
                try
                {
                    visitlist.VL_I_RePrintState = int.Parse(dr["VL_I_RePrintState"].ToString());
                }
                catch (Exception)
                {
                }
                visitlistList.Add(visitlist);
            }
            bool jilu = true;//记录值，记录查询出来的结果中是否有不满足条件的记录
            foreach (Mdl_VisitList vl in visitlistList)
            {
                if (vl.VL_I_RePrintState == 0)
                {
                    CanMedicalState = true;
                }
                else
                {
                    jilu = false;
                }
            }
            if (jilu)
            { return CanMedicalState; }
            else
            { return jilu; }
        }

        /// <summary>
        /// 查询指定流水号下的贴纸打印状态并返回bool值
        /// </summary>
        /// <param name="SerialNumber"></param>
        /// <returns></returns>
        public bool fB_CanSickState(List<string> SerialNumber)
        {
            bool CanSickState = false;
            List<Mdl_VisitList> visitlistList = new List<Mdl_VisitList>();
            Dal_DCanSendCard canSickState = new DAL.Dal_DCanSendCard();
            DataTable dt = canSickState.fD_CanSickState(SerialNumber);
            foreach (DataRow dr in dt.Rows)
            {
                Mdl_VisitList visitlist = new Mdl_VisitList();
                try
                {
                    visitlist.VL_I_SickPrintState = int.Parse(dr["VL_I_SickPrintState"].ToString());
                }
                catch (Exception)
                {
                }
                visitlistList.Add(visitlist);
            }
            bool jilu = true;//记录值，记录查询出来的结果中是否有不满足条件的记录
            foreach (Mdl_VisitList vl in visitlistList)
            {
                if (vl.VL_I_SickPrintState == 0)
                {
                    CanSickState = true;
                }
                else
                {
                    jilu = false;
                }
            }
            if (jilu)//判断记录是否都满足条件，决定返回的值
            { return CanSickState; }
            else
                return jilu;
        }
    }
}