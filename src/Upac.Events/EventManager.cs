namespace Upac.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using log4net;

    using umbraco.BusinessLogic;

    using Upac.Events.Configuration;

    public class EventManager : umbraco.BusinessLogic.ApplicationBase
    {
        #region Fields

        private static readonly ILog log = LogManager.GetLogger(typeof(EventManager));

        #endregion Fields

        #region Constructors

        public EventManager()
        {
            log.Info("Start - Attach eventhandlers");

            EventCollection events = ConfigurationManager.Settings.Events;
            log.Info(string.Format("    {0} events found", events.Count));

            foreach (EventElement e in events)
            {
                log.InfoFormat("        TargetType: {0} TargetEvent: {1} Enabled: {2}", e.TargetType, e.TargetEvent, e.Enabled);
                if (e.Enabled)
                {
                    if (e.Handlers.Count > 0)
                    {
                        log.Info(string.Format("            {0} handlers found", e.Handlers.Count));
                        foreach (HandlerElement handler in e.Handlers)
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
            log.Info("Ended - Attach eventhandlers");
        }

        #endregion Constructors
    }
}