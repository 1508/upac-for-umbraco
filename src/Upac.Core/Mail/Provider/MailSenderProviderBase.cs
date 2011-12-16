namespace Upac.Core.Mail.Provider
{
    using System.Configuration.Provider;
    using System.Net.Mail;

    public abstract class MailSenderProviderBase : ProviderBase
    {
        #region Fields

        protected string _name;

        #endregion Fields

        #region Properties

        public override string Name
        {
            get { return _name; }
        }

        #endregion Properties

        #region Methods

        public abstract bool Send(MailMessage message);

        private static MailSenderProviderBase provider = null;
        private static GenericProviderCollection<MailSenderProviderBase>
           providers = ProviderHelper.LoadProviders<MailSenderProviderBase>
               ("SearchProvider", out provider);


        #endregion Methods
    }
}