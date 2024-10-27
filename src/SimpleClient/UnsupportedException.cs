
[Serializable]
internal class UnsupportedException : Exception {
    public UnsupportedException() {
    }

    public UnsupportedException(String? message) : base(message) {
    }

    public UnsupportedException(String? message, Exception? innerException) : base(message, innerException) {
    }
}