using DoNotModify;

namespace HyperionTeam
{
    public class BulletViewWrapper
    {
        public BulletView BulletView { get; private set; } = null;

        public BulletViewWrapper()
        {
            
        }
        
        public BulletViewWrapper(BulletView bulletView)
        {
            BulletView = bulletView;
        }
        
        public static implicit operator BulletViewWrapper(BulletView value) { return new BulletViewWrapper { BulletView = value }; }

    }
}