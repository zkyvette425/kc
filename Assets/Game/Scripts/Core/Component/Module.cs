namespace KC
{
    public abstract class Module : Component
    {
        protected Module()
        {
            Id = IdGenerator.Instance.GenerateId();
        }
    }
}