//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.Spot
{
    [Serializable]
    public class ClearBehaviorFaultResponse : Message
    {
        public const string k_RosMessageName = "spot_msgs/ClearBehaviorFault";
        public override string RosMessageName => k_RosMessageName;

        public bool success;
        public string message;

        public ClearBehaviorFaultResponse()
        {
            this.success = false;
            this.message = "";
        }

        public ClearBehaviorFaultResponse(bool success, string message)
        {
            this.success = success;
            this.message = message;
        }

        public static ClearBehaviorFaultResponse Deserialize(MessageDeserializer deserializer) => new ClearBehaviorFaultResponse(deserializer);

        private ClearBehaviorFaultResponse(MessageDeserializer deserializer)
        {
            deserializer.Read(out this.success);
            deserializer.Read(out this.message);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.success);
            serializer.Write(this.message);
        }

        public override string ToString()
        {
            return "ClearBehaviorFaultResponse: " +
            "\nsuccess: " + success.ToString() +
            "\nmessage: " + message.ToString();
        }

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#else
        [UnityEngine.RuntimeInitializeOnLoadMethod]
#endif
        public static void Register()
        {
            MessageRegistry.Register(k_RosMessageName, Deserialize, MessageSubtopic.Response);
        }
    }
}