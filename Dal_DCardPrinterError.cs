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
    public class Dal_DCardPrinterError
    {
        public Dal_DCardPrinterError()
        {
        }
        /// <summary>
        /// 查询证卡打印机的错误提示
        /// </summary>
        /// <param name="CPR_I_Number"></param>
        /// <returns></returns>
        public DataTable fD_SelectPrinterError(string DEL_V_ErrNum)
        {
            StringBuilder sbrSQL = new StringBuilder();
            sbrSQL.Append("SELECT DEL_V_ErrorInfo FROM TBL_D_DeviceErrorLog WHERE DEL_V_ErrNum=@DEL_V_ErrNum");

            SqlParameter[] para = new SqlParameter[]{
                new SqlParameter ("@DEL_V_ErrNum",DEL_V_ErrNum)
             };
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para).Tables[0];
            return dt;
        }

        /// <summary>
        /// 添加错误信息
        /// </summary>
        /// <param name="mdel"></param>
        /// <returns></returns>
        public bool fD_AddErrorInfo(MDL.Mdl_DeviceErrorLog mdel)
        {
            StringBuilder sbrSQL = new StringBuilder();
            sbrSQL.Append(" INSERT INTO TBL_D_DeviceErrorLog ");
            sbrSQL.Append(" ( DEL_DI_V_DeviceId,DEL_V_ErrNum,DEL_V_ErrorInfo,DEL_I_State,DEL_D_ErrorTime) ");
            sbrSQL.Append(" VALUES ");
            sbrSQL.Append(" ('" +mdel.DEL_DI_V_DeviceId + "'" + "," + "'" + mdel.DEL_V_ErrNum+ "'" + ","
                + "'"  + mdel.DEL_V_ErrorInfo+ "'" + "," + "'" + mdel.DEL_I_State+ "'" + "," + "'" + mdel.DEL_D_ErrorTime + "'" +   ")");
            sbrSQL.Append(" ; ");
            int i =SqlHelper.ExecuteNonQuery(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString());
            if (i > 0)
            {
                return true;
            }
            else
                return false;
        }
    }
}