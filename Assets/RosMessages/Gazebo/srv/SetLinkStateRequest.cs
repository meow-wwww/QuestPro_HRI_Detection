//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.Gazebo
{
    [Serializable]
    public class SetLinkStateRequest : Message
    {
        public const string k_RosMessageName = "gazebo_msgs/SetLinkState";
        public override string RosMessageName => k_RosMessageName;

        public LinkStateMsg link_state;

        public SetLinkStateRequest()
        {
            this.link_state = new LinkStateMsg();
        }

        public SetLinkStateRequest(LinkStateMsg link_state)
        {
            this.link_state = link_state;
        }

        public static SetLinkStateRequest Deserialize(MessageDeserializer deserializer) => new SetLinkStateRequest(deserializer);

        private SetLinkStateRequest(MessageDeserializer deserializer)
        {
            this.link_state = LinkStateMsg.Deserialize(deserializer);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.link_state);
        }

        public override string ToString()
        {
            return "SetLinkStateRequest: " +
            "\nlink_state: " + link_state.ToString();
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
