//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.Gazebo
{
    [Serializable]
    public class SetModelStateResponse : Message
    {
        public const string k_RosMessageName = "gazebo_msgs/SetModelState";
        public override string RosMessageName => k_RosMessageName;

        public bool success;
        //  return true if setting state successful
        public string status_message;
        //  comments if available

        public SetModelStateResponse()
        {
            this.success = false;
            this.status_message = "";
        }

        public SetModelStateResponse(bool success, string status_message)
        {
            this.success = success;
            this.status_message = status_message;
        }

        public static SetModelStateResponse Deserialize(MessageDeserializer deserializer) => new SetModelStateResponse(deserializer);

        private SetModelStateResponse(MessageDeserializer deserializer)
        {
            deserializer.Read(out this.success);
            deserializer.Read(out this.status_message);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.success);
            serializer.Write(this.status_message);
        }

        public override string ToString()
        {
            return "SetModelStateResponse: " +
            "\nsuccess: " + success.ToString() +
            "\nstatus_message: " + status_message.ToString();
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
