namespace Mariolike
{
    public interface IUIViewScript
    {
        void show(System.Action callback = null);
        void hide(System.Action callback = null);
    }
}
