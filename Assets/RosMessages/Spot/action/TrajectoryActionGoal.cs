using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using RosMessageTypes.Std;
using RosMessageTypes.Actionlib;

namespace RosMessageTypes.Spot
{
    public class TrajectoryActionGoal : ActionGoal<TrajectoryGoal>
    {
        public const string k_RosMessageName = "spot_msgs/TrajectoryActionGoal";
        public override string RosMessageName => k_RosMessageName;


        public TrajectoryActionGoal() : base()
        {
            this.goal = new TrajectoryGoal();
        }

        public TrajectoryActionGoal(HeaderMsg header, GoalIDMsg goal_id, TrajectoryGoal goal) : base(header, goal_id)
        {
            this.goal = goal;
        }
        public static TrajectoryActionGoal Deserialize(MessageDeserializer deserializer) => new TrajectoryActionGoal(deserializer);

        TrajectoryActionGoal(MessageDeserializer deserializer) : base(deserializer)
        {
            this.goal = TrajectoryGoal.Deserialize(deserializer);
        }
        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.header);
            serializer.Write(this.goal_id);
            serializer.Write(this.goal);
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
