//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.Spot
{
    [Serializable]
    public class TrajectoryFeedback : Message
    {
        public const string k_RosMessageName = "spot_msgs/Trajectory";
        public override string RosMessageName => k_RosMessageName;

        public string feedback;

        public TrajectoryFeedback()
        {
            this.feedback = "";
        }

        public TrajectoryFeedback(string feedback)
        {
            this.feedback = feedback;
        }

        public static TrajectoryFeedback Deserialize(MessageDeserializer deserializer) => new TrajectoryFeedback(deserializer);

        private TrajectoryFeedback(MessageDeserializer deserializer)
        {
            deserializer.Read(out this.feedback);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.feedback);
        }

        public override string ToString()
        {
            return "TrajectoryFeedback: " +
            "\nfeedback: " + feedback.ToString();
        }

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#else
        [UnityEngine.RuntimeInitializeOnLoadMethod]
#endif
        public static void Register()
        {
            MessageRegistry.Register(k_RosMessageName, Deserialize, MessageSubtopic.Feedback);
        }
    }
}
