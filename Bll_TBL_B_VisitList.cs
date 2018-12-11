using Sunc_web_api.MDL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Drawing;
using Sunc_web_api.DAL;
using System.Data.SqlClient;
using Base.SQLHelp;

namespace Sunc_web_api.BLL
{
    public class Bll_TBL_D_VisitList
    {
        DAL.Dal_TBL_D_VisitList DPM = new DAL.Dal_TBL_D_VisitList();
        Dal_DVisitList DDV = new Dal_DVisitList();
        public Bll_TBL_D_VisitList()
        { }
        /// <summary>
        /// 查询就诊号/流水号/注册号下的所有报告
        /// </summary>
        /// <returns></returns>
        /// 
        public List<Mdl_MPatientInfo> fM_Select(string BarCodeResult, string SerialNumber, string ProcessNumber)
        {
            List<Mdl_MPatientInfo> MPGM = new List<MDL.Mdl_MPatientInfo>();
            DataTable dt = DPM.fm_Select(BarCodeResult, SerialNumber, ProcessNumber);
            #region 
            foreach (DataRow dr in dt.Rows)
            {
                Mdl_MPatientInfo MDP = new Mdl_MPatientInfo();
                //MDL.MDl_TBL_D_VisitList MPG = new MDL.MDl_TBL_D_VisitList();

                MDP.IsValid = false;
                MDP.PI_V_PatientNo = Convert.ToDecimal(dr["PI_V_PatientNo"].ToString());
                MDP.PI_V_Name = dr["PI_V_Name"].ToString();
                MDP.PI_V_CardCode = dr["PI_V_CardCode"].ToString();
                try
                {
                    MDP.PI_I_Age = int.Parse(dr["PI_I_Age"].ToString());
                }
                catch (Exception)
                { }

                MDP.PI_V_Sex = dr["PI_V_Sex"].ToString();
                MDP.PI_V_Address = dr["PI_V_Address"].ToString();
                MDP.PI_V_IDNumber = dr["PI_V_IDNumber"].ToString();
                MDP.PI_V_MedicareNumber = dr["PI_V_MedicareNumber"].ToString();
                MDP.PI_V_Phone = dr["PI_V_Phone"].ToString();
                MDP.PI_V_AgeUnit = dr["PI_V_AgeUnit"].ToString();
                MDP.PI_D_InsterTime = Convert.ToDateTime(dr["PI_D_InsterTime"].ToString()); 
 
               MDP.VL_V_SerialNumber = dr["VL_V_SerialNumber"].ToString();
                MDP.VL_PI_V_PatientNo = Convert.ToDecimal(dr["VL_PI_V_PatientNo"].ToString());
                MDP.VL_PI_V_CardCode = dr["VL_PI_V_CardCode"].ToString();
                MDP.VL_V_DeptName = dr["VL_V_DeptName"].ToString();
                MDP.VL_V_StudyBodyPart = dr["VL_V_StudyBodyPart"].ToString();
                MDP.VL_V_StudyDeptName = dr["VL_V_StudyDeptName"].ToString();

                try
                {
                    MDP.VL_D_RegistrationDate = Convert.ToDateTime(dr["VL_D_RegistrationDate"]);
                }
                catch (Exception)
                { }

                MDP.VL_I_StudyMethod_ID =int.Parse( dr["VL_I_StudyMethod_ID"].ToString());
                MDP.VL_I_StudyMethod_Name = dr["SM_V_VALUE"].ToString() + dr["SM_V_Name"].ToString();
                MDP.VL_V_MODALITY = dr["VL_V_MODALITY"].ToString();

                MDP.VL_I_DiseaseCategory_ID = int.Parse(dr["VL_I_DiseaseCategory_ID"].ToString());
                MDP.VL_I_RegistratorType = int.Parse(dr["VL_I_RegistratorType"].ToString());

                MDP.VL_V_RoomNum = dr["VL_V_RoomNum"].ToString();
                MDP.VL_V_BedNum = dr["VL_V_BedNum"].ToString();
                MDP.VL_V_HospitalName = dr["VL_V_HospitalName"].ToString();

                MDP.VL_I_SickPrintState = int.Parse(dr["VL_I_SickPrintState"].ToString());
                MDP.VL_I_RePrintState = int.Parse(dr["VL_I_RePrintState"].ToString());
                MDP.VL_I_State = int.Parse(dr["VL_I_State"].ToString());
             
                MDP.VL_V_ProcessNumber = dr["VL_V_ProcessNumber"].ToString();
                MDP.VL_I_FlagStudy =Convert.ToBoolean( dr["VL_I_FlagStudy"].ToString());

               MDP.VL_I_ImageCount = int.Parse(dr["VL_I_ImageCount"].ToString());
                MDP.VL_V_PathNO = dr["VL_V_PathNO"].ToString();
                MDP.VL_D_StudyDate = Convert.ToDateTime(dr["VL_D_StudyDate"]);

                MDP.DD_VL_V_SerialNumber = dr["DD_VL_V_SerialNumber"].ToString();
                MDP.DD_PI_V_CardCode = dr["DD_PI_V_CardCode"].ToString();
                MDP.DD_SUBMIT_DOC_ID = dr["DD_SUBMIT_DOC_ID"].ToString();
                MDP.DD_SUBMIT_DOC_NAME = dr["DD_SUBMIT_DOC_NAME"].ToString();
                MDP.DD_CHECK_DOC_ID = dr["DD_CHECK_DOC_ID"].ToString();

                MDP.DD_CHECK_DOC_NAME = dr["DD_CHECK_DOC_NAME"].ToString();
                MDP.DD_CHECK_DATETIME =Convert.ToDateTime( dr["DD_CHECK_DATETIME"].ToString());
                MDP.DD_FLAG_CHECK =Convert.ToBoolean( dr["DD_FLAG_CHECK"].ToString());
                MDP.DD_FLAG_INVALID = Convert.ToBoolean(dr["DD_FLAG_INVALID"].ToString());
                MDP.DD_T_ILLSUMMARY = dr["DD_T_ILLSUMMARY"].ToString();
                MDP.DD_T_DIAGNOSIS = dr["DD_T_DIAGNOSIS"].ToString();
                MDP.DD_T_DICOMFindings = dr["DD_T_DICOMFindings"].ToString();
                MDP.DD_T_DICOMConclusion = dr["DD_T_DICOMConclusion"].ToString();

                MPGM.Add(MDP);
            }

            #endregion
            return MPGM;

        }
        /// <summary>
        /// 查询就诊号/流水号/注册号下的所有报告(两个月前)
        /// </summary>
        /// <param name="BarCodeResult"></param>
        /// <param name="SerialNumber"></param>
        /// <param name="ProcessNumber"></param>
        /// <returns></returns>
        public IList<Mdl_OFFLINE_PATIENT_INFOR> fM_SelectTwoMonthAgo(string BarCodeResult, string SerialNumber, string ProcessNumber)
        {
            SqlDataReader sdr = DPM.fm_SelectTwoMonth(BarCodeResult, SerialNumber, ProcessNumber);
            IList<Mdl_OFFLINE_PATIENT_INFOR> mbpiList = DataReaderProcess.DataReaderToList<Mdl_OFFLINE_PATIENT_INFOR>(sdr);
            return mbpiList;
        }
        /// <summary>
        /// 设备端病人所有报告
        /// </summary>
        /// <param name="BarCodeResult"></param>
        /// <returns></returns>
        public IList<Mdl_MPatientInfo> fm_SelectAotu(string BarCodeResult) 
        {
            IList<Mdl_MPatientInfo> mbpiList_result = new List<Mdl_MPatientInfo>();
            SqlDataReader sdr = DPM.fm_SelectSelectAotu(BarCodeResult);
            IList<Mdl_MPatientInfo> mbpiList = DataReaderProcess.DataReaderToList<Mdl_MPatientInfo>(sdr);
            //IList<Mdl_MPatientInfo> mbpiList = DataReaderProcess.DataReaderToList<Mdl_MPatientInfo>(sdr);
            var oj = mbpiList.GroupBy(t => t.VL_D_RegistrationDate.Value.Date);
            //List<DateTime> dl = new List<DateTime>();
            int count = 0;          //审核未完成的数量
            foreach (var item in oj)
            {
                //dl.Add(item.Key);
                var LIstbyDate = mbpiList.Where(d => d.VL_D_RegistrationDate.Value.Date == item.Key).ToList();

                //1.无病历数据
                if (LIstbyDate.Count == 0)
                {
                    Mdl_MPatientInfo info = new Mdl_MPatientInfo();
                    info.PI_ID = -1;
                    mbpiList_result.Add(info);
                    continue;
                }
                //2.审核未完成
                for (int i = 0; i < LIstbyDate.Count; i++)//循环查到的list总数
                {
                    if (!LIstbyDate[i].DD_FLAG_CHECK)//判断里面的审核记录
                    {
                        count++;
                    }
                }
                if (count >= 1)//判断未完成数量是否等于list数
                {
                    Mdl_MPatientInfo info = new Mdl_MPatientInfo();
                    info.PI_ID = -2;
                    mbpiList_result.Add(info);
                    continue;
                }
                //3.审核完成
                if (count == 0)//判断未完成数=0且完成数=list数
                {
                    foreach (var e in LIstbyDate)
                    {
                        mbpiList_result.Add(e);
                    }
                }
            }
            return mbpiList_result;
        }

