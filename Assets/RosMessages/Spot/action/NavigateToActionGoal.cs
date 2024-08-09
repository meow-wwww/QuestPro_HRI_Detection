using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using RosMessageTypes.Std;
using RosMessageTypes.Actionlib;

namespace RosMessageTypes.Spot
{
    public class NavigateToActionGoal : ActionGoal<NavigateToGoal>
    {
        public const string k_RosMessageName = "spot_msgs/NavigateToActionGoal";
        public override string RosMessageName => k_RosMessageName;


        public NavigateToActionGoal() : base()
        {
            this.goal = new NavigateToGoal();
        }

        public NavigateToActionGoal(HeaderMsg header, GoalIDMsg goal_id, NavigateToGoal goal) : base(header, goal_id)
        {
            this.goal = goal;
        }
        public static NavigateToActionGoal Deserialize(MessageDeserializer deserializer) => new NavigateToActionGoal(deserializer);

        NavigateToActionGoal(MessageDeserializer deserializer) : base(deserializer)
        {
            this.goal = NavigateToGoal.Deserialize(deserializer);
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
