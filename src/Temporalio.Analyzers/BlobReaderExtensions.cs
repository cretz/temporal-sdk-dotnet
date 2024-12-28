

using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace Temporalio.Analyzers
{
    internal static class BlobReaderExtensions
    {
        private static readonly Dictionary<ILOpCode, OpCode> opCodes = new();

        static BlobReaderExtensions()
        {
            foreach (var field in typeof(OpCodes).GetFields())
            {
                if (field.GetValue(null) is OpCode opCode)
                {
                    opCodes[(ILOpCode)opCode.Value] = opCode;
                }
            }
        }

        public static ILOpCode ReadOpCode(ref this BlobReader reader)
        {
            byte firstByte = reader.ReadByte();

            // Handle two-byte opcodes (0xFE prefix)
            if (firstByte == 0xFE)
            {
                byte secondByte = reader.ReadByte();
                return (ILOpCode)((firstByte << 8) | secondByte);
            }
            else
            {
                return (ILOpCode)firstByte;
            }
        }

        public static void SkipOperand(ref this BlobReader reader, ILOpCode ilOpCode)
        {
            var opCode = opCodes[ilOpCode];

            switch (opCode.OperandType)
            {
                case OperandType.InlineSwitch:
                    var count = reader.ReadUInt32();
                    reader.Offset += (int)(count * 4);
                    break;
                case OperandType.InlineI8:
                case OperandType.InlineR:
                    reader.Offset += 8;
                    break;
                case OperandType.InlineBrTarget:
                case OperandType.InlineField:
                case OperandType.InlineI:
                case OperandType.InlineMethod:
                case OperandType.InlineString:
                case OperandType.InlineTok:
                case OperandType.InlineType:
                case OperandType.ShortInlineR:
                case OperandType.InlineSig:
                    reader.Offset += 4;
                    break;
                // case OperandType.InlineArg:
                case OperandType.InlineVar:
                    reader.Offset += 2;
                    break;
                case OperandType.ShortInlineBrTarget:
                case OperandType.ShortInlineI:
                // case OperandType.ShortInlineArg:
                case OperandType.ShortInlineVar:
                    reader.Offset += 1;
                    break;
                case OperandType.InlineNone:
                    break;
                default:
                    throw new InvalidOperationException($"Unrecognized opcode: ${ilOpCode}");
            }
        }
    }
}