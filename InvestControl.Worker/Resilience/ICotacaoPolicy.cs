using Polly;

namespace InvestControl.Consumer.Resilience
{
    public interface ICotacaoPolicy
    {
        IAsyncPolicy PolicyExecucao { get; }
    }
}
