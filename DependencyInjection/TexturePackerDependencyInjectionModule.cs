

using TexturePackerLoader;
using ZilloGames.OnyxEngine.Core.DependencyInjection.Interface;

namespace ZilloGames.OnyxEngine.Module.Rendering.DependencyInjection
{
    public class TexturePackerDependencyInjectionModule : IDependencyInjectionModule
    {
        public void RegisterModule(IDependencyInjectionContainer container)
        {
            container.AddSingleton<SpriteSheetLoader, SpriteSheetLoader>();
            
        }
    }
}
