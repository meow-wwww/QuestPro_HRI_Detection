using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using RosMessageTypes.Std;
using RosMessageTypes.Actionlib;

namespace RosMessageTypes.Spot
{
    public class TrajectoryActionResult : ActionResult<TrajectoryResult>
    {
        public const string k_RosMessageName = "spot_msgs/TrajectoryActionResult";
        public override string RosMessageName => k_RosMessageName;


        public TrajectoryActionResult() : base()
        {
            this.result = new TrajectoryResult();
        }

        public TrajectoryActionResult(HeaderMsg header, GoalStatusMsg status, TrajectoryResult result) : base(header, status)
        {
            this.result = result;
        }
        public static TrajectoryActionResult Deserialize(MessageDeserializer deserializer) => new TrajectoryActionResult(deserializer);

        TrajectoryActionResult(MessageDeserializer deserializer) : base(deserializer)
        {
            this.result = TrajectoryResult.Deserialize(deserializer);
        }
        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.header);
            serializer.Write(this.status);
            serializer.Write(this.result);
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
