// Created on 16/02/2023 15:22 by shell

using System;
using System.Threading;
using Shelland.ImageServer.AppServices.Services.Abstract.Common;

namespace Shelland.ImageServer.AppServices.Services.Common;

public class IdGenerator : IIdGenerator
{
    public Guid Id()
    {
        return SequentialInternalGuid.Create();
    }

    private static class SequentialInternalGuid
    {
        static SequentialInternalGuid()
        {
            _counter = DateTime.UtcNow.Ticks;
        }

        private static long _counter;

        public static Guid Create()
        {
            return StdGenerator.Generate(GetTicks());
        }

        private static long GetTicks()
        {
            return Interlocked.Increment(ref _counter);
        }

        private static class StdGenerator
        {
            public static Guid Generate(long number)
            {
                Span<byte> guidSpan = stackalloc byte[16];
                var guid = Guid.NewGuid();

                if (!guid.TryWriteBytes(guidSpan))
                {
                    throw new InvalidOperationException("Guid.TryWriteBytes failed");
                }

                if (!BitConverter.TryWriteBytes(guidSpan[..8], number))
                {
                    throw new InvalidOperationException("BitConverter.TryWriteBytes(Span<byte>, long) failed");
                }

                (guidSpan[0], guidSpan[1], guidSpan[2], guidSpan[3], guidSpan[4], guidSpan[5], guidSpan[6], guidSpan[7], guidSpan[8]) =
                    (guidSpan[4], guidSpan[5], guidSpan[6], guidSpan[7], guidSpan[2], guidSpan[3],
                        (byte)((guidSpan[1] << 4) | (guidSpan[0] >> 4)), // #2
                        (byte)(0x40 | (guidSpan[1] >> 4)), // #1
                        (byte)(0x80 | ((guidSpan[0] & 0xf) << 2) | (guidSpan[8] & 0x3)) // #3
                    );

                return new Guid(guidSpan);
            }
        }
    }
}