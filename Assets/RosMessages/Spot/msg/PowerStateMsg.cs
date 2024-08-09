//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using RosMessageTypes.Std;
using RosMessageTypes.BuiltinInterfaces;

namespace RosMessageTypes.Spot
{
    [Serializable]
    public class PowerStateMsg : Message
    {
        public const string k_RosMessageName = "spot_msgs/PowerState";
        public override string RosMessageName => k_RosMessageName;

        //  MotorPowerState
        public const byte STATE_UNKNOWN = 0;
        public const byte STATE_OFF = 1;
        public const byte STATE_ON = 2;
        public const byte STATE_POWERING_ON = 3;
        public const byte STATE_POWERING_OFF = 4;
        public const byte STATE_ERROR = 5;
        //  ShorePowerState
        public const byte STATE_UNKNOWN_SHORE_POWER = 0;
        public const byte STATE_ON_SHORE_POWER = 1;
        public const byte STATE_OFF_SHORE_POWER = 2;
        public HeaderMsg header;
        public byte motor_power_state;
        public byte shore_power_state;
        public double locomotion_charge_percentage;
        public DurationMsg locomotion_estimated_runtime;

        public PowerStateMsg()
        {
            this.header = new HeaderMsg();
            this.motor_power_state = 0;
            this.shore_power_state = 0;
            this.locomotion_charge_percentage = 0.0;
            this.locomotion_estimated_runtime = new DurationMsg();
        }

        public PowerStateMsg(HeaderMsg header, byte motor_power_state, byte shore_power_state, double locomotion_charge_percentage, DurationMsg locomotion_estimated_runtime)
        {
            this.header = header;
            this.motor_power_state = motor_power_state;
            this.shore_power_state = shore_power_state;
            this.locomotion_charge_percentage = locomotion_charge_percentage;
            this.locomotion_estimated_runtime = locomotion_estimated_runtime;
        }

        public static PowerStateMsg Deserialize(MessageDeserializer deserializer) => new PowerStateMsg(deserializer);

        private PowerStateMsg(MessageDeserializer deserializer)
        {
            this.header = HeaderMsg.Deserialize(deserializer);
            deserializer.Read(out this.motor_power_state);
            deserializer.Read(out this.shore_power_state);
            deserializer.Read(out this.locomotion_charge_percentage);
            this.locomotion_estimated_runtime = DurationMsg.Deserialize(deserializer);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.header);
            serializer.Write(this.motor_power_state);
            serializer.Write(this.shore_power_state);
            serializer.Write(this.locomotion_charge_percentage);
            serializer.Write(this.locomotion_estimated_runtime);
        }

        public override string ToString()
        {
            return "PowerStateMsg: " +
            "\nheader: " + header.ToString() +
            "\nmotor_power_state: " + motor_power_state.ToString() +
            "\nshore_power_state: " + shore_power_state.ToString() +
            "\nlocomotion_charge_percentage: " + locomotion_charge_percentage.ToString() +
            "\nlocomotion_estimated_runtime: " + locomotion_estimated_runtime.ToString();
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