        /// <summary>
        /// 查询到未发卡的病人姓名
        /// </summary>
        /// <returns></returns>
        public List<Mdl_MPatientName> fm_SelectName()
        {
            List<Mdl_MPatientName> PinfoName = new List<Mdl_MPatientName>();
            DataTable dt = DPM.fm_DSelectName();
            foreach (DataRow dr in dt.Rows)
            {
                Mdl_MPatientName mmpn = new MDL.Mdl_MPatientName();
                mmpn.PI_V_Name = dr["PI_V_Name"].ToString();
                PinfoName.Add(mmpn);
            }
            return PinfoName;
        }

        /// <summary>
        /// 查询就诊卡下指定流水号的报告信息
        /// </summary>
        /// <param name="BarCodeResult"></param>
        /// <param name="SerialNumber"></param>
        /// <returns></returns>
        public IList<Mdl_MPatientInfo> fM_SelectSerialNumber(string BarCodeResult, List<string> SerialNumber)
        {
            #region
            //BindingList<Mdl_MPatientInfo> MPGM = new BindingList<Mdl_MPatientInfo>();
            //DataTable dt = DPM.fm_SelectSerialNumber(BarCodeResult, SerialNumber);
            //foreach (DataRow dr in dt.Rows)
            //{
            //    Mdl_MPatientInfo MDP = new Mdl_MPatientInfo();
            //    //MDL.MDl_TBL_D_VisitList MPG = new MDL.MDl_TBL_D_VisitList();
            //    MDP.IsValid = false;
            //    MDP.PI_V_Name = dr["PI_V_Name"].ToString();
            //    MDP.PI_I_Age = int.Parse(dr["PI_I_Age"].ToString());
            //    MDP.PI_V_Sex = dr["PI_V_Sex"].ToString();
            //    MDP.PI_V_Address = dr["PI_V_Address"].ToString();
            //    MDP.PI_V_IDNumber = dr["PI_V_IDNumber"].ToString();
            //    MDP.PI_V_MedicareNumber = dr["PI_V_MedicareNumber"].ToString();
            //    MDP.PI_V_Phone = dr["PI_V_Phone"].ToString();
            //    MDP.PI_V_HpNO = dr["PI_V_HpNO"].ToString();
            //    MDP.VL_ID = int.Parse(dr["VL_ID"].ToString());
            //    MDP.VL_PI_V_CardCode = dr["VL_PI_V_CardCode"].ToString();
            //    MDP.Vl_V_ProcesSnumber = dr["VL_V_ProcessNumber"].ToString();
            //    MDP.VL_V_SerialNumber = dr["VL_V_SerialNumber"].ToString();
            //    MDP.VL_V_DeptName = dr["VL_V_DeptName"].ToString();
            //    MDP.VL_V_StudyBodyPart = dr["VL_V_StudyBodyPart"].ToString();
            //    MDP.VL_D_RegistrationDate = Convert.ToDateTime(dr["VL_D_RegistrationDate"]).ToString("yyyy年MM月dd日");
            //    MDP.IM_V_VALUE = dr["IM_V_VALUE"].ToString();
            //    MDP.CC_V_Name = dr["CC_V_Name"].ToString();
            //    MDP.VL_V_RoomNum = dr["VL_V_RoomNum"].ToString();
            //    MDP.VL_V_BedNum = dr["VL_V_BedNum"].ToString();
            //    MDP.VL_V_HospitalName = dr["VL_V_HospitalName"].ToString();
            //    MDP.VL_V_ReportID = dr["VL_V_ReportID"].ToString();
            //    MDP.VL_V_ReportName = dr["VL_V_ReportName"].ToString();
            //    MDP.VL_V_VerifyID = dr["VL_V_VerifyID"].ToString();
            //    MDP.VL_V_VerifyName = dr["VL_V_VerifyName"].ToString();
            //    MDP.VL_D_ReportDate = Convert.ToDateTime(dr["VL_D_ReportDate"].ToString());
            //    MDP.VL_I_State = int.Parse(dr["VL_I_State"].ToString());
            //    try
            //    {
            //        MDP.VL_I_SickState = int.Parse(dr["VL_I_SickState"].ToString());

            //    }
            //    catch (Exception)
            //    {
            //    }
            //    try
            //    {
            //        MDP.VL_I_MedicalState = int.Parse(dr["VL_I_MedicalState"].ToString());
            //    }
            //    catch (Exception)
            //    {
            //    }
            //    MDP.DD_T_Diagnosis = dr["DD_T_Diagnosis"].ToString();
            //    MDP.DD_T_ImagingConclusion = dr["DD_T_ImagingConclusion"].ToString();
            //    MDP.DD_T_ImagingFindings = dr["DD_T_ImagingFindings"].ToString();
            //    MDP.DD_T_Readme = dr["DD_T_Readme"].ToString();
            //    try
            //    {
            //        MDP.VL_V_PathNO = dr["VL_V_PathNO"].ToString();
            //    }
            //    catch (Exception)
            //    {
            //    }
            //    MDP.VL_V_VerifyReport = int.Parse(dr["VL_V_VerifyReport"].ToString());
            //    MPGM.Add(MDP);
            //}
            //// MPGM.GroupBy(x => x.检查日期).Select(x => new { StoreID = x.Key, List = x.ToList() });
            //return MPGM;
            #endregion
            SqlDataReader sdr = DPM.fm_SelectSerialNumber(BarCodeResult, SerialNumber);
            IList<Mdl_MPatientInfo> mbpiList = DataReaderProcess.DataReaderToList<Mdl_MPatientInfo>(sdr);
            return mbpiList;

        }

