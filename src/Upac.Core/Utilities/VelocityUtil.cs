namespace Upac.Core.Utilities
{
    using System.Collections;
    using System.IO;

    using NVelocity;
    using NVelocity.App;

    public static class VelocityUtil
    {
        #region Methods

        public static string Evaluate(string template, Hashtable variables)
        {
            string returnValue;
            if (variables.Count == 0)
            {
                returnValue = template;
            }
            else
            {
                VelocityContext context = new VelocityContext(variables);
                Velocity.Init();
                using (StringWriter velocityOutput = new StringWriter())
                {
                    Velocity.Evaluate(context, velocityOutput, "", template);
                    returnValue = velocityOutput.GetStringBuilder().ToString();
                }
            }
            return returnValue;
        }

        #endregion Methods
    }
}