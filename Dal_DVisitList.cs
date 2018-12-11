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
    public class Dal_DVisitList
    {
        public Dal_DVisitList()
        { }

        /// <summary>
        /// 设置报告打印状态
        /// </summary>
        /// <param name="VL_I_RePrintState">状态</param>
        /// <param name="VL_V_SerialNumber ">注册号</param>
        public bool fB_BSetMedicalState(string VL_I_RePrintState, string VL_V_SerialNumber)
        {
            StringBuilder sbrSQL = new StringBuilder();
            sbrSQL.Append("update TBL_D_VisitList set VL_I_RePrintState=@VL_I_RePrintState where VL_V_SerialNumber in (" + VL_V_SerialNumber + ")");
            SqlParameter[] para = new SqlParameter[] {
                new SqlParameter ("@VL_I_RePrintState",VL_I_RePrintState)
            };
            int i = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para);

            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }


        /// <summary>
        /// 设置报告打印状态---------------ago
        /// </summary>
        /// <param name="VL_I_RePrintState">状态</param>
        /// <param name="VL_V_SerialNumber ">注册号</param>
        public bool fB_BSetMedicalState_ago(string VL_I_RePrintState, string VL_V_SerialNumber)
        {
            StringBuilder sbrSQL = new StringBuilder();
            sbrSQL.Append("update UCARD_OFFLINE_INFOVIEW set OV_I_RePrintState=@VL_I_RePrintState where OV_V_SerialNumber in (" + VL_V_SerialNumber + ")");
            SqlParameter[] para = new SqlParameter[] {
                new SqlParameter ("@VL_I_RePrintState",VL_I_RePrintState)
            };
            int i = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para);

            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// 设置贴纸打印状态
        /// </summary>
        /// <param name="VL_I_SickPrintState">贴纸打印状态</param>
        /// <param name="VL_V_SerialNumber">注册号</param>
        /// <returns></returns>
        public bool fB_BSetSickState(int VL_I_SickPrintState, string VL_V_SerialNumber)
        {
            StringBuilder sbrSQL = new StringBuilder();
            sbrSQL.Append("update TBL_D_VisitList set VL_I_SickPrintState=@VL_I_SickPrintState where VL_V_SerialNumber in (" + VL_V_SerialNumber + ")");

            SqlParameter[] para = new SqlParameter[] {
                new SqlParameter ("@VL_I_SickPrintState",VL_I_SickPrintState)
            };

            int i = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para);

            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }


        /// <summary>
        /// 设置贴纸打印状态-----------ago
        /// </summary>
        /// <param name="VL_I_SickPrintState">贴纸打印状态</param>
        /// <param name="VL_V_SerialNumber">注册号</param>
        /// <returns></returns>
        public bool fB_BSetSickState_ago(int VL_I_SickPrintState, string VL_V_SerialNumber)
        {
            StringBuilder sbrSQL = new StringBuilder();
            sbrSQL.Append("update UCARD_OFFLINE_INFOVIEW set OV_I_SickPrintState=@VL_I_SickPrintState where OV_V_SerialNumber in (" + VL_V_SerialNumber + ")");

            SqlParameter[] para = new SqlParameter[] {
                new SqlParameter ("@VL_I_SickPrintState",VL_I_SickPrintState)
            };

            int i = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para);

            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// 设置发卡状态
        /// </summary>
        /// <param name="VL_I_State">发卡状态</param>
        /// <param name="VL_V_SerialNumber">注册号</param>
        /// <returns></returns>
        public bool fB_BSetState(int VL_I_State, string VL_V_SerialNumber)
        {
            StringBuilder sbrSQL = new StringBuilder();
            sbrSQL.Append("update TBL_D_VisitList set VL_I_State=@VL_I_State where VL_V_SerialNumber in (" + VL_V_SerialNumber + ")");

            // sbrSQL.Append(String.Join(",", VL_V_SerialNumber.ToArray()));

            SqlParameter[] para = new SqlParameter[] {
                new SqlParameter ("@VL_I_State",VL_I_State)

            };

            int i = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para);

            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 设置发卡状态--------------ago
        /// </summary>
        /// <param name="VL_I_State">发卡状态</param>
        /// <param name="VL_V_SerialNumber">注册号</param>
        /// <returns></returns>
        public bool fB_BSetState_ago(int VL_I_State, string VL_V_SerialNumber)
        {
            StringBuilder sbrSQL = new StringBuilder();
            sbrSQL.Append("update UCARD_OFFLINE_INFOVIEW set OV_I_State=@VL_I_State where OV_V_SerialNumber in (" + VL_V_SerialNumber + ")");

            // sbrSQL.Append(String.Join(",", VL_V_SerialNumber.ToArray()));

            SqlParameter[] para = new SqlParameter[] {
                new SqlParameter ("@VL_I_State",VL_I_State)

            };

            int i = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para);

            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// 添加已发卡片的记录
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool fB_BsetAdd(Mdl_BurnRecord BurnRecord)
        {
            #region
            StringBuilder sbrSQL = new StringBuilder();
                sbrSQL.Append("INSERT INTO TBL_D_BurnRecord");
                sbrSQL.Append(" (BR_V_UCardSerialNum,BR_V_CardCode,BR_V_Name,BR_V_SerialNumber,BR_V_ProcessNumber,BR_I_SendTerminalId,BR_V_SendTerminal,BR_D_SendTime)");
                sbrSQL.Append(" VALUES");
                sbrSQL.Append(" ('" + BurnRecord.BR_V_UCardSerialNum + "'" + "," + "'" + BurnRecord.BR_V_CardCode + "'" + "," + "'" + BurnRecord
                    .BR_V_Name + "'" + "," + "'" + BurnRecord.BR_V_SerialNumber + "'" + "," + "'" + BurnRecord.BR_V_ProcessNumber + "'" + "," + "'"
                    + BurnRecord.BR_I_SendTerminalId + "'" + "," + "'" + BurnRecord.BR_V_SendTerminal + "'" + "," + "'" + BurnRecord.BR_D_SendTime+ "'" + ")");
                sbrSQL.Append(" ; ");

            int i = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString());
            if (i > 0)
            {
                return true;
            }
            else
                return false;
            #endregion
        }

        public string tishi;
        /// <summary>
        /// 查询自助终端是否出现问题并返回问题结果集
        /// </summary>
        /// <returns></returns>
        public SqlDataReader fB_BfontError()
        {
            StringBuilder sbrSQL = new StringBuilder();
            sbrSQL.Append(" select TOP 1 * from TBL_D_DeviceErrorLog");
            sbrSQL.Append(" where DEL_I_State=0 ORDER BY DEL_D_ErrorTime DESC");
            SqlParameter[] para = new SqlParameter[] {

            };
            return SqlHelper.ExecuteReader(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para);
            //DataTable i = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para).Tables[0];

            //if (i.Rows.Count > 0)
            //{
            //    tishi = i.Rows[0][3].ToString();
            //    return tishi;
            //}
            //else
            //    return "false";
        }
        /// <summary>
        /// 修改自助终端问题修正状态
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool fB_ModifyAnError(int state)
        {
            StringBuilder sbrSQL = new StringBuilder();
            sbrSQL.Append("update TBL_D_DeviceErrorLog set DEL_I_State=1 WHERE DEL_ID=@DEL_ID");

            SqlParameter[] para = new SqlParameter[] {
                new SqlParameter ("@DEL_ID",state)
            };

            int i = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para);

            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// 查询胶片状态
        /// </summary>
        /// <param name="stateSerialNumber"></param>
        /// <returns></returns>
        public int fB_Bgetstate(List<string> stateSerialNumber)
        {
            List<int> datecount = new List<int>();
            StringBuilder sbrSQL = new StringBuilder();
            for (int il = 0; il < stateSerialNumber.Count; il++)
            {          
            sbrSQL.Append(" SELSECT VL_I_State FROM TBL_D_VisitList ");
            sbrSQL.Append(" WHERE VL_V_SerialNumber=@VL_V_SerialNumber ");
            //sbrSQL.Append(" AND FLAG_FILM_GOTTEN=1 ");

            SqlParameter[] para = new SqlParameter[] {
                new SqlParameter ("@VL_V_SerialNumber",stateSerialNumber[il])
            };
              DataTable tabl= SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para).Tables[0];
               int i = (int)tabl.Rows[0]["VL_I_State"];
                datecount.Add(i);
            }           
            return datecount[0];
        }
    }
}