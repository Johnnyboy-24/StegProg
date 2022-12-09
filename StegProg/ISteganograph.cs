using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StegProg
{
    public interface ISteganograph
    {
        Bitmap Hide(Bitmap container, BitArray secret);
        void Unvail(Bitmap image);
    }
}
