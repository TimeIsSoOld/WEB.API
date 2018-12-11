using Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace Sunc_web_api.DAL
{
    public class Dal_TBL_D_VisitList
    {

        public Dal_TBL_D_VisitList()
        { }
        /// <summary>
        /// 查询护士端扫描到的就诊卡号/流水号/注册号的报告信息
        /// </summary>
        /// <returns></returns> 
        public DataTable fm_Select(string BarCodeResult, string SerialNumber, string ProcessNumber)
        {

            StringBuilder sbrSQL = new StringBuilder();
            sbrSQL.Append(" SELECT * ");
            sbrSQL.Append(" FROM  UCARD_ALLINFO_VIEW ");
            //sbrSQL.Append(" LEFT JOIN TBL_D_VisitList AS V ON V.VL_PI_V_CardCode=P.PI_V_CardCode ");
            //sbrSQL.Append(" LEFT JOIN TBL_B_DiseaseDiagnosis AS D ON D.DD_VL_V_SerialNumber = V.VL_V_SerialNumber ");
            //sbrSQL.Append(" LEFT JOIN TBL_B_InspectionMethod AS I ON I.IM_ID=V.VL_I_StudyMethod_ID ");
            //sbrSQL.Append(" LEFT JOIN TBL_B_CourseCategory AS C ON C.CC_ID=V.VL_V_DiseaseCategory_ID ");
            //sbrSQL.Append(" LEFT JOIN TBL_B_DoctorSignature AS S ON S.PS_ID=V.VL_V_ReportID ");
            //sbrSQL.Append(" LEFT JOIN TBL_B_DoctorSignature AS SS ON SS.PS_ID = V.VL_V_VerifyID ");
            sbrSQL.Append(" WHERE ");
            bool b = false;
            if (!string.IsNullOrWhiteSpace(BarCodeResult))
            {
                b = true;
                sbrSQL.Append(" VL_PI_V_CardCode=@VL_PI_V_CardCode ");
            }
            if (!string.IsNullOrWhiteSpace(SerialNumber) && !b)
            {
                b = true;
                sbrSQL.Append(" VL_V_SerialNumber=@VL_V_SerialNumber ");
            }
            if (!string.IsNullOrWhiteSpace(ProcessNumber) && !b)
            {
                sbrSQL.Append(" VL_V_ProcessNumber=@VL_V_ProcessNumber ");
            }


            sbrSQL.Append(" order by VL_D_RegistrationDate desc");


            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@VL_PI_V_CardCode", BarCodeResult),
                new SqlParameter("@VL_V_SerialNumber", SerialNumber),
                new SqlParameter("@VL_V_ProcessNumber", ProcessNumber)
            };
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para).Tables[0];
            return dt;


        }


        /// <summary>
        /// 发卡状态为0的病人信息
        /// </summary>
        /// <param name="BarCodeResult"></param>
        /// <returns></returns>
        public SqlDataReader fm_SelectSelectAotu(string BarCodeResult)
        {
            StringBuilder sbrSQL = new StringBuilder();
            sbrSQL.Append(" SELECT * ");
            sbrSQL.Append(" FROM  UCARD_ALLINFO_VIEW ");
            sbrSQL.Append(" WHERE ");
            sbrSQL.Append(" VL_PI_V_CardCode=@VL_PI_V_CardCode ");
            sbrSQL.Append(" AND VL_I_State=0");

            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@VL_PI_V_CardCode", BarCodeResult)
            };
            return SqlHelper.ExecuteReader(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para);
        }
        /// <summary>
        /// 查询到未发卡的病人姓名
        /// </summary>
        /// <returns></returns>
        public DataTable fm_DSelectName()
        {
            StringBuilder sbrSQL = new StringBuilder();
            sbrSQL.Append("  select TOP 100  PI_V_Name, VL_D_RegistrationDate ");
            sbrSQL.Append("  from UCARD_ALLINFO_VIEW  ");
            sbrSQL.Append("    where VL_I_State = 0 ");
            sbrSQL.Append("    AND DD_FLAG_CHECK=1 ");
            sbrSQL.Append("   order by VL_D_RegistrationDate asc ");
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString()).Tables[0];
            return dt;
        }

        /// <summary>
        /// 查询扫描到的就诊卡号/流水号/注册号的报告信息(2个月前)
        /// </summary>
        /// <returns></returns> 
        public SqlDataReader fm_SelectTwoMonth(string BarCodeResult, string SerialNumber, string ProcessNumber)
        {
            StringBuilder sbrSQL = new StringBuilder();
            sbrSQL.Append(" SELECT * ");
            sbrSQL.Append(" FROM  UCARD_OFFLINE_INFOVIEW ");
            sbrSQL.Append(" WHERE ");
            bool b = false;
            if (!string.IsNullOrWhiteSpace(BarCodeResult))
            {
                b = true;
                sbrSQL.Append(" PIO_V_CardCode=@PIO_V_CardCode ");
            }
            if (!string.IsNullOrWhiteSpace(SerialNumber) && !b)
            {
                b = true;
                sbrSQL.Append(" OV_V_SerialNumber=@OV_V_SerialNumber ");
            }
            if (!string.IsNullOrWhiteSpace(ProcessNumber) && !b)
            {
                sbrSQL.Append(" OV_V_ProcessNumber=@OV_V_ProcessNumber ");
            }


            sbrSQL.Append(" order by OV_D_RegistrationDate desc");


            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@PIO_V_CardCode", BarCodeResult),
                new SqlParameter("@OV_V_SerialNumber", SerialNumber),
                new SqlParameter("@OV_V_ProcessNumber", ProcessNumber)
            };          
            return SqlHelper.ExecuteReader(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para);
        }
        /// <summary>
        /// 查询指定就诊卡号下指定刻录状态的就诊记录
        /// </summary>
        /// <param name="BarCodeResult"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public SqlDataReader fm_SelectBurnRecord(string BarCodeResult, string state)
        {
            StringBuilder sbrSQL = new StringBuilder();
            sbrSQL.Append("SELECT * ");
            sbrSQL.Append(" FROM  UCARD_ALLINFO_VIEW ");
            sbrSQL.Append(" WHERE ");
            sbrSQL.Append(" PI_V_CardCode=@PI_V_CardCode and VL_I_State in(" + state + ") order by VL_D_RegistrationDate desc");
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@PI_V_CardCode", BarCodeResult) };
            return SqlHelper.ExecuteReader(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para);
        }
        /// <summary>
        /// 查询指点就诊卡下指点的流水号报告信息
        /// </summary>
        /// <param name="BarCodeResult"></param>
        /// <param name="SerialNumber"></param>
        /// <returns></returns>
        public SqlDataReader fm_SelectSerialNumber(string BarCodeResult, List<string> SerialNumber)
        {
            StringBuilder sbrSQL = new StringBuilder();
            sbrSQL.Append("SELECT * ");
            sbrSQL.Append(" FROM  UCARD_ALLINFO_VIEW ");
            sbrSQL.Append(" WHERE ");
            sbrSQL.Append(" PI_V_CardCode=@VL_PI_V_CardCode and VL_V_SerialNumber in (");
            sbrSQL.Append(string.Join(",", SerialNumber.ToArray()));
            sbrSQL.Append(" ) order by VL_D_RegistrationDate desc ");


            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@VL_PI_V_CardCode", BarCodeResult),         
            };
            return SqlHelper.ExecuteReader(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para);
        }


        /// <summary>
        /// 查询指点就诊卡下指点的流水号报告信息--------------ago
        /// </summary>
        /// <param name="BarCodeResult"></param>
        /// <param name="SerialNumber"></param>
        /// <returns></returns>
        public SqlDataReader fm_SelectSerialNumber_ago(string BarCodeResult, List<string> SerialNumber)
        {
            StringBuilder sbrSQL = new StringBuilder();
            sbrSQL.Append("SELECT * ");
            sbrSQL.Append(" FROM  UCARD_OFFLINE_INFOVIEW ");
            sbrSQL.Append(" WHERE ");
            sbrSQL.Append(" PIO_V_CardCode=@VL_PI_V_CardCode and OV_V_SerialNumber in (");
            sbrSQL.Append(string.Join(",", SerialNumber.ToArray()));
            sbrSQL.Append(" ) order by OV_D_RegistrationDate desc ");


            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@VL_PI_V_CardCode", BarCodeResult),
            };
            return SqlHelper.ExecuteReader(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para);
        }
        /// <summary>
        /// 从数据库中提取指定的注册号下的打印报告的内容
        /// </summary>
        /// <param name="PI_V_CardCode"></param>
        /// <param name="SerialNumber"></param>
        /// <returns></returns>
        public SqlDataReader fM_patientInfo(string PI_V_CardCode, string SerialNumber)
        {
            StringBuilder sbrSQL = new StringBuilder();
            sbrSQL.Append("SELECT * ");
            sbrSQL.Append(" FROM  UCARD_ALLINFO_VIEW ");
            sbrSQL.Append(" WHERE  PI_V_CardCode=@PI_V_CardCode and VL_V_SerialNumber  in(" + SerialNumber + ") order by VL_D_RegistrationDate desc");
            SqlParameter[] para = new SqlParameter[]
           {
                new SqlParameter("@PI_V_CardCode", PI_V_CardCode),
           };
            return SqlHelper.ExecuteReader(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para);
        }


        /// <summary>
        /// 从数据库中提取指定的注册号下的打印报告的内容------------ago
        /// </summary>
        /// <param name="PI_V_CardCode"></param>
        /// <param name="SerialNumber"></param>
        /// <returns></returns>
        public SqlDataReader fM_patientInfo_ago(string PI_V_CardCode, string SerialNumber)
        {
            StringBuilder sbrSQL = new StringBuilder();
            sbrSQL.Append("SELECT * ");
            sbrSQL.Append(" FROM  UCARD_OFFLINE_INFOVIEW ");
            sbrSQL.Append(" WHERE  PIO_V_CardCode=@PI_V_CardCode and OV_V_SerialNumber in(" + SerialNumber + ") order by OV_D_RegistrationDate desc");
            SqlParameter[] para = new SqlParameter[]
           {
                new SqlParameter("@PI_V_CardCode", PI_V_CardCode),
           };
            return SqlHelper.ExecuteReader(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para);
        }
        /// <summary>
        /// 获取打印的报告内容(移动)
        /// </summary>
        /// <param name="PI_V_CardCode"></param>
        /// <param name="VL_V_ProcessNumber"></param>
        /// <returns></returns>
        public SqlDataReader fM_patientInfoMobile(string PI_V_CardCode)
        {
            StringBuilder sbrSQL = new StringBuilder();
            sbrSQL.Append("SELECT * ");
            sbrSQL.Append(" FROM  UCARD_ALLINFO_VIEW ");
            sbrSQL.Append(" WHERE  PI_V_CardCode=@PI_V_CardCode order by VL_D_RegistrationDate desc");
            SqlParameter[] para = new SqlParameter[]
           {
                new SqlParameter("@PI_V_CardCode", PI_V_CardCode),
           };
            return SqlHelper.ExecuteReader(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para);
        }
        /// <summary>
        /// 重载fM_patientInfoMobile
        /// </summary>
        /// <param name="PI_V_CardCode"></param>
        /// <param name="VL_V_ProcessNumber"></param>
        /// <returns></returns>
        public SqlDataReader fM_patientInfoMobile(string PI_V_CardCode, string VL_V_ProcessNumber)
        {
            StringBuilder sbrSQL = new StringBuilder();
            sbrSQL.Append("SELECT * ");
            sbrSQL.Append(" FROM  UCARD_ALLINFO_VIEW ");
            sbrSQL.Append(" WHERE  PI_V_CardCode=@PI_V_CardCode and VL_V_ProcessNumber in(" + VL_V_ProcessNumber + ") order by VL_D_RegistrationDate desc");
            SqlParameter[] para = new SqlParameter[]
           {
                new SqlParameter("@PI_V_CardCode", PI_V_CardCode),
           };
            return SqlHelper.ExecuteReader(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para);
        }


        public DataTable fM_patientPicture(string PS_V_Number)
        {
            StringBuilder sbrSQL = new StringBuilder();
            sbrSQL.Append(" SELECT PS_V_Picture ");
            sbrSQL.Append(" FROM TBL_B_DoctorSignature ");
            sbrSQL.Append(" WHERE  PS_V_Number=@PS_V_Number ");
            SqlParameter[] para = new SqlParameter[]
           {
                new SqlParameter("@PS_V_Number", PS_V_Number),
           };
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para).Tables[0];
            return dt;

        }
    }
}
