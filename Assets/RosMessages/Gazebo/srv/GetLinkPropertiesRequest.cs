//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.Gazebo
{
    [Serializable]
    public class GetLinkPropertiesRequest : Message
    {
        public const string k_RosMessageName = "gazebo_msgs/GetLinkProperties";
        public override string RosMessageName => k_RosMessageName;

        public string link_name;
        //  name of link
        //  link names are prefixed by model name, e.g. pr2::base_link

        public GetLinkPropertiesRequest()
        {
            this.link_name = "";
        }

        public GetLinkPropertiesRequest(string link_name)
        {
            this.link_name = link_name;
        }

        public static GetLinkPropertiesRequest Deserialize(MessageDeserializer deserializer) => new GetLinkPropertiesRequest(deserializer);

        private GetLinkPropertiesRequest(MessageDeserializer deserializer)
        {
            deserializer.Read(out this.link_name);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.link_name);
        }

        public override string ToString()
        {
            return "GetLinkPropertiesRequest: " +
            "\nlink_name: " + link_name.ToString();
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
