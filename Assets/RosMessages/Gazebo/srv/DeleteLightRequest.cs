//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.Gazebo
{
    [Serializable]
    public class DeleteLightRequest : Message
    {
        public const string k_RosMessageName = "gazebo_msgs/DeleteLight";
        public override string RosMessageName => k_RosMessageName;

        public string light_name;
        //  name of the light to be deleted

        public DeleteLightRequest()
        {
            this.light_name = "";
        }

        public DeleteLightRequest(string light_name)
        {
            this.light_name = light_name;
        }

        public static DeleteLightRequest Deserialize(MessageDeserializer deserializer) => new DeleteLightRequest(deserializer);

        private DeleteLightRequest(MessageDeserializer deserializer)
        {
            deserializer.Read(out this.light_name);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.light_name);
        }

        public override string ToString()
        {
            return "DeleteLightRequest: " +
            "\nlight_name: " + light_name.ToString();
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
