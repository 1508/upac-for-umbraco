namespace Upac.Core.Events
{
    using System;
    using System.Reflection;

    using Upac.Core.Configuration.Elements;

    using log4net;

    using umbraco.BusinessLogic;
    using umbraco.cms.businesslogic.web;

    public class EventManager : ApplicationBase
    {
        #region Fields

        private static readonly ILog log = LogManager.GetLogger(typeof(EventManager));

        #endregion Fields

        #region Constructors

        public EventManager()
        {
            log.Info("Start - Attach eventhandlers");

            Upac.Core.Configuration.Elements.Event[] events = Upac.Core.Configuration.ConfigurationManager.UpacSettings.UmbracoEvents.Events;
            log.Info(string.Format("    {0} events found", events.Length));

            foreach (Event e in events)
            {
                log.InfoFormat("        TargetType: {0} TargetEvent: {1} Enabled: {2}", e.TargetType, e.TargetEvent, e.Enabled);
                if (e.Enabled)
                {
                    if (e.Handlers.Length > 0)
                    {
                        log.Info(string.Format("            {0} handlers found", e.Handlers.Length));
                        foreach (Core.Configuration.Elements.EventHandler handler in e.Handlers)
                        {
                            log.Info(string.Format("                Type: {0} Method: {1} Enabled: {2}", handler.Type, handler.Method, handler.Enabled));
                            if (handler.Enabled)
                            {
                                log.Info(string.Format("                    Adding Type: {0} Method: {1}", handler.Type, handler.Method));

                                Type type = System.Web.Compilation.BuildManager.GetType(handler.Type, false);

                                if (type == null)
                                {
                                    log.Error(string.Format("               could not find type: {0}", handler.Type));
                                }
                                else
                                {
                                    MethodInfo handlerMethod = type.GetMethod(handler.Method);

                                    Type targetType = System.Web.Compilation.BuildManager.GetType(e.TargetType, true);
                                    if (targetType == null)
                                    {
                                        log.Error(string.Format("                   could not find targetType: {0}", e.TargetType));
                                    }
                                    else
                                    {
                                        EventInfo targetEvent = targetType.GetEvent(e.TargetEvent);
                                        if (targetEvent == null)
                                        {
                                            log.Error(string.Format("                    could not find targetEvent: {0}", e.TargetEvent));

                                        }
                                        else
                                        {
                                            Type handlerType = targetEvent.EventHandlerType;
                                            if (handlerType == null)
                                            {
                                                log.Error(string.Format("                      could not find handlerType on targetEvent: {0}", e.TargetEvent));
                                            }
                                            else
                                            {
                                                Delegate d = Delegate.CreateDelegate(handlerType, handlerMethod);
                                                targetEvent.AddEventHandler(targetType, d);
                                                log.InfoFormat("Event added");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
            }

            //Type type2 = System.Web.Compilation.BuildManager.GetType("Upac.Core.Events.MemberSave, Upac.Core", true);
            //MethodInfo handlerMethod2 = type2.GetMethod("Save");

            log.Info("Ended - Attach eventhandlers");
        }

        #endregion Constructors

    }
}