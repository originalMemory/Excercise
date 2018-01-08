using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net;
using System.IO;

namespace CSharpTest.Tools
{
    public class NewGSDataSDK
    {
        private string Appid;
        private string Appkey;


        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="appid">API用户唯一标识</param>
        /// <param name="appkey">API用户签名私钥</param>
        public NewGSDataSDK(string appid, string appkey)
        {
            Appid = appid;
            Appkey = appkey;
        }

        public string Call(string method, string service, string host, string request_parameters)
        {
            DateTime now = DateTime.Now;
            string gsdate = now.ToString("yyyyMMddTHHmmssZ");
            string datestamp = now.ToString("yyyyMMdd");

            //创建规范URI——从域到查询的URI的一部分 
            string canonical_uri = service;

            /*
             Step 3: 创建规范查询字符串。在这个例子中 (a GET request),
             按字符代码点以升序顺序对参数名称进行排序。例如，以大写字母 F 开头的参数名称排在以小写字母 b 开头的参数名称之前。
             请勿对 RFC 3986 定义的任何非预留字符进行 URI 编码，这些字符包括：A-Z、a-z、0-9、连字符 (-)、下划线 (_)、句点 (.) 和波浪符 ( ~ )。
             使用 %XY 对所有其他字符进行百分比编码，其中“X”和“Y”为十六进制字符（0-9 和大写字母 A-F）。例如，空格字符必须编码为 %20（不像某些编码方案那样使用“+”），扩展 UTF-8 字符必须采用格式 %XY%ZA%BC。
             */
            string canonical_querystring = request_parameters;

            /* Step 4: 添加规范标头. 
                # 规范标头包括您要包含在签名请求中的所有 HTTP 标头的列表
                # 要创建规范标头列表，请将所有标头名称转换为小写形式并删除前导空格和尾随空格。将标头值中的连续空格转换为单个空格
                # 追加小写标头名称，后跟冒号
                # 追加该标头的值的逗号分隔列表。请勿对有多个值的标头进行值排序
             */
            string canonical_headers = "host:" + host + "\n" + "x-gsdata-date:" + gsdate;

            /* # Step 5: 添加已签名的标头. 
#该值是您包含在规范标头中的标头列表。通过添加此标头列表，您可以向 GSDATA 告知请求中的哪些标头是签名过程的一部分以及在验证请求时 GSDATA 可以忽略哪些标头
# host 标头必须作为已签名标头包括在内。如果包括日期或 x-gsdata-date 标头，则还必须包括在已签名标头列表中的标头。
# 要创建已签名标头列表，请将所有标头名称转换为小写形式，按字符代码对其进行排序，并使用分号来分隔这些标头名称。
             */
            string signed_headers = "host;x-gsdata-date";

            /*
             * 使用 SHA256 等哈希 (摘要) 函数以基于 HTTP 或 HTTPS 请求正文中的负载创建哈希值. 
             * 如果负载为空，则使用空字符串作为哈希函数的输入.
             */
            string payload_hash = GetSHA256HashFromString("");

            //要构建完整的规范请求，请将来自每个步骤的所有组成部分组合为单个字符串
            string canonical_request = method + "\n" + canonical_uri + "\n" + canonical_querystring + "\n" + canonical_headers + "\n" + signed_headers + "\n" + payload_hash;

            //************* TASK 2: 创建要签名的字符串*************
            //以算法名称开头，后跟换行符。该值是您用于计算规范请求摘要的哈希[SHA256]
            string algorithm = "GSDATA-HMAC-SHA256";
            string string_to_sign = algorithm + "\n" + gsdate + "\n" + GetSHA256HashFromString(canonical_request);

            // ************* TASK 3: 计算签名 *************
//使用上面定义的函数创建签名密钥.
            string signing_key = getSignatureKey(Appkey, datestamp, service);
            //var signature = crypto.HmacSHA256(stringToSign, getSignatureKey(crypto, GSDataAPI_APP_KEY, dateStamp, canonicalURI)).toString()
            string signature=HmacSHA256(string_to_sign,signing_key);

            //************* TASK 4: 向请求添加签名信息 *************
            string authorization_header = algorithm + " " + "AppKey=" + Appid + ", " + "SignedHeaders=" + signed_headers + ", " + "Signature=" + signature;


            string request_url = "http://"+host+service + "?" + canonical_querystring;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(request_url);
            WebHeaderCollection headers = new WebHeaderCollection();
            headers.Add("x-gsdata-date", gsdate);
            headers.Add("x-Authorization-date", authorization_header);
            request.Headers = headers;

            var response = request.GetResponse() as HttpWebResponse;
            StreamReader sr = new StreamReader(response.GetResponseStream());
            return sr.ReadToEnd();
        }

        public static string GetSHA256HashFromString(string strData)
        {
            byte[] bytValue = System.Text.Encoding.UTF8.GetBytes(strData);
            try
            {
                SHA256 sha256 = new SHA256CryptoServiceProvider();
                byte[] retVal = sha256.ComputeHash(bytValue);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetSHA256HashFromString() fail,error:" + ex.Message);
            }
        }

        private string HmacSHA256(string message, string secret)
        {
            secret = secret ?? "";
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }
        
        string getSignatureKey(string key, string dateStamp, string serviceName)
        {
            string kDate = HmacSHA256(dateStamp, "GSDATA" + key);
            var kService = HmacSHA256(serviceName, kDate);
            var kSigning = HmacSHA256("gsdata_request", kService);
            return kSigning;
        }

        /// <summary>
        /// 哈希加密算法
        /// </summary>
        /// <param name="hashAlgorithm"> 所有加密哈希算法实现均必须从中派生的基类 </param>
        /// <param name="input"> 待加密的字符串 </param>
        /// <param name="encoding"> 字符编码 </param>
        /// <returns></returns>
        private string HashEncrypt(HashAlgorithm hashAlgorithm, string input, Encoding encoding)
        {
            var data = hashAlgorithm.ComputeHash(encoding.GetBytes(input));

            return BitConverter.ToString(data).Replace("-", "");
        }

        #region HMAC-SHA256 加密

        /// <summary>
        /// HMAC-SHA256 加密
        /// </summary>
        /// <param name="input"> 要加密的字符串 </param>
        /// <param name="key"> 密钥 </param>
        /// <param name="encoding"> 字符编码 </param>
        /// <returns></returns>
        public string HMACSHA256Encrypt(string input, string key, Encoding encoding)
        {
            return HashEncrypt(new HMACSHA256(encoding.GetBytes(key)), input, encoding);
        }

        #endregion HMAC-SHA256 加密
    }
}
