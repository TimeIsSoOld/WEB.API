using Base;
using Sunc_web_api.MDL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace Sunc_web_api.DAL
{
    public class Dal_BurnRecord
    {
        /// <summary>
        /// TBL_B_BurnRecord表的数据添加
        /// </summary>
        /// <param name="mdr"></param>
        /// <returns></returns>
        //public bool dal_insert(Mdl_BurnRecord mdr)
        //{
        //    StringBuilder sbrsql = new StringBuilder();
        //    sbrsql.Append("  INSERT INTO  TBL_B_BurnRecord ( ");
        //    sbrsql.Append("  BR_PI_ID ");
        //    sbrsql.Append("  ,BR_V_PI_V_CardCode ");
        //    sbrsql.Append("  ,BR_V_BC_V_NO ");
        //    sbrsql.Append("  ,BR_D_DateTime ");
        //    sbrsql.Append(" ) ");
        //    sbrsql.Append("  values ");
        //    sbrsql.Append(" ( ");
        //    sbrsql.Append(" @BR_PI_ID  ");
        //    sbrsql.Append(" ,@BR_V_PI_V_CardCode  ");
        //    sbrsql.Append(" ,@BR_V_BC_V_NO ");
        //    sbrsql.Append(" ,@BR_D_DateTime  ");
        //    sbrsql.Append(" ) ");
        //    SqlParameter[] para = new SqlParameter[]
        //    {
        //              new SqlParameter("BR_PI_ID",mdr.BR_PI_ID)
        //            ,new SqlParameter("BR_V_PI_V_CardCode",mdr.BR_V_PI_V_CardCode)
        //            ,new SqlParameter("BR_V_BC_V_NO",2)
        //            ,new SqlParameter("BR_D_DateTime",mdr.BR_D_DateTime)
        //     };

        //    int b = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnection(), CommandType.Text, sbrsql.ToString(), para);
        //    if (b > 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;

        //    }
        //}
        /// <summary>
        /// TBL_B_BurnRecord根据时间查询数量
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public string Getselectdate(DateTime time)
        {
            StringBuilder sbrsql = new StringBuilder();
            sbrsql.Append(" Select COUNT(*) as count  ");
            sbrsql.Append(" from TBL_D_BurnRecord  ");
            sbrsql.Append(" where CONVERT(varchar(100),BR_D_SendTime, 23)= @BR_D_SendTime ");
            //  sbrsql.Append("  and BR_V_BC_V_NO=2 ");

            SqlParameter[] para = new SqlParameter[]
                {
                new SqlParameter("@BR_D_SendTime",time)
                };
            DataTable b = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, sbrsql.ToString(), para).Tables[0];

            string date = b.Rows[0]["count"].ToString();
            return date;

        }


        /// <summary>
        /// TBL_B_BurnRecord根据时间查询自助发卡数量
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public string Getselectdate_1(DateTime time)
        {
            StringBuilder sbrsql = new StringBuilder();
            sbrsql.Append(" Select COUNT(*) as count  ");
            sbrsql.Append(" from TBL_D_BurnRecord  ");
            sbrsql.Append(" where CONVERT(varchar(100),BR_D_SendTime, 23)= @BR_D_DateTime ");
            sbrsql.Append("  and BR_I_SendTerminalId=1 ");

            SqlParameter[] para = new SqlParameter[]
                {
                new SqlParameter("@BR_D_DateTime",time)
                };
            DataTable b = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, sbrsql.ToString(), para).Tables[0];

            string date = b.Rows[0]["count"].ToString();
            return date;
        }

        /// <summary>
        /// TBL_B_BurnRecord根据时间查询自助发卡数量
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public string Getselectdate_2(DateTime time)
        {
            StringBuilder sbrsql = new StringBuilder();
            sbrsql.Append(" Select COUNT(*) as count  ");
            sbrsql.Append(" from TBL_D_BurnRecord  ");
            sbrsql.Append(" where CONVERT(varchar(100),BR_D_SendTime, 23)= @BR_D_DateTime ");
            sbrsql.Append("  and BR_I_SendTerminalId=2 ");

            SqlParameter[] para = new SqlParameter[]
                {
                new SqlParameter("@BR_D_DateTime",time)
                };
            DataTable b = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, sbrsql.ToString(), para).Tables[0];

            string date = b.Rows[0]["count"].ToString();
            return date;

        }

        public string Getselectdate_month(string time)
        {
            StringBuilder sbrsql = new StringBuilder();
            sbrsql.Append(" Select COUNT(*) as count  ");
            sbrsql.Append(" from TBL_D_BurnRecord  ");
            sbrsql.Append(" where convert(nvarchar(7),BR_D_SendTime,23)=@BR_D_DateTime ");
            //sbrsql.Append(" and BR_I_SendTerminalId=1 ");

            SqlParameter[] para = new SqlParameter[]
                {
                new SqlParameter("@BR_D_DateTime",time)
                };
            DataTable b = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, sbrsql.ToString(), para).Tables[0];

            string date = b.Rows[0]["count"].ToString();
            return date;
        }


        public string Getselectdate_year(string time)
        {
            StringBuilder sbrsql = new StringBuilder();
            sbrsql.Append(" Select COUNT(*) as count  ");
            sbrsql.Append(" from TBL_D_BurnRecord  ");
            sbrsql.Append(" where convert(nvarchar(4),BR_D_SendTime,23)=@BR_D_DateTime ");
            //sbrsql.Append(" and BR_I_SendTerminalId=1 ");

            SqlParameter[] para = new SqlParameter[]
                {
                new SqlParameter("@BR_D_DateTime",time)
                };
            DataTable b = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, sbrsql.ToString(), para).Tables[0];

            string date = b.Rows[0]["count"].ToString();
            return date;
        }
    }
    }
