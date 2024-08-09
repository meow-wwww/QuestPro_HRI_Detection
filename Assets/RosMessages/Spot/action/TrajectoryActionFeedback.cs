using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using RosMessageTypes.Std;
using RosMessageTypes.Actionlib;

namespace RosMessageTypes.Spot
{
    public class TrajectoryActionFeedback : ActionFeedback<TrajectoryFeedback>
    {
        public const string k_RosMessageName = "spot_msgs/TrajectoryActionFeedback";
        public override string RosMessageName => k_RosMessageName;


        public TrajectoryActionFeedback() : base()
        {
            this.feedback = new TrajectoryFeedback();
        }

        public TrajectoryActionFeedback(HeaderMsg header, GoalStatusMsg status, TrajectoryFeedback feedback) : base(header, status)
        {
            this.feedback = feedback;
        }
        public static TrajectoryActionFeedback Deserialize(MessageDeserializer deserializer) => new TrajectoryActionFeedback(deserializer);

        TrajectoryActionFeedback(MessageDeserializer deserializer) : base(deserializer)
        {
            this.feedback = TrajectoryFeedback.Deserialize(deserializer);
        }
        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.header);
            serializer.Write(this.status);
            serializer.Write(this.feedback);
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
