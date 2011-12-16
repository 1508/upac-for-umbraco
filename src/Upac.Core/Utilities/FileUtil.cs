namespace Upac.Core.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    using Upac.Core.Diagnostics;

    public static class FileUtil
    {
        #region Methods

        public static string GetVirtualPath(string physicalPath)
        {
            string rootpath = HttpContext.Current.Server.MapPath("/");
            physicalPath = physicalPath.Replace(rootpath, "");
            physicalPath = physicalPath.Replace("\\", "/");
            return "/" + physicalPath;
        }

        /// <summary>
        /// Maps the path. Can handle c:/drive/file.jpg or /folder/file.jpg
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static string MapPath(string path)
        {
            Assert.EnsureStringValue(path, "path");
            if ((path.Length == 0) || (path.IndexOf("://") >= 0))
            {
                return path;
            }
            int num = path.IndexOfAny(new char[] { '\\', '/' });
            if ((num >= 0) && (path[num] == '\\'))
            {
                return path.Replace('/', '\\');
            }
            path = path.Replace('\\', '/');
            HttpServerUtility server = null;
            if (HttpContext.Current != null)
            {
                server = HttpContext.Current.Server;
            }

            if (server != null)
            {
                return server.MapPath(path);
            }
            if (path[0] != '/')
            {
                return path;
            }
            return GetVirtualPath(path);
        }

        #endregion Methods
    }
}