using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Sunc_web_api.BLL
{
    public class Bll_BFileNames
    {
        public string B_FileName(string VL_V_ProcessNumber)
        {
            DAL.Dal_DFileAttribute DDFN = new DAL.Dal_DFileAttribute();
            DataTable dt = DDFN.D_Filenames(VL_V_ProcessNumber);
            try
            {
                string path = dt.Rows[0]["VL_V_PathNO"].ToString();
                return path;
            }
            catch (Exception ex)
            {
                return null;              
            }
           
        }

    }
}