namespace Upac.Core.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Text;
    using System.Web;

    using Upac.Core.Diagnostics;

    public class UrlString
    {
        #region Fields

        private string _hash;
        private string _hostName;
        private NameValueCollection _parameters;
        private string _path;
        private string _protocol;

        #endregion Fields

        #region Constructors

        public UrlString()
        {
            this._path = string.Empty;
            this._hash = string.Empty;
            this._parameters = new NameValueCollection();
            this._hostName = string.Empty;
            this._protocol = string.Empty;
        }

        public UrlString(NameValueCollection parameters)
        {
            this._path = string.Empty;
            this._hash = string.Empty;
            this._parameters = new NameValueCollection();
            this._hostName = string.Empty;
            this._protocol = string.Empty;
            Assert.EnsureNotNullReference(parameters, "parameters");
            this._parameters.Add(parameters);
        }

        public UrlString(string url)
        {
            this._path = string.Empty;
            this._hash = string.Empty;
            this._parameters = new NameValueCollection();
            this._hostName = string.Empty;
            this._protocol = string.Empty;
            Assert.EnsureNotNullReference(url, "url");
            this.Parse(url);
        }

        #endregion Constructors

        #region Properties

        public string Extension
        {
            set
            {
                Assert.EnsureStringValue(value, "value");
                int length = this._path.LastIndexOf(".");
                this._path = this._path.Substring(0, length) + "." + value;
            }
        }

        public string Hash
        {
            get
            {
                return this._hash;
            }
            set
            {
                Assert.EnsureNotNullReference(value, "value");
                this._hash = value;
            }
        }

        public bool HasPath
        {
            get
            {
                return !string.IsNullOrEmpty(this._path);
            }
        }

        public string HostName
        {
            get
            {
                return this._hostName;
            }
            set
            {
                Assert.EnsureStringValue(value, "value");
                this._hostName = value;
            }
        }

        public NameValueCollection Parameters
        {
            get
            {
                return this._parameters;
            }
        }

        public string Path
        {
            get
            {
                return this._path;
            }
            set
            {
                Assert.EnsureStringValue(value, "value");
                this._path = value;
            }
        }

        public string Protocol
        {
            get
            {
                string str = this._protocol;
                if (str.Length > 0)
                {
                    return str;
                }
                return "http";
            }
            set
            {
                Assert.EnsureNotNullReference(value, "value");
                if (value.IndexOf("://") < 0)
                {
                    throw new ArgumentException("Protocol must not contain '://', only the protocol name should be used (ex. http).");
                }
                this._protocol = value;
            }
        }

        public string Query
        {
            get
            {
                return this.GetQuery();
            }
            set
            {
                Assert.EnsureNotNullReference(value, "value");
                this._parameters.Clear();
                this.ParseQuery(value);
            }
        }

        #endregion Properties

        #region Indexers

        public string this[string key]
        {
            get
            {
                return this._parameters[key];
            }
            set
            {
                Assert.EnsureStringValue(key, "key");
                Assert.EnsureNotNullReference(value, "value");
                this.Append(key, value);
            }
        }

        #endregion Indexers

        #region Methods

        public string Add(string key, string value)
        {
            Assert.EnsureStringValue(key, "key");
            Assert.EnsureNotNullReference(value, "value");
            this._parameters[key] = HttpUtility.UrlEncode(value);
            return this.GetUrl();
        }

        public void Append(NameValueCollection arguments)
        {
            Assert.EnsureNotNullReference(arguments, "arguments");
            foreach (string str in arguments.Keys)
            {
                this.Append(str, arguments[str]);
            }
        }

        public void Append(UrlString url)
        {
            Assert.EnsureNotNullReference(url, "url");
            this.Append(url.Parameters);
        }

        public void Append(string key, string value)
        {
            Assert.EnsureStringValue(key, "key");
            Assert.EnsureNotNullReference(value, "value");
            this._parameters[key] = HttpUtility.UrlEncode(value);
        }

        public string GetUrl()
        {
            return this.GetUrl(false);
        }

        public string GetUrl(bool xhtmlFormat)
        {
            StringBuilder builder = new StringBuilder();
            if (this.HostName.Length > 0)
            {
                builder.Append(this.Protocol);
                builder.Append("://");
                builder.Append(this.HostName);
            }
            builder.Append(this._path);
            if ((this._path.Length > 0) && (this._parameters.Count > 0))
            {
                builder.Append("?");
            }
            builder.Append(this.GetQuery());
            if (!string.IsNullOrEmpty(this._hash))
            {
                builder.Append('#');
                builder.Append(this._hash);
            }
            if (xhtmlFormat)
            {
                builder.Replace("&", "&amp;");
            }
            return builder.ToString();
        }

        public string Remove(string key)
        {
            Assert.EnsureStringValue(key, "key");
            this._parameters.Remove(key);
            return this.GetUrl();
        }

        public override string ToString()
        {
            return this.GetUrl();
        }

        public void Truncate(string key)
        {
            Assert.EnsureStringValue(key, "key");
            this._parameters.Remove(key);
        }

        private string GetQuery()
        {
            StringBuilder builder = new StringBuilder();
            bool flag = true;
            foreach (string str in this._parameters.Keys)
            {
                if (!flag)
                {
                    builder.Append("&");
                }
                builder.Append(str);
                if (this._parameters[str] != string.Empty)
                {
                    builder.Append("=");
                    builder.Append(this._parameters[str]);
                }
                flag = false;
            }
            return builder.ToString();
        }

        private void Parse(string url)
        {
            Assert.EnsureNotNullReference(url, "url");
            int index = url.IndexOf("#", StringComparison.Ordinal);
            if (index >= 0)
            {
                this._hash = StringUtil.Mid(url, index + 1);
                url = StringUtil.Left(url, index);
            }
            int length = url.IndexOf("?", StringComparison.Ordinal);
            bool flag = length >= 0;
            bool flag2 = url.Contains("=");
            bool flag3 = flag || (!flag2 && !flag);
            if (flag && !flag2)
            {
                this._path = StringUtil.Left(url, length);
            }
            else if (!flag2)
            {
                this._path = url;
            }
            else if (flag2 && !flag3)
            {
                this.ParseQuery(url);
            }
            else if (flag)
            {
                this._path = url.Substring(0, length);
                string parameterSection = url.Substring(length + 1);
                this.ParseQuery(parameterSection);
            }
        }

        private void ParseQuery(string parameterSection)
        {
            Assert.EnsureNotNullReference(parameterSection, "parameterSection");
            foreach (string str in parameterSection.Split(new char[] { '&' }))
            {
                string[] strArray2 = str.Split(new char[] { '=' });
                if (strArray2.Length == 1)
                {
                    this._parameters.Add(strArray2[0], string.Empty);
                }
                else
                {
                    this._parameters.Add(strArray2[0], strArray2[1]);
                }
            }
        }

        #endregion Methods
    }
}