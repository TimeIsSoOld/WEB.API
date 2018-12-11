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
    public class Dal_ASendCard
    {
        public Dal_ASendCard()
        { }

        /// <summary>
        /// 查询TBL_D_BurnRecord中所有的BR_V_UCardSerialNum（串口）
        /// </summary>
        /// <returns></returns>
        public DataTable fM_SelectSNumber()
        {
            StringBuilder sbrSQL = new StringBuilder();//SQL字符串
            sbrSQL.Append("Select BR_V_UCardSerialNum From TBL_D_BurnRecord");//SQL字符串赋值


            SqlParameter[] para = new SqlParameter[]{
             };
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para).Tables[0];
            return dt;
        }
        /// <summary>
        /// 查询TBL_D_BurnRecord中是否存在BR_V_UCardSerialNum（串口）
        /// </summary>
        /// <returns></returns>
        public DataTable fM_SelectSNumber(string BR_V_UCardSerialNum)
        {
            StringBuilder sbrSQL = new StringBuilder();//SQL字符串
            sbrSQL.Append("Select BR_V_UCardSerialNum  From TBL_D_BurnRecord  WHERE BR_V_UCardSerialNum=@BR_V_UCardSerialNum");//SQL字符串赋值

            SqlParameter[] para = new SqlParameter[]{
                new SqlParameter ("@BR_V_UCardSerialNum",BR_V_UCardSerialNum)
             };
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para).Tables[0];
            return dt;
        }
    }
}