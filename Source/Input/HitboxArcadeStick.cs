using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSlugsEngine.Input
{
    public class HitboxArcadeStick : GameController
    {
        internal HitboxArcadeStick(InputDevice device) : base(device) { }

        protected override void OnConnect()
        {
            
        }

        protected override void OnDisconnect()
        {
            
        }
        
        protected override void ProcessDeviceBytes(byte[] bytes)
        {
            bool up = false;
            bool left = false;
            bool down = false;
            bool right = false;

            //Least significant 4 bits on this byte contain the "stick" buttons
            switch (bytes[5] & 0b1111)
            {
                case 0:
                    up = true;
                    break;
                case 1:
                    up = true;
                    right = true;
                    break;
                case 2:
                    right = true;
                    break;
                case 3:
                    right = true;
                    down = true;
                    break;
                case 4:
                    down = true;
                    break;
                case 5:
                    down = true;
                    left = true;
                    break;
                case 6:
                    left = true;
                    break;
                case 7:
                    left = true;
                    up = true; break;
            }

            //Bytes 5-6 contain, along with the dpad, a bitmask of all but two of the buttons
            //Remaining 2 buttons are in the 7th byte
            ushort buttons = BitConverter.ToUInt16(bytes, 5);

            bool btn0 = (buttons & 16) != 0;
            bool btn1 = (buttons & 32) != 0;
            bool btn2 = (buttons & 64) != 0;
            bool btn3 = (buttons & 128) != 0;
            bool btn4 = (buttons & 256) != 0;
            bool btn5 = (buttons & 512) != 0;
            bool btn6 = (buttons & 1024) != 0;
            bool btn7 = (buttons & 2048) != 0;
            bool btn8 = (buttons & 4096) != 0;
            bool btn9 = (buttons & 8192) != 0;
            bool btn10 = (bytes[7] & 1) != 0;
            bool btn11 = (bytes[7] & 2) != 0;
        }

        protected override void UpdateController()
        {
            
        }
    }
}
