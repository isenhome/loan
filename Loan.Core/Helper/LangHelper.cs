using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Xml.Linq;
using System.Web.UI;
using System.Web.Mvc;
using System.Web;

namespace Loan.Core.Helper
{
    /// <summary>
    /// Author：Bruce
    /// Blog：   http://coolcode.cnblogs.com
    /// </summary>
    public static class LangHelper
    {
        /// <summary>
        /// 在 Html 中直接使用
        /// </summary>
        /// <param name="htmlhelper"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Lang(this HtmlHelper htmlhelper, string key)
        {
            return GetLangString(key);
        }
        

        public static string Trans(this HtmlHelper htmlhelper, string text)
        {
            return GetLangTransString(text);
        }

        /// <summary>
        /// 在js中使用
        /// </summary>
        /// <param name="htmlhelper"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string LangOutJsVar(this HtmlHelper htmlhelper, string key)
        {
            string langstr = GetLangString(key);
            return string.Format("var {0} = '{1}'", key, langstr);
        }


        /// <summary>
        /// Controller中使用
        /// </summary>
        /// <param name="control"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Lang(this Controller control, string key)
        {
            return GetLangString(key);
        }

        public static string Trans(this Controller control, string text)
        {
            return GetLangTransString(text);
        }

        public static string Lang(this HttpContextBase httpContext, string key)
        {
            return GetLangString(key);
        }

        public static string Trans(this HttpContextBase httpContext, string text)
        {
            return GetLangTransString(text);
        }

        public static string Lang(this Control control, string key)
        {
            return GetLangString(key);
        }

        public static string Trans(this Control control, string text)
        {
            return GetLangTransString(text);
        }

        public static string Lang(string key)
        {
            return GetLangString(key);
        }

        public static string Trans(string text)
        {
            return GetLangTransString(text);
        }

        public static ILanguageProvider langProvider = null;

        private static string GetLangString(string key)
        {
            string lang = GetLangCode();
            /*加载语言，只在第一次使用时加载*/
            if (langProvider == null)
            {
                langProvider = new LanguageProvider();
            }
            return langProvider.GetLanguage(lang, key);
        }

        private static string GetLangTransString(string text)
        {
            string lang = GetLangCode();
            /*加载语言，只在第一次使用时加载*/
            if (langProvider == null)
            {
                langProvider = new LanguageProvider();
            }
            return langProvider.GetLanguageTrans(lang, text);
        }

        public static string GetLangCode()
        {
            //HttpRequest Request = HttpContext.Current.Request;
            ///*默认语言*/
            //string lang = FuncUtil.GetFunction("DefaultLang").Attrs["Value"];
            ///*检查cookies是否有用户的语言设置*/
            //if (Request.Cookies[CookieKeyConst.LanguageCodeKey] != null)
            //    lang = Request.Cookies[CookieKeyConst.LanguageCodeKey].Value;
            /*begin：如果cookies中没有就取客户端的语言*/
            //else if (Request.UserLanguages != null && Request.UserLanguages.Length > 0)
            //{
            //    lang = Request.UserLanguages[0];
            //}
            /*end：如果cookies中没有就取客户端的语言*/
            
            return "zh-cn";
        }

        public static bool IsEnglishVersion
        {
            get
            {
                return GetLangCode().ToLower() == "en-us";
            }
        }
    }

    /// <summary>
    /// 语言提供者接口
    /// </summary>
    public interface ILanguageProvider
    {
        /// <summary>
        /// 查询相应语言值
        /// </summary>
        /// <param name="lang">语言名称</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        string GetLanguage(string lang, string key);

        /// <summary>
        /// 查询数据库文本资源相应语言值
        /// </summary>
        /// <param name="lang">语言名称</param>
        /// <param name="text">语言文本</param>
        /// <returns></returns>
        string GetLanguageTrans(string lang, string text);
    }

    /// <summary>
    /// 语言提供者
    /// </summary>
    public class LanguageProvider : ILanguageProvider
    {
        /// <summary>
        /// 存放语言文件的虚拟路径(可自行修改为动态路径)
        /// </summary>
        private const string langPath = "~/Languages/";
        private const string transtext = "trans.xml";
        private const string transtextPath = langPath + transtext;
        const string langdb="zh-cn";
        /// <summary>
        /// 语言文件的后缀
        /// </summary>
        private string extName = ".resx";
        /// <summary>
        /// 语言集（key是语言的名称，如zh-cn，en-us等）
        /// </summary>
        private Dictionary<string, ILanguage> langResources;

        /// <summary>
        /// 数据库语言集合，key是中文，value是其他多语言集合（key是语言的名称，如zh-cn，en-us等）
        /// </summary>
        private Dictionary<string, Dictionary<string,string>> langTransResources;

        public LanguageProvider()
            : this(null)
        {
        }

        public LanguageProvider(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                filePath = HttpContext.Current.Server.MapPath(langPath);
            langResources = new Dictionary<string, ILanguage>();
            langTransResources = new Dictionary<string, Dictionary<string, string>>();

            string[] files = Directory.GetFiles(filePath);
            files = files.Where(c => c.EndsWith(extName)).ToArray();
            foreach (var file in files)
            {
                ILanguage reader = new Language();
                reader.Read(file);
                langResources.Add(getKey(file), reader);
            }

            loadDbResource(filePath);
            /*监视语言文件，如果有文件更新就更新相应的语言*/
            //Watch(filePath);
        }

