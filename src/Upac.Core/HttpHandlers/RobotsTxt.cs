namespace Upac.Core.HttpHandlers
{
    using System.Collections;
    using System.IO;
    using System.Web;

    /// <summary>
    /// This HttpHandler will pass the robots.txt file.
    /// It main purpose is to set Disallow: / when on a 1508test.dk server and when in prouction site set the sitemap.org path.
    /// Velocity Template Language (VTL) is used when parsing the file on the fly.
    /// 
    /// It should be registered in web.config
    /// 
    /// It will only works on IIS7
    /// </summary>
    /// <example>
    /// <code lang="xml" title="How to setup the handler in web.config">
    /// <![CDATA[
    /// <add verb="GET" name="RobotsTxt" path="/robots.txt" type="Upac.Core.HttpHandlers.RobotsTxt, Upac.Core"/>
    /// ]]>
    /// </code>
    /// </example>
    /// <example>
    /// <code lang="txt" title="Sample robots.txt file">
    /// <![CDATA[
    ///#*
    ///    This is a comment and will not be shown in the final output.
    ///	
    ///    This HttpHandler wil parse the contents of this file as a velocity template engine.
    ///    <add verb="GET" name="RobotsTxt" path="/robots.txt" type="Upac.Core.HttpHandlers.RobotsTxt, Upac.Core"/>
    ///	
    ///    One object HttpContext.Current.Request.ServerVariables with the key ServerVariables is given to the parsing
    ///	
    ///    A guide and reference for the Velocity Template Language (VTL). Can you find here
    ///    http://velocity.apache.org/engine/releases/velocity-1.5/vtl-reference-guide.html
    ///*#
    ///User-agent: *
    ///#if($ServerVariables.SERVER_NAME.Contains(".1508test.dk"))
    ///Disallow: /
    ///#else
    ///Disallow: /aspnet_client/
    ///Disallow: /bin/
    ///Disallow: /config/
    ///Disallow: /css/
    ///Disallow: /data/
    ///Disallow: /scripts/
    ///Disallow: /umbraco/
    ///Disallow: /umbraco_client/
    ///Disallow: /usercontrols/
    ///Disallow: /xslt/
    ///Sitemap: http://$ServerVariables.SERVER_NAME/SitemapDotOrg
    ///#end
    /// ]]>
    /// </code>
    /// </example>
    public class RobotsTxt : IHttpHandler
    {
        #region Properties

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"/> instance.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler"/> instance is reusable; otherwise, false.
        /// </returns>
        public bool IsReusable
        {
            get
            {
                return true;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string path = HttpContext.Current.Server.MapPath(VirtualPathUtility.ToAbsolute("~/robots.txt"));
            if (File.Exists(path))
            {
                StreamReader reader = File.OpenText(path);
                string fileContents = reader.ReadToEnd();
                reader.Close();
                reader.Dispose();

                Hashtable ht = new Hashtable();
                ht.Add("ServerVariables", HttpContext.Current.Request.ServerVariables);
                string newContent = Upac.Core.Utilities.VelocityUtil.Evaluate(fileContents, ht);
                context.Response.Write(newContent);
            }
            else
            {
                context.Response.Write("");
            }
        }

        #endregion Methods
    }
}