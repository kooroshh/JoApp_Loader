using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JoApp_Loader
{
    class JoLib
    {
        public static string string_0 = "REGISTER";
        public static string string_1 = "LOGIN";
        public static string string_2 = "GETCHARGE";
        public static string string_3 = "NEW_PROJECT";
        public static string string_4 = "ALLOW";
        public static string string_5 = "ALLOWFREE";
        public static string string_6 = "SUCCESS_PROJECT";
        public static string string_7 = "TEMP";
        public static string string_8 = "SUCCESS_REQUEST_IOS";
        public static string string_9 = "NEW_REQUEST_IOS";
        public static string GenerateNewProject()
        {

            //{ "GyOU":"GxIKK1OFG0cSD1D=","pt==":"ZN==","L3Mk":"ZmxjAwH1"}
            string format = "\"{0}\":\"{1}\",";
            string data = "{";
            data += String.Format(format, EncodeToServer("ACT"), "GxIKK1OFG0cSD1D=");
            data += String.Format(format, EncodeToServer("e"), EncodeToServer("0"));
            data += String.Format(format, "L3Mk", "ZmxjAwpl");
            data = data.Substring(0, data.Length - 1);
            data += "}";
            return data;
        }
        public static string GenerateTempPacket()
        {
            //{ "GyOU":"IRIAHN==","pt==":"ZGZlZwV=","JxMH":"2XwMulQLe9zR24mMuPQMuqv02daMuAva2Xbt2LUMughZVAva2LKndqva2LLt2X\/LfqhZ2XsMtqvdVAvh2YUMvAvf24jtDIOYVAzV2XmMvAviVAzT2X\/Lc9vk2X8hVAvb2Xpt2o7LgAvd24mLdAva2LoowPQLdgzS2XsLflQLdAdi24mLfqhZ2X8="}
            string format = "\"{0}\":\"{1}\",";
            string data = "{";
            data += String.Format(format, EncodeToServer("ACT"), "IRIAHN==");
            data += String.Format(format, EncodeToServer("e"), EncodeToServer("0"));
            // data += String.Format(format, EncodeToServer("isFree"), EncodeToServer("synfr"));
            // data += String.Format(format, EncodeToServer("pid"), EncodeToServer("12458"));

            data += String.Format(format, "qzMGMKWl", "MzSfp2H=");
            data += String.Format(format, "L3Mk", "ZmxjAwpl");
            data = data.Substring(0, data.Length - 1);
            data += "}";
            return data;
        }
        public static string GenerateSuccessProject()
        {
            //{"GyOU":"H1IQD0IGH19DHx9XEHAH","pt==":"ZN=="}
            string format = "\"{0}\":\"{1}\",";
            string data = "{";
            data += String.Format(format, EncodeToServer("ACT"), "H1IQD0IGH19DHx9XEHAH");
            data += String.Format(format, EncodeToServer("e"), EncodeToServer("0"));
            data = data.Substring(0, data.Length - 1);
            data += "}";
            return data;
        }
        public static string GenerateAllowPacket()
        {
            string format = "\"{0}\":\"{1}\",";
            string data = "{";
            data += String.Format(format, EncodeToServer("ACT"), EncodeToServer("ARJ_CEBWRPG"));
            data += String.Format(format, EncodeToServer("e"), EncodeToServer("0"));
            data += String.Format(format, EncodeToServer("pid"), EncodeToServer("390672"));
            data += String.Format(format, EncodeToServer("lic"), EncodeToServer("0"));
            data += String.Format(format, EncodeToServer("l"), EncodeToServer("0"));

            data = data.Substring(0, data.Length - 1);
            data += "}";
            return data;
        }
        public static string GenerateLoginPacket()
        {
            string format = "\"{0}\":\"{1}\",";
            string data = "{";
            data += String.Format(format, EncodeToServer("ACT"), "GR9UFH4=");
            data += String.Format(format, EncodeToServer("e"), EncodeToServer("0"));
            data = data.Substring(0, data.Length - 1);
            data += "}";
            return data;
        }
        public static string GenerateRemainingDays()
        {
            string format = "\"{0}\":\"{1}\",";
            string data = "{";
            data += String.Format(format, EncodeToServer("ACT"), EncodeToServer("TRGPUNETR"));
            data += String.Format(format, EncodeToServer("e"), EncodeToServer("0"));
            data += String.Format(format, EncodeToServer("charge"), EncodeToServer("100"));
            data += String.Format(format, EncodeToServer("umsg"), EncodeToServer("0"));
            data += String.Format(format, EncodeToServer("expiredate"), EncodeToServer("0"));
            data += String.Format(format, EncodeToServer("countday"), EncodeToServer("999"));
            data += String.Format(format, EncodeToServer("lastpaycode"), EncodeToServer("0"));
            data = data.Substring(0, data.Length - 1);
            data += "}";
            return data;
        }
        public static string DecodeFromServer(string txt)
        {
            return Encode(FromBase64(Encode(txt)));
        }
        public static string EncodeToServer(string txt)
        {
            return Encode(ToBase64(Encode(txt)));
        }
        public static string EncodePassword(string txt)
        {
            return Encode(ToBase64(ToBase64(txt)));
        }
        public static string ToBase64(string str)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
        }
        public static string FromBase64(string str)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(str));
        }
        public static string Encode(string str)
        {
            char[] array = str.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                int num = (int)array[i];
                if (num >= 97 && num <= 122)
                {
                    if (num > 109)
                    {
                        num -= 13;
                    }
                    else
                    {
                        num += 13;
                    }
                }
                else if (num >= 65 && num <= 90)
                {
                    if (num > 77)
                    {
                        num -= 13;
                    }
                    else
                    {
                        num += 13;
                    }
                }
                array[i] = (char)num;
            }
            return new string(array);
        }
        static Random r = new Random();

    }
}
