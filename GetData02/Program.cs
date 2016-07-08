using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;


namespace GetDataFromCityAndHid
{
    class Program
    {

        static string getHttp(string url, string queryString)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url + queryString);

            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            httpWebRequest.Timeout = 20000;

            string responseContent = "";
            HttpWebResponse httpWebResponse;
            StreamReader streamReader;
            try
            {
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                responseContent = streamReader.ReadToEnd();
                httpWebResponse.Close();
                streamReader.Close();
            }
            catch
            {
                responseContent = "";

            }
            finally
            {
            }

            return responseContent;
        }

        static void Main(string[] args)
        {
            string sDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            string qSql = "select lists_hid,lists_city from TB_ESTATE_LISTS_LEJUAPP";
            DataSet ds = SimpleDataHelper.Query(SimpleDataHelper.MSConnectionString, qSql);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                try

                {
                    string hid = ds.Tables[0].Rows[i][0].ToString();
                    string city = ds.Tables[0].Rows[i][1].ToString();

                    string house_statURL = @"http://mf.m.leju.com/v4/public/house_stat.json";
                    string house_statQueryString = @"?city_en="+ city +"&hid=" + hid + "&date=" + sDate;

                    string rtn = getHttp(house_statURL, house_statQueryString);

                    if (rtn.Length > 0)
                    {
                        JObject jo = JObject.Parse(rtn);

                        if (jo["entry"].Count() > 0)
                        {
                            string d_hid = (string)jo["entry"]["hid"];
                            string d_city_en = (string)jo["entry"]["city_en"];
                            string d_date = (string)jo["entry"]["date"];
                            string d_house_stat = (string)jo["entry"]["house_stat"];
                            string d_housetype_stat = (string)jo["entry"]["housetype_stat"];

                            string sql = "insert into TB_ESTATE_STAT_LEJUAPP ([house_stat_hid],[house_stat_city_en],[house_stat_date],[house_stat_house_stat],[house_stat_housetype_stat]) ";
                            sql += "values ('" + d_hid + "','" + d_city_en + "','" + d_date + "','" + d_house_stat + "','" + d_housetype_stat + "')";

                            Console.WriteLine(sql);

                            SimpleDataHelper.Excsql(SimpleDataHelper.MSConnectionString, sql);
                        }
                    }
                }
                catch (Exception ex)
                {
                    WriteLog.WriteLog.WLog(ex.Message);

                }

            }
        }
    }
}
