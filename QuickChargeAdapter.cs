using System;
using System.Device.Gpio;
using System.Threading;

namespace NanoFramework.QuickCharge
{
    public class QuickChargeAdapter
    {
        private readonly QcDataPin plusPin;
        private readonly QcDataPin minusPin;

        public QuickChargeAdapter(GpioController gpioController, int plusHight, int plusLow, int minusHight, int minusLow, float initializeVoltage = 5.0f)
        {
            CurrentVoltage = initializeVoltage;

            plusPin = new QcDataPin(gpioController, plusHight, plusLow);
            minusPin = new QcDataPin(gpioController, minusHight, minusLow);
        }

        public float CurrentVoltage { get; private set; }
        public bool IsEnabledContinuousMode { get; private set; } = false;

        public void InitializeConnection()
        {
            plusPin.SetVoltage06();
            Thread.Sleep(2500);

            minusPin.SetVoltage0();
            Thread.Sleep(3);
        }

        public void SetVoltage(int voltage)
        {
            switch (voltage)
            {
                case 5:
                    plusPin.SetVoltage06();
                    minusPin.SetVoltage0();
                    break;
                case 9:
                    plusPin.SetVoltage33();
                    minusPin.SetVoltage06();
                    break;
                case 12:
                    plusPin.SetVoltage06();
                    minusPin.SetVoltage06();
                    break;
                default:
                    SetAnyVoltage(voltage);
                    break;
            }

            CurrentVoltage = voltage;

            Thread.Sleep(60);
        }

        #region Continuous Mode
        public void SetAnyVoltage(float voltage)
        {
            if (!IsEnabledContinuousMode)
                EnableContinuousMode();

            bool isIncrementVoltage = voltage > CurrentVoltage;

            if (isIncrementVoltage)
                IncrementVoltageTo(voltage);
            else
                DecrementVoltageTo(voltage);
        }

        public void IncrementVoltageTo(float voltage)
        {
            var iterationCount = (voltage - CurrentVoltage) / 0.2f;

            for (int i = 0; i < iterationCount; i++)
            {
                IncrementVoltage();

                CurrentVoltage += 0.2f;
            }
        }

        public void DecrementVoltageTo(float voltage)
        {
            var iterationCount = (CurrentVoltage - voltage) / 0.2f;

            for (int i = 0; i < iterationCount; i++)
            {
                DecrementVoltage();

                CurrentVoltage -= 0.2f;
            }
        }

        public void IncrementVoltage()
        {
            plusPin.SetVoltage33();
            Thread.Sleep(1);
            plusPin.SetVoltage06();
            Thread.Sleep(1);
        }

        public void DecrementVoltage()
        {
            minusPin.SetVoltage06();
            Thread.Sleep(1);
            minusPin.SetVoltage33();
            Thread.Sleep(1);
        }

        public void EnableContinuousMode()
        {
            plusPin.SetVoltage06();
            minusPin.SetVoltage33();

            Thread.Sleep(60);

            IsEnabledContinuousMode = true;
        }
        #endregion
    }
}
