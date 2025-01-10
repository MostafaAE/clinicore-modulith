using System.Threading.Channels;
using CliniCore.Shared.Events;

namespace CliniCore.Shared.Messaging;

internal interface IEventChannel
{
    ChannelReader<IEvent> Reader { get; }
    ChannelWriter<IEvent> Writer { get; }
}