        /// <summary>
        /// 查询相应语言值
        /// </summary>
        /// <param name="lang">语言名称</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        public string GetLanguage(string lang, string key)
        {
            if (key == "") return string.Empty;
            var r = langResources.ContainsKey(lang) ? langResources[lang] : langResources.First().Value;
            return r.Get(key);
        }

        /// <summary
        /// 查询数据库文本资源相应语言值
        /// </summary>
        /// <param name="lang">语言名称</param>
        /// <param name="text">语言文本</param>
        /// <returns></returns>
        public string GetLanguageTrans(string lang, string text)
        {
            if(!langTransResources.ContainsKey(text.Trim()))
                return text;
            Dictionary<string,string> langs=langTransResources[text.Trim()];
            if (!langs.ContainsKey(lang))
                return text;
            return langs[lang];
        }

        /// <summary>
        /// 从文件路径获取语言的名字（例如C:\zh-cn.resx 的语言名字是zh-cn）
        /// </summary>
        /// <param name="file">文件路径</param>
        /// <returns></returns>
        private static string getKey(string file)
        {
            return Path.GetFileNameWithoutExtension(file);
        }

        private FileSystemWatcher watcher = new FileSystemWatcher();
        private FileSystemWatcher watcherTrans = new FileSystemWatcher();

        /// <summary>
        /// 监视语言文件
        /// </summary>
        /// <param name="filePath">存放语言文件的物理路径</param>
        private void Watch(string filePath)
        {
            watcher.Path = filePath;
            watcher.IncludeSubdirectories = true;
            watcher.Filter = "*" + extName;
            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;

            watcherTrans.Path = filePath;
            watcherTrans.IncludeSubdirectories = true;
            watcherTrans.Filter = transtext;
            watcherTrans.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
            watcherTrans.Created += new FileSystemEventHandler(OnChangedDbText);
            watcherTrans.Changed += new FileSystemEventHandler(OnChangedDbText);
            watcherTrans.EnableRaisingEvents = true;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            /*如果有文件更新就更新相应的语言*/
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                string file = e.FullPath;
                if (File.Exists(file))
                {
                    ILanguage reader = new Language();
                    reader.Read(file);
                    string lang = getKey(file);
                    if (!langResources.ContainsKey(lang))
                        langResources.Add(lang, reader);
                    else
                        langResources[lang] = reader;
                }
            }
        }

        private void OnChangedDbText(object sender, FileSystemEventArgs e)
        {
            /*如果有文件更新就更新相应的语言*/
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                string file = e.FullPath;
                if (File.Exists(file))
                {
					loadDbResource(file.Substring(0, file.LastIndexOf('\\') + 1));
                }//if (File.Exists(file))
            }//if (e.ChangeType == WatcherChangeTypes.Changed)
        }

        void loadDbResource(string filepath = "")
        {
            langTransResources.Clear();
            XElement xe = null;
            if (string.IsNullOrEmpty(filepath))
            {
                xe = XElement.Load(HttpContext.Current.Server.MapPath(transtextPath));
            }
            else
            {
                xe = XElement.Load(filepath + transtext);
            }
            var xes = xe.Descendants("text");
            foreach (var x in xes)
            {
                string text = x.Element(langdb).Value.Trim();
                if (!string.IsNullOrEmpty(text) && !langTransResources.Keys.Contains(text))
                {
                    Dictionary<string, string> item = new Dictionary<string, string>();
                    var xcs = x.Elements();
                    foreach (var c in xcs)
                    {
                        if (!item.Keys.Contains(c.Name.ToString()))
                        {
                            item.Add(c.Name.ToString(), c.Value);
                        }
                    }
                    langTransResources.Add(text, item);
                }
            }//foreach (var x in xes)
        }
    }

    /// <summary>
    /// 语言接口
    /// </summary>
    public interface ILanguage
    {
        /// <summary>
        /// 加载语言文件
        /// </summary>
        /// <param name="fileName">语言文件路径</param>
        void Read(string fileName);
        /// <summary>
        /// 获取语言
        /// </summary>
        /// <param name="key">键</param> 
        string Get(string key);
    }

    /// <summary>
    /// 语言 
    /// </summary>
    class Language : Dictionary<string, string>, ILanguage
    {
        /// <summary>
        /// 加载语言文件（这里语言文件是一般资源文件，其实只要是符合这样结构的xml文件就可以：
        ///    <data name="key"><value>文本</value></data>)
        /// </summary>
        /// <param name="fileName">语言文件路径</param>
        public void Read(string fileName)
        {
            var xe = XElement.Load(fileName);
            var xes = xe.Elements().Where(c => c.Name == "data" &&
                c.Attribute("name") != null && c.Element("value") != null).ToList();
            foreach (var x in xes)
            {
                this.Add(x.Attribute("name").Value, x.Element("value").Value);
            }
        }

        /// <summary>
        /// 获取语言
        /// </summary>
        /// <param name="key">键</param> 
        public string Get(string key)
        {
            if (this.ContainsKey(key))
                return this[key];
            return string.Format("Language about {0} not found!", key);
        }
    }

}