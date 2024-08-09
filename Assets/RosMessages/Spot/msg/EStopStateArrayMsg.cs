//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.Spot
{
    [Serializable]
    public class EStopStateArrayMsg : Message
    {
        public const string k_RosMessageName = "spot_msgs/EStopStateArray";
        public override string RosMessageName => k_RosMessageName;

        public EStopStateMsg[] estop_states;

        public EStopStateArrayMsg()
        {
            this.estop_states = new EStopStateMsg[0];
        }

        public EStopStateArrayMsg(EStopStateMsg[] estop_states)
        {
            this.estop_states = estop_states;
        }

        public static EStopStateArrayMsg Deserialize(MessageDeserializer deserializer) => new EStopStateArrayMsg(deserializer);

        private EStopStateArrayMsg(MessageDeserializer deserializer)
        {
            deserializer.Read(out this.estop_states, EStopStateMsg.Deserialize, deserializer.ReadLength());
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.WriteLength(this.estop_states);
            serializer.Write(this.estop_states);
        }

        public override string ToString()
        {
            return "EStopStateArrayMsg: " +
            "\nestop_states: " + System.String.Join(", ", estop_states.ToList());
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
