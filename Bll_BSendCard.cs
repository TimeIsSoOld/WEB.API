using Sunc_web_api.DAL;
using Sunc_web_api.MDL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;

namespace Sunc_web_api.BLL
{
    public class Bll_BSendCard
    {
        Dal_ASendCard asendcard = new Dal_ASendCard();
        /// <summary>
        /// 序列号的list
        /// </summary>
        /// <returns></returns>
        public List<Mdl_BurnRecord> fML_SelectSNumber()
        {
            List<Mdl_BurnRecord> msendcardlist = new List<Mdl_BurnRecord>();
           

            try
            {
                DataTable dt = asendcard.fM_SelectSNumber();
                foreach (DataRow dr in dt.Rows)
                {
                    Mdl_BurnRecord msendcard = new Mdl_BurnRecord();
                    msendcard.BR_V_UCardSerialNum = dr["BR_V_UCardSerialNum"].ToString();
                    msendcardlist.Add(msendcard);
                }
            }
            catch (Exception ex)
            { }

            return msendcardlist;
        }
        /// <summary>
        /// 存在串号返回FALSE
        /// </summary>
        /// <param name="BR_V_UCardSerialNum"></param>
        /// <returns></returns>
        public bool fML_SelectSNumber(string BR_V_UCardSerialNum)
        {
            DataTable dt = asendcard.fM_SelectSNumber(BR_V_UCardSerialNum);
            if (dt.Rows.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }        
        }
    }
}