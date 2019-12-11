using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;

namespace AdventOfCode2019.Utils
{
    public class IntCodeMachine
    {
        public long[] Memory { get; }
        public List<long> Outputs { get; } = new List<long>();
        private List<long> Inputs { get; set; } = new List<long>();
        private long PC { get; set; }
        private long SP { get; set; }
        public IntCodeStatus Status { get; private set; } = IntCodeStatus.Running;

        public void Add(long arg0, long arg1, long output)
        {
            Memory[output] = arg0 + arg1;
        }

        public void Multiply(long arg0, long arg1, long output)
        {
            Memory[output] = arg0 * arg1;
        }

        public void Terminate()
        {
            Status = IntCodeStatus.Stopped;
        }

        public void Input(long output)
        {
            if (Inputs.Count == 0)
            {
                PC -= 2;
                Status = IntCodeStatus.WaitingForInput;
                return;
            }
            Memory[output] = Inputs.First();
            Inputs = Inputs.Skip(1).ToList();
        }

        public void JumpIfTrue(long condition, long pc)
        {
            if (condition == 0) return;
            PC = pc;
        }

        public void JumpIfFalse(long condition, long pc)
        {
            if (condition != 0) return;
            PC = pc;
        }

        public void LessThan(long arg0, long arg1, long output)
        {
            Memory[output] = arg0 < arg1 ? 1 : 0;
        }

        public void Equals(long arg0, long arg1, long output)
        {
            Memory[output] = arg0 == arg1 ? 1 : 0;
        }

        private void Output(long arg0)
        {
            Outputs.Add(arg0);
        }

        public IntCodeMachine(IEnumerable<long> memory)
        {
            Memory = memory.Concat(Enumerable.Repeat(0L,10000)).ToArray();
        }

        public IntCodeStatus Run(params long[] inputs)
        {
            Status.Should().NotBe(IntCodeStatus.Stopped);
            (Status == IntCodeStatus.WaitingForInput && inputs.Length == 0).Should().BeFalse();
            Status = IntCodeStatus.Running;
            Inputs.Should().BeEmpty();
            Inputs = inputs.ToList();
            RunInternal();
            return Status;
        }

        public static IntCodeMachine RunUntilStopped(IEnumerable<long> memory, params long[] inputs)
        {
            var machine = new IntCodeMachine(memory);
            machine.Run(inputs);
            machine.Status.Should().Be(IntCodeStatus.Stopped);
            return machine;
        }

        private void RunInternal()
        {
            while (Status == IntCodeStatus.Running)
            {
                ProcessNextInstruction();
            }
        }

        private long ConsumeParameter(ParameterMode mode, bool isOutput)
        {
            var rawParam = Memory[PC++];
            if (isOutput)
            {
                return mode switch
                {
                    ParameterMode.Position => EnsureEnoughMemory(rawParam),
                    ParameterMode.Relative => EnsureEnoughMemory(SP + rawParam),
                    _ => throw new ApplicationException()
                };
            }

            return mode switch
            {
                ParameterMode.Position => ReadFromMemory(rawParam),
                ParameterMode.Literal => rawParam,
                ParameterMode.Relative => ReadFromMemory(SP + rawParam),
                _ => throw new ApplicationException()
            };
        }

        private long ReadFromMemory(long address)
        {
            return Memory[EnsureEnoughMemory(address)];
        }

        private int EnsureEnoughMemory(long address)
        {
            address.Should().BeLessOrEqualTo(int.MaxValue);
            Memory.Length.Should().BeGreaterOrEqualTo((int)address);
            return (int) address;
        }

        private void ProcessNextInstruction()
        {
            var rawInstruction = Memory[PC++];
            var opcode = rawInstruction % 100;
            var modes = (rawInstruction / 100).Decimate()
                .Concat(new long[] {0, 0, 0})
                .Take(3)
                .Select(it => it switch
                {
                    0 => ParameterMode.Position,
                    1 => ParameterMode.Literal,
                    2 => ParameterMode.Relative,
                    _ => throw new ApplicationException()
                })
                .ToArray();

            switch (opcode)
            {
                case 1: // Add A + B => C
                    Add(ConsumeParameter(modes[0], false), 
                        ConsumeParameter(modes[1], false),
                        ConsumeParameter(modes[2], true));
                    break;
                case 2:
                    Multiply(ConsumeParameter(modes[0], false),
                        ConsumeParameter(modes[1], false),
                        ConsumeParameter(modes[2], true));
                    break;
                case 3:
                    Input(ConsumeParameter(modes[0], true));
                    break;
                case 4:
                    Output(ConsumeParameter(modes[0], false));
                    break;
                case 5: // jump if true
                    JumpIfTrue(ConsumeParameter(modes[0], false),
                        ConsumeParameter(modes[1], false));
                    break;
                case 6: // jump if false
                    JumpIfFalse(ConsumeParameter(modes[0], false),
                        ConsumeParameter(modes[1], false));
                    break;
                case 7: // LessThan
                    LessThan(ConsumeParameter(modes[0], false),
                        ConsumeParameter(modes[1], false),
                        ConsumeParameter(modes[2], true));
                    break;
                case 8: // Equals
                    Equals(ConsumeParameter(modes[0], false),
                        ConsumeParameter(modes[1], false),
                        ConsumeParameter(modes[2], true));
                    break;
                case 9: // Adjust the SP
                    SP += ConsumeParameter(modes[0], false);
                    break;
                case 99:
                    Terminate();
                    break;
                default:
                    throw new ApplicationException();
            }
        }

        public IntCodeMachine Drive(Func<long> inputHandler, Action<List<long>> outputHandler)
        {
            Run();
            while (Status != IntCodeStatus.Stopped)
            {
                if (Outputs.Any())
                {
                    outputHandler(Outputs);
                    Outputs.Clear();
                }
                
                Run(inputHandler());
            }

            if (Outputs.Any())
            {
                outputHandler(Outputs);
                Outputs.Clear();
            }

            return this;
        }
    }

    public enum ParameterMode
    {
        Position,
        Literal,
        Relative
    }

    public enum IntCodeStatus
    {
        Running,
        WaitingForInput,
        Stopped
    }
}