using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Lock
{
    class LockMatrix
    {
        /// <summary>
        /// LockModes
        /// 
        /// --------    Locked    |  WriteLock   |    ReadLock
        /// Locked        NO            YES             YES
        /// WriteLock     YES            NO             NO
        /// ReadLock      YES            NO             YES
        /// </summary>
        private static readonly bool[,] matrix = { {false, true, true} ,
                                                   {true,  false, false},
                                                   {true, false, true} };

        public static bool IsModeCompatible(LockMode mode1, LockMode mode2)
        {
            return matrix[GetMatrixIndex(mode1), GetMatrixIndex(mode2)];
        }

        private static int GetMatrixIndex(LockMode mode)
        {
            int idx = -1;
            switch (mode)
            {
                case LockMode.Locked: idx = 0; break;
                case LockMode.WriteLock: idx = 2; break;
                case LockMode.ReadLock: idx = 3; break;
                default: break;
            }
            return idx;
        }
    }
}
