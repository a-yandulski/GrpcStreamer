using System;

namespace GrpcStreamer.Client
{
    public class Ref<T>
    {
        public T Value { get; set; }

        public Ref() : this(default(T))
        {
            
        }

        public Ref(T value)
        {
            this.Value = value;
        }

        public static implicit operator T (Ref<T> value)
        {
            return value != null ?  value.Value : default(T);
        }

        public static implicit operator Ref<T>(T value)
        {
            return new Ref<T>(value);
        }
    }
}
