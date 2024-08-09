//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.Spot
{
    [Serializable]
    public class SetVelocityRequest : Message
    {
        public const string k_RosMessageName = "spot_msgs/SetVelocity";
        public override string RosMessageName => k_RosMessageName;

        //  The api only takes into account x and y for linear velocity, and z for angular.
        //  Other values are ignored.
        public Geometry.TwistMsg velocity_limit;

        public SetVelocityRequest()
        {
            this.velocity_limit = new Geometry.TwistMsg();
        }

        public SetVelocityRequest(Geometry.TwistMsg velocity_limit)
        {
            this.velocity_limit = velocity_limit;
        }

        public static SetVelocityRequest Deserialize(MessageDeserializer deserializer) => new SetVelocityRequest(deserializer);

        private SetVelocityRequest(MessageDeserializer deserializer)
        {
            this.velocity_limit = Geometry.TwistMsg.Deserialize(deserializer);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.velocity_limit);
        }

        public override string ToString()
        {
            return "SetVelocityRequest: " +
            "\nvelocity_limit: " + velocity_limit.ToString();
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