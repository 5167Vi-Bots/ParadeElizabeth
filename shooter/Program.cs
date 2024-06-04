using CTRE.Phoenix;
using Microsoft.SPOT;

namespace shooter
{
    public class Program
    {
        public static void Main()
        {

            //Configuration
            const int PCMCANChannel = 6;
            const int PCMShooterSolenoidChannel = 1;
            const int PCMHornChannel = 0;
            const int PCMLightChannel = 2;

            const float ShooterAngleSpeed = .25f;


            //Initialization
            //GameController gamePad = new GameController(UsbHostDevice.GetInstance(0));
            PneumaticControlModule pcm = new PneumaticControlModule(PCMCANChannel);
            PWMMotor Motor1 = new PWMMotor(CTRE.HERO.IO.Port3.PWM_Pin4);
            PWMMotor Motor2 = new PWMMotor(CTRE.HERO.IO.Port3.PWM_Pin6);
            PWMMotor Motor3 = new PWMMotor(CTRE.HERO.IO.Port3.PWM_Pin7);
            PWMMotor Motor4 = new PWMMotor(CTRE.HERO.IO.Port3.PWM_Pin8);
            UsbHostDevice.GetInstance(0).SetSelectableXInputFilter(UsbHostDevice.SelectableXInputFilter.XInputDevices);
            PCMSwitch PCMHornSwitch = new PCMSwitch(pcm, PCMHornChannel);
            PCMSwitch LightSwitch = new PCMSwitch(pcm, PCMLightChannel);
            PCMSolenoid ShooterSolenoid = new PCMSolenoid(pcm, PCMShooterSolenoidChannel);
            Controller gamepad = new Controller();
            ControllerWatchdog controllerWatchdog = new ControllerWatchdog(gamepad, 1000); //Treat controller as disconnected after 2 seconds of the same input
            QuadMotorTankChassis robotChassis = new QuadMotorTankChassis(Motor1, Motor2,Motor3, Motor4);


            

            bool Runone = true;
            Motor2.Invert = true;
            Motor2.MaxForward = 1666; //33% power
            Motor2.MaxReverse = 1334;
            Motor1.MaxForward = 1666;
            Motor1.MaxReverse = 1334;


            /* loop forever */
            while (true)
            {
                if (gamepad.IsConnected)
                {
                    CTRE.Phoenix.Watchdog.Feed();
                }


                if (gamepad.B)
                {
                    PCMHornSwitch.TurnOn();
                } else
                {
                    PCMHornSwitch.TurnOff();
                }


                float ForwardReverseSpeed;
                if (gamepad.RightTriggerValue > 0)
                {
                    ForwardReverseSpeed = gamepad.RightTriggerValue;
                }
                else if (gamepad.LeftTriggerValue > 0)
                {
                    ForwardReverseSpeed = gamepad.LeftTriggerValue * -1;
                }
                else
                {
                    ForwardReverseSpeed = 0;
                }

                if (ForwardReverseSpeed != 0)
                {
                    robotChassis.SetSpeed(ForwardReverseSpeed, gamepad.LeftThumbStickHorizontalAxisValue);
                }

                if (ForwardReverseSpeed == 0 && gamepad.RightThumbStickHorizontalAxisValue != 0)
                {
                    //rotate
                    robotChassis.Rotate(gamepad.RightThumbStickHorizontalAxisValue);
                }
                else if (ForwardReverseSpeed == 0)
                {
                    robotChassis.SetSpeed(0, 0);
                }


                //Motor1.SetSpeed(gamepad.LeftThumbStickVerticalAxisValue);
                //Motor2.SetSpeed(gamepad.RightThumbStickVerticalAxisValue);

                //Motor1.SetSpeed(((gamepad.RightTriggerValue + 1)/2));

                /*
                if (buttonarray[2])
                {
                    buttonPressed = true;
                    shooty.SetSolenoidOutput(0, true);
                }
                else
                {
                    buttonPressed = false;
                    shooty.SetSolenoidOutput(0, false);
                }

                /* let button1 control the explicit PWM pin duration*/
                /*if (gamePad.GetButton(1) == true)
                {
                    pwm_6.Duration = 2000; / 2.0ms /  //Forward is 2000
                    pwm_4.Duration = 2000; /* 2.0ms /
                }
                else
                {
                    pwm_6.Duration = 1500; /* 1.0ms / //Stop is 1500
                    pwm_4.Duration = 1500; /* 1.0ms / //Reverse is 1000
                }

                */























                /*foreach (bool item in buttonarray)
                {


                    if (item)
                    {
                        buttonPressed = true;
                        sB.Append("Button Pressed");
                    }

                }
                */
                //Debug.Print(sB.ToString());
                //Debug.Print(buttonPressed.ToString());

                /*
                if (firstrun)
                {
                    shooty.SetSolenoidOutput(0, true);
                    firstrun = false;
                }
                shooty.SetSolenoidOutput(0, buttonPressed);
                    //shooty.SetSolenoidOutput(0, true);
                    shooty.SetSolenoidOutput(1, buttonPressed);
                    shooty.SetSolenoidOutput(2, buttonPressed);
                   shooty.SetSolenoidOutput(3, buttonPressed);*/
            }
        }
    }
}
