using Microsoft.SPOT;
using System;
using Microsoft.SPOT.Hardware;
using System.Threading;
//using System.Threading;

namespace shooter
{
    public class PWMMotor
    {
        const int Period = 50000; //period between pulses

        const int Duration = 1500; //duration of pulse

        public uint MaxForward { get; set; }

        public uint MaxReverse { get; set; }

        public uint NeutralValue { get; set; } 

        PWM pwm = null;

        public PWMMotor(Cpu.PWMChannel channel)
        {
            WatchdogCheckTimer = new Timer(CheckWatchdog, this, 250, 250);
            MaxForward = 2000;
            MaxReverse = 1000;
            NeutralValue = 1500;
            Invert = false;
            pwm = new PWM(channel, Period, Duration, PWM.ScaleFactor.Microseconds, false);
            pwm.Start();
        }
        Timer WatchdogCheckTimer;
        public void SetSpeed(double SpeedPercent)
        {
            if (!CTRE.Phoenix.Watchdog.IsEnabled())
            {
                //Debug.Print("Watchdog is off, not enabling output");
                return;
            }
                if (SpeedPercent < -1)
            {
                Debug.Print("PWM Motor Speed is under -1");
                return; 
            }
            if (SpeedPercent > 1)
            {
                Debug.Print("PWM Motor Speed is over 1");
                return;
            }

            if (SpeedPercent == 0)
                pwm.Duration = NeutralValue;

            if (SpeedPercent > 0) //Forward
            {
                uint PowerSpread;
                if (!Invert)
                    PowerSpread = MaxForward - NeutralValue;
                else
                    PowerSpread = NeutralValue - MaxReverse;

                if (!Invert)
                    pwm.Duration = (uint)(PowerSpread * SpeedPercent) + NeutralValue;
                else
                    pwm.Duration = NeutralValue - (uint)(PowerSpread * SpeedPercent);
            }

            if (SpeedPercent < 0) //Reverse 
            {
                uint PowerSpread;
                if (Invert)
                    PowerSpread = MaxForward - NeutralValue;
                else
                    PowerSpread = NeutralValue - MaxReverse;

                if (Invert)
                    pwm.Duration = NeutralValue - (uint)(PowerSpread * SpeedPercent);// - NeutralValue;
                else
                    pwm.Duration = NeutralValue + (uint)(PowerSpread * SpeedPercent);
            }

            Debug.Print("PWMMotor set to " + pwm.Duration.ToString());
        }

        public static void CheckWatchdog(Object stateInfo)
        {
            if (!CTRE.Phoenix.Watchdog.IsEnabled())
            {
                Debug.Print("Watchdog is disabled! Stop motor");
                ((PWMMotor)stateInfo).pwm.Duration = ((PWMMotor)stateInfo).NeutralValue;
            }
        }

        public bool Invert { get; set; }
    }
}
