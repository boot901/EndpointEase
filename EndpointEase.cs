using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
namespace EndpointEase
{
    public static class ConfigCreation
    {
        
        public static EndpointConfiguration StartConfig(string name, string errorName,bool enableinstall) {
            var e = new EndpointConfiguration(name);
            e.SendFailedMessagesTo(errorName);
            if (enableinstall) { e.EnableInstallers(); }
            return e;
        }


        #region usage
        public static void addUsage<serial,transport,persist>(ref EndpointConfiguration e,string connectionstring=null) where serial : NServiceBus.Serialization.SerializationDefinition,new()
                                                                                           where transport :NServiceBus.Transport.TransportDefinition,new()
                                                                                           where persist :NServiceBus.Persistence.PersistenceDefinition,new()
        {
            e.UseSerialization<serial>();
            e.UsePersistence<persist>();
            if (connectionstring == null)
            {
                e.UseTransport<transport>();
            }
            else
            {
                e.UseTransport<transport>().ConnectionString(connectionstring);
            }
        }
        public static void addUsage<serial, transport>(ref EndpointConfiguration e,string connectionstring=null,string notset=null) where serial : NServiceBus.Serialization.SerializationDefinition, new()
                                                                                           where transport : NServiceBus.Transport.TransportDefinition, new()
                                                                                          
        {
            e.UseSerialization<serial>();
            e.UsePersistence<InMemoryPersistence>();
            if (connectionstring == null)
            {
                e.UseTransport<transport>();
            }
            else {
                e.UseTransport<transport>().ConnectionString(connectionstring);
            }
        }

        public static void addUsage<transport, persist>(ref EndpointConfiguration e,string connectionstring=null) where transport : NServiceBus.Transport.TransportDefinition, new()
                                                                                      where persist : NServiceBus.Persistence.PersistenceDefinition, new()
        {
            e.UseSerialization<JsonSerializer>();
            e.UsePersistence<persist>();
            e.UseTransport<transport>();
        }
        public static void addUsage<serial, persist>(ref EndpointConfiguration e, object noset = null) where serial : NServiceBus.Serialization.SerializationDefinition, new()
                                                                                      where persist : NServiceBus.Persistence.PersistenceDefinition, new()
        {
            e.UseSerialization<serial>();
            e.UsePersistence<persist>();
            e.UseTransport<MsmqTransport>();

        }
        public static void addUsage<serial>(ref EndpointConfiguration e) where serial : NServiceBus.Serialization.SerializationDefinition, new()
        {
            e.UseSerialization<serial>();
            e.UsePersistence<InMemoryPersistence>();
            e.UseTransport<MsmqTransport>();
        }
        public static void addUsage<persist>(ref EndpointConfiguration e,object noset=null) where persist : NServiceBus.Persistence.PersistenceDefinition, new()
        {
            e.UseSerialization<JsonSerializer>();
            e.UsePersistence<persist>();
            e.UseTransport<MsmqTransport>();
        }
        public static void addUsage<transport>(ref EndpointConfiguration e, string connectionstring = null) where transport : NServiceBus.Transport.TransportDefinition, new()
        {
            e.UseSerialization<JsonSerializer>();
            e.UsePersistence<InMemoryPersistence>();
            if (connectionstring == null)
            {
                e.UseTransport<transport>();
            }
            else
            {
                e.UseTransport<transport>().ConnectionString(connectionstring);
            }
        }


        #endregion
      
        public static void Recoverability(ref EndpointConfiguration e,int immediateretries = 3, float delaysecs = 1, int delayretries = 3) {
            var recover = e.Recoverability();
            recover.Immediate(immediate=> { immediate.NumberOfRetries(immediateretries); });
            recover.Delayed(delay => { delay.TimeIncrease(TimeSpan.FromSeconds((double)delaysecs)); delay.NumberOfRetries(delayretries); });

        }

    }
}
