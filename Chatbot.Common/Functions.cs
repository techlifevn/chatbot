using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Data;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Chatbot.Common
{
    public static class Functions
    {
        public static string GetFullDiaPhuong(string sonha, string diachi, string xa, string huyen, string tinh = "")
        {
            string str = "";
            if (!String.IsNullOrEmpty(sonha))
            {
                str += sonha + " - ";
            }
            if (!String.IsNullOrEmpty(diachi))
            {
                str += diachi + " - ";
            }
            if (!String.IsNullOrEmpty(xa))
            {
                str += xa + " - ";
            }
            if (!String.IsNullOrEmpty(huyen))
            {
                str += huyen;
            }
            if (!String.IsNullOrEmpty(tinh))
            {
                str += " - " + tinh;
            }
            return str;
        }

        public static string ConvertDateToAPI(string date)
        {
            try
            {
                string str = "";
                if (date.IndexOf("/") > 0)
                {
                    string[] str_split = date.Split('/');
                    str += str_split[2] + "-" + str_split[1] + "-" + str_split[0];
                }
                return str;
            }
            catch
            {
                return DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        public static DateTime ConvertDateToSql(string date)
        {
            try
            {
                string str = "";
                if (!String.IsNullOrEmpty(date) && date.IndexOf("/") > 0)
                {
                    string[] str_split = date.Split('/');
                    str += str_split[2] + "-" + str_split[1] + "-" + str_split[0];
                }
                DateTime date_orc = Convert.ToDateTime(str);
                return date_orc;
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        public static DateTime? ConvertStringToDateTime(string date)
        {
            try
            {
                string str = "";
                if (string.IsNullOrWhiteSpace(date)) return null;
                if (!String.IsNullOrEmpty(date) && date.IndexOf("/") > 0)
                {
                    string[] str_split = date.Split('/');
                    str += str_split[2] + "-" + str_split[1] + "-" + str_split[0];
                }
                DateTime date_orc = Convert.ToDateTime(str);
                return date_orc;
            }
            catch
            {
                return null;
            }
        }
        public static DateTime ConvertToSqlDateTime(string date, string time)
        {
            try
            {
                string str = "";
                if (!String.IsNullOrEmpty(date) && date.IndexOf("/") > 0)
                {
                    string[] str_split = date.Split('/');
                    str += str_split[2] + "-" + str_split[1] + "-" + str_split[0];
                    if (!String.IsNullOrWhiteSpace(time))
                    {
                        str += " " + time;
                    }
                }
                DateTime date_orc = Convert.ToDateTime(str);
                return date_orc;
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public static DateTime ConvertDateCanNullToSql(string date)
        {
            try
            {
                if (date != null)
                {
                    string str = "";
                    if (date.IndexOf("/") > 0)
                    {
                        string[] str_split = date.Split('/');
                        str += str_split[2] + "-" + str_split[1] + "-" + str_split[0];
                    }
                    DateTime date_orc = Convert.ToDateTime(str + " " + 00 + ":" + 00 + ":" + 00);
                    return date_orc;
                }
                else
                {
                    return DateTime.MinValue;
                }
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public static DateTime ConvertFullDateCanNullToSql(string date)
        {
            try
            {
                if (date != null)
                {
                    string str = "";
                    string str_Hour = "";
                    if (date.IndexOf("/") > 0)
                    {
                        string[] str_fullSplit = date.Split(' ');
                        string[] str_split = str_fullSplit[1].Split('/');
                        str += str_split[2] + "-" + str_split[1] + "-" + str_split[0];
                        string[] str_splitHour = str_fullSplit[0].Split(':');
                        str_Hour += str_splitHour[0] + ":" + str_splitHour[1] + ":" + str_splitHour[2];
                    }
                    DateTime date_orc = Convert.ToDateTime(str + " " + str_Hour);
                    return date_orc;
                }
                else
                {
                    return DateTime.MinValue;
                }
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public static string ConvertDateToStringSql(string date)
        {
            try
            {
                string str = "";
                if (date.IndexOf("/") > 0)
                {
                    string[] str_split = date.Split('/');
                    str += str_split[2] + "-" + str_split[1] + "-" + str_split[0];
                }
                return str;
            }
            catch
            {
                return DateTime.MinValue.ToString();
            }
        }

        public static string ConvertDateJSToStringSql(string date, string min = "") // 30/01/2021 -> 2021-01-30T00:00
        {
            try
            {
                string str = "";
                if (min != "max")
                {
                    if (date.IndexOf("/") > 0)
                    {
                        string[] str_split = date.Split('/');
                        str += str_split[2] + "-" + str_split[1] + "-" + str_split[0] + "T00:00";
                    }
                }
                else
                {
                    if (date.IndexOf("/") > 0)
                    {
                        string[] str_split = date.Split('/');
                        str += str_split[2] + "-" + str_split[1] + "-" + str_split[0] + "T23:59";
                    }
                }

                return str;
            }
            catch
            {
                return DateTime.MinValue.ToString();
            }
        }

        public static string GetDatetimeToVn(DateTime? date)
        {
            if (date != null)
            {
                if (Convert.ToDateTime(date).Year > 0001)
                    return Convert.ToDateTime(date).ToString("dd/MM/yyyy");
                else return String.Empty;
            }
            else return String.Empty;
        }

        public static string GetTimeToVn(DateTime? date)
        {
            if (date == null) return string.Empty;
            var d = Convert.ToDateTime(date);
            if (d.Year > 0001)
                return d.ToString("HH:mm:ss dd/MM/yyyy");
            else return String.Empty;
        }

        public static string GetTimeVn(DateTime? date)
        {
            if (date == null) return string.Empty;
            var d = Convert.ToDateTime(date);
            if (d.Year > 0001)
                return d.ToString("HH:mm");
            else return String.Empty;
        }

        //public static string GetFullTimeToVn(DateTime date)
        //{
        //    if (date.Year > 0001)
        //        return date.ToString("hh:mm:ss dd/MM/yyyy");
        //    else return String.Empty;
        //}

        public static List<T> ToListData<T>(this DataTable dataTable)
        {
            var dataList = new List<T>();
            dataList = JsonConvert.DeserializeObject<List<T>>(ToJsonData(dataTable));
            return dataList;
        }

        public static T ToData<T>(this DataTable dataTable) where T : new()
        {
            var dataList = new T();
            dataList = JsonConvert.DeserializeObject<T>(ToJsonData(dataTable));
            return dataList;
        }

        private static string ToJsonData(DataTable dataTable)
        {
            return JsonConvert.SerializeObject(dataTable);
        }

        public static string ConvertDecimalToVnd(decimal value)
        {
            var cul = CultureInfo.GetCultureInfo("vi-VN");

            return Convert.ToDouble(value).ToString("#,### vnđ", cul.NumberFormat);
        }

        public static string ConvertDecimalToMonney(decimal value)
        {
            if (value == 0) return "0";
            return String.Format("{0:0,0}", value);
        }

        public static string ConvertDecimalVND(decimal value)
        {
            if (value == 0) return "0";
            var cul = CultureInfo.GetCultureInfo("en-us");

            return Convert.ToDecimal(value).ToString("#,### ", cul.NumberFormat);
        }

        public static string ConvertToMoney(decimal value)
        {
            return String.Format("{0:0,0}", value);
        }

        public static string ConvertIntToMoney(int value)
        {
            return String.Format("{0:0,0}", value);
        }

        public static string convertToUnSign(string s)
        {
            string stFormD = s.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }
            sb = sb.Replace('Đ', 'D');
            sb = sb.Replace('đ', 'd');
            return (sb.ToString().Normalize(NormalizationForm.FormD));
        }

        public static string SubString(string fileName, string fileType)
        {
            if (fileType == null)
            {
                if (fileName.Length <= 9)
                    return fileName;
                var type = "." + fileName.Split('.')[fileName.Split('.').Length - 1];
                return fileName.Substring(0, 5) + ".." + type;
            }
            else
            {
                if (fileName.Length <= 9)
                    return fileName;
                return fileName.Substring(0, 5) + ".." + fileType;
            }
        }

        public static string SubStringTitle(string content, int value)
        {
            var arr = content != null ? content.Split(' ') : null;
            int length = arr != null ? arr.Length : 0;
            if (length <= value)
                return content;

            string str = string.Empty;

            for (int i = 0; i < value; i++)
            {
                str += $"{arr[i]} ";
            }
            return str.Trim() + "...";
        }

        public static string SubStringContent(string content, int value)
        {
            return content;
            //var arr = content != null ? content.Split(' ') : null;
            //int length = arr != null ? arr.Length : 0;
            //if (length <= value)
            //    return content;

            //string str = string.Empty;

            //for (int i = 0; i < value; i++)
            //{
            //    str += $"{arr[i]} ";
            //}
            //return str.Trim() + "...";
        }

        public static string FullStringContent(string content, int value)
        {
            if (content.Length <= value)
                return null;
            return content;
        }

        public static string HtmlToPlainText(this string html)
        {
            const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";//matches one or more (white space or line breaks) between '>' and '<'
            const string stripFormatting = @"<[^>]*(>|$)";//match any character between '<' and '>', even when end tag is missing
            const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
            var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
            var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
            var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);

            var text = html;
            //Decode html specific characters
            text = System.Net.WebUtility.HtmlDecode(text);
            //Remove tag whitespace/line breaks
            text = tagWhiteSpaceRegex.Replace(text, "><");
            //Replace <br /> with line breaks
            text = lineBreakRegex.Replace(text, Environment.NewLine);
            //Strip formatting
            text = stripFormattingRegex.Replace(text, string.Empty);

            return text;
        }

        public static string[] GetImgToHtml(string html, string host = "")
        {
            if (!String.IsNullOrEmpty(html))
            {
                var imgs = Regex.Matches(html, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase);
                var videos = Regex.Matches(html, "<video.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase);
                var files = Regex.Matches(html, "<a.+?href=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase);
                string[] imgPath = new string[imgs.Count + videos.Count + files.Count];
                int i = 0;
                foreach (Match match in imgs)
                {
                    imgPath[i] = match.Groups[1].Value;
                    i++;
                }
                foreach (Match match in videos)
                {
                    imgPath[i] = match.Groups[1].Value;
                    i++;
                }
                foreach (Match match in files)
                {
                    imgPath[i] = match.Groups[1].Value;
                    i++;
                }
                return imgPath;
            }
            return null;
        }
        public static string[] GetVideoToHtml(string html, string host = "")
        {
            if (!String.IsNullOrEmpty(html))
            {
                var videos = Regex.Matches(html, "<video.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase);
                string[] imgPath = new string[videos.Count];
                int i = 0;
                foreach (Match match in videos)
                {
                    imgPath[i] = match.Groups[1].Value;
                    i++;

                }
                return imgPath;
            }
            return null;
        }
        public static string RenameFile(string oldFileName, string newFileName)
        {
            File.Move(oldFileName, newFileName);

            return newFileName;
        }

        public static double GetPageNum(int sotu)
        {
            int num = Convert.ToInt32(sotu / 250);
            double number = Convert.ToDouble(sotu) / 250;
            double a = Convert.ToDouble(num) / 2;
            if ((0 < (number - num)) && ((number - num) < 250))
            {
                return a + 0.5;
            }
            else
            {
                return a + 1;
            }
        }

        public static string GetWeatherByLanguage(string language, string weatherEn)
        {
            string strWeather = weatherEn;
            if (language == "vi")
            {
                switch (strWeather)
                {
                    case "Rain":
                        strWeather = "Mưa";
                        break;

                    case "Clouds":
                        strWeather = "Nhiều mây";
                        break;

                    default:
                        strWeather = weatherEn;
                        break;
                }
            }
            else if (language == "fr")
                switch (strWeather)
                {
                    case "Rain":
                        strWeather = "Pluie";
                        break;

                    case "Clouds":
                        strWeather = "Nuages";
                        break;

                    default:
                        strWeather = weatherEn;
                        break;
                }

            return strWeather;
        }

        public static int GetParentByLanguage(string language, int parentViId)
        {
            int parent = parentViId;
            if (language == "en")
            {
                switch (parent)
                {
                    case 40: // khám phá huế
                        parent = 47;
                        break;

                    case 44: // điều càn làm
                        //parent = 1154;
                        parent = 1155;
                        break;

                    case 53: // độc đáo huế
                        parent = 49;
                        break;

                    case 62: //tin tức
                        parent = 76;
                        break;

                    case 146: // hỗ trợ
                        parent = 80;
                        break;

                    case 1147: // lưu trú (con điều cần làm)
                        parent = 2176;
                        break;

                    default:
                        parent = parentViId;
                        break;
                }
            }
            if (language == "fr")
                switch (parent)
                {
                    case 40: // khám phá huế
                        parent = 2180;
                        break;

                    case 44: // điều càn làm
                        //parent = 1154;
                        parent = 2186;
                        break;

                    case 53: // độc đáo huế
                        parent = 2192;
                        break;

                    case 62: //tin tức
                        parent = 2197;
                        break;

                    case 146: // hỗ trợ
                        parent = 80;
                        break;

                    case 1147: // lưu trú (con điều cần làm)
                        parent = 2188;
                        break;

                    default:
                        parent = parentViId;
                        break;
                }
            return parent;
        }

        public static string GetCodeLanguage(string language, int parentViId)
        {
            if (language == "en")
            {
                switch (parentViId)
                {
                    case 47: // khám phá huế
                        return "visit-hue";

                    case 1155: // điều càn làm
                               //parent = 1154;
                        return "things-to-do";

                    case 49: // độc đáo huế
                        return "amazing-hue";

                    case 76: //tin tức
                        return "whats-on";

                    default:
                        return "traveller-essentials";
                }
            }
            else if (language == "fr")
            {
                switch (parentViId)
                {
                    case 2180: // khám phá huế
                        return "visit-hue";

                    case 2186: // điều càn làm
                        //parent = 1154;
                        return "things-to-do";

                    case 2192: // độc đáo huế
                        return "amazing-hue";

                    case 2197: //tin tức
                        return "whats-on";

                    default:
                        return "traveller-essentials";
                }
            }
            else
            {
                switch (parentViId)
                {
                    case 40: // khám phá huế
                        return "visit-hue";

                    case 44: // điều càn làm
                             //parent = 1154;
                        return "things-to-do";

                    case 53: // độc đáo huế
                        return "amazing-hue";

                    case 62: //tin tức
                        return "whats-on";

                    default:
                        return "traveller-essentials";
                }
            }
        }

        public static string ConvertTimeVn(int gio, int phut)
        {
            string result = "";
            if (gio < 10) result += "0" + gio.ToString() + " giờ";
            else result += gio.ToString() + " giờ"; ;
            if (phut < 10) result += ":0" + phut.ToString() + " phút";
            else result += ":" + phut.ToString() + " phút";

            return result;
        }

        private static string Chu(string gNumber)
        {
            string result = "";
            switch (gNumber)
            {
                case "0":
                    result = "không";
                    break;

                case "1":
                    result = "một";
                    break;

                case "2":
                    result = "hai";
                    break;

                case "3":
                    result = "ba";
                    break;

                case "4":
                    result = "bốn";
                    break;

                case "5":
                    result = "năm";
                    break;

                case "6":
                    result = "sáu";
                    break;

                case "7":
                    result = "bảy";
                    break;

                case "8":
                    result = "tám";
                    break;

                case "9":
                    result = "chín";
                    break;
            }
            return result;
        }

        private static string Donvi(string so)
        {
            string Kdonvi = "";
            if (so.Equals("1"))
                Kdonvi = "";
            if (so.Equals("2"))
                Kdonvi = "nghìn";
            if (so.Equals("3"))
                Kdonvi = "triệu";
            if (so.Equals("4"))
                Kdonvi = "tỷ";
            if (so.Equals("5"))
                Kdonvi = "nghìn tỷ";
            if (so.Equals("6"))
                Kdonvi = "triệu tỷ";
            if (so.Equals("7"))
                Kdonvi = "tỷ tỷ";
            return Kdonvi;
        }

        public static string GetLaMa(int num)
        {
            string Kdonvi = "";
            if (num == 1)
                Kdonvi = "I";
            if (num == 2)
                Kdonvi = "II";
            return Kdonvi;
        }

        private static string Tach(string tach3)
        {
            string Ktach = "";
            if (tach3.Equals("000"))
                return "";
            if (tach3.Length == 3)
            {
                string tr = tach3.Trim().Substring(0, 1).ToString().Trim();
                string ch = tach3.Trim().Substring(1, 1).ToString().Trim();
                string dv = tach3.Trim().Substring(2, 1).ToString().Trim();
                if (tr.Equals("0") && ch.Equals("0"))
                    Ktach = " không trăm lẻ " + Chu(dv.ToString().Trim()) + " ";
                if (!tr.Equals("0") && ch.Equals("0") && dv.Equals("0"))
                    Ktach = Chu(tr.ToString().Trim()).Trim() + " trăm ";
                if (!tr.Equals("0") && ch.Equals("0") && !dv.Equals("0"))
                    Ktach = Chu(tr.ToString().Trim()).Trim() + " trăm lẻ " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && dv.Equals("0"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && dv.Equals("5"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi lăm ";
                if (tr.Equals("0") && ch.Equals("1") && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = " không trăm mười " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && ch.Equals("1") && dv.Equals("0"))
                    Ktach = " không trăm mười ";
                if (tr.Equals("0") && ch.Equals("1") && dv.Equals("5"))
                    Ktach = " không trăm mười lăm ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi " + Chu(dv.Trim()).Trim() + " ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && dv.Equals("0"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi lăm ";
                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười " + Chu(dv.Trim()).Trim() + " ";

                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && dv.Equals("0"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười ";
                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười lăm ";
            }
            return Ktach;
        }

        public static string Read_Money_Into_Words(decimal gNum)
        {
            if (gNum == 0)
                return "Không đồng";

            string lso_chu = "";
            string tach_mod = "";
            string tach_conlai = "";
            decimal Num = Math.Round(gNum, 0);
            string gN = Convert.ToString(Num);
            int m = Convert.ToInt32(gN.Length / 3);
            int mod = gN.Length - m * 3;
            string dau = "[+]";

            // Dau [+ , - ]
            if (gNum < 0)
                dau = "[-]";
            dau = "";

            // Tach hang lon nhat
            if (mod.Equals(1))
                tach_mod = "00" + Convert.ToString(Num.ToString().Trim().Substring(0, 1)).Trim();
            if (mod.Equals(2))
                tach_mod = "0" + Convert.ToString(Num.ToString().Trim().Substring(0, 2)).Trim();
            if (mod.Equals(0))
                tach_mod = "000";
            // Tach hang con lai sau mod :
            if (Num.ToString().Length > 2)
                tach_conlai = Convert.ToString(Num.ToString().Trim().Substring(mod, Num.ToString().Length - mod)).Trim();

            ///don vi hang mod
            int im = m + 1;
            if (mod > 0)
                lso_chu = Tach(tach_mod).ToString().Trim() + " " + Donvi(im.ToString().Trim());
            /// Tach 3 trong tach_conlai

            int i = m;
            int _m = m;
            int j = 1;
            string tach3 = "";
            string tach3_ = "";

            while (i > 0)
            {
                tach3 = tach_conlai.Trim().Substring(0, 3).Trim();
                tach3_ = tach3;
                lso_chu = lso_chu.Trim() + " " + Tach(tach3.Trim()).Trim();
                m = _m + 1 - j;
                if (!tach3_.Equals("000"))
                    lso_chu = lso_chu.Trim() + " " + Donvi(m.ToString().Trim()).Trim();
                tach_conlai = tach_conlai.Trim().Substring(3, tach_conlai.Trim().Length - 3);

                i = i - 1;
                j = j + 1;
            }
            if (lso_chu.Trim().Substring(0, 1).Equals("k"))
                lso_chu = lso_chu.Trim().Substring(10, lso_chu.Trim().Length - 10).Trim();
            if (lso_chu.Trim().Substring(0, 1).Equals("l"))
                lso_chu = lso_chu.Trim().Substring(2, lso_chu.Trim().Length - 2).Trim();
            if (lso_chu.Trim().Length > 0)
                lso_chu = dau.Trim() + " " + lso_chu.Trim().Substring(0, 1).Trim().ToUpper() + lso_chu.Trim().Substring(1, lso_chu.Trim().Length - 1).Trim() + " đồng chẵn.";

            return lso_chu.ToString().Trim();
        }

        public static string EncodeId(int id, string key = SystemConstants.AppSettings.Key)
        {
            return HashUtil.EncodeID(id.ToString(), key);
        }
        public static string EncodeId(int? id, string key = SystemConstants.AppSettings.Key)
        {
            if (id == null || id == 0) return String.Empty;
            return HashUtil.EncodeID(id.ToString(), key);
        }
        public static int DecodeId(string id, string key = SystemConstants.AppSettings.Key)
        {
            return Convert.ToInt32(HashUtil.DecodeID(id, key));
        }

        public static string RemoveSpace(string value)
        {
            if (String.IsNullOrWhiteSpace(value))
                return String.Empty;
            var list = value.Split(' ').Where(s => !String.IsNullOrWhiteSpace(s));
            return string.Join("", list);
        }

        public static string RemoveMultiplesSpace(this string value)
        {
            if (String.IsNullOrWhiteSpace(value))
                return String.Empty;
            var list = value.Split(' ').Where(s => !String.IsNullOrWhiteSpace(s));
            return string.Join(" ", list);
        }

        public static bool IsImage(this IFormFile file)
        {
            return file.Length > 0 && file.ContentType.Contains("image");
        }

        public static bool IsVideo(this IFormFile file)
        {
            return file.Length > 0 && file.ContentType.Contains("video");
        }

        public static bool IsNumeric(this string value)
        {
            return value.All(char.IsNumber);
        }

        public static string GetLanguageId()
        {
            return SystemConstants.AppSettings.DefaultLanguageId;
        }

        public static string ConvertToUnSign(this string str)
        {
            if (String.IsNullOrWhiteSpace(str))
                return String.Empty;
            string stFormD = str.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }
            sb = sb.Replace('Đ', 'D');
            sb = sb.Replace('đ', 'd');
            return (sb.ToString().Normalize(NormalizationForm.FormD)).ReplaceSpecialCharacters('-').Trim('-').ToLower();
        }

        public static string ReplaceSpecialCharacters(this string str, char replace)
        {
            if (String.IsNullOrWhiteSpace(str))
                return String.Empty;
            char[] buffer = new char[str.Length];
            bool currentIsSpecialChar = false;
            int idx = 0;

            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                {
                    buffer[idx] = c;
                    ++idx;
                    currentIsSpecialChar = false;
                }
                else
                {
                    if (!currentIsSpecialChar)
                    {
                        buffer[idx] = replace;
                        ++idx;
                    }
                    currentIsSpecialChar = true;
                }
            }

            return new String(buffer, 0, idx).Trim(replace);
        }

        public static string ChuyenHinhAnhQuaThuMucMoi(string html, string path, string host, ref List<string> allimgs)
        {
            html = HttpUtility.HtmlDecode(html);

            var imgs = GetImgToHtml(html, host);
            if (imgs != null)
            {
                var dataImgs = imgs.Where(x => !x.Contains("data:image")).ToList();

                foreach (var img in dataImgs)
                {
                    try
                    {
                        var uri = new Uri(img);
                        if (host == $"{uri.Scheme}://{uri.Host}" || host == $"{uri.Scheme}://{uri.Host}/")
                        {
                            html = html.Replace(img, path + uri.AbsolutePath);
                            allimgs.Add(uri.AbsolutePath);
                        }
                    }
                    catch
                    {
                        html = html.Replace(img, path + img);
                        allimgs.Add(img);
                    }

                }
            }

            return html;
        }
        public static string ChuyenVideoThuMucMoi(string html, string path, string host, ref List<string> allimgs)
        {
            var imgs = GetVideoToHtml(html, host);
            if (imgs != null)
            {
                foreach (var img in imgs)
                {
                    try
                    {
                        var uri = new Uri(img);
                        if (host == $"{uri.Scheme}://{uri.Host}" || host == $"{uri.Scheme}://{uri.Host}/")
                        {
                            html = html.Replace(img, path + uri.AbsolutePath);
                            allimgs.Add(uri.AbsolutePath);
                        }
                    }
                    catch
                    {
                        html = html.Replace(img, path + img);
                        allimgs.Add(img);
                    }
                }
            }
            return html;
        }

        public static string ChuyenHinhAnhQuaThuMucMoi(string html, string path, string host, string[] imgnews)
        {
            var imgs = GetImgToHtml(html, host);
            imgnews = imgs;

            if (imgs != null)
            {
                foreach (var img in imgs)
                {
                    html = html.Replace(img, path + img);
                }
            }

            return html;
        }

        public static string StripTags(string html)
        {
            return string.IsNullOrWhiteSpace(html) ? string.Empty : HttpUtility.HtmlDecode(Regex.Replace(html, "<.*?>", string.Empty));
        }

        public static string ToRoman(int number)
        {
            if ((number < 0) || (number > 3999)) return string.Empty;
            if (number < 1) return string.Empty;
            if (number >= 1000) return "M" + ToRoman(number - 1000);
            if (number >= 900) return "CM" + ToRoman(number - 900);
            if (number >= 500) return "D" + ToRoman(number - 500);
            if (number >= 400) return "CD" + ToRoman(number - 400);
            if (number >= 100) return "C" + ToRoman(number - 100);
            if (number >= 90) return "XC" + ToRoman(number - 90);
            if (number >= 50) return "L" + ToRoman(number - 50);
            if (number >= 40) return "XL" + ToRoman(number - 40);
            if (number >= 10) return "X" + ToRoman(number - 10);
            if (number >= 9) return "IX" + ToRoman(number - 9);
            if (number >= 5) return "V" + ToRoman(number - 5);
            if (number >= 4) return "IV" + ToRoman(number - 4);
            if (number >= 1) return "I" + ToRoman(number - 1);
            return string.Empty;
        }

        public static string GetPathLog(string path, string logLevel)
        {
            var folderPath = Path.Combine(path, "wwwroot", "Logs", logLevel, DateTime.Now.ToString("yyyy"), DateTime.Now.ToString("MM"));
            var folder = new DirectoryInfo(folderPath);
            if (!folder.Exists)
            {
                Directory.CreateDirectory(folderPath);
            }
            string filePath = Path.Combine(folderPath, DateTime.Now.ToString("dd") + ".txt");
            if (!File.Exists(filePath))
            {
                using (var FS = File.Create(filePath))
                {
                    FS.Close();
                }
            }
            return filePath;
        }

        public static string GetFileName(string hrefLink)
        {
            Uri uri = new Uri(hrefLink);

            return System.IO.Path.GetFileName(uri.LocalPath);
        }

        public static string ReplaceOldContent(string content)
        {
            content = content.Replace(@"/Root/OldData/00.13.H57/portal/UploadFiles", @"/Root/OldData/00.13.H57/UploadFiles");

            return content;
        }

        public static string ImageToBase64(string imagePath, string webRootPath)
        {
            try
            {
                string _base64String = null;

                string _imagePath = webRootPath + imagePath.Replace("/", "\\");

                byte[] imageArray = System.IO.File.ReadAllBytes(_imagePath);

                _base64String = Convert.ToBase64String(imageArray);

                return "data:image/jpg;base64," + _base64String;
            }
            catch
            {
                return imagePath;
            }
        }
        public static string GetUrl(string pageid, string url)
        {
            if (String.IsNullOrWhiteSpace(pageid))
                return url;
            return url.Contains("http") || url.Contains("https") ? url : $"/p/{pageid}{url}";
        }

        public static string RemoveStyleInHtml(this string html, bool isMobile = false, bool isSetWidthImage = true)
        {
            if (!String.IsNullOrEmpty(html))
            {
                //html = Regex.Replace(html, "style=[\"'](.+?)[\"'].*?", "", RegexOptions.IgnoreCase);
                html = RemoveStyle(html, "line-height");
                html = RemoveStyle(html, "font-family");
                html = RemoveStyle(html, "font-size");
                html = html.Replace("<sub>", "");
                html = html.Replace("<sup>", "");
                html = html.Replace("</sub>", "");
                html = html.Replace("</sup>", "");
                if (isSetWidthImage)
                {
                    html = html.ReplaceWidthHeightAndCenterImage(isMobile);
                }
                html = html.AddResponsiveDivToTables(isMobile);
                html = html.RemoveAttributesFromTables(isMobile);
                html = html.ReplaceHTagsWithStrong(isMobile);
                html = html.AddLazyLoadingToImages();
                html = html.AddIFrameContainer(isMobile);

                return html;
            }
            return null;
        }
        static string ReplaceWidthHeightAndCenterImage(this string html, bool isMobile = false)
        {
            if (string.IsNullOrEmpty(html)) return string.Empty;
            // Biểu thức chính quy để tìm và loại bỏ thuộc tính width với đơn vị px trong style
            string pattern = "(<img[^>]*\\sstyle=\"[^\"]*)width:\\s*[0-9]+px;?(\\s*height:\\s*[0-9]+px;?)?([^\"]*\")";
            // Khởi tạo regex
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            // Thay thế các thuộc tính width theo đơn vị px bằng chuỗi rỗng
            if (isMobile)
                return regex.Replace(html, "$1width:100%;height:100%;display:block;margin:0 auto;$3");
            return regex.Replace(html, "$1width:80%;height:100%;display:block;margin:0 auto;$3");
        }
        static string AddResponsiveDivToTables(this string htmlContent, bool isMobile = false)
        {
            if (string.IsNullOrEmpty(htmlContent)) return string.Empty;
            // Biểu thức chính quy để tìm thẻ <table>
            string tablePattern = @"<table\b[^>]*>";
            string divOpenTag = "<div class=\"table-responsive\">$&";
            // Thay thế thẻ <table> bằng <div class="table-responsive"><table>
            string modifiedHtml = Regex.Replace(htmlContent, tablePattern, divOpenTag, RegexOptions.IgnoreCase);
            // Thêm thẻ </div> sau thẻ </table>
            string tableClosePattern = @"</table>";
            string divCloseTag = "</table></div>";
            modifiedHtml = Regex.Replace(modifiedHtml, tableClosePattern, divCloseTag, RegexOptions.IgnoreCase);

            return modifiedHtml;
        }
        static string RemoveAttributesFromTables(this string htmlContent, bool isMobile = false)
        {
            if (string.IsNullOrEmpty(htmlContent)) return string.Empty;
            // Biểu thức chính quy để loại bỏ các thuộc tính style, width, height từ các thẻ <table>
            string removeAttributesPattern = @"<table\b([^>]*)(\bstyle=[""'][^""']*[""']|\bwidth=[""'][^""']*[""']|\bheight=[""'][^""']*[""'])([^>]*)>";
            string removeAttributesReplacement = "<table$1$3>";
            // Biểu thức chính quy để loại bỏ các thuộc tính cụ thể bên trong thuộc tính style
            string removeSpecificStylesPattern = @"<table\b([^>]*)\bstyle=[""'][^""']*\b(width|height):[^;]*;?[^""']*[""']([^>]*)>";
            string removeSpecificStylesReplacement = "<table$1$3>";
            // Sử dụng regex để loại bỏ các thuộc tính style, width, height
            string modifiedHtml = Regex.Replace(htmlContent, removeAttributesPattern, removeAttributesReplacement, RegexOptions.IgnoreCase);
            modifiedHtml = Regex.Replace(modifiedHtml, removeSpecificStylesPattern, removeSpecificStylesReplacement, RegexOptions.IgnoreCase);
            return modifiedHtml;
        }
        static string ReplaceHTagsWithStrong(this string htmlContent, bool isMobile = false)
        {
            if (string.IsNullOrEmpty(htmlContent)) return string.Empty;
            // Biểu thức chính quy để tìm và thay thế các thẻ h từ h1 đến h6 mở và đóng
            string patternOpen = @"<h[1-6]\b[^>]*>";
            string patternClose = @"</h[1-6]>";
            // Thay thế thẻ h bằng thẻ strong
            string replacementOpen = "<strong>";
            string replacementClose = "</strong>"; // Thực hiện thay thế
            string modifiedHtml = Regex.Replace(htmlContent, patternOpen, replacementOpen, RegexOptions.IgnoreCase);
            modifiedHtml = Regex.Replace(modifiedHtml, patternClose, replacementClose, RegexOptions.IgnoreCase);
            return modifiedHtml;
        }
        static string AddIFrameContainer(this string htmlContent, bool isMobile = false)
        {
            if (string.IsNullOrEmpty(htmlContent)) return string.Empty;
            string pattern = @"(<iframe\b[^>]*>.*?</iframe>)"; // Chuỗi thay thế với thẻ <div class="iframe-container">
            string replacement = @"<div class=""iframe-container"">$1</div>"; // Sử dụng Regex để thay thế các thẻ <iframe> với phiên bản có thẻ <div> bao quanh
            string modifiedHtml = Regex.Replace(htmlContent, pattern, replacement, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            return modifiedHtml;
        }
        private static readonly Regex ImgTagRegex = new Regex(@"<img(?![^>]*loading=[""']\w+[""'])", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        static string AddLazyLoadingToImages(this string html)
        {
            if (string.IsNullOrEmpty(html)) return html; // Tránh lỗi khi html null hoặc rỗng

            return ImgTagRegex.Replace(html, "<img loading=\"lazy\"");
        }
        public static string RemoveStyle(string html, string style)
        {
            Regex regex = new Regex(style + "\\s*:.*?;?");

            return regex.Replace(html, string.Empty);
        }

        public static string GetFirstLetterOfLastName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName)) return "A";
            var parts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return parts.Length > 0 ? parts[^1][..1].ToUpper() : "A";
        }

        public static string GetTimeAgo(DateTime commentTime)
        {
            var now = DateTime.Now;
            var timeSpan = now - commentTime;

            if (timeSpan.TotalSeconds < 60)
                return "Vừa xong";
            if (timeSpan.TotalMinutes < 60)
                return $"{(int)timeSpan.TotalMinutes} phút trước";
            if (timeSpan.TotalHours < 24)
                return $"{(int)timeSpan.TotalHours} giờ trước";
            if (timeSpan.TotalDays < 30)
                return $"{(int)timeSpan.TotalDays} ngày trước";
            if (timeSpan.TotalDays < 365)
                return $"{(int)(timeSpan.TotalDays / 30)} tháng trước";

            return $"{(int)(timeSpan.TotalDays / 365)} năm trước";
        }

    }
}
