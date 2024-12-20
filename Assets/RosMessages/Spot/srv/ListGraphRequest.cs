//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.Spot
{
    [Serializable]
    public class ListGraphRequest : Message
    {
        public const string k_RosMessageName = "spot_msgs/ListGraph";
        public override string RosMessageName => k_RosMessageName;

        public string upload_filepath;

        public ListGraphRequest()
        {
            this.upload_filepath = "";
        }

        public ListGraphRequest(string upload_filepath)
        {
            this.upload_filepath = upload_filepath;
        }

        public static ListGraphRequest Deserialize(MessageDeserializer deserializer) => new ListGraphRequest(deserializer);

        private ListGraphRequest(MessageDeserializer deserializer)
        {
            deserializer.Read(out this.upload_filepath);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.upload_filepath);
        }

        public override string ToString()
        {
            return "ListGraphRequest: " +
            "\nupload_filepath: " + upload_filepath.ToString();
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
