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
    public class Dal_DPatientInfo
    {     
        /// <summary>
        /// 从数据库中提取指定的注册号下的打印报告的内容
        /// </summary>
        /// <param name="PI_V_CardCode"></param>
        /// <param name="SerialNumber"></param>
        /// <returns></returns>
        public DataTable fM_patientInfo(string PI_V_CardCode, string SerialNumber)
        {
            StringBuilder sbrSQL = new StringBuilder();

            sbrSQL.Append("SELECT *,s.PS_V_Picture as RPicture,ss.PS_V_Picture as VPicture FROM TBL_B_PatientInfo AS P");
            sbrSQL.Append(" LEFT JOIN TBL_D_VisitList AS V ON V.VL_PI_V_CardCode=P.PI_V_CardCode");
            sbrSQL.Append(" LEFT JOIN TBL_B_DiseaseDiagnosis AS D ON D.DD_VL_V_SerialNumber =V.VL_V_SerialNumber");
            sbrSQL.Append(" LEFT JOIN TBL_B_InspectionMethod AS I ON I.IM_ID=V.VL_I_StudyMethod_ID");
            sbrSQL.Append(" LEFT JOIN TBL_B_CourseCategory AS C ON C.CC_ID=V.VL_V_DiseaseCategory_ID");
            sbrSQL.Append(" LEFT JOIN TBL_B_DoctorSignature AS S ON S.PS_V_Number=V.VL_V_ReportID");
            sbrSQL.Append(" LEFT JOIN TBL_B_DoctorSignature AS SS ON SS.PS_V_Number=V.VL_V_VerifyID");
            sbrSQL.Append(" WHERE  PI_V_CardCode=@PI_V_CardCode and VL_V_SerialNumber in(" + SerialNumber + ") order by VL_D_RegistrationDate desc");

            SqlParameter[] para = new SqlParameter[] {
                new SqlParameter ("@PI_V_CardCode",PI_V_CardCode)
            };

            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para).Tables[0];
            return dt;
        }
    }
}