using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace ServiceStack.Messaging;

internal delegate IMessage MessageFactoryDelegate(object body);

public static class MessageFactory
{
    static readonly Dictionary<Type, MessageFactoryDelegate> CacheFn
        = new Dictionary<Type, MessageFactoryDelegate>();

    public static IMessage Create(object response)
    {
        if (response is IMessage responseMessage)
            return responseMessage;

        if (response == null) return null;
        var type = response.GetType();

        MessageFactoryDelegate factoryFn;
        lock (CacheFn) CacheFn.TryGetValue(type, out factoryFn);

        if (factoryFn != null)
            return factoryFn(response);

        var genericMessageType = typeof(Message<>).MakeGenericType(type);
        var mi = genericMessageType.GetMethod("Create",
            BindingFlags.Public | BindingFlags.Static);
        factoryFn = (MessageFactoryDelegate)Delegate.CreateDelegate(
            typeof(MessageFactoryDelegate), mi);

        lock (CacheFn) CacheFn[type] = factoryFn;

        return factoryFn(response);
    }
}

public class Message : IMessage
{
    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public long Priority { get; set; }
    public int RetryAttempts { get; set; }
    public Guid? ReplyId { get; set; }
    public string ReplyTo { get; set; }
    public int Options { get; set; }
    public ResponseStatus Error { get; set; }
    public string Tag { get; set; }
    public Dictionary<string, string> Meta { get; set; }
    public object Body { get; set; }
    public string TraceId { get; set; }

    public static Message<T> Create<T>(T request) => new(request);
}

/// <summary>
/// Basic implementation of IMessage[T]
/// </summary>
/// <typeparam name="T"></typeparam>
public class Message<T>
    : Message, IMessage<T>
{
    public Message()
    {
        this.Id = Guid.NewGuid();
        this.CreatedDate = DateTime.UtcNow;
        this.Options = (int)MessageOption.NotifyOneWay;
    }

    public Message(T body)
        : this()
    {
        Body = body;
    }

    public static IMessage Create(object oBody)
    {
        return new Message<T>((T)oBody);
    }

    public T GetBody()
    {
        return (T)Body;
    }

    public override string ToString()
    {
        return $"CreatedDate={this.CreatedDate}, Id={this.Id:N}, Type={typeof(T).Name}, Retry={this.RetryAttempts}";
    }

}