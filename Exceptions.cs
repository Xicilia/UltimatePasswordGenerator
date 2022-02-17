using System;

namespace UPG {
    class MaskLiteralNotFoundException : Exception {
        public MaskLiteralNotFoundException(string message) : base(message) {}
    }

    class IncorrectLengthException : Exception {
        public IncorrectLengthException() : base() {}
    }

    class InvalidPresetSyntaxException : Exception {
        public InvalidPresetSyntaxException() : base() {}
    }
}