using System.Device.Gpio;

namespace NanoFramework.QuickCharge
{
    public class QcDataPin
    {
        public GpioPin HightPin;
        public GpioPin LowPin;

        public QcDataPin(GpioController controller, int hightPin, int lowPin)
        {
            HightPin = controller.OpenPin(hightPin, PinMode.Output);
            LowPin = controller.OpenPin(lowPin, PinMode.Output);
        }

        public void SetVoltage0()
        {
            HightPin.Write(PinValue.Low);
            LowPin.Write(PinValue.Low);
        }

        public void SetVoltage33()
        {
            HightPin.Write(PinValue.High);
            LowPin.Write(PinValue.High);
        }

        public void SetVoltage06()
        {
            HightPin.Write(PinValue.Low);
            LowPin.Write(PinValue.High);
        }
    }
}
