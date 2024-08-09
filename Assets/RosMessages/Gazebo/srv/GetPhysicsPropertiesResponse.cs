//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.Gazebo
{
    [Serializable]
    public class GetPhysicsPropertiesResponse : Message
    {
        public const string k_RosMessageName = "gazebo_msgs/GetPhysicsProperties";
        public override string RosMessageName => k_RosMessageName;

        //  sets pose and twist of a link.  All children link poses/twists of the URDF tree will be updated accordingly
        public double time_step;
        //  dt in seconds
        public bool pause;
        //  true if physics engine is paused
        public double max_update_rate;
        //  throttle maximum physics update rate
        public Geometry.Vector3Msg gravity;
        //  gravity vector (e.g. earth ~[0,0,-9.81])
        public ODEPhysicsMsg ode_config;
        //  contains physics configurations pertaining to ODE
        public bool success;
        //  return true if set wrench successful
        public string status_message;
        //  comments if available

        public GetPhysicsPropertiesResponse()
        {
            this.time_step = 0.0;
            this.pause = false;
            this.max_update_rate = 0.0;
            this.gravity = new Geometry.Vector3Msg();
            this.ode_config = new ODEPhysicsMsg();
            this.success = false;
            this.status_message = "";
        }

        public GetPhysicsPropertiesResponse(double time_step, bool pause, double max_update_rate, Geometry.Vector3Msg gravity, ODEPhysicsMsg ode_config, bool success, string status_message)
        {
            this.time_step = time_step;
            this.pause = pause;
            this.max_update_rate = max_update_rate;
            this.gravity = gravity;
            this.ode_config = ode_config;
            this.success = success;
            this.status_message = status_message;
        }

        public static GetPhysicsPropertiesResponse Deserialize(MessageDeserializer deserializer) => new GetPhysicsPropertiesResponse(deserializer);

        private GetPhysicsPropertiesResponse(MessageDeserializer deserializer)
        {
            deserializer.Read(out this.time_step);
            deserializer.Read(out this.pause);
            deserializer.Read(out this.max_update_rate);
            this.gravity = Geometry.Vector3Msg.Deserialize(deserializer);
            this.ode_config = ODEPhysicsMsg.Deserialize(deserializer);
            deserializer.Read(out this.success);
            deserializer.Read(out this.status_message);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.time_step);
            serializer.Write(this.pause);
            serializer.Write(this.max_update_rate);
            serializer.Write(this.gravity);
            serializer.Write(this.ode_config);
            serializer.Write(this.success);
            serializer.Write(this.status_message);
        }

        public override string ToString()
        {
            return "GetPhysicsPropertiesResponse: " +
            "\ntime_step: " + time_step.ToString() +
            "\npause: " + pause.ToString() +
            "\nmax_update_rate: " + max_update_rate.ToString() +
            "\ngravity: " + gravity.ToString() +
            "\node_config: " + ode_config.ToString() +
            "\nsuccess: " + success.ToString() +
            "\nstatus_message: " + status_message.ToString();
        }

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#else
        [UnityEngine.RuntimeInitializeOnLoadMethod]
#endif
        public static void Register()
        {
            MessageRegistry.Register(k_RosMessageName, Deserialize, MessageSubtopic.Response);
        }
    }
}
