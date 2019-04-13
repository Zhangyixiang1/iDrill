using DrillingSymtemCSCV2.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DrillingSymtemCSCV2
{
    public static class Comm
    {
        /// <summary>
        /// 将Unix时间戳转换为DateTime类型时间
        /// </summary>
        /// <param name="d">double 型数字</param>
        /// <returns>DateTime</returns>
        public static System.DateTime ConvertIntDateTime(double d)
        {
            try
            {
                System.DateTime time = System.DateTime.MinValue;
                System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                time = startTime.AddMilliseconds(d);
                return DateTime.Parse(time.ToString("yyyy/MM/dd HH:mm:ss"));
            }
            catch { }
            return DateTime.Now;
        }

        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>long</returns>
        public static long ConvertDateTimeInt(System.DateTime time)
        {
            //double intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            //intResult = (time- startTime).TotalMilliseconds;
            long t = (time.Ticks - startTime.Ticks) / 10000;            //除10000调整为13位
            return t;
        }

        /// <summary>
        ///  公英制换算 ADD by ZAY in 2017.7.10
        /// </summary>
        /// <param name="listTag"></param>
        /// <param name="Tag"></param>
        /// <param name="DrillId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double UnitConversion(List<DrillTag> listTag, string Tag, string DrillId, double value)
        {
            try
            {
                if (AppDrill.UnitFormat == "yz")
                {
                    var Dirrl = 0;
                    if (!string.IsNullOrEmpty(DrillId))
                    {
                        Dirrl = int.Parse(DrillId);
                    }

                    var TagModel = listTag.Find(o => o.DrillId == Dirrl && o.Tag == Tag);
                    if (TagModel != null)
                    {
                        var UnitModel = AppData.UnitTransfer.Find(o => o.UnitFrom == TagModel.Unit);
                        if (UnitModel != null)
                        {
                            value = value * (double)UnitModel.Coefficient;
                            return Math.Round(value, 2);
                        }
                    }
                }
                else if (AppDrill.UnitFormat == "gz")
                {
                    return value;
                }
            }
            catch
            {
                return value;
            }
            return value;
        }

        /// <summary>
        /// Post请求数据
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="postDataStr"></param>
        /// <returns></returns>
        public static string HttpPost(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/json;charset=UTF-8";
            request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

    }
}
