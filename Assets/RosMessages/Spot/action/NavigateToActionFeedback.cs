using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using RosMessageTypes.Std;
using RosMessageTypes.Actionlib;

namespace RosMessageTypes.Spot
{
    public class NavigateToActionFeedback : ActionFeedback<NavigateToFeedback>
    {
        public const string k_RosMessageName = "spot_msgs/NavigateToActionFeedback";
        public override string RosMessageName => k_RosMessageName;


        public NavigateToActionFeedback() : base()
        {
            this.feedback = new NavigateToFeedback();
        }

        public NavigateToActionFeedback(HeaderMsg header, GoalStatusMsg status, NavigateToFeedback feedback) : base(header, status)
        {
            this.feedback = feedback;
        }
        public static NavigateToActionFeedback Deserialize(MessageDeserializer deserializer) => new NavigateToActionFeedback(deserializer);

        NavigateToActionFeedback(MessageDeserializer deserializer) : base(deserializer)
        {
            this.feedback = NavigateToFeedback.Deserialize(deserializer);
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
