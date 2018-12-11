using Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace Sunc_web_api.DAL
{
    public class Dal_DCanSendCard
    {
        public Dal_DCanSendCard()
        { }
        /// <summary>
        /// 主要用于查询是否可以发卡
        /// 在就诊表TBL_D_VisitList中根据就诊卡号VL_PI_V_CardCode
        /// 查询VL_I_State发卡状态，VL_I_DocState审核医生状态
        /// </summary>
        /// <param name="SerialNumber">流水号</param>
        /// <returns></returns>
        public DataTable fD_CanSendCard(List<string> SerialNumber)
        {
            StringBuilder sbrSQL = new StringBuilder();
            sbrSQL.Append(" Select VL_I_State,DD_FLAG_CHECK From UCARD_ALLINFO_VIEW  ");
            sbrSQL.Append(" Where VL_V_SerialNumber In ( ");
            sbrSQL.Append(string.Join(",", SerialNumber.ToArray()));
            sbrSQL.Append(" ) ");

            SqlParameter[] para = new SqlParameter[]{
                //new SqlParameter ("@VL_PI_V_CardCode ",CardCode )
             };
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para).Tables[0];
            return dt;
        }


        /// <summary>
        /// 主要用于查询是否可以发卡-------------------twoago
        /// 在就诊表TBL_D_VisitList中根据就诊卡号VL_PI_V_CardCode
        /// 查询VL_I_State发卡状态，VL_I_DocState审核医生状态
        /// </summary>
        /// <param name="SerialNumber">流水号</param>
        /// <returns></returns>
        public DataTable fD_CanSendCard_Tago(List<string> SerialNumber)
        {
            StringBuilder sbrSQL = new StringBuilder();
            sbrSQL.Append(" Select OV_I_State,OD_FLAG_CHECK From UCARD_OFFLINE_INFOVIEW  ");
            sbrSQL.Append(" Where OV_V_SerialNumber In ( ");
            sbrSQL.Append(string.Join(",", SerialNumber.ToArray()));
            sbrSQL.Append(" ) ");

            SqlParameter[] para = new SqlParameter[]{
                //new SqlParameter ("@VL_PI_V_CardCode ",CardCode )
             };
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para).Tables[0];
            return dt;
        }
        /// <summary>
        /// 查询报告的打印状态
        /// </summary>
        /// <param name="SerialNumber"></param>
        /// <returns></returns>
        public DataTable fD_CanMedicalState(List<string> SerialNumber)
        {
            StringBuilder sbrSQL = new StringBuilder();
            sbrSQL.Append("Select VL_I_RePrintState From UCARD_ALLINFO_VIEW ");
            sbrSQL.Append(" Where VL_V_SerialNumber In ( ");
            sbrSQL.Append(string.Join(",", SerialNumber.ToArray()));
            sbrSQL.Append(" ) ");
            SqlParameter[] para = new SqlParameter[] {
            };
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para).Tables[0];
            return dt;
        }
        /// <summary>
        /// 查询贴纸打印状态
        /// </summary>
        /// <param name="SerialNumber"></param>
        /// <returns></returns>
        public DataTable fD_CanSickState(List<string> SerialNumber)
        {
            StringBuilder sbrSQL = new StringBuilder();
            sbrSQL.Append(" Select VL_I_SickPrintState From UCARD_ALLINFO_VIEW ");
            sbrSQL.Append(" Where VL_V_SerialNumber In ( ");
            sbrSQL.Append(string.Join(",", SerialNumber.ToArray()));
            sbrSQL.Append(" ) ");
            SqlParameter[] para = new SqlParameter[] {
             };
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para).Tables[0];
            return dt;
        }
    }
}