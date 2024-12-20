//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.Spot
{
    [Serializable]
    public class LeaseOwnerMsg : Message
    {
        public const string k_RosMessageName = "spot_msgs/LeaseOwner";
        public override string RosMessageName => k_RosMessageName;

        public string client_name;
        public string user_name;

        public LeaseOwnerMsg()
        {
            this.client_name = "";
            this.user_name = "";
        }

        public LeaseOwnerMsg(string client_name, string user_name)
        {
            this.client_name = client_name;
            this.user_name = user_name;
        }

        public static LeaseOwnerMsg Deserialize(MessageDeserializer deserializer) => new LeaseOwnerMsg(deserializer);

        private LeaseOwnerMsg(MessageDeserializer deserializer)
        {
            deserializer.Read(out this.client_name);
            deserializer.Read(out this.user_name);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.client_name);
            serializer.Write(this.user_name);
        }

        public override string ToString()
        {
            return "LeaseOwnerMsg: " +
            "\nclient_name: " + client_name.ToString() +
            "\nuser_name: " + user_name.ToString();
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
