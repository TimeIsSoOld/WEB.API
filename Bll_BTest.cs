using Base;
using Sunc_web_api.DAL;
using Sunc_web_api.MDL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace Sunc_web_api.BLL
{
    public class Bll_BTest
    {
        /// <summary>
        /// 查询随机的就诊卡号
        /// </summary>
        /// <returns></returns>
        public string f_CardCode()
        {
            string CardCode = "";
            DataTable dt = f_CardCodeDate();

            int count = dt.Rows.Count;
            int index = new Random().Next(count);

            return CardCode = dt.Rows[index][0].ToString();
        }
        /// <summary>
        /// 查询随机的就诊卡号
        /// </summary>
        /// <returns></returns>
        private DataTable f_CardCodeDate()
        {
            StringBuilder sbrSQL = new StringBuilder();//SQL字符串
            sbrSQL.Append(" SELECT TOP 100  PI_V_CardCode FROM UCARD_ALLINFO_VIEW");
            sbrSQL.Append(" WHERE VL_I_State=0 AND VL_I_StudyMethod_ID=2 OR VL_I_StudyMethod_ID=3 ");
            sbrSQL.Append(" OR VL_I_StudyMethod_ID=8 OR VL_I_StudyMethod_ID=9  ");

            SqlParameter[] para = new SqlParameter[]{

            };
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para).Tables[0];
            return dt;
        }
    }
}