
namespace IA.Interface
{
    public interface IState
    {
        public void EnterState();
        public void UpdateState();
        public void FixedUpdateState();
    }
}