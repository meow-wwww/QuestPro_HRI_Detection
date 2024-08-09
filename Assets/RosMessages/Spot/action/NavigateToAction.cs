using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;


namespace RosMessageTypes.Spot
{
    public class NavigateToAction : Action<NavigateToActionGoal, NavigateToActionResult, NavigateToActionFeedback, NavigateToGoal, NavigateToResult, NavigateToFeedback>
    {
        public const string k_RosMessageName = "spot_msgs/NavigateToAction";
        public override string RosMessageName => k_RosMessageName;


        public NavigateToAction() : base()
        {
            this.action_goal = new NavigateToActionGoal();
            this.action_result = new NavigateToActionResult();
            this.action_feedback = new NavigateToActionFeedback();
        }

        public static NavigateToAction Deserialize(MessageDeserializer deserializer) => new NavigateToAction(deserializer);

        NavigateToAction(MessageDeserializer deserializer)
        {
            this.action_goal = NavigateToActionGoal.Deserialize(deserializer);
            this.action_result = NavigateToActionResult.Deserialize(deserializer);
            this.action_feedback = NavigateToActionFeedback.Deserialize(deserializer);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.action_goal);
            serializer.Write(this.action_result);
            serializer.Write(this.action_feedback);
        }

    }
}