        /// <summary>
        /// 查询就诊卡下指定流水号的报告信息-------ago
        /// </summary>
        /// <param name="BarCodeResult"></param>
        /// <param name="SerialNumber"></param>
        /// <returns></returns>
        public IList<Mdl_OFFLINE_PATIENT_INFOR> fM_SelectSerialNumber_ago(string BarCodeResult, List<string> SerialNumber)
        {
            #region
            //BindingList<Mdl_MPatientInfo> MPGM = new BindingList<Mdl_MPatientInfo>();
            //DataTable dt = DPM.fm_SelectSerialNumber(BarCodeResult, SerialNumber);
            //foreach (DataRow dr in dt.Rows)
            //{
            //    Mdl_MPatientInfo MDP = new Mdl_MPatientInfo();
            //    //MDL.MDl_TBL_D_VisitList MPG = new MDL.MDl_TBL_D_VisitList();
            //    MDP.IsValid = false;
            //    MDP.PI_V_Name = dr["PI_V_Name"].ToString();
            //    MDP.PI_I_Age = int.Parse(dr["PI_I_Age"].ToString());
            //    MDP.PI_V_Sex = dr["PI_V_Sex"].ToString();
            //    MDP.PI_V_Address = dr["PI_V_Address"].ToString();
            //    MDP.PI_V_IDNumber = dr["PI_V_IDNumber"].ToString();
            //    MDP.PI_V_MedicareNumber = dr["PI_V_MedicareNumber"].ToString();
            //    MDP.PI_V_Phone = dr["PI_V_Phone"].ToString();
            //    MDP.PI_V_HpNO = dr["PI_V_HpNO"].ToString();
            //    MDP.VL_ID = int.Parse(dr["VL_ID"].ToString());
            //    MDP.VL_PI_V_CardCode = dr["VL_PI_V_CardCode"].ToString();
            //    MDP.Vl_V_ProcesSnumber = dr["VL_V_ProcessNumber"].ToString();
            //    MDP.VL_V_SerialNumber = dr["VL_V_SerialNumber"].ToString();
            //    MDP.VL_V_DeptName = dr["VL_V_DeptName"].ToString();
            //    MDP.VL_V_StudyBodyPart = dr["VL_V_StudyBodyPart"].ToString();
            //    MDP.VL_D_RegistrationDate = Convert.ToDateTime(dr["VL_D_RegistrationDate"]).ToString("yyyy年MM月dd日");
            //    MDP.IM_V_VALUE = dr["IM_V_VALUE"].ToString();
            //    MDP.CC_V_Name = dr["CC_V_Name"].ToString();
            //    MDP.VL_V_RoomNum = dr["VL_V_RoomNum"].ToString();
            //    MDP.VL_V_BedNum = dr["VL_V_BedNum"].ToString();
            //    MDP.VL_V_HospitalName = dr["VL_V_HospitalName"].ToString();
            //    MDP.VL_V_ReportID = dr["VL_V_ReportID"].ToString();
            //    MDP.VL_V_ReportName = dr["VL_V_ReportName"].ToString();
            //    MDP.VL_V_VerifyID = dr["VL_V_VerifyID"].ToString();
            //    MDP.VL_V_VerifyName = dr["VL_V_VerifyName"].ToString();
            //    MDP.VL_D_ReportDate = Convert.ToDateTime(dr["VL_D_ReportDate"].ToString());
            //    MDP.VL_I_State = int.Parse(dr["VL_I_State"].ToString());
            //    try
            //    {
            //        MDP.VL_I_SickState = int.Parse(dr["VL_I_SickState"].ToString());

            //    }
            //    catch (Exception)
            //    {
            //    }
            //    try
            //    {
            //        MDP.VL_I_MedicalState = int.Parse(dr["VL_I_MedicalState"].ToString());
            //    }
            //    catch (Exception)
            //    {
            //    }
            //    MDP.DD_T_Diagnosis = dr["DD_T_Diagnosis"].ToString();
            //    MDP.DD_T_ImagingConclusion = dr["DD_T_ImagingConclusion"].ToString();
            //    MDP.DD_T_ImagingFindings = dr["DD_T_ImagingFindings"].ToString();
            //    MDP.DD_T_Readme = dr["DD_T_Readme"].ToString();
            //    try
            //    {
            //        MDP.VL_V_PathNO = dr["VL_V_PathNO"].ToString();
            //    }
            //    catch (Exception)
            //    {
            //    }
            //    MDP.VL_V_VerifyReport = int.Parse(dr["VL_V_VerifyReport"].ToString());
            //    MPGM.Add(MDP);
            //}
            //// MPGM.GroupBy(x => x.检查日期).Select(x => new { StoreID = x.Key, List = x.ToList() });
            //return MPGM;
            #endregion
            SqlDataReader sdr = DPM.fm_SelectSerialNumber_ago(BarCodeResult, SerialNumber);
            IList<Mdl_OFFLINE_PATIENT_INFOR> mbpiList = DataReaderProcess.DataReaderToList<Mdl_OFFLINE_PATIENT_INFOR>(sdr);
            return mbpiList;

        }
        /// <summary>
        /// 查询就诊号下指定刻录状态的报告信息
        /// </summary>
        /// <param name="BarCodeResult"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public IList<Mdl_MPatientInfo> fM_SelectBurnRecord(string BarCodeResult, string state)
        {
            #region 
            //BindingList<Mdl_MPatientInfo> MPGM = new BindingList<Mdl_MPatientInfo>();
            //DataTable dt = DPM.fm_SelectBurnRecord(BarCodeResult, state);
            //foreach (DataRow dr in dt.Rows)
            //{
            //    Mdl_MPatientInfo MDP = new Mdl_MPatientInfo();
            //    //MDL.MDl_TBL_D_VisitList MPG = new MDL.MDl_TBL_D_VisitList();

            //    MDP.IsValid = false;
            //    MDP.PI_V_Name = dr["PI_V_Name"].ToString();
            //    MDP.PI_I_Age = int.Parse(dr["PI_I_Age"].ToString());
            //    MDP.PI_V_Sex = dr["PI_V_Sex"].ToString();
            //    MDP.PI_V_Address = dr["PI_V_Address"].ToString();
            //    MDP.PI_V_IDNumber = dr["PI_V_IDNumber"].ToString();
            //    MDP.PI_V_MedicareNumber = dr["PI_V_MedicareNumber"].ToString();
            //    MDP.PI_V_Phone = dr["PI_V_Phone"].ToString();
            //    MDP.PI_V_HpNO = dr["PI_V_HpNO"].ToString();

            //    MDP.VL_ID = int.Parse(dr["VL_ID"].ToString());
            //    MDP.VL_PI_V_CardCode = dr["VL_PI_V_CardCode"].ToString();
            //    MDP.Vl_V_ProcesSnumber = dr["VL_V_ProcessNumber"].ToString();
            //    MDP.VL_V_SerialNumber = dr["VL_V_SerialNumber"].ToString();
            //    MDP.VL_V_DeptName = dr["VL_V_DeptName"].ToString();
            //    MDP.VL_V_StudyBodyPart = dr["VL_V_StudyBodyPart"].ToString();
            //    MDP.VL_D_RegistrationDate = Convert.ToDateTime(dr["VL_D_RegistrationDate"]).ToString("yyyy年MM月dd日");
            //    MDP.IM_V_VALUE = dr["IM_V_VALUE"].ToString();
            //    MDP.CC_V_Name = dr["CC_V_Name"].ToString();
            //    MDP.VL_V_RoomNum = dr["VL_V_RoomNum"].ToString();
            //    MDP.VL_V_BedNum = dr["VL_V_BedNum"].ToString();
            //    MDP.VL_V_HospitalName = dr["VL_V_HospitalName"].ToString();
            //    MDP.VL_V_ReportID = dr["VL_V_ReportID"].ToString();
            //    MDP.VL_V_ReportName = dr["VL_V_ReportName"].ToString();
            //    MDP.VL_V_VerifyID = dr["VL_V_VerifyID"].ToString();
            //    MDP.VL_V_VerifyName = dr["VL_V_VerifyName"].ToString();
            //    MDP.VL_D_ReportDate = Convert.ToDateTime(dr["VL_D_ReportDate"].ToString());
            //    MDP.VL_I_State = int.Parse(dr["VL_I_State"].ToString());
            //    try
            //    {
            //        MDP.VL_I_SickState = int.Parse(dr["VL_I_SickState"].ToString());
            //    }
            //    catch (Exception)
            //    {
            //    }
            //    try
            //    {
            //        MDP.VL_I_MedicalState = int.Parse(dr["VL_I_MedicalState"].ToString());
            //    }
            //    catch (Exception)
            //    {
            //    }
            //    MDP.DD_T_Diagnosis = dr["DD_T_Diagnosis"].ToString();
            //    MDP.DD_T_ImagingConclusion = dr["DD_T_ImagingConclusion"].ToString();
            //    MDP.DD_T_ImagingFindings = dr["DD_T_ImagingFindings"].ToString();
            //    MDP.DD_T_Readme = dr["DD_T_Readme"].ToString();
            //    try
            //    {
            //        MDP.VL_V_PathNO = dr["VL_V_PathNO"].ToString();
            //    }
            //    catch (Exception)
            //    {
            //    }
            //    MDP.VL_V_VerifyReport = int.Parse(dr["VL_V_VerifyReport"].ToString());
            //    MPGM.Add(MDP);
            //}
            //// MPGM.GroupBy(x => x.检查日期).Select(x => new { StoreID = x.Key, List = x.ToList() });
            //return MPGM;
            #endregion
            SqlDataReader sdr = DPM.fm_SelectBurnRecord(BarCodeResult, state);
            IList<Mdl_MPatientInfo> mbpiList = DataReaderProcess.DataReaderToList<Mdl_MPatientInfo>(sdr);
            return mbpiList;
        }
        /// <summary>
        /// 获取打印的报告内容
        /// </summary>
        /// <param name="cardCode"></param>
        /// <param name="SerialNumber"></param>
        /// <returns></returns>
        public IList<Mdl_MPatientInfo> fML_PatientInfo(string cardCode, string SerialNumber)
        {
          
            SqlDataReader sdr = DPM.fM_patientInfo(cardCode, SerialNumber);
            IList<Mdl_MPatientInfo> mbpiList = DataReaderProcess.DataReaderToList<Mdl_MPatientInfo>(sdr);
            return mbpiList;
        }

