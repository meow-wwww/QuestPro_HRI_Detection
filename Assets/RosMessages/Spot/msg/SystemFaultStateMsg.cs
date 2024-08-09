//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.Spot
{
    [Serializable]
    public class SystemFaultStateMsg : Message
    {
        public const string k_RosMessageName = "spot_msgs/SystemFaultState";
        public override string RosMessageName => k_RosMessageName;

        public SystemFaultMsg[] faults;
        public SystemFaultMsg[] historical_faults;

        public SystemFaultStateMsg()
        {
            this.faults = new SystemFaultMsg[0];
            this.historical_faults = new SystemFaultMsg[0];
        }

        public SystemFaultStateMsg(SystemFaultMsg[] faults, SystemFaultMsg[] historical_faults)
        {
            this.faults = faults;
            this.historical_faults = historical_faults;
        }

        public static SystemFaultStateMsg Deserialize(MessageDeserializer deserializer) => new SystemFaultStateMsg(deserializer);

        private SystemFaultStateMsg(MessageDeserializer deserializer)
        {
            deserializer.Read(out this.faults, SystemFaultMsg.Deserialize, deserializer.ReadLength());
            deserializer.Read(out this.historical_faults, SystemFaultMsg.Deserialize, deserializer.ReadLength());
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.WriteLength(this.faults);
            serializer.Write(this.faults);
            serializer.WriteLength(this.historical_faults);
            serializer.Write(this.historical_faults);
        }

        public override string ToString()
        {
            return "SystemFaultStateMsg: " +
            "\nfaults: " + System.String.Join(", ", faults.ToList()) +
            "\nhistorical_faults: " + System.String.Join(", ", historical_faults.ToList());
        }

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#else
        [UnityEngine.RuntimeInitializeOnLoadMethod]
#endif
        public static void Register()
        {
            MessageRegistry.Register(k_RosMessageName, Deserialize);
        }
    }
}
