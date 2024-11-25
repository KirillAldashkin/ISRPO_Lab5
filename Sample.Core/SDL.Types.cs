using System.Runtime.InteropServices;

namespace Sample.Core;

public static partial class SDL
{
    public struct Window;

    public struct Renderer;

    [StructLayout(LayoutKind.Explicit, Size = 56)]
    public struct Event
    {
        [FieldOffset(0)] public EventType Type;
        [FieldOffset(4)] public uint TimeStamp;
        // [FieldOffset(8)] public DisplayEvent Display;
        // [FieldOffset(8)] public WindowEvent Window;
        [FieldOffset(8)] public KeyboardEvent Key;
        // [FieldOffset(8)] public TextEditingEvent Edit;
        // [FieldOffset(8)] public TextEditingExtEvent EditExt;
        // [FieldOffset(8)] public TextInputEvent Text;
        // [FieldOffset(8)] public MouseMotionEvent Motion;
        // [FieldOffset(8)] public MouseButtonEvent Button;
        [FieldOffset(8)] public MouseWheelEvent Wheel;
        // [FieldOffset(8)] public JoyAxisEvent JAxis;
        // [FieldOffset(8)] public JoyBallEvent JBall;
        // [FieldOffset(8)] public JoyHatEvent JHat;
        // [FieldOffset(8)] public JoyButtonEvent JButton;
        // [FieldOffset(8)] public JoyDeviceEvent JDevice;
        // [FieldOffset(8)] public JoyBatteryEvent JBattery;
        // [FieldOffset(8)] public ControllerAxisEvent CAxis;
        // [FieldOffset(8)] public ControllerButtonEvent CButton;
        // [FieldOffset(8)] public ControllerDeviceEvent CDevice;
        // [FieldOffset(8)] public ControllerTouchpadEvent CTouchPad;
        // [FieldOffset(8)] public ControllerSensorEvent CSensor;
        // [FieldOffset(8)] public AudioDeviceEvent ADevice;
        // [FieldOffset(8)] public SensorEvent Sensor;
        // [FieldOffset(8)] public QuitEvent Quit;
        // [FieldOffset(8)] public UserEvent User;
        // [FieldOffset(8)] public SysWMEvent SysWM;
        [FieldOffset(8)] public TouchFingerEvent TFinger;
        // [FieldOffset(8)] public MultiGestureEvent MGesture;
        // [FieldOffset(8)] public DollarGestureEvent DGesture;
        // [FieldOffset(8)] public DropEvent Drop;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct KeyboardEvent
    {
        public uint WindowID;
        public PressState State;
        public bool Repeat;
        private byte _padding2;
        private byte _padding3;
        public Keysym KeySym;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TouchFingerEvent
    {
        public TouchID TouchID;
        public FingerID FingerID;
        public float X;
        public float Y;
        public float DX;
        public float DY;
        public float Pressure;
        public uint WindowID;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MouseWheelEvent
    {
        public uint WindowID;
        public uint Which;
        public int X;
        public int Y;
        public MouseWheelDirection Direction;
        public float PreciseX;
        public float PreciseY;
        public int MouseX;
        public int MouseY;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public readonly struct TouchID
    {
        public readonly long Value;
 
        public static bool operator==(TouchID l, TouchID r) => l.Value == r.Value;
        public static bool operator!=(TouchID l, TouchID r) => l.Value != r.Value;

        public override bool Equals(object? obj) => obj is TouchID t && t == this;

        public override int GetHashCode() => Value.GetHashCode();
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FingerID
    {
        public readonly long Value;

        public static bool operator ==(FingerID l, FingerID r) => l.Value == r.Value;
        public static bool operator !=(FingerID l, FingerID r) => l.Value != r.Value;

        public override bool Equals(object? obj) => obj is FingerID t && t == this;

        public override int GetHashCode() => Value.GetHashCode();
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Keysym
    {
        public Scancode Scancode;
        public KeyCode Sym;
        public Keymod Mod;
        private uint unused;
    }
}