        /// <summary>
        /// 获取打印的报告内容---------ago
        /// </summary>
        /// <param name="cardCode"></param>
        /// <param name="SerialNumber"></param>
        /// <returns></returns>
        public IList<Mdl_OFFLINE_PATIENT_INFOR> fML_PatientInfo_ago(string cardCode, string SerialNumber)
        {

            SqlDataReader sdr = DPM.fM_patientInfo_ago(cardCode, SerialNumber);
            IList<Mdl_OFFLINE_PATIENT_INFOR> mbpiList = DataReaderProcess.DataReaderToList<Mdl_OFFLINE_PATIENT_INFOR>(sdr);
            return mbpiList;
        }
        /// <summary>
        /// 获取打印的报告内容(移动)
        /// </summary>
        /// <param name="cardCode"></param>
        /// <param name="VL_V_ProcessNumber"></param>
        /// <returns></returns>
        public IList<Mdl_MPatientInfo> fML_PatientInfoMobile(string cardCode)
        {           
            SqlDataReader sdr = DPM.fM_patientInfoMobile(cardCode);
            IList<Mdl_MPatientInfo> mbpiList = DataReaderProcess.DataReaderToList<Mdl_MPatientInfo>(sdr);
            return mbpiList;
        }
        /// <summary>
        /// 重载fM_patientInfoMobile
        /// </summary>
        /// <param name="cardCode"></param>
        /// <param name="VL_V_ProcessNumber"></param>
        /// <returns></returns>
        public IList<Mdl_MPatientInfo> fML_PatientInfoMobile(string cardCode, string VL_V_ProcessNumber)
        {
            SqlDataReader sdr = DPM.fM_patientInfoMobile(cardCode, VL_V_ProcessNumber);
            IList<Mdl_MPatientInfo> mbpiList = DataReaderProcess.DataReaderToList<Mdl_MPatientInfo>(sdr);
            return mbpiList;
        }
        public string fML_Picture(string PS_V_Number)
        {
            DataTable dt = DPM.fM_patientPicture(PS_V_Number);
            byte[] byteArray = (byte[])dt.Rows[0]["PS_V_Picture"];
            string picture = Convert.ToBase64String(byteArray);
            return picture;
        }
        public IList<Mdl_DeviceErrorLog> GetRrror_bll()
        {
            SqlDataReader sdr = DDV.fB_BfontError();
            IList<Mdl_DeviceErrorLog> mbpiList = DataReaderProcess.DataReaderToList<Mdl_DeviceErrorLog>(sdr);
            return mbpiList;
        }

