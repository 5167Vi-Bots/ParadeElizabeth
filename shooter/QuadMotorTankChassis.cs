using Microsoft.SPOT;
using System;

namespace shooter
{
    class QuadMotorTankChassis : TankChassis
    {
        PWMMotor LeftForward;
        PWMMotor LeftReverse; 
        PWMMotor RightForward;
        PWMMotor RightReverse;
        public QuadMotorTankChassis(PWMMotor leftForward, PWMMotor leftReverse, PWMMotor rightForward, PWMMotor rightReverse)
        {
            LeftForward = leftForward;
            LeftReverse = leftReverse;
            RightForward = rightForward;
            RightReverse = rightReverse;
        }

        public void SetSpeed(double ForwardReverse, double LeftRight)
        {
            LeftForward.SetSpeed(GetLeftSpeed(ForwardReverse, LeftRight));
            LeftReverse.SetSpeed(GetLeftSpeed(ForwardReverse, LeftRight));
            RightForward.SetSpeed(GetRightSpeed(ForwardReverse, LeftRight));
            RightReverse.SetSpeed(GetRightSpeed(ForwardReverse, LeftRight));
        }
        public void Rotate(double DirectionSpeed)
        {
            if (DirectionSpeed > 0)
            {
                RightForward.SetSpeed(DirectionSpeed);
                RightReverse.SetSpeed(DirectionSpeed);
                LeftForward.SetSpeed(DirectionSpeed * -1);
                LeftReverse.SetSpeed(DirectionSpeed * -1);
            }
            else
            {
                RightForward.SetSpeed(DirectionSpeed);
                RightReverse.SetSpeed(DirectionSpeed);
                LeftForward.SetSpeed(DirectionSpeed * -1);
                LeftReverse.SetSpeed(DirectionSpeed * -1);
            }
        }
    }
}
