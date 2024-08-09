using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;


namespace RosMessageTypes.Spot
{
    public class TrajectoryAction : Action<TrajectoryActionGoal, TrajectoryActionResult, TrajectoryActionFeedback, TrajectoryGoal, TrajectoryResult, TrajectoryFeedback>
    {
        public const string k_RosMessageName = "spot_msgs/TrajectoryAction";
        public override string RosMessageName => k_RosMessageName;


        public TrajectoryAction() : base()
        {
            this.action_goal = new TrajectoryActionGoal();
            this.action_result = new TrajectoryActionResult();
            this.action_feedback = new TrajectoryActionFeedback();
        }

        public static TrajectoryAction Deserialize(MessageDeserializer deserializer) => new TrajectoryAction(deserializer);

        TrajectoryAction(MessageDeserializer deserializer)
        {
            this.action_goal = TrajectoryActionGoal.Deserialize(deserializer);
            this.action_result = TrajectoryActionResult.Deserialize(deserializer);
            this.action_feedback = TrajectoryActionFeedback.Deserialize(deserializer);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.action_goal);
            serializer.Write(this.action_result);
            serializer.Write(this.action_feedback);
        }

    }
}
