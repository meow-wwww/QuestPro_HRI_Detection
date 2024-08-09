using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using RosMessageTypes.Std;
using RosMessageTypes.Actionlib;

namespace RosMessageTypes.Spot
{
    public class NavigateToActionResult : ActionResult<NavigateToResult>
    {
        public const string k_RosMessageName = "spot_msgs/NavigateToActionResult";
        public override string RosMessageName => k_RosMessageName;


        public NavigateToActionResult() : base()
        {
            this.result = new NavigateToResult();
        }

        public NavigateToActionResult(HeaderMsg header, GoalStatusMsg status, NavigateToResult result) : base(header, status)
        {
            this.result = result;
        }
        public static NavigateToActionResult Deserialize(MessageDeserializer deserializer) => new NavigateToActionResult(deserializer);

        NavigateToActionResult(MessageDeserializer deserializer) : base(deserializer)
        {
            this.result = NavigateToResult.Deserialize(deserializer);
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
