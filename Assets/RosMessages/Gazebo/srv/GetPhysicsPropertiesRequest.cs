//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.Gazebo
{
    [Serializable]
    public class GetPhysicsPropertiesRequest : Message
    {
        public const string k_RosMessageName = "gazebo_msgs/GetPhysicsProperties";
        public override string RosMessageName => k_RosMessageName;


        public GetPhysicsPropertiesRequest()
        {
        }
        public static GetPhysicsPropertiesRequest Deserialize(MessageDeserializer deserializer) => new GetPhysicsPropertiesRequest(deserializer);

        private GetPhysicsPropertiesRequest(MessageDeserializer deserializer)
        {
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
        }

        public override string ToString()
        {
            return "GetPhysicsPropertiesRequest: ";
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
