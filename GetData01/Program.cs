using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace GetDataCityHidList
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
            string listsURL = @"http://mf.m.leju.com/v4/house/lists.json";

            int argsIdx = 0;
            while (argsIdx<args.Length)
            {
                int index = 1;

                string city = args[argsIdx];
                while (true)
                {
                    try
                    {

                        string listsQueryString = @"?city="+ city + "&appkey=2408231234&page=" + index.ToString() + "&pcount=50";

                        string rtn = getHttp(listsURL, listsQueryString);
                        if (rtn.Length > 0)
                        {
                            JObject jo = JObject.Parse(rtn);

                            if (jo["entry"].Count() == 0)
                            {
                                break;
                            }

                            for (int i = 0; i < jo["entry"].Count(); i++)
                            {
                                string d_house_id = (string)jo["entry"][i]["house_id"];
                                string d_hid = (string)jo["entry"][i]["hid"];
                                string d_salestate = (string)jo["entry"][i]["salestate"];
                                string d_price_avg = (string)jo["entry"][i]["price_avg"];
                                string d_name = (string)jo["entry"][i]["name"];
                                string d_address = (string)jo["entry"][i]["address"];
                                string d_main_housetype = (string)jo["entry"][i]["main_housetype"];
                                string d_pic_s = (string)jo["entry"][i]["pic_s"];
                                string d_price_display = (string)jo["entry"][i]["price_display"];
                                string d_district = (string)jo["entry"][i]["district"];
                                string d_price_time = (string)jo["entry"][i]["price_time"];
                                string d_price_trend = (string)jo["entry"][i]["price_trend"];
                                string d_price_rate = (string)jo["entry"][i]["price_rate"];

                                string sql = "delete from TB_ESTATE_LISTS_LEJUAPP where lists_hid = '" + d_hid + "' and lists_city ='"+ city +"'; ";
                                sql += "insert into TB_ESTATE_LISTS_LEJUAPP ([lists_city],[lists_house_id],[lists_hid],[lists_salestate],[lists_price_avg],[lists_name],[lists_address],[lists_main_housetype],[lists_pic_s],[lists_price_display],[lists_district],[lists_price_time],[lists_price_trend],[lists_price_rate]) ";
                                sql += "values ('"+ city +"','" + d_house_id + "','" + d_hid + "','" + d_salestate + "','" + d_price_avg + "','" + d_name + "','" + d_address + "','" + d_main_housetype + "','" + d_pic_s + "','" + d_price_display + "','" + d_district + "','" + d_price_time + "','" + d_price_trend + "','" + d_price_rate + "'); ";

                                Console.WriteLine(sql);

                                SimpleDataHelper.Excsql(SimpleDataHelper.MSConnectionString, sql);
                            }
                        }
                        else
                        {
                            break;
                        }

                        index++;
                    }
                    catch (Exception ex)
                    {
                        WriteLog.WriteLog.WLog(ex.Message);

                    }
                }

                argsIdx++;
            }

        }
    }
}
