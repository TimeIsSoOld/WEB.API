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
    public class Dal_DIsCardLegal
    {

        /// <summary>
        /// 从病人信息表TBL_B_PatientInfo中查找所有就诊卡号PI_V_CardCode
        /// 用于判断就诊卡是否合法
        /// </summary>
        /// <returns></returns>
        public DataTable fD_SelectCardCode()
        {
            StringBuilder sbrSQL = new StringBuilder();
            sbrSQL.Append("Select PI_V_CardCode From TBL_B_PatientInfo");

            SqlParameter[] para = new SqlParameter[]{
             };
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para).Tables[0];
            return dt;
        }
        /// <summary>
        /// 通过就诊卡号查询病人信息
        /// </summary>
        /// <param name="CardCode"></param>
        /// <returns></returns>
        public DataTable fD_SelectPatientInfoByCardCode(string CardCode)
        {
            StringBuilder sbrSQL = new StringBuilder();
            sbrSQL.Append(" Select   PI_V_CardCode,PI_V_Name,VL_V_StudyBodyPart  From UCARD_ALLINFO_VIEW ");
            sbrSQL.Append(" WHERE PI_V_CardCode=@PI_V_CardCode ");
            SqlParameter[] para = new SqlParameter[]{
                new SqlParameter("@PI_V_CardCode",CardCode)
            };
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para).Tables[0];
            return dt;
        }

        public DataTable fD_getBCR(string SerialNumber, string ProcessNumber)
        {
            StringBuilder sbrSQL = new StringBuilder();
            DataTable dt = new DataTable();
            if (SerialNumber != null && ProcessNumber == null)
            {
                sbrSQL.Append(" Select  VL_PI_V_CardCode   ");
                sbrSQL.Append(" From TBL_D_VisitList");
                sbrSQL.Append("  where VL_V_SerialNumber = @VL_V_SerialNumber ");

                SqlParameter[] para = new SqlParameter[]
                {
                    new SqlParameter("@VL_V_SerialNumber",SerialNumber)
                };
                dt = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para).Tables[0];
            }
            if (SerialNumber == null && ProcessNumber != null)
            {
                sbrSQL.Append(" Select  VL_PI_V_CardCode  From TBL_D_VisitList ");
                sbrSQL.Append("  where VL_V_ProcessNumber=@VL_V_ProcessNumber ");
                SqlParameter[] para = new SqlParameter[]
                {
                    new SqlParameter("@VL_V_ProcessNumber",ProcessNumber)
                };
                dt = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para).Tables[0];
            }
            if (SerialNumber != null && ProcessNumber != null)
            {
                sbrSQL.Append(" Select  VL_PI_V_CardCode   ");
                sbrSQL.Append(" From TBL_D_VisitList");
                sbrSQL.Append("  where VL_V_SerialNumber = @VL_V_SerialNumber ");

                SqlParameter[] para = new SqlParameter[]
                {
                    new SqlParameter("@VL_V_SerialNumber",SerialNumber)
                };
                dt = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para).Tables[0];
            }
            return dt;
        }


        public DataTable fD_getBCR(List<string> SerialNumber, string ProcessNumber)
        {
            StringBuilder sbrSQL = new StringBuilder();
            DataTable dt = new DataTable();
            if (SerialNumber != null && ProcessNumber == null)
            {
                sbrSQL.Append(" Select  VL_PI_V_CardCode   ");
                sbrSQL.Append(" From TBL_D_VisitList");
                sbrSQL.Append("  where VL_V_SerialNumber = @VL_V_SerialNumber ");

                SqlParameter[] para = new SqlParameter[]
                {
                    new SqlParameter("@VL_V_SerialNumber",SerialNumber[0])
                };
                dt = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para).Tables[0];

            }
            if (SerialNumber == null && ProcessNumber != null)
            {
                sbrSQL.Append(" Select  VL_PI_V_CardCode  From TBL_D_VisitList ");
                sbrSQL.Append("  where VL_V_ProcessNumber=@VL_V_ProcessNumber ");
                SqlParameter[] para = new SqlParameter[]
                {
                    new SqlParameter("@VL_V_ProcessNumber",ProcessNumber)
                };
                dt = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para).Tables[0];
            }
            if (SerialNumber != null && ProcessNumber != null)
            {
                sbrSQL.Append(" Select  VL_PI_V_CardCode   ");
                sbrSQL.Append(" From TBL_D_VisitList");
                sbrSQL.Append("  where VL_V_SerialNumber = @VL_V_SerialNumber ");

                SqlParameter[] para = new SqlParameter[]
                {
                    new SqlParameter("@VL_V_SerialNumber",SerialNumber[0])
                };
                dt = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para).Tables[0];

            }

            return dt;
        }


        public DataTable fD_getBCR_agotwo(List<string> SerialNumber, string ProcessNumber)
        {
            StringBuilder sbrSQL = new StringBuilder();
            DataTable dt = new DataTable();
            if (SerialNumber != null && ProcessNumber == null)
            {
                sbrSQL.Append(" Select  OV_PI_V_CardCode   ");
                sbrSQL.Append(" From UCARD_OFFLINE_INFOVIEW");
                sbrSQL.Append("  where OV_V_SerialNumber = @VL_V_SerialNumber ");

                SqlParameter[] para = new SqlParameter[]
                {
                    new SqlParameter("@VL_V_SerialNumber",SerialNumber[0])
                };
                dt = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para).Tables[0];

            }
            if (SerialNumber == null && ProcessNumber != null)
            {
                sbrSQL.Append(" Select  OV_PI_V_CardCode  From UCARD_OFFLINE_INFOVIEW ");
                sbrSQL.Append("  where OV_V_ProcessNumber=@VL_V_ProcessNumber ");
                SqlParameter[] para = new SqlParameter[]
                {
                    new SqlParameter("@VL_V_ProcessNumber",ProcessNumber)
                };
                dt = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para).Tables[0];
            }
            if (SerialNumber != null && ProcessNumber != null)
            {
                sbrSQL.Append(" Select  OV_PI_V_CardCode   ");
                sbrSQL.Append(" From UCARD_OFFLINE_INFOVIEW");
                sbrSQL.Append("  where OV_V_SerialNumber = @VL_V_SerialNumber ");

                SqlParameter[] para = new SqlParameter[]
                {
                    new SqlParameter("@VL_V_SerialNumber",SerialNumber[0])
                };
                dt = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para).Tables[0];

            }

            return dt;
        }
    }
}