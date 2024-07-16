//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.PepperMoveitConfig
{
    [Serializable]
    public class PepperArmMoveitJointsMsg : Message
    {
        public const string k_RosMessageName = "pepper_moveit_config/PepperArmMoveitJoints";
        public override string RosMessageName => k_RosMessageName;

        public double[] joints;

        public PepperArmMoveitJointsMsg()
        {
            this.joints = new double[5];
        }

        public PepperArmMoveitJointsMsg(double[] joints)
        {
            this.joints = joints;
        }

        public static PepperArmMoveitJointsMsg Deserialize(MessageDeserializer deserializer) => new PepperArmMoveitJointsMsg(deserializer);

        private PepperArmMoveitJointsMsg(MessageDeserializer deserializer)
        {
            deserializer.Read(out this.joints, sizeof(double), 5);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.joints);
        }

        public override string ToString()
        {
            return "PepperArmMoveitJointsMsg: " +
            "\njoints: " + System.String.Join(", ", joints.ToList());
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
