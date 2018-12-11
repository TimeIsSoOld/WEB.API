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
    public class Dal_DHospitalInfo
    {
        /// <summary>
        /// 获取当前医院名字
        /// </summary>
        /// <returns></returns>
        public DataTable fD_GetHospitalName()
        {
            StringBuilder sbrSQL = new StringBuilder();
            sbrSQL.Append("Select  Distinct HI_V_Name From TBL_B_HospitalInfo");

            SqlParameter[] para = new SqlParameter[]{
             };
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para).Tables[0];
            return dt;
        }
    }
}