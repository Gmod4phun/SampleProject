using System;

public class ResourceUnsafeToDeleteException : Exception {
    public ResourceUnsafeToDeleteException(string message) : base(message) { }
}

public class ResourceNotFoundException : Exception {
    public ResourceNotFoundException(string message) : base(message) { }
}

public class ResourceAlreadyExistsException : Exception {
    public ResourceAlreadyExistsException(string message) : base(message) { }
}
