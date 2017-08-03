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
        //starts the endpoint with an erorqueue and installers if needed.
        public static EndpointConfiguration StartConfig(string name, string errorName,bool enableinstall=true) {
            var e = new EndpointConfiguration(name);
            e.SendFailedMessagesTo(errorName);
            if (enableinstall) { e.EnableInstallers(); }
            return e;
        }

     
        
        public static void addUsage<serial,transport,persist>(ref EndpointConfiguration e,string connectionstring=null) 
            where serial : NServiceBus.Serialization.SerializationDefinition,new()
            where transport :NServiceBus.Transport.TransportDefinition,new()
            where persist :NServiceBus.Persistence.PersistenceDefinition
        {
            e.UseSerialization<serial>();
            e.UsePersistence<persist>();

            if (connectionstring == null){e.UseTransport<transport>();}
            else{ e.UseTransport<transport>().ConnectionString(connectionstring); }
        }
        // in this region and function above you can find a bunch of methods that 
        //allocates the endpoint to have different uses like for example JsonSerializer
        #region usage


        public static void addUsage<serial, transport, persist>(ref EndpointConfiguration e,ref object ser, ref object pers, ref object trans, string connectionstring = null) where serial : NServiceBus.Serialization.SerializationDefinition, new()
                                                                                           where transport : NServiceBus.Transport.TransportDefinition, new()
                                                                                           where persist : NServiceBus.Persistence.PersistenceDefinition
        {
           ser= e.UseSerialization<serial>();
            pers=e.UsePersistence<persist>();
            if (connectionstring == null)
            {
              trans=e.UseTransport<transport>();
            }
            else
            {
                trans=e.UseTransport<transport>().ConnectionString(connectionstring);
            }
            
        }
        public static void addUsage<serial, transport, persist,databus,container>(ref EndpointConfiguration e, Action<NserviceBus.Container.ContainerCustomizations> custom = null, string connectionstring = null) where serial : NServiceBus.Serialization.SerializationDefinition, new()
                                                                                           where transport : NServiceBus.Transport.TransportDefinition, new()
                                                                                           where persist : NServiceBus.Persistence.PersistenceDefinition
        {
            e.UseContainer<container>();
            e.UseDataBus<databus>();
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
        public static void addUsage<serial, transport, persist,databus,container>(ref EndpointConfiguration e, ref object ser, ref object pers, ref object trans, ref object dat, Action<NserviceBus.Container.ContainerCustomizations> custom = null, string connectionstring = null) where serial : NServiceBus.Serialization.SerializationDefinition, new()
                                                                                           where transport : NServiceBus.Transport.TransportDefinition, new()
                                                                                           where persist : NServiceBus.Persistence.PersistenceDefinition
                                                                                           where databus : NServiceBus.DataBus.DataBusDefinition,new()
                                                                                           where container : NServiceBus.Container.ContainerDefinition,new()
                                                                                
        {
            e.UseContainer<container>();
            dat = e.UseDataBus<databus>();
            ser = e.UseSerialization<serial>();
            pers = e.UsePersistence<persist>();
            if (connectionstring == null)
            {
                trans = e.UseTransport<transport>();
            }
            else
            {
                trans = e.UseTransport<transport>().ConnectionString(connectionstring);
            }

        }




        #endregion

        // adds recoverability if needed.
        //recoverability allows code to retry for say example, if someone is starting to 
        //connect to internet but not fully connected and once they connect it will work.
        public static void recover(ref EndpointConfiguration e,int immediateretries = 3, float delaysecs = 1, int delayretries = 3) {

            var recover = e.Recoverability();

            recover.Immediate(immediate=> { immediate.NumberOfRetries(immediateretries); });
            recover.Delayed(delay => { delay.TimeIncrease(TimeSpan.FromSeconds((double)delaysecs)); delay.NumberOfRetries(delayretries); });

        }
        //makes an endpoint no time flat
        
        public static EndpointConfiguration FullConfig<serial, transport, persist>(string name, string errorName, bool enableinstall = true, 
                                                                                   string connection = null, bool useRetries = false, 
                                                                                   int immediateretries = 0, float delaysecs = 0, 
                                                                                   int delayretries = 0)
            where serial : NServiceBus.Serialization.SerializationDefinition,new()
            where transport :NServiceBus.Transport.TransportDefinition,new()
            where persist : NServiceBus.Persistence.PersistenceDefinition
        {
           var e= StartConfig(name, errorName, enableinstall);
            addUsage<serial, transport, persist>(ref e, connection);
            if (useRetries) { recover(ref e, immediateretries, delaysecs, delayretries); }


        }
        #region fullconfig
        public static EndpointConfiguration FullConfig<serial, transport, persist,databus,container>(string name, string errorName, bool enableinstall = true,
                                                                                                    string connection = null, Action<NserviceBus.Container.ContainerCustomizations> custom = null, bool useRetries = false,
                                                                                                    int immediateretries = 0, float delaysecs = 0,
                                                                                                     int delayretries = 0)
            where serial : NServiceBus.Serialization.SerializationDefinition, new()
            where transport : NServiceBus.Transport.TransportDefinition, new()
            where persist : NServiceBus.Persistence.PersistenceDefinition
            where databus : NServiceBus.DataBus.DataBusDefinition, new()
            where container : NServiceBus.Container.ContainerDefinition, new()

        {
            var e = StartConfig(name, errorName, enableinstall);
            addUsage<serial, transport, persist,databus,container>(ref e,custom , connection);
            if (useRetries) { recover(ref e, immediateretries, delaysecs, delayretries); }


        }
        public static EndpointConfiguration FullConfig<serial, transport, persist>(string name, string errorName, ref object ser, 
                                                                                   ref object pers, ref object trans, bool enableinstall = true,
                                                                                   string connection = null, bool useRetries = false,
                                                                                   int immediateretries = 0, float delaysecs = 0,
                                                                                   int delayretries = 0)
           where serial : NServiceBus.Serialization.SerializationDefinition, new()
           where transport : NServiceBus.Transport.TransportDefinition, new()
           where persist : NServiceBus.Persistence.PersistenceDefinition
        {
            var e = StartConfig(name, errorName, enableinstall);
            addUsage<serial, transport, persist>(ref e, ref ser, ref pers, ref trans, connection);
            if (useRetries) { recover(ref e, immediateretries, delaysecs, delayretries); }


        }
        public static EndpointConfiguration FullConfig<serial, transport, persist, databus, container>(string name, string errorName, ref object ser,
                                                                                                       ref object pers, ref object trans,ref object dat , bool enableinstall = true,
                                                                                                       string connection = null, 
                                                                                                       Action<NserviceBus.Container.ContainerCustomizations> custom = null, 
                                                                                                       bool useRetries = false,
                                                                                                       int immediateretries = 0, float delaysecs = 0,
                                                                                                       int delayretries = 0)
            where serial : NServiceBus.Serialization.SerializationDefinition, new()
            where transport : NServiceBus.Transport.TransportDefinition, new()
            where persist : NServiceBus.Persistence.PersistenceDefinition
            where databus : NServiceBus.DataBus.DataBusDefinition, new()
            where container : NServiceBus.Container.ContainerDefinition, new()

        {
            var e = StartConfig(name, errorName, enableinstall);
            addUsage<serial, transport, persist, databus, container>(ref e,ser,pers,trans,dat, custom, connection);
            if (useRetries) { recover(ref e, immediateretries, delaysecs, delayretries); }


        }
#endregion


    }
}
