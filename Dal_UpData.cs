using Base;
using Sunc_web_api.MDL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace Sunc_web_api.DAL
{
    public class Dal_UpData
    {
        public Dal_UpData()
        { }
        /// <summary>
        /// 根据提供的卡片内串号查询关联的病人信息
        /// </summary>
        /// <param name="SC_I_SerialNumber"></param>
        /// <returns></returns>
        public DataTable fm_CheckChuanhao(string SC_I_SerialNumber)
        {
            StringBuilder sbrSQL = new StringBuilder();//SQL字符串
            sbrSQL.Append("Select *  From TBL_B_SendCard");//SQL字符串赋值
            sbrSQL.Append(" where SC_I_SerialNumber=@SC_I_SerialNumber");

            SqlParameter[] para = new SqlParameter[]{
                new SqlParameter ("@SC_I_SerialNumber ",SC_I_SerialNumber )
             };
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para).Tables[0];
            return dt;
        }

    }
}