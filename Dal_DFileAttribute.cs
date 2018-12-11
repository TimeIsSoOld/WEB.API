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
    public class Dal_DFileAttribute
    {
        public DataTable D_Filenames(string VL_V_ProcessNumber)
        {
            StringBuilder sbrSQL = new StringBuilder();
            sbrSQL.Append(" SELECT VL_V_PathNO   ");
            sbrSQL.Append(" FROM TBL_D_VisitList  ");
            sbrSQL.Append(" WHERE VL_V_ProcessNumber=@VL_V_ProcessNumber  ");

            SqlParameter[] para = new SqlParameter[]
               {
                    new SqlParameter("@VL_V_ProcessNumber",VL_V_ProcessNumber)
               };
            DataTable dt  = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, sbrSQL.ToString(), para).Tables[0];
            return dt;

        }
    }
}