        /// <summary>
        /// 将获取的医生签名照转换成指定大小
        /// </summary>
        /// <param name="image"></param>
        /// <param name="iBitmapWidth"></param>
        /// <param name="iBitmapHeight"></param>
        /// <returns></returns>
        private Bitmap ConvertBitmapToScreen(System.Drawing.Image image, int iBitmapWidth, int iBitmapHeight)
        {
            //装载图片  
            // System.Drawing.Image image = System.Drawing.Image.FromFile(strBitmapPath);

            //获取图片的实际宽度与高度  
            int srcWidth = image.Width;
            int srcHeight = image.Height;

            if (iBitmapHeight == 0 && iBitmapWidth == 0)
            {
                return null;
            }

            //创建Bitmap对象，并设置Bitmap的宽度和高度。  
            Bitmap bmp = new Bitmap(iBitmapWidth, iBitmapHeight);

            //从Bitmap创建一个System.Drawing.Graphics对象，用来绘制高质量的缩小图。  
            System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(bmp);
            //设置 System.Drawing.Graphics对象的SmoothingMode属性为HighQuality  
            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //下面这个也设成高质量  
            gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            //下面这个设成High  
            gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //把原始图像绘制成上面所设置宽高的缩小图  
            System.Drawing.Rectangle rectDestination = new System.Drawing.Rectangle(0, 0, iBitmapWidth, iBitmapHeight);
            gr.DrawImage(image, rectDestination, 0, 0, srcWidth, srcHeight, GraphicsUnit.Pixel);

            image.Dispose();

            return bmp;
        }
    }
}

